using Serilog;
using Serilog.Filters;

namespace Discus.Shared.WebApi.Extensions
{
    public static class SerilogBuilderExtension
    {
        public static WebApplicationBuilder UseDefaultSerilog(this WebApplicationBuilder builder, LogConfig logConfig, LogStashConfig logStash)
        {
            //使用日志
            builder.Host.UseSerilog((context, logger) =>
            {
                logger
                 .Enrich.FromLogContext()
                 .ReadFrom.Configuration(context.Configuration)
                 .WriteTo.Console()
                 .WriteTo.MySQL(logConfig.ConnectionString, logConfig.TableName)
                 .WriteTo.LogstashHttp(logStash.LogstashUri);
            });
            return builder;
        }
    }
}
