

using System.Collections.Generic;
using Tameenk.AutoLeasing.Identity.Domain;

namespace Tameenk.AutoLeasing.Identity
{
    public interface IAdminService
    {
        ApplicationUser GetUserById(string id);
        List<ApplicationUser> GetAllUsers();
        ApplicationUser GetHeavyUserById(string id);
        List<ApplicationRole> GetUserRoles(string userId);
        List<ApplicationRole> GetAllUserRoles();
        int ChangeUserStatus(string id, bool status);
        ApplicationUser GetUserByEmail(OneTimePasswordModel oneTimePasswordModel);
    }
}
