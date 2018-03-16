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
        [StringLength(32, ErrorMessage = "帐户名必须为4~32", MinimumLength = 4)]
        [DisplayName("帐户名")]
        public string AccountName { get; set; }

        [DisplayName("手机号")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11, ErrorMessage = "电话号码长度必须为11位", MinimumLength = 11)]
        public string MobileNumber { get; set; }

        [StringLength(64, ErrorMessage = "帐户名必须为4~64", MinimumLength = 4)]
        [DisplayName("商家地址")]
        public string Address { get; set; }

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

        [DisplayName("商户名")]//single
        [StringLength(32, ErrorMessage = "商户名必须为4~32", MinimumLength = 4)]
        [Required]
        public string StoreName { get; set; }

        [DisplayName("账户信息")]
        public virtual ICollection<BalanceModel> Balances { get; set; }

        [DisplayName("库存信息")]
        public virtual ICollection<WarehouseModel> Warehouses { get; set; }

        //[DisplayName("借订单用户信息")]
        //public virtual ICollection<BorrowModel> Borrows { get; set; }

        [DisplayName("借订单运送信息")]
        public virtual ICollection<BorrowModel> BorrowTransport { get; set; }


        //[DisplayName("还订单用户信息")]
        //public virtual ICollection<PaybackModel> Paybacks { get; set; }

        [DisplayName("还订单运送信息")]
        public virtual ICollection<PaybackModel> PaybackTransport { get; set; }
    }
}
