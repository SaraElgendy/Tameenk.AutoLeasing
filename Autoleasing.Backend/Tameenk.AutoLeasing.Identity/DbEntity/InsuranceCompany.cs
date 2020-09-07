using System;
using System.Collections.Generic;
using System.Text;

namespace Tameenk.AutoLeasing.Identity
{
    public class InsuranceCompany
    {
        public int InsuranceCompanyID { get; set; }

        public string PolicyFailureRecipient { get; set; }
        public string NameAR { get; set; }

        public string NameEN { get; set; }

        public string DescAR { get; set; }

        public string DescEN { get; set; }
        public string NamespaceTypeName { get; set; }
        public string ClassTypeName { get; set; }
        public string ReportTemplateName { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public Guid? ModifiedBy { get; set; }

        public int? AddressId { get; set; }

        public int? ContactId { get; set; }
        public bool IsActive { get; set; }
        public bool? IsUseNumberOfAccident { get; set; }
        public string NajmNcdFreeYearsToUseNumberOfAccident { get; set; }//0,11,13
        public string Key { get; set; }
        public bool? AllowAnonymousRequest { get; set; }
        public bool? ShowQuotationToUser { get; set; }
        public bool? HasDiscount { get; set; }
        public string DiscountText { get; set; }
        public string VAT { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public bool IsActiveTPL { get; set; }
        public bool IsActiveComprehensive { get; set; }

        public bool IsAddressValidationEnabled { get; set; }
        public bool? UsePhoneCamera { get; set; }

        public int Order { get; set; }
        public string TermsAndConditionsFilePath { get; set; }
        public ICollection<AspNetUserCompany> UserCompanies { get; set; }



    }
}
