using Discus.User.Application.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.User.Application.Contracts.Services
{
    public interface IElasticsearchService : IService
    {
        Task<ApiResult> Create();

        Task<ApiResult> Update();

        Task<UserInfoDto> GetById(int id);
    }
}
