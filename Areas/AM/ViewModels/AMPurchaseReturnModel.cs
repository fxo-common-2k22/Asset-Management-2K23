using FAPP.Model;
using PagedList;
using System.Collections.Generic;

namespace FAPP.Areas.AM.ViewModels
{
    public class AMPurchaseReturnModel
    {
        public string Search { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public bool All { get; set; }
        public bool WithBalance { get; set; }
        public string CalDigit { get; set; }
        public long? InvoiceNo { get; set; }
        public AMPurchaseReturn PurchaseReturn { get; set; }
        public IPagedList<AMPurchaseReturn> PurchaseReturnPagedList { get; set; }
        public List<AMPurchaseReturnProduct> PurchaseReturnProduct { get; set; }
        public AMSupplierRefundInvoice SupplierRefundInvoice { get; set; }
    }
}