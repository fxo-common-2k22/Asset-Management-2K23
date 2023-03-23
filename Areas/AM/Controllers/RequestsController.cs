using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.INV.Models;
using FAPP.Model;
using FAPP.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FAPP.Areas.AM.FormActionsReportsRights.FormActionRights;

namespace FAPP.Areas.AM.Controllers
{
    public class RequestsController : FAPP.Controllers.BaseController
    {
        //private OneDbContext db = new OneDbContext();
        private short branch_ID = (short)SessionHelper.BranchId;
        private int fiscalYearId = SessionHelper.FiscalYearId;
        private int CreatedBy = SessionHelper.UserID;
        // GET: AM/Requests
        public ActionResult Index()
        {
            return View();
        }

        #region Previous
        [HttpGet]
        public ActionResult AddEditRequest1(int? id)
        {

            var vm = new AddEditRequestViewModel();
            vm.RequestDate = DateTime.Now;
            vm.OrderPurchaseDate = DateTime.Now;
            var request = db.InvRequests.Find(id);
            if (request != null && id > 0)
            {
                vm.EditMode = true;
                vm.RequestId = request.RequestId;
                vm.RequestDate = request.RequestDate;
                if (request.EmployeeId != null)
                    vm.EmployeeId = (int)request.EmployeeId;
                if (request.DepartmentId != null)
                    vm.DepartmentId = (short)request.DepartmentId;
                vm.Description = request.Description;
                vm.StatusId = request.StatusId??0;
                vm.Details = (from reqDetails in db.InvRequestDetails
                              where reqDetails.RequestId == id
                              select new RequestDetailsViewModel
                              {
                                  RequestDetailId =reqDetails.RequestDetailId,
                                  Description = reqDetails.Description,
                                  ProductId = reqDetails.ProductId,
                                  Quantity =reqDetails.Quantity,
                                  RoomNumber = reqDetails.RoomId,
                                  RoomId = reqDetails.RoomId
                              }).ToList();
                var dds = FillDD();
                vm.DepartmentsDD = dds.DepartmentsDD;
                vm.EmployeeDD = dds.EmployeeDD;
                vm.ItemsDD = dds.ItemsDD;
                vm.RoomDD = dds.RoomDD;
                vm.Status = dds.Status;
                vm.ClientsDD = dds.ClientsDD;
                return View(vm);
            }
            else if (id > 0)
            {
                ViewBag.Error = "Error";
                var dds = FillDD();
                vm.DepartmentsDD = dds.DepartmentsDD;
                vm.EmployeeDD = dds.EmployeeDD;
                vm.ItemsDD = dds.ItemsDD;
                vm.Status = dds.Status;
                vm.ClientsDD = dds.ClientsDD;
                vm.RoomDD = dds.RoomDD;
                return View(vm);
            }
            else
            {
                var dds = FillDD();
                vm.DepartmentsDD = dds.DepartmentsDD;
                vm.EmployeeDD = dds.EmployeeDD;
                vm.ItemsDD = dds.ItemsDD;
                vm.RoomDD = dds.RoomDD;
                vm.ClientsDD = dds.ClientsDD;
                vm.Status = dds.Status;
                return View(vm);
            }
        }
        private AddEditRequestViewModel FillDD()
        {
            var vm = new AddEditRequestViewModel();
            vm.DepartmentsDD = from dept in db.Departments
                               where (dept.BranchId == branch_ID)
                               select new SelectListItem
                               {
                                   Text = dept.DepartmentName,
                                   Value = dept.DepartmentId.ToString()
                               };
            vm.EmployeeDD = from emp in db.Employees

                            where emp.BranchId == branch_ID
                            select new SelectListItem
                            {
                                Value = emp.EmployeeId.ToString(),
                                Text = emp.EmpName
                            };
            vm.ItemsDD = db.InvProducts.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true).Select(s => new SelectListItem
            {
                Value = s.ProductId.ToString(),
                Text = s.ProductName.ToString()
            }).ToList();

            vm.Status = from sts in db.InvRequestStatuses
                        select new SelectListItem()
                        {
                            Text = sts.StatusName,
                            Value = sts.StatusId.ToString()
                        };
            var roomList = new List<SelectListItem>();
            roomList.Add(new SelectListItem() { Text = "1", Value = "1" });
            roomList.Add(new SelectListItem() { Text = "2", Value = "2" });
            roomList.Add(new SelectListItem() { Text = "3", Value = "3" });
            vm.RoomDD = roomList;

            vm.ClientsDD = from client in db.Clients
                           where client.BranchId == branch_ID && client.IsSupplier == true
                           select new SelectListItem
                           {
                               Value = client.ClientId.ToString(),
                               Text = client.Name,
                           };
            return vm;
        }
        [HttpPost]
        public ActionResult SaveRequest(AddEditRequestViewModel vm)
        {
            try
            {
                if (vm.RequestId > 0 && vm.Details.Count() > 1)
                {
                    return UpdateRequests(vm);
                }
                else
                {
                    return InsertNewRequest(vm);
                }
            }
            catch (Exception)
            {
                return RedirectToAction("AddEditRequest", new { id = vm.RequestId });
            }
        }

        private ActionResult UpdateRequests(AddEditRequestViewModel vm)
        {
            var request = db.InvRequests.Find(vm.RequestId);
            if (request != null)
            {
                request.RequestDate = vm.RequestDate.Date;
                request.Description = vm.Description;
                request.EmployeeId = vm.EmployeeId;
                request.DepartmentId = vm.DepartmentId;
                request.ModifiedOn = DateTime.Now;
                request.ModifiedBy = CreatedBy;
                request.BranchId = (short)branch_ID;
                request.StatusId = vm.StatusId;
                db.SaveChanges();

                for (int i = 0; i < vm.Details.Count() - 1; i++)
                {
                    var item = vm.Details[i];
                    if (item.ProductId != null)
                    {
                        InvRequestDetail requestDetail = db.InvRequestDetails.Find(item.RequestDetailId);
                        if (requestDetail != null)
                        {
                            requestDetail.RequestId = request.RequestId;
                            requestDetail.ProductId = (int)item.ProductId;
                            requestDetail.Quantity = item.Quantity;
                            requestDetail.RoomId = item.RoomNumber;
                            requestDetail.Description = item.Description;
                            requestDetail.ModifiedOn = DateTime.Now;
                            requestDetail.ModifiedBy = CreatedBy;
                        }
                        else
                        {
                            requestDetail.RequestId = request.RequestId;
                            requestDetail.ProductId = (int)item.ProductId;
                            requestDetail.Quantity = item.Quantity;
                            requestDetail.RoomId = item.RoomNumber;
                            requestDetail.Description = item.Description;
                            requestDetail.ModifiedOn = DateTime.Now;
                            requestDetail.ModifiedBy = CreatedBy;
                            db.InvRequestDetails.Add(requestDetail);
                        }
                        db.SaveChanges();
                    }
                }

                var req = db.InvRequests.Find(request.RequestId);
                vm.RequestId = req.RequestId;
                vm.RequestDate = req.RequestDate;
                vm.StatusId = req.StatusId??0;
                if (req.EmployeeId != null)
                    vm.EmployeeId = (int)req.EmployeeId;
                if (req.DepartmentId != null)
                    vm.DepartmentId = (short)req.DepartmentId;
                vm.Description = req.Description;
                vm.RequestNo = req.RequestId;
                vm.Details = (from reqDetails in db.InvRequestDetails
                              where reqDetails.RequestId == request.RequestId
                              select new RequestDetailsViewModel
                              {
                                  RequestDetailId = reqDetails.RequestDetailId,
                                  Description = reqDetails.Description,
                                  ProductId = reqDetails.ProductId,
                                  Quantity = reqDetails.Quantity,
                                  RoomNumber = reqDetails.RoomId
                              }).ToList();
                var dds = FillDD();
                vm.DepartmentsDD = dds.DepartmentsDD;
                vm.EmployeeDD = dds.EmployeeDD;
                vm.ItemsDD = dds.ItemsDD;
                vm.Status = dds.Status;
                vm.RoomDD = dds.RoomDD;
                vm.EditMode = true;
                return PartialView("_PartialAddEditRequest", vm);
            }
            else return null;
        }
        private ActionResult InsertNewRequest(AddEditRequestViewModel vm)
        {
            if (vm.Details.Count() > 0)
            {
                InvRequest request = new InvRequest();
                request.RequestDate = vm.RequestDate.Date;
                request.Description = vm.Description;
                request.EmployeeId = vm.EmployeeId;
                request.DepartmentId = vm.DepartmentId;
                request.CreatedOn = DateTime.Now;
                request.CreatedBy = CreatedBy;
                request.BranchId = (short)branch_ID;
                request.StatusId = vm.StatusId;
                db.InvRequests.Add(request);
                db.SaveChanges();

                for (int i = 0; i < vm.Details.Count() - 1; i++)
                {
                    var item = vm.Details[i];
                    InvRequestDetail requestDetail = new InvRequestDetail();
                    requestDetail.RequestId = request.RequestId;
                    requestDetail.ProductId = (int)item.ProductId;
                    requestDetail.Quantity = item.Quantity;
                    requestDetail.RoomId = item.RoomNumber;
                    requestDetail.Description = item.Description;
                    requestDetail.CreatedOn = DateTime.Now;
                    requestDetail.CreatedBy = CreatedBy;
                    db.InvRequestDetails.Add(requestDetail);
                }
                db.SaveChanges();
                var req = db.InvRequests.Find(request.RequestId);
                vm.RequestId = req.RequestId;
                vm.RequestNo = req.RequestId;
                vm.RequestDate = req.RequestDate;
                vm.StatusId = req.StatusId??0;
                if (req.EmployeeId != null)
                    vm.EmployeeId = (int)req.EmployeeId;
                if (req.DepartmentId != null)
                    vm.DepartmentId = (short)req.DepartmentId;
                vm.Description = req.Description;


                vm.Details = (from reqDetails in db.InvRequestDetails
                              where reqDetails.RequestId == request.RequestId
                              select new RequestDetailsViewModel
                              {
                                  RequestDetailId = reqDetails.RequestDetailId,
                                  Description = reqDetails.Description,
                                  ProductId = reqDetails.ProductId,
                                  Quantity = reqDetails.Quantity,
                                  RoomNumber = reqDetails.RoomId
                              }).ToList();
                var dds = FillDD();
                vm.DepartmentsDD = dds.DepartmentsDD;
                vm.EmployeeDD = dds.EmployeeDD;
                vm.ItemsDD = dds.ItemsDD;
                vm.Status = dds.Status;
                vm.RoomDD = dds.RoomDD;
                return PartialView("_PartialAddEditRequest", vm);
                //var temp = new
                //{
                //    RequestId = request.RequestId,
                //    RequestDetailIds = db.AMRequestDetail.Where(s=> s.RequestId == request.RequestId).ToArray()
                //};
                //return Json(temp, JsonRequestBehavior.AllowGet); 
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        public bool SaveOrderPurchase(AddEditRequestViewModel vm)
        {
            try
            {
                long maxId = 0;
                if (db.InvPurchaseOrders.Count() > 0)
                    maxId = db.InvPurchaseOrders.Max(s => s.PurchaseOrderId);
                var code = db.InvPurchaseOrders.Max(s => s.PurchaseOrderCode);
                if (string.IsNullOrEmpty(code))
                {
                    //generate new code
                    code = "PO-" + String.Format("{0:D5}", 1);

                }
                else
                {
                    Regex re = new Regex(@"\d+");
                    Match result = re.Match(code);
                    int numaricPart = Convert.ToInt32(result.Value);
                    code = "PO-" + String.Format("{0:D5}", numaricPart + 1);
                }
                InvPurchaseOrder entity = new InvPurchaseOrder()
                {
                    PurchaseOrderCode = code,
                    RequestId = vm.RequestId,
                    Description = vm.PurchaseOrderDescription,
                    PurchaseOrderDate = vm.OrderPurchaseDate,
                    PurchaseOrderId = (maxId == 0 ? 1 : (long)maxId + 1),
                    SupplierId = vm.ClientId,
                    CreatedBy = CreatedBy,
                    CreatedOn = DateTime.Now,
                };
                db.InvPurchaseOrders.Add(entity);
                db.SaveChanges();

                var requestDetails = from detail in db.AMRequestDetail
                                     where detail.RequestId == vm.RequestId
                                     select detail;
                foreach (var item in requestDetails)
                {
                    var puchaseOrderProduct = new AMPurchaseOrderProduct()
                    {
                        Item = item.Item,
                        ItemId = item.ItemId,
                        PurchaseOrderId = entity.PurchaseOrderId,
                        Quantity = item.Quantity,
                    };
                    db.AMPurchaseOrderProducts.Add(puchaseOrderProduct);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception x)
            {
                var innerEx = x.InnerException;
                return false;
            }
        }

        [HttpGet]
        public ActionResult ManageRequests()
        {

            var vm = new ManageRequestsViewModel();
            vm.DepartmentsDD = from dept in db.Departments
                               where (dept.BranchId == branch_ID)
                               select new SelectListItem
                               {
                                   Text = dept.DepartmentName,
                                   Value = dept.DepartmentId.ToString()
                               };
            vm.EmployeeDD = from emp in db.Employees
                            where emp.BranchId == branch_ID
                            select new SelectListItem
                            {
                                Value = emp.EmployeeId.ToString(),
                                Text = emp.EmpName
                            };
            vm.ItemsDD = db.InvProducts.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true && u.IsFixedAsset == true).Select(s => new SelectListItem
            {
                Value = s.ProductId.ToString(),
                Text = s.ProductName.ToString()
            }).ToList();

            ViewBag.Statuses = from sts in db.InvRequestStatuses
                               select new SelectListItem()
                               {
                                   Text = sts.StatusName,
                                   Value = sts.StatusId.ToString()
                               };
            //vm.Requests = db.AMRequests.Include("Department").Include("Employee");
            
            //Code to add form action right and report formats, function is located in BLL
            string[] forms = { "AddNewRequests", "GoToRequestList" };
            Request_FAR.ManageRequests_FAR(db, Request.RawUrl, forms);

            //-------------------------
            vm = RequestList(vm);
            return View(vm);
        }


        [HttpPost]
        public JsonResult DeleteRequest(int id)
        {
            try
            {
                if (id > 0)
                {
                    if (db.InvPurchaseOrders.Where(w => w.RequestId == id).Any())
                    {
                        return Json(new { Error = "Please delete Purchase Order associated to this Request Number first." }, JsonRequestBehavior.AllowGet);
                    }
                    var request = db.InvRequests.Find(id);
                    var requestDetails = db.InvRequestDetails.Where(s => s.RequestId == id);
                    foreach (var item in requestDetails)
                    {
                        db.InvRequestDetails.Remove(item);
                    }
                    db.SaveChanges();
                    db.InvRequests.Remove(request);
                    db.SaveChanges();
                    return Json(new { Messages = "Successfully Deleted" }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { Error = "Request Number cannot be null or empty!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exs)
            {
                return Json(new { Error = "Some thing went wrong while processing your request,Please try again." }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult SearchRequests(ManageRequestsViewModel vm)
        {
            //vm.Requests = db.AMRequests.Include("Department").Include("Employee");
            //if (vm.RequestId != null)
            //    vm.Requests = vm.Requests.Where(s => s.RequestId == vm.RequestId);
            //if (vm.DepartmentId != null)
            //    vm.Requests = vm.Requests.Where(s => s.DepartmentId == vm.DepartmentId);
            //if (vm.EmployeeId != null)
            //    vm.Requests = vm.Requests.Where(s => s.EmployeeId == vm.EmployeeId);
            //if (vm.RequestId != null)
            //    vm.Requests = vm.Requests.Where(s => s.RequestId == vm.RequestId);
            //if (vm.RequestDate != DateTime.MinValue)
            //    vm.Requests = vm.Requests.Where(s => s.RequestDate.Month == vm.RequestDate.Month && s.RequestDate.Year == vm.RequestDate.Year && s.RequestDate.Day == vm.RequestDate.Day);
            //if (vm.StatusId != null)
            //    vm.Requests = vm.Requests.Where(s => s.StatusId == vm.StatusId);
            vm = RequestList(vm);
            return PartialView("_PartialManageRequests", vm);
        }

        ManageRequestsViewModel RequestList(ManageRequestsViewModel vm)
        {
            vm.Requests = (from request in db.InvRequests
                           join porder in db.InvPurchaseOrders on request.RequestId equals porder.RequestId into orders
                           from porder in orders.DefaultIfEmpty()
                           join emp in db.Employees on request.EmployeeId equals emp.EmployeeId into emps
                           from emp in emps.DefaultIfEmpty()
                           join status in db.InvRequestStatuses on request.StatusId equals status.StatusId into statuses
                           from status in statuses.DefaultIfEmpty()
                           join dept in db.Departments on request.DepartmentId equals dept.DepartmentId into depts
                           from dept in depts.DefaultIfEmpty()
                           select new AMRequestVM()
                           {
                               RequestId = request.RequestId,
                               DepartmentId = request.DepartmentId,
                               EmployeeId = request.EmployeeId,
                               StatusId = request.StatusId??0,
                               RequestDate = request.RequestDate,
                               Description = request.Description,
                               EmpName = emp.EmpName,
                               DepartmentName = dept.DepartmentName,
                               StatusName = status.StatusName,
                               PurchaseOrderId = porder.PurchaseOrderId
                           }).OrderByDescending(x=>x.RequestId).ToList();
            if (vm.RequestId != null)
                vm.Requests = vm.Requests.Where(s => s.RequestId == vm.RequestId);
            if (vm.DepartmentId != null)
                vm.Requests = vm.Requests.Where(s => s.DepartmentId == vm.DepartmentId);
            if (vm.EmployeeId != null)
                vm.Requests = vm.Requests.Where(s => s.EmployeeId == vm.EmployeeId);
            if (vm.RequestId != null)
                vm.Requests = vm.Requests.Where(s => s.RequestId == vm.RequestId);
            if (vm.RequestDate != DateTime.MinValue)
                vm.Requests = vm.Requests.Where(s => s.RequestDate.Month == vm.RequestDate.Month && s.RequestDate.Year == vm.RequestDate.Year && s.RequestDate.Day == vm.RequestDate.Day);
            if (vm.StatusId != null)
                vm.Requests = vm.Requests.Where(s => s.StatusId == vm.StatusId);
            return vm;
        }
        #endregion

        #region AddEditRequest

        public ActionResult AddEditRequest(Int64? id, string message)
        {
            RequestModel ex = new RequestModel();
            ex = AddEditRequest(ex, id);
            return View(ex);
        }

        [HttpPost]
        public ActionResult AddEditRequest(RequestModel ex, string Command)
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                ex = PostRequest(ex, Command);
                var invoice = new
                {
                    ex.Request.RequestId,
                    ex.Request.Description,
                    ex.PurchaseOrder,
                    ex.Request.StatusId
                };
                var detaillist = db.InvRequestDetails.Where(u => u.RequestId == ex.Request.RequestId).ToList().Select(u => new
                {
                    RequestDetailId = u.RequestDetailId,
                    RequestId = u.RequestId,
                    Description = u.Description ?? "",
                    RoomNumber = u.RoomId == null ? "" : u.RoomId.ToString(),
                    u.Quantity,
                    ItemName = u.Item == null ? "" : u.Item.ProductName
                }).ToList();
                List<Service.Helper> PartialList = new List<Service.Helper>();
                PartialList.Add(new Service.Helper { divToReplace = "", IdToHideModalPopup = "ServiceModal" });

                var data = new
                {
                    IsSuccess = ViewBag.result,
                    Case = Command,
                    IsCancelled = Convert.ToBoolean(ViewBag.IsCancelled) == true ? true : false,
                    IsPosted = Convert.ToBoolean(ViewBag.IsPosted) == true ? true : false,
                    Modification = ViewBag.Modification,
                    RequestId = ViewBag.RequestId,

                    Detail = detaillist,
                    Receipts = DBNull.Value.ToString(),
                    Invoice = invoice,
                    ErrorMsg = ViewBag.error,
                    SuccessMsg = ViewBag.success,
                    PartialList = PartialList
                };
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            else
            {
                switch (Command)
                {
                    case "LoadInvoice":
                        if (ex.InvoiceNo != null)
                        {
                            var prevId = ex.Request.RequestId;
                            var isExists = db.InvRequests.Where(u => u.RequestId == ex.InvoiceNo).FirstOrDefault();
                            if (isExists != null)
                                ex = AddEditRequest(ex, isExists.RequestId);
                            else
                            {
                                ViewBag.Cancelled = "Invoice does not exist";
                                ex = AddEditRequest(ex, prevId);
                            }
                        }
                        else
                            ex = AddEditRequest(ex, ex.Request.RequestId);

                        break;
                }
                ModelState.Clear();
                return View(ex);
            }
        }

        RequestModel AddEditRequest(RequestModel ex, Int64? id = 0)
        {
            ex.InvoiceNo = id;
            FillDD_New();
            ViewBag.EditMode = false;
            if (id > 0)
            {
                ex.Request = db.InvRequests.Where(u => u.RequestId == id).FirstOrDefault();
                if (ex.Request != null)
                {
                    ViewBag.EditMode = true;
                    ViewBag.ModifiedBy = db.Users.FirstOrDefault(x => x.UserID == ex.Request.ModifiedBy)?.Username;
                    ViewBag.CreatedBy = db.Users.FirstOrDefault(x => x.UserID == ex.Request.CreatedBy)?.Username;
                    ex.PurchaseOrder = new InvPurchaseOrder();
                    ex.PurchaseOrder.PurchaseOrderId = db.InvPurchaseOrders
                .Where(u => u.RequestId == ex.Request.RequestId).Select(u => u.PurchaseOrderId).FirstOrDefault();
                }
                ex.RequestDetail = db.InvRequestDetails.Where(u => u.RequestId == id).ToList();
            }
            else
            {
                ex.Request = new InvRequest();
                ex.Request.RequestDate = DateTime.Now;
            }
            return ex;
        }

        void FillDD_New()
        {
            ViewBag.Items = db.InvProducts.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true).Select(s => new SelectListItem
            {
                Value = s.ProductId.ToString(),
                Text = s.ProductName.ToString()
            }).ToList();

            ViewBag.Departments = from dept in db.Departments
                                  where dept.BranchId == branch_ID
                                  select new SelectListItem
                                  {
                                      Text = dept.DepartmentName,
                                      Value = dept.DepartmentId.ToString()
                                  };
            ViewBag.Employees = from emp in db.Employees
                                where emp.BranchId == branch_ID && emp.Active == true
                                select new SelectListItem
                                {
                                    Value = emp.EmployeeId.ToString(),
                                    Text = emp.EmpName
                                };
            ViewBag.Statuses = from sts in db.InvRequestStatuses
                               select new SelectListItem()
                               {
                                   Text = sts.StatusName,
                                   Value = sts.StatusId.ToString()
                               };
            ViewBag.Rooms = from dept in db.CompanyRooms
                            where dept.BranchId == branch_ID
                            select new SelectListItem
                            {
                                Text = dept.Floors.Buildings.BuildingName + ">" + dept.Floors.FloorName + ">" + dept.RoomDoorNo,
                                Value = dept.RoomId.ToString()
                            };
            ViewBag.Suppliers = db.Clients.Where(u => u.BranchId == branch_ID && u.IsSupplier == true).OrderBy(u => u.Name).ToList();

            ViewBag.ConditionTypesDD = from cond in db.InvConditionTypes
                                       select new SelectListItem
                                       {
                                           Text = cond.Name,
                                           Value = cond.ConditionTypeId.ToString()
                                       };

        }

        public JsonResult ItemIdByBarcode(string barcode)
        {
            var id = db.InvProducts.Where(u => u.Barcode == barcode && u.IsInventoryItem == true && u.IsFixedAsset == true).Select(u => u.ProductId).FirstOrDefault();
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        RequestModel PostRequest(RequestModel ex, string Command)
        {
            ViewBag.result = true;
            if (ex.Request != null)
            {
                switch (Command)
                {
                    case "InsertUpdate":
                        if (ex.Request != null)
                        {
                            if (ex.Request.RequestId > 0) // Updation
                            {
                                var sale = db.InvRequests.Where(u => u.RequestId == ex.Request.RequestId).FirstOrDefault();
                                if (Request_Update(ex.Request))
                                {
                                    if (ex.RequestDetail != null)
                                    {
                                        foreach (var item in ex.RequestDetail)
                                        {
                                            if (item.RequestDetailId > 0)
                                                RequestDetail_Update(item);
                                            else
                                                RequestDetail_Insert(ex.Request.RequestId, item);
                                        }
                                    }
                                    else
                                        ViewBag.result = false;
                                }
                                else
                                    ViewBag.result = false;
                            }
                            else // Insertion
                            {
                                ex.Request.RequestId = Request_Insert(ex.Request);
                                if (ex.Request.RequestId > 0)
                                {
                                    if (ex.RequestDetail != null)
                                    {
                                        foreach (var item in ex.RequestDetail)
                                        {
                                            if (item.RequestDetailId > 0)
                                                RequestDetail_Update(item);
                                            else
                                                RequestDetail_Insert(ex.Request.RequestId, item);
                                        }
                                    }
                                    else
                                        ViewBag.result = false;

                                }
                                else
                                    ViewBag.result = false;
                            }
                        }
                        else
                            ViewBag.result = false;
                        break;
                    case "savePurchaseOrder":
                        if (ex.Request != null)
                        {
                            if (ex.Request.RequestId > 0 && ex.PurchaseOrder != null) // update Service
                            {
                                if (ConvertRequestToPurchaseOrder(ex.Request.RequestId, ex.PurchaseOrder))
                                    ViewBag.success = "Purchase order created succesfully with ref # " + ViewBag.RequestId;
                                else
                                    ViewBag.result = false;
                            }
                            else
                            {
                                ViewBag.error = "Request not found, Purchase order creation failed";
                                ViewBag.result = false;
                            }
                        }
                        else
                        {
                            ViewBag.error = "Request not found, Purchase order creation failed";
                            ViewBag.result = false;
                        }
                        break;
                }

            }
            else
                ViewBag.result = false;
            if (ViewBag.result)
            {
                ex.Request = db.InvRequests.Where(u => u.RequestId == ex.Request.RequestId).FirstOrDefault();
                ex.RequestDetail = db.InvRequestDetails.Where(u => u.RequestId == ex.Request.RequestId).ToList();
                ViewBag.RequestId = ex.Request.RequestId;
                var ModifiedBy = db.Users.Where(u => u.UserID == ex.Request.ModifiedBy).Select(u => u.Username).FirstOrDefault();
                var CreatedBy = db.Users.Where(u => u.UserID == ex.Request.CreatedBy).Select(u => u.Username).FirstOrDefault();
                var Modification = "";
                if (!string.IsNullOrEmpty(CreatedBy))
                    Modification = "Created By: " + CreatedBy;
                if (!string.IsNullOrEmpty(ModifiedBy))
                    Modification = Modification + "        Modified By: " + ModifiedBy;
                ViewBag.Modification = Modification;
                //ViewBag.Received = db.PosClientInvoicePayments.Where(u => u.RequestId == ex.Request.RequestId && u.BranhId == branch_ID).Sum(u => u.Amount);
            }
            return ex;
        }

        public int Request_Insert(InvRequest objRequest)
        {
            InvRequest _Request = new InvRequest();
            _Request.RequestDate = objRequest.RequestDate;
            if (string.IsNullOrEmpty(objRequest.Description))
                _Request.Description = objRequest.Description;
            else
             _Request.Description = objRequest.Description;
            _Request.EmployeeId = objRequest.EmployeeId;
            _Request.DepartmentId = objRequest.DepartmentId;
            _Request.RequestedBy = objRequest.RequestedBy;
            _Request.StatusId = objRequest.StatusId;
            _Request.BranchId = branch_ID;
            _Request.CreatedBy = SessionHelper.UserID;
            _Request.CreatedOn = DateTime.Now;
            //_Request.IpAddress = SessionHelper.IP;
            db.InvRequests.Add(_Request);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return _Request.RequestId;
        }

        public bool Request_Update(InvRequest objRequest)
        {
            bool res = false;
            if (objRequest.RequestId > 0)
            {
                InvRequest _Request = db.InvRequests.Where(u => u.RequestId == objRequest.RequestId).FirstOrDefault();
                if (_Request != null)
                {
                    _Request.Description = objRequest.Description ?? "";
                    _Request.EmployeeId = objRequest.EmployeeId;
                    _Request.DepartmentId = objRequest.DepartmentId;
                    _Request.RequestedBy = objRequest.RequestedBy;
                    _Request.StatusId = objRequest.StatusId;
                    _Request.ModifiedBy = SessionHelper.UserID;
                    _Request.ModifiedOn = DateTime.Now;
                    db.Entry(_Request).State = EntityState.Modified;
                    db.SaveChanges();
                    res = true;
                }
            }
            return res;
        }

        public Int64 RequestDetail_Insert(int RequestId, InvRequestDetail objRequestDetail)
        {
            if (objRequestDetail.ProductId > 0)
            {
                InvRequestDetail _RequestDetail = objRequestDetail;
                _RequestDetail.RequestId = RequestId;
                _RequestDetail.ProductId = objRequestDetail.ProductId;
                _RequestDetail.Quantity = objRequestDetail.Quantity;
                _RequestDetail.Price = objRequestDetail.Price;
                _RequestDetail.Tax = objRequestDetail.Tax;
                _RequestDetail.TotalAmount = objRequestDetail.TotalAmount;
                _RequestDetail.Description = objRequestDetail.Description;
                _RequestDetail.RoomId = objRequestDetail.RoomId;
                _RequestDetail.ConditionTypeId = objRequestDetail.ConditionTypeId;
                _RequestDetail.CreatedBy = SessionHelper.UserID;
                _RequestDetail.CreatedOn = DateTime.Now;
                db.InvRequestDetails.Add(_RequestDetail);
                db.SaveChanges();
            }
            return RequestId;
        }

        public bool RequestDetail_Update(InvRequestDetail objRequestDetail)
        {
            bool res = false;
            if (objRequestDetail.RequestDetailId > 0)
            {
                InvRequestDetail _RequestDetail = db.InvRequestDetails.Where(u => u.RequestDetailId == objRequestDetail.RequestDetailId).FirstOrDefault();
                if (_RequestDetail != null)
                {
                    _RequestDetail.ProductId = objRequestDetail.ProductId;
                    _RequestDetail.Quantity = objRequestDetail.Quantity;
                    _RequestDetail.Price = objRequestDetail.Price;
                    _RequestDetail.Tax = objRequestDetail.Tax;
                    _RequestDetail.TotalAmount = objRequestDetail.TotalAmount;
                    _RequestDetail.Description = objRequestDetail.Description;
                    _RequestDetail.RoomId = objRequestDetail.RoomId;
                    _RequestDetail.ConditionTypeId = objRequestDetail.ConditionTypeId;
                    _RequestDetail.ModifiedBy = SessionHelper.UserID;
                    _RequestDetail.ModifiedOn = DateTime.Now;
                    db.Entry(_RequestDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    res = true;
                }
            }
            return res;
        }

        public ActionResult LoadPurchaseOrder(long? InvoiceNo = 0)
        {
            var list = db.InvPurchaseOrders
                .Where(u => u.RequestId == InvoiceNo).Select(u => u.PurchaseOrderId).FirstOrDefault();
            return Json(list.ToString(), JsonRequestBehavior.AllowGet);
        }

        bool ConvertRequestToPurchaseOrder(int RequestId, InvPurchaseOrder objPurchaseOrder)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var _Request = db.InvRequests.Find(RequestId);
                    if (_Request != null)
                    {
                        InvPurchaseOrder _PurchaseOrder = new InvPurchaseOrder();
                        var code = db.InvPurchaseOrders.Max(s => s.PurchaseOrderCode);
                        _PurchaseOrder.PurchaseOrderCode = StringFormate.CreatePurchaseCode(code);
                        _PurchaseOrder.PurchaseOrderDate = _Request.RequestDate;
                        _PurchaseOrder.SupplierId = objPurchaseOrder.SupplierId;
                        _PurchaseOrder.StatusId = 1;
                        _PurchaseOrder.RequestId = _Request.RequestId;
                        _PurchaseOrder.Description = objPurchaseOrder.Description;
                        _PurchaseOrder.CreatedBy = SessionHelper.UserID;
                        _PurchaseOrder.CreatedOn = DateTime.Now;
                        db.InvPurchaseOrders.Add(_PurchaseOrder);
                        var _RequestDetail = db.InvRequestDetails.Where(u => u.RequestId == RequestId).ToList();
                        if (_RequestDetail != null)
                        {
                            InvPurchaseOrderProduct _PurchaseOrderProduct;
                            foreach (var item in _RequestDetail)
                            {
                                _PurchaseOrderProduct = new InvPurchaseOrderProduct();
                                _PurchaseOrderProduct.PurchaseOrderId = _PurchaseOrder.PurchaseOrderId;
                                _PurchaseOrderProduct.ProductId = item.ProductId;
                                _PurchaseOrderProduct.Description = item.Description;
                                _PurchaseOrderProduct.Quantity = item.Quantity;
                                _PurchaseOrderProduct.ConditionTypeId = item.ConditionTypeId;
                                _PurchaseOrderProduct.UnitPrice = Convert.ToDecimal(item.Item?.CostPrice);
                                _PurchaseOrderProduct.LineTotal = Convert.ToDecimal(item.Item?.CostPrice) * item.Quantity;
                                db.InvPurchaseOrderProducts.Add(_PurchaseOrderProduct);
                            }
                            db.SaveChanges();
                            trans.Commit();
                            ViewBag.RequestId = _PurchaseOrder.RequestId;
                            objPurchaseOrder.PurchaseOrderId = _PurchaseOrder.PurchaseOrderId;
                            return true;
                        }
                        else
                        {
                            ViewBag.error = "Request product(s) not found, Purchase Order creation failed";
                            return false;
                        }
                    }
                    else
                    {
                        ViewBag.error = "Request not found, Purchase Order creation failed";
                        return false;
                    }
                }
                catch
                {
                    throw;
                }
            }

        }


        #endregion

    }
}