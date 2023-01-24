using FAPP.AM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class ReportModel
    {
        public string Search { get; set; }
        public DateTime? FromDateTime { get; set; }
        public DateTime? ToDateTime { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductId { get; set; }
        public int? StatusId { get; set; }
        public short? DepartmentId { get; set; }
        public int? EmployeeId { get; set; }
        public int? LocationId { get; set; }
        public List<IssuanceVM> IssuanceList { get; set; }
        public List<TransferHistory> TransferHistory { get; set; }
        public List<SummarizeData> SummarizeReport { get; set; }

    }

    public class IssuanceVM
    {
        public int ItemRegisterId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public string ItemCode { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Value { get; set; }
        public DateTime IssueDate { get; set; }
        public int Status { get; set; }
        public short DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public int ConditionTypeId { get; set; }



    }
    public class SummarizeData
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? Available { get; set; }
        public int? Issued { get; set; }
        public int? Damaged { get; set; }
        public int? InStock { get; set; }
    }



}