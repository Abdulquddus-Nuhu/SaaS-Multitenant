using Infrastructure.Data;

namespace API.MultitenantSetup
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TenantIdentifier tenantIdentifier)
        {
            // Assuming the tenant ID is stored as a claim named "tenant_id"
            tenantIdentifier.CurrentTenantId = context.User.FindFirst("datakey")?.Value;
            //tenantIdentifier.CurrentTenantId = context.User.FindFirst("tenant_id")?.Value;

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

}
