using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FAPP.Areas.AM.BLL;
namespace FAPP.Areas.AM.Controllers
{
    public class ReturnIssueController : FAPP.Controllers.BaseController
    {
        private int branch_ID = SessionHelper.BranchId;
        private int CreatedBy = SessionHelper.UserID;

        public ActionResult Index()
        {
            return View();
        }

        #region AddEditReturnIssue
        [HttpGet]
        public ActionResult AddEditReturnIssue(int? id)
        {
            var vm = GetAddEditReturnIssue(id);
            return View(vm);
        }

        private ReturnIssueModel GetAddEditReturnIssue(int? id)
        {
            var vm = new ReturnIssueModel();
            FillDDs_Return();
            var _ReturnIssue = db.AMReturnIssue.Where(u => u.BranchId == branch_ID && u.ReturnIssueId == id).FirstOrDefault();
            if (_ReturnIssue != null)
            {
                vm.ReturnIssue = _ReturnIssue;
                vm.ReturnIssueDetail = db.AMReturnIssueDetails.Where(u => u.BranchId == branch_ID && u.ReturnIssueId == id).ToList();
            }
            else
            {
                vm.ReturnIssue = new AMReturnIssue();
                vm.ReturnIssue.ReturnIssueDate = DateTime.Now.Date;
            }
            return vm;
        }

        public ActionResult SaveReturnIssue(ReturnIssueModel vm)
        {
            var _ReturnIssue = db.AMReturnIssue.Where(u => u.BranchId == branch_ID && u.ReturnIssueId == vm.ReturnIssue.ReturnIssueId).FirstOrDefault();
            if (_ReturnIssue != null)
            {
                return UpdateReturnIssue(vm, _ReturnIssue);
            }
            else
            {
                return InsertReturnIssue(vm);
            }
        }

        private ActionResult UpdateReturnIssue(ReturnIssueModel vm, AMReturnIssue _ReturnIssue)
        {
            bool result = false;
            string msg = "Updated successfully";
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    _ReturnIssue.ReturnIssueDate = vm.ReturnIssue.ReturnIssueDate;
                    _ReturnIssue.Description = vm.ReturnIssue.Description;
                    _ReturnIssue.ModifiedOn = DateTime.Now;
                    _ReturnIssue.ModifiedBy = CreatedBy;
                    _ReturnIssue.BranchId = (short)branch_ID;
                    db.SaveChanges();

                    for (int i = 0; i < vm.ReturnIssueDetail.Count(); i++)
                    {
                        var item = vm.ReturnIssueDetail[i];
                        if (item.ItemId > 0 && item.Quantity > 0)
                        {
                            var _ReturnIssueDetail = db.AMReturnIssueDetails.Find(item.ReturnIssueDetailId);
                            if (_ReturnIssueDetail != null)
                            {
                                _ReturnIssueDetail.BranchId = (short)branch_ID;
                                _ReturnIssueDetail.ModifiedBy = CreatedBy;
                                _ReturnIssueDetail.ModifiedOn = DateTime.Now;
                                _ReturnIssueDetail.WarehouseId = item.WarehouseId;
                                _ReturnIssueDetail.ReturnIssueId = _ReturnIssue.ReturnIssueId;
                                if (item.PIPDetailId != null)
                                    _ReturnIssueDetail.PIPDetailId = item.PIPDetailId;
                                _ReturnIssueDetail.ConditionTypeId = item.ConditionTypeId;
                                _ReturnIssueDetail.ItemId = item.ItemId;
                                _ReturnIssueDetail.Quantity = item.Quantity;
                                _ReturnIssueDetail.Description = item.Description;
                            }
                            else
                            {
                                var detail = new AMReturnIssueDetail();
                                detail.BranchId = (short)branch_ID;
                                detail.CreatedBy = CreatedBy;
                                detail.CreatedOn = DateTime.Now;
                                detail.WarehouseId = item.WarehouseId;
                                detail.ReturnIssueId = _ReturnIssue.ReturnIssueId;
                                if (item.PIPDetailId != null)
                                    detail.PIPDetailId = item.PIPDetailId;
                                detail.ConditionTypeId = item.ConditionTypeId;
                                detail.ItemId = item.ItemId;
                                detail.Quantity = item.Quantity;
                                db.AMReturnIssueDetails.Add(detail);
                            }
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();
                    result = true;
                }
                catch (Exception exc)
                {

                    msg = "Updation failed, " + ExtensionMethods.GetExceptionMessages(exc);
                    throw;
                }
            }
            FillDDs_Return();
            vm.ReturnIssue = db.AMReturnIssue.Where(u => u.BranchId == branch_ID && u.ReturnIssueId == _ReturnIssue.ReturnIssueId).FirstOrDefault();
            vm.ReturnIssueDetail = db.AMReturnIssueDetails.Where(u => u.BranchId == branch_ID && u.ReturnIssueId == _ReturnIssue.ReturnIssueId).ToList();

            List<Service.Helper> PartialList = new List<Service.Helper>();
            PartialList.Add(new Service.Helper { divToReplace = "mainsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReturnIssue", vm) });
            PartialList.Add(new Service.Helper { divToReplace = "printsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReturnIssuePrintSection", vm) });
            return Json(new { status = result, msg = msg, PartialList }, JsonRequestBehavior.AllowGet);
        }

        private ActionResult InsertReturnIssue(ReturnIssueModel vm)
        {
            bool result = false;
            string msg = "Saved successfully";
            AMReturnIssue _ReturnIssue = new AMReturnIssue();
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var idd = db.AMReturnIssue.Where(u => u.BranchId == branch_ID).Max(u => (int?)u.ReturnIssueId);
                    if (idd == null)
                        idd = 1000;
                    idd++;
                    _ReturnIssue.ReturnIssueId = Convert.ToInt32(idd);
                    _ReturnIssue.ReturnIssueDate = vm.ReturnIssue.ReturnIssueDate;
                    _ReturnIssue.Description = vm.ReturnIssue.Description;
                    _ReturnIssue.CreatedOn = DateTime.Now;
                    _ReturnIssue.CreatedBy = CreatedBy;
                    _ReturnIssue.BranchId = (short)branch_ID;
                    db.AMReturnIssue.Add(_ReturnIssue);
                    db.SaveChanges();
                    for (int i = 0; i < vm.ReturnIssueDetail.Count(); i++)
                    {
                        var item = vm.ReturnIssueDetail[i];
                        if (item.ItemId > 0 && item.Quantity > 0)
                        {
                            var detail = new AMReturnIssueDetail();
                            detail.BranchId = (short)branch_ID;
                            detail.CreatedBy = CreatedBy;
                            detail.CreatedOn = DateTime.Now;
                            detail.WarehouseId = item.WarehouseId;
                            detail.ReturnIssueId = _ReturnIssue.ReturnIssueId;
                            if (item.PIPDetailId != null)
                                detail.PIPDetailId = item.PIPDetailId;
                            detail.ItemId = item.ItemId;
                            detail.ConditionTypeId = item.ConditionTypeId;
                            detail.Quantity = item.Quantity;
                            detail.Description = item.Description;
                            db.AMReturnIssueDetails.Add(detail);
                        }
                        db.SaveChanges();
                        result = true;
                    }
                    trans.Commit();

                }
                catch (Exception exc)
                {

                    msg = "Saving failed, " + ExtensionMethods.GetExceptionMessages(exc);
                    throw;
                }
            }
            FillDDs_Return();
            vm.ReturnIssue = db.AMReturnIssue.Where(u => u.BranchId == branch_ID && u.ReturnIssueId == _ReturnIssue.ReturnIssueId).FirstOrDefault();
            vm.ReturnIssueDetail = db.AMReturnIssueDetails.Where(u => u.BranchId == branch_ID && u.ReturnIssueId == _ReturnIssue.ReturnIssueId).ToList();

            //return PartialView("_PartialIssueitem", vm);
            List<Service.Helper> PartialList = new List<Service.Helper>();
            PartialList.Add(new Service.Helper { divToReplace = "mainsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReturnIssue", vm) });
            PartialList.Add(new Service.Helper { divToReplace = "printsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReturnIssuePrintSection", vm) });
            return Json(new { status = result, msg = msg, PartialList }, JsonRequestBehavior.AllowGet);
        }

        private void FillDDs_Return()
        {
            ViewBag.ItemsDD = from item in db.AMItems
                              where item.BranchId == branch_ID
                              select new SelectListItem
                              {
                                  Value = item.ItemId.ToString(),
                                  Text = item.ItemName
                              };
            ViewBag.WarehousesDD = from emp in db.AMWarehouses
                                   where emp.BranchId == branch_ID
                                   select new SelectListItem
                                   {
                                       Text = emp.WarehouseName,
                                       Value = emp.WarehouseId.ToString()
                                   };
            ViewBag.ItemCodes = AMProceduresModel.v_mnl_AMIssueItemExistCodeList(db).ToList();
            
            //ViewBag.ItemCodes = from codes in db.AMPurchaseInvoiceProductDetails
            //                    select new TextValueId
            //                    {
            //                        Id = codes.ItemId,
            //                        Text = codes.ItemCode,
            //                        Value = codes.DetailId.ToString()
            //                    };

            ViewBag.ConditionTypesDD = from cond in db.AMConditionTypes
                                       select new SelectListItem
                                       {
                                           Text = cond.Name,
                                           Value = cond.ConditionTypeId.ToString()
                                       };
        }

        public JsonResult GetItemCodesById(int id)
        {
            return ItemCodesById(id);
        }

        JsonResult ItemCodesById(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var itemCodes = AMProceduresModel.v_mnl_AMIssueItemCodeList(db).Where(u => u.ItemId == id).Select(u => new
            {
                Qty = u.Quantity,
                Text = u.ItemCode,
                Value = u.DetailId,
                IsConsumable = u.IsConsumable
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
                return ItemCodesById(item.ItemId);
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

        public async Task<ActionResult> PostReturnIssueVoucher(int? id)
        {
            var result = false;
            string msg = "";
            var vm = new ReturnIssueModel();
            List<Service.Helper> PartialList = new List<Service.Helper>();
            var _ReturnIssue = await db.AMReturnIssue.Where(u => u.ReturnIssueId == id && u.BranchId == branch_ID).FirstOrDefaultAsync();
            if (_ReturnIssue != null)
            {
                _ReturnIssue.IsPosted = true;
                await db.SaveChangesAsync();
                result = true;
                msg = "Voucher posted successfully";

                vm = GetAddEditReturnIssue(id);
                PartialList.Add(new Service.Helper { divToReplace = "headersection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReturnHeaderSection", vm) });
                PartialList.Add(new Service.Helper { divToReplace = "mainsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReturnIssue", vm) });
                PartialList.Add(new Service.Helper { divToReplace = "printsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReturnIssuePrintSection", vm) });
            }
            return Json(new { status = result, msg = msg, PartialList }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UnPostReturnIssueVoucher(int? id)
        {
            var result = false;
            string msg = "";
            var vm = new ReturnIssueModel();
            List<Service.Helper> PartialList = new List<Service.Helper>();
            var _ReturnIssue = await db.AMReturnIssue.Where(u => u.ReturnIssueId == id && u.BranchId == branch_ID).FirstOrDefaultAsync();
            if (_ReturnIssue != null)
            {
                _ReturnIssue.IsPosted = false;
                await db.SaveChangesAsync();
                result = true;
                msg = "Voucher unposted successfully";

                vm = GetAddEditReturnIssue(id);
                PartialList.Add(new Service.Helper { divToReplace = "headersection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReturnHeaderSection", vm) });
                PartialList.Add(new Service.Helper { divToReplace = "mainsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReturnIssue", vm) });
                PartialList.Add(new Service.Helper { divToReplace = "printsection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReturnIssuePrintSection", vm) });
            }
            return Json(new { status = result, msg = msg, PartialList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteReturnDetail(int? id)
        {
            bool status = false;
            string msg = "";
            List<long?> li = new List<long?>();
            using (var trans = db.Database.BeginTransaction())
            {
                var docSpec = db.AMReturnIssueDetails.Where(u => u.ReturnIssueDetailId == id && u.BranchId == branch_ID).FirstOrDefault();
                if (docSpec != null)
                {
                    //var returns = db.AMReturnIssueDetails.Where(u => u.PIPDetailId == docSpec.PIPDetailId).Any();
                    if (docSpec.ReturnIssue?.IsPosted == true)
                        msg = "Posted invoice can't be delete";
                    //if (returns == true)
                    //msg = "Item used in Return voucher";
                    try
                    {
                        if (string.IsNullOrEmpty(msg))
                        {
                            db.AMReturnIssueDetails.Remove(docSpec);
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

        #region ReturnIssuedItems
        public ActionResult ReturnIssuedItems(int? page)
        {
            ManageIssuedItemsViewModel ex = new ManageIssuedItemsViewModel();
            DateTime now = DateTime.Now;
            ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.ToDate = now.ToShortDateString();
            ex.ReturnIssuePagedList = ReturnIssuedItemsPagedList(ex, page);
            return View(ex);
        }

        [HttpPost]
        public PartialViewResult ReturnIssuedItems(ManageIssuedItemsViewModel ex, int? page)
        {
            ex.ReturnIssuePagedList = ReturnIssuedItemsPagedList(ex, page);
            ModelState.Clear();
            return PartialView("_PartialReturnIssuedItems", ex);
        }

        IPagedList<AMReturnIssue> ReturnIssuedItemsPagedList(ManageIssuedItemsViewModel ex, int? page)
        {
            var list = db.AMReturnIssue.AsQueryable();
            if (!string.IsNullOrEmpty(ex.FromDate) && !string.IsNullOrEmpty(ex.ToDate))
            {
                var fdate = Convert.ToDateTime(ex.FromDate);
                var tdate = Convert.ToDateTime(ex.ToDate);
                list = list.Where(u => u.BranchId == branch_ID && DbFunctions.TruncateTime(u.ReturnIssueDate) >= fdate && DbFunctions.TruncateTime(u.ReturnIssueDate) <= tdate);
            }
            else
            {
                if (!string.IsNullOrEmpty(ex.FromDate))
                {
                    var date = Convert.ToDateTime(ex.FromDate);
                    list = list.Where(u => u.BranchId == branch_ID && DbFunctions.TruncateTime(u.ReturnIssueDate) >= date);
                }
                if (!string.IsNullOrEmpty(ex.ToDate))
                {
                    var date = Convert.ToDateTime(ex.ToDate);
                    list = list.Where(u => u.BranchId == branch_ID && DbFunctions.TruncateTime(u.ReturnIssueDate) <= date);
                }
            }
            return list.OrderByDescending(u => u.ReturnIssueDate).ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, ex.pagesize);
        }


        #endregion


    }
}