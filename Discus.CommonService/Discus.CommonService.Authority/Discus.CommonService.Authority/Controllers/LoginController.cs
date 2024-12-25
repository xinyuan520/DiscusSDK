using Discus.CommonService.Authority.Application.Contracts.Dtos;
using Discus.SDK.Tools.HttpResult;

namespace Discus.CommonService.Authority.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ApiResult> Login(LoginRequestDto request)
        {
            return await _loginService.Login(request);
        }

        /// <summary>
        /// 手机号登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("LoginPhone")]
        public async Task<ApiResult> LoginPhone(LoginPhoneRequestDto request)
        { 
            return await _loginService.LoginPhone(request);

        }

    }
}
