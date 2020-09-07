
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Tameenk.AutoLeasing.Identity
{
    public static class ClaimsExtensions
    {
        public static ApplicationUser GetUser(this ClaimsPrincipal principal)
        {
            return JsonConvert.DeserializeObject<ApplicationUser>(principal?.Claims?.FirstOrDefault(x => x.Type == "User").Value);
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "UserId")?.Value;
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "UserName")?.Value;
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "Email")?.Value;
        }
        public static string GetSponserId(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(x => x.Type == "SponserId").Value;
        }
        public static bool GetIsTestingUser(this ClaimsPrincipal principal)
        {
            var val = principal?.Claims?.FirstOrDefault(x => x.Type == "IsTestingUser").Value;
            bool.TryParse(val, out bool result);
            return result;
        }
        public static bool GetUserInRole(this ClaimsPrincipal principal, string role)
        {
            var val = principal?.Claims?.FirstOrDefault(x => x.Type == "Roles").Value;
            List<string> roleList = JsonConvert.DeserializeObject<List<string>>(val);
            return roleList.Contains(role);
        }


    }
}
