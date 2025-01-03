﻿using Discus.SDK.Tools.HttpResult;
using Discus.Shared.Webapi.Controller;
using Discus.Shared.WebApi.Authorization;
using Discus.User.Application.Contracts.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Discus.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : BasicController
    {
        private readonly IJwtService _jwtService;

        public JwtController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ApiResult> Login(LoginRequestDto request)
        {
            return await _jwtService.Login(request);
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetById"), CustomerAuthorize("GetById")]
        public async Task<UserInfoDto> GetById()
        {
            AutoInfoModel autoInfoModel = this.GetAutoInfoModel();
            return await _jwtService.GetById(autoInfoModel.Id);
        }
    }
}
