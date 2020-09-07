

using System.ComponentModel.DataAnnotations;
using Tameenk.AutoLeasing.Resources.Messages;

namespace Tameenk.AutoLeasing.Identity.Domain
{
    public class OneTimePasswordModel : ModelBase
    {

        [Regex(RegexTypes.Email, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "Checkout_error_email")]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "EmailRequired")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordRequired")]
        public string Password { get; set; }
    }
}