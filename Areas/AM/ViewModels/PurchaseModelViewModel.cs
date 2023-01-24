using FAPP.Model;
using PagedList;
using System;
using System.Collections.Generic;
using AMSupplierPayment = FAPP.Areas.Contact.Models.SupplierPayment;
using AMSupplierInvoicePayment = FAPP.Areas.Contact.Models.SupplierInvoicePayment;
using FAPP.Areas.AM.BLL;
namespace FAPP.Areas.AM.ViewModels
{
    public class PurchaseModelViewModel
    {
        public bool PostedToAccount { get; set; }
        public bool UnPostedToAccount { get; set; }
        public bool All { get; set; }
        public bool UnPosted { get; set; }
        public bool Posted { get; set; }
        public string CashAccountId { get; set; }
        public string BankAccountId { get; set; }
        public bool WithBalance { get; set; }
        public decimal? PaymentAmount { get; set; }
        public int? SupplierId { get; set; }
        public string Search { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Int64? InvoiceNo { get; set; }
        public string CalDigit { get; set; }
        //public AMPurchaseInvoice PurchaseInvoice { get; set; }
        public AMSupplierPayment SupplierPayment { get; set; }
        public List<AMPurchaseInvoiceProduct> PurchaseInvoiceProduct { get; set; }
        public List<v_mnl_PurchaseInvoices_Result> v_mnl_PurchaseInvoiceList { get; set; }
        public IPagedList<v_mnl_PurchaseInvoices_Result> v_mnl_PurchaseInvoicesPagedList { get; set; }
        public IPagedList<AMSupplierPayment> SupplierPaymentPagedList { get; set; }
        public List<AMSupplierInvoicePayment> SupplierInvoicePaymentList { get; set; }
        public AMSupplierInvoicePayment SupplierInvoicePayment { get; set; }
        public IPagedList<AMSupplierInvoicePayment> SupplierInvoicePaymentPagedList { get; set; }
        public List<AMSupplierInvoicePayment> RelatedSupplierInvoicePayments { get; set; }
        public AMIssuedItem IssuedItem { get; set; }
        public IEnumerable<PurchaseInvoiceProductDetailVM> PIPDs { get; set; }
        public bool IsFixedAsset { get; set; }
        public bool HasPendingInvoicePosts { get; set; }
        public string FeedbackMessage { get; set; }
    }
    public class PurchaseInvoiceProductDetailVM
    {
        public int PIPDId { get; set; }
        public int? OpeningStockId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
    }
}