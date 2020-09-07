using System;

namespace Tameenk.AutoLeasing.Utilities
{
    public class PolicyPdfModel
    {
        public string Header { get; set; }
        public string ImagesUrl { set; get; }
        public string CompanyName { set; get; }        
       

        //Policy Data
        public string PolicyNo { get; set; }
        public DateTime? PolicyIssuanceDate { get; set; }
        public DateTime? PolicyEffectiveDate { get; set; }
        public DateTime? PolicyExpiryDate { get; set; }
        public string Filename { get; set; }
        public string FilePath { get; set; }
    }
}
