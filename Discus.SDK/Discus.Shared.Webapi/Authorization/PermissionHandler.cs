using Microsoft.AspNetCore.Http;

namespace Discus.Shared.WebApi.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 检查 Claims
            if (context.User.Identity.IsAuthenticated && context.Resource is HttpContext httpContext)
            {
                var authHeader = httpContext.Request.Headers["Authorization"].ToString();
                if (authHeader != null)
                { 
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
