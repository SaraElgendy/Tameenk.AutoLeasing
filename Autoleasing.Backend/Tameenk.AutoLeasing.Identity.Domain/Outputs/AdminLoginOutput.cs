using System;
using System.Collections.Generic;

namespace Tameenk.AutoLeasing.Identity.Domain
{
    public class AdminLoginOutput
    {
        public string Token { get; set; }
        public int Expires { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string UserId { get; set; }
        public List<LoginRole> Roles { get; set; } 

    }
    public class LoginRole
    {
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string ModuleAr { get; set; }
        public string ModuleEn { get; set; }

        public string Url { get; set; }
        public string Icon { get; set; }
        public int Order { get; set; }
    }
}
