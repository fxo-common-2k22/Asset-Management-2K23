using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FAPP.Model;
using FAPP.DAL;
using PagedList;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
using FAPP.Areas.AM.BLL;
using FAPP.Areas.AM.ViewModels;

namespace FAPP.Areas.AM.Controllers
{
    public class SetupController : FAPP.Controllers.BaseController
    {
        //private OneDbContext db = new OneDbContext();
        private static int pageNo = 1;
        private int branch_ID = SessionHelper.BranchId;
        private int fiscalYearId = SessionHelper.FiscalYearId;
        private int CreatedBy = SessionHelper.UserID;
        string InventoryParentAccountId, DepreciationParentAccountId, COGIParentAccountId;
        private ManageAMCategoryViewModel CategoriesVM;
        // GET: AM/Setup
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManageItems()
        {
            ManageAMItemsViewModel ex = new ManageAMItemsViewModel();
            ex.Item = new AMItem();
            ex.Item.IsConsumable = false;
            ex.Items = db.AMItems.Where(u => u.BranchId == branch_ID);
            FillDD_Item(null, null, null);
            return View(ex);
        }
        private IEnumerable<SelectListItem> GetDepriciationType()
        {
            if (!db.DepriciationTypes.Any())
            {
                var depreciation = new DepriciationType
                {
                    DepriciationTypeName = "Straight-Line Depreciation Method"
                };
                db.DepriciationTypes.Add(depreciation);
                db.SaveChanges();
            }

            return from depType in db.DepriciationTypes
                   select new SelectListItem
                   {
                       Value = depType.DepriciationTypeId.ToString(),
                       Text = depType.DepriciationTypeName
                   };
        }
        [HttpPost]
        public ActionResult ManageItems(ManageAMItemsViewModel vm)
        {
            IEnumerable<AMItem> items = db.AMItems;
            if (vm.CategoryId != null)
                items = items.Where(s => s.CategoryId == vm.CategoryId && s.BranchId == branch_ID);
            if (vm.SearchConsumableRadioBtn == true && vm.SearchFixedRadioBtn == true)
            {

            }
            else
            {
                if (vm.SearchFixedRadioBtn)
                    items = items.Where(s => s.IsConsumable == false);
                if (vm.SearchConsumableRadioBtn)
                    items = items.Where(s => s.IsConsumable == true);
            }
            if (vm.SearchItem != null)
                items = items.Where(s => s.ItemName.ToLower().Contains(vm.SearchItem.ToLower()) || s.ItemName.StartsWith(vm.SearchItem));

            //ViewBag.Categoires = new SelectList(db.AMCategories, "CategoryId", "CategoryName");
            //ViewBag.Units = new SelectList(db.AMUnits, "UnitId", "UnitName");
            //ViewBag.DepriciationTypes = new SelectList(GetDepriciationType(), "Value", "Text");
            //return PartialView("_PartialManageItems", new ManageAMItemsViewModel() { Items = items });
            vm.Items = items.Where(u => u.BranchId == branch_ID);
            List<Service.Helper> PartialList = new List<Service.Helper>();
            PartialList.Add(new Service.Helper { divToReplace = "UpdateTarget", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialManageItems", vm) });
            return Json(new { status = true, HideMsg = true, PartialList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteProduct(long? id)
        {
            bool status = false;
            string msg = "";
            using (var trans = db.Database.BeginTransaction())
            {
                var docSpec = db.AMItems.Where(u => u.ItemId == id).FirstOrDefault();
                if (docSpec != null)
                {
                    //if (docSpec.BYDEFAULT)
                    //    msg = "Default Account can't delete";
                    //if (string.IsNullOrEmpty(msg))
                    //{
                    //    var _ob = db.OpeningBalances.Where(u => u.AccountId == docSpec.autokey).FirstOrDefault();
                    //    if (_ob != null)
                    //    {
                    //        if (_ob.OBDebitAmount > 0 || _ob.OBCreditAmount > 0)
                    //            msg = "Account can't delete because it have opeining balance";
                    //        else
                    //            db.OpeningBalances.Remove(_ob);
                    //    }

                    //    var _yb = db.YearlyBalances.Where(u => u.AccountId == docSpec.autokey).FirstOrDefault();
                    //    if (_yb != null)
                    //    {
                    //        if (_yb.OBDebitAmount > 0 || _yb.OBCreditAmount > 0 || _yb.TransactionDebitAmount > 0 || _yb.TransactionCreditAmount > 0)
                    //            msg = "Account can't delete because it have opeining balance";
                    //        else
                    //            db.YearlyBalances.Remove(_yb);
                    //    }
                    //}

                    try
                    {
                        if (string.IsNullOrEmpty(msg))
                        {
                            db.AMItems.Remove(docSpec);
                            db.SaveChanges();
                            trans.Commit();
                            status = true;
                            msg = "Deleted successfully";
                        }
                    }
                    catch (Exception exc)
                    {
                        msg = ExtensionMethods.GetExceptionMessages(exc);
                        trans.Rollback();
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

        [HttpPost]
        public ActionResult AddItem(ManageAMItemsViewModel ex)
        {
            bool status = false;
            string msg = "";

            if (!string.IsNullOrEmpty(ex.Item.ItemName))
            {
                string C_InvAcc = "", C_COGIAcc = "", C_DepAcc = "", InvAcc = "", COGIAcc = "", DepAcc = "";
                if (ex.Item.CategoryId > 0)
                {
                    var Category = db.AMCategories.Find(ex.Item.CategoryId);
                    if (Category == null)
                        msg = "Category not found";
                    else
                    {
                        C_COGIAcc = Category.C_COGIAccountId;
                        C_DepAcc = Category.C_DepreciationAccountId;
                        C_InvAcc = Category.C_InventoryAccountId;
                        COGIAcc = Category.T_COGIAccountId;
                        DepAcc = Category.T_DepreciationAccountId;
                        InvAcc = Category.T_InventoryAccountId;
                    }
                }
                else
                    msg = "Category not found";

                if (!string.IsNullOrEmpty(C_InvAcc))
                {
                    if (string.IsNullOrEmpty(InvAcc))
                        msg = "Category's Inventory transaction account not found";
                    if (!string.IsNullOrEmpty(C_COGIAcc))
                    {
                        if (string.IsNullOrEmpty(COGIAcc))
                            msg = "Category's COGI transaction account not found";
                        if (!string.IsNullOrEmpty(C_DepAcc))
                        {
                            if (string.IsNullOrEmpty(DepAcc))
                                msg = "Category's Depreciation transaction account not found";
                        }
                        else
                            msg = "Category's Depreciation control account not found";
                    }
                    else
                        msg = "Category's COGI control account not found";
                }
                else
                    msg = "Category's Inventory control account not found";



                if (string.IsNullOrEmpty(msg))
                {
                    AMItem _Product = ex.Item;
                    if (string.IsNullOrEmpty(ex.Item.ProductCode))
                        _Product.ProductCode = GenerateProductcode(Convert.ToInt32(ex.Item.CategoryId), _Product.ItemId);
                    if (!string.IsNullOrEmpty(ex.Item.Barcode))
                    {
                        if (IsBarcodeExist(ex.Item.Barcode))
                            _Product.Barcode = ex.Item.Barcode;
                        else
                            msg = "'" + ex.Item.Barcode + "' Barcode already exist! Try another ";
                    }
                    if (string.IsNullOrEmpty(msg))
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                if (SessionHelper.IsIndividualItemAccounts_AM)
                                {
                                    COGIAcc = ProceduresModel.CreateNewAccountByParentAccountCode(db, ex.Item.ItemName + "_COGI", C_COGIAcc, false);

                                    DepAcc = ProceduresModel.CreateNewAccountByParentAccountCode(db, ex.Item.ItemName + "_DEP", C_DepAcc, false);

                                    InvAcc = ProceduresModel.CreateNewAccountByParentAccountCode(db, ex.Item.ItemName + "_INV", C_InvAcc, false);
                                }
                                //_Product.ShortName = StringFormate.GenerateItemShortName(ex.Item.ItemName, ex.Item.CategoryId);
                                _Product.ShortName = ex.Item.ShortName;
                                _Product.Price = ex.Item.Price;
                                _Product.NatureId = ex.Item.NatureId;
                                _Product.Description = ex.Item.Description;
                                _Product.UnitId = ex.Item.UnitId;
                                _Product.DepriciationTypeId = ex.Item.DepriciationTypeId;
                                _Product.CategoryId = ex.Item.CategoryId;
                                _Product.CreatedOn = DateTime.Now;
                                _Product.CreatedBy = SessionHelper.UserID;
                                _Product.BranchId = branch_ID;
                                _Product.InventoryAccountId = InvAcc;
                                _Product.COGIAccountId = COGIAcc;
                                _Product.DepreciationAccountId = DepAcc;
                                _Product.IsDepAccountCreated = SessionHelper.IsIndividualItemAccounts_AM;
                                _Product.IsInvAccountCreated = SessionHelper.IsIndividualItemAccounts_AM;
                                _Product.IsCOGIAccountCreated = SessionHelper.IsIndividualItemAccounts_AM;

                                db.AMItems.Add(_Product);
                                db.SaveChanges();
                                trans.Commit();
                                msg = "Product(s) added successfully";
                                status = true;
                            }
                            catch
                            {
                                //trans.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            else
                msg = "Product Name can't be empty";

            List<Service.Helper> PartialList = new List<Service.Helper>();
            ex.Items = db.AMItems.Where(u => u.BranchId == branch_ID);
            PartialList.Add(new Service.Helper { IdToHideModalPopup = "AddItemModel", divToReplace = "UpdateTarget", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialManageItems", ex) });
            return Json(new { status = status, msg = msg, HideMsg = false, PartialList }, JsonRequestBehavior.AllowGet);
        }

        string GenerateProductcode(int CategoryId, int? ProductId)
        {
            string productcode = "";
            productcode = ProceduresModel.GetNewProductCode_AM(db, CategoryId).ToString();
            while (!IsProductCodeExist(productcode))
            {
                var code = Convert.ToInt64(productcode);
                code++;
                productcode = code.ToString();
            }
            return productcode;
        }

        bool IsBarcodeExist(string barcode)
        {
            var res = db.AMItems.Where(u => u.Barcode == barcode).FirstOrDefault();
            if (res == null)
                return true;
            return false;
        }

        bool IsProductCodeExist(string productcode)
        {
            var res = db.AMItems.Where(u => u.ProductCode == productcode).FirstOrDefault();
            if (res == null)
                return true;
            return false;
        }

        void FillDD_Item(int? UnitId, int? CategoryId, int? DepriciationTypeId)
        {
            ViewBag.Units = new SelectList(db.AMUnits, "UnitId", "UnitName", UnitId == 0 ? null : UnitId);
            ViewBag.Categoires = new SelectList(db.AMCategories, "CategoryId", "CategoryName", CategoryId == 0 ? null : CategoryId);
            ViewBag.DepriciationTypes = new SelectList(GetDepriciationType(), "Value", "Text", DepriciationTypeId == 0 ? null : DepriciationTypeId);
        }

        [HttpPost]
        public ActionResult EditItem(ManageAMItemsViewModel vm)
        {
            vm.Item = db.AMItems.FirstOrDefault(s => s.ItemId == vm.ItemId);
            if (vm.Item == null)
                vm.Item = new AMItem();
            //var item = db.AMItems.FirstOrDefault(s => s.ItemId == vm.ItemId);
            //vm.ItemId = item.ItemId;
            //vm.Name = item.ItemName;
            //vm.ShortName = item.ShortName;
            //vm.Price = item.Price;
            //vm.NatureId = item.NatureId;
            //vm.Description = item.Description;
            //vm.DepriciationId = item.DepriciationTypeId != null ? (int)item.DepriciationTypeId : 0;
            //vm.IsConsumable = item.IsConsumable == true ? "Yes" : "No";
            //vm.UnitId = item.UnitId;
            //ViewBag.Units = db.ItemUnits;
            //vm.Units = new SelectList(db.AMUnits, "UnitId", "UnitName", vm.Item.UnitId);
            //vm.Categoires = new SelectList(db.AMCategories, "CategoryId", "CategoryName", vm.Item.CategoryId);
            //ViewBag.DepriciationTypes = new SelectList(GetDepriciationType(), "Value", "Text", vm.Item.DepriciationTypeId);

            //List<ValueAndText> accoutList = new List<ValueAndText>();
            //if (vm.Item.COGIAccount != null)
            //    accoutList.Add(new ValueAndText() { Value = vm.Item.COGIAccount.ACCOUNT_ID.ToString(), Text = vm.Item.COGIAccount.TITLE });
            //if (vm.Item.InventoryAccount != null)
            //    accoutList.Add(new ValueAndText() { Value = vm.Item.InventoryAccount.ACCOUNT_ID.ToString(), Text = vm.Item.InventoryAccount.TITLE });
            //vm.Accounts = accoutList;
            //return PartialView("_ParitalEditItem", vm);

            List<Service.Helper> PartialList = new List<Service.Helper>();
            FillDD_Item(vm.Item.UnitId, vm.Item.CategoryId, vm.Item.DepriciationTypeId);
            //vm.Categories = db.AMCategories.Where(u => u.BranchId == branch_ID);
            PartialList.Add(new Service.Helper { IdToShowModalPopup = "EditModal", divToReplace = "EditModalTarget", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_ParitalEditItem", vm) });
            return Json(new { status = true, HideMsg = true, PartialList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveEditedItem(ManageAMItemsViewModel ex)
        {
            //var vm = db.AMItems.FirstOrDefault(s => s.ItemId == item.ItemId);
            //vm.ItemName = item.Name;
            //vm.ShortName = item.ShortName.ToUpper();
            //vm.Price = item.Price;
            //vm.NatureId = item.NatureId;
            ////vm.ShortName = StringFormate.GenerateItemShortName(item.Name, (int)item.CategoryId);
            //if (item.IsConsumable.Contains("true"))
            //{
            //    vm.IsConsumable = true;
            //}
            //else
            //{
            //    vm.IsConsumable = false;
            //}
            //vm.Description = item.Description;
            //vm.CategoryId = (item.CategoryId > 0 ? (int)item.CategoryId : 0);
            //vm.UnitId = (item.UnitId > 0 ? item.UnitId : 0);
            //db.SaveChanges();
            //item.Items = db.AMItems;
            //return PartialView("_PartialManageItems", item);
            bool status = false;
            string msg = "";
            if (ex.Item.ItemId > 0)
            {
                AMItem _Product = db.AMItems.Where(u => u.ItemId == ex.Item.ItemId).FirstOrDefault();
                if (_Product != null)
                {
                    if (string.IsNullOrEmpty(_Product.ProductCode))
                        _Product.ProductCode = GenerateProductcode(Convert.ToInt32(ex.Item.CategoryId), _Product.ItemId);
                    _Product.ItemName = ex.Item.ItemName;
                    _Product.ShortName = ex.Item.ShortName;
                    _Product.Price = ex.Item.Price;
                    _Product.NatureId = ex.Item.NatureId;
                    _Product.Description = ex.Item.Description;
                    _Product.UnitId = ex.Item.UnitId;
                    _Product.DepriciationTypeId = ex.Item.DepriciationTypeId;
                    _Product.CategoryId = ex.Item.CategoryId;
                    _Product.IsConsumable = ex.Item.IsConsumable;
                    _Product.ModifiedOn = DateTime.Now;
                    _Product.ModifiedBy = SessionHelper.UserID;
                    if (!string.IsNullOrEmpty(ex.Item.Barcode) && _Product.Barcode != ex.Item.Barcode)
                    {
                        if (IsBarcodeExist(ex.Item.Barcode))
                            _Product.Barcode = ex.Item.Barcode;
                        else
                            msg = "'" + ex.Item.Barcode + "' Barcode already exist! Try another ";
                    }

                    if (string.IsNullOrEmpty(msg))
                    {
                        db.Entry(_Product).State = EntityState.Modified;
                        db.SaveChanges();
                        msg = "Product(s) updated successfully";
                        status = true;
                    }
                }
            }

            List<Service.Helper> PartialList = new List<Service.Helper>();
            ex.Items = db.AMItems.Where(u => u.BranchId == branch_ID);
            PartialList.Add(new Service.Helper { IdToHideModalPopup = "EditModal", divToReplace = "UpdateTarget", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialManageItems", ex) });
            return Json(new { status = status, msg = msg, HideMsg = true, PartialList }, JsonRequestBehavior.AllowGet);
        }

        void FilDD_AccSetting()
        {
            ViewBag.AccountsDD = ProceduresModel.p_mnl_Account__Search(db, null, null, null, null, branch_ID, null, true, null).ToList();
            ViewBag.SupplierDD = db.Clients.Where(w => w.BranchId == branch_ID && w.IsSupplier == true).Select(s => new
            {
                s.ClientId,
                s.PresentableName
            }).ToList();
        }
        [HttpGet]
        public async Task<ActionResult> AccountSettings()
        {
            var vm = new AMAccountSettingsViewModel();
            vm.Nature = await db.AMNatures.Where(u => u.BranchId == branch_ID).FirstOrDefaultAsync();
            if (vm.Nature == null)
                vm.Nature = new AMNature();
            FilDD_AccSetting();
            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> AccountSettings(AMAccountSettingsViewModel vm)
        {
            var itemNature = await db.AMNatures.Where(u => u.BranchId == branch_ID).FirstOrDefaultAsync();
            if (itemNature != null)
            {
                itemNature.COGIParentAccountId = vm.Nature.COGIParentAccountId;
                itemNature.InventoryParentAccountId = vm.Nature.InventoryParentAccountId;
                itemNature.DepreciationParentAccountId = vm.Nature.DepreciationParentAccountId;
                itemNature.CreateItemAccounts = vm.Nature.CreateItemAccounts;
                itemNature.DefaultSupplierId = vm.Nature.DefaultSupplierId;
                itemNature.BranchId = (short)branch_ID;
                await db.SaveChangesAsync();
            }
            else
            {
                itemNature = vm.Nature;
                itemNature.BranchId = (short)branch_ID;
                db.AMNatures.Add(itemNature);
                await db.SaveChangesAsync();
            }
            vm.Nature = await db.AMNatures.Where(u => u.BranchId == branch_ID).FirstOrDefaultAsync();
            FilDD_AccSetting();
            return PartialView("_PartialAccountSettings", vm);
        }

        [HttpGet]
        public ActionResult ManageCategory()
        {

            return View(new ManageAMCategoryViewModel()
            {
                Categories = db.AMCategories.Where(x => x.BranchId == branch_ID),
                ParentCategories = new SelectList(db.AMCategories, "CategoryId", "CategoryName"),
                Natures = new SelectList(db.AMNatures, "NatureId", "NatureName"),
            });

        }
        string CategoryConstraint()
        {
            string msg = "";
            var _acc = db.AMNatures.Where(u => u.BranchId == branch_ID).FirstOrDefault();
            if (_acc == null)
            {
                msg = "Account setting not found";
            }
            else
            {
                InventoryParentAccountId = _acc.InventoryParentAccountId;
                DepreciationParentAccountId = _acc.DepreciationParentAccountId;
                COGIParentAccountId = _acc.COGIParentAccountId;
                if (string.IsNullOrEmpty(_acc.InventoryParentAccountId))
                {
                    msg = "Inventroy parent account not found<br>";
                }
                if (string.IsNullOrEmpty(_acc.COGIParentAccountId))
                {
                    msg += "COGI parent account not found<br>";
                }
                if (string.IsNullOrEmpty(_acc.DepreciationParentAccountId))
                {
                    msg += "Depreciation parent account not found<br>";
                }
            }
            return msg;
        }

        [HttpPost]
        public ActionResult ManageCategory(ManageAMCategoryViewModel vm)
        {
            //if (vm.Search != null)
            //    vm.Categories = db.AMCategories.Where(u => u.BranchId == branch_ID).Where(s => s.CategoryName.Contains(vm.Search));
            //else
            //    vm.Categories = db.AMCategories.Where(u => u.BranchId == branch_ID);
            //return PartialView("_PartialManageAMCategory", vm);

            List<Service.Helper> PartialList = new List<Service.Helper>();
            if (vm.Search != null)
                vm.Categories = db.AMCategories.Where(u => u.BranchId == branch_ID).Where(s => s.CategoryName.Contains(vm.Search));
            else
                vm.Categories = db.AMCategories.Where(u => u.BranchId == branch_ID);

            PartialList.Add(new Service.Helper { divToReplace = "UpdateTarget", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialManageAMCategory", vm) });
            return Json(new { status = true, HideMsg = true, PartialList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddCategory(ManageAMCategoryViewModel ex)
        {
            bool status = false;
            string msg = "Insertion failed";
            AMCategory _Category = ex.Category;
            try
            {
                var checkexist = db.AMCategories.Where(u => u.BranchId == branch_ID).Where(u => u.CategoryName.ToUpper() == _Category.CategoryName.ToUpper()).FirstOrDefault();
                if (checkexist != null)
                    msg = "Category Name must be unique";
                else
                {
                    msg = CategoryConstraint();
                    if (string.IsNullOrEmpty(msg))
                    {
                        using (var trans = db.Database.BeginTransaction())
                        {
                            try
                            {
                                _Category.CategoryCode = ProceduresModel.p_mnl_GetNextCategoryCode_AM(db);
                                _Category.CreatedOn = DateTime.Now;
                                _Category.CreatedBy = SessionHelper.UserID;
                                _Category.BranchId = branch_ID;
                                string[] AccIds;
                                AccIds = ProceduresModel.CreateCategoryNewAccounts(db, _Category.CategoryName + "_COGI", COGIParentAccountId);//Create COGI Accounts
                                if (AccIds != null)
                                {
                                    _Category.C_COGIAccountId = AccIds[0];
                                    _Category.T_COGIAccountId = AccIds[1];
                                }
                                AccIds = ProceduresModel.CreateCategoryNewAccounts(db, _Category.CategoryName + "_INV", InventoryParentAccountId);//Create Inventory Accounts
                                if (AccIds != null)
                                {
                                    _Category.C_InventoryAccountId = AccIds[0];
                                    _Category.T_InventoryAccountId = AccIds[1];

                                }
                                AccIds = ProceduresModel.CreateCategoryNewAccounts(db, _Category.CategoryName + "_DEP", DepreciationParentAccountId);//Create IncomeAccount Accounts
                                if (AccIds != null)
                                {
                                    _Category.C_DepreciationAccountId = AccIds[0];
                                    _Category.T_DepreciationAccountId = AccIds[1];
                                }

                                db.AMCategories.Add(_Category);
                                db.SaveChanges();
                                trans.Commit();
                                msg = "Category added successfully";
                                status = true;
                            }
                            catch
                            {
                                //trans.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            //return PartialView("_PartialManageAMCategory", ex);

            List<Service.Helper> PartialList = new List<Service.Helper>();
            ex.Categories = db.AMCategories.Where(u => u.BranchId == branch_ID);
            PartialList.Add(new Service.Helper { IdToHideModalPopup = "AddCategoryModal", divToReplace = "UpdateTarget", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialManageAMCategory", ex) });
            return Json(new { status = status, msg = msg, HideMsg = false, PartialList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditCategory(ManageAMCategoryViewModel vm)
        {
            //vm.Category = db.AMCategories.FirstOrDefault(s => s.CategoryId == vm.CategoryId);
            //ViewBag.AccountDDL = ProceduresModel.p_mnl_Account__Search(db, null, null, null, null, branch_ID, null, true, null).ToList();
            //return PartialView("_ParitalEditAMCategory", vm);

            List<Service.Helper> PartialList = new List<Service.Helper>();
            vm.Category = db.AMCategories.FirstOrDefault(s => s.CategoryId == vm.CategoryId);
            PartialList.Add(new Service.Helper { IdToShowModalPopup = "EditModal", divToReplace = "EditModalTarget", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_ParitalEditAMCategory", vm) });
            return Json(new { status = true, HideMsg = true, PartialList }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult SaveEditedCategory(ManageAMCategoryViewModel vm)
        {
            bool result = false;
            string msg = "";
            if (vm.Category.CategoryId == 0)
                msg = "Category Id not found";
            if (string.IsNullOrEmpty(msg))
                msg = CategoryConstraint();
            if (string.IsNullOrEmpty(msg))
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        AMCategory _Category = db.AMCategories.Where(u => u.BranchId == branch_ID).Where(u => u.CategoryId == vm.Category.CategoryId).FirstOrDefault();
                        if (_Category != null)
                        {
                            if (!_Category.CategoryCode.HasValue)
                                _Category.CategoryCode = ProceduresModel.p_mnl_GetNextCategoryCode_AM(db);
                            _Category.CategoryName = vm.Category.CategoryName;
                            _Category.ShortName = vm.Category.ShortName;
                            _Category.ModifiedOn = DateTime.Now;
                            _Category.ModifiedBy = SessionHelper.UserID;
                            string[] AccIds;
                            if (string.IsNullOrEmpty(_Category.C_COGIAccountId))
                            {
                                AccIds = ProceduresModel.CreateCategoryNewAccounts(db, _Category.CategoryName + "_COGI", COGIParentAccountId);//Create COGI Accounts
                                if (AccIds != null)
                                {
                                    _Category.C_COGIAccountId = AccIds[0];
                                    _Category.T_COGIAccountId = AccIds[1];
                                }
                            }
                            if (string.IsNullOrEmpty(_Category.C_InventoryAccountId))
                            {
                                AccIds = ProceduresModel.CreateCategoryNewAccounts(db, _Category.CategoryName + "_INV", InventoryParentAccountId);//Create Inventory Accounts
                                if (AccIds != null)
                                {
                                    _Category.C_InventoryAccountId = AccIds[0];
                                    _Category.T_InventoryAccountId = AccIds[1];

                                }
                            }
                            if (string.IsNullOrEmpty(_Category.C_DepreciationAccountId))
                            {
                                AccIds = ProceduresModel.CreateCategoryNewAccounts(db, _Category.CategoryName + "_DEP", DepreciationParentAccountId);//Create Depreciation Accounts
                                if (AccIds != null)
                                {
                                    _Category.C_DepreciationAccountId = AccIds[0];
                                    _Category.T_DepreciationAccountId = AccIds[1];
                                }
                            }
                        }

                        db.Entry(_Category).State = EntityState.Modified;
                        db.SaveChanges();
                        trans.Commit();
                        result = true;
                        msg = "Updated successfully";
                    }
                    catch (Exception)
                    {
                        //trans.Rollback();
                        throw;
                    }
                }
            }
            List<Service.Helper> PartialList = new List<Service.Helper>();
            vm.Categories = db.AMCategories.Where(u => u.BranchId == branch_ID);
            PartialList.Add(new Service.Helper { IdToHideModalPopup = "EditModal", divToReplace = "UpdateTarget", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialManageAMCategory", vm) });
            return Json(new { status = result, msg = msg, PartialList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public bool SaveEditedCategoryAccounts(ManageAMCategoryViewModel vm)
        {
            var category = db.AMCategories.Find(vm.CategoryId);
            //fetch previous accounts
            if (!string.IsNullOrEmpty(category.C_COGIAccountId))
            {
                if (!db.Accounts.Select(s => s.ParentAccountId).Contains(category.C_COGIAccountId))
                {
                    var account = db.Accounts.Find(category.C_COGIAccountId);
                    db.Accounts.Remove(account);
                    db.SaveChanges();
                }
                else return false;
            }
            if (!string.IsNullOrEmpty(category.C_InventoryAccountId))
            {
                if (!db.Accounts.Select(s => s.ParentAccountId).Contains(category.C_InventoryAccountId))
                {
                    var account = db.Accounts.Find(category.C_InventoryAccountId);
                    db.Accounts.Remove(account);
                    db.SaveChanges();
                }
                else return false;
            }

            if (vm.NatureId != null)
            {
                AMNature itemNature = db.AMNatures.Find(vm.NatureId);
                if (itemNature != null)
                {
                    var fixedAccount = db.Accounts.Find(itemNature.InventoryParentAccountId);
                    var consumableAccount = db.Accounts.Find(itemNature.COGIParentAccountId);

                    if (fixedAccount != null)
                        category.C_InventoryAccountId = CreateNewAccountByParentAccountCode(category.CategoryName, fixedAccount.TITLE, true, true);
                    if (consumableAccount != null)
                        category.C_COGIAccountId = CreateNewAccountByParentAccountCode(category.CategoryName, consumableAccount.TITLE, true, true);
                    //category.NatureId = vm.NatureId;
                }
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }
        [HttpGet]
        public ActionResult ManageUnits()
        {
            return View(new ManageAMUnitsViewModel() { Units = db.AMUnits });
        }

        [HttpPost]
        public ActionResult ManageUnits(ManageAMUnitsViewModel vm)
        {
            if (vm.Search != null)
            {
                vm.Units = db.AMUnits.Where(s => s.UnitName.Contains(vm.Search));
            }
            else
            {
                vm.Units = db.AMUnits;
            }
            return PartialView("_PartialManageAMUnits", vm);
        }

        [HttpPost]
        public ActionResult AddUnit(ManageAMUnitsViewModel vm)
        {
            var entity = new AMUnit();
            entity.UnitName = vm.UnitName;
            db.AMUnits.Add(entity);
            db.SaveChanges();

            vm.Units = db.AMUnits;
            return PartialView("_PartialManageAMUnits", vm);
        }

        [HttpPost]
        public ActionResult EditUnit(ManageAMUnitsViewModel vm)
        {
            var unit = db.AMUnits.FirstOrDefault(s => s.UnitId == vm.UnitId);
            vm.UnitName = unit.UnitName;
            return PartialView("_ParitalEditAMUnit", vm);
        }

        [HttpPost]
        public ActionResult SaveEditedUnit(ManageAMUnitsViewModel vm)
        {
            var unit = db.AMUnits.FirstOrDefault(s => s.UnitId == vm.UnitId);
            unit.UnitName = vm.UnitName;
            db.SaveChanges();
            vm.Units = db.AMUnits;
            return PartialView("_PartialManageAMUnits", vm);
        }

        [HttpPost]
        public bool DeleteUnit(int id)
        {
            try
            {
                db.AMUnits.Remove(db.AMUnits.Find(id));
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        #region OpeningStock

        public ActionResult OpeningStock(int? page)
        {

            SetupModelViewModel ex = new SetupModelViewModel();
            ViewBag.WareHouses = db.AMWarehouses.Where(u => u.BranchId == branch_ID).ToList();
            var list = new List<AMProceduresModel.p_mnl__OpeningStock_Result>();
            ex.p_mnl__OpeningStockPagedList = list.ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, 20);
            ex.p_mnl__OpeningStockList = ex.p_mnl__OpeningStockPagedList.ToList();
            return View(ex);
        }

        [HttpPost]
        public PartialViewResult OpeningStock(SetupModelViewModel ex, int? page)
        {
            ViewBag.WareHouses = db.AMWarehouses.Where(u => u.BranchId == branch_ID).ToList();
            var WareHouseId = 0;
            if (ex.WareHouseId != null)
                WareHouseId = Convert.ToInt32(ex.WareHouseId);
            var list = AMProceduresModel.p_mnl__POSOpeningStock(db, WareHouseId, (short)branch_ID).ToList();
            if (!string.IsNullOrEmpty(ex.Search))
            {
                ex.Search = ex.Search.ToUpper();
                list = list.Where(u => (u.ItemName.ToUpper()).Contains(ex.Search.ToUpper()) ||
                    (u.ShortName == null ? "" : u.ShortName.ToUpper()).Contains(ex.Search.ToUpper()) ||
                    (u.Barcode == null ? "" : u.Barcode.ToUpper()).Contains(ex.Search.ToUpper())).ToList();
            }
            ex.p_mnl__OpeningStockPagedList = list.ToPagedList(page.HasValue ? Convert.ToInt32(page) : 1, 20);
            ex.p_mnl__OpeningStockList = ex.p_mnl__OpeningStockPagedList.ToList();
            ModelState.Clear();
            return PartialView("_PartialOpeningStockList", ex);
        }

        public JsonResult UpdateOpenningStock(int Id, decimal? stock = 0)
        {
            string result = "Failed";
            Model.AMOpeningStock _OpeningStock = db.AMOpeningStocks.Where(u => u.OpeningStockId == Id).FirstOrDefault();
            if (_OpeningStock != null)
            {
                _OpeningStock.Stock = stock;
                _OpeningStock.CreatedOn = DateTime.Now;
                _OpeningStock.CreatedBy = CreatedBy;
                db.Entry(_OpeningStock).State = EntityState.Modified;
                db.SaveChanges();
                if (stock != null)
                {
                    if (IsFixedAssetItem(_OpeningStock.ProductId))
                    {
                        InsertInvoiceProductDetails(_OpeningStock.OpeningStockId, _OpeningStock.ProductId, (decimal)stock);
                    }
                }
                result = "Updated";
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FetchFAItems(int id)
        {
            var vm = new PurchaseModelViewModel();
            vm.PIPDs = from detail in db.AMPurchaseInvoiceProductDetails
                       where detail.OpeningStockId == id
                       select new PurchaseInvoiceProductDetailVM()
                       {
                           PIPDId = detail.DetailId,
                           ItemCode = detail.ItemCode,
                           ItemName = (detail.Items != null ? detail.Items.ItemName : ""),
                           OpeningStockId = detail.OpeningStockId,
                       };
            return PartialView("_PartialPIPDModal", vm);
        }

        public ActionResult DeletePIPDItem(int id)
        {
            var vm = new PurchaseModelViewModel();
            bool status = false;
            string msg = "Deletion failed";
            try
            {
                var entity = db.AMPurchaseInvoiceProductDetails.Find(id);
                int? openingStockId = entity.OpeningStockId;
                ViewBag.openingStockId = openingStockId;
                if (entity != null)
                {
                    db.AMPurchaseInvoiceProductDetails.Remove(entity);
                    db.SaveChanges();
                    //var vm = new PurchaseModelViewModel();
                    //vm.PIPDs = from detail in db.AMPurchaseInvoiceProductDetails
                    //           where detail.OpeningStockId == openingStockId
                    //           select new PurchaseInvoiceProductDetailVM()
                    //           {
                    //               PIPDId = detail.DetailId,
                    //               ItemCode = detail.ItemCode,
                    //               ItemName = (detail.Items != null ? detail.Items.ItemName : ""),
                    //               OpeningStockId = detail.OpeningStockId,
                    //           };
                    vm.PIPDs = from detail in db.AMPurchaseInvoiceProductDetails
                               join pIP in db.AMPurchaseInvoiceProducts on detail.PurchaseInvoiceProductId equals pIP.PurchaseInvoiceProductId
                               join item in db.AMItems on pIP.ItemId equals item.ItemId
                               where pIP.PurchaseInvoiceProductId == entity.PurchaseInvoiceProductId
                               select new PurchaseInvoiceProductDetailVM
                               {
                                   PIPDId = detail.DetailId,
                                   ItemCode = detail.ItemCode,
                                   ItemName = item.ItemName,
                                   Qty = (int)detail.Qty
                               };
                    AMOpeningStock _OpeningStock = db.AMOpeningStocks.Where(u => u.OpeningStockId == openingStockId).FirstOrDefault();
                    if (_OpeningStock != null)
                    {
                        _OpeningStock.Stock = vm.PIPDs.Count();
                        _OpeningStock.CreatedOn = DateTime.Now;
                        _OpeningStock.CreatedBy = CreatedBy;
                        db.Entry(_OpeningStock).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    status = true;
                    msg = "Deletion success";
                    //return PartialView("_PartialPIPDModal", vm);
                }
                else
                {
                    //var vm = new PurchaseModelViewModel();
                    //vm.PIPDs = from detail in db.AMPurchaseInvoiceProductDetails
                    //           where detail.OpeningStockId == openingStockId
                    //           select new PurchaseInvoiceProductDetailVM()
                    //           {
                    //               PIPDId = detail.DetailId,
                    //               ItemCode = detail.ItemCode,
                    //               ItemName = (detail.Items != null ? detail.Items.ItemName : ""),
                    //               OpeningStockId = detail.OpeningStockId,
                    //           };
                    //return PartialView("_PartialPIPDModal", vm);
                }
            }
            catch (Exception)
            {
                throw;
            }

            List<Service.Helper> PartialList = new List<Service.Helper>();
            if (vm.PIPDs != null)
            {
                PartialList.Add(new Service.Helper { divToReplace = "UpdatePIPD", partialData = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialPIPDModal", vm) });
            }
            return Json(new { status, msg, PartialList }, JsonRequestBehavior.AllowGet);
        }
        private void InsertInvoiceProductDetails(int openingStockId, int itemId, decimal quantity)
        {
            var item = db.AMItems.Find(itemId);
            var category = db.AMCategories.Find(item.CategoryId);
            string newName = category.ShortName + "-" + item.ShortName + "-";
            var codes = db.AMPurchaseInvoiceProductDetails.Where(s => s.ItemCode.Contains(newName) && s.OpeningStockId == openingStockId).Select(s => s.ItemCode).ToList();
            var temp = codes.Max();
            if (string.IsNullOrEmpty(temp))
            {
                temp = db.AMPurchaseInvoiceProductDetails.Where(s => s.ItemCode.Contains(newName)).Select(s => s.ItemCode).Max();
            }
            for (int i = 0; i < (codes.Count() > 0 ? quantity - codes.Count() : quantity); i++)
            {
                var entity = new AMPurchaseInvoiceProductDetail();
                entity.OpeningStockId = openingStockId;
                if (string.IsNullOrEmpty(temp))
                {
                    entity.ItemCode = newName + String.Format("{0:D5}", i + 1);
                    entity.ItemId = item.ItemId;
                }
                else
                {
                    Regex re = new Regex(@"\d+");
                    Match result = re.Match(temp);
                    int numaricPart = Convert.ToInt32(result.Value);
                    entity.ItemCode = newName + String.Format("{0:D5}", numaricPart + i + 1);
                    entity.ItemId = item.ItemId;
                }


                db.AMPurchaseInvoiceProductDetails.Add(entity);
                db.SaveChanges();
            }
        }
        private bool IsFixedAssetItem(int id)
        {
            //var result = (from item in db.AMItems
            //              join cat in db.AMCategories on item.CategoryId equals cat.CategoryId
            //              join nature in db.AMNatures on cat.NatureId equals nature.NatureId
            //              where item.ItemId == id
            //              select nature.NatureName).FirstOrDefault();
            //if (result == "Fixed Assets")
            //    return true;
            //else
            return true;
        }
        #endregion

        private bool ChangeAccountControlAndTransaction(string autokey, bool isControl = false, bool isTranaction = true)
        {
            try
            {
                var account = db.Accounts.Find(autokey);
                account.CONTROLACCOUNT = isControl;
                account.ISTRANSACTION = isTranaction;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public string CreateNewAccountByParentAccountCode(string TITLE, string type, bool isControl = false, bool isTransaction = true)
        {
            var parentAccount = db.Accounts.Where(u => (u.TITLE == null ? "" : u.TITLE.ToUpper()) == type.ToUpper() && u.BYDEFAULT == true && u.BranchId == branch_ID).FirstOrDefault();
            if (parentAccount != null)
            {
                Account _Account = new Account();
                long? newAcc = GetNewAccoundID(parentAccount.autokey);
                if (newAcc != null)
                {
                    Guid gu = Guid.NewGuid();
                    _Account.ParentAccountId = parentAccount.autokey;
                    _Account.ParentId = parentAccount.ACCOUNT_ID;
                    _Account.BranchId = (short)branch_ID;
                    _Account.autokey = gu.ToString();
                    _Account.ACCOUNT_ID = Convert.ToInt64(newAcc);
                    _Account.TITLE = TITLE;
                    _Account.DEPTH = GetAccountDepth(parentAccount, parentAccount.autokey);
                    _Account.LINEAGE = GetAccountLineage(parentAccount, parentAccount.autokey);
                    _Account.CONTROLACCOUNT = isControl;
                    _Account.ISTRANSACTION = isTransaction;
                    _Account.GroupNo = 1;
                    _Account.BYDEFAULT = true;
                    db.Accounts.Add(_Account);
                    db.SaveChanges();
                    return _Account.autokey;
                }
                return null;
            }
            else return null;
        }

        private long? GetNewAccoundID(string autoKey)
        {
            long? newAcc = null;
            if (string.IsNullOrEmpty(autoKey))
                return newAcc;

            Account parentAccount = db.Accounts.Where(u => u.autokey == autoKey).FirstOrDefault();
            if (parentAccount != null)
            {
                newAcc = db.Accounts.Where(u => u.ParentId == parentAccount.ACCOUNT_ID && u.BranchId == branch_ID).Max(u => (long?)u.ACCOUNT_ID);

                if (newAcc == null)
                    newAcc = Convert.ToInt64(parentAccount.ACCOUNT_ID.ToString() + "0001");
                else
                    newAcc++;
            }
            return newAcc;
        }

        public short? GetAccountDepth(Account parentAccount, string autoKey)
        {
            short? Depth = null;
            //Account _parentAccount = db.Accounts.Where(u => u.autokey == autoKey).FirstOrDefault();
            if (parentAccount != null)
            {
                parentAccount.DEPTH++;
                Depth = parentAccount.DEPTH;
            }
            return Depth;
        }
        public string GetAccountLineage(Account parentAccount, string autoKey)
        {
            string lineage = null;
            //Account _parentAccount = db.Accounts.Where(u => u.autokey == autoKey).FirstOrDefault();
            if (parentAccount != null)
            {
                lineage = parentAccount.LINEAGE + parentAccount.ACCOUNT_ID.ToString() + "/";
            }
            return lineage;
        }

        #region FixItems
        public async Task<ActionResult> FixItems()
        {
            var _products = await db.AMItems.OrderBy(u => u.ItemId).ToListAsync();
            if (_products.Any())
            {
                string CategoryIds = "-1,";
                foreach (var item in _products.GroupBy(u => u.CategoryId))
                {
                    CategoryIds += item.Key.ToString() + ",";
                }
                ProceduresModel.ResetProductCode_AM(db, ProceduresModel.ReplaceLastOccurrence(CategoryIds, ",", ""));
            }
            else
            {
                TempData["error"] = "No product found";
                return View();
            }
            using (var trans = db.Database.BeginTransaction())
            {
                string C_InvAcc = "", C_COGSAcc = "", C_DepAcc = "", InvAcc = "", COGSAcc = "", DepAcc = "", message = "";
                int count = 0;
                try
                {
                    _products = await db.AMItems.OrderBy(u => u.ItemId).ToListAsync();
                    foreach (var item in _products)
                    {
                        if (item.AMCategory == null)
                            message += item.ItemName + " Category not found<br>";
                        else
                        {
                            C_COGSAcc = item.AMCategory.C_COGIAccountId;
                            C_DepAcc = item.AMCategory.C_DepreciationAccountId;
                            C_InvAcc = item.AMCategory.C_InventoryAccountId;
                            COGSAcc = item.AMCategory.T_COGIAccountId;
                            DepAcc = item.AMCategory.T_DepreciationAccountId;
                            InvAcc = item.AMCategory.T_InventoryAccountId;
                        }
                        if (!string.IsNullOrEmpty(C_InvAcc))
                        {
                            if (string.IsNullOrEmpty(InvAcc))
                                message += item.AMCategory.CategoryName + " Category's Inventory transaction account not found<br>";
                            if (!string.IsNullOrEmpty(C_COGSAcc))
                            {
                                if (string.IsNullOrEmpty(COGSAcc))
                                    message += item.AMCategory.CategoryName + " Category's COGI transaction account not found<br>";
                                if (!string.IsNullOrEmpty(C_DepAcc))
                                {
                                    if (string.IsNullOrEmpty(DepAcc))
                                        message += item.AMCategory.CategoryName + " Category's Depreciation transaction account not found<br>";
                                }
                                else
                                    message += item.AMCategory.CategoryName + " Category's Depreciation control account not found<br>";
                            }
                            else
                                message += item.AMCategory.CategoryName + " Category's COGI control account not found<br>";
                        }
                        else
                            message += item.AMCategory.CategoryName + " Category's Inventory control account not found<br>";

                        if (string.IsNullOrEmpty(message))
                        {
                            item.ProductCode = GenerateProductcode(Convert.ToInt32(item.CategoryId), item.ItemId);
                            if (SessionHelper.IsIndividualItemAccounts_AM)
                            {
                                if (item.IsCOGIAccountCreated == false)
                                    COGSAcc = ProceduresModel.CreateNewAccountByParentAccountCode(db, item.ItemName + "_COGI", C_COGSAcc, false);
                                else
                                    COGSAcc = item.COGIAccountId;

                                if (item.IsDepAccountCreated == false)
                                    DepAcc = ProceduresModel.CreateNewAccountByParentAccountCode(db, item.ItemName + "_DEP", C_DepAcc, false);
                                else
                                    DepAcc = item.DepreciationAccountId;

                                if (item.IsInvAccountCreated == false)
                                    InvAcc = ProceduresModel.CreateNewAccountByParentAccountCode(db, item.ItemName + "_INV", C_InvAcc, false);
                                else
                                    InvAcc = item.InventoryAccountId;
                            }

                            item.InventoryAccountId = InvAcc;
                            item.COGIAccountId = COGSAcc;
                            item.DepreciationAccountId = DepAcc;
                            item.IsDepAccountCreated = SessionHelper.IsIndividualItemAccounts_AM;
                            item.IsInvAccountCreated = SessionHelper.IsIndividualItemAccounts_AM;
                            item.IsCOGIAccountCreated = SessionHelper.IsIndividualItemAccounts_AM;
                        }

                        db.Entry(item).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        count++;

                    }
                    TempData["error"] = string.IsNullOrEmpty(message) == true ? null : message;
                    TempData["success"] = count + " product(s) updated";
                    //db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception)
                {
                    //trans.Rollback();
                    throw;
                }
            }
            return View();
        }
        #endregion

        #region FixCategory
        public async Task<ActionResult> FixCategories()
        {
            TempData["error"] = CategoryConstraint();
            if (!string.IsNullOrEmpty(TempData["error"].ToString()))
                return View();
            var _categogies = await db.AMCategories.Where(u => u.BranchId == branch_ID).OrderBy(u => u.CategoryId).ToListAsync();
            if (_categogies.Any())
            {
                string CategoryIds = "-1,";
                foreach (var item in _categogies.GroupBy(u => u.CategoryId))
                {
                    CategoryIds += item.Key.ToString() + ",";
                }
                ProceduresModel.ResetCategoryCode_AM(db, ProceduresModel.ReplaceLastOccurrence(CategoryIds, ",", ""));

            }
            else
            {
                TempData["error"] = "No category found";
                return View();
            }

            using (var trans = db.Database.BeginTransaction())
            {
                int count = 0;
                try
                {
                    _categogies = await db.AMCategories.Where(u => u.BranchId == branch_ID).OrderBy(u => u.CategoryId).ToListAsync();
                    foreach (var _Category in _categogies)
                    {
                        if (_Category != null)
                        {
                            //if (!_Category.CategoryCode.HasValue)
                            _Category.CategoryCode = ProceduresModel.p_mnl_GetNextCategoryCode_AM(db);
                            _Category.ModifiedOn = DateTime.Now;
                            _Category.ModifiedBy = SessionHelper.UserID;
                            if (string.IsNullOrEmpty(_Category.C_COGIAccountId))
                            {
                                _Category.C_COGIAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_COGI", COGIParentAccountId, true);//Create COGS control Account  
                            }
                            if (string.IsNullOrEmpty(_Category.T_COGIAccountId) && !string.IsNullOrEmpty(_Category.C_COGIAccountId))
                            {
                                _Category.T_COGIAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_COGI", _Category.C_COGIAccountId, false);//Create COGS transation Account                                
                            }
                            if (string.IsNullOrEmpty(_Category.C_InventoryAccountId))
                            {
                                _Category.C_InventoryAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_INV", InventoryParentAccountId, true);//Create INV control Account  
                            }
                            if (string.IsNullOrEmpty(_Category.T_InventoryAccountId) && !string.IsNullOrEmpty(_Category.C_InventoryAccountId))
                            {
                                _Category.T_InventoryAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_INV", _Category.C_InventoryAccountId, false);//Create INV transation Account                                
                            }
                            if (string.IsNullOrEmpty(_Category.C_DepreciationAccountId))
                            {
                                _Category.C_DepreciationAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_DEP", DepreciationParentAccountId, true);//Create INC control Account  
                            }
                            if (string.IsNullOrEmpty(_Category.T_DepreciationAccountId) && !string.IsNullOrEmpty(_Category.C_DepreciationAccountId))
                            {
                                _Category.T_DepreciationAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_DEP", _Category.C_DepreciationAccountId, false);//Create INC transation Account 
                            }
                            count++;
                        }
                        db.Entry(_Category).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    TempData["success"] = count + " Categories updated";
                    trans.Commit();
                }
                catch (Exception)
                {
                    //trans.Rollback();
                    throw;
                }
            }
            return View();
        }
        #endregion

        #region ManageWarehouses
        public ActionResult ManageWarehouses(int? page)
        {
            page = page ?? 1;
            pageNo = page.Value;
            var WarehouseVM = new SetupModelViewModel();
            GetWarehouses(WarehouseVM);
            if (Request.IsAjaxRequest())
            {
                var result = new
                {
                    PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "~/Areas/AM/Views/Setup/_PartialWarehousesList.cshtml", WarehouseVM),
                    GridId = "WarehousesGrid"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return View(WarehouseVM);
        }

        private void GetWarehouses(SetupModelViewModel vm)
        {
            IQueryable<AMWarehouse> queryable = db.AMWarehouses.Where(x => x.BranchId == branch_ID);
            if (!(string.IsNullOrEmpty(vm.Search)))
            {
                queryable = queryable.Where(s => s.WarehouseName.ToLower().Contains(vm.Search.ToLower()));
            }
            vm.AMWarehousePagedList = queryable.OrderBy(l => l.WarehouseName).ToPagedList(pageNo, 25);
        }

        public ActionResult SearchWarehouses(SetupModelViewModel vm)

        {
            GetWarehouses(vm);
            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "~/Areas/AM/Views/Setup/_PartialWarehousesList.cshtml", vm),
                GridId = "WarehousesGrid"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateWarehouse(SetupModelViewModel vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            string warnings = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = new AMWarehouse
                {

                    WarehouseName = vm.AMWarehouse.WarehouseName,
                    BranchId = SessionHelper.BranchId,
                    Phone = vm.AMWarehouse.Phone,
                    Address = vm.AMWarehouse.Address



                };
                db.AMWarehouses.Add(entity);
                try
                {
                    await db.SaveChangesAsync();
                    message = "Warehouse has been created successfully...";
                }
                catch (DbEntityValidationException ex)
                {
                    error = ex.GetExceptionMessages();
                }
                await db.SaveChangesAsync();
            }
            else
            {
                warnings = GetModelStateErrors();
            }
            GetWarehouses(vm);
            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "~/Areas/AM/Views/Setup/_PartialWarehousesList.cshtml", vm),
                Messages = message,
                Error = error,
                Warnings = warnings,
                GridId = "WarehousesGrid"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> EditWarehouse(int? id)
        {
            var WarehouseVM = new SetupModelViewModel();
            var entity = await db.AMWarehouses.FindAsync(id);
            if (id == null || entity == null)
            {
                return Json(new { Error = "Invalid request found...", }, JsonRequestBehavior.AllowGet);
            }
            //vm.FillFeeHeadDDs();
            WarehouseVM.AMWarehouse = entity;

            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "~/Areas/AM/Views/Setup/_PartialEditWarehouse.cshtml", WarehouseVM),
                TargetId = "frmModalContent",
                ModalId = "frmModal",
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> EditWarehouse(SetupModelViewModel vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            string warnings = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = await db.AMWarehouses.FindAsync(vm.AMWarehouse.WarehouseId);
                if (entity != null)
                {
                    entity.WarehouseName = vm.AMWarehouse.WarehouseName;
                    entity.Phone = vm.AMWarehouse.Phone;
                    entity.Address = vm.AMWarehouse.Address;




                    try
                    {
                        await db.SaveChangesAsync();
                        message = $"Warehouse '+{entity.WarehouseName}' has been updated successfully...";
                        // vm.CurrentBrands = db.C.OrderBy(l => l.CurrencyName).ToPagedList(pageNo, 25);
                    }
                    catch (DbEntityValidationException ex)
                    {
                        error = ex.GetExceptionMessages();
                    }
                }
                else
                {
                    error = "Warehouse not found";
                }
            }
            else
            {
                warnings = GetModelStateErrors();
            }

            GetWarehouses(vm);

            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "~/Areas/AM/Views/Setup/_PartialWarehousesList.cshtml", vm),
                Error = error,
                Warnings = warnings,
                Messages = message,
                GridId = "WarehousesGrid",
                Reset = "true",
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveWarehouse(int? id)
        {
            string message = string.Empty;
            string error = string.Empty;
            string warnings = string.Empty;
            var entity = await db.AMWarehouses.FindAsync(id);
            if (entity != null)
            {
                db.AMWarehouses.Remove(entity);
                try
                {
                    await db.SaveChangesAsync();
                    message = "Warehouse has been deleted successfully...";
                }
                catch (Exception)
                {
                    error = "Warehouse Cannot be deleted because it is in use...";
                }
            }
            string partialView = string.Empty;
            var WarehouseVM = new SetupModelViewModel();
            GetWarehouses(WarehouseVM);

            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "~/Areas/AM/Views/Setup/_PartialWarehousesList.cshtml", WarehouseVM),
                Error = error,
                Messages = message,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ManageWarehouses(SetupModelViewModel ex, string Command)
        {
            var list = db.AMWarehouses.Where(u => u.BranchId == branch_ID).ToList();
            switch (Command)
            {
                case "Create":
                    AMWarehouse _Warehouse = ex.AMWarehouse;
                    try
                    {
                        _Warehouse.BranchId = SessionHelper.BranchId;
                        //_Warehouse.CreatedOn = DateTime.Now;
                        //_Warehouse.Createdy = SessionHelper.UserID;
                        db.AMWarehouses.Add(_Warehouse);
                        db.SaveChanges();
                        ViewBag.success = "Ware house added successfully";
                        list = db.AMWarehouses.Where(u => u.BranchId == branch_ID).ToList();
                    }
                    catch
                    {
                        throw;
                    }
                    break;
                default:
                    if (!string.IsNullOrEmpty(ex.Search))
                    {
                        list = list.Where(u => (u.WarehouseName.ToUpper()).Contains(ex.Search.ToUpper())).ToList();
                    }
                    break;
            }

            ex.AMWarehouseList = list;
            return View(ex);
        }

        public JsonResult UpdateWarehouse(int id, string Name, string Address, string Phone)
        {
            string result = "false";
            if (id == 0)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            AMWarehouse _Warehouse = db.AMWarehouses.Where(u => u.WarehouseId == id).FirstOrDefault();
            if (_Warehouse != null)
            {
                _Warehouse.WarehouseName = Name;
                _Warehouse.Address = Address;
                _Warehouse.Phone = Phone;
            }
            try
            {
                db.Entry(_Warehouse).State = EntityState.Modified;
                db.SaveChanges();
                result = "Updated successfully";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult DeleteWarehouse(int? id)
        {
            var docSpec = db.AMWarehouses.Where(u => u.WarehouseId == id).FirstOrDefault();
            if (docSpec != null)
            {
                db.AMWarehouses.Remove(docSpec);
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

        #endregion


        #region ManageCategories
        public ActionResult ManageCategories(int? page)
        {
            page = page ?? 1;
            pageNo = page.Value;
            CategoriesVM = new ManageAMCategoryViewModel();

            GetCategories(CategoriesVM);
            return View(CategoriesVM);
        }

        private void GetCategories(ManageAMCategoryViewModel vm)
        {
            IQueryable<AMCategory> queryable = db.AMCategories.Where(u => u.BranchId == branch_ID);
            if (!(string.IsNullOrEmpty(vm.Search)))
            {
                queryable = queryable.Where(s => s.CategoryName.ToLower().Contains(vm.Search.ToLower()));
            }
            vm.AMCategoryPagedList = queryable.OrderBy(l => l.CategoryName).ToPagedList(pageNo, 25);
        }


        public ActionResult SearchCategories(ManageAMCategoryViewModel vm)
        {

            GetCategories(vm);
            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialCategoriesList", vm),
                GridId = "CategoriesGrid"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public async Task<ActionResult> CreateCategory(CategoriesViewModel vm)
        //{
        //    string message = string.Empty;
        //    string error = string.Empty;
        //    string warnings = string.Empty;
        //    if (ModelState.IsValid)
        //    {
        //        Models.POSCategory _Category = vm.CategoryItem;
        //        var checkexist = db.PosCategories.Where(u => u.BranchId == branch_ID).Where(u => u.CategoryName.ToUpper() == _Category.CategoryName.ToUpper()).FirstOrDefault();
        //        if (checkexist != null)
        //        {
        //            error = "Category Name must be unique";

        //        }
        //        else
        //        {
        //            ViewBag.error = CategoryConstraint();
        //            if (!string.IsNullOrEmpty(ViewBag.error))
        //            {

        //            }

        //            //using (var trans = db.Database.BeginTransaction())
        //            //{
        //            try
        //            {
        //                var idd = db.PosCategories.Max(u => (int?)u.CategoryId);
        //                if (idd == null)
        //                {
        //                    idd = 0;
        //                    var Parent = new POSCategory();
        //                    Parent.CategoryId = (int)idd;
        //                    Parent.CategoryName = "Main";
        //                    Parent.BranchId = branch_ID;

        //                    db.PosCategories.Add(Parent);
        //                    db.SaveChanges();
        //                }

        //                idd++;
        //                if (!vm.CategoryItem.ParentId.HasValue)
        //                {
        //                    var Main = db.PosCategories.Where(u => u.BranchId == branch_ID).Where(u => u.CategoryId == 0).FirstOrDefault();

        //                    _Category.ParentId = Main.CategoryId;
        //                }
        //                else
        //                {

        //                    _Category.ParentId = vm.CategoryItem.ParentId;

        //                }
        //                var parentCategory = db.PosCategories.Where(u => u.BranchId == branch_ID).Where(u => u.CategoryId == _Category.ParentId).FirstOrDefault();
        //                _Category.CategoryCode = ProceduresModel.p_mnl_GetNextCategoryCode_POS(db);
        //                _Category.CategoryId = Convert.ToInt32(idd);
        //                _Category.CostGroupId = vm.CategoryItem.CostGroupId;
        //                _Category.Lineage = GetCategoryLineage(parentCategory);
        //                _Category.Depth = GetCategoryDepth(parentCategory);
        //                _Category.IsLocked = false;
        //                _Category.CreatedOn = DateTime.Now;
        //                _Category.CreatedBy = SessionHelper.UserID;
        //                _Category.BranchId = branch_ID;
        //                _Category.IP = SessionHelper.IP;

        //                if (string.IsNullOrEmpty(_Category.C_CogsAccountId))
        //                {
        //                    _Category.C_CogsAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_COGS", COGSAccountId, true);//Create COGS control Account  
        //                }
        //                if (string.IsNullOrEmpty(_Category.T_CogsAccountId) && !string.IsNullOrEmpty(_Category.C_CogsAccountId))
        //                {
        //                    _Category.T_CogsAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_COGS", _Category.C_CogsAccountId, false);//Create COGS transation Account                                
        //                }
        //                if (string.IsNullOrEmpty(_Category.C_InventoryAccountId))
        //                {
        //                    _Category.C_InventoryAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_INV", InventoryAccountId, true);//Create INV control Account  
        //                }
        //                if (string.IsNullOrEmpty(_Category.T_InventoryAccountId) && !string.IsNullOrEmpty(_Category.C_InventoryAccountId))
        //                {
        //                    _Category.T_InventoryAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_INV", _Category.C_InventoryAccountId, false);//Create INV transation Account                                
        //                }
        //                if (string.IsNullOrEmpty(_Category.C_IncomeAccountId))
        //                {
        //                    _Category.C_IncomeAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_INC", IncomeAccountId, true);//Create INC control Account  
        //                }
        //                if (string.IsNullOrEmpty(_Category.T_IncomeAccountId) && !string.IsNullOrEmpty(_Category.C_IncomeAccountId))
        //                {
        //                    _Category.T_IncomeAccountId = ProceduresModel.CreateCategorySingleAccount(db, _Category.CategoryName + "_INC", _Category.C_IncomeAccountId, false);//Create INC transation Account 
        //                }

        //                db.PosCategories.Add(_Category);
        //                await db.SaveChangesAsync();
        //                //trans.Commit();
        //                message = "Category added successfully";
        //                //return RedirectToAction("ManageCategories");
        //            }
        //            catch (Exception ex)
        //            {
        //                //trans.Rollback();
        //                error = ex.GetExceptionMessages();
        //            }
        //            //}
        //        }

        //    }
        //    else
        //    {
        //        warnings = GetModelStateErrors();
        //    }
        //    FillCategoriesViewModelDDs(vm);
        //    GetCategories(vm);
        //    var result = new
        //    {
        //        PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialCategoriesList", vm),
        //        Messages = message,
        //        Error = error,
        //        Warnings = warnings,
        //        GridId = "CategoriesGrid",
        //        TotalRecords = vm.CurrenctCategories.TotalItemCount,
        //        ModalId = "PopupModal"
        //    };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        [HttpGet]
        public async Task<ActionResult> EditCategory(int? id)
        {
            CategoriesVM = new ManageAMCategoryViewModel();
            var entity = await db.AMCategories.FindAsync(id);
            if (id == null || entity == null)
            {
                return Json(new { Error = "Invalid request found...", }, JsonRequestBehavior.AllowGet);
            }
            CategoriesVM.AMCategory = entity;

            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialEditCategory", CategoriesVM),
                TargetId = "frmModalContent",
                ModalId = "frmModal",
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> EditCategoryn(ManageAMCategoryViewModel vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            string warnings = string.Empty;
            if (ModelState.IsValid)
            {
                var entity = await db.AMCategories.FindAsync(vm.AMCategory.CategoryId);
                if (entity != null)
                {
                    entity.CategoryName = vm.AMCategory.CategoryName;
                    entity.CategoryId = vm.AMCategory.CategoryId;
                    entity.ModifiedBy = SessionHelper.UserId;
                    entity.ModifiedOn = DateTime.Now;


                    try
                    {
                        await db.SaveChangesAsync();
                        message = $"Category '+{entity.CategoryName}' has been updated successfully...";
                        // vm.CurrentBrands = db.C.OrderBy(l => l.CurrencyName).ToPagedList(pageNo, 25);
                    }
                    catch (DbEntityValidationException ex)
                    {
                        error = ex.GetExceptionMessages();
                    }
                }
                else
                {
                    error = "Category not found";
                }
            }
            else
            {
                warnings = GetModelStateErrors();
            }
            GetCategories(vm);

            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialCategoriesList", vm),
                Error = error,
                Warnings = warnings,
                Messages = message,
                GridId = "CategoriesGrid",
                Reset = "true",
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> RemoveCategory(int? id)
        {
            string message = string.Empty;
            string error = string.Empty;
            string warnings = string.Empty;
            var entity = await db.AMCategories.FindAsync(id);
            if (entity != null)
            {
                db.AMCategories.Remove(entity);
                try
                {
                    await db.SaveChangesAsync();
                    message = "Category has been deleted successfully...";
                }
                catch (Exception)
                {
                    error = "Category Cannot be deleted because it is in use...";
                }
            }
            string partialView = string.Empty;
            CategoriesVM = new ManageAMCategoryViewModel();
            GetCategories(CategoriesVM);

            var result = new
            {
                PartialView = Service.CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialCategoriesList", CategoriesVM),
                Error = error,
                Messages = message,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}