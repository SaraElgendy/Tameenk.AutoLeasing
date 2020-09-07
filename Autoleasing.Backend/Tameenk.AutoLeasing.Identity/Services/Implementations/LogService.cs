
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Tameenk.AutoLeasing.Identity
{
    public static class LogService
    {
      
        public static void AddAdminRequestLogs(AdminRequestLog log)
        {
            var Context = new AdminContext();
            Context.AdminRequestLog.Add(log);
            Context.SaveChanges();
        }
        
    }

}
