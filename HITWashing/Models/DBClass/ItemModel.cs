using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HITWashing.Models.DBClass
{
    public class ItemModel
    {
        [Key]
        [DisplayName("IID")]
        public int ItemID { get; set; }

        [Required]
        [DisplayName("物品名称")]
        public string ItemName { get; set; }

        [Required]
        [DisplayName("物品价值")]
        public double ItemValue { get; set; }

    }
}
