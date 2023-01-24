using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FAPP.Areas.AM.ViewModels
{
    public class AddEditRequestViewModel
    {
        public int? RequestId { get; set; }
        public int? RequestNo { get; set; }
        public bool EditMode { get; set; }
        public short DepartmentId { get; set; }
        public IEnumerable<SelectListItem> DepartmentsDD { get; set; }
        public int EmployeeId { get; set; }
        public IEnumerable<SelectListItem> EmployeeDD { get; set; }
        public DateTime RequestDate { get; set; }
        public string Description { get; set; }
        public List<RequestDetailsViewModel> Details { get; set; }
        public IEnumerable<SelectListItem> ItemsDD { get; set; }
        public IEnumerable<SelectListItem> RoomDD { get; set; }
        public IEnumerable<SelectListItem> Status { get; set; }
        public int StatusId { get; set; }

        public DateTime OrderPurchaseDate { get; set; }
        public int ClientId { get; set; }
        public string PurchaseOrderDescription { get; set; }
        public IEnumerable<SelectListItem> ClientsDD { get; set; }
    }

    public class RequestDetailsViewModel
    {
        public int? RoomNumber { get; set; }
        public int RequestDetailId { get; set; }
        public int? ProductId { get; set; }
        public int? RoomId { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}