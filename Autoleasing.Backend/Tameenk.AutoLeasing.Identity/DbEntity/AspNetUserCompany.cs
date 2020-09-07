using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tameenk.AutoLeasing.Identity
{
    public class AspNetUserCompany
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
       
        public virtual ApplicationUser User { get; set; }

            [ForeignKey("Company")]
        public int CompanyId { get; set; }
    
        public virtual InsuranceCompany Company { get; set; }
    }
}
