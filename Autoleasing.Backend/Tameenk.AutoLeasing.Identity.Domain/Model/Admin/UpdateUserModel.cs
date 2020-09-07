
using System.ComponentModel.DataAnnotations;

namespace Tameenk.AutoLeasing.Identity.Domain
{
    public class UpdateUserModel : ModelBase
    {
        [Required(AllowEmptyStrings = false)]
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public bool NewStatus { get; set; }

    }
}
