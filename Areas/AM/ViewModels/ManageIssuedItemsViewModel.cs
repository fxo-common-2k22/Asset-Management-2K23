using System;
using System.Collections.Generic;
using FAPP.Model;
using System.Web.Mvc;
using PagedList;
using FAPP.Areas.AM.BLL;
using FAPP.AM.Models;

namespace FAPP.Areas.AM.ViewModels
{
    public class ManageIssuedItemsViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public readonly int pagesize = 200;// OneDbContext.pagesize;
        public int? IssuedItemId { get; set; }
        public DateTime? IssueDate { get; set; }
        public short? DepartmentId { get; set; }
        public IEnumerable<SelectListItem> DepartmentsDD { get; set; }
        public IEnumerable<AMIssuedItem> IssuedItems { get; set; }
        public IPagedList<AMIssuedItem> IssuedItemPagedList { get; set; }
        public IPagedList<AMReturnIssue> ReturnIssuePagedList { get; set; }
        public IPagedList<v_mnl_AMItemPosition_Result> v_mnl_AMItemPositionPagedList { get; set; }
        public List<v_mnl_AMItemPosition_Result> v_mnl_AMItemPositionList { get; set; }
        public ItemRegister ItemRegisterItem { get; set; } = new ItemRegister() { DateOfEntry = DateTime.Now, Qty = 1 };
        public IPagedList<ItemRegister> CurrentItemRegister { get; set; }
        public int? CategoryId { get; set; }
        public int? ItemId { get; set; }
        public int? ItemCodeDetailId { get; set; }
        public string ItemCode { get; set; }
        public int? ConditionTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public int? RoomId { get; set; }
        public int? Type { get; set; }
        public string status { get; set; }
        public ItemRegisterEnum StatusID { get; set; }
    }

    public class ItemRegisterVM
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public readonly int pagesize = 200;
        public int? CategoryId { get; set; }
        public int? ItemId { get; set; }
        public short? DepartmentId { get; set; }
        public int? ConditionTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public int? RoomId { get; set; }
        public string ItemCode { get; set; }
        public string status { get; set; }
        //  public ItemRegisterEnum StatusID { get; set; }
        public ItemRegister ItemRegisterItem { get; set; } = new ItemRegister() { DateOfEntry = DateTime.Now, Qty = 1 };
        public IPagedList<ItemRegister> CurrentItemRegister { get; set; }
    }
}