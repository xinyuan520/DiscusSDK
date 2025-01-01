using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.User.ServerClient
{
    public interface NacosServerClient
    {
        [Get("/api/nacos/get")]
        Task<string> Get();
    }
}
