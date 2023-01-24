using FAPP.BLL;
using FAPP.DAL;
using FAPP.Model;
using FAPP.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
using PrimaryData = FAPP.Areas.AM.ViewModels.PrimaryData;
using FAPP.Areas.AM.ViewModels;
using FAPP.Areas.AM.ViewModel;
using FAPP.Areas.AM.FormsBLL;

namespace FAPP.Controllers
{
    public class HomeController : BaseController
    {
        private short _BranchId = SessionHelper.BranchId;
        private Guid _SessionId = SessionHelper.CurrentSessionId;
        //OneDbContext db = new OneDbContext();

        public ActionResult Index()
        {
            GraphsModel vm = new GraphsModel();
            //ViewBag.ErrorMsg = CheckAppConstraints();
            return View(vm);
        }

        public ActionResult Index1()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult SaveTheme(string theme)
        {

            var result = db.Users.Where(u => u.UserID == SessionHelper.UserID).FirstOrDefault();
            if (result != null)
            {
                result.ThemeColor = theme;
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                SessionHelper.ThemeColor = theme;
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Branches()
        //{
        //    var list = ProceduresModel.v_mnl_UserBranches(db).Where(u => u.Active == true && u.UserId == FAPP.DAL.SessionHelper.UserID).ToList();
        //    return PartialView("_Braches", list);
        //}


        //public ActionResult FiscalYears(string URL)
        //{
        //    var list = db.FiscalYears.Where(u => u.BranchId == SessionHelper.BranchId).OrderByDescending(u => u.FiscalYearId).ToList();
        //    return PartialView("_FiscalYear", list);
        //}


        public ActionResult SaveBranch(int? id, string url)
        {
            if (id != null)
            {
                var _branch = db.Branches.Where(u => u.BranchId == id).FirstOrDefault();
                if (_branch != null)
                {
                    SessionHelper.BranchId = Convert.ToInt16(id);
                    SessionHelper.BranchName = _branch.Name;
                    SessionHelper.BranchCode = _branch.BranchCode;
                    ProceduresModel.ResetSessionHelper_Branch(db, _branch, _branch.BranchId, SessionHelper.UserId);
                }
            }
            return Redirect("~/Home/Index");
            //if (string.IsNullOrEmpty(url))
            //{
            //    return Redirect("~/Home/Index");
            //}
            //else
            //{
            //    return Redirect(url);
            //}
        }

        public JsonResult loadgif()
        {
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Summary_Subjects(string Day)
        {
            //var list = db.v__subjects_Graphically.Where(u => u.BranchId == SessionHelper.BranchId && u.DayName == Day).ToList();
            var list = "";
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        public ActionResult SaveFiscalYear(int? id, string url)
        {
            if (id != null)
            {
                var fiscal = db.FiscalYears.Where(u => u.FiscalYearId == id).FirstOrDefault();
                if (fiscal != null)
                {
                    SessionHelper.FiscalYearId = fiscal.FiscalYearId;
                    SessionHelper.FiscalYearName = fiscal.FiscalYearName;
                }
            }
            if (string.IsNullOrEmpty(url))
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                return Redirect(url);
            }
        }

        public ActionResult SaveFiscalYearAjax(int? id)
        {
            bool result = false;
            if (id != null)
            {
                var fiscal = db.FiscalYears.Where(u => u.FiscalYearId == id).FirstOrDefault();
                if (fiscal != null)
                {
                    result = true;
                    SessionHelper.FiscalYearId = fiscal.FiscalYearId;
                    SessionHelper.FiscalYearName = fiscal.FiscalYearName;
                    SessionHelper.FiscalEndDate = fiscal.EndDate;
                    SessionHelper.FiscalStartDate = fiscal.StartDate;
                    SessionHelper.FiscalYear = fiscal.FiscalYearName;
                    SessionHelper.FiscalYearName = fiscal.FiscalYearName;
                }
            }
            List<Service.Helper> PartialList = new List<Service.Helper>();
            var list = db.FiscalYears
                .Where(u => u.BranchId == SessionHelper.BranchId)
                .OrderByDescending(u => u.FiscalYearId)
                .ToList();
            PartialList.Add(new Service.Helper { divToReplace = "divtoreplacefiscalyear", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_FiscalYear", list) });
            if (result)
            {
                PartialList.Add(new Service.Helper { divToReplace = "PopupModalSection", IdToShowModalPopup = "PopupModal", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialPopupDisplayMessage", "Fiscal year changed successfully") });
            }

            return Json(new { status = result, HideMsg = true, PartialList }, JsonRequestBehavior.AllowGet);
        }


        string CheckAppConstraints()
        {
            string msg = "";
            var fiscal = db.FiscalYears.Where(u => u.Active == true && u.BranchId == SessionHelper.BranchId).OrderByDescending(u => u.FiscalYearId).FirstOrDefault();
            if (fiscal == null)
            {
                msg = "* Fiscal Year not found<br>";
            }
            ////Shop warehouse
            //var _warehouse = db.Warehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
            //if (_warehouse == null)
            //{
            //    msg += "* Shop's warehouse not found<br>";
            //}
            //POS warehouse
            var _poswarehouse = db.InvWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
            if (_poswarehouse == null)
            {
                msg += "* POS's warehouse not found<br>";
            }
            ////Front Desk warehouse
            //var _fdwarehouse = db.ResWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
            //if (_fdwarehouse == null)
            //{
            //    msg += "* Front Desk's warehouse not found<br>";
            //}
            ////Res warehouse
            //var _reswarehouse = db.ResWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
            //if (_reswarehouse == null)
            //{
            //    msg += "* Restaurant's warehouse not found<br>";
            //}
            //HRPayroll Account setting
            var _AccountSettings = db.AccountSettings.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
            if (_AccountSettings == null)
            {
                msg += "* Human resource setting not found<br>";
            }
            else
            {
                msg += _AccountSettings.EmpParentAccountId == null ? "* Employee parent account not found<br>" : "";
                msg += _AccountSettings.HRBasicSalaryAccountId == null ? "* Basic salary account not found<br>" : "";
                msg += _AccountSettings.HRAllowancesParentAccountId == null ? "* Allowances parent account not found<br>" : "";
                msg += _AccountSettings.HRDeductionsParentAccountId == null ? "* Deductions parent account not found<br>" : "";
                msg += _AccountSettings.EmpAdvancePaymentParentAccountId == null ? "* Employee Advance payment parent account not found<br>" : "";
                msg += _AccountSettings.EmpSecurityParentAccountId == null ? "* Employee Security parent account not found<br>" : "";
                msg += _AccountSettings.EmpAdvancePaymentDefaultAccountId == null ? "* Employee Advance payment default account not found<br>" : "";
            }
            return msg;
        }

        public JsonResult DashboardGraphs(string id, string url, string area, string contoller, string actionnn)
        {
            GraphsModel ex = new GraphsModel();
            ex.Url = url;
            ex.Module = id;
            ex.Area = area;
            ex.Contoller = contoller;
            ex.Action = actionnn;
            ex = FetchGraphsData(ex);
            return Json(ex, JsonRequestBehavior.AllowGet);
        }

        private GraphsModel FetchGraphsData(GraphsModel ex)
        {
            if (!string.IsNullOrEmpty(ex.Url))
            {
                DateTime today = DateTime.Now.Date;
                var formrights = ProceduresModel.v_mnl_DashboardViews(db, SessionHelper.UserGroupId.Value, true, "Can Read", ex.Url).ToList();
                if (formrights == null)
                {
                    return ex;
                }

                ex.DashboardConstraint = ProceduresModel.CheckAppConstraints(db, ex.Module);
                ex.v_mnl_FormRights = formrights;

                ex._views = new List<Service.Helper>();

                if (formrights.Where(u => u.FormURL == "_PartialShowStats").Any())
                {
                    ex.logins = db.UserLogs.Where(u => u.UserId == SessionHelper.UserID && DbFunctions.TruncateTime(u.LoginTime) == today).Count();
                    ex.clients = db.Clients.Where(u => u.BranchId == SessionHelper.BranchId && u.IsClient).Count();
                    ex.suppliers = db.Clients.Where(u => u.BranchId == SessionHelper.BranchId && u.IsSupplier).Count();
                    ex.users = db.Users.Count();

                }
                if (formrights.Where(u => u.FormURL == "_PartialShowWeeklySale").Any())
                {
                    ex.WeeklySaleList = ProceduresModel.v_mnl_WeeklySaleDashboardGraph(db, SessionHelper.BranchId).Select(u => new v_mnl_TopTenProducts_Result() { ED = u.Day, MM = u.Day, Percentage = u.NetTotal }).ToList();
                }
                if (formrights.Where(u => u.FormURL == "_PartialMonthlyAttendanceGraph").Any())
                {
                    ex.MonthlyAttendanceList = ProceduresModel.v_mnl_WeeklySaleDashboardGraph(db, SessionHelper.BranchId).Select(u => new v_mnl_TopTenProducts_Result() { ED = u.Day, MM = u.Day, Percentage = u.NetTotal }).ToList();
                }
                if (formrights.Where(u => u.FormURL == "_PartialShowTransactions").Any())
                {
                    ex.TransactionList = new List<v_mnl_TopTenProducts_Result>(); /*HomeBLL.v_mnl_financedashboard_Graph(db, SessionHelper.FiscalYearId, SessionHelper.BranchId).Select(u => new v_mnl_TopTenProducts_Result() { ED = u.Type, MM = u.ProductName, Percentage = u.NetTotal }).ToList();*/
                }
                if (formrights.Where(u => u.FormURL == "_PartialEmployeeCheckins").Any())
                {
                    // ex.v_mnl_EmployeeAttendance = ProceduresModel.v_mnl_EmployeeAttendanceList(db).ToList();

                    ex._views.Add(new Service.Helper { divToReplace = "EmployeeCheckins", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialEmployeeCheckins", ex) });
                }
                if (formrights.Where(u => u.FormURL == "_PartialUpcomingEvents").Any())
                {
                    ex.v_mnl_UpcomingEvent = ProceduresModel.v_mnl_UpcomingEventList(db, SessionHelper.BranchId).ToList();
                    ex._views.Add(new Service.Helper { divToReplace = "UpcomingEvents", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialUpcomingEvents", ex) });
                }

                ex.Deparments = db.Departments.Count();
                ex.Designations = db.Designations.Count();

                ex.Males = db.Employees.Count(m => m.BranchId == _BranchId && m.Gender == true);
                ex.Females = db.Employees.Count(m => m.BranchId == _BranchId && m.Gender == false);

                //  ex.DepartmentWiseEmployeeGraph = ProceduresModel.DepartmentWiseEmployeeGraph(db, SessionHelper.BranchId);
                //  ex.EmployeeGendersGraph = ProceduresModel.EmployeeGenderPieChart(db, SessionHelper.BranchId);
                //  ex.StudentGendersGraph = HomeBLL.StudentGenderPieChart(db);

                if (formrights.Where(u => u.FormURL == "_PartialStudentStrengthGraph").Any())
                {
                    //   ex.StudentStrengthGraphDataList = HomeBLL.StudentStrength(db);//Academic Dashboard BLL
                }
                if (formrights.Where(u => u.FormURL == "_PartialStudentAdmissionWithdrawl").Any())
                {
                    //   ex.StudentAdmissionWithDrawlList = HomeBLL.studentAdmissionWithdrawl(db);//Academic Dashboard BLL
                }
                //     ex.DailyAttendanceGraph = ProceduresModel.DailyAttendanceResults(db);//needs fixing

                ex._views.Add(new Service.Helper
                {
                    divToReplace = "replace",
                    partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialDashboardOrdering", ex)
                });
                ex._views.Add(new Service.Helper
                {
                    divToReplace = "toprightsection",
                    partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialShowTopIcons", ex)
                });
            }

            return ex;
        }

        public ActionResult UpdateMenus()
        {
            var formMenus = new PrimaryData.MenuLinks(db);
            formMenus.AddUpdateAllFormMenus();
            var userGroupId = db.UserGroups.Where(s => s.UserGroupName == "Admin").Select(s => s.UserGroupId).FirstOrDefault();
            if (userGroupId > 0)
            {
                //ADDING RIGHTS TO ADDED FORMS 
                formMenus.AddGroupRights(userGroupId);
                formMenus.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public JsonResult InsertMembershipForms(Form ex)
        {
            int result = ProceduresModel.InsertInMembershipForms(db, ex, SessionHelper.BranchId);
            if (result > 0)
            {
                Messages = "Form Added Successfully";
            }
            return Json(new { Messages }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InsertMembershipFormsWithStaticFormId(Form ex)
        {
            int result = ProceduresModel.InsertInMembershipFormsWithStaticFormId(db, ex, SessionHelper.BranchId);
            if (result > 0)
            {
                Messages = "Form Added Successfully";
            }
            return Json(new { Messages }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateMembershipForms(Form ex)
        {
            int result = ProceduresModel.InsertInMembershipForms(db, ex, SessionHelper.BranchId);
            if (result > 0)
            {
                Messages = "Form Added Successfully";
            }
            return Json(new { Messages }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PageHistory(string PageUrl)
        {
            HomeVM ex = new HomeVM();
            if (!string.IsNullOrEmpty(PageUrl))
            {
                ex.Audits = db.Audits.Include(u => u.UserLog).Where(w => w.Url == PageUrl).OrderByDescending(s => s.AuditDate).Take(50).ToList();
                if (ex.Audits == null)
                {
                    ex.Audits = new List<Audit>();
                }
            }
            else
            {
                ex.Audits = new List<Audit>();
            }
            var res = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialPageHistoryModal", ex),
                TargetId = "frmModalContentLg",
                ModalId = "frmModalLg",
            };

            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateMenu()
        {
            string Error = string.Empty;
            string Message = string.Empty;
            AMFormsBLL.InsertAMForms(db, 1603, ref Error, ref Message);
            
            if (!string.IsNullOrEmpty(Error))
            {
                ViewBag.Error = Error;
            }
            if (!string.IsNullOrEmpty(Message))
            {
                ViewBag.Message = Message;
            }

            return View();
        }
    }
}
