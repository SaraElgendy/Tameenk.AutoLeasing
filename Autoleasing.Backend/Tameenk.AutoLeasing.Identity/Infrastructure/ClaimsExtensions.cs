using System.Linq;
using System.Security.Claims;

namespace Medical.Services.Admin.Identity
{
    public static class ClaimsExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "UserId")?.Value;
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "UserName").Value;
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "Email").Value;
        }
       
    }
}
