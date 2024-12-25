using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.SDK.Core.Configuration
{
    public class NacosConfig
    {
        public string EndPoint { get; set; } = string.Empty;
        public List<string> ServerAddresses { get; set; } = new List<string>();

        public int DefaultTimeOut { get; set; }

        public string Namespace { get; set; } = string.Empty;

        public int ListenInterval { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public string GroupName { get; set; } = string.Empty;

        public string ClusterName { get; set; } = string.Empty;

        public string Ip { get; set; } = string.Empty;

        public string PreferredNetworks { get; set; } = string.Empty;

        public int Port { get; set; }

        public int Weight { get; set; }

        public bool RegisterEnabled { get; set; }

        public bool InstanceEnabled { get; set; }

        public bool Ephemeral { get; set; }

        public bool Secure { get; set; }

        public string AccessKey { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool ConfigUseRpc { get; set; }

        public bool NamingUseRpc { get; set; }

        public string NamingLoadCacheAtStart { get; set; } = string.Empty;

        public string LBStrategy { get; set; } = string.Empty;

        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        public List<NacosKeyPath> NacosKeyPath { get; set; }
    }

    public class NacosKeyPath()
    {
        public string DataId { get; set; } = string.Empty;

        public string Group { get; set; } = string.Empty;

        public long TimeoutMs { get; set; }
    }
}
