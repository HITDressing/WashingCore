using BSXWashing.Models.EnumClass;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BSXWashing.Models.ViewModel
{
    public class RangeDiscount
    {
        [Required]
        [DisplayName("用户级别")]
        public EnumAccountLevel Level { get; set; }

        [Required]
        [DisplayName("折扣系数")]
        [Range(0, 2147483647, ErrorMessage = "折扣不能为负")]
        public double DiscountValue { get; set; }
    }
}
