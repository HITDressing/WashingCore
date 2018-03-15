using HITWashing.Models.EnumClass;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HITWashing.Models.DBClass
{
    public class AccountModel
    {
        [Key]
        [DisplayName("AID")]
        public int AccountID { get; set; }

        [DisplayName("手机号")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11, ErrorMessage = "电话号码长度必须为11位", MinimumLength = 11)]
        public string MobileNumber { get; set; }

        [Required]
        [DisplayName("坐标X")]
        public double LocationX { get; set; }

        [Required]
        [DisplayName("坐标Y")]
        public double LocationY { get; set; }

        [Required]
        [DisplayName("用户类别")]
        public EnumAccountType Type { get; set; }

        [DisplayName("用户密码")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "密码长度必须在8到32之间", MinimumLength = 8)]
        [Required]
        public string Password { get; set; }

        [DisplayName("密码确认")]
        [NotMapped]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "密码长度必须在8到32之间", MinimumLength = 8)]
        public string ConfirmPassword { get; set; }

        [DisplayName("混淆盐")]
        public string Salt { get; set; }

        [DisplayName("商户名")]
        public string StoreName { get; set; }

        [DisplayName("账户信息")]
        public virtual ICollection<BalanceModel> Balances { get; set; }

        [DisplayName("借订单信息")]
        public virtual ICollection<BorrowModel> Borrows { get; set; }

        [DisplayName("运送订单信息")]
        public virtual ICollection<BorrowModel> BorrowTransport { get; set; }
    }
}
