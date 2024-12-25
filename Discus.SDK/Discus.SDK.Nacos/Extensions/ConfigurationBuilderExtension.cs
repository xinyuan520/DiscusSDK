using Discus.SDK.Core.Configuration;
using Discus.SDK.Nacos.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nacos.V2;
using Nacos.V2.Config;
using Nacos.V2.Config.FilterImpl;
using Nacos.V2.Config.Impl;
using Nacos.V2.Naming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Discus.SDK.Nacos.Extensions
{
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder AddNacosConfiguration(this IConfigurationBuilder configurationBuilder, NacosConfig config, bool reloadOnChanges = false)
        {
            // 创建日志工厂
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

            // 配置 NacosSdkOptions
            var nacosOptions = Options.Create(new NacosSdkOptions
            {
                ServerAddresses = config.ServerAddresses,
                UserName = config.UserName,
                Password = config.Password,
                ConfigFilterAssemblies = new List<string>() { config.ServiceName },
                ConfigUseRpc = false
        });

            // 创建 NacosConfigService 实例
            var configService = new NacosConfigService(loggerFactory, nacosOptions);
            foreach (var keyPath in config.NacosKeyPath)
            {
                configurationBuilder.Add(new BasicNacosSource(configService, keyPath, reloadOnChanges));
            }

            return configurationBuilder;
        }
    }
}
