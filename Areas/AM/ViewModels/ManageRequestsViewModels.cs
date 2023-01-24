using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FAPP.Areas.AM.ViewModels
{
    public class ManageRequestsViewModel
    {
        public short? DepartmentId { get; set; }
        public IEnumerable<SelectListItem> DepartmentsDD { get; set; }
        public int? EmployeeId { get; set; }
        public IEnumerable<SelectListItem> EmployeeDD { get; set; }
        public DateTime RequestDate { get; set; }
        public IEnumerable<SelectListItem> ItemsDD { get; set; }
        public int? RequestId { get; set; }
        //public IEnumerable<AMRequest> Requests { get; set; }
        public IEnumerable<AMRequestVM> Requests { get; set; }
        public List<RequestDetailsViewModel> Details { get; set; }
        public int? StatusId { get; set; }
    }
    public class AMRequestVM
    {
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Description { get; set; }
        public int? EmployeeId { get; set; }
        public short? DepartmentId { get; set; }
        public int StatusId { get; set; }
        public string EmpName { get; set; }
        public string DepartmentName { get; set; }
        public string StatusName { get; set; }
        public long? PurchaseOrderId { get; set; }
    }
}