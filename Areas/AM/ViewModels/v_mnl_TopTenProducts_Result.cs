using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class v_mnl_TopTenProducts_Result
    {
        public string ED { get; set; }
        public string MM { get; set; }
        public decimal? Percentage { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Quantity { get; set; }
        public string Type { get; set; }
        public decimal? NetTotal { get; set; }
        public DateTime SaleInvoiceDate { get; set; }
        public string Day { get; set; }
    }
}