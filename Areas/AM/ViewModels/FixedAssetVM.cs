using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class FixedAssetVM
    {
        public string Search { get; set; }
        public string Status { get; set; }
        public bool InStock { get; set; }
        public string Command { get; set; }
        public int? CategoryId { get; set; }
        public short? DepartmentId { get; set; }
        public int? ProductId { get; set; }
        public int? EmployeeId { get; set; }
        public int? LocationId { get; set; }
        public int? ConditionTypeId { get; set; }
        public readonly int pagesize = 200;
        public List<FixedAssetRegVM> FixedAssetRegVM { get; set; } = new List<FixedAssetRegVM>();
        public IPagedList<FixedAssetRegVM> FixedAssetRegPager { get; set; }

        public PostingData PostingData { get; set; } = new PostingData();
    }

    public class FixedAssetRegVM
    {
        public bool check { get; set; } = false;
        public int TransferHistoryId { get; set; }
        public int ItemRegisterId { get; set; }
        public DateTime IssueDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string ItemCode { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public decimal? Value { get; set; }
        public int Status { get; set; }
        public short? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int? LocationId { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public int? ConditionTypeId { get; set; }
    }
    public class PostingData
    {
        public short? DepartmentId { get; set; }
        public int? EmployeeId { get; set; }
        public int? LocationId { get; set; }
        public int? ConditionTypeId { get; set; }
        public string Description { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
    }
}