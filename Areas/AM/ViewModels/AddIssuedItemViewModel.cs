using FAPP.AM.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FAPP.Areas.AM.ViewModels
{
    public class AddIssuedItemViewModel
    {
        public int TransferHistoryId { get; set; }
        public DateTime IssueDate { get; set; }
        public short? DepartmentId { get; set; }
        public string Description { get; set; }
        public IEnumerable<SelectListItem> DepartmentsDD { get; set; }
        public IEnumerable<SelectListItem> ItemsDD { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> EmployeesDD { get; set; }
        public IEnumerable<SelectListItem> RoomsDD { get; set; }
        public IEnumerable<SelectListItem> ConditionTypesDD { get; set; }
        public IEnumerable<TextValueId> ItemCodes { get; set; }
        public List<IssuedItemsDetailViewModel> Details { get; set; }
        public TransferHistory IssuedItems { get; set; } = new TransferHistory();
        public List<Model.AMIssuedItemDetail> IssuedItemDetails { get; set; } = new List<Model.AMIssuedItemDetail>();
    }

    public class IssuedItemsDetailViewModel
    {
        public int TransferHistoryId { get; set; }
        public int ItemRegisterId { get; set; }
        public DateTime IssueDate { get; set; }
        public string Barcode { get; set; }
        public int ProductId { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public int? EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? ConditionTypeId { get; set; }
        public int? RoomId { get; set; }
        public bool IsFixedAsset { get; set; }
    }
    public class TextValueId
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
    }
}