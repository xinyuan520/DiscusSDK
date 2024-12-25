using Discus.SDK.Repository.SqlSugar.Repository;
using Discus.User.Application.Contracts.Dtos;
using Discus.User.Repository.Entities;
using Elastic.Clients.Elasticsearch;
using MySqlX.XDevAPI;

namespace Discus.User.Application.Services
{
    public class ElasticsearchService : BasicService, IElasticsearchService
    {
        private static readonly string index = "my_index6";

        private readonly IBaseRepository<UserInfo> _userinfoRepository;

        private readonly IMapper _mapper;

        private readonly ElasticsearchClient _client;
        public ElasticsearchService(IBaseRepository<UserInfo> userinfoRepository, IMapper mapper, ElasticsearchClient client)
        {
            _client = client;
            _mapper = mapper;
            _userinfoRepository = userinfoRepository;
        }

        public async Task<ApiResult> Create()
        {
            var userInfo = await _userinfoRepository.GetByIdAsync(1);
            var response = await _client.IndexAsync(userInfo, idx => idx.Index(index));
            if (response.IsValidResponse)
            {
                return ApiResult.IsSuccess("操作成功！", ApiResultCode.Succeed, response);
            }
            return ApiResult.IsFailed("操作失败！");
        }

        public async Task<ApiResult> Update()
        {
            var userInfo = await _userinfoRepository.GetByIdAsync(1);
            userInfo.EditByTime = DateTime.Now;
            //还需要修改
            var response = await _client.IndexAsync(userInfo, idx => idx.Index(index));
            if (response.IsValidResponse)
            {
                return ApiResult.IsSuccess("操作成功！", ApiResultCode.Succeed, response);
            }
            return ApiResult.IsFailed("操作失败！");
        }


        public async Task<UserInfoDto> GetById(int id)
        {
            var response = await _client.GetAsync<UserInfo>(id, g => g.Index(index));
            if (response.IsValidResponse)
            {
                return _mapper.Map<UserInfoDto>(response.Source);
            }
            return new UserInfoDto();
        }
    }
}
