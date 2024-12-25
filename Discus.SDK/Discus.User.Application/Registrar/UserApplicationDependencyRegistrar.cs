using Discus.Shared.Application.Registrar;
using Discus.User.Application.MessageHandler;
using Discus.User.Repository.IRepositories;
using Discus.User.Repository.Repositories;
using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Discus.User.Application.Registrar
{
    public sealed class UserApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
    {
        public static readonly string ApplicationFullName = Assembly.GetExecutingAssembly().FullName;
        public override Assembly ApplicationLayerAssembly => Assembly.GetExecutingAssembly();

        public override Assembly ContractsLayerAssembly => typeof(IThirdPartyService).Assembly;

        public UserApplicationDependencyRegistrar(IServiceCollection services) : base(services)
        {

        }

        public override void AddService()
        {
            AddApplicaitonDefault();

            //rabbitmq按需引入
            AddEventBusCap();

            Services.AddScoped<ICapSubscribe, CapSubscribe>();
            Services.AddScoped<IUserInfoRepository, UserInfoRepository>();
        }
    }
}
