using Discus.Shared.ServerClient;
using Elastic.Clients.Elasticsearch;
using System.Text.Json.Nodes;
namespace Discus.User.ServerClient
{
    public class NacosServerClient : BasicServerClient
    {
        private const string _path = "/api/nacos";
        private const string SERVICE_NAME = "discus-user-webapi";
        private const string GROUP_NAME = "DEFAULT_GROUP";

        public NacosServerClient() : base(_path,SERVICE_NAME, GROUP_NAME)
        {
        }

        public async Task<JsonObject> GetServerAdressByNacos()
        {
            var result = await ServerInvoke.DoGetAsync<JsonObject>("/Get");
            return result;
        }
    }
}
