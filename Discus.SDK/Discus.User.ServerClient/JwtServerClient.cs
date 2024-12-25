using Discus.SDK.Tools.HttpResult;
using Discus.Shared.ServerClient;
using Discus.User.Application.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.User.ServerClient
{
    public class JwtServerClient : BasicServerClient
    {
        private const string _path = "/api/jwt";
        private const string SERVICE_NAME = "discus-user-webapi";
        private const string GROUP_NAME = "DEFAULT_GROUP";

        public JwtServerClient() : base(_path, SERVICE_NAME, GROUP_NAME)
        {
        }

        public async Task<ApiResult> GetServerAdressByNacos(LoginRequestDto request)
        {
            var result = await ServerInvoke.DoPostAsync<ApiResult>("/Login", request);
            return result;
        }
    }
}
