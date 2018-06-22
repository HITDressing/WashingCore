namespace BSXWashing.Models.DBClass
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public class TopupModel
    {
        [Key]
        [DisplayName("TID")]
        public int TopupID { get; set; }

        [Required]
        [DisplayName("充值金额")]
        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        public double TopupValue { get; set; }

        [DisplayName("充值时间")]
        [DataType(DataType.DateTime)]
        public DateTime TopupTime { get; set; }

        [DisplayName("充值备注")]
        [DataType(DataType.MultilineText)]
        public string TopupNote { get; set; }

        [Required]
        [DisplayName("用户名")]
        public string AccountName { get; set; }
        public virtual AccountModel Account { get; set; }
    }
}
