using FAPP.Model;
using FAPP.ViewModel;
using FAPP.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FAPP.ViewModel.Common.Student
{
    public class FeeStatsGraphModel
    {
        public List<DrillDownColoumnBar> FeeStatsColoumns { get; set; }
    }
    public class StudentsTodaysAttendanceStatsGraphModel
    {
        public List<DrillDownColoumnBar> StudentsTodaysAttendanceStatsColoumns { get; set; }
    }
    public class EmployeesTodaysAttendanceStatsGraphModel
    {
        public List<DrillDownColoumnBar> EmployeesTodaysAttendanceStatsColoumns { get; set; }
    }
    public class StudentAttendancePieGraphModel
    {
        public List<PieChartSliceWithColor> AttendanceSlices { get; set; }
    }
    public class DailyAttendanceGrapModel
    {
        public List<string> Days { get; set; }
        public List<DailyAttendanceResult> DailyAttendanceResults { get; set; }
    }
    public class DepartmentWiseEmployeesGraphModel
    {
        public List<string> Departments { get; set; }
        public List<int> Employees { get; set; }
    }
    public class DailyAttendanceResult
    {
        public string name { get; set; }
        public List<int> data { get; set; }
        public DailyAttendanceResult()
        {
            data = new List<int>();
        }
    }
    public class StudentStrengthSectionWiseModel
    {
        public List<string> Classes { get; set; }
        public List<StudentStrengthSectionWise> StudentStrengthSectionWiseList { get; set; }
    }
    public class MonthWiseVoucherStatsModel
    {
        public List<string> Months { get; set; }
        public List<MonthWiseVoucherStats> VoucherStatsList { get; set; }
    }
    public class StudentStrengthSectionWise
    {
        public string name { get; set; }
        public List<int> data { get; set; }
        public StudentStrengthSectionWise()
        {
            data = new List<int>();
        }
    }
    public class MonthWiseVoucherStats
    {
        public string name { get; set; }
        public List<int> data { get; set; }
        public MonthWiseVoucherStats()
        {
            data = new List<int>();
        }
    }
    public class StudentAdmissionWithdrawlModel
    {
        public List<string> Days { get; set; }
        public List<StudentAdmissionWithdrawl> studentAdmissionWithDrawl { get; set; }
    }
    public class StudentAdmissionWithdrawl
    {
        public string name { get; set; }
        public List<int> data { get; set; }
        public StudentAdmissionWithdrawl()
        {
            data = new List<int>();
        }
    }
   
}