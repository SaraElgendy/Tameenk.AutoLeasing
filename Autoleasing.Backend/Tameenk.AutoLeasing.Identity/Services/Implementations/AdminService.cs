using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

using System;
using System.Security.Cryptography;
using System.Text;
using Tameenk.AutoLeasing.Identity.Domain;

namespace Tameenk.AutoLeasing.Identity
{
    public class AdminService : IAdminService
    {
        public AdminContext context;
        public AdminService(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdminContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            this.context = new AdminContext(optionsBuilder.Options);
        }
        public ApplicationUser GetUserById(string id)
        {
            var user = context.Users.AsNoTracking().FirstOrDefault(x => x.Id == id);
            return user;
        }
        public ApplicationUser GetHeavyUserById(string userid)
        {
            var user = context.Users
                .FirstOrDefaultAsync(x => x.Id == userid).Result;
            return user;

        }
        public List<ApplicationUser> GetAllUsers()
        {
            var users = context.Users.Include(x => x.UserRoles).ToList();
            return users;
        }
      
       
      

        public List<ApplicationRole> GetUserRoles(string userId)
        {
            return context.UserRoles.Include(x => x.Role)
                   .Where(x => x.UserId == userId && x.IsActive).Select(x => x.Role).ToList();
        }

        public List<ApplicationRole> GetAllUserRoles()
        {
            return context.Roles.Where(x => x.IsActive).ToList();
        }

        public int ChangeUserStatus(string id, bool status)
        {
            var output = 0;
            try
            {
                var page = context.Set<ApplicationUser>().FirstOrDefault(x => x.Id == id.ToString());
                page.IsActive = status;
                context.Set<ApplicationUser>().Update(page);
                context.SaveChanges();
              //  output.ErrorCode = Output<int>.ErrorCodes.Success;
                return output;
            }
            catch (Exception ex)
            {
               // output.ErrorCode = Output<int>.ErrorCodes.ServerException;
                //output.ErrorDescription = ex.Message;
                return output;
            }

        }

        public ApplicationUser GetUserByEmail(OneTimePasswordModel model)
        {
            var user = context.Users.Where(
                 x => x.Email == model.Email &&
                 x.OneTimePassword == Convert.ToBase64String(new SHA1CryptoServiceProvider()
                 .ComputeHash(Encoding.Unicode.GetBytes(model.Password)))).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            user.OneTimePassword = null;
            context.Users.Update(user);
            context.SaveChanges();
            return user;
        }
    }
}
