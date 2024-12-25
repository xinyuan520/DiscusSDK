using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Discus.Shared.ServerClient
{
    public class HttpClientPool : IHttpClientFactory, IDisposable
    {

        private readonly ConcurrentDictionary<string, Lazy<HttpClient>> _httpClients;
        private readonly SocketsHttpHandler _handler;
        private static readonly object _lock = new object();
        private static HttpClientPool? _instance;
        private bool _disposed;

        public static HttpClientPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new HttpClientPool();
                    }
                }
                return _instance;
            }
        }

        private HttpClientPool()
        {
            _handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 100,
                EnableMultipleHttp2Connections = true,
                KeepAlivePingPolicy = HttpKeepAlivePingPolicy.WithActiveRequests,
                KeepAlivePingDelay = TimeSpan.FromSeconds(30),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(5),
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _httpClients = new ConcurrentDictionary<string, Lazy<HttpClient>>();
        }


        public HttpClient CreateClient(string name)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(HttpClientPool));
            }

            return _httpClients.GetOrAdd(name, clientName => new Lazy<HttpClient>(() =>
            {
                var client = new HttpClient(_handler, disposeHandler: false)
                {
                    Timeout = TimeSpan.FromSeconds(30)
                };

                client.DefaultRequestHeaders.Add("User-Agent", "Discus-Client/1.0");
                return client;
            })).Value;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            foreach (var client in _httpClients.Values)
            {
                if (client.IsValueCreated)
                {
                    client.Value.Dispose();
                }
            }

            _httpClients.Clear();
            _handler.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
