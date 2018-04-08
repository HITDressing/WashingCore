using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSXWashing.Models.ViewModel
{
    public class ItemViewModel
    {
        public string ItemName { get; set; }
        public double ItemTrueUnitValue { get; set; }
        public int ItemQuantity { get; set; }
        public double ItemTrueTotalValue => ItemTrueUnitValue * ItemQuantity;

    }
}
