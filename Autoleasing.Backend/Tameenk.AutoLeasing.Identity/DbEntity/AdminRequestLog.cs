using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tameenk.AutoLeasing.Identity
{
    public class AdminRequestLog
    {
        public int ID { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public string UserID { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        [StringLength(255)]
        public string PageURL { get; set; }

        [StringLength(255)]
        public string PageName { get; set; }

        public int? ErrorCode { get; set; }

        public string ErrorDescription { get; set; }

        [StringLength(50)]
        public string ServerIP { get; set; }

        [StringLength(50)]
        public string UserIP { get; set; }

        [StringLength(255)]
        public string UserAgent { get; set; }

        public double? ServiceResponseTimeInSeconds { get; set; }

        [StringLength(255)]
        public string MethodName { get; set; }

        [StringLength(255)]
        public string ApiURL { get; set; }
    }
}
