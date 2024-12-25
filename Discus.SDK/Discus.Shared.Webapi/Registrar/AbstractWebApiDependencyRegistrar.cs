using Discus.Shared.WebApi.Authorization;
using Nacos.AspNetCore.V2;

namespace Discus.Shared.Webapi.Registrar
{
    public abstract partial class AbstractWebApiDependencyRegistrar : IDependencyRegistrar
    {
        public string Name => "";
        protected IConfiguration Configuration { get; init; }
        protected IServiceCollection Services { get; init; }
        protected IServiceInfo ServiceInfo { get; init; }
        public AbstractWebApiDependencyRegistrar(IServiceCollection services)
        {
            Services = services;
            Configuration = services.GetConfiguration();
            ServiceInfo = services.GetServiceInfo();
        }

        public abstract void AddService();

        protected virtual void AddWebApiDefault()
        {
            Services.AddHttpContextAccessor().AddMemoryCache().AddControllers().AddJsonOptions(options =>
            {
                // 使用驼峰命名
                options.JsonSerializerOptions.PropertyNamingPolicy = null;// JsonNamingPolicy.CamelCase;
            });
            AddAuthentication(Configuration);
            AddAuthorization();
            Services.AddEndpointsApiExplorer();
            Services.AddAutoMapper(ServiceInfo.GetApplicationAssembly());
            Configure();
            AddCors();
            AddHealthChecks(false, true, true);
            AddSwaggerGen();
            AddMiniProfiler();
            AddApplicationServices();
        }
    }
}
