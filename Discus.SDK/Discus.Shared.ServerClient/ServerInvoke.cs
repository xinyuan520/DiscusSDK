using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nacos.V2;
using Nacos.V2.DependencyInjection;
using Nacos.V2.Naming;
using Polly.Retry;
using Polly;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Nacos.V2.Naming.Dtos;

namespace Discus.Shared.ServerClient
{
    public class ServerInvoke
    {
        private readonly Dictionary<Type, string> _pathMapping = new();

        internal void RegisterClientPath(Type clientType, string path)
        {
            _pathMapping[clientType] = path;
        }

        private string GetCallerPath()
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                var frame = stackTrace.GetFrame(i);
                var method = frame?.GetMethod();
                var declaringType = method?.DeclaringType;

                if (declaringType != null && _pathMapping.ContainsKey(declaringType))
                {
                    return _pathMapping[declaringType];
                }
            }
            throw new InvalidOperationException("Cannot determine caller path. Make sure the call is made from a registered client.");
        }

        private static IHttpClientFactory _httpClientFactory;

        // 添加单例实例
        private static ServerInvoke _instance;
        private static readonly object _lock = new object();

        // 添加单例获取方法
        public static ServerInvoke Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new ServerInvoke();
                    }
                }
                return _instance;
            }
        }

        public static IHttpClientFactory GetHttpClientFactory()
        {
            _httpClientFactory = HttpClientPool.Instance;
            return _httpClientFactory;
        }
        public static INacosNamingService GetNacosNamingService()
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            var optionAccs = Options.Create(new NacosSdkOptions()
            {
                ServerAddresses = new List<string> { "http://192.168.32.129:8848" },
                Namespace = "",
                ListenInterval = 1000,
                UserName="nacos",
                Password="123456"
            });
            var nacosNamingService = new NacosNamingService(loggerFactory, optionAccs, _httpClientFactory);
            return nacosNamingService;
        }

        public static async Task<T> DoGetAsync<T>(string url, Dictionary<string, string> headers = null) => await Instance.InternalDoGetAsync<T>(url, headers);

        public static async Task<T> DoPostAsync<T>(string url, object data, Dictionary<string, string> headers = null) => await Instance.InternalDoPostAsync<T>(url, data, headers);

        private async Task<T> InternalDoGetAsync<T>(string url, Dictionary<string, string> headers = null)
        {
            var path = Instance.GetCallerPath();
            var client = new BasicServerClient(path, "discus-user-webapi", "DEFAULT_GROUP");
            return await client.DoGetAsync<T>(url, headers);
        }
        private async Task<T> InternalDoPostAsync<T>(string url, object data, Dictionary<string, string> headers = null)
        {
            var path = Instance.GetCallerPath();
            var client = new BasicServerClient(path, "discus-user-webapi", "DEFAULT_GROUP");
            return await client.DoPostAsync<T>(url, data, headers);
        }
    }
}
