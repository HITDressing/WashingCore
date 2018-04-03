namespace BSXWashing.Models.DBClass
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public class DiscountModel
    {
        [Key]
        [DisplayName("DID")]
        public int DiscountID { get; set; }

        [Required]
        [DisplayName("折扣系数")]
        [Range(0, 2147483647, ErrorMessage = "折扣不能为负")]
        public double DiscountValue { get; set; }

        [Required]
        public string AccountName { get; set; }
        public virtual AccountModel Account { get; set; }

        [DisplayName("折扣备注")]
        [DataType(DataType.MultilineText)]
        public string DiscountNote { get; set; }
    }
}
