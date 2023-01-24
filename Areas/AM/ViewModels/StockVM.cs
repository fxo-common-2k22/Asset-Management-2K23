using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class StockVM
    {
    }
    public class ItemCodes
    {
        public int ItemRegisterId { get; set; }
        public int ProductId { get; set; }
        public string ItemCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}