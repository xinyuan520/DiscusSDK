using DotXxlJob.Core;

namespace Discus.DistributeCalculate
{
    public class XxlJobExecutorMiddleware
    {
        private readonly IServiceProvider _provider;
        private readonly RequestDelegate _next;

        private readonly XxlRestfulServiceHandler _rpcService;
        public XxlJobExecutorMiddleware(IServiceProvider provider, RequestDelegate next)
        {
            this._provider = provider;
            this._next = next;
            this._rpcService = _provider.GetRequiredService<XxlRestfulServiceHandler>();
        }

        public async Task Invoke(HttpContext context)
        {
            string contentType = context.Request.ContentType;

            if ("POST".Equals(context.Request.Method, StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(contentType)
                && contentType.ToLower().StartsWith("application/json"))
            {

                await _rpcService.HandlerAsync(context.Request, context.Response);

                return;
            }

            await _next.Invoke(context);
        }
    }
}
