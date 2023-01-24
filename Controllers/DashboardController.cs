using FAPP.DAL;
using FAPP.BLL;
using FAPP.Helpers;
using FAPP.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using FAPP.ViewModel.Common.Student;
using System.Data.Entity;

namespace FAPP.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Dashboard
        public async Task<ActionResult> School()
        {
            GraphsModel graphsModel = new GraphsModel();
            graphsModel.Action = ControllerContext.RouteData.Values["action"].ToString();
            graphsModel.Contoller = ControllerContext.RouteData.Values["Controller"].ToString();
            //graphsModel.Area = ControllerContext.RouteData.DataTokens["area"].ToString();
            //graphsModel.Module = ControllerContext.RouteData.DataTokens["area"].ToString();
            DateTime today = DateTime.Now.Date;
            graphsModel.Url = Request.RawUrl;
            graphsModel = Utilities.FetchGraphsData(db, graphsModel);
            DashboardGraphsBLL.GetDataForDashboardItems(db, graphsModel, SessionHelper.BranchId, today, SessionHelper.UserId, SessionHelper.CurrentSessionId);
            graphsModel = await GetChartData(graphsModel);
            return View(graphsModel);
        }
        public ActionResult Clinic()
        {
            return View();
        }
        [Obsolete("Slowing process")]
        private async Task<GraphsModel> GetChartData(GraphsModel vm)
        {
            //try
            //{
            //    int? moduleId = 1;
            //    string url = $@"{SessionHelper.Urlapi}/api/Messaging/";
            //    using (var client = new HttpClient())
            //    {
            //        client.BaseAddress = new Uri(url);
            //        client.DefaultRequestHeaders.Accept.Clear();
            //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //        // New code:
            //        HttpResponseMessage response = await client.GetAsync($@"GetSMSQueue?BranchId={SessionHelper.BranchId}&Page=1&TotalRecords&SearchParams=Sent&SMSQueueId&ModuleId={moduleId}&Batch");
            //        if (response.IsSuccessStatusCode)
            //        {
            //            vm.SMSQueue = await response.Content.ReadAsAsync<List<MessagingViewModel>>();
            //        }

            //    }
            //}
            //catch (Exception)
            //{

            //}
            //var types = db.TemplateTypes.Include(m => m.Module)
            //    .Where(s => s.ModuleId == (moduleId.HasValue && moduleId.Value > 0 ? moduleId.Value : s.ModuleId))
            //    .ToList();
            //var typesIds = types.Select(s => (short?)s.TemplateTypeId).ToList();
            //var smsQueue = db.SmsQueues.Where(q => typesIds.Contains(q.TemplateTypeId));
            try
            {
                //vm.SmsChart = new Dictionary<string, int>();
                //foreach (var type in vm.SMSQueue)
                //{
                //    var count = vm.SMSQueue.Where(q => q.TemplateTypeId == type.TemplateTypeId).Count();
                //    if (!vm.SmsChart.ContainsKey(type.TemplateTypeName))
                //    {
                //        vm.SmsChart.Add(type.TemplateTypeName, count);
                //    }
                //}
                var sc = await db.Database.SqlQuery<KV>($@"
                    SELECT tt.TemplateTypeName as [Key], COUNT(*) AS [Value]
                    FROM Media.SmsQueue AS sq
                    INNER JOIN Media.TemplateTypes AS tt ON sq.TemplateTypeId = tt.TemplateTypeId
                    WHERE (sq.ScheduledOnDate = '{DateTime.Today.ToddMMMyyyyString()}')
	                    AND (sq.BranchId = {SessionHelper.BranchId})
	                    AND (sq.ModuleId = {SessionHelper.ModuleId})
	                    AND (sq.MessageStatus = 'Sent')
                    GROUP BY tt.TemplateTypeName, sq.BranchId").ToListAsync();

                vm.SmsChart = sc.ToDictionary(x => x.Key, y => y.Value);
            }
            catch (Exception)
            {

            }
            return vm;
        }
        //[HttpGet]
        //public async Task<JsonResult> FeeStatsChartData()
        //{
        //    string url = $@"{SessionHelper.Urlapi}/api/Academics/Fee/";
        //    FeeStatsViewModel feeStats = new FeeStatsViewModel();
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(url);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        // New code:
        //        HttpResponseMessage response = await client.GetAsync($@"GetFeeStats?BranchId={SessionHelper.BranchId}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            feeStats = await response.Content.ReadAsAsync<FeeStatsViewModel>();
        //        }

        //    }

        //    List<DrillDownColoumnBar> coloumnsList = new List<DrillDownColoumnBar>();
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Total Generated Amount", y = Convert.ToDecimal(feeStats.TotalGeneratedAmount), drilldown = null });
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Total Received Amount", y = Convert.ToDecimal(feeStats.TotalReceivedAmount), drilldown = null });
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Total Defaulters Amount", y = Convert.ToDecimal(feeStats.DefaulterAmount), drilldown = null });

        //    var FeeStatsChart = new ViewModel.Common.Student.FeeStatsGraphModel()
        //    {
        //        FeeStatsColoumns = coloumnsList,
        //    };

        //    return Json(FeeStatsChart, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public async Task<JsonResult> EmployeeTodaysAttendanceStatsChartData()
        //{
        //    string url = $@"{SessionHelper.Urlapi}/api/Admin/Attendance/";
        //    EmployeeOneDayAttendanceStats EmployeeAttStats = new EmployeeOneDayAttendanceStats();
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(url);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        // New code:
        //        HttpResponseMessage response = await client.GetAsync($@"EmployeesTodaysAttendance?BranchId={SessionHelper.BranchId}&AttendanceDate={DateTime.Today.ToString("MM/dd/yyyy")}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            EmployeeAttStats = await response.Content.ReadAsAsync<EmployeeOneDayAttendanceStats>();
        //        }

        //    }

        //    List<DrillDownColoumnBar> coloumnsList = new List<DrillDownColoumnBar>();
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Total", y = EmployeeAttStats.Total, drilldown = null });
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Present", y = EmployeeAttStats.Present, drilldown = null });
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Absent", y = EmployeeAttStats.Absent, drilldown = null });
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Leave", y = EmployeeAttStats.Leave, drilldown = null });

        //    var EmployeeAttendanceStatsChart = new ViewModel.Common.Student.EmployeesTodaysAttendanceStatsGraphModel()
        //    {
        //        EmployeesTodaysAttendanceStatsColoumns = coloumnsList,
        //    };

        //    return Json(EmployeeAttendanceStatsChart, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public async Task<JsonResult> StudentsTodaysAttendanceStatsChartData()
        //{
        //    string url = $@"{SessionHelper.Urlapi}/api/Admin/Attendance/";
        //    StudentsOneDayAttendanceStats studentAttStats = new StudentsOneDayAttendanceStats();
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(url);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        // New code:
        //        HttpResponseMessage response = await client.GetAsync($@"StudentsTodaysAttendance?BranchId={SessionHelper.BranchId}&AttendanceDate={DateTime.Today.ToString("MM/dd/yyyy")}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            studentAttStats = await response.Content.ReadAsAsync<StudentsOneDayAttendanceStats>();
        //        }

        //    }


        //    List<DrillDownColoumnBar> coloumnsList = new List<DrillDownColoumnBar>();
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Total", y = Convert.ToDecimal(studentAttStats.Total), drilldown = null });
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Present", y = Convert.ToDecimal(studentAttStats.Present), drilldown = null });
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Absent", y = Convert.ToDecimal(studentAttStats.Absent), drilldown = null });
        //    coloumnsList.Add(new DrillDownColoumnBar() { name = "Leave", y = Convert.ToDecimal(studentAttStats.Leave), drilldown = null });

        //    var StudentsTodaysAttendanceStatsChart = new StudentsTodaysAttendanceStatsGraphModel()
        //    {
        //        StudentsTodaysAttendanceStatsColoumns = coloumnsList,
        //    };

        //    return Json(StudentsTodaysAttendanceStatsChart, JsonRequestBehavior.AllowGet);
        //}
        //public async System.Threading.Tasks.Task<JsonResult> SearchStudentsLive(string q)
        //{
        //    List<FAPP.Areas.Contact.ContactList> _ContactList = new List<FAPP.Areas.Contact.ContactList>();
        //    List<FAPP.Areas.Contact.ContactList> lists = new List<FAPP.Areas.Contact.ContactList>();
        //    //_ContactList.Add(new FAPP.Areas.Contact.ContactList { id = "", text = "==Select Agent==" });
        //    if (!string.IsNullOrEmpty(q))
        //    {
        //        lists = await db.Students.Where(u => u.BranchId == SessionHelper.BranchId && (u.FullName.ToUpper()).Contains(q.ToUpper()) ||
        //            (u.RegistrationNumber == null ? "" : u.RegistrationNumber.ToUpper()).Contains(q.ToUpper())).
        //            Select(u => new FAPP.Areas.Contact.ContactList { id = u.StudentId.ToString(), text = u.StudentId.ToString() + "-" + u.RegistrationNumber + "-" + u.FullName }).ToListAsync();
        //    }

        //    foreach (var item in lists)
        //    {
        //        _ContactList.Add(new FAPP.Areas.Contact.ContactList { id = item.id, text = item.text });
        //    }
        //    return Json(_ContactList, JsonRequestBehavior.AllowGet);
        //}
    }
    public class KV
    {
        public string Key { get; set; }
        public int Value { get; set; }
    }
}