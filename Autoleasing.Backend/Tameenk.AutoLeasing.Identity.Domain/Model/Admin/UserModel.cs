
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tameenk.AutoLeasing.Resources.Messages;

namespace Tameenk.AutoLeasing.Identity.Domain
{
    public class UserModel : ModelBase
    {


        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "EmailRequired")]
        [Regex(RegexTypes.Email, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "EmailFormatError")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "UsernameRequired")]
        public string UserName { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordRequired")]
        public string Userpassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordRequired")]

        public string PhoneNumber { get; set; }
        public List<int> Companies { get; set; }


    }

    public class EditUserModel : ModelBase
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "EmailRequired")]
        [Regex(RegexTypes.Email, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "EmailFormatError")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "UsernameRequired")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordRequired")]
        public string PhoneNumber { get; set; }


    }

    public class ResetPasswordModel : ModelBase
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordRequired")]
        public string Userpassword { get; set; }


    }
    public class ChangePasswordModel : ModelBase
    {

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordRequired")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "ConfirmPasswordRequired")]
        [Compare("Password", ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "PasswordsNotMatches")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}

