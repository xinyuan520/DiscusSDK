using Discus.SDK.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Nacos.V2.Config;
using Nacos.V2.Naming.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Discus.SDK.Nacos.Configurations
{
    public class BasicNacosProvider : ConfigurationProvider
    {
        private readonly NacosConfigService _configService;

        private readonly NacosKeyPath _nacosKeyPath;

        private readonly bool _reloadOnChanges;

        private readonly int _waitMillisecond;

        private Task? _pollTask;

        public BasicNacosProvider(NacosConfigService configService, NacosKeyPath nacosKeyPath, bool reloadOnChanges)
        {
            _configService = configService;
            _nacosKeyPath = nacosKeyPath;
            _reloadOnChanges = reloadOnChanges;
            _waitMillisecond = 1000 * 3;
        }

        public override void Load()
        {
            if (_pollTask != null)
            {
                return;
            }
            LoadData(GetData().GetAwaiter().GetResult());
            PollReaload();
        }

        private void LoadData(string result)
        {
            Data = JsonConfigurationFileParser.Parse(result);
        }

        private async Task<string> GetData()
        {
            var res = await _configService.GetConfig(_nacosKeyPath.DataId, _nacosKeyPath.Group, _nacosKeyPath.TimeoutMs);
            if (!string.IsNullOrEmpty(res))
            {
                return res;
            }
            throw new Exception($"加载Nacos配置发生错误！");
        }

        private void PollReaload()
        {
            if (_reloadOnChanges)
            {
                _pollTask = Task.Run(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            string latestConfig = await GetData();
                            if (!string.Equals(latestConfig, Data.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                LoadData(latestConfig);
                                OnReload();
                            }
                            else
                            {
                                await Task.Delay(_waitMillisecond);
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception or handle it as needed
                            Console.WriteLine($"Error while polling for configuration changes: {ex.Message}");
                            await Task.Delay(_waitMillisecond);
                            continue;
                        }
                    }
                });
            }
        }
    }
}
