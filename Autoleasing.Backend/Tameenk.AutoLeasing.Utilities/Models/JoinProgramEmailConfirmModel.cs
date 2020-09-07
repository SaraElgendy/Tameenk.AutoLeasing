using System;

namespace Tameenk.AutoLeasing.Utilities
{
    public class JoinProgramEmailConfirmModel
    {
        public string UserId { get; set; }

        public string UserEmail { get; set; }

        public DateTime JoinRequestedDate { get; set; }

        public int PromotionProgramId { get; set; }
        public string ConfirmationUrl { get; set;  }
        public string EmailHeader { get; set; }
        public string CompanyName { get; set; }
        public string ImagesUrl { set; get; }

    }
}

