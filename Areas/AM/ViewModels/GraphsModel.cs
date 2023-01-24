using FAPP.Areas.AM.BLL;
using FAPP.Models;
using FAPP.Areas.AM.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;


namespace FAPP.Areas.AM.ViewModels
{
    public partial class GraphsModel
    {
        public GraphsModel()
        {
            //AttendanceStatus = new AttendanceStatus();
        }


        public string Action { get; set; }
        public string Contoller { get; set; }
        public string Area { get; set; }
        public string Url { get; set; }
        public string Module { get; set; }


        public bool ShowStats { get; set; }
        public bool ShowWeeklySale { get; set; }
        public bool ShowMonthlyAttendance { get; set; }
        public bool ShowTransactions { get; set; }
        public bool ShowQuickLinks { get; set; }
        public bool ShowSomething { get; set; }
        public bool ShowDashboardMenus { get; set; }
        public bool ShowIncomeExpense { get; set; }

        public int logins { get; set; }
        public int clients { get; set; }
        public int suppliers { get; set; }
        public int users { get; set; }
        //UrlSettings
        public UrlSettingVM UrlSetting { get; set; }
        //Session Management Stats
        public int SessionDays { get; set; }
        public int SessionGroups { get; set; }
        public int SessionStudents { get; set; }
        public int SessionTeachers { get; set; }
        public string SessionStartTime { get; set; }
        public string SessionEndTime { get; set; }

        public int Deparments { get; set; }
        public int Designations { get; set; }
        public int Males { get; set; }
        public int Females { get; set; }

        public string Links { get; set; }
        public string weeklyStrength { get; set; }
        public string DashboardConstraint { get; set; }

        public List<v_mnl_TopTenProducts_Result> WeeklySaleList { get; set; }
        public List<v_mnl_TopTenProducts_Result> MonthlyAttendanceList { get; set; }
        public List<v_mnl_TopTenProducts_Result> TransactionList { get; set; }
        public List<v_mnl_TopTenProducts_Result> IncomeExpenseList { get; set; }
        public List<v_mnl_FormRights_Result> v_mnl_FormRights { get; set; }
        // public List<v_mnl_EmployeeAttendance> v_mnl_EmployeeAttendance { get; set; }
        public List<FAPP.Service.Helper> _views { get; set; }
        public List<v_mnl_UpcomingEvent_Result> v_mnl_UpcomingEvent { get; set; }
        // public DailyAttendanceGrapModel DailyAttendanceGraph { get; set; }
        //  public DepartmentWiseEmployeesGraphModel DepartmentWiseEmployeeGraph { get; set; }
        //  public EmployeeGendersGraphModel EmployeeGendersGraph { get; set; }
        //  public StudentGendersGraphModel StudentGendersGraph { get; set; }
        //  public FeeStatsGraphModel FeeStatsGraph { get; set; }
        public List<string> Classes { get; set; }
        //public List<StudentStrengthGraphData> StudentStrengthGraphDataList { get; set; }
        //   public StudentStrengthSectionWiseModel StudentStrengthGraphDataList { get; set; }
        //    public StudentStrengthSectionWiseModel StudentAttendanceStatsGraphList { get; set; }
        //   public StudentAdmissionWithdrawlModel StudentAdmissionWithDrawlList { get; set; }
        //    public AttendanceStatus AttendanceStatus { get; set; }
        // public List<Teachers> Teachers { get; set; }
        //public List<SentSMSViewModel> SMSQueue { get; set; }
        public string availableTeacherHtml { get; set; }
        //public StudentAttendancePieGraphModel StudentAttendancePieChart { get; set; }
        //   public Company Company { get; set; }
        public IEnumerable<SelectListItem> AuthTypesDD { get; set; }
        public Dictionary<string, int> SmsChart { get; set; }

    }
    //POS
    public partial class GraphsModel
    {
        public List<ClientSaleInvoiceBalanceList> ClientSaleInvoiceBalanceList { get; set; }
    }
}