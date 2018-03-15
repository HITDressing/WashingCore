using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HITWashing.Models.DBClass
{
    public class PaybackModel
    {
        [Key]
        [DisplayName("POID")]
        public int PaybackOrderID { get; set; }

        public int UserID { get; set; }
    }
}
