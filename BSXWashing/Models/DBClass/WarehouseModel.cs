namespace BSXWashing.Models.DBClass
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public class WarehouseModel : WashingMother
    {
        [Key]
        [DisplayName("WID")]
        public int WarehouseID { get; set; }

        [Required]
        public string AccountName { get; set; }
        public virtual AccountModel Account { get; set; }

        [DisplayName("库存备注")]
        [DataType(DataType.MultilineText)]
        public string WareNote { get; set; }
    }
}
