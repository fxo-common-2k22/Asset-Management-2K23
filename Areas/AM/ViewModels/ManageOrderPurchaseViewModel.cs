using FAPP.INV.Models;
using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FAPP.Areas.AM.ViewModels
{
    public class ManageOrderPurchaseViewModel
    {
        public DateTime? OrderFrom { get; set; }
        public DateTime? OrderTo { get; set; }
        public string PurchaseOrderCode { get; set; }
        public int? SupplierId { get; set; }
        public int? OrderPurchaseId { get; set; }
        public IEnumerable<InvPurchaseOrder> PurchaseOrders { get; set; }
        public IEnumerable<SelectListItem> SuppliersDD { get; set; }
    }
}