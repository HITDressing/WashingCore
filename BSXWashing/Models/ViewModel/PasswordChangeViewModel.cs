namespace BSXWashing.Models.ViewModel
{
    using BSXWashing.Models.EnumClass;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class PasswordChangeViewModel
    {
        [DisplayName("旧密码")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "密码长度必须在8到32之间", MinimumLength = 8)]
        [Required]
        public string OldPassword { get; set; }

        [DisplayName("新密码")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "密码长度必须在8到32之间", MinimumLength = 8)]
        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword",ErrorMessage ="新密码确认必须和新密码相同")]
        [DisplayName("新密码确认")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "密码长度必须在8到32之间", MinimumLength = 8)]
        public string ConfirmNewPassword { get; set; }
    }
}
