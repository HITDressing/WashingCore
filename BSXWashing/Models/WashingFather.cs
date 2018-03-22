using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BSXWashing.Models
{
    public class WashingFather
    {
        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("床单 1.2M")]
        public int ItemNum1 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("床单 1.5M")]
        public int ItemNum2 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("床单 1.8M")]
        public int ItemNum3 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("床单 1.2M")]
        public int ItemNum4 { get; set; }
    }
}
