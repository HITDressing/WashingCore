using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HITWashing.Models.DBClass
{
    public class PaybackModel
    {
        [Key]
        [DisplayName("POID")]
        public int PaybackOrderID { get; set; }

        [DisplayName("运送人员ID")]

        public string UserName { get; set; }//运送ID

        public string AccountName { get; set; }
        public virtual AccountModel Account { get; set; }

        [DisplayName("床单")]
        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [Required]
        public int ItemNum_1 { get; set; }

        [DisplayName("枕套")]
        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [Required]
        public int ItemNum_2 { get; set; }

        [DisplayName("浴巾")]
        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [Required]
        public int ItemNum_3 { get; set; }

        [DisplayName("是否取消")]
        public bool IsCanceled { get; set; }

        [DisplayName("是否完成")]
        public bool IsCompleted { get; set; }

    }
}
