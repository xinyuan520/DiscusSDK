using Discus.SDK.Core.Configuration;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Discus.SDK.Es.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceEs(this IServiceCollection services, IConfigurationSection elasticSearchSection)
        {
            if (services.HasRegistered(nameof(AddServiceEs)))
                return services;

            var elasticSearchConfig = elasticSearchSection.Get<ElasticSearchConfig>();
            services.AddSingleton<ElasticsearchClient>(sp =>
            {
                var settings = new ElasticsearchClientSettings(new Uri(elasticSearchConfig.Url));
                    //.Authentication(new BasicAuthentication(elasticSearchConfig.UserName, elasticSearchConfig.Password));
                return new ElasticsearchClient(settings);
            });

            return services;
        }
    }
}
