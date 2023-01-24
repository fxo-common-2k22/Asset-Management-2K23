using FAPP.Model;
using PagedList;
using System.Collections.Generic;
using FAPP.Areas.AM.BLL;
namespace FAPP.Areas.AM.ViewModels
{
    public class PurchaseReturnModelViewModel
    {
        public bool UnPosted { get; set; }
        public bool Posted { get; set; }
        public string Search { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public bool All { get; set; }
        public bool WithBalance { get; set; }
        public string CalDigit { get; set; }
        public long? InvoiceNo { get; set; }
        public AMPurchaseReturn PurchaseReturn { get; set; }
        public List<AMProceduresModel.v_mnl_PurchaseReturns_Result> v_mnl_PurchaseReturnsList { get; set; }
        public IPagedList<AMPurchaseReturn> PurchaseReturnPagedList { get; set; }
        public List<AMPurchaseReturnProduct> PurchaseReturnProduct { get; set; }
        public AMSupplierRefundInvoice SupplierRefundInvoice { get; set; }
    }
}