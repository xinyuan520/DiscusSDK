using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nacos.V2;
using Nacos.V2.Naming;
using SqlSugar;

namespace Discus.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NacosController : ControllerBase
    {
        private readonly INacosConfigService _nacosConfigService;

        private readonly INacosNamingService _nacosNamingService;

        public NacosController(INacosConfigService nacosConfigService, INacosNamingService nacosNamingService)
        {
            _nacosConfigService = nacosConfigService;
            _nacosNamingService = nacosNamingService;
        }

        [HttpGet("Get")]
        public async Task<string> Get(string dataId = "common.appsettings.json")
        {
            var res = await _nacosConfigService.GetConfig(dataId, "DEFAULT_GROUP", 3000).ConfigureAwait(false);

            return res ?? "empty config";
        }

        [HttpGet("ServerAdress")]
        public async Task<string> GetServerAdressByNacos()
        {
            var serviceName = "discus-user-webapi";
            var groupName = "DEFAULT_GROUP";
            var instances = await _nacosNamingService.GetAllInstances(serviceName, groupName);
           
            if (instances == null || !instances.Any())
            {
                return "No service instances found";
            }
            else 
            {
                var str = string.Join(",", instances.Select(x => $"{x.Ip}:{x.Port}"));
                return str;
            }
        }
    }
}
