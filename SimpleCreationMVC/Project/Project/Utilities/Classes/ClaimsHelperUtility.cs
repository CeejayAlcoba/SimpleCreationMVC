
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Utilities.Interfaces;

namespace Utilities.Classes
{
    public class ClaimsHelperUtility : IClaimsHelperUtility
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsHelperUtility(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? GetUserId()
        {
            string? strUserId = _httpContextAccessor?.HttpContext?.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(strUserId)) return null;
            return int.Parse(strUserId);
        }
    }
}
