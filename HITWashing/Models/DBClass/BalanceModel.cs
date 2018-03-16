using HITWashing.Models.EnumClass;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HITWashing.Models.DBClass
{
    public class BalanceModel
    {
        [Key]
        [DisplayName("BID")]
        public int BalanceID { get; set; }

        [DisplayName("余额")]
        public double Balance { get; set; }

        public string AccountName { get; set; }
        public virtual AccountModel Account { get; set; }
    }
}
