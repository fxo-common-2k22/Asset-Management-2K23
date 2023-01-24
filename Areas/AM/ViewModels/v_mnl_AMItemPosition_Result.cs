using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class v_mnl_AMItemPosition_Result
    {
        public long InvoiceId { get; set; }
        public short? BranchId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string EmpName { get; set; }
        public string DepartmentName { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public decimal? Qty { get; set; }
        public string Location { get; set; }
        public string Method { get; set; }
        public bool IsConsumable { get; set; }
        public int? EmployeeId { get; set; }
        public int? ItemId { get; set; }
        public int? ItemCodeDetailId { get; set; }
        public int? ConditionTypeId { get; set; }
        public string ConditionName { get; set; }
        public short? DepartmentId { get; set; }
        public int? RoomId { get; set; }
        public bool IsCheked { get; set; }
        public int? PurchaseInvoiceProductDetailId { get; set; }
        public long? PurchaseInvoiceProductId { get; set; }
    }
}