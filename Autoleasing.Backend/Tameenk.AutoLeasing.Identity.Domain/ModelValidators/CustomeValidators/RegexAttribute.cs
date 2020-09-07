using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Tameenk.AutoLeasing.Identity.Domain
{
    public class RegexAttribute : ValidationAttribute
    {
        private RegexTypes Type { get; set; }
        private string Pattern { get; set; }
        public RegexAttribute(RegexTypes type, string pattern = null)
        {
            Type = type;
            if (!string.IsNullOrEmpty(pattern))
            {
                Pattern = pattern;
            }
            if (Type == RegexTypes.Email)
            {
                Pattern = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            }
            else if (Type == RegexTypes.Phone)
            {
                Pattern = @"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$";
            }
            else if (Type == RegexTypes.Barcode)
            {
                Pattern = @"^ASD-[A-Z]{3}-\d+";
            }
            else if (Type == RegexTypes.Number)
            {
                Pattern = @"^[0-9]+$";
            }
            else if (Type == RegexTypes.Text)
            {
                Pattern = @"^[0-9]+$";
            }
            else if (Type == RegexTypes.English)
            {
                Pattern = @"^[A-Za-z ]+$";
            }
            else if (Type == RegexTypes.NationalId)
            {
                pattern = @"^[0-9]{10}$"; 
            }
        }
        public override bool IsValid(object value)
        {
            return Regex.IsMatch(value.ToString(), Pattern);
        }
    }
    public class MyModel
    {
        [Regex(RegexTypes.Email)]
        public string email { get; set; }
    }
    public enum RegexTypes
    {
        Email,
        Phone,
        Barcode,
        Number,
        Text,
        English,
        NationalId
    }
}
