using Discus.SDK.Tools.HttpResult;
using Discus.User.Application.Contracts.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Discus.User.ServerClient
{
    public interface JwtServerClient
    {
        [Post("/api/jwt/login")]
        Task<ApiResult> Login(LoginRequestDto request);
    }
}
