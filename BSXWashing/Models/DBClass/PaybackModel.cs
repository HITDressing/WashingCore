using System;
namespace BSXWashing.Models.DBClass
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public class PaybackModel : WashingMother
    {
        [Key]
        [DisplayName("POID")]
        public int PaybackOrderID { get; set; }

        [DisplayName("配送人员")]
        public string TranName { get; set; }

        public string AccountName { get; set; }
        public virtual AccountModel Account { get; set; }

        [DisplayName("开始时间")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime { get; set; }

        [DisplayName("送达时间")]
        [DataType(DataType.DateTime)]
        public DateTime TranTime { get; set; }

        [DisplayName("完成时间")]
        [DataType(DataType.DateTime)]
        public DateTime FinishTime { get; set; }

        //-------------------------------------------------

        [DisplayName("是否取消")]
        public bool IsCanceled { get; set; }

        [DisplayName("是否完成")]
        public bool IsCompleted { get; set; }

        [DisplayName("是否送达")]
        public bool IsTraned { get; set; }

        [DisplayName("回收订单备注")]
        [DataType(DataType.MultilineText)]
        public string BorrowNote { get; set; }
    }
}
