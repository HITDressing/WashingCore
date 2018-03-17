using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HITWashing.Models.DBClass
{
    public class WarehouseModel
    {
        [Key]
        [DisplayName("WID")]
        public int WarehouseID { get; set; }

        [Required]
        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        public int ItemNum_1 { get; set; }

        [Required]
        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        public int ItemNum_2 { get; set; }

        [Required]
        [Range(0, 2147483647, ErrorMessage = "非法数字")]

        public int ItemNum_3 { get; set; }

        public string AccountName { get; set; }
        public virtual AccountModel Account { get; set; }

    }
}
