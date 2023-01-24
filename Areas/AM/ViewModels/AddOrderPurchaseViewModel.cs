using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FAPP.Areas.AM.ViewModels
{
    public class AddOrderPurchaseViewModel
    {
        public long PurchaseOrderId { get; set; }
        public string PurchaseOrderCode { get; set; }
        public DateTime PurchaseOrderDate { get; set; }
        public int SupplierId { get; set; }
        public IEnumerable<SelectListItem> SuppliersDD { get; set; }
        public string Description { get; set; }
        public IEnumerable<SelectListItem> ItemsDD { get; set; }
        public List<PurchaseOrderViewModel> Details { get; set; }
        public bool EditMode { get; set; }

        //purhcase Invoice Properties
        public DateTime PurchaseInvoiceDate { get; set; }
        public string PIDescription { get; set; }
        public int PISupplierId { get; set; }
        public int StatusId { get; set; }
        public bool PICreated { get; set; }
        public long? PurchaseInvoiceId { get; set; }
    }
    public class PurchaseOrderViewModel
    {
        public string Barcode { get; set; }
        public long PurchaseOrderId { get; set; }
        public long POProductId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int? ConditionTypeId { get; set; }
    }
}