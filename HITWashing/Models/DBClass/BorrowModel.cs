using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HITWashing.Models.DBClass
{
    public class BorrowModel
    {
        [Key]
        [DisplayName("BOID")]
        public int BorrowOrderID { get; set; }

        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual AccountModel UAccount { get; set; }

        public int TransportID { get; set; }
        [ForeignKey("TransportID")]
        public virtual AccountModel TAccount { get; set; }

        [DisplayName("是否取消")]
        public bool isCanceled { get; set; }

        [DisplayName("是否完成")]
        public bool isComplited { get; set; }
    }
}
