using Discus.CommonService.Authority.Application.Contracts.Dtos;
using Discus.SDK.Tools.HttpResult;

namespace Discus.CommonService.Authority.Application.Contracts.Services
{
    public interface ILoginService : IService
    {
        Task<ApiResult> Login(LoginRequestDto request);

        Task<ApiResult> LoginPhone(LoginPhoneRequestDto request);
    }
}
