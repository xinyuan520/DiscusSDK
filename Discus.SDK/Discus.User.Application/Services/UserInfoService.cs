using Discus.SDK.IdGenerater.IdGeneraterFactory;
using Discus.SDK.Tools.PageResult;
using Discus.User.Application.Contracts.Dtos;
using Discus.User.Application.Contracts.QueryParam;
using Discus.User.Repository.Entities;
using Discus.User.Repository.IRepositories;

namespace Discus.User.Application.Services
{
    public class UserInfoService : BasicService, IUserInfoService 
    {
        private readonly IMapper _mapper;
        //private readonly IBaseRepository<UserInfo> _userinfoRepository;

        private readonly IUserInfoRepository _userInfoRepository;
        public UserInfoService(IMapper mapper, IUserInfoRepository userInfoRepository)
        {
            _mapper = mapper;
            _userInfoRepository = userInfoRepository;
        }

        public async Task<ApiResult> Create(UserInfoRequestDto request)
        {
            var userinfo = _mapper.Map<UserInfo>(request);
            userinfo.Id = IdGenerater.GetNextId();
            int count = await _userInfoRepository.AddAsync(userinfo);
            return count > 0 ? ApiResult.IsSuccess("新增成功！") : ApiResult.IsFailed("新增失败！");
        }

        public async Task<ApiResult> Update(UserInfoRequestDto request)
        {
            var userInfo = _mapper.Map<UserInfo>(request);
            int count = await _userInfoRepository.UpdateIgnoreAsync(userInfo, UpdatingProps<UserInfo>(u => u.Password));
            return count > 0 ? ApiResult.IsSuccess("更新成功！") : ApiResult.IsFailed("更新失败！");
        }

        public async Task<ApiResult> Delete(long id)
        {
            var userInfo = await _userInfoRepository.GetByIdAsync(id);
            if (userInfo == null)
                return ApiResult.IsFailed("无效用户信息，删除失败！");
            userInfo.IsDeleted = true;
            int count = await _userInfoRepository.UpdateContainAsync(userInfo, UpdatingProps<UserInfo>(u => u.IsDeleted));
            return count > 0 ? ApiResult.IsSuccess("删除成功！") : ApiResult.IsFailed("删除失败！");
        }

        public async Task<UserInfoDto> GetById(long Id)
        {
            var userInfo = await _userInfoRepository.GetByIdAsync(Id);
            return _mapper.Map<UserInfoDto>(userInfo);
        }
        public async Task<List<UserInfoDto>> GetAll()
        {
            var userInfoList = await _userInfoRepository.GetAllAsync();
            return _mapper.Map<List<UserInfoDto>>(userInfoList);
        }
        public async Task<PageModel<UserInfoDto>> GetUserPageList(UserPageParam param)
        {
            
            var pageList = await _userInfoRepository.GetUserPageList(param.UserName,param.PageIndex,param.PageSize);
            var userInfoDtos = _mapper.Map<List<UserInfoDto>>(pageList.Data);
            return new PageModel<UserInfoDto>(userInfoDtos, pageList.Count);
        }
    }
}
