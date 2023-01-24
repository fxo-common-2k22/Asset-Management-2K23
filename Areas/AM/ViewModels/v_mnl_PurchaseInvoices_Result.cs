using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class v_mnl_PurchaseInvoices_Result
    {
        public long PurchaseInvoiceId { get; set; }
        public string AccountPostedName { get; set; }
        public long? PurchaseOrderId { get; set; }
        public DateTime PurchaseInvoiceDate { get; set; }
        public int? SupplierId { get; set; }
        public decimal Discount { get; set; }
        public string Description { get; set; }
        public bool IsPosted { get; set; }
        public DateTime? PostedOn { get; set; }
        public int? PostedBy { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CancelledOn { get; set; }
        public int? CancelledBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public short? CurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public bool MovedToWarehouse { get; set; }
        public long? VoucherId { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? LabourCharges { get; set; }
        public decimal? OtherCharges { get; set; }
        public decimal? FareCharges { get; set; }
        public decimal? NetTotal { get; set; }
        public short? BranchId { get; set; }
        public bool IsApplyTax { get; set; }
        public bool IsAccountPosted { get; set; }
        public decimal? ReceivedAmount { get; set; }
        public string PostedName { get; set; }
        public string CancelledName { get; set; }
        public string ModifiedName { get; set; }
        public string CreatedName { get; set; }
        public string ClientName { get; set; }
        public decimal? Paid { get; set; }
        public bool? IsCheked { get; set; }
        public bool IsCreatedFromOpenningStock { get; set; }

        public int? WareHouseId { get; set; }
    }
}