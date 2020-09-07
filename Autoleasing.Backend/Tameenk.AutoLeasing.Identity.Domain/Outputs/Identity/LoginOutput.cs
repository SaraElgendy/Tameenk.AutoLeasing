using System;

namespace Tameenk.AutoLeasing.Identity.Domain
{

    public class LoginOutput
    {
        public string Token { get; set; }
        public int Expires { get; set; }
        public string Email { get; set; }
        public  string TempPassword { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string UserId { get; set; }
        public string MobileNumber { get; set; }
        
    }


}
