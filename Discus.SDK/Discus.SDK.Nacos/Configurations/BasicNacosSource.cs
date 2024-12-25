using Discus.SDK.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Nacos.V2.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.SDK.Nacos.Configurations
{
    public class BasicNacosSource : IConfigurationSource
    {
        private readonly NacosConfigService _configService;

        private readonly NacosKeyPath _nacosKeyPath;

        private readonly bool _reloadOnChanges;

        public BasicNacosSource(NacosConfigService configService, NacosKeyPath nacosKeyPath, bool reloadOnChanges)
        {
            _configService = configService;
            _nacosKeyPath = nacosKeyPath;
            _reloadOnChanges = reloadOnChanges;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new BasicNacosProvider(_configService, _nacosKeyPath, _reloadOnChanges);
        }
    }
}
