using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationMiddleware> _logger;
    private readonly IConfiguration _configuration;
    private readonly byte[] _secretKeyBytes;

    public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger,
                                    IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _configuration = configuration;
        var secretKey = _configuration["AppSettings:SecretKey"];
        _secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Bỏ qua kiểm tra token nếu request là cho route /Login
        if (context.Request.Path.StartsWithSegments("/api/User/Login", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context); // Tiếp tục mà không kiểm tra token
            return;
        }

        var endpoint = context.GetEndpoint();
        var allowAnonymous = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute>() != null;

        // Bỏ qua kiếm tra token nếu request là AllowAnonymous
        if (allowAnonymous)
        {
            await _next(context);
            return;
        }

        // Kiểm tra yêu cầu xem có Authorization header không và nó có phải là token hợp lệ không
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            try
            {
                var principal = ValidateToken(token);
                if (principal != null)
                {
                    //Nếu token hợp lệ, gắn principal vào context
                    context.User = principal;
                }
                else
                {
                    //Nếu token thì trả về mã lỗi 401
                    _logger.LogWarning("Invalid Token");
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid Token");
                    return;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có
                _logger.LogError(ex, "Error validating token.");
                context.Response.StatusCode = 401;  // Unauthorized
                await context.Response.WriteAsync("An error occurred while validating the token.");
                return;
            }

        }
        else
        {
            // Nếu không có token, trả về lỗi hoặc chuyển hướng đến trang đăng nhập
            _logger.LogWarning("Request does not have a valid JWT token");
            context.Response.StatusCode = 401;  // Unauthorized
            await context.Response.WriteAsync("You need to log in");
            return;
        }

        // Tiếp tục xử lý yêu cầu nếu token hợp lệ
        await _next(context);
    }

    private ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(_secretKeyBytes),  // Dùng key từ AppSettings
                ValidateIssuer = false,  // Không kiểm tra Issuer nếu không cần
                ValidateAudience = false,  // Không kiểm tra Audience nếu không cần
                ValidateLifetime = true,  // Kiểm tra token hết hạn
                ClockSkew = TimeSpan.Zero  // Không có độ trễ trong thời gian hết hạn
            };

            //Giải mã và xác thực token
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

            return principal;
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Token validation failed.");
            return null;
        }
    }
}
