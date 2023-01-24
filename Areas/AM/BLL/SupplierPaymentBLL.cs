using FAPP.DAL;
using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
namespace FAPP.Areas.AM.BLL
{
    public class SupplierPaymentBLL
    {

        short branch_ID = Convert.ToInt16(SessionHelper.BranchId);

        public long? CreateModifySupplierPaymentVoucher(OneDbContext db, int SupplierPaymentId, int userId, bool isPosted)
        {
            long? processedVoucherId = null;
            // save header 
            try
            {
                var currencyId = db.Currencies.Select(u => u.CurrencyId).FirstOrDefault();
                var value = db.CurrencyValues.Where(u => u.CurrencyId == currencyId).Select(p => p.Value).FirstOrDefault();
                var SupplierPayment = db.SupplierPayments.Where(u => u.BranchId == branch_ID && u.SupplierPaymentId == SupplierPaymentId).FirstOrDefault();
                var transactionDate = SupplierPayment != null && SupplierPayment.PaymentDate.HasValue ? (DateTime)SupplierPayment.PaymentDate : DateTime.Now;
                long VoucherId = 0;
                if (!SupplierPayment.VoucherId.HasValue)
                {

                    VoucherId = ProceduresModel.GetVoucherId(db, SessionHelper.BranchId, "PIP", 0, transactionDate);

                    Voucher _vouvher = new Voucher();

                    if (VoucherId > 0)
                    {
                        _vouvher.VoucherType = "PIP";
                        _vouvher.TransactionDate = transactionDate;
                        //_vouvher.VoucherStatus = "Draft";
                        _vouvher.CurrencyId = currencyId;
                        _vouvher.ExchangeRate = value;
                        _vouvher.VoucherId = VoucherId;
                        _vouvher.Particulars = SupplierPayment != null ? SupplierPayment.Description : "Voucher For SupplierPayment" + SupplierPaymentId;
                        _vouvher.CBAccountId = db.Clients.Where(u => u.ClientId == SupplierPayment.SupplierId).Select(u => u.AccountId).FirstOrDefault();
                        _vouvher.VoucherId = VoucherId;
                        _vouvher.BranchId = branch_ID;
                        _vouvher.DebitAmount = _vouvher.CreditAmount = Convert.ToDecimal(SupplierPayment.Amount);

                        if (isPosted == true)
                        {
                            _vouvher.VoucherStatus = "Posted";
                            _vouvher.IsPosted = true;
                            _vouvher.PostedOn = DateTime.Now;
                            _vouvher.PostedBy = userId;
                        }
                        else
                        {
                            _vouvher.VoucherStatus = "Draft";
                        }

                        _vouvher.CreatedOn = DateTime.Now;
                        _vouvher.CreatedBy = userId;



                        db.Vouchers.Add(_vouvher);

                    }

                    var fiscalYear = db.FiscalYears.Where(s => _vouvher.TransactionDate >= s.StartDate && _vouvher.TransactionDate <= s.EndDate && s.BranchId == SessionHelper.BranchId).FirstOrDefault();
                    if (fiscalYear != null)
                    {
                        _vouvher.FiscalYearId = fiscalYear.FiscalYearId;
                    }
                    else
                    {
                        _vouvher.FiscalYearId = SessionHelper.FiscalYearId;
                    }
                    _vouvher.CreatedOn = DateTime.Now;


                    var voucher = db.Vouchers.Where(m => m.BranchId == branch_ID && m.VoucherId == VoucherId).FirstOrDefault();

                    var debitEntry = new VoucherDetail();
                    debitEntry.VoucherId = VoucherId;
                    debitEntry.AccountId = SupplierPayment.Client.AccountId;
                    //debitEntry.Credit = debitEntry.Debit = Convert.ToDecimal(Amount);
                    debitEntry.Debit = Convert.ToDecimal(SupplierPayment.Amount);
                    debitEntry.Credit = 0;


                    debitEntry.Narration = SupplierPayment != null ? SupplierPayment.Description : "Debit Entry";
                    debitEntry.TransactionType = "Dr";
                    debitEntry.TransactionId = 1;




                    var creditEntry = new VoucherDetail();
                    creditEntry.VoucherId = VoucherId;
                    creditEntry.AccountId = SupplierPayment.AccountId;
                    creditEntry.Narration = SupplierPayment != null ? SupplierPayment.Description : "Credit Entry";
                    creditEntry.Debit = 0;
                    creditEntry.Credit = Convert.ToDecimal(SupplierPayment.Amount);
                    creditEntry.TransactionType = "Cr";
                    creditEntry.TransactionId = 2;


                    db.VoucherDetails.Add(creditEntry);
                    db.VoucherDetails.Add(debitEntry);
                    processedVoucherId = VoucherId;


                }
                else
                {
                    var voucher = db.Vouchers.Where(m => m.VoucherId == SupplierPayment.VoucherId).FirstOrDefault();
                    voucher.CreditAmount = voucher.DebitAmount = Convert.ToDecimal(SupplierPayment.Amount);
                    voucher.ModifiedOn = DateTime.Now;
                    voucher.TransactionDate = transactionDate;
                    if (isPosted == true)
                    {
                        voucher.VoucherStatus = "Posted";
                        voucher.IsPosted = true;
                        voucher.PostedOn = DateTime.Now;
                        voucher.PostedBy = userId;
                    }

                    var voucherDetails = db.VoucherDetails.Where(v => v.VoucherId == SupplierPayment.VoucherId).ToList();
                    if (voucherDetails != null)
                    {
                        db.VoucherDetails.RemoveRange(voucherDetails);
                    }


                    var debitEntry = new VoucherDetail();
                    debitEntry.VoucherId = voucher.VoucherId;
                    debitEntry.AccountId = SupplierPayment.Client.AccountId;
                    //debitEntry.Credit = debitEntry.Debit = Convert.ToDecimal(Amount);
                    debitEntry.Debit = Convert.ToDecimal(SupplierPayment.Amount);
                    debitEntry.Credit = 0;


                    debitEntry.Narration = SupplierPayment != null ? SupplierPayment.Description : "Debit Entry"; ;
                    debitEntry.TransactionType = "Dr";
                    debitEntry.TransactionId = 1;



                    var creditEntry = new VoucherDetail();
                    creditEntry.VoucherId = voucher.VoucherId;
                    creditEntry.AccountId = SupplierPayment.AccountId;
                    creditEntry.Narration = SupplierPayment != null ? SupplierPayment.Description : "Credit Entry";
                    creditEntry.Debit = 0;
                    creditEntry.Credit = Convert.ToDecimal(SupplierPayment.Amount);
                    creditEntry.TransactionType = "Cr";
                    creditEntry.TransactionId = 2;


                    db.VoucherDetails.Add(creditEntry);
                    db.VoucherDetails.Add(debitEntry);
                    processedVoucherId = voucher.VoucherId;
                }



                SupplierPayment.IsPosted = true;
                SupplierPayment.PostedBy = userId;
                SupplierPayment.PostedOn = DateTime.Now;
                SupplierPayment.VoucherId = processedVoucherId;


                db.SaveChanges();
                return processedVoucherId;
            }
            catch (Exception)
            {

                return null;
            }


        }


    }
}