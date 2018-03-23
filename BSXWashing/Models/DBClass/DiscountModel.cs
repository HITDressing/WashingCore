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
        [Range(0,1,ErrorMessage ="折扣为0~1之间的数")]
        public double DiscountValue { get; set; }

        [Required]
        public string AccountName { get; set; }
        public virtual AccountModel Account { get; set; }

        [DisplayName("库存备注")]
        [DataType(DataType.MultilineText)]
        public string DiscountNote { get; set; }
    }
}
