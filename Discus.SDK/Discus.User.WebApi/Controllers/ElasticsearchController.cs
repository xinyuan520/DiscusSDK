using Discus.SDK.Tools.HttpResult;
using Discus.User.Application.Contracts.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discus.User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticsearchController : ControllerBase
    {
        private readonly IElasticsearchService _elasticsearchService;
        public ElasticsearchController(IElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ApiResult> Create()
        {
            return await _elasticsearchService.Create();
        }

        [HttpPost("Update")]
        public async Task<ApiResult> Update()
        {
            return await _elasticsearchService.Update();
        }

        /// <summary>
        /// 根据Id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<UserInfoDto> GetById(int id)
        {
            return await _elasticsearchService.GetById(id);
        }
    }
}
