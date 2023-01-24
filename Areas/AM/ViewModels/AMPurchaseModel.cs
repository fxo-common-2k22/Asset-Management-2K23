
using PagedList;
using System;
using System.Collections.Generic;
using SupplierPayments = FAPP.Areas.Contact.Models.SupplierPayment;
using SupplierInvoicePayments = FAPP.Areas.Contact.Models.SupplierInvoicePayment;
using FAPP.Model;
using FAPP.Areas.AM.BLL;
using FAPP.INV.Models;

namespace FAPP.Areas.AM.ViewModels
{
    public class AMPurchaseModel
    {
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
        public InvPurchaseInvoice PurchaseInvoice { get; set; }
        public SupplierPayments SupplierPayment { get; set; }
        public List<InvPurchaseInvoiceProduct> PurchaseInvoiceProduct { get; set; }
        public List<v_mnl_PurchaseInvoices_Result> v_mnl_PurchaseInvoiceList { get; set; }
        public IPagedList<v_mnl_PurchaseInvoices_Result> v_mnl_PurchaseInvoicesPagedList { get; set; }
        public IPagedList<SupplierPayments> SupplierPaymentPagedList { get; set; }
        public List<SupplierInvoicePayments> SupplierInvoicePaymentList { get; set; }
        public SupplierInvoicePayments SupplierInvoicePayment { get; set; }
        public IPagedList<AMSupplierInvoicePayment> SupplierInvoicePaymentPagedList { get; set; }
        public AMIssuedItem IssuedItem { get; set; }
        public string PaymentType { get; set; }
    }
}