using Discus.SDK.Tools.HttpResult;
using Discus.User.Application.Contracts.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.User.ServerClient
{
    public interface UserInfoServerClient
    {
        [Get("/api/UserInfo/GetById/{id}")]
        Task<UserInfoDto> GetById(long id);
    }
}
