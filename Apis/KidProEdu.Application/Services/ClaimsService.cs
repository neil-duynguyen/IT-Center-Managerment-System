using KidProEdu.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace KidProEdu.Application.Services
{
    public class ClaimsService : IClaimsService
    {
         public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            // Check if HttpContext is not null before accessing its properties
            if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.User != null)
            {
                // todo implementation to get the current userId
                var userIdClaim = httpContextAccessor.HttpContext.User.FindFirst("userId");

                // Check if the userIdClaim is not null before accessing its Value property
                var Id = userIdClaim?.Value;
                GetCurrentUserId = string.IsNullOrEmpty(Id) ? Guid.Empty : Guid.Parse(Id);
            }
            else
            {
                GetCurrentUserId = Guid.Empty;
            }
        }

        public Guid GetCurrentUserId { get; }
    }
}
