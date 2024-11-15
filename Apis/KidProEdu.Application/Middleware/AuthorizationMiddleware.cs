using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationMiddleware> _logger;

        public AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>() != null;

            if (allowAnonymous)
            {
                // Nếu có [AllowAnonymous], bỏ qua middleware này và tiếp tục
                await _next(context);
                return;
            }

            // Kiểm tra quyền truy cập (Role-based authorization)
            if (!await HasPermission(context))
            {
                _logger.LogWarning("User do not have access to this API.");
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("You do not have access to this resource.");
                return;
            }

            // Nếu tất cả đều hợp lệ, tiếp tục yêu cầu
            await _next(context);
        }
        private async Task<bool> HasPermission(HttpContext context)
        {
            // Lấy thông tin role từ claims trong token
            var roles = context.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var requiredRoles = context.GetEndpoint()?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()?.Roles?.Split(',');

            if (requiredRoles != null && requiredRoles.Any())
            {
                //So sánh role torng token với role yêu cầu
                return requiredRoles.Any(role => roles.Contains(role.Trim()));
            }

            // Nếu không có role yêu cầu (trường hợp endpoint không yêu cầu role), cho phép truy cập
            return true;
        }

    }
}
