using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class v_mnl_AMPurchaseItemCode_Result
    {
        public long PurchaseInvoiceId { get; set; }
        public int DetailId { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
        public int ItemId { get; set; }
        public bool IsConsumable { get; set; }
    }
}