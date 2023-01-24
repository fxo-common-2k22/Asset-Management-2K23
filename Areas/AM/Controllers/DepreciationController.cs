using FAPP.AM.Models;
using FAPP.Areas.AM.ViewModels;
using FAPP.Controllers;
using FAPP.DAL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAPP.Areas.AM.Controllers
{
    public class DepreciationController : BaseController
    {
        private int branch_ID = SessionHelper.BranchId;
        private int fiscalYearId = SessionHelper.FiscalYearId;
        private int CreatedBy = SessionHelper.UserID;
        public int pageNo = 1;
        // GET: AM/Depreciation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage(int? page)
        {
            var vm = new DepreciationMainViewModel();
            DateTime now = DateTime.Now;
            vm.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            vm.ToDate = now.ToShortDateString();
            GetItemRegister(vm, page);
            return View(vm);
        }
        [HttpPost]
        public PartialViewResult Manage(DepreciationMainViewModel vm, int? page)
        {
            page = page ?? 1;
            pageNo = page.Value;
            GetItemRegister(vm, page);
            ModelState.Clear();
            return PartialView("_PartialDepreciations", vm);
        }
        public ActionResult DepreciationDetails(int? id, int? page)
        {
            var vm = new DepreciationMainViewModel();
            DateTime now = DateTime.Now;
            vm.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            vm.ToDate = now.ToShortDateString();
            if(id != null)
            {
                vm.DepreciationMainId = id;
            }
            else
            {
                return RedirectToAction("Manage");
            }
            GetDepreciationDetails(vm, page);
            return View(vm);
        }
        [HttpPost]
        public ActionResult DepreciationDetails(DepreciationMainViewModel vm, int? page)
        {
            page = page ?? 1;
            pageNo = page.Value;
            GetDepreciationDetails(vm, page);
            ModelState.Clear();
            return PartialView("_PartialDepreciationsDetails", vm);
        }
        private void GetItemRegister(DepreciationMainViewModel vm, int? page)
        {
            FillDDs_Depreciation();
            IQueryable<DepreciationMain> queryable = db.Depreciations.Where(x => x.BranchId == branch_ID);

            if (!string.IsNullOrEmpty(vm.FromDate) && !string.IsNullOrEmpty(vm.ToDate))
            {
                var fdate = Convert.ToDateTime(vm.FromDate);
                var tdate = Convert.ToDateTime(vm.ToDate);
                queryable = queryable.Where(u => u.CreatedOn >= fdate && u.CreatedOn <= tdate);
            }
            else
            {
                if (!string.IsNullOrEmpty(vm.FromDate))
                {
                    var date = Convert.ToDateTime(vm.FromDate);
                    queryable = queryable.Where(u => u.CreatedOn >= date);
                }
                if (!string.IsNullOrEmpty(vm.ToDate))
                {
                    var date = Convert.ToDateTime(vm.ToDate);
                    queryable = queryable.Where(u => u.CreatedOn <= date);
                }
            }
            if (vm.DepreciationTypeId > 0)
                queryable = queryable.Where(u => u.DepreciationTypeId == vm.DepreciationTypeId);
            vm.Depreciations = queryable.OrderBy(l => l.CreatedOn).ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, vm.pagesize);
        }
        private void GetDepreciationDetails(DepreciationMainViewModel vm, int? page)
        {
            FillDDs_Depreciation();

            IQueryable<DepreciationDetail> queryable = db.DepreciationDetails.Include("ItemRegister").Where(x => x.BranchId == branch_ID);
            if(vm.DepreciationMainId != null && vm.DepreciationMainId > 0)
            {
                queryable = queryable.Where(u => u.DepreciationMainId == vm.DepreciationMainId);
            }
            if (!string.IsNullOrEmpty(vm.FromDate) && !string.IsNullOrEmpty(vm.ToDate))
            {
                var fdate = Convert.ToDateTime(vm.FromDate);
                var tdate = Convert.ToDateTime(vm.ToDate);
                queryable = queryable.Where(u => u.CreatedOn >= fdate && u.CreatedOn <= tdate);
            }
            else
            {
                if (!string.IsNullOrEmpty(vm.FromDate))
                {
                    var date = Convert.ToDateTime(vm.FromDate);
                    queryable = queryable.Where(u => u.CreatedOn >= date);
                }
                if (!string.IsNullOrEmpty(vm.ToDate))
                {
                    var date = Convert.ToDateTime(vm.ToDate);
                    queryable = queryable.Where(u => u.CreatedOn <= date);
                }
            }
            vm.DepreciationDetails = queryable.OrderBy(l => l.CreatedOn).ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, vm.pagesize);
        }

        private void FillDDs_Depreciation()
        {
            ViewBag.DepartmentsDD = from dept in db.Departments
                                    where dept.BranchId == SessionHelper.BranchId
                                    select new SelectListItem
                                    {
                                        Text = dept.DepartmentName,
                                        Value = dept.DepartmentId.ToString(),
                                    };
            ViewBag.Categories = db.InvCategories.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true && u.ParentCategory.CategoryName.ToLower().Contains("fixed")).Select(s => new SelectListItem
            {
                Value = s.CategoryId.ToString(),
                Text = s.CategoryName.ToString()
            }).ToList();
            ViewBag.ItemsDD = db.InvProducts.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true && u.IsFixedAsset == true).Select(s => new SelectListItem
            {
                Value = s.ProductId.ToString(),
                Text = s.ProductName.ToString()
            }).ToList();
            ViewBag.EmployeesDD = from emp in db.Employees
                                  where emp.BranchId == branch_ID && emp.Active == true
                                  select new SelectListItem
                                  {
                                      Text = emp.EmpName,
                                      Value = emp.EmployeeId.ToString()
                                  };
            ViewBag.DepreciationTypes = from depreciationType in db.AMDepreciationTypes
                                  select new SelectListItem
                                  {
                                      Text = depreciationType.DepreciationTypeName,
                                      Value = depreciationType.DepreciationTypeId.ToString()
                                  };
            ViewBag.ItemCodes = from codes in db.AMPurchaseInvoiceProductDetails
                                select new TextValueId
                                {
                                    Id = codes.ItemId,
                                    Text = codes.ItemCode,
                                    Value = codes.DetailId.ToString()
                                };
            ViewBag.RoomsDD = from dept in db.CompanyRooms
                              where dept.BranchId == branch_ID
                              select new SelectListItem
                              {
                                  Text = dept.Floors.Buildings.BuildingName + ">" + dept.Floors.FloorName + ">" + dept.RoomDoorNo,
                                  Value = dept.RoomId.ToString()
                              };
            ViewBag.ConditionTypesDD = from cond in db.AMConditionTypes
                                       select new SelectListItem
                                       {
                                           Text = cond.Name,
                                           Value = cond.ConditionTypeId.ToString()
                                       };
            List<SelectListItem> ii = new List<SelectListItem>();
            ii.Add(new SelectListItem() { Text = "InStock", Value = "InStock" });
            ii.Add(new SelectListItem() { Text = "Issued", Value = "Issued" });
            ii.Add(new SelectListItem() { Text = "Returned", Value = "Returned" });
            ViewBag.Statuses = ii;

            List<SelectListItem> type = new List<SelectListItem>();
            type.Add(new SelectListItem() { Text = "Fixed", Value = "0" });
            type.Add(new SelectListItem() { Text = "Consumable", Value = "1" });

            ViewBag.Types = type;
        }

    }
}