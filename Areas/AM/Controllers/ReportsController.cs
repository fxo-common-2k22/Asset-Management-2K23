using FAPP.Areas.AM.BLL;
using FAPP.Areas.AM.ViewModels;
using FAPP.Controllers;
using FAPP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAPP.Areas.AM.Controllers
{
    public class ReportsController : BaseController
    {
        short branch_ID = Convert.ToInt16(SessionHelper.BranchId);
        string Error = string.Empty;
        // GET: AM/Reports
        public ActionResult Index()
        {
            GraphsModel graphsModel = new GraphsModel();
            graphsModel.Action = ControllerContext.RouteData.Values["action"].ToString();
            graphsModel.Contoller = ControllerContext.RouteData.Values["Controller"].ToString();
            graphsModel.Area = ControllerContext.RouteData.DataTokens["area"].ToString();
            graphsModel.Module = ControllerContext.RouteData.DataTokens["area"].ToString();
            return View(graphsModel);
        }

        public ActionResult Issuance()
        {
            fillDD();
            ReportModel ex = new ReportModel();
            DateTime now = DateTime.Now;
            ex.FromDateTime = now.AddDays(-7);
            ex.ToDateTime = new DateTime(now.Year, now.Month, now.Day);
            ex.StatusId = 2;
            ex.IssuanceList = AMReportBll.GetItemRegisterData(db, ex, branch_ID);
            return View(ex);
        }


        [HttpPost]
        public PartialViewResult Issuance(ReportModel ex)
        {
            fillDD();
            ex.IssuanceList = AMReportBll.GetItemRegisterData(db, ex, branch_ID);
            return PartialView("_Issuance", ex);
        }

        public ActionResult Damages()
        {
            fillDD();
            ReportModel ex = new ReportModel();
            DateTime now = DateTime.Now;
            ex.FromDateTime = now.AddDays(-7);
            ex.ToDateTime = new DateTime(now.Year, now.Month, now.Day);
            ex.StatusId = 3;
            ex.IssuanceList = AMReportBll.GetItemRegisterData(db, ex, branch_ID);
            return View(ex);
        }


        [HttpPost]
        public PartialViewResult Damages(ReportModel ex)
        {
            fillDD();
            ex.IssuanceList = AMReportBll.GetItemRegisterData(db, ex, branch_ID);
            return PartialView("_Issuance", ex);
        }

        public ActionResult AssetLog()
        {
            fillDD();
            ReportModel ex = new ReportModel();
            DateTime now = DateTime.Now;
            ex.FromDateTime = now.AddDays(-7);
            //  now = now.AddDays(1);
            ex.ToDateTime = new DateTime(now.Year, now.Month, now.Day);
            ex.TransferHistory = AMReportBll.GetAssetLog(db, ex, out Error);
            return View(ex);
        }


        [HttpPost]
        public PartialViewResult AssetLog(ReportModel ex)
        {
            fillDD();
            ex.TransferHistory = AMReportBll.GetAssetLog(db, ex, out Error);
            return PartialView("_AssetLog", ex);
        }

        public ActionResult Summary()
        {
            fillDD();
            ReportModel ex = new ReportModel();
            DateTime now = DateTime.Now;
            ex.FromDateTime = now.AddDays(-7);
            ex.ToDateTime = new DateTime(now.Year, now.Month, now.Day);
            ex.SummarizeReport = AMReportBll.GetSummarizeData(db, ex, branch_ID);
            return View(ex);
        }


        [HttpPost]
        public PartialViewResult Summary(ReportModel ex)
        {
            fillDD();
            ex.SummarizeReport = AMReportBll.GetSummarizeData(db, ex, branch_ID);
            return PartialView("_Summary", ex);
        }
        [NonAction]
        public void fillDD()
        {


            List<SelectListItem> ii = new List<SelectListItem>();
            ii.Add(new SelectListItem() { Text = "Available", Value = "1" });
            ii.Add(new SelectListItem() { Text = "Issued", Value = "2" });
            ii.Add(new SelectListItem() { Text = "Damage", Value = "3" });
            ViewBag.Statuses = ii;
            ViewBag.Categories = db.InvCategories.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true && u.ParentCategory.CategoryName.ToLower().Contains("fixed")).Select(s => new SelectListItem
            {
                Value = s.CategoryId.ToString(),
                Text = s.CategoryName.ToString()
            }).ToList();
            ViewBag.Products = db.InvProducts.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true && u.IsFixedAsset == true).Select(s => new SelectListItem
            {
                Value = s.ProductId.ToString(),
                Text = s.ProductName.ToString()
            }).ToList();

            ViewBag.Departments = db.Departments.Where(u => u.BranchId == branch_ID).Select(s => new SelectListItem
            {
                Value = s.DepartmentId.ToString(),
                Text = s.DepartmentName.ToString()
            }).ToList();

            ViewBag.Employees = db.Employees.Where(u => u.BranchId == branch_ID).Select(s => new SelectListItem
            {
                Value = s.EmployeeId.ToString(),
                Text = s.EmpName.ToString()
            }).ToList();

            ViewBag.Locations = db.CompanyRooms.Where(u => u.BranchId == branch_ID).Select(s => new SelectListItem
            {
                Value = s.RoomId.ToString(),
                Text = s.RoomDoorNo.ToString()
            }).ToList();
        }
    }
}