using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Tameenk.AutoLeasing.Identity
{

    public class ApplicationUser : IdentityUser
    {       
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public string OneTimePassword { get; set; }
        public DateTime? OneTimePasswordExpirationDate { get; set; }
        public bool PasswordConfirmed { get; set; }
        public ICollection<AspNetUserCompany> UserCompanies { get; set; }

    }
    public class ApplicationRole : IdentityRole
    {
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public string ModuleAr { get; set; }
        public string ModuleEn { get; set; }
        public int Order { get; set; }
        public string RelativeUrl { get; set; }
        public string Icon { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
        public bool IsActive { get; set; } = true;
    }


}
