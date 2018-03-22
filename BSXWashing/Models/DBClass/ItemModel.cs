using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BSXWashing.Models.DBClass
{
    public class ItemModel
    {
        [Key]
        [DisplayName("物品名称")]
        public string ItemName { get; set; }

        [Required]
        [DisplayName("物品价值")]
        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        public double ItemValue { get; set; }

        [DisplayName("物品备注")]
        [DataType(DataType.MultilineText)]
        public string ItemNote { get; set; }
    }
}
