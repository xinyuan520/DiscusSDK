using Discus.SDK.Core.Configuration;
using Discus.SDK.Core.Consts;
using Discus.SDK.Tools.HttpResult;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nacos.V2;
using Nacos.V2.Naming;
using Polly;
using Polly.Retry;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Discus.Shared.ServerClient
{
    public class BasicServerClient
    {

        //private readonly ILogger<BasicServerClient> _logger;


        private static INacosNamingService _nacosNamingService;
        private static IHttpClientFactory _httpClientFactory;
        private readonly string _serviceName;
        private readonly string _groupName;
        private const string CLIENT_NAME = "BasicServerClient";
        private readonly string _path;

        private AsyncRetryPolicy<HttpResponseMessage> _retryPolicy => Policy
       .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
       .Or<HttpRequestException>()
       .WaitAndRetryAsync(
           retryCount: 3,
           sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
           onRetry: (result, timeSpan, retryCount, context) =>
           {
               Console.WriteLine($"重试访问地址：{result.Result?.RequestMessage?.RequestUri}");
               Console.WriteLine($"重试次数：{retryCount}，失败原因: {result.Exception?.Message ?? result.Result.StatusCode.ToString()}");
               //_logger.LogInformation($"重试次数：{retryCount}，失败原因: {result.Exception?.Message ?? result.Result.StatusCode.ToString()}");
           });

        public BasicServerClient()
        {
        }

        public BasicServerClient(string path, string serviceName, string groupName)
        {
            _path = path;
            _serviceName = serviceName;
            _groupName = groupName;
            _httpClientFactory = ServerInvoke.GetHttpClientFactory();
            _nacosNamingService = ServerInvoke.GetNacosNamingService();
            ServerInvoke.Instance.RegisterClientPath(this.GetType(), path);
        }

        private async Task<string> GetFullUrlAsync(string url)
        {
            if (_nacosNamingService == null)
            {
                _nacosNamingService = ServerInvoke.GetNacosNamingService();
            }
            var instance = await _nacosNamingService.SelectOneHealthyInstance(_serviceName, _groupName);
            if (instance == null)
            {
                throw new Exception($"服务名称无效: {_serviceName}");
            }
            var baseUrl = $"http://{instance.Ip}:{instance.Port}";
            return $"{baseUrl.TrimEnd('/')}/{_path.TrimStart('/')}/{url.TrimStart('/')}";
        }

        public async Task<T> DoGetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            var fullUrl = await GetFullUrlAsync(url);
            if (_httpClientFactory == null)
            {
                _httpClientFactory = ServerInvoke.GetHttpClientFactory();
            }
            using var client = _httpClientFactory.CreateClient(CLIENT_NAME + DateTime.Now.ToString());
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            var response = await _retryPolicy.ExecuteAsync(() => client.GetAsync(fullUrl));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content);
        }

        public async Task<T> DoPostAsync<T>(string url, object data, Dictionary<string, string> headers = null)
        {
            var fullUrl = await GetFullUrlAsync(url);

            using var client = _httpClientFactory.CreateClient(CLIENT_NAME + DateTime.Now.ToString());
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _retryPolicy.ExecuteAsync(() => client.PostAsync(fullUrl, content));
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseContent);
        }
    }
}
