using Discus.SDK.Tools.HttpResult;
using Discus.SDK.Tools.PageResult;
using Discus.Shared.WebApi.Authorization;
using Discus.User.Application.Contracts.Dtos;
using Discus.User.Application.Contracts.QueryParam;
using Microsoft.AspNetCore.Authorization;

namespace Discus.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoService _userInfoService;
        private readonly ILogger<UserInfoController> _logger;
        public UserInfoController(IUserInfoService userInfoService, ILogger<UserInfoController> logger)
        {
            _logger = logger;
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("Create"), CustomerAuthorize("")]
        public async Task<ApiResult> Create(UserInfoRequestDto request)
        {
            return await _userInfoService.Create(request);
        }


        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("Update"), CustomerAuthorize("")]
        public async Task<ApiResult> Update(UserInfoRequestDto request)
        {
            return await _userInfoService.Update(request);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{userId}"), CustomerAuthorize("")]
        public async Task<ApiResult> Delete(long userId)
        {
            return await _userInfoService.Delete(userId);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[CustomerAuthorize]
        [HttpGet("GetById/{id}"), CustomerAuthorize("")]
        public async Task<UserInfoDto> GetById(long id)
        {
            return await _userInfoService.GetById(id);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        [HttpGet("GetAll"), CustomerAuthorize("")]
        public async Task<List<UserInfoDto>> GetAll()
        {
            return await _userInfoService.GetAll();
        }

        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("GetUserPageList")]
        [CustomerAuthorize("GetUserPageList")]
        public async Task<PageModel<UserInfoDto>> GetUserPageList(UserPageParam param)
        {
            return await _userInfoService.GetUserPageList(param);
        }
    }
}
