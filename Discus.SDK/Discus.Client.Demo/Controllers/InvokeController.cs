using Discus.SDK.Tools.HttpResult;
using Discus.User.Application.Contracts.Dtos;
using Discus.User.ServerClient;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Net.Http;
using System.Text.Json;

namespace Discus.Client.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvokeController : ControllerBase
    {
        private readonly string baseurl = "http://192.168.32.1:6007";

        [HttpGet("get")]
        public async Task<ApiResult> Get()
        {
            var jwtApi = RestService.For<JwtServerClient>(baseurl);
            var result = await jwtApi.Login(new LoginRequestDto()
            {
                Account = "admin",
                Password = "E10ADC3949BA59ABBE56E057F20F883E",
            });
            return result;
        }

        [HttpGet("GetNacosInfo")]
        public async Task<string> GetNacosInfo()
        {
            var nacosApi = RestService.For<NacosServerClient>(baseurl);
            var result = await nacosApi.Get();
            return result;
        }

        [HttpGet("getbyid")]
        public async Task<UserInfoDto> GetById()
        {
            long id = 1;
            var jwtApi = RestService.For<JwtServerClient>(baseurl);
            var result = await jwtApi.Login(new LoginRequestDto()
            {
                Account = "admin",
                Password = "E10ADC3949BA59ABBE56E057F20F883E",
            });

            var jwtApiResult = JsonSerializer.Deserialize<UserTokenInfoResultDto>(result.Data.ToString());
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseurl)
            };
            var userInfoApi = RestService.For<UserInfoServerClient>(httpClient);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwtApiResult.Token}");
            var userInfoDto = await userInfoApi.GetById(1);
            return userInfoDto;
        }

        public class UserTokenInfoResultDto
        {
            public string Token { get; set; }

            public string Expire { get; set; }
        }
    }
}
