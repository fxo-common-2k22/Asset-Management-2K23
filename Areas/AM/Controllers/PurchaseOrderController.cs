using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.INV.Models;
using FAPP.Model;
using FAPP.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FAPP.Areas.AM.Controllers
{
    public class PurchaseOrderController : FAPP.Controllers.BaseController
    {
        //private OneDbContext db = new OneDbContext();
        private int branch_ID = SessionHelper.BranchId;
        private int fiscalYearId = SessionHelper.FiscalYearId;
        private int CreatedBy = SessionHelper.UserID;
        // GET: AM/OrderPurchase
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddEditPurchaseOrder(int? id)
        {
            var vm = new AddOrderPurchaseViewModel();
            var purchaseOrder = db.InvPurchaseOrders.Find(id);
            if (purchaseOrder != null)
            {
                vm = GetPurchaseOrderById(purchaseOrder);
                vm.EditMode = true;
                var result = db.AMPurchaseInvoices.Where(s => s.PurchaseOrderId == purchaseOrder.PurchaseOrderId).FirstOrDefault();
                vm.PISupplierId = vm.SupplierId;
                if (result != null)
                {
                    vm.PICreated = true;
                    vm.PISupplierId = vm.SupplierId;

                    vm.Description = purchaseOrder.Description;
                    vm.PIDescription = "Purchase Invoice For Purchase Order Number " + purchaseOrder.PurchaseOrderCode;
                    vm.PurchaseInvoiceId = result.PurchaseInvoiceId;

                    //    vm.PurchaseInvoiceId = new AMPurchaseOrder();
                    //    ex.PurchaseOrder.PurchaseOrderId = db.AMPurchaseOrders
                    //.Where(u => u.RequestId == ex.Request.RequestId).Select(u => u.PurchaseOrderId).FirstOrDefault();
                }
                else
                {
                    vm.PICreated = false;
                }
            }
            else
            {
                vm.PurchaseOrderDate = DateTime.Now;

            }
            var DDs = FillDDs();
            vm.ItemsDD = DDs.ItemsDD;
            vm.SuppliersDD = DDs.SuppliersDD;
            vm.PurchaseInvoiceDate = DateTime.Now;
            return View(vm);
        }
        [HttpPost]
        public ActionResult SavePurchaseOrder(AddOrderPurchaseViewModel vm)
        {
            try
            {
                var purchaseOrder = db.InvPurchaseOrders.Find(vm.PurchaseOrderId);
                if (purchaseOrder != null)
                {
                    return UpdatePO(vm, purchaseOrder);
                }
                else
                {
                    return InsertNewPO(vm);
                }
            }
            catch (Exception x)
            {
                var innerEx = x.InnerException;
                throw;
            }

        }
        private ActionResult UpdatePO(AddOrderPurchaseViewModel vm, InvPurchaseOrder purchaseOrder)
        {
            purchaseOrder.PurchaseOrderDate = vm.PurchaseOrderDate;
            purchaseOrder.Description = vm.Description;
            purchaseOrder.ModifiedBy = CreatedBy;
            purchaseOrder.ModifiedOn = DateTime.Now;
            purchaseOrder.StatusId = vm.StatusId;
            purchaseOrder.SupplierId = vm.SupplierId;
            db.SaveChanges();
            if (vm.Details.Count() > 1)
            {
                for (int i = 0; i < vm.Details.Count() - 1; i++)
                {
                    var item = vm.Details[i];
                    var purchaseOP = db.InvPurchaseOrderProducts.Find(item.POProductId);
                    if (purchaseOP != null)
                    {
                        purchaseOP.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        purchaseOP.ProductId = item.ProductId;
                        purchaseOP.Quantity = item.Quantity;
                        purchaseOP.Description = item.Description;
                        purchaseOP.ConditionTypeId = item.ConditionTypeId;
                        db.SaveChanges();
                    }
                    else
                    {
                        var pOProduct = new InvPurchaseOrderProduct()
                        {
                            PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Description = item.Description,
                            ConditionTypeId = item.ConditionTypeId
                        };
                        db.InvPurchaseOrderProducts.Add(pOProduct);
                        db.SaveChanges();
                    }
                }
            }
            var DDs = FillDDs();
            vm.ItemsDD = DDs.ItemsDD;
            vm.SuppliersDD = DDs.SuppliersDD;
            vm.Details = (from det in db.InvPurchaseOrderProducts
                          where det.PurchaseOrderId == purchaseOrder.PurchaseOrderId
                          select new PurchaseOrderViewModel()
                          {
                              Quantity = (int)det.Quantity,
                              PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                              POProductId = det.PurchaseOrderProductId,
                              ProductId = det.ProductId,
                              Description = det.Description,
                              ConditionTypeId = det.ConditionTypeId
                          }).ToList();
            vm.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
            vm.PurchaseOrderCode = purchaseOrder.PurchaseOrderCode;
            vm.EditMode = true;
            return PartialView("_PartialAddPurchaseOrder", vm);
        }
        private ActionResult InsertNewPO(AddOrderPurchaseViewModel vm)
        {
            if (vm.PurchaseOrderDate != DateTime.MinValue)
            {
                var code = db.InvPurchaseOrders.Max(s => s.PurchaseOrderCode);
                var purchaseOrder = new InvPurchaseOrder()
                {
                    PurchaseOrderDate = vm.PurchaseOrderDate,
                    PurchaseOrderCode = StringFormate.CreatePurchaseCode(code),
                    StatusId = vm.StatusId,
                    SupplierId = vm.SupplierId,
                    Description = vm.Description,
                    CreatedOn = DateTime.Now,
                    CreatedBy = CreatedBy,
                };
                db.InvPurchaseOrders.Add(purchaseOrder);
                db.SaveChanges();
                if (vm.Details.Count() > 1)
                {
                    for (int i = 0; i < vm.Details.Count() - 1; i++)
                    {
                        var item = vm.Details[i];
                        var pOProduct = new InvPurchaseOrderProduct()
                        {
                            PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            Description = item.Description,
                            ConditionTypeId = item.ConditionTypeId
                        };
                        db.InvPurchaseOrderProducts.Add(pOProduct);
                    }
                    db.SaveChanges();
                }
                var DDs = FillDDs();
                vm.ItemsDD = DDs.ItemsDD;
                vm.SuppliersDD = DDs.SuppliersDD;
                vm.Details = (from det in db.InvPurchaseOrderProducts
                              where det.PurchaseOrderId == purchaseOrder.PurchaseOrderId
                              select new PurchaseOrderViewModel()
                              {
                                  Quantity = (int)det.Quantity,
                                  PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                                  POProductId = det.PurchaseOrderProductId,
                                  ProductId = det.ProductId,
                                  Description = det.Description,
                                  ConditionTypeId = det.ConditionTypeId
                              }).ToList();
                vm.EditMode = true;
                vm.PurchaseOrderCode = purchaseOrder.PurchaseOrderCode;
                vm.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
            }
            return PartialView("_PartialAddPurchaseOrder", vm);
        }
        private AddOrderPurchaseViewModel FillDDs()
        {
            var vm = new AddOrderPurchaseViewModel();
            vm.SuppliersDD = from client in db.Clients
                             where client.BranchId == branch_ID && client.IsSupplier == true
                             select new SelectListItem
                             {
                                 Text = client.Name,
                                 Value = client.ClientId.ToString()
                             };
            vm.ItemsDD = db.InvProducts.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true && u.IsFixedAsset == true).Select(s => new SelectListItem
            {
                Value = s.ProductId.ToString(),
                Text = s.ProductName.ToString()
            }).ToList();
            // from item in db.AMItems
            //             where item.BranchId == branch_ID
            //             select new SelectListItem
            //             {
            //                 Text = item.ItemName,
            //                 Value = item.ItemId.ToString()
            //             };
            ViewBag.ConditionTypesDD = from cond in db.InvConditionTypes
                                       select new SelectListItem
                                       {
                                           Text = cond.Name,
                                           Value = cond.ConditionTypeId.ToString()
                                       };

            ViewBag.Statuses = from sts in db.InvRequestStatuses
                               select new SelectListItem()
                               {
                                   Text = sts.StatusName,
                                   Value = sts.StatusId.ToString()
                               };
            return vm;
        }
        private AddOrderPurchaseViewModel GetPurchaseOrderById(InvPurchaseOrder purchaseOrder)
        {
            var vm = new AddOrderPurchaseViewModel();
            vm.Description = purchaseOrder.Description;
            vm.PurchaseOrderCode = purchaseOrder.PurchaseOrderCode;
            vm.PurchaseOrderDate = purchaseOrder.PurchaseOrderDate;
            vm.SupplierId = purchaseOrder.SupplierId;
            vm.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
            vm.Details = (from det in db.InvPurchaseOrderProducts
                          where det.PurchaseOrderId == purchaseOrder.PurchaseOrderId
                          select new PurchaseOrderViewModel()
                          {
                              Quantity = (int)det.Quantity,
                              PurchaseOrderId = purchaseOrder.PurchaseOrderId,
                              POProductId = det.PurchaseOrderProductId,
                              ProductId = det.ProductId,
                              Description = det.Description,
                              ConditionTypeId = det.ConditionTypeId,
                          }).ToList();
            return vm;
        }

        [HttpGet]
        public ActionResult ManagePurchaseOrders()
        {

            var vm = new ManageOrderPurchaseViewModel()
            {
                PurchaseOrders = db.InvPurchaseOrders,
                SuppliersDD = from clt in db.Clients
                              where clt.IsSupplier == true
                              select new SelectListItem
                              {
                                  Text = clt.Name,
                                  Value = clt.ClientId.ToString()
                              }
            };
            return View(vm);
        }
        [HttpPost]
        public ActionResult ManagePurchaseOrders(ManageOrderPurchaseViewModel vm)
        {
            var records = db.InvPurchaseOrders.ToList();
            if (!string.IsNullOrEmpty(vm.PurchaseOrderCode))
            {
                records = records.Where(s => s.PurchaseOrderCode.Contains(vm.PurchaseOrderCode) || s.PurchaseOrderCode.StartsWith(vm.PurchaseOrderCode)).ToList();
            }
            if (vm.OrderFrom != null && vm.OrderFrom != DateTime.MinValue && vm.OrderTo != null && vm.OrderTo != DateTime.MinValue)
            {
                records = records.Where(s => s.PurchaseOrderDate.Month >= vm.OrderFrom.Value.Month &&
                                            s.PurchaseOrderDate.Year >= vm.OrderFrom.Value.Year &&
                                            s.PurchaseOrderDate.Day >= vm.OrderFrom.Value.Day &&
                                            s.PurchaseOrderDate.Month <= vm.OrderTo.Value.Month &&
                                            s.PurchaseOrderDate.Year <= vm.OrderTo.Value.Year &&
                                            s.PurchaseOrderDate.Day <= vm.OrderTo.Value.Day
                                            ).ToList();
            }
            else
            {
                if (vm.OrderFrom != null && vm.OrderFrom != DateTime.MinValue)
                {
                    records = records.Where(s => s.PurchaseOrderDate.Month >= vm.OrderFrom.Value.Month &&
                                                 s.PurchaseOrderDate.Year >= vm.OrderFrom.Value.Year &&
                                                 s.PurchaseOrderDate.Day >= vm.OrderFrom.Value.Day
                                                 ).ToList();
                }
                if (vm.OrderTo != null && vm.OrderTo != DateTime.MinValue)
                {
                    records = records.Where(s => s.PurchaseOrderDate.Month <= vm.OrderTo.Value.Month &&
                                                 s.PurchaseOrderDate.Year <= vm.OrderTo.Value.Year &&
                                                 s.PurchaseOrderDate.Day <= vm.OrderTo.Value.Day
                                                 ).ToList();
                }
            }

            if (vm.SupplierId != null)
            {
                records = records.Where(s => s.SupplierId == (int)vm.SupplierId).ToList();
            }
            vm.PurchaseOrders = records;
            return PartialView("_PartialManageOrderPurchases", vm);
        }

        [HttpPost]
        public long SavePurchaseInvoice(AddOrderPurchaseViewModel vm)
        {
            try
            {
                var CurrencyId = db.Currencies.Select(u => u.CurrencyId).FirstOrDefault();
                var value = db.CurrencyValues.Where(u => u.CurrencyId == CurrencyId).Select(p => p.Value).FirstOrDefault();
                var idd = db.AMPurchaseInvoices.Max(u => (Int64?)u.PurchaseInvoiceId);
                if (idd == null)
                    idd = 1000;
                idd++;
                AMPurchaseInvoice purchaseInvoice = new AMPurchaseInvoice()
                {
                    BranchId = (short)branch_ID,
                    CreatedBy = CreatedBy,
                    CreatedOn = DateTime.Now,
                    Description = vm.PIDescription,
                    Discount = 0,
                    IsAccountPosted = false,
                    IsApplyTax = false,
                    IsCancelled = false,
                    IsPosted = false,
                    MovedToWarehouse = false,
                    CurrencyId = CurrencyId,
                    ExchangeRate = value,
                    PurchaseInvoiceDate = vm.PurchaseInvoiceDate,
                    PurchaseInvoiceId = Convert.ToInt64(idd),
                    PurchaseOrderId = vm.PurchaseOrderId,
                    SupplierId = vm.PISupplierId,
                };
                db.AMPurchaseInvoices.Add(purchaseInvoice);
                db.SaveChanges();
                List<AMPurchaseOrderProduct> purchaseOPs = (from products in db.AMPurchaseOrderProducts
                                                            where products.PurchaseOrderId == vm.PurchaseOrderId
                                                            select products).ToList();
                foreach (var item in purchaseOPs)
                {
                    decimal? unitPrice = (from prod in db.AMItems
                                          where prod.ItemId == item.ItemId
                                          select prod.Price).FirstOrDefault();
                    AMPurchaseInvoiceProduct purchaseInvoiceProduct = new AMPurchaseInvoiceProduct()
                    {
                        ItemId = item.ItemId,
                        PurchaseInvoiceId = purchaseInvoice.PurchaseInvoiceId,
                        ConditionTypeId = item.ConditionTypeId,
                        Quantity = item.Quantity,
                        Description = item.Description,
                        UnitPrice = (unitPrice != null ? (decimal)unitPrice : 0),
                        LineTotal = (unitPrice != null ? (decimal)unitPrice * item.Quantity : 0),
                        NetTotal = (unitPrice != null ? (decimal)unitPrice * item.Quantity : 0),
                        Tax = 0,
                    };
                    db.AMPurchaseInvoiceProducts.Add(purchaseInvoiceProduct);
                    db.SaveChanges();
                    //ProceduresModel.InsertInvoiceProductDetails(db, purchaseInvoiceProduct.PurchaseInvoiceProductId, item.ItemId, purchaseInvoiceProduct.Quantity, IsFixedAssetItem(purchaseInvoiceProduct.ItemId));
                    //if (IsFixedAssetItem(item.ItemId))
                    //{
                    //    InsertInvoiceProductDetails(purchaseInvoiceProduct.PurchaseInvoiceProductId, item.ItemId, purchaseInvoiceProduct.Quantity);
                    //}
                }

                return purchaseInvoice.PurchaseInvoiceId;
            }
            catch (Exception ex)
            {
                return 0;
                throw;
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
            //    return false;
            var result = db.InvProducts.Where(u => u.BranchId == branch_ID && u.IsInventoryItem == true && u.ProductId == id).FirstOrDefault();
            if (result != null)
            {
                return result.IsFixedAsset;
            }
            return false;
            //     return result;
        }
    }
}