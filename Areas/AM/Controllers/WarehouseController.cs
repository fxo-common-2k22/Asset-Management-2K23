using FAPP.Areas.AM.BLL;
using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace FAPP.Areas.AM.Controllers
{
    public class WarehouseController : FAPP.Controllers.BaseController
    {
        //OneDbContext db = new OneDbContext();
        short branch_ID = Convert.ToInt16(SessionHelper.BranchId);

        // GET: Shop/Warehouse
        public ActionResult Index()
        {
            return View();
        }

        #region Log
        public ActionResult Log()
        {

            WarehouseViewModel ex = new WarehouseViewModel();
            DateTime now = DateTime.Now;
            ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.ToDate = now.ToShortDateString();
            ex.WarehouseProductList = WarehouseProductList(ex);
            return View(ex);
        }

        [HttpPost]
        public ActionResult Log(WarehouseViewModel ex, string Command)
        {

            ex.WarehouseProductList = WarehouseProductList(ex);
            ModelState.Clear();
            return View(ex);
        }

        void FillDD()
        {
            ViewBag.TransferMethods = AMProceduresModel.p_mnl_POSTranferMethods(db).ToList();
            ViewBag.TransferTypes = AMProceduresModel.p_mnl_POSTranferTypes(db).ToList();
            ViewBag.Warehouses = db.AMWarehouses.ToList();
            ViewBag.Products = db.AMItems.Where(u => u.BranchId == branch_ID).ToList();
        }

        List<AMWarehouseProduct> WarehouseProductList(WarehouseViewModel ex)
        {
            FillDD();
            var list = db.AMWarehouseProducts.ToList();
            if (!string.IsNullOrEmpty(ex.FromDate) && !string.IsNullOrEmpty(ex.ToDate))
            {
                var fdate = Convert.ToDateTime(ex.FromDate);
                var tdate = Convert.ToDateTime(ex.ToDate);
                list = db.AMWarehouseProducts.Where(u => DbFunctions.TruncateTime(u.TransferDate) >= fdate && DbFunctions.TruncateTime(u.TransferDate) <= tdate).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(ex.FromDate))
                {
                    var date = Convert.ToDateTime(ex.FromDate);
                    list = db.AMWarehouseProducts.Where(u => DbFunctions.TruncateTime(u.TransferDate) >= date).ToList();
                }
                if (!string.IsNullOrEmpty(ex.ToDate))
                {
                    var date = Convert.ToDateTime(ex.ToDate);
                    list = db.AMWarehouseProducts.Where(u => DbFunctions.TruncateTime(u.TransferDate) <= date).ToList();
                }
            }
            if (!string.IsNullOrEmpty(ex.Search))
            {
                ex.Search = ex.Search.ToUpper();
                list = list.Where(u => (u.InvoiceId.ToString() == ex.Search)).ToList();
            }
            if (ex.TransferMethod != null)
                list = list.Where(u => u.TransferMethod == ex.TransferMethod).ToList();
            if (ex.TransferType != null)
                list = list.Where(u => u.TransferType == ex.TransferType).ToList();
            if (ex.WarehouseId != null)
                list = list.Where(u => u.WarehouseId == ex.WarehouseId).ToList();
            if (ex.ProductId != null)
                list = list.Where(u => u.ItemId == ex.ProductId).ToList();

            ex.WarehouseProductList = list;
            return ex.WarehouseProductList;
        }

        #endregion

        #region StockPosition

        public ActionResult StockPosition()
        {
            WarehouseViewModel ex = new WarehouseViewModel();
            ViewBag.Warehouses = db.AMWarehouses.Where(x => x.BranchId == branch_ID).ToList();
            DateTime now = DateTime.Now;
            ex.FromDate = new DateTime(now.Year, now.Month, 1).ToShortDateString();
            ex.ToDate = now.ToShortDateString();
            return View(ex);
        }

        [HttpPost]
        public ActionResult StockPosition(WarehouseViewModel ex, string Command)
        {

            ViewBag.Warehouses = db.AMWarehouses.Where(x => x.BranchId == branch_ID).ToList();
            ex.v_mnl_OpenningstockList = OpenningstockList(ex);
            ModelState.Clear();
            return View(ex);
        }


        List<v_mnl_Openningstock_Result> OpenningstockList(WarehouseViewModel ex)
        {
            var list = AMProceduresModel.p_mnl_POSOpenningstock(db, ex.WarehouseId, Convert.ToDateTime(ex.FromDate), Convert.ToDateTime(ex.ToDate)).ToList();
            ex.v_mnl_OpenningstockList = list;
            return ex.v_mnl_OpenningstockList;
        }

        #endregion
    }
}