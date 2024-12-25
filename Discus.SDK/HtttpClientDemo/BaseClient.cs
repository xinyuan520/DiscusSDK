using Discus.User.ServerClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtttpClientDemo
{
    public class BaseClient
    {
        public static NacosServerClient NacosApiServerClient { get; } = new NacosServerClient();
        public static JwtServerClient JwtServerClient { get; } = new JwtServerClient();
        
        //public static NacosServerClient NacosApiServerClient => new NacosServerClient();
    }
}
