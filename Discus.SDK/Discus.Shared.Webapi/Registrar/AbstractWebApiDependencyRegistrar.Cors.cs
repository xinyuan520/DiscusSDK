using Discus.Shared.WebApi.Authorization;

namespace Discus.Shared.Webapi.Registrar
{
    public abstract partial class AbstractWebApiDependencyRegistrar
    {
        /// <summary>
        /// 注册跨域组件
        /// </summary>
        protected virtual void AddCors()
        {
            Services.AddCors(options =>
            {
                options.AddPolicy(AuthorizePolicy.Policy, policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }
    }
}
