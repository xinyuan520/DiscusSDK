using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.SDK.Redis.Configurations
{
    public class RedisConfiguration
    {
        public string Provider { get; set; } = "CsRedis";
        //public bool EnableBloomFilter { get; set; }
        //public string SerializerName { get; set; }
        public string MasterConnectionString { get; set; }

        public string SlaveConnectionString { get; set; }

        public string[] SlaveConnectionStrings { get; set; }

    }
}
