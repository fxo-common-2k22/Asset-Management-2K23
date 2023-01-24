using FAPP.AM.Models;
using FAPP.Areas.AM.BLL;
using FAPP.Areas.AM.PrimaryData;
using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
namespace FAPP.Areas.AM.Controllers
{
    public class IssueController : FAPP.Controllers.BaseController
    {
        //private OneDbContext db = new OneDbContext();
        private int branch_ID = SessionHelper.BranchId;
        private int fiscalYearId = SessionHelper.FiscalYearId;
        private int CreatedBy = SessionHelper.UserID;
        public int pageNo = 1;
        // GET: AM/Issue
        public ActionResult Index()
        {
            return View();
        }

        #region AddEditIssuedItem
        [HttpGet]
        public ActionResult ManageIssuedItems()
        {

            var vm = new ManageIssuedItemsViewModel();
            vm.IssuedItems = db.AMIssuedItems.Include("Department").Include("UserCreated");
            vm.DepartmentsDD = from dept in db.Departments
                               where dept.BranchId == SessionHelper.BranchId
                               select new SelectListItem
                               {
                                   Text = dept.DepartmentName,
                                   Value = dept.DepartmentId.ToString()
                               };
            vm.IssueDate = null;
            vm.IssuedItemId = null;
            return View(vm);
        }

        [HttpPost]
        public ActionResult ManageIssuedItems(ManageIssuedItemsViewModel vm)
        {
            var issuedItems = from issItem in db.AMIssuedItems
                              select issItem;
            if (vm.IssuedItemId != null)
            {
                issuedItems = issuedItems.Where(s => s.IssuedItemId == vm.IssuedItemId);
            }
            if (vm.IssueDate != null)
            {
                issuedItems = issuedItems.Where(s => s.IssueDate.Month == vm.IssueDate.Value.Month && s.IssueDate.Year == vm.IssueDate.Value.Year && s.IssueDate.Day == vm.IssueDate.Value.Day);
            }
            if (vm.DepartmentId != null)
            {
                issuedItems = issuedItems.Where(s => s.DepartmentId == vm.DepartmentId);
            }

            vm.IssuedItems = issuedItems;
            return PartialView("_PartialManageIssuedItem", vm);
        }
        #endregion

        #region AddEditIssuedItem
        [HttpGet]
        public ActionResult AddEditIssuedItem(int? id)
        {
            var vm = GetAddEditIssuedItem(id);
            return View(vm);
        }

        private AddIssuedItemViewModel GetAddEditIssuedItem(int? id)
        {
            var vm = new AddIssuedItemViewModel();
            var issueItem = db.TransferHistory.Find(id);
            if (issueItem != null)
            {
                vm.TransferHistoryId = issueItem.TransferHistoryId;
                vm.DepartmentId = issueItem.DepartmentId;
                vm.IssueDate = issueItem.Date;
                vm.Description = issueItem.Description;
                vm.Details = (from detail in db.TransferHistory
                              where detail.TransferHistoryId == issueItem.TransferHistoryId
                              select new IssuedItemsDetailViewModel
                              {
                                  Description = detail.Description,
                                  EmployeeId = detail.EmployeeId, // detail.EmployeeId != null ? (int)detail.EmployeeId : 0,
                                  ItemRegisterId = detail.ItemRegisterId,
                                  ItemCode = (from PIPD in db.ItemRegister
                                              where PIPD.ItemRegisterId == detail.ItemRegisterId
                                              select PIPD.ItemCode).FirstOrDefault(),
                                  ProductId = (from PIPD in db.ItemRegister
                                               where PIPD.ItemRegisterId == detail.ItemRegisterId
                                               select PIPD.ProductId).FirstOrDefault(),
                                  Quantity = 1,
                                  RoomId = (int?)detail.LocationId,
                                  DepartmentId = (int?)detail.DepartmentId,
                                  IssueDate = detail.Date,
                                  TransferHistoryId = detail.TransferHistoryId,
                                  ConditionTypeId = detail.ConditionTypeId,
                              }).ToList();
                var dds = FillDDs();
                vm.DepartmentsDD = dds.DepartmentsDD;
                vm.EmployeesDD = dds.EmployeesDD;
                vm.RoomsDD = dds.RoomsDD;
                vm.ItemsDD = dds.ItemsDD;
                vm.ItemCodes = dds.ItemCodes;
                vm.ConditionTypesDD = dds.ConditionTypesDD;
                vm.IssuedItems = db.TransferHistory.Find(vm.TransferHistoryId);
                // vm.IssuedItemDetails = db.AMIssuedItemDetails.Where(u => u.IssuedItemId == vm.IssuedItemId).ToList();

            }
            else
            {
                vm = FillDDs();
                vm.TransferHistoryId = 0;
                vm.IssueDate = DateTime.Now.Date;
            }
            return vm;
        }
        // Commented By Zakki
        //public ActionResult SaveIssuedItem(AddIssuedItemViewModel vm)
        //{
        //    var issueItem = db.AMIssuedItems.Find(vm.IssuedItemId);
        //    if (issueItem != null)
        //    {
        //        return UpdateIssuedItem(vm, issueItem);
        //    }
        //    else
        //    {
        //        return InsertIssuedItems(vm);
        //    }
        //}

        //private ActionResult UpdateIssuedItem(AddIssuedItemViewModel vm, AMIssuedItem issueItem)
        //{
        //    bool result = false;
        //    string msg = "Updated successfully";
        //    using (var trans = db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            issueItem.IssueDate = vm.IssueDate;
        //            issueItem.DepartmentId = vm.DepartmentId;
        //            issueItem.Description = vm.Description;
        //            issueItem.ModifiedOn = DateTime.Now;
        //            issueItem.ModifiedBy = CreatedBy;
        //            issueItem.BranchId = (short)branch_ID;
        //            db.SaveChanges();

        //            for (int i = 0; i < vm.Details.Count(); i++)
        //            {
        //                var item = vm.Details[i];
        //                if (item.ItemId > 0 && item.Quantity > 0)
        //                {
        //                    var issueDetail = db.AMIssuedItemDetails.Find(item.IssuedItemDetailId);
        //                    if (issueDetail != null)
        //                    {
        //                        issueDetail.BranchId = (short)branch_ID;
        //                        issueDetail.ModifiedBy = CreatedBy;
        //                        issueDetail.ModifiedOn = DateTime.Now;
        //                        issueDetail.EmployeeId = item.EmployeeId;
        //                        issueDetail.IssuedItemId = issueItem.IssuedItemId;
        //                        if (item.ItemCode != null)
        //                        {
        //                            issueDetail.PIPDetailId = db.AMPurchaseInvoiceProductDetails.FirstOrDefault(s => s.DetailId == item.ItemCode).DetailId;
        //                        }
        //                        issueDetail.ConditionTypeId = item.ConditionTypeId;
        //                        issueDetail.ItemId = item.ItemId;
        //                        issueDetail.Quantity = item.Quantity;
        //                        issueDetail.RoomId = item.RoomId;
        //                        issueDetail.Description = item.Description;
        //                    }
        //                    else
        //                    {
        //                        var detail = new AMIssuedItemDetail();
        //                        detail.BranchId = (short)branch_ID;
        //                        detail.CreateBy = CreatedBy;
        //                        detail.CreatedOn = DateTime.Now;
        //                        detail.EmployeeId = item.EmployeeId;
        //                        detail.IssuedItemId = issueItem.IssuedItemId;
        //                        if (item.ItemCode != null)
        //                        {
        //                            detail.PIPDetailId = db.AMPurchaseInvoiceProductDetails.FirstOrDefault(s => s.DetailId == item.ItemCode).DetailId;
        //                        }
        //                        detail.ConditionTypeId = item.ConditionTypeId;
        //                        detail.ItemId = item.ItemId;
        //                        detail.Quantity = item.Quantity;
        //                        detail.RoomId = item.RoomId;
        //                        db.AMIssuedItemDetails.Add(detail);
        //                    }
        //                }
        //            }

        //            db.SaveChanges();
        //            trans.Commit();
        //            result = true;
        //        }
        //        catch (Exception exc)
        //        {

        //            msg = "Updation failed, " + ExtensionMethods.GetExceptionMessages(exc);
        //            throw;
        //        }
        //    }
        //    vm.Details = (from detail in db.AMIssuedItemDetails
        //                  where detail.IssuedItemId == issueItem.IssuedItemId
        //                  select new IssuedItemsDetailViewModel
        //                  {
        //                      Description = detail.Description,
        //                      EmployeeId = detail.EmployeeId != null ? (int)detail.EmployeeId : 0,
        //                      IssuedItemDetailId = detail.IssuedItemDetailId,
        //                      ItemCode = (from PIPD in db.AMPurchaseInvoiceProductDetails
        //                                  where PIPD.DetailId == detail.PIPDetailId
        //                                  select PIPD.DetailId).FirstOrDefault(),
        //                      ItemId = detail.ItemId,
        //                      Quantity = detail.Quantity,
        //                      RoomId = detail.RoomId,
        //                      ConditionTypeId = detail.ConditionTypeId,
        //                  }).ToList();
        //    var dds = FillDDs();
        //    vm.DepartmentsDD = dds.DepartmentsDD;
        //    vm.EmployeesDD = dds.EmployeesDD;
        //    vm.RoomsDD = dds.RoomsDD;
        //    vm.ItemsDD = dds.ItemsDD;
        //    vm.ItemCodes = dds.ItemCodes;
        //    vm.IssuedItemId = issueItem.IssuedItemId;
        //    vm.ConditionTypesDD = dds.ConditionTypesDD;
        //    //return PartialView("_PartialIssueitem", vm);

        //    vm.IssuedItems = db.AMIssuedItems.Find(vm.IssuedItemId);
        //    vm.IssuedItemDetails = db.AMIssuedItemDetails.Where(u => u.IssuedItemId == vm.IssuedItemId).ToList();

        //    List<Service.Helper> PartialList = new List<Service.Helper>();
        //    PartialList.Add(new Service.Helper { divToReplace = "mainsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialIssueitem", vm) });
        //    PartialList.Add(new Service.Helper { divToReplace = "printsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialIssueItemPrintSection", vm) });
        //    return Json(new { status = result, msg = msg, PartialList }, JsonRequestBehavior.AllowGet);
        //}

        //private ActionResult InsertIssuedItems(AddIssuedItemViewModel vm)
        //{
        //    bool result = false;
        //    string msg = "Saved successfully";
        //    AMIssuedItem issueItem = new AMIssuedItem();
        //    using (var trans = db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            issueItem.IssueDate = vm.IssueDate;
        //            issueItem.DepartmentId = vm.DepartmentId;
        //            issueItem.Description = vm.Description;
        //            issueItem.CreatedOn = DateTime.Now;
        //            issueItem.CreatedBy = CreatedBy;
        //            issueItem.BranchId = (short)branch_ID;
        //            db.AMIssuedItems.Add(issueItem);
        //            db.SaveChanges();
        //            for (int i = 0; i < vm.Details.Count(); i++)
        //            {
        //                var item = vm.Details[i];
        //                if (item.ItemId > 0 && item.Quantity > 0)
        //                {
        //                    var detail = new AMIssuedItemDetail();
        //                    detail.BranchId = (short)branch_ID;
        //                    detail.CreateBy = CreatedBy;
        //                    detail.CreatedOn = DateTime.Now;
        //                    detail.EmployeeId = item.EmployeeId;
        //                    detail.IssuedItemId = issueItem.IssuedItemId;
        //                    if (item.ItemCode != null)
        //                    {
        //                        detail.PIPDetailId = db.AMPurchaseInvoiceProductDetails.FirstOrDefault(s => s.DetailId == item.ItemCode).DetailId;
        //                    }
        //                    detail.ItemId = item.ItemId;
        //                    detail.ConditionTypeId = item.ConditionTypeId;
        //                    detail.Quantity = item.Quantity;
        //                    detail.RoomId = item.RoomId;
        //                    detail.Description = item.Description;
        //                    db.AMIssuedItemDetails.Add(detail);
        //                }
        //                db.SaveChanges();
        //                result = true;
        //            }
        //            trans.Commit();
        //        }
        //        catch (Exception exc)
        //        {

        //            msg = "Saving failed, " + ExtensionMethods.GetExceptionMessages(exc);
        //            throw;
        //        }
        //    }
        //    vm.Details = (from detail in db.AMIssuedItemDetails
        //                  where detail.IssuedItemId == issueItem.IssuedItemId
        //                  select new IssuedItemsDetailViewModel
        //                  {
        //                      Description = detail.Description,
        //                      EmployeeId = detail.EmployeeId != null ? (int)detail.EmployeeId : 0,
        //                      IssuedItemDetailId = detail.IssuedItemDetailId,
        //                      ItemCode = (from PIPD in db.AMPurchaseInvoiceProductDetails
        //                                  where PIPD.DetailId == detail.PIPDetailId
        //                                  select PIPD.DetailId).FirstOrDefault(),
        //                      ItemId = detail.ItemId,
        //                      Quantity = detail.Quantity,
        //                      RoomId = detail.RoomId,
        //                      ConditionTypeId = detail.ConditionTypeId,
        //                  }).ToList();
        //    var dds = FillDDs();
        //    vm.DepartmentsDD = dds.DepartmentsDD;
        //    vm.EmployeesDD = dds.EmployeesDD;
        //    vm.RoomsDD = dds.RoomsDD;
        //    vm.ItemsDD = dds.ItemsDD;
        //    vm.ItemCodes = dds.ItemCodes;
        //    vm.ConditionTypesDD = dds.ConditionTypesDD;
        //    vm.IssuedItemId = issueItem.IssuedItemId;
        //    vm.IssuedItems = db.AMIssuedItems.Find(vm.IssuedItemId);
        //    vm.IssuedItemDetails = db.AMIssuedItemDetails.Where(u => u.IssuedItemId == vm.IssuedItemId).ToList();

        //    //return PartialView("_PartialIssueitem", vm);
        //    List<Service.Helper> PartialList = new List<Service.Helper>();
        //    PartialList.Add(new Service.Helper { divToReplace = "mainsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialIssueitem", vm) });
        //    PartialList.Add(new Service.Helper { divToReplace = "printsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialIssueItemPrintSection", vm) });
        //    return Json(new { status = result, msg = msg, PartialList }, JsonRequestBehavior.AllowGet);
        //}

        private AddIssuedItemViewModel FillDDs()
        {
            var vm = new AddIssuedItemViewModel();
            vm.DepartmentsDD = from dept in db.Departments
                               where dept.BranchId == SessionHelper.BranchId
                               select new SelectListItem
                               {
                                   Text = dept.DepartmentName,
                                   Value = dept.DepartmentId.ToString(),
                               };
            //vm.ItemsDD = from item in db.AMItems
            //             where item.BranchId == branch_ID
            //             select new SelectListItem
            //             {
            //                 Value = item.ItemId.ToString(),
            //                 Text = item.ItemName
            //             };
            ViewBag.ItemsDD = db.InvProducts.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true && u.IsFixedAsset == true).Select(s => new SelectListItem
            {
                Value = s.ProductId.ToString(),
                Text = s.ProductName.ToString()
            }).ToList();
            vm.EmployeesDD = from emp in db.Employees
                             where emp.BranchId == branch_ID && emp.Active == true
                             select new SelectListItem
                             {
                                 Text = emp.EmpName,
                                 Value = emp.EmployeeId.ToString()
                             };
            //vm.ItemCodes = from codes in db.AMPurchaseInvoiceProductDetails
            //               select new TextValueId
            //               {
            //                   Id = codes.ItemId,
            //                   Text = codes.ItemCode,
            //                   Value = codes.DetailId.ToString()
            //               };
            //List<SelectListItem> roomsList = new List<SelectListItem>();
            //roomsList.Add(new SelectListItem() { Text = "1", Value = "1" });
            //roomsList.Add(new SelectListItem() { Text = "2", Value = "2" });
            //roomsList.Add(new SelectListItem() { Text = "3", Value = "3" });
            //vm.RoomsDD = roomsList;
            vm.RoomsDD = from dept in db.CompanyRooms
                         where dept.BranchId == branch_ID
                         select new SelectListItem
                         {
                             Text = dept.Floors.Buildings.BuildingName + ">" + dept.Floors.FloorName + ">" + dept.RoomDoorNo,
                             Value = dept.RoomId.ToString()
                         };
            vm.ConditionTypesDD = from cond in db.AMConditionTypes
                                  select new SelectListItem
                                  {
                                      Text = cond.Name,
                                      Value = cond.ConditionTypeId.ToString()
                                  };

            return vm;
        }

        public JsonResult GetItemCodesById(int id)
        {
            //List<SelectListItem> list = new List<SelectListItem>();
            //var itemCodes = from detail in db.AMPurchaseInvoiceProductDetails
            //                where detail.ItemId == id
            //                select new
            //                {
            //                    Text = detail.ItemCode,
            //                    Value = detail.DetailId
            //                };
            //var codes = new SelectList(itemCodes, "Value", "Text");
            //var IsConsumable = db.AMItems.Where(u=>u.ItemId == id).Select(u=>u.IsConsumable).FirstOrDefault();
            //return Json(new { codes, IsConsumable }, JsonRequestBehavior.AllowGet);
            return ItemCodesById(id);
        }

        JsonResult ItemCodesById(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            //var itemCodes = ProceduresModel.v_mnl_AMPurchaseItemCodeList(db).Where(u => u.ItemId == id).Select(u => new
            //{
            //    Qty = u.Quantity,
            //    Text = u.ItemCode,
            //    Value = u.DetailId,
            //    IsConsumable = u.IsConsumable
            //}).ToList();
            // Status 1 for Available 2 Issued 3 for Damaged
            var itemCodes = ProceduresModel.GetItemCodes(db, id, 1, 1, branch_ID).Select(u => new
            {
                Qty = u.Quantity,
                Text = u.ItemCode,
                Value = u.ItemRegisterId,
                IsConsumable = 1
            }).ToList();
            var codes = itemCodes;// new SelectList(itemCodes, "Value", "Text");
            var IsConsumable = itemCodes.Select(u => u.IsConsumable).FirstOrDefault();
            return Json(new { codes, IsConsumable, ItemId = id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetItemByBarcode(string id)
        {
            var item = db.AMItems.Where(u => u.ProductCode == id).FirstOrDefault();
            if (item != null)
            {
                //var result = new { ItemId = item.ItemId };
                //return Json(result, JsonRequestBehavior.AllowGet);
                return ItemCodesById(item.ItemId);
            }
            else return null;
        }

        [HttpPost]
        public async Task<JsonResult> GetItemByitemCode(string id)
        {
            var _item = await db.AMItems.Where(u => u.ProductCode == id || u.Barcode == id).FirstOrDefaultAsync();
            if (_item != null)
            {
                var item = await db.AMPurchaseInvoiceProductDetails.FirstOrDefaultAsync(s => s.ItemCode == id);
                if (item != null)
                {
                    var itemCodes = from detail in db.AMPurchaseInvoiceProductDetails
                                    where detail.ItemId == item.ItemId
                                    select new
                                    {
                                        Text = detail.ItemCode,
                                        Value = detail.DetailId
                                    };
                    var result = new
                    {
                        ItemId = item.ItemId,
                        ItemCode = item.ItemCode,
                        DetailId = item.DetailId,
                        ItemCodes = new SelectList(itemCodes, "Value", "Text", item.ItemId)
                    };
                    return Json(new { result }, JsonRequestBehavior.AllowGet);
                }
                else return null;
            }
            else return null;
        }

        public async Task<ActionResult> PostIssuanceVoucher(int? id)
        {
            var result = false;
            string msg = "";
            var vm = new AddIssuedItemViewModel();
            List<Service.Helper> PartialList = new List<Service.Helper>();
            var issueItem = db.AMIssuedItems.Find(id);
            if (issueItem != null)
            {
                issueItem.IsPosted = true;
                await db.SaveChangesAsync();
                result = true;
                msg = "Voucher posted successfully";
                vm = GetAddEditIssuedItem(id);
                PartialList.Add(new Service.Helper { divToReplace = "headersection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialIssueHeaderSection", vm) });
                PartialList.Add(new Service.Helper { divToReplace = "mainsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialIssueitem", vm) });
                PartialList.Add(new Service.Helper { divToReplace = "printsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialIssueItemPrintSection", vm) });
            }
            return Json(new { status = result, msg = msg, PartialList }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UnPostIssuanceVoucher(int? id)
        {
            var result = false;
            string msg = "";
            var vm = new AddIssuedItemViewModel();
            List<Service.Helper> PartialList = new List<Service.Helper>();
            var issueItem = db.AMIssuedItems.Find(id);
            if (issueItem != null)
            {
                issueItem.IsPosted = false;
                await db.SaveChangesAsync();
                result = true;
                msg = "Voucher unposted successfully";

                vm = GetAddEditIssuedItem(id);
                PartialList.Add(new Service.Helper { divToReplace = "headersection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialIssueHeaderSection", vm) });
                PartialList.Add(new Service.Helper { divToReplace = "mainsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialIssueitem", vm) });
                PartialList.Add(new Service.Helper { divToReplace = "printsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialIssueItemPrintSection", vm) });
            }
            return Json(new { status = result, msg = msg, PartialList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteIssuanceDetail(int? id)
        {
            bool status = false;
            string msg = "";
            List<long?> li = new List<long?>();
            using (var trans = db.Database.BeginTransaction())
            {
                var docSpec = db.AMIssuedItemDetails.Where(u => u.IssuedItemDetailId == id && u.BranchId == branch_ID).FirstOrDefault();
                if (docSpec != null)
                {
                    var returns = db.AMReturnIssueDetails.Where(u => u.PIPDetailId == docSpec.PIPDetailId).Any();
                    if (docSpec.IssuedItem?.IsPosted == true)
                        msg = "Posted invoice can't be delete";
                    if (returns == true)
                        msg = "Item used in Return voucher";
                    try
                    {
                        if (string.IsNullOrEmpty(msg))
                        {
                            db.AMIssuedItemDetails.Remove(docSpec);
                            db.SaveChanges();
                            trans.Commit();
                            status = true;
                            msg = "Deleted successfully";
                        }
                    }
                    catch (Exception exc)
                    {
                        msg = ExtensionMethods.GetExceptionMessages(exc);
                    }
                    var data = new { status = status, Msg = msg };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    msg = "Deletion failed";
                    var data = new { status = status, Msg = msg };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
        }

        #endregion

        #region ItemPosition
        public ActionResult ItemPosition(int? page)
        {
            ItemRegisterVM ex = new ItemRegisterVM();
            FillDDs_ItemPosition();
            var pd = new ApplicationPrimaryData(db);
            pd.AddUpdateAMConditionType();
            db.SaveChanges();
            DateTime now = DateTime.Now;
            ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.ToDate = now.ToShortDateString();
            GetItemRegister(ex, page);
            //  ex.v_mnl_AMItemPositionPagedList = ItemPositionPagedList(ex, page);
            return View(ex);
        }


        [HttpPost]
        public PartialViewResult ItemPosition(int? page, string FromDate, string ToDate, int? CategoryId, short? DepartmentId, int? ConditionTypeId, int? EmployeeId, int? RoomId, string ItemCode, string status)
        {
            ItemRegisterVM itemRegister = new ItemRegisterVM();
            itemRegister.FromDate = FromDate;
            itemRegister.ToDate = ToDate;
            itemRegister.CategoryId = CategoryId;
            itemRegister.DepartmentId = DepartmentId;
            itemRegister.ConditionTypeId = ConditionTypeId;
            itemRegister.EmployeeId = EmployeeId;
            itemRegister.RoomId = RoomId;
            itemRegister.ItemCode = ItemCode;
            itemRegister.status = status;
            //= new ManageIssuedItemsViewModel();
            page = page ?? 1;
            pageNo = page.Value;
            //if (!string.IsNullOrEmpty(command))
            //{
            //    if (command == "Retrun")
            //    {
            //        int count = 0;
            //        if (ex.v_mnl_AMItemPositionList != null && ex.v_mnl_AMItemPositionList.Where(u => u.IsCheked && u.Method == "Issued").Count() > 0)
            //        {
            //            count = ex.v_mnl_AMItemPositionList.Where(u => u.IsCheked && u.Method == "Issued").Count();
            //            var res = MakeReturnVoucher(ex.v_mnl_AMItemPositionList);
            //            if (res.HasValue)
            //            {
            //                ViewBag.ReturnIssueId = res;
            //                ViewBag.message = count + " Items returned successfully";
            //            }
            //            else
            //                ViewBag.message = 0 + " Items returned successfully";
            //        }
            //        else
            //            ViewBag.message = "No Items found";
            //    }
            //    if (command == "Issue")
            //    {
            //        int count = 0;
            //        if (ex.v_mnl_AMItemPositionList != null && ex.v_mnl_AMItemPositionList.Where(u => u.IsCheked && u.Method == "InStock").Count() > 0)
            //        {
            //            count = ex.v_mnl_AMItemPositionList.Where(u => u.IsCheked && u.Method == "InStock").Count();
            //            var res = MakeIssueVoucher(ex.v_mnl_AMItemPositionList);
            //            if (res.HasValue)
            //            {
            //                ViewBag.IssueItemId = res;
            //                ViewBag.message = count + " Items issued successfully";
            //            }
            //            else
            //                ViewBag.message = 0 + " Items issued successfully";
            //        }
            //        else
            //            ViewBag.message = "No Items found";
            //    }
            //}
            //ex.v_mnl_AMItemPositionPagedList = ItemPositionPagedList(ex, page);

            GetItemRegister(itemRegister, page);
            ModelState.Clear();
            return PartialView("_PartialItemPosition", itemRegister);
        }
        string GetItemCode(int ProductId)
        {
            var item = db.InvProducts.Find(ProductId);
            var category = db.InvCategories.Find(item.CategoryId);

            string newName = category.Prefix + "-" + (item.ItemPrefix) + "-";
            var temp = db.ItemRegister.Where(s => s.ItemCode.Contains(newName)).Select(s => s.ItemCode).Max();
            var ItemCode = string.Empty;
            if (string.IsNullOrEmpty(temp))
            {
                ItemCode = newName + String.Format("{0:D5}", 1);

            }
            else
            {
                Regex re = new Regex(@"\d+");
                Match result = re.Match(temp);
                int numaricPart = Convert.ToInt32(result.Value);
                ItemCode = newName + String.Format("{0:D5}", numaricPart + 1);

            }
            return ItemCode;
        }
        [HttpPost]
        public async Task<ActionResult> CreateItemRegister(ItemRegisterVM vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            string warnings = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new ItemRegister
                {

                    ProductId = vm.ItemRegisterItem.ProductId,
                    BranchId = (short)branch_ID,
                    Description = vm.ItemRegisterItem.Description,
                    Value = vm.ItemRegisterItem.Value,
                    DateOfEntry = vm.ItemRegisterItem.DateOfEntry,
                    ItemCode = GetItemCode(vm.ItemRegisterItem.ProductId),
                    ItemManualCode = vm.ItemRegisterItem.ItemManualCode,
                    SalvageValue = vm.ItemRegisterItem.SalvageValue,
                    CurrentValue = vm.ItemRegisterItem.CurrentValue,
                    LastDepreciationDate = vm.ItemRegisterItem.LastDepreciationDate,
                    NextDueDepreciationDate = vm.ItemRegisterItem.NextDueDepreciationDate,
                    DepreciationPercentage = vm.ItemRegisterItem.DepreciationPercentage,
                    //PurchaseInvoiceProductId = vm.ItemRegisterItem.PurchaseInvoiceProductId,
                    //EmployeeId = vm.ItemRegisterItem.EmployeeId,
                    //CurrentLocationId = vm.ItemRegisterItem.CurrentLocationId,
                    //CurrentdepartmentId = vm.ItemRegisterItem.CurrentdepartmentId,
                    ConditionTypeId = vm.ItemRegisterItem.ConditionTypeId,
                    Qty = 1,
                    Status = vm.ItemRegisterItem.Status,
                    CreatedOn = DateTime.Now,
                    CreatedBy = SessionHelper.UserID



                };
                db.ItemRegister.Add(entity);
                try
                {
                    await db.SaveChangesAsync();
                    message = "Item Register has been created successfully...";
                }
                catch (DbEntityValidationException ex)
                {
                    error = ex.GetExceptionMessages();
                }
                //await db.SaveChangesAsync();
            }
            else
            {
                warnings = GetModelStateErrors();
            }
            GetItemRegister(vm, 1);
            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialItemPosition", vm),
                Messages = message,
                Error = error,
                Warnings = warnings,
                GridId = "divStudents"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        int? MakeReturnVoucher(List<v_mnl_AMItemPosition_Result> v_mnl_AMItemPositionList)
        {
            int? idd = null;
            bool status = false;
            if (v_mnl_AMItemPositionList.Where(u => u.IsCheked && u.Method == "Issued").Count() > 0)
            {
                AMReturnIssue _ReturnIssue = new AMReturnIssue();
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        idd = db.AMReturnIssue.Where(u => u.BranchId == branch_ID).Max(u => (int?)u.ReturnIssueId);
                        if (idd == null)
                            idd = 1000;
                        idd++;
                        _ReturnIssue.ReturnIssueId = Convert.ToInt32(idd);
                        _ReturnIssue.ReturnIssueDate = DateTime.Now;
                        _ReturnIssue.Description = "";
                        _ReturnIssue.CreatedOn = DateTime.Now;
                        _ReturnIssue.CreatedBy = CreatedBy;
                        _ReturnIssue.BranchId = (short)branch_ID;
                        db.AMReturnIssue.Add(_ReturnIssue);
                        db.SaveChanges();
                        foreach (var item in v_mnl_AMItemPositionList.Where(u => u.IsCheked && u.Method == "Issued"))
                        {
                            if (item.ItemId > 0 && item.Qty > 0)
                            {
                                var detail = new AMReturnIssueDetail();
                                detail.BranchId = (short)branch_ID;
                                detail.CreatedBy = CreatedBy;
                                detail.CreatedOn = DateTime.Now;
                                //detail.WarehouseId = item.WarehouseId;
                                detail.ReturnIssueId = _ReturnIssue.ReturnIssueId;
                                detail.PIPDetailId = item.PurchaseInvoiceProductDetailId;
                                detail.ItemId = (int)item.ItemId;
                                //detail.ConditionTypeId = item.ConditionTypeId;
                                detail.Quantity = (int)item.Qty;
                                detail.Description = "";
                                db.AMReturnIssueDetails.Add(detail);
                                db.SaveChanges();
                            }
                        }
                        trans.Commit();
                        status = true;
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                }
            }
            return status == true ? idd : null;
        }

        int? MakeIssueVoucher(List<v_mnl_AMItemPosition_Result> v_mnl_AMItemPositionList)
        {
            int? idd = null;
            bool status = false;
            if (v_mnl_AMItemPositionList.Where(u => u.IsCheked && u.Method == "InStock").Count() > 0)
            {
                AMIssuedItem _IssuedItem = new AMIssuedItem();
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        _IssuedItem.IssueDate = DateTime.Now;
                        _IssuedItem.Description = "";
                        _IssuedItem.CreatedOn = DateTime.Now;
                        _IssuedItem.CreatedBy = CreatedBy;
                        _IssuedItem.BranchId = (short)branch_ID;
                        db.AMIssuedItems.Add(_IssuedItem);
                        db.SaveChanges();
                        idd = _IssuedItem.IssuedItemId;
                        foreach (var item in v_mnl_AMItemPositionList.Where(u => u.IsCheked && u.Method == "InStock"))
                        {
                            if (item.ItemId > 0 && item.Qty > 0)
                            {
                                var detail = new AMIssuedItemDetail();
                                detail.BranchId = (short)branch_ID;
                                detail.CreateBy = CreatedBy;
                                detail.CreatedOn = DateTime.Now;
                                //detail.WarehouseId = item.WarehouseId;
                                detail.IssuedItemId = _IssuedItem.IssuedItemId;
                                detail.PIPDetailId = item.PurchaseInvoiceProductDetailId;
                                detail.ItemId = (int)item.ItemId;
                                detail.ConditionTypeId = item.ConditionTypeId;
                                detail.Quantity = (int)item.Qty;
                                detail.Description = "";
                                db.AMIssuedItemDetails.Add(detail);
                                db.SaveChanges();
                            }
                        }
                        trans.Commit();
                        status = true;
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                }
            }
            return status == true ? idd : null;
        }

        IPagedList<v_mnl_AMItemPosition_Result> ItemPositionPagedList(ManageIssuedItemsViewModel ex, int? page)
        {
            var list = ProceduresModel.v_mnl_AMItemPositionList(db);
            if (!string.IsNullOrEmpty(ex.FromDate) && !string.IsNullOrEmpty(ex.ToDate))
            {
                var fdate = Convert.ToDateTime(ex.FromDate);
                var tdate = Convert.ToDateTime(ex.ToDate);
                list = list.Where(u => u.InvoiceDate.Date >= fdate && u.InvoiceDate.Date <= tdate);
            }
            else
            {
                if (!string.IsNullOrEmpty(ex.FromDate))
                {
                    var date = Convert.ToDateTime(ex.FromDate);
                    list = list.Where(u => u.InvoiceDate.Date >= date);
                }
                if (!string.IsNullOrEmpty(ex.ToDate))
                {
                    var date = Convert.ToDateTime(ex.ToDate);
                    list = list.Where(u => u.InvoiceDate.Date <= date);
                }
            }
            if (ex.DepartmentId.HasValue)
                list = list.Where(u => u.DepartmentId == ex.DepartmentId);
            if (ex.ItemId.HasValue)
                list = list.Where(u => u.ItemId == ex.ItemId);
            if (ex.ItemCodeDetailId.HasValue)
                list = list.Where(u => u.ItemCodeDetailId == ex.ItemCodeDetailId);
            if (ex.ConditionTypeId.HasValue)
                list = list.Where(u => u.ConditionTypeId == ex.ConditionTypeId);
            if (ex.EmployeeId.HasValue)
                list = list.Where(u => u.EmployeeId == ex.EmployeeId);
            if (ex.RoomId.HasValue)
                list = list.Where(u => u.RoomId == ex.RoomId);
            if (!string.IsNullOrEmpty(ex.status))
                list = list.Where(u => u.Method == ex.status);
            if (ex.Type.HasValue)
            {
                var ID = ex.Type.ToInt32();
                list = list.Where(u => u.IsConsumable == Convert.ToBoolean(ID));
            }

            return list.OrderByDescending(u => u.InvoiceDate).ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, ex.pagesize);
        }

        private void FillDDs_ItemPosition()
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
            //ViewBag.ItemsDD = from item in db.AMItems
            //                  where item.BranchId == branch_ID
            //                  select new SelectListItem
            //                  {
            //                      Value = item.ItemId.ToString(),
            //                      Text = item.ItemName
            //                  };

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
        private void GetItemRegister(ItemRegisterVM vm, int? page)
        {
            FillDD_ItemRegister();
            IQueryable<ItemRegister> queryable = db.ItemRegister.Include(x => x.Product).Include(x => x.AMConditionType).Where(x => x.BranchId == branch_ID);


            if (!string.IsNullOrEmpty(vm.ItemCode))
            {
                queryable = queryable.Where(s => s.Product.ProductName.ToLower().Contains(vm.ItemCode.ToLower()) ||
                s.ItemCode.ToLower().Contains(vm.ItemCode.ToLower()));
            }

            if (!string.IsNullOrEmpty(vm.FromDate) && !string.IsNullOrEmpty(vm.ToDate))
            {
                var fdate = Convert.ToDateTime(vm.FromDate);
                var tdate = Convert.ToDateTime(vm.ToDate);
                queryable = queryable.Where(u => u.DateOfEntry >= fdate && u.DateOfEntry <= tdate);
            }
            else
            {
                if (!string.IsNullOrEmpty(vm.FromDate))
                {
                    var date = Convert.ToDateTime(vm.FromDate);
                    queryable = queryable.Where(u => u.DateOfEntry >= date);
                }
                if (!string.IsNullOrEmpty(vm.ToDate))
                {
                    var date = Convert.ToDateTime(vm.ToDate);
                    queryable = queryable.Where(u => u.DateOfEntry <= date);
                }
            }
            if (vm.DepartmentId.HasValue)
                queryable = queryable.Where(u => u.CurrentdepartmentId == vm.DepartmentId);
            if (vm.CategoryId.HasValue)
                queryable = queryable.Where(u => u.Product.CategoryId == vm.CategoryId);
            //if (vm.ItemCodeDetailId.HasValue)
            //    list = list.Where(u => u.ItemCodeDetailId == ex.ItemCodeDetailId);
            if (vm.ConditionTypeId.HasValue)
                queryable = queryable.Where(u => u.ConditionTypeId == vm.ConditionTypeId);
            if (vm.EmployeeId.HasValue)
                queryable = queryable.Where(u => u.EmployeeId == vm.EmployeeId);
            if (vm.RoomId.HasValue)
                queryable = queryable.Where(u => u.CurrentLocationId == vm.RoomId);
            if (!string.IsNullOrEmpty(vm.status))
                queryable = queryable.ToList().Where(u => Convert.ToInt16(u.Status).ToString() == vm.status).AsQueryable();

            //if (!string.IsNullOrEmpty(vm.ItemCode))
            //    queryable = queryable.Where(u => u.ItemCode.Contains(vm.ItemCode));
            //if (vm.Type.HasValue)
            //{
            //    var ID = vm.Type.ToInt32();
            //    queryable = queryable.Where(u => u.IsConsumable == Convert.ToBoolean(ID));
            //}


            vm.CurrentItemRegister = queryable.OrderBy(l => l.ItemRegisterId).ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, vm.pagesize);
        }
        void FillDD_ItemRegister()
        {

            ViewBag.Products = db.InvProducts.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true && u.IsFixedAsset == true).Select(s => new SelectListItem
            {
                Value = s.ProductId.ToString(),
                Text = s.ProductName.ToString()
            }).ToList();

            ViewBag.Categories = db.InvCategories.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true && u.ParentCategory.CategoryName.ToLower().Contains("fixed")).Select(s => new SelectListItem
            {
                Value = s.CategoryId.ToString(),
                Text = s.CategoryName.ToString()
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
            ViewBag.ConditionTypes = db.AMConditionTypes.Select(s => new SelectListItem
            {
                Value = s.ConditionTypeId.ToString(),
                Text = s.Name.ToString()
            }).ToList();

            List<SelectListItem> ii = new List<SelectListItem>();
            ii.Add(new SelectListItem() { Text = "Available", Value = "1" });
            ii.Add(new SelectListItem() { Text = "Issued", Value = "2" });
            ii.Add(new SelectListItem() { Text = "Damage", Value = "3" });
            ViewBag.Statuses = ii;

            //List<SelectListItem> PaymentMode = new List<SelectListItem>();
            //PaymentMode.Add(new SelectListItem() { Text = "Cash", Value = "Cash" });
            //PaymentMode.Add(new SelectListItem() { Text = "Bank", Value = "Bank" });
            //ViewBag.PaymentMode = PaymentMode;
            //ViewBag.CashAccount = ProceduresModel.p_mnl_Account_GetCashAndBankAccounts(db, true, false, branch_ID, null).ToList();
            //ViewBag.BankAccount = ProceduresModel.p_mnl_Account_GetCashAndBankAccounts(db, false, true, branch_ID, null).ToList();

        }



        [HttpGet]
        public async Task<ActionResult> EditItemRegister(int? id)
        {
            ItemRegisterVM ItemRegister = new ItemRegisterVM();
            var entity = await db.ItemRegister.FindAsync(id);
            if (id == null || entity == null)
            {
                return Json(new { Error = "Invalid request found...", }, JsonRequestBehavior.AllowGet);
            }
            //vm.FillFeeHeadDDs();
            ItemRegister.ItemRegisterItem = entity;
            FillDD_ItemRegister();
            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialEditItemRegister", ItemRegister),
                TargetId = "frmModalContent",
                ModalId = "frmModal",
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> EditItemRegister(ItemRegisterVM vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            string warnings = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = await db.ItemRegister.FindAsync(vm.ItemRegisterItem.ItemRegisterId);
                if (entity != null)
                {
                    entity.ProductId = vm.ItemRegisterItem.ProductId;
                    entity.BranchId = (short)branch_ID;
                    entity.Description = vm.ItemRegisterItem.Description;
                    entity.Value = vm.ItemRegisterItem.Value;
                    entity.DateOfEntry = vm.ItemRegisterItem.DateOfEntry;
                    // entity.ItemCode = vm.ItemRegisterItem.ItemCode;
                    entity.ItemManualCode = vm.ItemRegisterItem.ItemManualCode;
                    //entity.PurchaseInvoiceProductId = vm.ItemRegisterItem.PurchaseInvoiceProductId;
                    //entity.EmployeeId = vm.ItemRegisterItem.EmployeeId;
                    //entity.CurrentLocationId = vm.ItemRegisterItem.CurrentLocationId;
                    entity.ConditionTypeId = vm.ItemRegisterItem.ConditionTypeId;
                    entity.Qty = 1;
                    entity.Status = vm.ItemRegisterItem.Status;
                    //entity.CurrentdepartmentId = vm.ItemRegisterItem.CurrentdepartmentId;
                    entity.ModifiedOn = DateTime.Now;
                    entity.ModifiedBy = SessionHelper.UserID;


                    try
                    {
                        await db.SaveChangesAsync();
                        message = $"Item Register '+{entity.Product.ProductName}' has been updated successfully...";
                        // vm.CurrentBrands = db.C.OrderBy(l => l.CurrencyName).ToPagedList(pageNo, 25);
                    }
                    catch (DbEntityValidationException ex)
                    {
                        error = ex.GetExceptionMessages();
                    }
                }
                else
                {
                    error = "Item not found";
                }
            }
            else
            {
                warnings = GetModelStateErrors();
            }

            GetItemRegister(vm, 1);

            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialItemPosition", vm),
                Error = error,
                Warnings = warnings,
                Messages = message,
                GridId = "divStudents",
                Reset = "true",
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> RemoveItemRegister(int? id)
        {
            string message = string.Empty;
            string error = string.Empty;
            string warnings = string.Empty;
            var entity = await db.ItemRegister.FindAsync(id);
            if (entity != null)
            {
                db.ItemRegister.Remove(entity);
                try
                {
                    await db.SaveChangesAsync();
                    message = "Item Register has been deleted successfully...";
                }
                catch (Exception)
                {
                    error = "Item Register Cannot be deleted because it is in use...";
                }
            }
            string partialView = string.Empty;
            ItemRegisterVM ItemRegister = new ItemRegisterVM();
            GetItemRegister(ItemRegister, 1);

            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialItemPosition", ItemRegister),
                Error = error,
                Messages = message,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region ---- Issue Items ----
        [HttpGet]
        public ActionResult IssueItems(int? pageNo)
        {
            pageNo = pageNo ?? 1;
            var vm = new FixedAssetVM();
            GetFixedAssetRegs(vm, pageNo);
            return View(vm);
        }

        public async Task<JsonResult> SearchDepartments(string q)
        {
            q = Regex.Replace(q, @"\s+", "");

            var resultList = await db.Departments.Where(r => r.DepartmentName.ToLower().Contains(q.ToLower()))
                    .Select(s => new
                    {
                        text = s.DepartmentName,
                        id = s.DepartmentId.ToString()
                    }).ToListAsync();
           
            return GetJsonResult(resultList);
        }
        public async Task<JsonResult> SearchEmployees(string q)
        {
            q = Regex.Replace(q, @"\s+", "");

            var resultList = await db.Employees.Where(r => r.EmpName.ToLower().Contains(q.ToLower()))
                    .Select(s => new
                    {
                        text = s.EmpName,
                        id = s.EmployeeId.ToString()
                    }).ToListAsync();
           
            return GetJsonResult(resultList);
        }
        public async Task<JsonResult> SearchLocations(string q)
        {
            q = Regex.Replace(q, @"\s+", "");

            var resultList = await db.CompanyRooms.Where(r => r.RoomCode.ToLower().Contains(q.ToLower()))
                    .Select(s => new
                    {
                        text = s.RoomCode,
                        id = s.RoomId.ToString()
                    }).ToListAsync();
           
            return GetJsonResult(resultList);
        }

        [HttpPost]
        public ActionResult IssueItems(int? pageNo, int? CategoryId, short? DepartmentId, int? ConditionTypeId, int? EmployeeId, int? LocationId, string Search, string Status)
        {
            FixedAssetVM vm = new FixedAssetVM();
            vm.CategoryId = CategoryId;
            vm.DepartmentId = DepartmentId;
            vm.ConditionTypeId = ConditionTypeId;
            vm.EmployeeId = EmployeeId;
            vm.LocationId = LocationId;
            vm.Search = Search;
            vm.Status = Status;
            GetFixedAssetRegs(vm, pageNo);
            return PartialView("_IssueItems", vm);
        }

        [HttpPost]
        public ActionResult PostIssueItems(FixedAssetVM vm)
        {
            string error = string.Empty;
            try
            {
                // Insertion/Updation In AM.TransferHistory
                var getAllChecked = vm.FixedAssetRegVM.Where(x => x.check);
                if (getAllChecked != null && getAllChecked.Count() > 0)
                {
                    foreach (var item in getAllChecked)
                    {

                        TransferType TransferType = TransferType.Issue;
                        ItemRegisterEnum ItemRegisterEnum = ItemRegisterEnum.Issued;
                        bool Instock = false;
                        if (vm.Command == "Return")
                        {
                            TransferType = TransferType.Return;
                            ItemRegisterEnum = ItemRegisterEnum.Available;
                            Instock = true;
                        }
                        else if (vm.Command == "Damage")
                        {
                            TransferType = TransferType.Damage;
                            ItemRegisterEnum = ItemRegisterEnum.Damaged;
                            Instock = true;
                        }
                        else if (vm.Command == "Transfer")
                        {
                            TransferType = TransferType.Transfer;
                            ItemRegisterEnum = ItemRegisterEnum.Issued;
                            Instock = false;
                        }
                        var transHistory = new TransferHistory
                        {
                            ItemRegisterId = item.ItemRegisterId,
                            DepartmentId = vm.PostingData.DepartmentId,
                            LocationId = vm.PostingData.LocationId,
                            EmployeeId = vm.PostingData.EmployeeId,
                            ConditionTypeId = (int)(vm.PostingData.ConditionTypeId == null ? 1 : vm.PostingData.ConditionTypeId),
                            Description = vm.PostingData.Description,
                            Date = vm.PostingData.IssueDate,
                            TransferType = TransferType,
                            BranchId = (short)branch_ID,
                            CreatedBy = CreatedBy,
                            CreatedOn = DateTime.Now,
                        };
                        db.TransferHistory.Add(transHistory);


                        //if (item.TransferHistoryId > 0)
                        //{
                        //    // Updation
                        //    var transHistory = db.TransferHistory.FirstOrDefault(x => x.TransferHistoryId == item.TransferHistoryId && item.ItemRegisterId == item.ItemRegisterId);
                        //    if (transHistory != null)
                        //    {
                        //        transHistory.DepartmentId = (short)item.DepartmentId;
                        //        transHistory.LocationId = (int)item.LocationId;
                        //        transHistory.EmployeeId = (int)item.EmployeeId;
                        //        transHistory.ConditionTypeId = (int)item.ConditionTypeId;
                        //        transHistory.Description = item.Description;
                        //        // transHistory.Date = DateTime.Now;
                        //        //transHistory.TransferType = TransferType.Issue;
                        //        transHistory.BranchId = (short)branch_ID;
                        //        transHistory.ModifiedBy = CreatedBy;
                        //        transHistory.ModifiedOn = DateTime.Now;
                        //    }
                        //}
                        //else
                        //{
                        //    // Insertion
                        //}

                        // Update Status In AM.ItemRegister against itemregisterId 
                        var itemReg = db.ItemRegister.FirstOrDefault(x => x.ItemRegisterId == item.ItemRegisterId);
                        if (itemReg != null)
                        {
                            itemReg.Status = ItemRegisterEnum;
                            itemReg.InStock = Instock;
                            itemReg.Description = vm.PostingData.Description;
                            itemReg.ConditionTypeId = (int)(vm.PostingData.ConditionTypeId == null ? 1 : vm.PostingData.ConditionTypeId);
                            if (vm.Command == "Issue" || vm.Command == "Transfer")
                            {
                                itemReg.CurrentdepartmentId = (short)vm.PostingData.DepartmentId;
                                itemReg.EmployeeId = vm.PostingData.EmployeeId; //(short?)
                                itemReg.CurrentLocationId = (short?)vm.PostingData.LocationId;
                            }
                            else
                            {
                                itemReg.CurrentdepartmentId = null;
                                itemReg.EmployeeId = null;
                                itemReg.CurrentLocationId = null;
                            }
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception err)
            {
                error = err.Message;
                if (err.InnerException?.InnerException?.Message != null)
                {
                    error = err.InnerException.InnerException.Message;
                }
            }
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }
            GetFixedAssetRegs(vm, 1);
            return PartialView("_IssueItems", vm);

            //var result = new
            //{
            //    PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_IssueItems", vm),
            //    GridId = "divStudents",
            //    Error = error,
            //};
            //return Json(result, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private void GetFixedAssetRegs(FixedAssetVM vm, int? pageNo)
        {
            FillDD_ItemRegister();
            vm.Status = string.IsNullOrEmpty(vm.Status) ? "1" : vm.Status;
            var statusID = vm.Status.ToInt32();
            var list = AMProceduresModel.GetItemRegisterIssue(db, 1, statusID, branch_ID);

            if (!string.IsNullOrEmpty(vm.Search))
            {
                list = list.Where(s => s.ProductName.ToLower().Contains(vm.Search.ToLower()) ||
                s.ItemCode.ToLower().Contains(vm.Search.ToLower())
                ).ToList();
            }
            if (vm.DepartmentId.HasValue)
            {
                list = list.Where(x => x.DepartmentId == vm.DepartmentId).ToList();
            }
            if (vm.EmployeeId.HasValue)
            {
                list = list.Where(x => x.EmployeeId == vm.EmployeeId).ToList();
            }
            if (vm.LocationId.HasValue)
            {
                list = list.Where(x => x.LocationId == vm.LocationId).ToList();
            }
            if (vm.ConditionTypeId.HasValue)
            {
                list = list.Where(x => x.ConditionTypeId == vm.ConditionTypeId).ToList();
            }     
            if (vm.ProductId.HasValue)
            {
                list = list.Where(x => x.ProductId == vm.ProductId).ToList();
            }
            if (vm.CategoryId.HasValue)
            {
                list = list.Where(x => x.CategoryId == vm.CategoryId).ToList();
            }


            vm.FixedAssetRegPager = list.ToPagedList(pageNo.HasValue ? Convert.ToInt32(pageNo) : 1, vm.pagesize);
            vm.FixedAssetRegVM = vm.FixedAssetRegPager.ToList();
            //return List;
        }
        #endregion
    }
}