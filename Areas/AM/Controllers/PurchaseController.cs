using FAPP.Areas.AM.BLL;
using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using SupplierPayment = FAPP.Areas.Contact.Models.SupplierPayment;
using SupplierInvoicePayment = FAPP.Areas.Contact.Models.SupplierInvoicePayment;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
using FAPP.INV.Models;

namespace FAPP.Areas.AM.Controllers
{
    public class PurchaseController : FAPP.Controllers.BaseController
    {
        //OneDbContext db = new OneDbContext();
        short branch_ID = Convert.ToInt16(SessionHelper.BranchId);

        bool reIntialize = false;

        PurchaseInvoiceBL purchaseBl = new PurchaseInvoiceBL();

        public ActionResult Index()
        {
            GraphsModel ex = new GraphsModel();
            DateTime today = DateTime.Now.Date;
            ex.Action = ControllerContext.RouteData.Values["action"].ToString();
            ex.Contoller = ControllerContext.RouteData.Values["Controller"].ToString();
            ex.Area = ControllerContext.RouteData.DataTokens["area"].ToString();
            ex.Module = ControllerContext.RouteData.DataTokens["area"].ToString();
            var url = "";
            if (ex.Area != null)
                url += "/" + ex.Area;
            if (ex.Contoller != null)
                url += "/" + ex.Contoller;
            if (ex.Action != null)
                url += "/" + ex.Action;
            ex.Url = url;
            return View(ex);
        }

        #region AddEditPurchaseInvoice

        public ActionResult AddEditPurchaseInvoice(Int64? id, string message)
        {

            PurchaseModelViewModel ex = new PurchaseModelViewModel();
            ex = GetPurchaseInvoice(ex, id);
            ViewBag.Cancelled = message;
            return View(ex);
        }

        PurchaseModelViewModel GetPurchaseInvoice(PurchaseModelViewModel ex, Int64? id = 0)
        {
            if (reIntialize)
            {
                db = new OneDbContext();
            }

            ex.InvoiceNo = id;
            ViewBag.PurchaseInvoiceId = id;
            FillDD();
            ViewBag.EditMode = false;
            if (id > 0)
            {
                ex.PurchaseInvoice = db.InvPurchaseInvoices.Where(u => u.PurchaseInvoiceId == id).FirstOrDefault();
                if (ex.PurchaseInvoice != null)
                {
                    ViewBag.IsCancelled = ex.PurchaseInvoice.IsCancelled;
                    if (ex.PurchaseInvoice.IsCancelled)
                    {
                        ViewBag.Cancelled = "Invoice has been cancelled by " + db.Users.Where(u => u.UserID == ex.PurchaseInvoice.CancelledBy).Select(u => u.Username).FirstOrDefault() + " on " + ex.PurchaseInvoice.CancelledOn.Value.ToShortDateString();
                    }

                    ViewBag.IsPosted = ex.PurchaseInvoice.IsPosted;
                    if (ex.PurchaseInvoice.IsPosted)
                    {
                        ViewBag.success = "Invoice has been Posted by " + db.Users.Where(u => u.UserID == ex.PurchaseInvoice.PostedBy).Select(u => u.Username).FirstOrDefault() + " on " + ex.PurchaseInvoice.PostedOn.Value.ToShortDateString();
                    }

                    ViewBag.EditMode = true;
                    ViewBag.ModifiedBy = db.Users.Where(u => u.UserID == ex.PurchaseInvoice.ModifiedBy).Select(u => u.Username).FirstOrDefault();
                    ViewBag.CreatedBy = db.Users.Where(u => u.UserID == ex.PurchaseInvoice.CreatedBy).Select(u => u.Username).FirstOrDefault();
                }
                ex.PurchaseInvoiceProduct = db.AMPurchaseInvoiceProducts.Where(u => u.PurchaseInvoiceId == id).ToList();
                //ex.IsFixedAsset = IsFixedAssetItem(ex.PurchaseInvoiceProduct.ItemId);
            }
            else
            {
                ex.PurchaseInvoice = new Model.AMPurchaseInvoice();
                ex.PurchaseInvoice.PurchaseInvoiceDate = DateTime.Now;
            }
            return ex;
        }

        [HttpPost]
        public ActionResult AddEditPurchaseInvoice(PurchaseModelViewModel ex, string Command)
        {
            switch (Command)
            {
                case "LoadInvoice":
                    if (ex.InvoiceNo != null)
                    {
                        var prevId = ex.PurchaseInvoice.PurchaseInvoiceId;
                        var isExists = db.AMPurchaseInvoices.Where(u => u.PurchaseInvoiceId == ex.InvoiceNo).FirstOrDefault();
                        if (isExists != null)
                        {
                            ex = GetPurchaseInvoice(ex, isExists.PurchaseInvoiceId);
                        }
                        else
                        {
                            ViewBag.Cancelled = "Invoice does not exist";
                            ex = GetPurchaseInvoice(ex, prevId);
                        }
                    }
                    else
                    {
                        ex = GetPurchaseInvoice(ex, ex.PurchaseInvoice.PurchaseInvoiceId);
                    }

                    break;
            }
            return View(ex);
        }

        [HttpPost]
        public ActionResult PostPurchaseInvoice(AMPurchaseModel ex, string Command)
        {
            reIntialize = true;
            ViewBag.Case = Command;
            ex = PostPurchase(ex, Command);
            var vm = new PurchaseModelViewModel();
            vm = GetPurchaseInvoice(vm, ex.PurchaseInvoice.PurchaseInvoiceId);
            List<Service.Helper> PartialList = new List<Service.Helper>();
            PartialList.Add(new Service.Helper { divToReplace = "UpdatePartial", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialAddEditPI", vm) });
            return Json(new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialAddEditPI", vm),
                IsSuccess = ViewBag.result,
                PurchaseInvoiceId = vm.PurchaseInvoice.PurchaseInvoiceId,
                status = Convert.ToBoolean(ViewBag.result),
                HideMsg = true,
                Case = Command,
                IsPosted = vm.PurchaseInvoice.IsPosted,
                GridId = "UpdatePartial",
                ModalId = "AddPaymentModal"

            }, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public ActionResult PostPurchaseInvoice(Model.AMPurchaseModel ex, string Command)
        //{
        //    reIntialize = true;
        //    ViewBag.Case = Command;
        //    ex = PostPurchase(ex, Command);
        //    var vm = new PurchaseModelViewModel();
        //    vm = GetPurchaseInvoice(vm, ex.PurchaseInvoice.PurchaseInvoiceId);
        //    List<Service.Helper> PartialList = new List<Service.Helper>();
        //    PartialList.Add(new Service.Helper { IdToHideModalPopup = "AddPaymentModal", divToReplace = "UpdatePartial", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialAddEditPI", vm) });
        //    return Json(new
        //    {
        //        IsSuccess = ViewBag.result,
        //        PurchaseInvoiceId = ViewBag.PurchaseInvoiceId,
        //        status = Convert.ToBoolean(ViewBag.result),
        //        HideMsg = true,
        //        Case = Command,
        //        IsPosted= ex.PurchaseInvoice.IsPosted,
        //        PartialList
        //    }, JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult FetchPIPD(long id)
        {
            var vm = new PurchaseModelViewModel();
            vm.PIPDs = from detail in db.AMPurchaseInvoiceProductDetails
                       join pIP in db.AMPurchaseInvoiceProducts on detail.PurchaseInvoiceProductId equals pIP.PurchaseInvoiceProductId
                       join item in db.AMItems on pIP.ItemId equals item.ItemId
                       where pIP.PurchaseInvoiceProductId == id
                       select new PurchaseInvoiceProductDetailVM
                       {
                           PIPDId = detail.DetailId,
                           ItemCode = detail.ItemCode,
                           ItemName = item.ItemName,
                           Qty = (int)detail.Qty
                       };
            return PartialView("_PartialPIPDModal", vm);
        }

        void FillDD()
        {
            //ViewBag.Currency = db.Currencies.ToList();
            ViewBag.Suppliers = db.Clients.Where(u => u.BranchId == branch_ID && u.IsSupplier == true).ToList();
            ViewBag.Products = db.AMItems.Where(u => u.BranchId == branch_ID)
                               .Select(s => new
                               {
                                   s.ItemId,
                                   s.ItemName,
                                   s.ShortName,
                                   ItemNameWithPrice = s.ItemName + "-" + s.Price,
                                   s.Price
                               }).ToList();
            ViewBag.Warehouses = db.AMWarehouses.Where(u => u.BranchId == branch_ID).ToList();
            ViewBag.Departments = db.Departments.Where(x => x.BranchId == branch_ID).ToList();
            List<SelectListItem> PaymentMode = new List<SelectListItem>();
            PaymentMode.Add(new SelectListItem() { Text = "Cash", Value = "Cash" });
            PaymentMode.Add(new SelectListItem() { Text = "Bank", Value = "Bank" });
            ViewBag.PaymentMode = PaymentMode;
            ViewBag.CashAccount = ProceduresModel.p_mnl_Account_GetCashAndBankAccounts(db, true, false, branch_ID, null).ToList();
            ViewBag.BankAccount = ProceduresModel.p_mnl_Account_GetCashAndBankAccounts(db, false, true, branch_ID, null).ToList();
            var ConditionType = db.AMConditionTypes.FirstOrDefault();
            if (ConditionType == null)
            {
                db.Database.ExecuteSqlCommand($@"
                                                Insert into am.ConditionTypes(ConditionTypeId,Name)
                                                values(1,'New')
                                                Insert into am.ConditionTypes(ConditionTypeId,Name)
                                                values(2,'Used')
                                                Insert into am.ConditionTypes(ConditionTypeId,Name)
                                                values(3,'Damaged')
                                                Insert into am.ConditionTypes(ConditionTypeId,Name)
                                                values(4,'Renewd')");
            }
            ViewBag.ConditionTypesDD = from cond in db.AMConditionTypes
                                       select new SelectListItem
                                       {
                                           Text = cond.Name,
                                           Value = cond.ConditionTypeId.ToString()
                                       };
        }


        public JsonResult ProductIdByBarcode(string barcode)
        {
            var purchaseInvoiceBL = new BLL.PurchaseInvoiceBL();
            var id = purchaseInvoiceBL.GetProductIdByBarcode(db, barcode);
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductDetailById(int? id = 0)
        {
            bool result = false;
            var purchaseInvoiceBl = new BLL.PurchaseInvoiceBL();
            var productdata = purchaseInvoiceBl.GetProductDetailById(db, id);
            if (productdata != null)
            {
                result = true;
            }

            var data = new
            {
                IsSuccess = result,
                Data = productdata
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostToStock(AMPurchaseModel ex)
        {


            return View();
        }

        public ActionResult PostToAccounts(AMPurchaseModel ex)
        {
            var purchaseInvoiceBL = new BLL.PurchaseInvoiceBL();
            purchaseInvoiceBL.PostToAccounts();
            return View();
        }

        public ActionResult UnpostFromAccounts(AMPurchaseModel ex)
        {


            return View();
        }

        public ActionResult UnpostFromStock(AMPurchaseModel ex)
        {


            return View();
        }

        //Model.AMPurchaseModel PostPurchase(Model.AMPurchaseModel ex, string Command)
        //{
        //    ViewBag.result = true;
        //    ViewBag.success = "Updated successfully";
        //    using (var trans = db.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            if (ex.PurchaseInvoice != null)
        //            {
        //                switch (Command)
        //                {
        //                    case "InsertUpdate":
        //                        if (ex.PurchaseInvoice != null)
        //                        {
        //                            if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Updation
        //                            {
        //                                var purchase = db.AMPurchaseInvoices.Where(u => u.PurchaseInvoiceId == ex.PurchaseInvoice.PurchaseInvoiceId).FirstOrDefault();
        //                                if (!purchase.IsPosted && !purchase.IsCancelled)
        //                                {
        //                                    if (PurchaseInvoice_Update(ex.PurchaseInvoice))
        //                                    {
        //                                        if (ex.PurchaseInvoiceProduct != null)
        //                                        {
        //                                            foreach (var item in ex.PurchaseInvoiceProduct)
        //                                            {
        //                                                if (item.PurchaseInvoiceProductId > 0)
        //                                                {
        //                                                    PurchaseInvoiceProduct_Update(item);
        //                                                }
        //                                                else
        //                                                {
        //                                                    PurchaseInvoiceProduct_Insert(ex.PurchaseInvoice.PurchaseInvoiceId, item);
        //                                                }
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            ViewBag.result = false;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        ViewBag.result = false;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    ViewBag.result = false;
        //                                }
        //                            }
        //                            else // Insertion
        //                            {
        //                                ex.PurchaseInvoice.PurchaseInvoiceId = PurchaseInvoice_Insert(ex.PurchaseInvoice);
        //                                ViewBag.PurchaseInvoiceId = ex.PurchaseInvoice.PurchaseInvoiceId;
        //                                if (ex.PurchaseInvoice.PurchaseInvoiceId > 0)
        //                                {
        //                                    if (ex.PurchaseInvoiceProduct != null)
        //                                    {
        //                                        foreach (var item in ex.PurchaseInvoiceProduct)
        //                                        {
        //                                            if (item.PurchaseInvoiceProductId > 0)
        //                                            {
        //                                                PurchaseInvoiceProduct_Update(item);
        //                                            }
        //                                            else
        //                                            {
        //                                                PurchaseInvoiceProduct_Insert(ex.PurchaseInvoice.PurchaseInvoiceId, item);
        //                                            }
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        ViewBag.result = false;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    ViewBag.result = false;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ViewBag.result = false;
        //                        }

        //                        break;
        //                    case "PostVoucher":
        //                        if (ex.PurchaseInvoice != null)
        //                        {
        //                            if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Posting
        //                            {
        //                                ex.PurchaseInvoice.IsPosted = true;
        //                                if (!PurchaseInvoice_Update(ex.PurchaseInvoice))
        //                                {
        //                                    ViewBag.result = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                ViewBag.result = false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ViewBag.result = false;
        //                        }

        //                        break;
        //                    case "UnPostVoucher":
        //                        if (ex.PurchaseInvoice != null)
        //                        {
        //                            if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Posting
        //                            {
        //                                ex.PurchaseInvoice.IsPosted = false;
        //                                if (!PurchaseInvoice_Update(ex.PurchaseInvoice))
        //                                {
        //                                    ViewBag.result = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                ViewBag.result = false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ViewBag.result = false;
        //                        }

        //                        break;
        //                    case "CancelVoucher":
        //                        if (ex.PurchaseInvoice != null)
        //                        {
        //                            if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Cancellation
        //                            {
        //                                ex.PurchaseInvoice.IsCancelled = true;
        //                                if (!PurchaseInvoice_Update(ex.PurchaseInvoice))
        //                                {
        //                                    ViewBag.result = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                ViewBag.result = false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ViewBag.result = false;
        //                        }

        //                        break;
        //                    case "UnCancelVoucher":
        //                        if (ex.PurchaseInvoice != null)
        //                        {
        //                            if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Cancellation
        //                            {
        //                                ex.PurchaseInvoice.IsCancelled = false;
        //                                if (!PurchaseInvoice_Update(ex.PurchaseInvoice))
        //                                {
        //                                    ViewBag.result = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                ViewBag.result = false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ViewBag.result = false;
        //                        }

        //                        break;
        //                    case "savePayment":
        //                        if (ex.PurchaseInvoice != null)
        //                        {
        //                            if (ex.PurchaseInvoice.PurchaseInvoiceId > 0 && ex.SupplierInvoicePayment != null) // SavePayment
        //                            {
        //                                Model.AMSupplierPayment _SupplierPayment = new Model.AMSupplierPayment();
        //                                string cashAccount = "";
        //                                if (ex.PaymentType == "Cash")
        //                                {
        //                                    cashAccount = ex.CashAccountId;
        //                                }
        //                                else
        //                                {
        //                                    cashAccount = ex.BankAccountId;
        //                                }

        //                                if (!string.IsNullOrEmpty(cashAccount))
        //                                {
        //                                    _SupplierPayment.AccountId = cashAccount;
        //                                    _SupplierPayment.Amount = ex.SupplierInvoicePayment.Amount;
        //                                    _SupplierPayment.SupplierId = Convert.ToInt16(ex.PurchaseInvoice.SupplierId);
        //                                    _SupplierPayment.PaymentType = "Cash";
        //                                    _SupplierPayment.Description = ex.SupplierInvoicePayment.Description;
        //                                    _SupplierPayment.CreatedOn = ex.SupplierInvoicePayment.CreatedOn;
        //                                    _SupplierPayment.PaymentDate = ex.SupplierInvoicePayment.PaymentDate;

        //                                    _SupplierPayment.SupplierPaymentId = db.AMSupplierInvoicePayments.Where(u => u.PurchaseInvoiceId == ex.PurchaseInvoice.PurchaseInvoiceId).Select(u => u.SupplierPaymentId).FirstOrDefault() ?? 0;
        //                                    var SupplierPaymentId = InsertUpdateSupplierPayments(_SupplierPayment);
        //                                    if (SupplierPaymentId > 0 && ex.SupplierInvoicePayment.Amount > 0)
        //                                    {
        //                                        AMSupplierInvoicePayment _SupplierInvoicePayment;
        //                                        _SupplierInvoicePayment = new AMSupplierInvoicePayment();
        //                                        _SupplierInvoicePayment.SupplierPaymentId = SupplierPaymentId;
        //                                        _SupplierInvoicePayment.PurchaseInvoiceId = ex.PurchaseInvoice.PurchaseInvoiceId;
        //                                        _SupplierInvoicePayment.Amount = _SupplierPayment.Amount;
        //                                        _SupplierInvoicePayment.CreatedOn = _SupplierPayment.CreatedOn;
        //                                        _SupplierInvoicePayment.PaymentDate = _SupplierPayment.PaymentDate;
        //                                        _SupplierInvoicePayment.Description = _SupplierPayment.Description;
        //                                        SaveSupplierInvoicePayments(_SupplierInvoicePayment, cashAccount);
        //                                        ViewBag.success = "Payment saved successfully";

        //                                    }
        //                                    else
        //                                    {
        //                                        ViewBag.result = false;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    ViewBag.result = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                ViewBag.result = false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ViewBag.result = false;
        //                        }

        //                        break;
        //                    case "saveIssueItem":
        //                        if (ex.PurchaseInvoice != null)
        //                        {
        //                            if (ex.PurchaseInvoice.PurchaseInvoiceId > 0 && ex.IssuedItem != null) // update Service
        //                            {
        //                                if (ConvertPurchaseInvoiceToIssuedItems(ex.PurchaseInvoice.PurchaseInvoiceId, ex.IssuedItem))
        //                                {
        //                                    ViewBag.success = "Purchase invoice issued succesfully with ref # " + ViewBag.IssuedItemId;
        //                                }
        //                                else
        //                                {
        //                                    ViewBag.result = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                ViewBag.error = "Purchase invoice not found, conversion failed";
        //                                ViewBag.result = false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            ViewBag.error = "Purchase invoice not found, conversion failed";
        //                            ViewBag.result = false;
        //                        }
        //                        break;
        //                }

        //            }
        //            else
        //            {
        //                ViewBag.result = false;
        //            }

        //            if (ViewBag.result)
        //            {
        //                trans.Commit();
        //                ViewBag.result = true;
        //            }
        //        }
        //        catch (Exception exc)
        //        {
        //            var str = ExtensionMethods.GetExceptionMessages(exc);
        //            if (str.Contains("Deletion"))
        //            {
        //                str = "Can't change quantity, It is used in issue items";
        //            }

        //            ViewBag.error = str;
        //            ViewBag.result = false;
        //        }
        //    }
        //    return ex;
        //}

        AMPurchaseModel PostPurchase(AMPurchaseModel ex, string Command)
        {
            ViewBag.result = true;
            ViewBag.success = "Updated successfully";
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (ex.PurchaseInvoice != null)
                    {
                        switch (Command)
                        {
                            case "InsertUpdate":
                                if (ex.PurchaseInvoice != null)
                                {
                                    if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Updation
                                    {
                                        var purchase = db.AMPurchaseInvoices.Where(u => u.PurchaseInvoiceId == ex.PurchaseInvoice.PurchaseInvoiceId).FirstOrDefault();
                                        if (!purchase.IsPosted && !purchase.IsCancelled)
                                        {
                                            if (PurchaseInvoice_Update(ex.PurchaseInvoice))
                                            {
                                                if (ex.PurchaseInvoiceProduct != null)
                                                {
                                                    foreach (var item in ex.PurchaseInvoiceProduct)
                                                    {
                                                        if (item.PurchaseInvoiceProductId > 0)
                                                        {
                                                            PurchaseInvoiceProduct_Update(item);
                                                        }
                                                        else
                                                        {
                                                            PurchaseInvoiceProduct_Insert(ex.PurchaseInvoice.PurchaseInvoiceId, item);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    ViewBag.result = false;
                                                }
                                            }
                                            else
                                            {
                                                ViewBag.result = false;
                                            }
                                        }
                                        else
                                        {
                                            ViewBag.result = false;
                                        }
                                    }
                                    else // Insertion
                                    {
                                        ex.PurchaseInvoice.PurchaseInvoiceId = PurchaseInvoice_Insert(ex.PurchaseInvoice);
                                        if (ex.PurchaseInvoice.PurchaseInvoiceId > 0)
                                        {
                                            if (ex.PurchaseInvoiceProduct != null)
                                            {
                                                foreach (var item in ex.PurchaseInvoiceProduct)
                                                {
                                                    if (item.PurchaseInvoiceProductId > 0)
                                                    {
                                                        PurchaseInvoiceProduct_Update(item);
                                                    }
                                                    else
                                                    {
                                                        PurchaseInvoiceProduct_Insert(ex.PurchaseInvoice.PurchaseInvoiceId, item);
                                                    }
                                                }



                                            }
                                            else
                                            {
                                                ViewBag.result = false;
                                            }
                                        }
                                        else
                                        {
                                            ViewBag.result = false;
                                        }
                                    }
                                }
                                else
                                {
                                    ViewBag.result = false;
                                }

                                break;
                            case "PostVoucher":
                                if (ex.PurchaseInvoice != null)
                                {
                                    if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Posting
                                    {
                                        if (!PostSingleInvoice(ex.PurchaseInvoice.PurchaseInvoiceId))
                                        {
                                            ViewBag.result = false;
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.result = false;
                                    }
                                }
                                else
                                {
                                    ViewBag.result = false;
                                }

                                break;
                            case "UnPostVoucher":
                                if (ex.PurchaseInvoice != null)
                                {
                                    if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Posting
                                    {
                                        if (!UnpostSingleInvoice(ex.PurchaseInvoice.PurchaseInvoiceId))
                                        {
                                            ViewBag.result = false;
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.result = false;
                                    }
                                }
                                else
                                {
                                    ViewBag.result = false;
                                }

                                break;
                            case "CancelVoucher":
                                if (ex.PurchaseInvoice != null)
                                {
                                    if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Cancellation
                                    {
                                        ex.PurchaseInvoice.IsCancelled = true;
                                        if (!PurchaseInvoice_Update(ex.PurchaseInvoice))
                                        {
                                            ViewBag.result = false;
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.result = false;
                                    }
                                }
                                else
                                {
                                    ViewBag.result = false;
                                }

                                break;
                            case "UnCancelVoucher":
                                if (ex.PurchaseInvoice != null)
                                {
                                    if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Cancellation
                                    {
                                        ex.PurchaseInvoice.IsCancelled = false;
                                        if (!PurchaseInvoice_Update(ex.PurchaseInvoice))
                                        {
                                            ViewBag.result = false;
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.result = false;
                                    }
                                }
                                else
                                {
                                    ViewBag.result = false;
                                }

                                break;
                            case "saveIssueItem":
                                if (ex.PurchaseInvoice != null)
                                {
                                    if (ex.PurchaseInvoice.PurchaseInvoiceId > 0 && ex.IssuedItem != null)
                                    // update Service
                                    {
                                        if (ConvertPurchaseInvoiceToIssuedItems(ex.PurchaseInvoice.PurchaseInvoiceId, ex.IssuedItem))
                                        {
                                            ViewBag.success = "Purchase invoice issued succesfully with ref # " + ViewBag.IssuedItemId;
                                        }
                                        else
                                        {
                                            ViewBag.result = false;
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.error = "Purchase invoice not found, conversion failed";
                                        ViewBag.result = false;
                                    }
                                }
                                else
                                {
                                    ViewBag.error = "Purchase invoice not found, conversion failed";
                                    ViewBag.result = false;
                                }
                                break;

                            case "savePayment":
                                if (ex.PurchaseInvoice != null)
                                {
                                    if (ex.PurchaseInvoice.PurchaseInvoiceId > 0 && ex.SupplierInvoicePayment != null) // SavePayment
                                    {
                                        SupplierPayment _SupplierPayment = new SupplierPayment();


                                        string cashAccount = "";
                                        if (ex.PaymentType == "Cash")
                                        {
                                            cashAccount = ex.CashAccountId;
                                        }
                                        else
                                        {
                                            cashAccount = ex.BankAccountId;
                                        }
                                        //var cashAccount = db.CashAccounts.Select(u => u.CashAccountId).FirstOrDefault();
                                        if (!string.IsNullOrEmpty(cashAccount))
                                        {
                                            _SupplierPayment.AccountId = cashAccount;
                                            _SupplierPayment.Amount = ex.SupplierInvoicePayment.Amount;
                                            _SupplierPayment.SupplierId = ex.PurchaseInvoice.SupplierId;
                                            _SupplierPayment.PaymentType = "Cash";
                                            _SupplierPayment.Description = ex.SupplierInvoicePayment.Description;
                                            _SupplierPayment.CreatedOn = ex.SupplierInvoicePayment.CreatedOn;
                                            _SupplierPayment.PaymentDate = ex.SupplierInvoicePayment.PaymentDate;

                                            var SupplierPaymentId = SaveSupplierPayments(_SupplierPayment);
                                            if (SupplierPaymentId > 0 && ex.SupplierInvoicePayment.Amount > 0)
                                            {
                                                SupplierInvoicePayment _SupplierInvoicePayment;
                                                _SupplierInvoicePayment = new SupplierInvoicePayment();
                                                _SupplierInvoicePayment.SupplierPaymentId = Convert.ToInt32(SupplierPaymentId);
                                                _SupplierInvoicePayment.PurchaseInvoiceId = ex.PurchaseInvoice.PurchaseInvoiceId;
                                                _SupplierInvoicePayment.Amount = _SupplierPayment.Amount;
                                                _SupplierInvoicePayment.CreatedOn = _SupplierPayment.CreatedOn;
                                                _SupplierInvoicePayment.PaymentDate = _SupplierPayment.PaymentDate;
                                                _SupplierInvoicePayment.ModuleId = module_ID;
                                                _SupplierInvoicePayment.Description = _SupplierPayment.Description;
                                                SaveSupplierInvoicePayments(_SupplierInvoicePayment, cashAccount);
                                                ViewBag.success = Messages = "Payment saved successfully";

                                            }
                                            else
                                            {
                                                ViewBag.result = false;
                                            }
                                        }
                                        else
                                        {
                                            ViewBag.result = false;
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.result = false;
                                    }
                                }
                                else
                                {
                                    ViewBag.result = false;
                                }

                                break;
                        }

                    }
                    else
                    {
                        ViewBag.result = false;
                    }

                    if (ViewBag.result)
                    {
                        trans.Commit();
                        ViewBag.result = true;
                    }
                }
                catch (Exception exc)
                {
                    var str = ExtensionMethods.GetExceptionMessages(exc);
                    if (str.Contains("Deletion"))
                    {
                        str = "Can't change quantity, It is used in issue items";
                    }
                    trans.Rollback();
                    ViewBag.error = str;
                    ViewBag.result = false;
                }
            }
            return ex;
        }
        bool PostSingleInvoice(long PurchaseInvoiceId)
        {
            var isPosted = db.AMPurchaseInvoices.Where(u => u.PurchaseInvoiceId == PurchaseInvoiceId).Select(u => u.IsPosted).FirstOrDefault();
            if (!isPosted)
            {
                if (!checkWarehouse(PurchaseInvoiceId))
                {
                    return false;
                }

                var _PurchaseInvoiceProduct = db.AMPurchaseInvoiceProducts.Where(u => u.PurchaseInvoiceId == PurchaseInvoiceId).ToList();
                if (_PurchaseInvoiceProduct != null)
                {
                    foreach (var item in _PurchaseInvoiceProduct)
                    {
                        ProceduresModel.InsertInvoiceProductDetails(db, item.PurchaseInvoiceProductId, item.ItemId, item.Quantity, IsFixedAssetItem(item.ItemId));
                    }
                }
                string count = p_mnl_PostUnPostAMAssetOpening(db, "Post", SessionHelper.UserID, PurchaseInvoiceId.ToString(), DateTime.Now);
                ViewBag.error = ViewBag.Error;
                if (!string.IsNullOrEmpty(count) && count != "0")
                {
                    return true;
                }
                else
                {
                    ViewBag.error = "Posting failed";
                    return false;
                }
            }
            else
            {
                ViewBag.error = "Invoice already posted";
                return false;
            }
        }

        public static string p_mnl_PostUnPostAMAssetOpening(OneDbContext db, string Command, int? UserId, string InvoiceIds, DateTime? Date)
        {
            var result = db.Database.ExecuteSqlCommand($@"if (@Command = 'Post') 
BEGIN 	
	Insert into inv.WarehouseProducts(WarehouseId, ProductId, Quantity, TransferDate, TransferType, StockPrice, StockValue, 
	TransferMethod, InvoiceId,BranchId,CreatedOn) 
	SELECT        
    sid.WareHouseId, sid.ProductId, sid.Quantity, si.PurchaseInvoiceDate, 'In' AS Expr2, sid.UnitPrice, p.CostPrice, 
	'Purchase' AS Expr3, sid.PurchaseInvoiceId,{SessionHelper.BranchId},GetDate() 
	FROM inv.PurchaseInvoiceProducts AS sid 
	INNER JOIN inv.Products AS p ON sid.ProductId = p.ProductId 
	INNER JOIN INV.PurchaseInvoices AS si ON sid.PurchaseInvoiceId = si.PurchaseInvoiceId 
	WHERE(si.PurchaseInvoiceId IN({InvoiceIds}) and si.IsCancelled = 0 and si.IsPosted = 0) 
	update inv.PurchaseInvoices  set IsPosted=1, PostedBy=@UserId, PostedOn=@Date where PurchaseInvoiceId in({InvoiceIds}) 
	and IsCancelled=0 and IsPosted=0  and IsAccountPosted=0  
END 
if (@Command = 'Unpost') 
BEGIN 
	delete wp from INV.WarehouseProducts as wp inner join INV.PurchaseInvoices as pinv on pinv.PurchaseInvoiceId= InvoiceId 
	where TransferMethod = 'Purchase Invoice' and pinv.PurchaseInvoiceId in({InvoiceIds}) and IsCancelled = 0 and IsPosted = 1  
	update INV.PurchaseInvoices  set IsPosted=0, PostedBy=null, PostedOn=null where PurchaseInvoiceId in({InvoiceIds}) 
	and IsCancelled=0 and IsPosted=1  and IsAccountPosted=0   	
END",
                new SqlParameter("@Command", ProceduresModel.GetDBNullOrValue(Command)),
                new SqlParameter("@UserId", ProceduresModel.GetDBNullOrValue(UserId)),
                new SqlParameter("@Date", ProceduresModel.GetDBNullOrValue(Date))
                ).ToString();
            return result;
        }

        public Int64 PurchaseInvoice_Insert(InvPurchaseInvoice objPurchaseInvoice)
        {
            var CurrencyId = db.Currencies.Select(u => u.CurrencyId).FirstOrDefault();
            var value = db.CurrencyValues.Where(u => u.CurrencyId == CurrencyId).Select(p => p.Value).FirstOrDefault();

            InvPurchaseInvoice _PurchaseInvoice = new InvPurchaseInvoice();
            var idd = db.AMPurchaseInvoices.Max(u => (Int64?)u.PurchaseInvoiceId);
            if (idd == null)
            {
                idd = 1000;
            }

            idd++;
            _PurchaseInvoice.PurchaseInvoiceId = Convert.ToInt64(idd);
            _PurchaseInvoice.PurchaseOrderId = objPurchaseInvoice.PurchaseOrderId;
            _PurchaseInvoice.PurchaseInvoiceDate = objPurchaseInvoice.PurchaseInvoiceDate;
            _PurchaseInvoice.SupplierId = objPurchaseInvoice.SupplierId;
            _PurchaseInvoice.Discount = objPurchaseInvoice.Discount;
            _PurchaseInvoice.Description = objPurchaseInvoice.Description ?? "";
            _PurchaseInvoice.CurrencyId = CurrencyId;
            _PurchaseInvoice.ExchangeRate = value;
            //_PurchaseInvoice.SupplierId = objPurchaseInvoice.SupplierId;
            _PurchaseInvoice.MovedToWarehouse = objPurchaseInvoice.MovedToWarehouse;
            _PurchaseInvoice.VoucherId = objPurchaseInvoice.VoucherId;
            _PurchaseInvoice.TotalAmount = objPurchaseInvoice.TotalAmount;
            _PurchaseInvoice.LabourCharges = objPurchaseInvoice.LabourCharges;
            _PurchaseInvoice.OtherCharges = objPurchaseInvoice.OtherCharges;
            _PurchaseInvoice.FareCharges = objPurchaseInvoice.FareCharges;
            _PurchaseInvoice.NetTotal = objPurchaseInvoice.NetTotal;
            _PurchaseInvoice.Version = objPurchaseInvoice.Version;
            _PurchaseInvoice.IsApplyTax = objPurchaseInvoice.IsApplyTax;
            if (_PurchaseInvoice.IsPosted)
            {
                _PurchaseInvoice.PostedBy = SessionHelper.UserID;
                _PurchaseInvoice.PostedOn = DateTime.Now;
            }
            if (_PurchaseInvoice.IsCancelled)
            {
                _PurchaseInvoice.CancelledOn = DateTime.Now;
                _PurchaseInvoice.CancelledBy = SessionHelper.UserID;
            }
            _PurchaseInvoice.BranchId = branch_ID;
            _PurchaseInvoice.CreatedBy = SessionHelper.UserID;
            _PurchaseInvoice.CreatedOn = DateTime.Now;
            db.InvPurchaseInvoices.Add(_PurchaseInvoice);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
            return _PurchaseInvoice.PurchaseInvoiceId;
        }

        public bool PurchaseInvoice_Update(InvPurchaseInvoice objPurchaseInvoice)
        {
            bool res = false;
            if (objPurchaseInvoice.PurchaseInvoiceId > 0)
            {
                InvPurchaseInvoice _PurchaseInvoice = db.InvPurchaseInvoices.Where(u => u.PurchaseInvoiceId == objPurchaseInvoice.PurchaseInvoiceId).FirstOrDefault();
                if (_PurchaseInvoice != null)
                {
                    _PurchaseInvoice.PurchaseOrderId = objPurchaseInvoice.PurchaseOrderId;
                    //_PurchaseInvoice.PurchaseInvoiceDate = objPurchaseInvoice.PurchaseInvoiceDate;
                    _PurchaseInvoice.SupplierId = objPurchaseInvoice.SupplierId;
                    _PurchaseInvoice.Discount = objPurchaseInvoice.Discount;
                    _PurchaseInvoice.Description = objPurchaseInvoice.Description ?? "";
                    //_PurchaseInvoice.CurrencyId = objPurchaseInvoice.CurrencyId;
                    //_PurchaseInvoice.ExchangeRate = objPurchaseInvoice.ExchangeRate;
                    _PurchaseInvoice.MovedToWarehouse = objPurchaseInvoice.MovedToWarehouse;
                    //_PurchaseInvoice.VoucherId = objPurchaseInvoice.VoucherId;
                    _PurchaseInvoice.TotalAmount = objPurchaseInvoice.TotalAmount;
                    _PurchaseInvoice.LabourCharges = objPurchaseInvoice.LabourCharges;
                    _PurchaseInvoice.OtherCharges = objPurchaseInvoice.OtherCharges;
                    _PurchaseInvoice.FareCharges = objPurchaseInvoice.FareCharges;
                    _PurchaseInvoice.NetTotal = objPurchaseInvoice.NetTotal;
                    _PurchaseInvoice.Version = objPurchaseInvoice.Version;
                    _PurchaseInvoice.IsApplyTax = objPurchaseInvoice.IsApplyTax;
                    if (objPurchaseInvoice.IsPosted)
                    {
                        _PurchaseInvoice.IsPosted = objPurchaseInvoice.IsPosted;
                        _PurchaseInvoice.PostedBy = SessionHelper.UserID;
                        _PurchaseInvoice.PostedOn = DateTime.Now;
                    }
                    else
                    {
                        _PurchaseInvoice.IsPosted = objPurchaseInvoice.IsPosted;
                        _PurchaseInvoice.PostedBy = null;
                        _PurchaseInvoice.PostedOn = null;
                    }
                    if (objPurchaseInvoice.IsCancelled)
                    {
                        _PurchaseInvoice.IsCancelled = objPurchaseInvoice.IsCancelled;
                        _PurchaseInvoice.CancelledOn = DateTime.Now;
                        _PurchaseInvoice.CancelledBy = SessionHelper.UserID;
                    }
                    else
                    {
                        _PurchaseInvoice.IsCancelled = objPurchaseInvoice.IsCancelled;
                        _PurchaseInvoice.CancelledOn = null;
                        _PurchaseInvoice.CancelledBy = null;
                    }
                    _PurchaseInvoice.ModifiedBy = SessionHelper.UserID;
                    _PurchaseInvoice.ModifiedOn = DateTime.Now;
                    db.Entry(_PurchaseInvoice).State = EntityState.Modified;
                    db.SaveChanges();
                    res = true;
                }
            }
            return res;
        }


        bool checkWarehouse(long SaleInvoiceId)
        {
            var _details = db.PosSaleInvoiceProducts.Where(u => u.BranchId == branch_ID).Where(u => u.SaleInvoiceId == SaleInvoiceId).ToList();
            if (_details != null)
            {
                foreach (var item in _details)
                {
                    if (item.Product.CreateCogsEntryInVoucher && item.WareHouseId == null)
                    {
                        ViewBag.error = item.Product.ProductName + "'s warehouse not found, Posting failed";
                        return false;
                    }
                }
            }
            return true;
        }
        public Int64 PurchaseInvoiceProduct_Insert(Int64 PurchaseInvoiceId, InvPurchaseInvoiceProduct objPurchaseInvoiceProduct)
        {
            if (objPurchaseInvoiceProduct.ProductId > 0)
            {
                InvPurchaseInvoiceProduct _PurchaseInvoiceProduct = objPurchaseInvoiceProduct;
                _PurchaseInvoiceProduct.PurchaseInvoiceId = PurchaseInvoiceId;
                _PurchaseInvoiceProduct.ProductId = objPurchaseInvoiceProduct.ProductId;
                _PurchaseInvoiceProduct.ManufacturerProductNo = objPurchaseInvoiceProduct.ManufacturerProductNo;
                _PurchaseInvoiceProduct.OrgWidth = objPurchaseInvoiceProduct.OrgWidth;
                _PurchaseInvoiceProduct.OrgLength = objPurchaseInvoiceProduct.OrgLength;
                _PurchaseInvoiceProduct.CalWidth = objPurchaseInvoiceProduct.CalWidth;
                _PurchaseInvoiceProduct.CalLength = objPurchaseInvoiceProduct.CalLength;
                _PurchaseInvoiceProduct.CalDigit = objPurchaseInvoiceProduct.CalDigit;
                _PurchaseInvoiceProduct.Quantity = objPurchaseInvoiceProduct.Quantity;
                _PurchaseInvoiceProduct.SqFeet = objPurchaseInvoiceProduct.SqFeet;
                _PurchaseInvoiceProduct.UnitPrice = objPurchaseInvoiceProduct.UnitPrice;
                _PurchaseInvoiceProduct.LineTotal = objPurchaseInvoiceProduct.LineTotal;
                _PurchaseInvoiceProduct.Discount = objPurchaseInvoiceProduct.Discount;
                _PurchaseInvoiceProduct.Tax = objPurchaseInvoiceProduct.Tax;
                _PurchaseInvoiceProduct.NetTotal = objPurchaseInvoiceProduct.NetTotal;
                _PurchaseInvoiceProduct.WareHouseId = objPurchaseInvoiceProduct.WareHouseId;
                //_PurchaseInvoiceProduct.ConditionTypeId = objPurchaseInvoiceProduct.ConditionTypeId;
                db.InvPurchaseInvoiceProducts.Add(_PurchaseInvoiceProduct);
                db.SaveChanges();
                //ProceduresModel.InsertInvoiceProductDetails(db, _PurchaseInvoiceProduct.PurchaseInvoiceProductId, objPurchaseInvoiceProduct.ItemId, objPurchaseInvoiceProduct.Quantity, IsFixedAssetItem(objPurchaseInvoiceProduct.ItemId));

            }
            return PurchaseInvoiceId;
        }

        private bool IsFixedAssetItem(int id)
        {
            //var result = (from item in db.AMItems
            //              join cat in db.AMCategories on item.CategoryId equals cat.CategoryId
            //              join nature in db.AMNatures on cat.NatureId equals nature.NatureId
            //              where item.ItemId == id
            //              select nature.NatureName).FirstOrDefault();
            var result = (from item in db.AMItems
                          where item.ItemId == id
                          select item.IsConsumable).FirstOrDefault();
            //if (result == true)
            //    return true;
            //else
            return result;
        }
        bool UnpostSingleInvoice(long PurchaseInvoiceId)
        {
            ProceduresModel.DeleteInvoiceProductDetails(db, PurchaseInvoiceId);
            string count = p_mnl_PostUnPostAMAssetOpening(db, "Unpost", SessionHelper.UserID, PurchaseInvoiceId.ToString(), DateTime.Now);
            var purchaseinoivce = db.InvPurchaseInvoices.Where(w => w.PurchaseInvoiceId == PurchaseInvoiceId).FirstOrDefault();
            if (purchaseinoivce != null)
            {
                purchaseinoivce.IsPosted = false;
                db.SaveChanges();
            }
            if (!string.IsNullOrEmpty(count))
            {
                return true;
            }
            else
            {
                ViewBag.error = "Unposting failed";
                return false;
            }
        }
        public bool PurchaseInvoiceProduct_Update(InvPurchaseInvoiceProduct objPurchaseInvoiceProduct)
        {
            bool res = false;
            if (objPurchaseInvoiceProduct.PurchaseInvoiceProductId > 0)
            {
                InvPurchaseInvoiceProduct _PurchaseInvoiceProduct = db.InvPurchaseInvoiceProducts.Where(u => u.PurchaseInvoiceProductId == objPurchaseInvoiceProduct.PurchaseInvoiceProductId).FirstOrDefault();
                if (_PurchaseInvoiceProduct != null)
                {
                    //_PurchaseInvoiceProduct.PurchaseInvoiceId = PurchaseInvoiceId;
                    _PurchaseInvoiceProduct.ProductId = objPurchaseInvoiceProduct.ProductId;
                    _PurchaseInvoiceProduct.ManufacturerProductNo = objPurchaseInvoiceProduct.ManufacturerProductNo;
                    _PurchaseInvoiceProduct.OrgWidth = objPurchaseInvoiceProduct.OrgWidth;
                    _PurchaseInvoiceProduct.OrgLength = objPurchaseInvoiceProduct.OrgLength;
                    _PurchaseInvoiceProduct.CalWidth = objPurchaseInvoiceProduct.CalWidth;
                    _PurchaseInvoiceProduct.CalLength = objPurchaseInvoiceProduct.CalLength;
                    _PurchaseInvoiceProduct.Sheets = objPurchaseInvoiceProduct.Sheets;
                    _PurchaseInvoiceProduct.CalDigit = objPurchaseInvoiceProduct.CalDigit;
                    //if (_PurchaseInvoiceProduct.Quantity != objPurchaseInvoiceProduct.Quantity)
                    //    ProceduresModel.InsertInvoiceProductDetails(db, _PurchaseInvoiceProduct.PurchaseInvoiceProductId, objPurchaseInvoiceProduct.ItemId, objPurchaseInvoiceProduct.Quantity, IsFixedAssetItem(objPurchaseInvoiceProduct.ItemId));
                    _PurchaseInvoiceProduct.Quantity = objPurchaseInvoiceProduct.Quantity;
                    _PurchaseInvoiceProduct.SqFeet = objPurchaseInvoiceProduct.SqFeet;
                    _PurchaseInvoiceProduct.UnitPrice = objPurchaseInvoiceProduct.UnitPrice;
                    _PurchaseInvoiceProduct.LineTotal = objPurchaseInvoiceProduct.LineTotal;
                    _PurchaseInvoiceProduct.Discount = objPurchaseInvoiceProduct.Discount;
                    _PurchaseInvoiceProduct.Tax = objPurchaseInvoiceProduct.Tax;
                    _PurchaseInvoiceProduct.NetTotal = objPurchaseInvoiceProduct.NetTotal;
                    _PurchaseInvoiceProduct.WareHouseId = objPurchaseInvoiceProduct.WareHouseId;
                    //_PurchaseInvoiceProduct.ConditionTypeId = objPurchaseInvoiceProduct.ConditionTypeId;
                    db.Entry(_PurchaseInvoiceProduct).State = EntityState.Modified;
                    db.SaveChanges();
                    //if (_PurchaseInvoiceProduct.Quantity != objPurchaseInvoiceProduct.Quantity)
                    //    ProceduresModel.InsertInvoiceProductDetails(db, _PurchaseInvoiceProduct.PurchaseInvoiceProductId, objPurchaseInvoiceProduct.ItemId, objPurchaseInvoiceProduct.Quantity, IsFixedAssetItem(objPurchaseInvoiceProduct.ItemId));
                    res = true;
                }
            }
            return res;
        }

        public ActionResult LoadInvoicePayments(long? InvoiceNo = 0)
        {
            //var list = db.AMSupplierInvoicePayments
            //    .Where(u => u.BranchId == branch_ID && u.PurchaseInvoiceId == InvoiceNo)
            //    .ToList()
            //    .Select(u => new
            //    {
            //        Date = u.CreatedOn == null ? "" : u.CreatedOn.Value.ToString("dd MMM yyyy"),
            //        Amount = u.Amount,
            //        Description = u.Description
            //    }).ToList();
            var list = db.SupplierInvoicePayments
                .Where(u => u.BranchId == branch_ID && u.PurchaseInvoiceId == InvoiceNo && u.ModuleId == module_ID)
                .ToList()
                .Select(u => new
                {
                    supplierPaymentId = u.SupplierPaymentId?.ToString(),
                    Date = u.PaymentDate == null ? "" : u.PaymentDate.Value.ToString("dd MMM yyyy"),
                    Amount = u.Amount,
                    Description = u.Description
                }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadIssueItem(long? InvoiceNo = 0)
        {
            var list = db.AMIssuedItems
                .Where(u => u.PurchaseInvoiceId == InvoiceNo).Select(u => u.IssuedItemId).FirstOrDefault();
            return Json(list.ToString(), JsonRequestBehavior.AllowGet);
        }

        bool ConvertPurchaseInvoiceToIssuedItems(long PurchaseInvoiceId, AMIssuedItem objIssuedItem)
        {
            var _PurchaseInvoice = db.AMPurchaseInvoices.Find(PurchaseInvoiceId);
            if (_PurchaseInvoice != null)
            {
                AMIssuedItem _IssuedItem = new AMIssuedItem();
                _IssuedItem.DepartmentId = objIssuedItem.DepartmentId;
                _IssuedItem.IssueDate = DateTime.Now;
                _IssuedItem.Description = objIssuedItem.Description;
                _IssuedItem.CreatedOn = DateTime.Now;
                _IssuedItem.CreatedBy = SessionHelper.UserID;
                _IssuedItem.BranchId = branch_ID;
                _IssuedItem.CreatedByIP = SessionHelper.IP;
                _IssuedItem.PurchaseInvoiceId = PurchaseInvoiceId;
                db.AMIssuedItems.Add(_IssuedItem);
                var _PurchaseInvoiceProducts = db.AMPurchaseInvoiceProducts.Where(u => u.PurchaseInvoiceId == PurchaseInvoiceId).ToList();
                if (_PurchaseInvoiceProducts != null)
                {
                    AMIssuedItemDetail _IssuedItemDetail;
                    List<AMPurchaseInvoiceProductDetail> _PurchaseInvoiceProductDetail;
                    foreach (var item in _PurchaseInvoiceProducts)
                    {
                        _PurchaseInvoiceProductDetail = db.AMPurchaseInvoiceProductDetails.Where(u => u.PurchaseInvoiceProductId == item.PurchaseInvoiceProductId).ToList();
                        if (_PurchaseInvoiceProductDetail != null)
                        {
                            foreach (var purchaseInvoiceProducts in _PurchaseInvoiceProductDetail)
                            {
                                _IssuedItemDetail = new AMIssuedItemDetail();
                                _IssuedItemDetail.IssuedItemId = _IssuedItem.IssuedItemId;
                                _IssuedItemDetail.ItemId = purchaseInvoiceProducts.ItemId;
                                _IssuedItemDetail.PIPDetailId = purchaseInvoiceProducts.DetailId;
                                _IssuedItemDetail.ConditionTypeId = item.ConditionTypeId;
                                _IssuedItemDetail.Quantity = Convert.ToInt16(purchaseInvoiceProducts.Qty);
                                _IssuedItemDetail.CreatedOn = DateTime.Now;
                                _IssuedItemDetail.CreateBy = SessionHelper.UserID;
                                _IssuedItemDetail.BranchId = branch_ID;
                                db.AMIssuedItemDetails.Add(_IssuedItemDetail);
                            }
                        }
                        else
                        {
                            ViewBag.error = "Purchase Invoice product(s) not found, conversion failed";
                            return false;
                        }
                    }
                    db.SaveChanges();
                    ViewBag.IssuedItemId = _IssuedItem.IssuedItemId;
                    return true;
                }
                else
                {
                    ViewBag.error = "Purchase Invoice product(s) not found, conversion failed";
                    return false;
                }
            }
            else
            {
                ViewBag.error = "Purchase Invoice found, conversion failed";
                return false;
            }

        }


        #endregion

        #region ManagePurchaseInvoices

        public ActionResult ManagePurchaseInvoices(int? page)
        {

            PurchaseModelViewModel ex = new PurchaseModelViewModel();
            DateTime now = DateTime.Now;
            //ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.FromDate = string.Format("{0:dd/MM/yyyy}", SessionHelper.FiscalStartDate);
            ex.ToDate = now.ToShortDateString();
            ex.All = true;
            ex.WithBalance = false;
            ex.v_mnl_PurchaseInvoicesPagedList = PurchaseInvoicePagedList(ex, page);
            ex.HasPendingInvoicePosts = db.AMPurchaseInvoices.Any(m => m.IsPosted == false);
            var Area = this.ControllerContext.RouteData.DataTokens["Area"];
            ViewBag.Forms = db.Forms.Where(m => m.FormURL.ToLower().StartsWith("/" + Area + "/PurchaseReports/".ToLower())).ToList();

            return View(ex);
        }

        [HttpPost]
        public PartialViewResult ManagePurchaseInvoices(PurchaseModelViewModel ex, int? page)
        {
            ex.v_mnl_PurchaseInvoicesPagedList = PurchaseInvoicePagedList(ex, page);
            ModelState.Clear();
            return PartialView("_PartialPurchaseInvoiceList", ex);
        }

        IPagedList<v_mnl_PurchaseInvoices_Result> PurchaseInvoicePagedList(PurchaseModelViewModel ex, int? page)
        {
            var list = ProceduresModel.v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID).ToList();
            if (!string.IsNullOrEmpty(ex.FromDate) && !string.IsNullOrEmpty(ex.ToDate))
            {
                var fdate = Convert.ToDateTime(ex.FromDate);
                var tdate = Convert.ToDateTime(ex.ToDate);
                list = ProceduresModel.v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID && u.PurchaseInvoiceDate.Date >= fdate && u.PurchaseInvoiceDate <= tdate).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(ex.FromDate))
                {
                    var date = Convert.ToDateTime(ex.FromDate);
                    list = ProceduresModel.v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID && u.PurchaseInvoiceDate >= date).ToList();
                }
                if (!string.IsNullOrEmpty(ex.ToDate))
                {
                    var date = Convert.ToDateTime(ex.ToDate);
                    list = ProceduresModel.v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID && u.PurchaseInvoiceDate <= date).ToList();
                }
            }
            if (!string.IsNullOrEmpty(ex.Search))
            {
                ex.Search = ex.Search.ToUpper();
                list = list.Where(u => (u.ClientName == null ? "" : u.ClientName.ToUpper()).Contains(ex.Search.ToUpper()) ||
                    (u.PurchaseInvoiceId.ToString() == ex.Search) ||
                    (u.Description == null ? "" : u.Description.ToUpper()).Contains(ex.Search.ToUpper())).ToList();
            }
            if (ex.All) { }
            else
            {
                if (ex.WithBalance)
                {
                    list = list.Where(u => (u.NetTotal - u.ReceivedAmount) > 0).ToList();
                }
                else
                {
                    list = list.Where(u => (u.NetTotal - u.ReceivedAmount) == 0).ToList();
                }
            }
            ex.v_mnl_PurchaseInvoicesPagedList = list.ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, 20);
            return ex.v_mnl_PurchaseInvoicesPagedList;
        }


        #endregion

        #region AddEditPurchaseReturn

        public ActionResult AddEditPurchaseReturn(Int64? id, string message)
        {

            PurchaseReturnModelViewModel ex = new PurchaseReturnModelViewModel();
            ex = GetPurchaseReturnInvoice(ex, id);
            ViewBag.Cancelled = message;
            return View(ex);
        }

        PurchaseReturnModelViewModel GetPurchaseReturnInvoice(PurchaseReturnModelViewModel ex, Int64? id = 0)
        {
            ex.InvoiceNo = id;
            FillDD();
            ViewBag.EditMode = false;
            if (id > 0)
            {
                ex.PurchaseReturn = db.AMPurchaseReturns.Where(u => u.PurchaseReturnId == id).FirstOrDefault();
                if (ex.PurchaseReturn != null)
                {
                    ViewBag.IsCancelled = ex.PurchaseReturn.IsCancelled;
                    if (ex.PurchaseReturn.IsCancelled)
                    {
                        ViewBag.Cancelled = "Invoice has been cancelled by " + db.Users.Where(u => u.UserID == ex.PurchaseReturn.CancelledBy).Select(u => u.Username).FirstOrDefault() + " on " + ex.PurchaseReturn.CancelledOn.Value.ToShortDateString();
                    }

                    ViewBag.IsPosted = ex.PurchaseReturn.IsPosted;
                    if (ex.PurchaseReturn.IsPosted)
                    {
                        ViewBag.success = "Invoice has been Posted by " + db.Users.Where(u => u.UserID == ex.PurchaseReturn.PostedBy).Select(u => u.Username).FirstOrDefault() + " on " + ex.PurchaseReturn.PostedOn.Value.ToShortDateString();
                    }

                    ViewBag.EditMode = true;
                    ViewBag.ModifiedBy = db.Users.Where(u => u.UserID == ex.PurchaseReturn.ModifiedBy).Select(u => u.Username).FirstOrDefault();
                    ViewBag.CreatedBy = db.Users.Where(u => u.UserID == ex.PurchaseReturn.CreatedBy).Select(u => u.Username).FirstOrDefault();
                }
                ex.PurchaseReturnProduct = db.AMPurchaseReturnProducts.Where(u => u.PurchaseReturnId == id).ToList();
            }
            else
            {
                ex.PurchaseReturn = new Model.AMPurchaseReturn();
                ex.PurchaseReturn.PurchaseReturnDate = DateTime.Now;
            }
            return ex;
        }

        [HttpPost]
        public ActionResult AddEditPurchaseReturn(PurchaseReturnModelViewModel ex, string Command)
        {
            switch (Command)
            {
                case "LoadInvoice":
                    if (ex.InvoiceNo != null)
                    {
                        var prevId = ex.PurchaseReturn.PurchaseReturnId;
                        var isExists = db.AMPurchaseReturns.Where(u => u.PurchaseReturnId == ex.InvoiceNo).FirstOrDefault();
                        if (isExists != null)
                        {
                            ex = GetPurchaseReturnInvoice(ex, isExists.PurchaseReturnId);
                        }
                        else
                        {
                            ViewBag.Cancelled = "Invoice does not exist";
                            ex = GetPurchaseReturnInvoice(ex, prevId);
                        }
                    }
                    else
                    {
                        ex = GetPurchaseReturnInvoice(ex, ex.PurchaseReturn.PurchaseReturnId);
                    }

                    break;
            }
            return View(ex);
        }


        [HttpPost]
        public ActionResult PostPurchaseReturn(AMPurchaseReturnModel ex, string Command)
        {
            ex = PostPurchaseReturns(ex, Command);
            var clName = db.Clients.Where(u => u.ClientId == ex.PurchaseReturn.SupplierId).Select(u => u.Name).FirstOrDefault();
            var invoice = new
            {
                ex.PurchaseReturn.OtherCharges,
                ex.PurchaseReturn.Discount,
                ex.PurchaseReturn.NetTotal,
                Received = Convert.ToDecimal(ex.PurchaseReturn.Received),
                NetBalance = Convert.ToDecimal(ex.PurchaseReturn.NetTotal) - Convert.ToDecimal(ex.PurchaseReturn.Received),
                ClientName = clName,
                ex.PurchaseReturn.PurchaseReturnId
            };
            var detaillist = db.AMPurchaseReturnProducts.Where(u => u.PurchaseReturnId == ex.PurchaseReturn.PurchaseReturnId).ToList().Select(u => new
            {
                u.PurchaseReturnProductId,
                u.PurchaseReturnId,
                u.CalDigit,
                u.CalWidth,
                u.CalLength,
                u.Tax,
                u.UnitPrice,
                u.Sheets,
                u.SqFeet,
                u.Quantity,
                u.LineTotal,
                ProductName = u.Item == null ? "" : u.Item.ItemName
            }).ToList();
            var receipts = db.AMSupplierRefundInvoices.Where(u => u.PurchaseReturnId == ex.PurchaseReturn.PurchaseReturnId).ToList();
            if (receipts != null)
            {
                var paymentist = receipts.Select(u => new
                {
                    Date = u.CreatedOn == null ? "" : u.CreatedOn.Value.ToString("dd MMM yyyy"),
                    Amount = u.Amount,
                    Description = u.Description
                }).ToList();
                var data = new
                {
                    IsSuccess = ViewBag.result,
                    Case = Command,
                    IsCancelled = Convert.ToBoolean(ViewBag.IsCancelled) == true ? true : false,
                    IsPosted = Convert.ToBoolean(ViewBag.IsPosted) == true ? true : false,
                    Modification = ViewBag.Modification,
                    PurchaseReturnId = ViewBag.PurchaseReturnId,
                    Detail = detaillist,
                    Receipts = paymentist,
                    Invoice = invoice
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = new
                {
                    IsSuccess = ViewBag.result,
                    Case = Command,
                    IsCancelled = Convert.ToBoolean(ViewBag.IsCancelled) == true ? true : false,
                    IsPosted = Convert.ToBoolean(ViewBag.IsPosted) == true ? true : false,
                    Modification = ViewBag.Modification,
                    PurchaseReturnId = ViewBag.PurchaseReturnId,
                    Detail = detaillist,
                    Receipts = DBNull.Value.ToString(),
                    Invoice = invoice
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            //return Json(data, JsonRequestBehavior.AllowGet);
        }

        AMPurchaseReturnModel PostPurchaseReturns(AMPurchaseReturnModel ex, string Command)
        {
            ViewBag.result = true;
            if (ex.PurchaseReturn != null)
            {
                switch (Command)
                {
                    case "InsertUpdate":
                        if (ex.PurchaseReturn != null)
                        {
                            if (ex.PurchaseReturn.PurchaseReturnId > 0) // Updation
                            {
                                var purchase = db.AMPurchaseReturns.Where(u => u.PurchaseReturnId == ex.PurchaseReturn.PurchaseReturnId).FirstOrDefault();
                                if (!purchase.IsPosted && !purchase.IsCancelled)
                                {
                                    if (PurchaseReturn_Update(ex.PurchaseReturn, Command))
                                    {
                                        if (ex.PurchaseReturnProduct != null)
                                        {
                                            foreach (var item in ex.PurchaseReturnProduct)
                                            {
                                                if (item.PurchaseReturnProductId > 0)
                                                {
                                                    PurchaseReturnProduct_Update(item);
                                                }
                                                else
                                                {
                                                    PurchaseReturnProduct_Insert(ex.PurchaseReturn.PurchaseReturnId, item);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ViewBag.result = false;
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.result = false;
                                    }
                                }
                                else
                                {
                                    ViewBag.result = false;
                                }
                            }
                            else // Insertion
                            {
                                ex.PurchaseReturn.PurchaseReturnId = PurchaseReturn_Insert(ex.PurchaseReturn);
                                if (ex.PurchaseReturn.PurchaseReturnId > 0)
                                {
                                    if (ex.PurchaseReturnProduct != null)
                                    {
                                        foreach (var item in ex.PurchaseReturnProduct)
                                        {
                                            if (item.PurchaseReturnProductId > 0)
                                            {
                                                PurchaseReturnProduct_Update(item);
                                            }
                                            else
                                            {
                                                PurchaseReturnProduct_Insert(ex.PurchaseReturn.PurchaseReturnId, item);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.result = false;
                                    }
                                }
                                else
                                {
                                    ViewBag.result = false;
                                }
                            }
                        }
                        else
                        {
                            ViewBag.result = false;
                        }

                        break;
                    case "PostVoucher":
                        if (ex.PurchaseReturn != null)
                        {
                            if (ex.PurchaseReturn.PurchaseReturnId > 0) // Posting
                            {
                                ex.PurchaseReturn.IsPosted = true;
                                if (!PurchaseReturn_Update(ex.PurchaseReturn, Command))
                                {
                                    ViewBag.result = false;
                                }
                            }
                            else
                            {
                                ViewBag.result = false;
                            }
                        }
                        else
                        {
                            ViewBag.result = false;
                        }

                        break;
                    case "UnPostVoucher":
                        if (ex.PurchaseReturn != null)
                        {
                            if (ex.PurchaseReturn.PurchaseReturnId > 0) // Posting
                            {
                                ex.PurchaseReturn.IsPosted = false;
                                if (!PurchaseReturn_Update(ex.PurchaseReturn, Command))
                                {
                                    ViewBag.result = false;
                                }
                            }
                            else
                            {
                                ViewBag.result = false;
                            }
                        }
                        else
                        {
                            ViewBag.result = false;
                        }

                        break;
                    case "CancelVoucher":
                        if (ex.PurchaseReturn != null)
                        {
                            if (ex.PurchaseReturn.PurchaseReturnId > 0) // Cancellation
                            {
                                ex.PurchaseReturn.IsCancelled = true;
                                if (!PurchaseReturn_Update(ex.PurchaseReturn, Command))
                                {
                                    ViewBag.result = false;
                                }
                            }
                            else
                            {
                                ViewBag.result = false;
                            }
                        }
                        else
                        {
                            ViewBag.result = false;
                        }

                        break;
                    case "UnCancelVoucher":
                        if (ex.PurchaseReturn != null)
                        {
                            if (ex.PurchaseReturn.PurchaseReturnId > 0) // Cancellation
                            {
                                ex.PurchaseReturn.IsCancelled = false;
                                if (!PurchaseReturn_Update(ex.PurchaseReturn, Command))
                                {
                                    ViewBag.result = false;
                                }
                            }
                            else
                            {
                                ViewBag.result = false;
                            }
                        }
                        else
                        {
                            ViewBag.result = false;
                        }

                        break;
                    case "savePayment":
                        if (ex.PurchaseReturn != null)
                        {
                            if (ex.PurchaseReturn.PurchaseReturnId > 0 && ex.SupplierRefundInvoice != null) // SavePayment
                            {
                                //if (!SaveClientInvoicePayments(ex.SaleInvoice.SaleInvoiceId, ex.ClientInvoicePayment))
                                Model.AMSupplierRefund _SupplierRefund = new Model.AMSupplierRefund();
                                var cashAccount = db.CashAccounts.Select(u => u.CashAccountId).FirstOrDefault();
                                if (!string.IsNullOrEmpty(cashAccount))
                                {
                                    _SupplierRefund.AccountId = cashAccount;
                                    _SupplierRefund.Amount = ex.SupplierRefundInvoice.Amount;
                                    _SupplierRefund.SupplierId = ex.PurchaseReturn.SupplierId;
                                    _SupplierRefund.PaymentType = "Cash";
                                    var SupplierRefundId = SaveSupplierRefunds(_SupplierRefund);
                                    if (SupplierRefundId > 0 && ex.SupplierRefundInvoice.Amount > 0)
                                    {
                                        Model.AMSupplierRefundInvoice _SupplierRefundInvoice;
                                        _SupplierRefundInvoice = new Model.AMSupplierRefundInvoice();
                                        _SupplierRefundInvoice.SupplierRefundId = SupplierRefundId;
                                        _SupplierRefundInvoice.PurchaseReturnId = ex.PurchaseReturn.PurchaseReturnId;
                                        _SupplierRefundInvoice.Amount = _SupplierRefund.Amount;
                                        _SupplierRefundInvoice.Description = _SupplierRefund.Description;
                                        SaveSupplierRefundInvoices(_SupplierRefundInvoice);
                                        ViewBag.success = "Payment saved successfully";

                                    }
                                    else
                                    {
                                        ViewBag.result = false;
                                    }
                                }
                                else
                                {
                                    ViewBag.result = false;
                                }
                            }
                            else
                            {
                                ViewBag.result = false;
                            }
                        }
                        else
                        {
                            ViewBag.result = false;
                        }

                        break;
                }

            }
            else
            {
                ViewBag.result = false;
            }

            if (ViewBag.result)
            {
                ex.PurchaseReturn = db.AMPurchaseReturns.Where(u => u.PurchaseReturnId == ex.PurchaseReturn.PurchaseReturnId).FirstOrDefault();
                ex.PurchaseReturnProduct = db.AMPurchaseReturnProducts.Where(u => u.PurchaseReturnId == ex.PurchaseReturn.PurchaseReturnId).ToList();
                ViewBag.PurchaseReturnId = ex.PurchaseReturn.PurchaseReturnId;
                ViewBag.IsCancelled = ex.PurchaseReturn.IsCancelled;
                ViewBag.IsPosted = ex.PurchaseReturn.IsPosted;
                var ModifiedBy = db.Users.Where(u => u.UserID == ex.PurchaseReturn.ModifiedBy).Select(u => u.Username).FirstOrDefault();
                var CreatedBy = db.Users.Where(u => u.UserID == ex.PurchaseReturn.CreatedBy).Select(u => u.Username).FirstOrDefault();
                var Modification = "";
                if (!string.IsNullOrEmpty(CreatedBy))
                {
                    Modification = "Created By: " + CreatedBy;
                }

                if (!string.IsNullOrEmpty(ModifiedBy))
                {
                    Modification = Modification + "        Modified By: " + ModifiedBy;
                }

                ViewBag.Modification = Modification;
            }
            return ex;
        }

        public Int64 PurchaseReturn_Insert(Model.AMPurchaseReturn objPurchaseReturn)
        {
            var CurrencyId = db.Currencies.Select(u => u.CurrencyId).FirstOrDefault();
            var value = db.CurrencyValues.Where(u => u.CurrencyId == CurrencyId).Select(p => p.Value).FirstOrDefault();

            Model.AMPurchaseReturn _PurchaseReturn = new Model.AMPurchaseReturn();
            var idd = db.AMPurchaseReturns.Max(u => (Int64?)u.PurchaseReturnId);
            if (idd == null)
            {
                idd = 1000;
            }

            idd++;
            _PurchaseReturn.PurchaseReturnId = Convert.ToInt64(idd);
            _PurchaseReturn.PurchaseReturnDate = objPurchaseReturn.PurchaseReturnDate;
            _PurchaseReturn.SupplierId = objPurchaseReturn.SupplierId;
            _PurchaseReturn.Discount = objPurchaseReturn.Discount;
            _PurchaseReturn.Description = objPurchaseReturn.Description ?? "";
            _PurchaseReturn.CurrencyId = CurrencyId;
            _PurchaseReturn.ExchangeRate = value;
            //_PurchaseReturn.SupplierId = objPurchaseReturn.SupplierId;
            _PurchaseReturn.MovedFromWarehouse = objPurchaseReturn.MovedFromWarehouse;
            _PurchaseReturn.VoucherId = objPurchaseReturn.VoucherId;
            _PurchaseReturn.TotalAmount = objPurchaseReturn.TotalAmount;
            _PurchaseReturn.LabourCharges = objPurchaseReturn.LabourCharges;
            _PurchaseReturn.OtherCharges = objPurchaseReturn.OtherCharges;
            _PurchaseReturn.FareCharges = objPurchaseReturn.FareCharges;
            _PurchaseReturn.NetTotal = objPurchaseReturn.NetTotal;
            _PurchaseReturn.Version = objPurchaseReturn.Version;
            _PurchaseReturn.IsApplyTax = objPurchaseReturn.IsApplyTax;
            if (_PurchaseReturn.IsPosted)
            {
                _PurchaseReturn.PostedBy = SessionHelper.UserID;
                _PurchaseReturn.PostedOn = DateTime.Now;
            }
            if (_PurchaseReturn.IsCancelled)
            {
                _PurchaseReturn.CancelledOn = DateTime.Now;
                _PurchaseReturn.CancelledBy = SessionHelper.UserID;
            }
            _PurchaseReturn.BranchId = branch_ID;
            _PurchaseReturn.CreatedBy = SessionHelper.UserID;
            _PurchaseReturn.CreatedOn = DateTime.Now;
            db.AMPurchaseReturns.Add(_PurchaseReturn);
            db.SaveChanges();
            return _PurchaseReturn.PurchaseReturnId;
        }

        public bool PurchaseReturn_Update(AMPurchaseReturn objPurchaseReturn, string Command)
        {
            bool res = false;
            if (objPurchaseReturn.PurchaseReturnId > 0)
            {
                Model.AMPurchaseReturn _PurchaseReturn = db.AMPurchaseReturns.Where(u => u.PurchaseReturnId == objPurchaseReturn.PurchaseReturnId).FirstOrDefault();
                if (_PurchaseReturn != null)
                {
                    if (Command == "InsertUpdate")
                    {
                        //_PurchaseReturn.PurchaseOrderId = objPurchaseReturn.PurchaseOrderId;
                        //_PurchaseReturn.PurchaseReturnDate = objPurchaseReturn.PurchaseReturnDate;
                        _PurchaseReturn.SupplierId = objPurchaseReturn.SupplierId;
                        _PurchaseReturn.Discount = objPurchaseReturn.Discount;
                        _PurchaseReturn.Description = objPurchaseReturn.Description ?? "";
                        //_PurchaseReturn.CurrencyId = objPurchaseReturn.CurrencyId;
                        //_PurchaseReturn.ExchangeRate = objPurchaseReturn.ExchangeRate;
                        _PurchaseReturn.MovedFromWarehouse = objPurchaseReturn.MovedFromWarehouse;
                        //_PurchaseReturn.VoucherId = objPurchaseReturn.VoucherId;
                        _PurchaseReturn.TotalAmount = objPurchaseReturn.TotalAmount;
                        _PurchaseReturn.LabourCharges = objPurchaseReturn.LabourCharges;
                        _PurchaseReturn.OtherCharges = objPurchaseReturn.OtherCharges;
                        _PurchaseReturn.FareCharges = objPurchaseReturn.FareCharges;
                        _PurchaseReturn.NetTotal = objPurchaseReturn.NetTotal;
                        _PurchaseReturn.Version = objPurchaseReturn.Version;
                        _PurchaseReturn.IsApplyTax = objPurchaseReturn.IsApplyTax;
                    }
                    if (objPurchaseReturn.IsPosted)
                    {
                        _PurchaseReturn.IsPosted = objPurchaseReturn.IsPosted;
                        _PurchaseReturn.PostedBy = SessionHelper.UserID;
                        _PurchaseReturn.PostedOn = DateTime.Now;
                    }
                    else
                    {
                        _PurchaseReturn.IsPosted = objPurchaseReturn.IsPosted;
                        _PurchaseReturn.PostedBy = null;
                        _PurchaseReturn.PostedOn = null;
                    }
                    if (objPurchaseReturn.IsCancelled)
                    {
                        _PurchaseReturn.IsCancelled = objPurchaseReturn.IsCancelled;
                        _PurchaseReturn.CancelledOn = DateTime.Now;
                        _PurchaseReturn.CancelledBy = SessionHelper.UserID;
                    }
                    else
                    {
                        _PurchaseReturn.IsCancelled = objPurchaseReturn.IsCancelled;
                        _PurchaseReturn.CancelledOn = null;
                        _PurchaseReturn.CancelledBy = null;
                    }
                    _PurchaseReturn.ModifiedBy = SessionHelper.UserID;
                    _PurchaseReturn.ModifiedOn = DateTime.Now;
                    db.Entry(_PurchaseReturn).State = EntityState.Modified;
                    db.SaveChanges();
                    res = true;
                }
            }
            return res;
        }

        public Int64 PurchaseReturnProduct_Insert(Int64 PurchaseReturnId, Model.AMPurchaseReturnProduct objPurchaseReturnProduct)
        {
            if (objPurchaseReturnProduct.ItemId > 0)
            {
                Model.AMPurchaseReturnProduct _PurchaseReturnProduct = objPurchaseReturnProduct;
                _PurchaseReturnProduct.PurchaseReturnId = PurchaseReturnId;
                _PurchaseReturnProduct.ItemId = objPurchaseReturnProduct.ItemId;
                _PurchaseReturnProduct.ManufacturerProductNo = objPurchaseReturnProduct.ManufacturerProductNo;
                _PurchaseReturnProduct.OrgWidth = objPurchaseReturnProduct.OrgWidth;
                _PurchaseReturnProduct.OrgLength = objPurchaseReturnProduct.OrgLength;
                _PurchaseReturnProduct.CalWidth = objPurchaseReturnProduct.CalWidth;
                _PurchaseReturnProduct.CalLength = objPurchaseReturnProduct.CalLength;
                _PurchaseReturnProduct.CalDigit = objPurchaseReturnProduct.CalDigit;
                _PurchaseReturnProduct.Quantity = objPurchaseReturnProduct.Quantity;
                _PurchaseReturnProduct.SqFeet = objPurchaseReturnProduct.SqFeet;
                _PurchaseReturnProduct.UnitPrice = objPurchaseReturnProduct.UnitPrice;
                _PurchaseReturnProduct.LineTotal = objPurchaseReturnProduct.LineTotal;
                _PurchaseReturnProduct.Discount = objPurchaseReturnProduct.Discount;
                _PurchaseReturnProduct.Tax = objPurchaseReturnProduct.Tax;
                _PurchaseReturnProduct.NetTotal = objPurchaseReturnProduct.NetTotal;
                _PurchaseReturnProduct.WareHouseId = objPurchaseReturnProduct.WareHouseId;
                db.AMPurchaseReturnProducts.Add(_PurchaseReturnProduct);
                db.SaveChanges();
            }
            return PurchaseReturnId;
        }

        public bool PurchaseReturnProduct_Update(Model.AMPurchaseReturnProduct objPurchaseReturnProduct)
        {
            bool res = false;
            if (objPurchaseReturnProduct.PurchaseReturnProductId > 0)
            {
                Model.AMPurchaseReturnProduct _PurchaseReturnProduct = db.AMPurchaseReturnProducts.Where(u => u.PurchaseReturnProductId == objPurchaseReturnProduct.PurchaseReturnProductId).FirstOrDefault();
                if (_PurchaseReturnProduct != null)
                {
                    //_PurchaseReturnProduct.PurchaseReturnId = PurchaseReturnId;
                    _PurchaseReturnProduct.ItemId = objPurchaseReturnProduct.ItemId;
                    _PurchaseReturnProduct.ManufacturerProductNo = objPurchaseReturnProduct.ManufacturerProductNo;
                    _PurchaseReturnProduct.OrgWidth = objPurchaseReturnProduct.OrgWidth;
                    _PurchaseReturnProduct.OrgLength = objPurchaseReturnProduct.OrgLength;
                    _PurchaseReturnProduct.CalWidth = objPurchaseReturnProduct.CalWidth;
                    _PurchaseReturnProduct.CalLength = objPurchaseReturnProduct.CalLength;
                    _PurchaseReturnProduct.Sheets = objPurchaseReturnProduct.Sheets;
                    _PurchaseReturnProduct.CalDigit = objPurchaseReturnProduct.CalDigit;
                    _PurchaseReturnProduct.Quantity = objPurchaseReturnProduct.Quantity;
                    _PurchaseReturnProduct.SqFeet = objPurchaseReturnProduct.SqFeet;
                    _PurchaseReturnProduct.UnitPrice = objPurchaseReturnProduct.UnitPrice;
                    _PurchaseReturnProduct.LineTotal = objPurchaseReturnProduct.LineTotal;
                    _PurchaseReturnProduct.Discount = objPurchaseReturnProduct.Discount;
                    _PurchaseReturnProduct.Tax = objPurchaseReturnProduct.Tax;
                    _PurchaseReturnProduct.NetTotal = objPurchaseReturnProduct.NetTotal;
                    _PurchaseReturnProduct.WareHouseId = objPurchaseReturnProduct.WareHouseId;
                    db.Entry(_PurchaseReturnProduct).State = EntityState.Modified;
                    db.SaveChanges();
                    res = true;
                }
            }
            return res;
        }

        public ActionResult LoadPurchaseReturnInvoicePayments(long? InvoiceNo = 0)
        {
            var list = db.AMSupplierRefundInvoices
                .Where(u => u.BranchId == branch_ID && u.PurchaseReturnId == InvoiceNo)
                .ToList()
                .Select(u => new
                {
                    Date = u.CreatedOn == null ? "" : u.CreatedOn.Value.ToString("dd MMM yyyy"),
                    Amount = u.Amount,
                    Description = u.Description
                }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public int? SaveSupplierRefunds(Model.AMSupplierRefund objSupplierRefund)
        {
            Model.AMSupplierRefund _SupplierRefund = objSupplierRefund;
            if (_SupplierRefund != null)
            {
                _SupplierRefund.CreatedBy = SessionHelper.UserID;
                _SupplierRefund.CreatedOn = DateTime.Now;
                _SupplierRefund.BranchId = branch_ID;
                _SupplierRefund.IsPosted = false;
                _SupplierRefund.IsCancelled = false;
                db.AMSupplierRefunds.Add(_SupplierRefund);
                db.SaveChanges();
                return _SupplierRefund.SupplierRefundId;
            }
            else
            {
                return null;
            }
        }

        public bool SaveSupplierRefundInvoices(Model.AMSupplierRefundInvoice objSupplierRefundInvoice)
        {
            Model.AMSupplierRefundInvoice _SupplierRefundInvoice = objSupplierRefundInvoice;
            if (_SupplierRefundInvoice.PurchaseReturnId > 0)
            {
                _SupplierRefundInvoice.CreatedBy = SessionHelper.UserID;
                _SupplierRefundInvoice.CreatedOn = DateTime.Now;
                _SupplierRefundInvoice.BranchId = branch_ID;
                _SupplierRefundInvoice.IsPosted = false;
                _SupplierRefundInvoice.IsCancelled = false;
                db.AMSupplierRefundInvoices.Add(_SupplierRefundInvoice);
                db.SaveChanges();
                AMProceduresModel.t_POSSupplierRefundInvoice(db, _SupplierRefundInvoice.PurchaseReturnId, branch_ID);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ManagePurchaseReturns

        public ActionResult ManagePurchaseReturns(int? page)
        {

            PurchaseReturnModelViewModel ex = new PurchaseReturnModelViewModel();
            DateTime now = DateTime.Now;
            //ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.FromDate = string.Format("{0:dd/MM/yyyy}", SessionHelper.FiscalStartDate);
            ex.ToDate = now.ToShortDateString();
            ex.All = true;
            ex.WithBalance = false;
            ex.PurchaseReturnPagedList = PurchaseReturnPagedList(ex, page);
            return View(ex);
        }

        [HttpPost]
        public PartialViewResult ManagePurchaseReturns(PurchaseReturnModelViewModel ex, int? page)
        {
            ex.PurchaseReturnPagedList = PurchaseReturnPagedList(ex, page);
            ModelState.Clear();
            return PartialView("_PartialPurchaseReturnList", ex);
        }

        IPagedList<Model.AMPurchaseReturn> PurchaseReturnPagedList(PurchaseReturnModelViewModel ex, int? page)
        {
            var list = db.AMPurchaseReturns.Where(u => u.BranchId == branch_ID).ToList();
            if (!string.IsNullOrEmpty(ex.FromDate) && !string.IsNullOrEmpty(ex.ToDate))
            {
                var fdate = Convert.ToDateTime(ex.FromDate);
                var tdate = Convert.ToDateTime(ex.ToDate);
                list = db.AMPurchaseReturns.Where(u => u.BranchId == branch_ID && DbFunctions.TruncateTime(u.PurchaseReturnDate) >= fdate && DbFunctions.TruncateTime(u.PurchaseReturnDate) <= tdate).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(ex.FromDate))
                {
                    var date = Convert.ToDateTime(ex.FromDate);
                    list = db.AMPurchaseReturns.Where(u => u.BranchId == branch_ID && DbFunctions.TruncateTime(u.PurchaseReturnDate) >= date).ToList();
                }
                if (!string.IsNullOrEmpty(ex.ToDate))
                {
                    var date = Convert.ToDateTime(ex.ToDate);
                    list = db.AMPurchaseReturns.Where(u => u.BranchId == branch_ID && DbFunctions.TruncateTime(u.PurchaseReturnDate) <= date).ToList();
                }
            }
            if (!string.IsNullOrEmpty(ex.Search))
            {
                ex.Search = ex.Search.ToUpper();
                list = list.Where(u => (u.Client == null ? "" : u.Client.Name.ToUpper()).Contains(ex.Search.ToUpper()) ||
                    (u.PurchaseReturnId.ToString() == ex.Search) ||
                    (u.Description == null ? "" : u.Description.ToUpper()).Contains(ex.Search.ToUpper())).ToList();
            }
            if (!ex.All)
            {
                if (ex.WithBalance)
                {
                    list = list.Where(u => (u.NetTotal - u.Received) > 0).ToList();
                }
                else
                {
                    list = list.Where(u => (u.NetTotal - u.Received) == 0).ToList();
                }
            }

            ex.PurchaseReturnPagedList = list.ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, 20);
            return ex.PurchaseReturnPagedList;
        }


        #endregion

        #region MakePayment
        List<v_mnl_PurchaseInvoices_Result> PurchaseInvoiceList(PurchaseModelViewModel ex)
        {
            var list = ProceduresModel.v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID).ToList();
            if (!string.IsNullOrEmpty(ex.FromDate) && !string.IsNullOrEmpty(ex.ToDate))
            {
                var fdate = Convert.ToDateTime(ex.FromDate);
                var tdate = Convert.ToDateTime(ex.ToDate);
                list = ProceduresModel.v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID && u.PurchaseInvoiceDate.Date >= fdate && u.PurchaseInvoiceDate <= tdate).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(ex.FromDate))
                {
                    var date = Convert.ToDateTime(ex.FromDate);
                    list = ProceduresModel.v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID && u.PurchaseInvoiceDate >= date).ToList();
                }
                if (!string.IsNullOrEmpty(ex.ToDate))
                {
                    var date = Convert.ToDateTime(ex.ToDate);
                    list = ProceduresModel.v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID && u.PurchaseInvoiceDate <= date).ToList();
                }
            }
            if (!string.IsNullOrEmpty(ex.Search))
            {
                ex.Search = ex.Search.ToUpper();
                list = list.Where(u => (u.ClientName == null ? "" : u.ClientName.ToUpper()).Contains(ex.Search.ToUpper()) ||
                    (u.PurchaseInvoiceId.ToString() == ex.Search) ||
                    (u.Description == null ? "" : u.Description.ToUpper()).Contains(ex.Search.ToUpper())).ToList();
            }
            if (ex.All) { }
            else
            {
                if (ex.WithBalance)
                {
                    list = list.Where(u => (u.NetTotal - u.ReceivedAmount) > 0).ToList();
                }
                else
                {
                    list = list.Where(u => (u.NetTotal - u.ReceivedAmount) == 0).ToList();
                }
            }
            if (ex.SupplierId != null)
            {
                list = list.Where(u => u.SupplierId == ex.SupplierId).ToList();
            }
            if (ex.Posted && ex.UnPosted)
            {
            }
            else
            {
                if (ex.Posted)
                {
                    list = list.Where(u => u.IsPosted == true).ToList();
                }

                if (ex.UnPosted)
                {
                    list = list.Where(u => u.IsPosted == false).ToList();
                }
            }
            if (ex.PostedToAccount)
            {
                list = list.Where(u => u.IsPosted == true && u.IsAccountPosted == true).ToList();
            }

            if (ex.UnPostedToAccount)
            {
                list = list.Where(u => u.IsPosted == true && u.IsAccountPosted == false).ToList();
            }

            ex.v_mnl_PurchaseInvoiceList = list;
            return ex.v_mnl_PurchaseInvoiceList;
        }

        public ActionResult MakePayment(int? id = 0)
        {

            FillDD_Payments();
            PurchaseModelViewModel ex = new PurchaseModelViewModel();
            ex.SupplierPayment = new SupplierPayment();
            ViewBag.InvoiceDetailExists = false;
            if (id > 0)
            {
                ex.WithBalance = true;
                ex.SupplierPayment = db.SupplierPayments.Where(u => u.SupplierPaymentId == id).FirstOrDefault();
                if (ex.SupplierPayment != null)
                {
                    ex.SupplierId = ex.SupplierPayment.SupplierId;
                    ex.SupplierInvoicePaymentList = db.SupplierInvoicePayments.Where(u => u.SupplierPaymentId == ex.SupplierPayment.SupplierPaymentId && u.ModuleId == module_ID).ToList();
                    if (ex.SupplierInvoicePaymentList != null)
                    {
                        ViewBag.InvoiceDetailExists = true;
                    }

                    if (ex.SupplierPayment.PaymentType == "Cash")
                    {
                        ex.CashAccountId = ex.SupplierPayment.AccountId;
                    }
                    else
                    {
                        ex.BankAccountId = ex.SupplierPayment.AccountId;
                    }

                    ex.v_mnl_PurchaseInvoiceList = PurchaseInvoiceList(ex);
                }
                ViewBag.InvoiceExists = true;
            }
            else
            {
                ViewBag.InvoiceExists = false;
            }

            return View(ex);
        }
        void FillDD_Payments()
        {
            ViewBag.Suppliers = db.Clients.Where(u => u.BranchId == branch_ID && u.IsSupplier == true).ToList();
            List<SelectListItem> PaymentMode = new List<SelectListItem>();
            PaymentMode.Add(new SelectListItem() { Text = "Cash", Value = "Cash" });
            PaymentMode.Add(new SelectListItem() { Text = "Bank", Value = "Bank" });
            ViewBag.PaymentMode = PaymentMode;
            ViewBag.CashAccount = AMProceduresModel.p_mnl_Account_GetCashAndBankAccounts(db, true, false, branch_ID, null).ToList();
            ViewBag.BankAccount = AMProceduresModel.p_mnl_Account_GetCashAndBankAccounts(db, false, true, branch_ID, null).ToList();

        }

        [HttpPost]
        public ActionResult MakePayment(PurchaseModelViewModel ex, string Command)
        {
            bool status = false;
            string msg = "";
            var redirecToPayment = 0;
            var SupplierPaymentId = 0;
            FillDD_Payments();
            ViewBag.InvoiceExists = false;
            ViewBag.InvoiceDetailExists = false;
            switch (Command)
            {
                case "SavePayment":
                    ViewBag.success = "Payment not saved";
                    if (ex.SupplierPayment != null)
                    {
                        if (ex.SupplierPayment.Amount > 0)
                        {
                            if (ex.SupplierPayment.PaymentType == "Cash")
                            {
                                ex.SupplierPayment.AccountId = ex.CashAccountId;
                            }
                            else
                            {
                                ex.SupplierPayment.AccountId = ex.BankAccountId;
                            }

                            SupplierPaymentId = (int)SaveSupplierPayments(ex.SupplierPayment);
                            if (SupplierPaymentId > 0)
                            {
                                ex.SupplierPayment.SupplierPaymentId = Convert.ToInt32(SupplierPaymentId);
                            }
                        }
                        status = true;
                        msg = "Payment Saved";
                        redirecToPayment = SupplierPaymentId;
                    }
                    break;

                case "updatePayment":
                    //ViewBag.success = "Payment not saved";
                    if (ex.SupplierPayment != null)
                    {
                        var SupplierPayment = db.SupplierPayments.Where(m => m.BranchId == branch_ID && m.SupplierPaymentId == ex.SupplierPayment.SupplierPaymentId).FirstOrDefault();
                        SupplierPayment.Description = ex.SupplierPayment.Description;
                        SupplierPayment.ChequeNo = ex.SupplierPayment.ChequeNo;
                        SupplierPayment.ChequeDate = ex.SupplierPayment.ChequeDate;
                        SupplierPayment.PaymentType = ex.SupplierPayment.PaymentType;
                        SupplierPayment.PaymentDate = ex.SupplierPayment.PaymentDate;

                        if (ex.SupplierPayment.PaymentType.ToLower() == "Cash".ToLower())
                        {
                            SupplierPayment.AccountId = ex.CashAccountId;
                        }
                        else
                        {
                            SupplierPayment.AccountId = ex.BankAccountId;
                        }
                        SupplierPayment.Amount = ex.SupplierPayment.Amount;

                        SupplierPayment.ModifiedBy = SessionHelper.UserId;
                        SupplierPayment.ModifiedOn = DateTime.Now;

                        db.SaveChanges();

                        status = true;
                        msg = "Payment Updated";

                    }
                    break;
                case "SavePaymentInvoice":
                    ViewBag.success = "Payment not saved";
                    if (ex.SupplierPayment != null)
                    {
                        if (ex.SupplierPayment.Amount > 0 && ex.SupplierPayment.SupplierPaymentId > 0)
                        {
                            if (ex.SupplierPayment.PaymentType == "Cash")
                            {
                                ex.SupplierPayment.AccountId = ex.CashAccountId;
                            }
                            else
                            {
                                ex.SupplierPayment.AccountId = ex.BankAccountId;
                            }

                            if (ex.SupplierInvoicePaymentList != null)
                            {
                                SupplierInvoicePayment _SupplierInvoicePayment;
                                foreach (var item in ex.SupplierInvoicePaymentList)
                                {
                                    if (item.Amount > 0)
                                    {
                                        _SupplierInvoicePayment = db.SupplierInvoicePayments.Where(u => u.SupplierInvoicePaymentId == item.SupplierInvoicePaymentId && u.ModuleId == module_ID).FirstOrDefault();
                                        if (_SupplierInvoicePayment != null)
                                        {
                                            _SupplierInvoicePayment.ModuleId = module_ID;
                                            _SupplierInvoicePayment.Amount = item.Amount;
                                            _SupplierInvoicePayment.Description = item.Description;
                                            _SupplierInvoicePayment.ModifiedBy = SessionHelper.UserID;
                                            _SupplierInvoicePayment.ModifiedOn = DateTime.Now;
                                            if (_SupplierInvoicePayment.VoucherDetailId > 0)
                                            {
                                                UpdateVoucherDetailForPayment(Convert.ToInt64(_SupplierInvoicePayment.VoucherDetailId), ex.SupplierPayment.AccountId, Convert.ToDouble(_SupplierInvoicePayment.Amount));
                                            }

                                            db.Entry(_SupplierInvoicePayment).State = EntityState.Modified;
                                            db.SaveChanges();
                                            ViewBag.success = "Payment saved successfully";
                                            AMProceduresModel.t_POSSupplierInvoicePayment(db, item.PurchaseInvoiceId, branch_ID, module_ID);

                                        }
                                    }
                                }
                            }
                            if (ex.v_mnl_PurchaseInvoiceList != null)
                            {
                                SupplierInvoicePayment _SupplierInvoicePayment;
                                foreach (var item in ex.v_mnl_PurchaseInvoiceList)
                                {
                                    if (item.Paid > 0)
                                    {
                                        _SupplierInvoicePayment = new SupplierInvoicePayment();
                                        _SupplierInvoicePayment.ModuleId = module_ID;
                                        _SupplierInvoicePayment.SupplierPaymentId = ex.SupplierPayment.SupplierPaymentId;
                                        _SupplierInvoicePayment.PurchaseInvoiceId = item.PurchaseInvoiceId;
                                        _SupplierInvoicePayment.Amount = item.Paid;
                                        _SupplierInvoicePayment.Description = ex.SupplierPayment.Description;
                                        SaveSupplierInvoicePayments(_SupplierInvoicePayment, ex.SupplierPayment.AccountId);
                                        ViewBag.success = "Payment saved successfully";
                                    }
                                }
                            }
                        }
                    }
                    break;
            }
            if (ex.SupplierPayment.SupplierId > 0)
            {
                ex.WithBalance = true;
                ex.SupplierId = ex.SupplierPayment.SupplierId;
                if (ex.SupplierPayment.SupplierPaymentId > 0)
                {
                    var _SupplierPayment = db.SupplierPayments.Where(u => u.SupplierPaymentId == ex.SupplierPayment.SupplierPaymentId).FirstOrDefault();
                    ex.SupplierPayment = _SupplierPayment;
                }
                if (ex.SupplierPayment != null)
                {
                    var SupplierInvoicePaymentList = db.SupplierInvoicePayments.Where(u => u.SupplierPaymentId == ex.SupplierPayment.SupplierPaymentId && u.ModuleId == module_ID).ToList();
                    ex.SupplierInvoicePaymentList = SupplierInvoicePaymentList;
                    if (ex.SupplierInvoicePaymentList != null)
                    {
                        ViewBag.InvoiceDetailExists = true;
                    }

                    if (ex.SupplierPayment.PaymentType == "Cash")
                    {
                        ex.CashAccountId = ex.SupplierPayment.AccountId;
                    }
                    else
                    {
                        ex.BankAccountId = ex.SupplierPayment.AccountId;
                    }
                }
                ex.v_mnl_PurchaseInvoiceList = PurchaseInvoiceList(ex);
                ViewBag.InvoiceExists = true;
                status = true;
                msg = " payment updated, Invoices and Payments Loaded";
            }
            List<Service.Helper> PartialList = new List<Service.Helper>();
            //PartialList.Add(new Service.Helper { divToReplace = "adjustableAmountSection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReceivePaymentAdujstableAmount", ex) });
            //PartialList.Add(new Service.Helper { divToReplace = "cleintInvoicePayments", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReceivePaymentMatchedInvoices", ex) });
            //PartialList.Add(new Service.Helper { divToReplace = "invoicesSection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReceivePaymentUnPaidInvoices", ex) });

            PartialList = MakePaymentPartialViews(ex);

            var res = new
            {
                status = status,
                msg = msg,
                PartialList = PartialList,
                redirecToPayment = redirecToPayment
            };


            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveMatchedInvoice(int? SupplierPaymentId, long? PurchaseInvoiceId)
        {
            bool status = false;
            string msg = "";
            var ex = new PurchaseModelViewModel();

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {


                    var SupplierPayment = db.SupplierPayments.Where(m => m.SupplierPaymentId == SupplierPaymentId && m.BranchId == branch_ID)
                        .Include(m => m.Client)
                        .FirstOrDefault();

                    var SupplierInvoicePayments = db.SupplierInvoicePayments.Where(u => u.SupplierPaymentId == SupplierPayment.SupplierPaymentId && u.ModuleId == module_ID).ToList();

                    var PurchaseInvoice = db.AMPurchaseInvoices.Where(m => m.PurchaseInvoiceId == PurchaseInvoiceId && m.BranchId == branch_ID)
                        .FirstOrDefault();
                    var SupplierPaymentAmount = SupplierPayment.Amount;

                    var SupplierPaymentBalance = (decimal)(SupplierPaymentAmount - ((SupplierInvoicePayments.Sum(m => m.Amount))));

                    var InvoiceBalance = (PurchaseInvoice.NetTotal - (PurchaseInvoice.Received ?? 0));
                    var payableAmount = 0m;

                    if (InvoiceBalance >= SupplierPaymentBalance)
                    {
                        payableAmount = SupplierPaymentBalance;

                    }
                    else
                    {
                        if (InvoiceBalance < (decimal)(SupplierPaymentAmount - ((SupplierInvoicePayments.Sum(m => m.Amount)))))
                        {

                            payableAmount = (decimal)InvoiceBalance;


                        }

                        else
                        {

                            payableAmount = (decimal)InvoiceBalance - (decimal)(SupplierPaymentAmount - (decimal)((SupplierInvoicePayments.Sum(m => m.Amount))));

                        }
                    }




                    //if (InvoiceBalance < (decimal)(ClientPaymentAmout - ((ClientInvoicePayments.Sum(m => m.Amount)))) )
                    //{
                    //    payableAmount = (decimal)InvoiceBalance;
                    //}

                    //else
                    //{
                    //    if ( ) {
                    //    }

                    //    payableAmount = (decimal)InvoiceBalance - (decimal)(ClientPaymentAmout - ((ClientInvoicePayments.Sum(m => m.Amount))));

                    //}

                    var _SupplierInvoicePayment = new SupplierInvoicePayment(); ;
                    _SupplierInvoicePayment.SupplierPaymentId = SupplierPayment.SupplierPaymentId;
                    _SupplierInvoicePayment.PurchaseInvoiceId = PurchaseInvoice.PurchaseInvoiceId;
                    _SupplierInvoicePayment.Amount = payableAmount;
                    _SupplierInvoicePayment.Description = SupplierPayment.Description;
                    _SupplierInvoicePayment.PaymentDate = SupplierPayment.PaymentDate;
                    _SupplierInvoicePayment.ModuleId = module_ID;
                    SaveSupplierInvoicePayments(_SupplierInvoicePayment, SupplierPayment.AccountId);


                    if (SupplierPayment.SupplierId > 0)
                    {
                        ex.WithBalance = true;
                        ex.SupplierId = SupplierPayment.SupplierId;

                        if (SupplierPayment.SupplierPaymentId > 0)
                        {

                            ex.SupplierPayment = SupplierPayment;
                        }
                        if (ex.SupplierPayment != null)
                        {
                            var SupplierInvoicePaymentList = db.SupplierInvoicePayments.Where(u => u.SupplierPaymentId == SupplierPayment.SupplierPaymentId && u.ModuleId == module_ID).ToList();
                            ex.SupplierInvoicePaymentList = SupplierInvoicePaymentList;
                            if (ex.SupplierInvoicePaymentList != null)
                            {
                                ViewBag.InvoiceDetailExists = true;
                            }

                            if (ex.SupplierPayment.PaymentType == "Cash")
                            {
                                ex.CashAccountId = SupplierPayment.AccountId;
                            }
                            else
                            {
                                ex.BankAccountId = SupplierPayment.AccountId;
                            }


                        }
                        ex.v_mnl_PurchaseInvoiceList = PurchaseInvoiceList(ex);
                        ViewBag.InvoiceExists = true;

                    }
                    trans.Commit();
                    status = true;
                    msg = "Payment Matched and Saved";
                }

                catch (Exception)
                {

                    status = false;
                    msg = "Payment not Saved....";
                    trans.Rollback();

                }
            }
            List<Service.Helper> PartialList = new List<Service.Helper>();

            //PartialList.Add(new Service.Helper { divToReplace = "adjustableAmountSection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReceivePaymentAdujstableAmount", ex) });
            //PartialList.Add(new Service.Helper { divToReplace = "cleintInvoicePayments", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReceivePaymentMatchedInvoices", ex) });
            //PartialList.Add(new Service.Helper { divToReplace = "invoicesSection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialReceivePaymentUnPaidInvoices", ex) });

            PartialList = MakePaymentPartialViews(ex);


            var res = new
            {
                status = status,
                msg = msg,
                PartialList = PartialList
            };


            return Json(res, JsonRequestBehavior.AllowGet);
        }


        public int? SaveSupplierPayments(SupplierPayment objSupplierPayment)
        {
            SupplierPayment _SupplierPayment = objSupplierPayment;
            if (_SupplierPayment != null)
            {
                _SupplierPayment.CreatedBy = SessionHelper.UserID;
                _SupplierPayment.CreatedOn = DateTime.Now;
                _SupplierPayment.BranchId = branch_ID;
                _SupplierPayment.IsPosted = false;
                _SupplierPayment.IsCancelled = false;
                _SupplierPayment.VoucherId = CreateVoucherForPayment(_SupplierPayment.SupplierId);
                db.SupplierPayments.Add(_SupplierPayment);
                db.SaveChanges();
                return _SupplierPayment.SupplierPaymentId;
            }
            else
            {
                return null;
            }
        }
        public int? InsertUpdateSupplierPayments(SupplierPayment objSupplierPayment)
        {
            SupplierPayment _SupplierPayment = objSupplierPayment;
            if (_SupplierPayment != null)
            {
                if (_SupplierPayment.SupplierPaymentId > 0)
                {
                    var SupplierPaymentss = db.SupplierPayments.Where(u => u.SupplierPaymentId == _SupplierPayment.SupplierPaymentId).FirstOrDefault();
                    SupplierPaymentss.ModifiedBy = SessionHelper.UserID;
                    SupplierPaymentss.ModifiedOn = DateTime.Now;
                    SupplierPaymentss.Amount += _SupplierPayment.Amount;
                    db.Entry(SupplierPaymentss).State = EntityState.Modified;
                }
                else
                {
                    _SupplierPayment.CreatedBy = SessionHelper.UserID;
                    if (!_SupplierPayment.CreatedOn.HasValue)
                    {
                        _SupplierPayment.CreatedOn = DateTime.Now;
                    }

                    _SupplierPayment.BranchId = branch_ID;
                    _SupplierPayment.IsPosted = false;
                    _SupplierPayment.IsCancelled = false;
                    //_SupplierPayment.VoucherId = CreateVoucherForPayment(_SupplierPayment.SupplierId);
                    db.SupplierPayments.Add(_SupplierPayment);
                }
                db.SaveChanges();

                return _SupplierPayment.SupplierPaymentId;
            }
            else
            {
                return null;
            }
        }

        public bool SaveSupplierInvoicePayments(SupplierInvoicePayment objSupplierInvoicePayment, string CBAccountId)
        {
            SupplierInvoicePayment _SupplierInvoicePayment = objSupplierInvoicePayment;
            if (_SupplierInvoicePayment.PurchaseInvoiceId > 0)
            {
                _SupplierInvoicePayment.CreatedBy = SessionHelper.UserID;
                _SupplierInvoicePayment.CreatedOn = DateTime.Now;
                _SupplierInvoicePayment.BranchId = branch_ID;
                _SupplierInvoicePayment.IsPosted = false;
                _SupplierInvoicePayment.IsCancelled = false;
                _SupplierInvoicePayment.ModuleId = module_ID;
                if (_SupplierInvoicePayment.SupplierPaymentId > 0)
                {
                    var voucherid = db.SupplierPayments.Where(u => u.SupplierPaymentId == _SupplierInvoicePayment.SupplierPaymentId).Select(u => u.VoucherId).FirstOrDefault();
                    if (voucherid != null)
                    {
                        _SupplierInvoicePayment.VoucherDetailId = CreateVoucherDetailForPayment(Convert.ToInt16(_SupplierInvoicePayment.SupplierPaymentId), Convert.ToInt64(voucherid), CBAccountId, Convert.ToDouble(_SupplierInvoicePayment.Amount));
                        AMProceduresModel.t_VoucherDetail(db, voucherid);
                    }
                }
                db.SupplierInvoicePayments.Add(_SupplierInvoicePayment);
                db.SaveChanges();
                AMProceduresModel.t_POSSupplierInvoicePayment(db, _SupplierInvoicePayment.PurchaseInvoiceId, branch_ID, module_ID);
                return true;
            }
            else
            {
                return false;
            }
        }

        public JsonResult DeleteInvoicePayment(int? id)
        {
            var docSpec = db.SupplierInvoicePayments.Where(u => u.SupplierInvoicePaymentId == id && u.ModuleId == module_ID).FirstOrDefault();
            if (docSpec != null)
            {
                var PurchaseInvoiceId = docSpec.PurchaseInvoiceId;
                //if (docSpec.VoucherDetailId > 0)
                //{
                //    if (docSpec.VoucherDetail != null)
                //    {
                //        var idd = docSpec.VoucherDetail.VoucherId;
                //        docSpec.VoucherDetail.Debit = docSpec.VoucherDetail.Credit = 0;
                //        AMProceduresModel.VoucherDetail_Update(db, docSpec.VoucherDetail);
                //        AMProceduresModel.t_VoucherDetail(db, idd);
                //        db.VoucherDetails.Remove(docSpec.VoucherDetail);
                //    }
                //}
                db.SupplierInvoicePayments.Remove(docSpec);
                if (PurchaseInvoiceId > 0)
                {
                    AMProceduresModel.t_POSSupplierInvoicePayment(db, PurchaseInvoiceId, branch_ID, module_ID);
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult CreateModifySupplierPaymentVoucher(int SupplierPaymentId, bool isPosted)
        {
            bool status = false;

            SupplierPaymentBLL SupplierPaymentBLL = new SupplierPaymentBLL();
            if (SupplierPaymentBLL.CreateModifySupplierPaymentVoucher(db, SupplierPaymentId, SessionHelper.UserId, isPosted).HasValue)
            {
                status = true;

            }
            else
            {

                status = false;

            }

            var res = new
            {
                status = status
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        long? CreateVoucherForPayment(int SupplierId)
        {
            long? VoucherId;
            var CurrencyId = db.Currencies.Select(u => u.CurrencyId).FirstOrDefault();
            var value = db.CurrencyValues.Where(u => u.CurrencyId == CurrencyId).Select(p => p.Value).FirstOrDefault();
            Voucher _vouvher = new Voucher();
            _vouvher.VoucherType = "PIP";
            _vouvher.TransactionDate = DateTime.Now;
            _vouvher.VoucherStatus = "Draft";
            _vouvher.CurrencyId = CurrencyId;
            _vouvher.ExchangeRate = value;
            _vouvher.CBAccountId = db.Clients.Where(u => u.ClientId == SupplierId).Select(u => u.AccountId).FirstOrDefault();
            _vouvher.Particulars = "Supplier Payment for PI";
            VoucherId = ProceduresModel.Voucher_Insert(db, _vouvher);
            return VoucherId;
        }

        bool UpdateVoucherForPayment(long VoucherId, int SupplierId)
        {
            var result = false;
            Voucher _vouvher = db.Vouchers.Where(u => u.VoucherId == VoucherId).FirstOrDefault();
            if (_vouvher != null)
            {
                _vouvher.CBAccountId = db.Clients.Where(u => u.ClientId == SupplierId).Select(u => u.AccountId).FirstOrDefault();
                result = AMProceduresModel.Voucher_Update(db, null, _vouvher);
            }
            return result;
        }

        long? CreateVoucherDetailForPayment(int SupplierPaymetId, long VoucherId, string AccountId, double Amount)
        {
            long? VoucherdetailId;
            var vDetails = new VoucherDetail();
            vDetails.VoucherId = VoucherId;
            vDetails.AccountId = AccountId;
            vDetails.Credit = vDetails.Debit = Convert.ToDecimal(Amount);
            vDetails.Narration = "Supplier Paymet for PI # " + SupplierPaymetId;
            VoucherdetailId = ProceduresModel.VoucherDetail_Insert(db, VoucherId, "PIP", vDetails);
            return VoucherdetailId;
        }

        bool UpdateVoucherDetailForPayment(long VoucherDetailId, string AccountId, double Amount)
        {
            var result = false;
            VoucherDetail _VoucherDetail = db.VoucherDetails.Where(u => u.VoucherId == VoucherDetailId).FirstOrDefault();
            if (_VoucherDetail != null)
            {
                _VoucherDetail.AccountId = AccountId;
                _VoucherDetail.Credit = _VoucherDetail.Debit = Convert.ToDecimal(Amount);
                result = ProceduresModel.VoucherDetail_Update(db, _VoucherDetail);
                ProceduresModel.t_VoucherDetail(db, _VoucherDetail.VoucherId);
            }
            return result;
        }


        [HttpPost]
        public JsonResult DeleteSupplierInvoicePayment(int? id)
        {
            bool status = false;
            string msg = "";
            PurchaseModelViewModel ex = new PurchaseModelViewModel();
            try
            {
                var supplierInvoicePayment = db.SupplierInvoicePayments.Where(m => m.BranchId == branch_ID && m.SupplierInvoicePaymentId == id && m.ModuleId == module_ID).FirstOrDefault();
                var SupplierPayment = db.SupplierPayments.Where(m => m.SupplierPaymentId == supplierInvoicePayment.SupplierPaymentId).FirstOrDefault();
                if (supplierInvoicePayment != null)
                {
                    var PurchaseInvoice = db.AMPurchaseInvoices.Where(m => m.PurchaseInvoiceId == supplierInvoicePayment.PurchaseInvoiceId && m.BranchId == branch_ID).FirstOrDefault();
                    if (PurchaseInvoice != null)
                    {
                        PurchaseInvoice.Received = PurchaseInvoice.Received - supplierInvoicePayment.Amount;
                    }
                }

                db.SupplierInvoicePayments.Remove(supplierInvoicePayment);
                db.SaveChanges();

                if (SupplierPayment.SupplierId > 0)
                {
                    ex.WithBalance = true;
                    ex.SupplierId = SupplierPayment.SupplierId;
                    if (SupplierPayment.SupplierPaymentId > 0)
                    {
                        ex.SupplierPayment = SupplierPayment;
                    }
                    if (ex.SupplierPayment != null)
                    {
                        var SupplierInvoicePaymentList = db.SupplierInvoicePayments.Where(u => u.SupplierPaymentId == SupplierPayment.SupplierPaymentId && u.ModuleId == module_ID).ToList();
                        ex.SupplierInvoicePaymentList = SupplierInvoicePaymentList;
                        if (ex.SupplierInvoicePaymentList != null)
                        {
                            ViewBag.InvoiceDetailExists = true;
                        }

                        if (ex.SupplierPayment.PaymentType == "Cash")
                        {
                            ex.CashAccountId = SupplierPayment.AccountId;
                        }
                        else
                        {
                            ex.BankAccountId = SupplierPayment.AccountId;
                        }
                    }
                    ex.v_mnl_PurchaseInvoiceList = PurchaseInvoiceList(ex);

                    ViewBag.InvoiceExists = true;
                    status = true;
                    msg = " payment updated, Invoices and Payments Loaded";
                }


                status = true;
                msg = "Deleted Successfully";

            }
            catch (Exception)
            {

                status = false;
                msg = "Deletion Failed !..";
            }

            List<Service.Helper> PartialList = new List<Service.Helper>();

            PartialList = MakePaymentPartialViews(ex);

            var res = new
            {
                status = status,
                Msg = msg,
                PartialList = PartialList,
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteSupplierPayment(int id)
        {
            bool status = false;
            string msg = "";
            try
            {
                var SupplierPayment = db.SupplierPayments.Where(m => m.BranchId == branch_ID && m.SupplierPaymentId == id).FirstOrDefault();
                var voucher = db.Vouchers.Where(m => m.BranchId == branch_ID && m.VoucherId == SupplierPayment.VoucherId).FirstOrDefault();

                var SupplierInvoicePayments = db.SupplierInvoicePayments.Where(m => m.BranchId == branch_ID && m.SupplierPaymentId == SupplierPayment.SupplierPaymentId && m.ModuleId == module_ID).ToList();

                //if (ClientInvoicePayments != null) {
                //    db.AMClientInvoicePayments.RemoveRange(ClientInvoicePayments);
                //}

                foreach (var item in SupplierInvoicePayments)
                {

                    var PurchaseInvoice = db.AMPurchaseInvoices.Where(m => m.BranchId == branch_ID && m.PurchaseInvoiceId == item.PurchaseInvoiceId).FirstOrDefault();

                    if (PurchaseInvoice != null)
                    {

                        PurchaseInvoice.Received = PurchaseInvoice.Received - item.Amount;

                    }

                    db.SupplierInvoicePayments.Remove(item);


                }


                if (voucher != null)
                {
                    voucher.IsCancelled = true;
                    voucher.VoucherStatus = "Draft";
                    voucher.IsPosted = false;
                }

                db.SupplierPayments.Remove(SupplierPayment);
                db.SaveChanges();

                msg = "Delete Successfully";
                status = true;
            }
            catch (Exception ex)
            {
                msg = ex.GetExceptionMessages();

            }
            var res = new
            {
                status = status,
                msg = msg
            };

            return Json(res, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public JsonResult updatePaymentMethod(int? id, string paymentType)
        {
            bool status = false;
            string msg = "";
            try
            {
                var supplierPayment = db.SupplierPayments.Where(m => m.SupplierPaymentId == id && m.BranchId == branch_ID).FirstOrDefault();
                supplierPayment.PaymentType = paymentType;
                db.SaveChanges();
                status = true;
                msg = "Payment type updated";


            }
            catch (Exception)
            {
                status = false;
                msg = "Failed Payment type not updated";

            }
            var res = new
            {
                status = status,
                Msg = msg

            };

            return Json(res, JsonRequestBehavior.AllowGet);


        }


        private List<Service.Helper> MakePaymentPartialViews(PurchaseModelViewModel ex)
        {

            List<Service.Helper> PartialList = new List<Service.Helper>();
            PartialList.Add(new Service.Helper { divToReplace = "adjustableAmountSection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialMakePaymentAdujstableAmount", ex) });
            PartialList.Add(new Service.Helper { divToReplace = "InvoicePayments", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialMakePaymentMatchedInvoices", ex) });
            PartialList.Add(new Service.Helper { divToReplace = "invoicesSection", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialMakePaymentUnPaidInvoices", ex) });

            return PartialList;
        }


        #endregion

        #region ManageInvoicePayments

        public ActionResult ManageInvoicePayments(int? page)
        {

            PurchaseModelViewModel ex = new PurchaseModelViewModel();
            DateTime now = DateTime.Now;
            //ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.FromDate = string.Format("{0:dd/MM/yyyy}", SessionHelper.FiscalStartDate);
            ex.ToDate = now.ToShortDateString();
            ex.SupplierPaymentPagedList = SupplierPaymentsList(ex, page);
            ex.SupplierPaymentPagedList = SupplierPaymentsList(ex, page);
            ex.RelatedSupplierInvoicePayments = new List<SupplierInvoicePayment>();
            foreach (var item in ex.SupplierPaymentPagedList)
            {
                var SupplierInvoicePayments = db.SupplierInvoicePayments.Where(m => m.SupplierPaymentId == item.SupplierPaymentId && m.ModuleId == module_ID);
                foreach (var Sip in SupplierInvoicePayments)
                {
                    ex.RelatedSupplierInvoicePayments.Add(Sip);
                }
            }
            ex.RelatedSupplierInvoicePayments.RemoveAll(m => m == null);
            return View(ex);
        }

        [HttpPost]
        public PartialViewResult ManageInvoicePayments(PurchaseModelViewModel ex, int? page)
        {
            ex.SupplierPaymentPagedList = SupplierPaymentsList(ex, page);
            ex.RelatedSupplierInvoicePayments = new List<SupplierInvoicePayment>();
            foreach (var item in ex.SupplierPaymentPagedList)
            {
                var SupplierInvoicePayments = db.SupplierInvoicePayments.Where(m => m.SupplierPaymentId == item.SupplierPaymentId && m.ModuleId == module_ID);
                foreach (var Sip in SupplierInvoicePayments)
                {
                    ex.RelatedSupplierInvoicePayments.Add(Sip);
                }
            }
            ex.RelatedSupplierInvoicePayments.RemoveAll(m => m == null); ;
            ModelState.Clear();
            return PartialView("_PartialPurchaseInvoicePaymentstList", ex);
        }

        IPagedList<SupplierPayment> SupplierPaymentsList(PurchaseModelViewModel ex, int? page)
        {
            var list = db.SupplierPayments.Where(u => u.BranchId == branch_ID).ToList();
            if (!string.IsNullOrEmpty(ex.FromDate) && !string.IsNullOrEmpty(ex.ToDate))
            {
                var fdate = Convert.ToDateTime(ex.FromDate);
                var tdate = Convert.ToDateTime(ex.ToDate);
                list = db.SupplierPayments.Where(u => u.BranchId == branch_ID && DbFunctions.TruncateTime(u.CreatedOn) >= fdate && DbFunctions.TruncateTime(u.CreatedOn) <= tdate).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(ex.FromDate))
                {
                    var date = Convert.ToDateTime(ex.FromDate);
                    list = db.SupplierPayments.Where(u => u.BranchId == branch_ID && DbFunctions.TruncateTime(u.CreatedOn) >= date).ToList();
                }
                if (!string.IsNullOrEmpty(ex.ToDate))
                {
                    var date = Convert.ToDateTime(ex.ToDate);
                    list = db.SupplierPayments.Where(u => u.BranchId == branch_ID && DbFunctions.TruncateTime(u.CreatedOn) <= date).ToList();
                }
            }
            if (!string.IsNullOrEmpty(ex.Search))
            {
                ex.Search = ex.Search.ToUpper();
                list = list.Where(u => ((u.Client == null ? "" : u.Client.Name.ToString().ToUpper()) == ex.Search.ToUpper()) || (u.SupplierPaymentId.ToString() == ex.Search) ||
                    (u.Description == null ? "" : u.Description.ToUpper()).Contains(ex.Search.ToUpper())).ToList();
            }
            ex.SupplierPaymentPagedList = list.ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, 20);
            return ex.SupplierPaymentPagedList;
        }


        #endregion

        #region PostUnPostPurchaseInvoices

        public ActionResult PostUnPostPurchaseInvoices()
        {
            PurchaseModelViewModel ex = new PurchaseModelViewModel();
            DateTime now = DateTime.Now;
            //ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.FromDate = string.Format("{0:dd/MM/yyyy}", SessionHelper.FiscalStartDate);
            ex.ToDate = now.ToShortDateString();
            ex.PostedToAccount = false;
            ex.UnPosted = true;
            ex.Posted = false;
            ex.All = true;
            ex.v_mnl_PurchaseInvoiceList = PurchaseInvoiceList(ex);
            return View(ex);
        }

        [HttpPost]
        public ActionResult PostUnPostPurchaseInvoices(PurchaseModelViewModel ex, string Command)
        {
            string result = "", InvoiceIds = "-1,";
            VoucherAndReceiptModel vr = new VoucherAndReceiptModel();

            if (ex.v_mnl_PurchaseInvoiceList != null)
            {
                foreach (var item in ex.v_mnl_PurchaseInvoiceList)
                {
                    if (item.IsCheked == true)
                    {
                        InvoiceIds += item.PurchaseInvoiceId.ToString() + ",";
                    }
                }
            }
            InvoiceIds = ProceduresModel.ReplaceLastOccurrence(InvoiceIds, ",", "");
            if (Command != null)
            {
                try
                {
                    using (var trans = db.Database.BeginTransaction())
                    {

                        if (Command == "Post")
                        {
                            ProceduresModel.InsertPurchaseInvoiceProductDetails(db, InvoiceIds);
                            result = AMProceduresModel.p_mnl_PostUnPostAMPurchaseInvoices(db, Command, SessionHelper.UserID, InvoiceIds, DateTime.Now);
                            if (ex.v_mnl_PurchaseInvoiceList != null)
                            {
                                foreach (var item in ex.v_mnl_PurchaseInvoiceList)
                                {
                                    if (item.IsCheked == true)
                                    {
                                        InvoiceIds += item.PurchaseInvoiceId.ToString() + ",";
                                        var message = purchaseBl.PostAMPurchaseInvoice(db, item.PurchaseInvoiceId);
                                        if (message != null)
                                        {
                                            ex.FeedbackMessage = message;
                                        }
                                        #region voucherGeneration Logic 
                                        //var pInvoice = db.AMPurchaseInvoices.Where(u => u.BranchId == branch_ID).Where(u => u.PurchaseInvoiceId == item.PurchaseInvoiceId).FirstOrDefault();

                                        //if (pInvoice != null)
                                        //{
                                        //    if (pInvoice.IsPosted && !pInvoice.IsAccountPosted && !pInvoice.IsCancelled)
                                        //    {
                                        //        vr.Voucher = new Voucher();
                                        //        vr.Voucher.VoucherType = "PI";
                                        //        vr.Voucher.TransactionDate = pInvoice.PurchaseInvoiceDate;
                                        //        vr.Voucher.VoucherStatus = "Posted";
                                        //        vr.Voucher.CurrencyId = pInvoice.CurrencyId;
                                        //        vr.Voucher.ExchangeRate = pInvoice.ExchangeRate;
                                        //        vr.Voucher.CBAccountId = db.Clients.Where(u => u.ClientId == pInvoice.SupplierId).Select(u => u.AccountId).FirstOrDefault();
                                        //        vr.Voucher.Particulars = "Purchase Invoice # " + pInvoice.PurchaseInvoiceId;


                                        //        var SupplierAccount = db.Clients.Where(u => u.ClientId == pInvoice.SupplierId).Select(u => u.AccountId).FirstOrDefault();


                                        //        if (pInvoice.VoucherId == null)
                                        //        {
                                        //            vr.Voucher.VoucherId = ProceduresModel.Voucher_Insert(db, vr.Voucher);
                                        //            pInvoice.VoucherId = vr.Voucher.VoucherId;


                                        //        }
                                        //        else
                                        //        {
                                        //            vr.Voucher.VoucherId = Convert.ToInt64(pInvoice.VoucherId);
                                        //            ProceduresModel.Voucher_Update(db, "Posted", vr.Voucher);
                                        //        }

                                        //        pInvoice.IsAccountPosted = true;
                                        //        pInvoice.AccountPostedOn = DateTime.Now;
                                        //        pInvoice.AccountPostedBy = SessionHelper.UserID;
                                        //        db.Entry(pInvoice).State = EntityState.Modified;
                                        //        db.SaveChanges();

                                        //        if (vr.Voucher.VoucherId > 0)
                                        //        {
                                        //            //Delete VoucherDetails
                                        //            ProceduresModel.p_DeleteVoucherDetailsExceptfirst(db, vr.Voucher.VoucherId);
                                        //            var pDetails = db.AMPurchaseInvoiceProducts.Where(u => u.PurchaseInvoiceId == item.PurchaseInvoiceId).ToList();
                                        //            if (pDetails != null)
                                        //            {
                                        //                foreach (var details in pDetails)
                                        //                {
                                        //                    var vDetails = new VoucherDetail();
                                        //                    vDetails.VoucherId = vr.Voucher.VoucherId;
                                        //                    vDetails.AccountId = details.Item.InventoryAccountId;
                                        //                    vDetails.Debit = Convert.ToDecimal(details.NetTotal);
                                        //                    vDetails.Narration = vr.Voucher.Particulars;

                                        //                    if (details.VoucherDetailId > 0 && db.VoucherDetails.Where(u => u.VoucherDetailId == details.VoucherDetailId).Any())
                                        //                    {
                                        //                        vDetails.VoucherDetailId = Convert.ToInt64(details.VoucherDetailId);
                                        //                        ProceduresModel.VoucherDetail_Update(db, vDetails);
                                        //                    }
                                        //                    else
                                        //                    {
                                        //                        var VoucherDetailId = ProceduresModel.VoucherDetail_Insert(db, vr.Voucher.VoucherId, vr.Voucher.VoucherType, vDetails);
                                        //                        if (VoucherDetailId > 0)
                                        //                        {
                                        //                            details.VoucherDetailId = VoucherDetailId;
                                        //                            db.Entry(details).State = EntityState.Modified;
                                        //                            db.SaveChanges();
                                        //                        }
                                        //                    }
                                        //                }
                                        //                //VoucherDetail Trigger
                                        //                ProceduresModel.t_VoucherDetail(db, vr.Voucher.VoucherId);
                                        //            }

                                        //            #region DiscountEntry
                                        //            if (pInvoice.Discount > 0 && !string.IsNullOrEmpty(SessionHelper.DiscountAccountId_POS))
                                        //            {
                                        //                //string PosPurhcaseDiscoutAcc = "";
                                        //                //var acc = db.AccountSettings.Where(m => m.BranchId == branch_ID).FirstOrDefault();
                                        //                //if (acc != null)
                                        //                //{
                                        //                //    PosPurhcaseDiscoutAcc = acc.PurchaseDiscountAccountId;
                                        //                //}
                                        //                //else {

                                        //                //}

                                        //                //Discount Credit Voucher Entry
                                        //                var vDetails = new VoucherDetail();
                                        //                vDetails.VoucherId = (long)pInvoice.VoucherId;
                                        //                //vDetails.AccountId = clientAccount;
                                        //                //vDetails.AccountId = SessionHelper.DiscountAccountId_POS;
                                        //                vDetails.AccountId = SessionHelper.PurchaseDiscountAccountId_POS;


                                        //                vDetails.Credit = pInvoice.Discount;
                                        //                //vDetails.Narration = _Voucher.Particulars + ", Discount debit entry";
                                        //                vDetails.Narration = vr.Voucher.Particulars + ", Discount credit entry";

                                        //                //var VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, _Voucher.VoucherId, "SI", vDetails);
                                        //                var VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, vr.Voucher.VoucherId, "PI", vDetails);

                                        //                if (VoucherDetailId > 0)
                                        //                {
                                        //                    //pInvoice.DiscountDebitVoucherDetailId = VoucherDetailId;
                                        //                    db.Entry(pInvoice).State = EntityState.Modified;
                                        //                    db.SaveChanges();
                                        //                }

                                        //                //Discount Debit Voucher Entry
                                        //                vDetails = new VoucherDetail();
                                        //                vDetails.VoucherId = vr.Voucher.VoucherId;
                                        //                vDetails.AccountId = SupplierAccount;
                                        //                //vDetails.AccountId = SessionHelper.DiscountAccountId_POS;
                                        //                vDetails.Debit = pInvoice.Discount;
                                        //                //vDetails.Narration = _Voucher.Particulars + ", Discount credit entry";
                                        //                vDetails.Narration = vr.Voucher.Particulars + ", Discount debit entry";



                                        //                VoucherDetailId = ProceduresModel.JV_VoucherDetail_Insert(db, vr.Voucher.VoucherId, "PI", vDetails);
                                        //                if (VoucherDetailId > 0)
                                        //                {
                                        //                    //pInvoice.DiscountCreditVoucherDetailId = VoucherDetailId;
                                        //                    db.Entry(pInvoice).State = EntityState.Modified;
                                        //                    db.SaveChanges();
                                        //                }

                                        //            }
                                        //            #endregion

                                        //        }
                                        //    }
                                        //}

                                        #endregion

                                    }
                                }
                            }
                        }
                        if (Command == "Unpost")
                        {
                            ProceduresModel.DeletePurchaseInvoiceProductDetails(db, InvoiceIds);
                        }

                        if (Command == "Cancel")
                        {
                            ProceduresModel.DeletePurchaseInvoiceProductDetails(db, InvoiceIds);
                        }

                        trans.Commit();
                        ViewBag.Message = result + " Invoice(s) " + Command;
                    }
                }
                catch (DbEntityValidationException exs)
                {
                    if (exs.EntityValidationErrors.Count() > 0)
                    {
                        var er = exs.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault();
                        Error = er.ErrorMessage;
                    }
                }


            }
            ex.All = true;
            ex.v_mnl_PurchaseInvoiceList = PurchaseInvoiceList(ex);
            ModelState.Clear();
            return View(ex);
        }

        #endregion

        #region PostUnPostPurchaseReturns

        List<AMProceduresModel.v_mnl_PurchaseReturns_Result> PurchaseReturnList(PurchaseReturnModelViewModel ex)
        {
            var list = AMProceduresModel.v_mnl_POSPurchaseReturns(db).Where(u => u.BranchId == branch_ID).ToList();
            if (!string.IsNullOrEmpty(ex.FromDate) && !string.IsNullOrEmpty(ex.ToDate))
            {
                var fdate = Convert.ToDateTime(ex.FromDate);
                var tdate = Convert.ToDateTime(ex.ToDate);
                list = AMProceduresModel.v_mnl_POSPurchaseReturns(db).Where(u => u.BranchId == branch_ID && u.PurchaseReturnDate.Date >= fdate && u.PurchaseReturnDate <= tdate).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(ex.FromDate))
                {
                    var date = Convert.ToDateTime(ex.FromDate);
                    list = AMProceduresModel.v_mnl_POSPurchaseReturns(db).Where(u => u.BranchId == branch_ID && u.PurchaseReturnDate >= date).ToList();
                }
                if (!string.IsNullOrEmpty(ex.ToDate))
                {
                    var date = Convert.ToDateTime(ex.ToDate);
                    list = AMProceduresModel.v_mnl_POSPurchaseReturns(db).Where(u => u.BranchId == branch_ID && u.PurchaseReturnDate <= date).ToList();
                }
            }
            if (!string.IsNullOrEmpty(ex.Search))
            {
                ex.Search = ex.Search.ToUpper();
                list = list.Where(u => (u.ClientName == null ? "" : u.ClientName.ToUpper()).Contains(ex.Search.ToUpper()) ||
                    (u.PurchaseReturnId.ToString() == ex.Search) ||
                    (u.Description == null ? "" : u.Description.ToUpper()).Contains(ex.Search.ToUpper())).ToList();
            }
            if (ex.All) { }
            else
            {
                if (ex.WithBalance)
                {
                    list = list.Where(u => (u.NetTotal - u.ReceivedAmount) > 0).ToList();
                }
                else
                {
                    list = list.Where(u => (u.NetTotal - u.ReceivedAmount) == 0).ToList();
                }
            }
            if (ex.Posted)
            {
                list = list.Where(u => u.IsPosted == true).ToList();
            }

            if (ex.UnPosted)
            {
                list = list.Where(u => u.IsPosted == false).ToList();
            }

            ex.v_mnl_PurchaseReturnsList = list;
            return ex.v_mnl_PurchaseReturnsList;
        }

        public ActionResult PostUnPostPurchaseReturns()
        {
            PurchaseReturnModelViewModel ex = new PurchaseReturnModelViewModel();
            DateTime now = DateTime.Now;
            //ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.FromDate = string.Format("{0:dd/MM/yyyy}", SessionHelper.FiscalStartDate);
            ex.ToDate = now.ToShortDateString();
            ex.UnPosted = true;
            ex.Posted = false;
            ex.All = true;
            ex.v_mnl_PurchaseReturnsList = PurchaseReturnList(ex);
            return View(ex);
        }

        [HttpPost]
        public ActionResult PostUnPostPurchaseReturns(PurchaseReturnModelViewModel ex, string Command)
        {
            string result = "", InvoiceIds = "-1,";
            if (ex.v_mnl_PurchaseReturnsList != null)
            {
                foreach (var item in ex.v_mnl_PurchaseReturnsList)
                {
                    if (item.IsCheked == true)
                    {
                        InvoiceIds += item.PurchaseReturnId.ToString() + ",";
                    }
                }
            }
            InvoiceIds = AMProceduresModel.ReplaceLastOccurrence(InvoiceIds, ",", "");
            if (Command != null)
            {
                result = AMProceduresModel.p_mnl_PostUnPostPOSPurchaseReturns(db, Command, SessionHelper.UserID, InvoiceIds, DateTime.Now);
                ViewBag.Message = result + " Invoice(s) " + Command;
            }
            ex.All = true;
            ex.v_mnl_PurchaseReturnsList = PurchaseReturnList(ex);
            ModelState.Clear();
            return View(ex);
        }

        #endregion

        #region PostUnPostPurInvToAccount

        public ActionResult PostUnPostPurInvToAccount()
        {

            PurchaseModelViewModel ex = new PurchaseModelViewModel();
            DateTime now = DateTime.Now;
            //ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.FromDate = string.Format("{0:dd/MM/yyyy}", SessionHelper.FiscalStartDate);
            ex.ToDate = now.ToShortDateString();
            ex.Posted = true;
            ex.UnPostedToAccount = true;
            ex.PostedToAccount = false;
            ex.All = true;
            ex.v_mnl_PurchaseInvoiceList = PurchaseInvoiceList(ex);
            return View(ex);
        }

        [HttpPost]
        public ActionResult PostUnPostPurInvToAccount(PurchaseModelViewModel ex, string Command)
        {
            string result = "", InvoiceIds = "-1,";
            if (Command == "Unpost")
            {
                if (ex.v_mnl_PurchaseInvoiceList != null)
                {
                    foreach (var item in ex.v_mnl_PurchaseInvoiceList)
                    {
                        if (item.IsCheked == true)
                        {
                            InvoiceIds += item.PurchaseInvoiceId.ToString() + ",";
                        }
                    }
                }
                InvoiceIds = ProceduresModel.ReplaceLastOccurrence(InvoiceIds, ",", "");
                result = ProceduresModel.p_mnl_UnPostAMPurchaseInvoicesToAccount(db, SessionHelper.UserID, InvoiceIds);
                ViewBag.Message = result + " Invoice(s) Unposted";
            }
            if (Command == "Post")
            {
                var count = AccountPosting(ex);
                ViewBag.Message = count + " Invoice(s) Posted";
            }
            ex.All = true;
            ex.v_mnl_PurchaseInvoiceList = PurchaseInvoiceList(ex);
            ModelState.Clear();
            return View(ex);
        }


        int AccountPosting(PurchaseModelViewModel ex)
        {
            var count = 0;
            if (ex.v_mnl_PurchaseInvoiceList != null)
            {
                VoucherAndReceiptModel vr = new VoucherAndReceiptModel();

                foreach (var item in ex.v_mnl_PurchaseInvoiceList)
                {
                    if (item.IsCheked == true)
                    {
                        var pInvoice = db.AMPurchaseInvoices.Where(u => u.PurchaseInvoiceId == item.PurchaseInvoiceId).FirstOrDefault();
                        if (pInvoice != null)
                        {
                            if (pInvoice.IsPosted && !pInvoice.IsAccountPosted && !pInvoice.IsCancelled)
                            {
                                vr.Voucher = new Voucher();
                                vr.Voucher.VoucherType = "PI";
                                vr.Voucher.TransactionDate = pInvoice.PurchaseInvoiceDate;
                                vr.Voucher.VoucherStatus = "Posted";
                                vr.Voucher.CurrencyId = pInvoice.CurrencyId;
                                vr.Voucher.ExchangeRate = pInvoice.ExchangeRate;
                                vr.Voucher.CBAccountId = db.Clients.Where(u => u.ClientId == pInvoice.SupplierId).Select(u => u.AccountId).FirstOrDefault();
                                vr.Voucher.Particulars = "Purchase Invoice # " + pInvoice.PurchaseInvoiceId;

                                if (pInvoice.VoucherId == null)
                                {
                                    vr.Voucher.VoucherId = AMProceduresModel.Voucher_Insert(db, vr.Voucher);
                                    pInvoice.VoucherId = vr.Voucher.VoucherId;
                                }
                                else
                                {
                                    vr.Voucher.VoucherId = Convert.ToInt64(pInvoice.VoucherId);
                                    AMProceduresModel.Voucher_Update(db, "Posted", vr.Voucher);
                                }

                                pInvoice.IsAccountPosted = true;
                                pInvoice.AccountPostedOn = DateTime.Now;
                                pInvoice.AccountPostedBy = SessionHelper.UserID;
                                db.Entry(pInvoice).State = EntityState.Modified;
                                db.SaveChanges();

                                count++;
                                if (vr.Voucher.VoucherId > 0)
                                {
                                    var pDetails = db.AMPurchaseInvoiceProducts.Where(u => u.PurchaseInvoiceId == item.PurchaseInvoiceId).ToList();
                                    if (pDetails != null)
                                    {
                                        foreach (var details in pDetails)
                                        {
                                            var vDetails = new VoucherDetail();
                                            vDetails.VoucherId = vr.Voucher.VoucherId;
                                            vDetails.AccountId = details.Item.COGIAccountId;
                                            vDetails.Debit = Convert.ToDecimal(details.NetTotal);
                                            vDetails.Narration = vr.Voucher.Particulars;

                                            if (details.VoucherDetailId > 0)
                                            {
                                                vDetails.VoucherDetailId = Convert.ToInt64(details.VoucherDetailId);
                                                AMProceduresModel.VoucherDetail_Update(db, vDetails);
                                            }
                                            else
                                            {
                                                var VoucherDetailId = AMProceduresModel.VoucherDetail_Insert(db, vr.Voucher.VoucherId, vr.Voucher.VoucherType, vDetails);
                                                if (VoucherDetailId > 0)
                                                {
                                                    details.VoucherDetailId = VoucherDetailId;
                                                    db.Entry(details).State = EntityState.Modified;
                                                    db.SaveChanges();
                                                }
                                            }
                                        }
                                        //VoucherDetail Trigger
                                        AMProceduresModel.t_VoucherDetail(db, vr.Voucher.VoucherId);
                                    }

                                }
                            }
                        }

                    }
                }
            }
            return count;
        }
        #endregion

        [AllowAnonymous]
        [HttpPost]
        public ActionResult IsCBAccountCreated(string SupplierId)
        {
            bool status = false;
            //var CBAccount= db.Clients.Where(u => u.ClientId == SupplierId).Select(u => u.AccountId).FirstOrDefault();
            //if(CBAccount!=null)
            //{
            //    status = true;
            //}
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }
}