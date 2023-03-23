using FAPP.Areas.AM.BLL;
using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.INV.Models;
using FAPP.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
//using static FAPP.ViewModel.Common.ProceduresModel;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
namespace FAPP.Areas.AM.Controllers
{
    public class AssetController : FAPP.Controllers.BaseController
    {
        //OneDbContext db = new OneDbContext();
        short branch_ID = Convert.ToInt16(SessionHelper.BranchId);
        bool reIntialize = false;

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

        #region AddEditOpenningStock

        public ActionResult AddEditOpenningStock(Int64? id, string message)
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
                ex.PurchaseInvoice = new InvPurchaseInvoice();
                ex.PurchaseInvoice.PurchaseInvoiceDate = DateTime.Now;
            }
            return ex;
        }

        [HttpPost]
        public ActionResult AddEditOpenningStock(PurchaseModelViewModel ex, string Command)
        {
            switch (Command)
            {
                case "LoadInvoice":
                    if (ex.InvoiceNo != null)
                    {
                        var prevId = ex.PurchaseInvoice.PurchaseInvoiceId;
                        var isExists = db.InvPurchaseInvoices.Where(u => u.PurchaseInvoiceId == ex.InvoiceNo).FirstOrDefault();
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
            PartialList.Add(new Service.Helper { divToReplace = "UpdatePartial", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialAddEditOpenningStock", vm) });
            return Json(new { status = Convert.ToBoolean(ViewBag.result), HideMsg = true, PartialList }, JsonRequestBehavior.AllowGet);
        }

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
            ViewBag.Products = db.AMItems.Where(u => u.BranchId == branch_ID).ToList();
            ViewBag.Warehouses = db.AMWarehouses.Where(u => u.BranchId == branch_ID).ToList();
            ViewBag.Departments = db.Departments.Where(w => w.BranchId == SessionHelper.BranchId).ToList();
            List<SelectListItem> PaymentMode = new List<SelectListItem>();
            PaymentMode.Add(new SelectListItem() { Text = "Cash", Value = "Cash" });
            PaymentMode.Add(new SelectListItem() { Text = "Bank", Value = "Bank" });
            ViewBag.PaymentMode = PaymentMode;
            ViewBag.CashAccount = ProceduresModel.p_mnl_Account_GetCashAndBankAccounts(db, true, false, branch_ID, null).ToList();
            ViewBag.BankAccount = ProceduresModel.p_mnl_Account_GetCashAndBankAccounts(db, false, true, branch_ID, null).ToList();
        }

        public JsonResult ProductIdByBarcode(string barcode)
        {
            var id = db.AMItems.Where(u => u.Barcode == barcode).Select(u => u.ItemId).FirstOrDefault();
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductDetailById(int? id = 0)
        {
            bool result = false;
            var productdata = db.AMItems.Where(u => u.ItemId == id).Select(u => new { u.Price }).FirstOrDefault();
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

        AMPurchaseModel PostPurchase(AMPurchaseModel ex, string Command)
        {
            db.Database.ExecuteSqlCommand("PRINT '1'");
            ViewBag.result = true;
            ViewBag.success = "Updated successfully";
            using (var trans = db.Database.BeginTransaction())
            {
                db.Database.ExecuteSqlCommand("PRINT '2'");
                try
                {
                    if (ex.PurchaseInvoice != null)
                    {
                        db.Database.ExecuteSqlCommand("PRINT '3a'");
                        switch (Command)
                        {
                            case "InsertUpdate":
                                if (ex.PurchaseInvoice != null)
                                {
                                    if (ex.PurchaseInvoice.PurchaseInvoiceId > 0) // Updation
                                    {
                                        var purchase = db.InvPurchaseInvoices.Where(u => u.PurchaseInvoiceId == ex.PurchaseInvoice.PurchaseInvoiceId).FirstOrDefault();
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
                        }

                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("PRINT '3b'");
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

                    ViewBag.error = str;
                    ViewBag.result = false;
                }
            }
            return ex;
        }

        public Int64 PurchaseInvoice_Insert(InvPurchaseInvoice objPurchaseInvoice)
        {
            var CurrencyId = db.Currencies.Select(u => u.CurrencyId).FirstOrDefault();
            var value = db.CurrencyValues.Where(u => u.CurrencyId == CurrencyId).Select(p => p.Value).FirstOrDefault();

            InvPurchaseInvoice _PurchaseInvoice = new InvPurchaseInvoice();
            var idd = db.InvPurchaseInvoices.Max(u => (Int64?)u.PurchaseInvoiceId);
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
            //_PurchaseInvoice.IsCreatedFromOpenningStock = true;
            db.InvPurchaseInvoices.Add(_PurchaseInvoice);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Messages = ex.GetExceptionMessages();
                return -1;
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
                    _PurchaseInvoice.PurchaseInvoiceDate = objPurchaseInvoice.PurchaseInvoiceDate;
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
                    db.Entry(_PurchaseInvoiceProduct).State = EntityState.Modified;
                    db.SaveChanges();
                    res = true;
                }
            }
            return res;
        }

        bool PostSingleInvoice(long PurchaseInvoiceId)
        {
            var isPosted = db.InvPurchaseInvoices.Where(u => u.PurchaseInvoiceId == PurchaseInvoiceId).Select(u => u.IsPosted).FirstOrDefault();
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
                string count = ProceduresModel.p_mnl_PostUnPostAMAssetOpening(db, "Post", SessionHelper.UserID, PurchaseInvoiceId.ToString(), DateTime.Now);
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

        bool UnpostSingleInvoice(long PurchaseInvoiceId)
        {
            ProceduresModel.DeleteInvoiceProductDetails(db, PurchaseInvoiceId);
            string count = ProceduresModel.p_mnl_PostUnPostAMAssetOpening(db, "Unpost", SessionHelper.UserID, PurchaseInvoiceId.ToString(), DateTime.Now);
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

        #endregion

        #region ManageAssetOpenningStock

        public ActionResult ManageAssetOpenningStock(int? page)
        {
            PurchaseModelViewModel ex = new PurchaseModelViewModel();
            DateTime now = DateTime.Now;
            ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.ToDate = now.ToShortDateString();
            ex.v_mnl_PurchaseInvoicesPagedList = PurchaseInvoicePagedList(ex, page);

            return View(ex);
        }

        [HttpPost]
        public PartialViewResult ManageAssetOpenningStock(PurchaseModelViewModel ex, int? page)
        {
            ex.v_mnl_PurchaseInvoicesPagedList = PurchaseInvoicePagedList(ex, page);

            ModelState.Clear();
            return PartialView("_PartialAssetOpenningStockList", ex);
        }
        [HttpPost]
        public JsonResult ManageAssetOpenningStockPageWise(PurchaseModelViewModel ex, int? page)
        {
            ex.v_mnl_PurchaseInvoicesPagedList = PurchaseInvoicePagedList(ex, page);
            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialAssetOpenningStockList", ex),
                GridId = "divStudents"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        IPagedList<v_mnl_PurchaseInvoices_Result> PurchaseInvoicePagedList(PurchaseModelViewModel ex, int? page)
        {
            var list = v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID).ToList();
            if (!string.IsNullOrEmpty(ex.FromDate) && !string.IsNullOrEmpty(ex.ToDate))
            {
                var fdate = Convert.ToDateTime(ex.FromDate);
                var tdate = Convert.ToDateTime(ex.ToDate);
                list = v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID && u.PurchaseInvoiceDate.Date >= fdate && u.PurchaseInvoiceDate <= tdate).ToList();
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
                    list = v_mnl_AMPurchaseInvoices(db).Where(u => u.BranchId == branch_ID && u.PurchaseInvoiceDate <= date).ToList();
                }
            }
            if (!string.IsNullOrEmpty(ex.Search))
            {
                ex.Search = ex.Search.ToUpper();
                list = list.Where(u => (u.ClientName == null ? "" : u.ClientName.ToUpper()).Contains(ex.Search.ToUpper()) ||
                    (u.PurchaseInvoiceId.ToString() == ex.Search) ||
                    (u.Description == null ? "" : u.Description.ToUpper()).Contains(ex.Search.ToUpper())).ToList();
            }
            list = list.Where(u => u.IsCreatedFromOpenningStock).ToList();
            ex.v_mnl_PurchaseInvoicesPagedList = list.ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, 50);
            return ex.v_mnl_PurchaseInvoicesPagedList;
        }
        public static List<v_mnl_PurchaseInvoices_Result> v_mnl_AMPurchaseInvoices(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_PurchaseInvoices_Result>($@"
            SELECT        
            pu.PurchaseInvoiceId, pu.PurchaseOrderId, pu.PurchaseInvoiceDate, pu.SupplierId, pu.Discount,
            pu.Description, pu.IsPosted, pu.PostedOn, pu.PostedBy, pu.IsCancelled, pu.CancelledOn, pu.CancelledBy,
            pu.CreatedOn,pu.CreatedBy, pu.ModifiedOn, pu.ModifiedBy, pu.CurrencyId, pu.ExchangeRate, pu.MovedToWarehouse,
            pu.VoucherId, pu.TotalAmount, pu.LabourCharges, pu.OtherCharges, pu.FareCharges, ISNULL(pu.NetTotal, 0) AS NetTotal,
            pu.BranchId, pu.Version, pu.IsApplyTax, ISNULL(pu.Received, 0) AS ReceivedAmount,
            Membership.Users.Username AS PostedName,User_2.Username AS CancelledName, User_1.Username AS ModifiedName,
            User_3.Username AS CreatedName, cl.Name AS ClientName, CONVERT(decimal(18, 2), 0) AS Paid, pu.IsAccountPosted, 
            pu.AccountPostedOn, pu.AccountPostedBy, User_5.Username AS AccountPostedName,pu.IsCreatedFromOpenningStock,
            (select 
            top 1 WareHouseId 
            from am.PurchaseInvoiceProducts as det 
            where det.PurchaseInvoiceId = pu.PurchaseInvoiceId) as WareHouseId
            FROM AM.PurchaseInvoices AS pu 
            LEFT OUTER JOIN Membership.Users AS User_5 ON pu.AccountPostedBy = User_5.UserID 
            LEFT OUTER JOIN Membership.Users AS User_3 ON pu.CreatedBy = User_3.UserID
            LEFT OUTER JOIN Membership.Users AS User_2 ON pu.CancelledBy = User_2.UserID 
            LEFT OUTER JOIN Membership.Users AS User_1 ON pu.ModifiedBy = User_1.UserID 
            LEFT OUTER JOIN Membership.Users ON pu.PostedBy = Membership.Users.UserID 
            LEFT OUTER JOIN Client.Clients AS cl ON pu.SupplierId = cl.ClientId
            where pu.IsCreatedFromOpenningStock=1").ToList();
            return result;
        }

        public JsonResult DeletePurchaseInvoice(long id, long PurchaseInvoiceId)
        {
            if (id > 0 && PurchaseInvoiceId > 0)
            {
                bool isposted = false;
                isposted = db.InvPurchaseInvoices.Where(w => w.PurchaseInvoiceId == PurchaseInvoiceId).Select(s => s.IsPosted).FirstOrDefault();
                if (isposted)
                {
                    Error = "Please unpost the purchase invoice and then try again!";
                    Messages = "";
                }
                else
                {
                    int removerow = db.Database.ExecuteSqlCommand($@"delete from am.PurchaseInvoiceProducts where PurchaseInvoiceProductId='{id}'");
                    Messages = "sussessfully Deleted";
                    Error = "";
                }
            }

            var vm = new PurchaseModelViewModel();
            vm = GetPurchaseInvoice(vm, PurchaseInvoiceId);
            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialAddEditOpenningStock", vm),
                Messages,
                Error
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}