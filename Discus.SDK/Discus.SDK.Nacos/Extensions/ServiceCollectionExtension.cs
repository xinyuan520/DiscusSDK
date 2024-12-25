using Discus.SDK.Core.Configuration;
using Discus.SDK.Core.Consts;
using Discus.SDK.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nacos.AspNetCore.V2;
using Nacos.V2.DependencyInjection;

namespace Discus.SDK.Nacos.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceNacos(this IServiceCollection services, IConfigurationSection nacosSection, IConfiguration Configuration)
        {
            if (services.HasRegistered(nameof(AddServiceNacos)))
                return services;

            //注册nacos
            services.AddNacosAspNet(Configuration);

            //用于管理Nacos的配置的信息
            var nacosConfig = nacosSection.Get<NacosConfig>();
            return services.AddNacosV2Config(x =>
            {
                x.ServerAddresses = nacosConfig.ServerAddresses;
                x.Namespace = "";//这里的Namespace对应nacos服务的TenantId
                x.UserName = nacosConfig.UserName;
                x.Password = nacosConfig.Password;
                x.ConfigFilterAssemblies = new List<string>() { nacosConfig.ServiceName };
                x.ConfigUseRpc = false;
            });
        }
    }
}
