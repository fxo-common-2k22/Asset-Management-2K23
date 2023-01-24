using FAPP.DAL;
using FAPP.Model;
using FAPP.ViewModel.Common;
using FAPP.ViewModel.Common.Finance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAPP.Controllers
{
    // GET: SharedFinance
    public class SharedFinanceController : BaseController
    {
        short branch_ID = Convert.ToInt16(SessionHelper.BranchId);
        // GET: SharedFinance
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewVoucher(Int64? id, string vt)
        {
            if (string.IsNullOrEmpty(vt))
            {
                vt = "CP";
            }

            VoucherAndReceiptModel ex = GetVoucher(id, vt);
            return View(ex);
        }
        VoucherAndReceiptModel GetVoucher(Int64? id, string vt)
        {
            VoucherAndReceiptModel ex = new VoucherAndReceiptModel();
            ex.VoucherNo = id;
            FillDD_Vouchers();
            ex.Voucher = new Voucher();
            ex.Voucher.TransactionDate = DateTime.Now;
            ex.Voucher.Currency = new Currency { CurrencyName = "", CurrencyId = 0 };
            ex.Voucher.FiscalYear = db.FiscalYears.FirstOrDefault(x => x.FiscalYearId == SessionHelper.FiscalYearId);
            ViewBag.EditMode = false;
            ViewBag.IsPosted = false;
            if (id > 0)
            {
                // db.Entry(Voucher).Reload();
                ex.Voucher = db.Vouchers.Where(u => u.VoucherId == id).Include(s => s.FiscalYear).FirstOrDefault();
                db.Entry(ex.Voucher).Reload();
                // ex.Voucher= 
                //ex.Voucher = db.Vouchers.Where(u => u.VoucherId == id).Include(s => s.FiscalYear).FirstOrDefault();
                if (ex.Voucher != null)
                {
                    ViewBag.IsCancelled = ex.Voucher.IsCancelled;
                    if (ex.Voucher.IsCancelled)
                    {
                        ViewBag.Cancelled = "Voucher has been cancelled by " + ex?.Voucher?.CancelledByUser?.Username + " on " + ex.Voucher?.CancelledOn?.ToShortDateString();
                    }

                    ViewBag.IsPosted = ex.Voucher.IsPosted;
                    if (ex.Voucher.IsPosted)
                    {
                        ViewBag.success = "Voucher has been Posted by" + ex?.Voucher?.PostedByUser?.Username + " on " + ex?.Voucher?.PostedOn?.ToShortDateString();
                    }
                    if (!string.IsNullOrEmpty(ex.Voucher.CBAccountId))
                    {
                        ex.CashBankAccount = db.Accounts.FirstOrDefault(x => x.autokey == ex.Voucher.CBAccountId).TITLE;
                    }

                    ViewBag.EditMode = true;
                    ViewBag.ModifiedBy = ex?.Voucher?.ModifiedByUser?.Username;
                    ViewBag.CreatedBy = ex?.Voucher?.CreatedByUser?.Username;
                }
                ex.VoucherDetail = db.VoucherDetails.Include(s => s.Account).Where(u => u.VoucherId == id).ToList();

                ex.AmountInWords = ProceduresModel.NumberToWords((double)ex.VoucherDetail.Sum(x => x.Debit));
            }
            else
            {
                if (!string.IsNullOrEmpty(vt))
                {
                    ex.Voucher.VoucherType = vt.ToUpper();
                }
            }
            if (ex.Voucher != null)
            {
                var type = db.VoucherTypes.Where(u => u.VoucherTypeId == ex.Voucher.VoucherType).Select(u => u.VoucherTypeName).FirstOrDefault();

                if (string.IsNullOrEmpty(type))
                {
                    ViewBag.PageTitle = "Voucher";
                }
                else
                {
                    ViewBag.PageTitle = type;
                }
            }
            else
            {
                ex.Voucher = new Voucher();
                //ex.Voucher.Currency = new Currency { CurrencyName = "", CurrencyId = 0 };
            }
            return ex;
        }


        void FillDD_Vouchers()
        {
            ViewBag.Currency = db.Currencies.ToList();
            var CashA = ProceduresModel.p_mnl_Account_GetCashAndBankAccounts(db, true, false, branch_ID, null).ToList();
            var BankA = ProceduresModel.p_mnl_Account_GetCashAndBankAccounts(db, false, true, branch_ID, null).ToList();
            ViewBag.CashAccount = CashA;
            ViewBag.BankAccount = BankA;
            ViewBag.CashBankAccounts = CashA.Concat(BankA).ToList();
            ViewBag.TransactionAccount = ProceduresModel.p_mnl_dbo_Account__Search(db, true, null, null, null, branch_ID, true).ToList();
            ViewBag.CostGroups = ProceduresModel.p_mnl_CostGroup_Search(db, branch_ID).ToList();
        }
    }
}