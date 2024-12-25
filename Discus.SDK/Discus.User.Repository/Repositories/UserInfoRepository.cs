using Discus.SDK.Repository.SqlSugar.Repository;
using Discus.SDK.Tools.PageResult;
using Discus.User.Repository.Entities;
using Discus.User.Repository.IRepositories;

namespace Discus.User.Repository.Repositories
{
    public class UserInfoRepository : BaseRepository<UserInfo>, IUserInfoRepository
    {
        private readonly ISqlSugarClient _sugarClient;

        public UserInfoRepository(ISqlSugarClient sugarClient) : base(sugarClient)
        {
            _sugarClient = sugarClient;
        }

        public async Task<PageModel<UserInfo>> GetUserPageList(string userName, int pageIndex, int pageSize)
        {
            var query = _sugarClient.Queryable<UserInfo>();
            RefAsync<int> totalCount = 0;

            var total = await query.CountAsync();
            var items = await query.OrderBy(u => u.Id)
                                   .Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            var pageList = await _sugarClient.Queryable<UserInfo>()
                 .Where(x => x.IsDeleted == false)
                 .WhereIF(!string.IsNullOrWhiteSpace(userName), urv => urv.UserName.Contains(userName))
                 .OrderByDescending(x => x.EditByTime)
                 .ToPageListAsync(pageIndex, pageSize, totalCount);
            return new PageModel<UserInfo>(pageList, totalCount);
        }
    }
}
