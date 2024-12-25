using AutoMapper;
using Discus.CommonService.Authority.Application.Contracts.Dtos;
using Discus.CommonService.Authority.Repository.Entities;
using Discus.SDK.Core.Configuration;
using Discus.SDK.Repository.SqlSugar.Repository;
using Discus.SDK.Tools.HttpResult;
using Discus.SDK.Tools.HttpResult.Enums;
using Discus.Shared.WebApi.Authorization.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Discus.CommonService.Authority.Application.Services
{
    public class LoginService : BasicService, ILoginService
    {

        private readonly IBaseRepository<UserInfo> _userinfoRepository;
        private readonly IOptions<JWTConfig> _jwtOptions;
        private readonly ILogger<LoginService> _logger;

        public LoginService(IBaseRepository<UserInfo> userinfoRepository, IOptions<JWTConfig> jwtOptions, ILogger<LoginService> logger)
        {
            _userinfoRepository = userinfoRepository;
            _logger = logger;
            _jwtOptions = jwtOptions;
        }


        public async Task<ApiResult> Login(LoginRequestDto request)
        {
            var userInfo = await _userinfoRepository.FirstOrDefaultAsync(x => x.UserName == request.Account && x.Password == request.Password);
            if (userInfo != null)
            {
                var accessToken = JwtTokenHelper.CreateAccessToken(_jwtOptions.Value, userInfo.Id.ToString(), userInfo.UserName);
                var tokenInfo = new UserTokenInfoDto(accessToken.Token, accessToken.Expire);
                return ApiResult.IsSuccess("登录成功！", ApiResultCode.Succeed, tokenInfo);
            }
            return ApiResult.IsFailed("用户名或密码错误！");
        }

        public async Task<ApiResult> LoginPhone(LoginPhoneRequestDto request)
        { 
                return ApiResult.IsSuccess("登录成功！", ApiResultCode.Succeed, null);
        }
    }
}
