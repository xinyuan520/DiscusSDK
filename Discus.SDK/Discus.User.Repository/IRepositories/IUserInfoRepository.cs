using Discus.SDK.Repository.SqlSugar.Repository;
using Discus.SDK.Tools.PageResult;
using Discus.User.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discus.User.Repository.IRepositories
{
    public interface IUserInfoRepository : IBaseRepository<UserInfo>
    {
        Task<PageModel<UserInfo>> GetUserPageList(string userName, int pageIndex, int pageSize);
    }
}
