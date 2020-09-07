using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Tameenk.AutoLeasing.Identity
{
    public static class InsuranceCompanyService
    {

        public static bool CheckCompanyExist(int companyId)
        {
            var Context = new AdminContext();
            return Context.InsuranceCompanies.Any(x => x.InsuranceCompanyID == companyId);

        }
    }
}
