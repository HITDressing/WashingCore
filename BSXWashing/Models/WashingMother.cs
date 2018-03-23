using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BSXWashing.Models
{
    public class WashingMother
    {
        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("床单 1.2M")]
        public int ItemNum1 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("床单 1.5M")]
        public int ItemNum2 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("床单 1.8M")]
        public int ItemNum3 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("被套 1.2M")]
        public int ItemNum4 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("被套 1.5M")]
        public int ItemNum5 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("被套 1.8M")]
        public int ItemNum6 { get; set; }

        //-------------------------------------------------

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("枕套")]
        public int ItemNum7 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("浴巾")]
        public int ItemNum8 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("地巾")]
        public int ItemNum9 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("毛巾")]
        public int ItemNum10 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("方巾")]
        public int ItemNum11 { get; set; }

        //---------------------------------------------------

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("大台布")]
        public int ItemNum12 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("香巾")]
        public int ItemNum13 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("小台布")]
        public int ItemNum14 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("口布")]
        public int ItemNum15 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("毛毯")]
        public int ItemNum16 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("桌围裙")]
        public int ItemNum17 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("厨衣")]
        public int ItemNum18 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("窗帘")]
        public int ItemNum19 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("窗帘内胆")]
        public int ItemNum20 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("浴帘")]
        public int ItemNum21 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("浴服")]
        public int ItemNum22 { get; set; }

        //-----------翻页--------------翻页-----------------

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("椅套")]
        public int ItemNum23 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("帽子")]
        public int ItemNum24 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("床裙")]
        public int ItemNum25 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("缎料工服")]
        public int ItemNum26 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("唐装")]
        public int ItemNum27 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("免烫工服")]
        public int ItemNum28 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("旗袍")]
        public int ItemNum29 { get; set; }

        //-----------------------------------------------

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("西服")]
        public int ItemNum30 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("领带")]
        public int ItemNum31 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("已烫工服")]
        public int ItemNum32 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("沙发套")]
        public int ItemNum33 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("床罩")]
        public int ItemNum34 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("抹布")]
        public int ItemNum35 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("保护垫")]
        public int ItemNum36 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("地毯清洗")]
        public int ItemNum37 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("美容足浴窄床单")]
        public int ItemNum38 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("预留")]
        public int ItemNum39 { get; set; }

        [Range(0, 2147483647, ErrorMessage = "非法数字")]
        [DisplayName("预留")]
        public int ItemNum40 { get; set; }
    }
}
