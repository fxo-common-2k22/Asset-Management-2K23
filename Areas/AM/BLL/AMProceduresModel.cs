using FAPP.DAL;
//using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FAPP.Areas.AM.ViewModels;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;
using System.Web;
using FAPP.Model;

namespace FAPP.Areas.AM.BLL
{
    public static class AMProceduresModel
    {
        static short branch_ID = SessionHelper.BranchId;
        static string v__Accounts = "SELECT        CASE 1 WHEN 1 THEN REPLICATE('-', (a.DEPTH - 1) * 2) ELSE '' END + CASE WHEN CONTROLACCOUNT = 1 THEN '[C]' WHEN ISTRANSACTION = 1 THEN '[T]' ELSE '[]' END + ' [ ' + CAST(a.ACCOUNT_ID AS VARCHAR(20)) + ' ] ' + UPPER(a.TITLE) AS TitleS, '[' + CAST(ACCOUNT_ID as VARCHAR(50)) + '] ' + TITLE as ATitle, a.autokey, a.LINEAGE, CASE WHEN a.BYDEFAULT = 'True' THEN 'lock.png' ELSE 'Spacer.gif' END AS Locked, a.BYDEFAULT AS IsLocked, CASE WHEN[ACCOUNT_ID] = 0 THEN 'False' ELSE 'True' END AS IsRoot, a.LINEAGE + CAST(a.ACCOUNT_ID AS VARCHAR(24)) AS OB,ch.Children, a.ParentId, a.DEPTH, a.ACCOUNT_ID, a.BranchId, a.CONTROLACCOUNT, a.AccountCode, a.TITLE, a.TITLE AS AccountTitle, a.ISTRANSACTION, ControlAccountRef FROM            Finance.Accounts AS a LEFT OUTER JOIN (SELECT        COUNT(*) AS Children, ParentId FROM [Finance].[Accounts] AS aa GROUP BY ParentId) AS ch ON a.ACCOUNT_ID = ch.ParentId";

        public static object GetDBNullOrValue<T>(this T val)
        {
            bool isDbNull = true;
            var t = typeof(T);

            if (Nullable.GetUnderlyingType(t) != null)
            {
                isDbNull = EqualityComparer<T>.Default.Equals(default(T), val);
            }
            else if (t.IsValueType)
            {
                isDbNull = false;
            }
            else
            {
                isDbNull = val == null;
            }

            return isDbNull ? DBNull.Value : (object)val;
        }

        #region Cash/Bank Voucher
        public static Int64 GetVoucherId(OneDbContext db, short? BranchId, string VoucherType, Int64 VoucherId, DateTime? TransactionDate)
        {
            var result = db.Database.SqlQuery<Int64>("DECLARE @VoucherPrefix VARCHAR(50) = '',           @BranchCode    VARCHAR(10),           @VoucherTypeNo VARCHAR(5),           @TwoDigitYear  VARCHAR(2);            SELECT @VoucherTypeNo = [VoucherTypeNo]          FROM[Finance].[VoucherTypes] AS[vt]          WHERE[vt].[VoucherTypeId] = @VoucherType        SELECT @BranchCode = CAST([BranchCode] AS VARCHAR(10)) FROM [Company].[Branches]          WHERE[BranchId] = @BranchId;            SELECT @TwoDigitYear = RIGHT(CONVERT(CHAR(4),YEAR(@TransactionDate)), 2)         SET @VoucherPrefix = @VoucherTypeNo + @BranchCode + @TwoDigitYear        SELECT @VoucherId = MAX(CAST(RIGHT(CAST([v].[VoucherId] AS VARCHAR), 7) AS BIGINT))         FROM[Finance].[Vouchers] AS[v]          WHERE[v].[VoucherType] = @VoucherType            AND[v].[BranchId] = @BranchId            AND YEAR(@TransactionDate) = YEAR([v].[TransactionDate])		  GROUP BY[v].[VoucherType] , [v].[BranchId]        IF @VoucherId IS NULL            BEGIN                SET @VoucherId = @VoucherPrefix + '00001';			END;	ELSE            BEGIN                SET @VoucherId = CAST(RIGHT(CAST(@VoucherId AS VARCHAR(20)), 5) AS INT);        SET @VoucherId+=1;        SET @VoucherId = CAST(@VoucherPrefix + RIGHT('00000' + CAST(@VoucherId AS VARCHAR(20)), 5) AS BIGINT);        IF EXISTS(SELECT* FROM Finance.Vouchers v WHERE v.VoucherId = @VoucherId)                BEGIN                    SELECT @VoucherId = MAX(CAST(RIGHT(CAST([v].[VoucherId] AS VARCHAR) , 7) AS BIGINT))     FROM[Finance].[Vouchers]        AS [v]                   WHERE[v].[VoucherType] = @VoucherType AND YEAR(@TransactionDate) = YEAR([v].[TransactionDate])                    GROUP BY[v].[VoucherType]        SET @VoucherId = CAST(RIGHT(CAST(@VoucherId AS VARCHAR(20)), 5) AS INT);        SET @VoucherId+=1;        SET @VoucherId = CAST(@VoucherPrefix + RIGHT('00000' + CAST(@VoucherId AS VARCHAR(20)), 5) AS BIGINT);        END    END;        SELECT @VoucherId",
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId)),
                         new SqlParameter("@VoucherType", GetDBNullOrValue(VoucherType)),
                         new SqlParameter("@VoucherId", GetDBNullOrValue(VoucherId)),
                         new SqlParameter("@TransactionDate", GetDBNullOrValue(TransactionDate))
                         ).FirstOrDefault();
            return result;
        }

        public static Int64 Voucher_Insert(OneDbContext db, Voucher objVoucher)
        {
            Int64 VoucherId = GetVoucherId(db, SessionHelper.BranchId, objVoucher.VoucherType, 0, objVoucher.TransactionDate);
            if (VoucherId > 0)
            {
                Voucher _Voucher = new Voucher();
                _Voucher.VoucherId = VoucherId;
                _Voucher.VoucherStatus = "Draft";
                _Voucher.TransactionDate = objVoucher.TransactionDate;
                if (_Voucher.VoucherStatus == "Posted")
                {
                    _Voucher.IsPosted = true;
                    _Voucher.PostedBy = SessionHelper.UserID;
                }
                else
                {
                    _Voucher.IsPosted = false;
                }

                if (_Voucher.VoucherStatus == "Cancelled")
                {
                    _Voucher.IsCancelled = true;
                    _Voucher.CancelledBy = SessionHelper.UserID;
                }
                else
                {
                    _Voucher.IsCancelled = false;
                }

                _Voucher.IsApproved = false;
                _Voucher.DebitAmount = 0;
                _Voucher.CreditAmount = 0;
                _Voucher.VoucherType = objVoucher.VoucherType;
                _Voucher.CurrencyId = objVoucher.CurrencyId;
                _Voucher.ExchangeRate = objVoucher.ExchangeRate;
                _Voucher.CBAccountId = objVoucher.CBAccountId;
                _Voucher.Particulars = objVoucher.Particulars;
                _Voucher.BranchId = branch_ID;
                _Voucher.FiscalYearId = SessionHelper.FiscalYearId;
                _Voucher.CreatedBy = SessionHelper.UserID;
                _Voucher.CreatedOn = DateTime.Now;
                db.Vouchers.Add(_Voucher);
                //db.SaveChanges();

                //  Debit/Credit Entry
                VoucherDetail _VoucherDetail = new VoucherDetail();
                _VoucherDetail.TransactionId = GetTransactionId(db, VoucherId);
                _VoucherDetail.VoucherId = VoucherId;
                if (GetTransactionType(objVoucher.VoucherType) == "Cr")
                {
                    _VoucherDetail.TransactionType = "Dr";
                }
                else
                {
                    _VoucherDetail.TransactionType = "Cr";
                }

                _VoucherDetail.AccountId = objVoucher.CBAccountId;
                _VoucherDetail.Debit = 0;
                _VoucherDetail.Credit = 0;
                _VoucherDetail.Narration = objVoucher.Particulars == null ? "" : objVoucher.Particulars;
                db.VoucherDetails.Add(_VoucherDetail);
                db.SaveChanges();
            }
            return VoucherId;
        }

        public static long VoucherDetail_Insert(OneDbContext db, Int64 VoucherId, string VoucherType, VoucherDetail objVoucherDetail)
        {
            long voucherDetailId = 0;
            if (!string.IsNullOrEmpty(objVoucherDetail.AccountId))
            {
                decimal amount = objVoucherDetail.Debit;
                VoucherDetail _VoucherDetail = objVoucherDetail;
                _VoucherDetail.VoucherId = VoucherId;
                _VoucherDetail.TransactionId = GetTransactionId(db, VoucherId);
                _VoucherDetail.TransactionType = GetTransactionType(VoucherType);
                _VoucherDetail.AccountId = objVoucherDetail.AccountId;
                if (_VoucherDetail.TransactionType == "Dr")
                {
                    _VoucherDetail.Debit = amount;
                    _VoucherDetail.Credit = 0;
                }
                else
                {
                    _VoucherDetail.Debit = 0;
                    _VoucherDetail.Credit = amount;
                }
                _VoucherDetail.ChequeNo = objVoucherDetail.ChequeNo;
                _VoucherDetail.ChequeDate = objVoucherDetail.ChequeDate;
                _VoucherDetail.ChequeClearDate = objVoucherDetail.ChequeClearDate;
                _VoucherDetail.Narration = objVoucherDetail.Narration == null ? "" : objVoucherDetail.Narration;
                _VoucherDetail.CostGroupId = objVoucherDetail.CostGroupId;
                db.VoucherDetails.Add(_VoucherDetail);
                db.SaveChanges();
                voucherDetailId = _VoucherDetail.VoucherDetailId;
            }
            return voucherDetailId;
        }

        public static bool VoucherDetail_Update(OneDbContext db, VoucherDetail objVoucherDetail)
        {
            bool res = false;
            if (objVoucherDetail.VoucherDetailId > 0)
            {
                VoucherDetail _VoucherDetail = db.VoucherDetails.Where(u => u.VoucherDetailId == objVoucherDetail.VoucherDetailId).FirstOrDefault();
                if (_VoucherDetail != null)
                {
                    _VoucherDetail.AccountId = objVoucherDetail.AccountId;
                    if (_VoucherDetail.TransactionType == "Dr")
                    {
                        _VoucherDetail.Debit = objVoucherDetail.Debit;
                        _VoucherDetail.Credit = 0;
                    }
                    else
                    {
                        _VoucherDetail.Debit = 0;
                        _VoucherDetail.Credit = objVoucherDetail.Credit;
                    }
                    _VoucherDetail.ChequeNo = objVoucherDetail.ChequeNo;
                    _VoucherDetail.ChequeDate = objVoucherDetail.ChequeDate;
                    _VoucherDetail.ChequeClearDate = objVoucherDetail.ChequeClearDate;
                    _VoucherDetail.Narration = objVoucherDetail.Narration == null ? "" : objVoucherDetail.Narration;
                    _VoucherDetail.CostGroupId = objVoucherDetail.CostGroupId;
                    db.Entry(_VoucherDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    res = true;
                }
            }
            return res;
        }

        public static bool Voucher_Update(OneDbContext db, string VoucherStatus, Voucher objVoucher)
        {
            bool res = false;
            if (string.IsNullOrEmpty(VoucherStatus))
            {
                VoucherStatus = "Draft";
            }

            if (objVoucher.VoucherId > 0)
            {
                Voucher _Voucher = db.Vouchers.Where(u => u.VoucherId == objVoucher.VoucherId).FirstOrDefault();
                if (_Voucher != null)
                {
                    _Voucher.CurrencyId = objVoucher.CurrencyId;
                    _Voucher.VoucherStatus = VoucherStatus;
                    if (_Voucher.VoucherStatus == "Posted")
                    {
                        _Voucher.IsPosted = true;
                        _Voucher.PostedOn = DateTime.Now;
                        _Voucher.PostedBy = SessionHelper.UserID;
                        _Voucher.IsCancelled = false;
                        _Voucher.CancelledBy = null;
                        _Voucher.CancelledOn = null;
                    }
                    if (_Voucher.VoucherStatus == "Unposted")
                    {
                        _Voucher.IsPosted = false;
                        _Voucher.PostedBy = null;
                        _Voucher.PostedOn = null;
                        _Voucher.VoucherStatus = "Draft";
                    }
                    if (_Voucher.VoucherStatus == "Cancelled")
                    {
                        _Voucher.IsCancelled = true;
                        _Voucher.CancelledOn = DateTime.Now;
                        _Voucher.CancelledBy = SessionHelper.UserID;
                        _Voucher.IsPosted = false;
                        _Voucher.PostedBy = null;
                        _Voucher.PostedOn = null;
                    }
                    if (_Voucher.VoucherStatus == "Uncancelled")
                    {
                        _Voucher.IsCancelled = false;
                        _Voucher.CancelledBy = null;
                        _Voucher.CancelledOn = null;
                        _Voucher.VoucherStatus = "Draft";
                    }
                    if (_Voucher.VoucherStatus == "Approved")
                    {
                        _Voucher.IsApproved = true;
                        _Voucher.ApprovedBy = SessionHelper.UserID;
                    }
                    if (_Voucher.VoucherStatus == "Unapproved")
                    {
                        _Voucher.IsApproved = false;
                        _Voucher.ApprovedBy = null;
                        _Voucher.VoucherStatus = "Draft";
                    }
                    _Voucher.ExchangeRate = objVoucher.ExchangeRate;
                    _Voucher.CBAccountId = objVoucher.CBAccountId;
                    _Voucher.Particulars = objVoucher.Particulars;
                    _Voucher.ModifiedBy = SessionHelper.UserID;
                    _Voucher.ModifiedOn = DateTime.Now;
                    db.Entry(_Voucher).State = EntityState.Modified;
                    db.SaveChanges();
                    res = true;

                    VoucherDetail _VoucherDetail = db.VoucherDetails.Where(u => u.VoucherId == objVoucher.VoucherId && u.TransactionId == 1).FirstOrDefault();
                    if (_VoucherDetail != null)
                    {
                        _VoucherDetail.AccountId = objVoucher.CBAccountId;
                        db.Entry(_VoucherDetail).State = EntityState.Modified;
                        db.SaveChanges();
                        res = true;
                    }
                }
            }
            return res;
        }

        static string GetTransactionType(string VoucherType)
        {
            var type = "Dr";
            string[] T1 = new string[] { "CP", "BP", "CPS", "BPS", "CPL", "BPL" };
            string[] T2 = new string[] { "CR", "BR", "CRF", "BRF", "SV", "CRI", "BRI", "CRM", "BRM" };
            if (T2.Contains(VoucherType))
            {
                type = "Cr";
            }

            return type;
        }

        static short GetTransactionId(OneDbContext db, Int64 VoucherId)
        {
            var TransactionId = db.VoucherDetails.Where(u => u.VoucherId == VoucherId).Max(u => (int?)u.TransactionId);
            TransactionId = Convert.ToInt16(TransactionId) + 1;
            return Convert.ToInt16(TransactionId);
        }
        #endregion

        #region Journal Voucher

        public static Int64 JV_Voucher_Insert(OneDbContext db, Voucher objVoucher)
        {
            Int64 VoucherId = GetVoucherId(db, branch_ID, objVoucher.VoucherType, 0, objVoucher.TransactionDate);
            if (VoucherId > 0)
            {
                Voucher _Voucher = new Voucher();
                _Voucher.VoucherId = VoucherId;
                _Voucher.VoucherStatus = "Draft";
                _Voucher.TransactionDate = objVoucher.TransactionDate;
                if (_Voucher.VoucherStatus == "Posted")
                {
                    _Voucher.IsPosted = true;
                    _Voucher.PostedBy = SessionHelper.UserID;
                }
                else
                {
                    _Voucher.IsPosted = false;
                }

                if (_Voucher.VoucherStatus == "Cancelled")
                {
                    _Voucher.IsCancelled = true;
                    _Voucher.CancelledBy = SessionHelper.UserID;
                }
                else
                {
                    _Voucher.IsCancelled = false;
                }

                _Voucher.IsApproved = false;
                _Voucher.DebitAmount = 0;
                _Voucher.CreditAmount = 0;
                _Voucher.VoucherType = objVoucher.VoucherType;
                _Voucher.CurrencyId = objVoucher.CurrencyId;
                _Voucher.ExchangeRate = objVoucher.ExchangeRate;
                _Voucher.CBAccountId = objVoucher.CBAccountId;
                _Voucher.Particulars = objVoucher.Particulars;
                _Voucher.BranchId = branch_ID;
                _Voucher.FiscalYearId = SessionHelper.FiscalYearId;
                _Voucher.CreatedBy = SessionHelper.UserID;
                _Voucher.CreatedOn = DateTime.Now;
                db.Vouchers.Add(_Voucher);
                db.SaveChanges();
            }
            return VoucherId;
        }

        public static bool JV_Voucher_Update(OneDbContext db, string VoucherStatus, Voucher objVoucher)
        {
            bool res = false;
            if (string.IsNullOrEmpty(VoucherStatus))
            {
                VoucherStatus = "Draft";
            }

            if (objVoucher.VoucherId > 0)
            {
                Voucher _Voucher = db.Vouchers.Where(u => u.VoucherId == objVoucher.VoucherId).FirstOrDefault();
                if (_Voucher != null)
                {
                    _Voucher.CurrencyId = objVoucher.CurrencyId;
                    _Voucher.VoucherStatus = VoucherStatus;
                    if (_Voucher.VoucherStatus == "Posted")
                    {
                        _Voucher.IsPosted = true;
                        _Voucher.PostedOn = DateTime.Now;
                        _Voucher.PostedBy = SessionHelper.UserID;
                        _Voucher.IsCancelled = false;
                        _Voucher.CancelledBy = null;
                        _Voucher.CancelledOn = null;
                    }
                    if (_Voucher.VoucherStatus == "Unposted")
                    {
                        _Voucher.IsPosted = false;
                        _Voucher.PostedBy = null;
                        _Voucher.PostedOn = null;
                        _Voucher.VoucherStatus = "Draft";
                    }
                    if (_Voucher.VoucherStatus == "Cancelled")
                    {
                        _Voucher.IsCancelled = true;
                        _Voucher.CancelledOn = DateTime.Now;
                        _Voucher.CancelledBy = SessionHelper.UserID;
                        _Voucher.IsPosted = false;
                        _Voucher.PostedBy = null;
                        _Voucher.PostedOn = null;
                    }
                    if (_Voucher.VoucherStatus == "Uncancelled")
                    {
                        _Voucher.IsCancelled = false;
                        _Voucher.CancelledBy = null;
                        _Voucher.CancelledOn = null;
                        _Voucher.VoucherStatus = "Draft";
                    }
                    if (_Voucher.VoucherStatus == "Approved")
                    {
                        _Voucher.IsApproved = true;
                        _Voucher.ApprovedBy = SessionHelper.UserID;
                    }
                    if (_Voucher.VoucherStatus == "Unapproved")
                    {
                        _Voucher.IsApproved = false;
                        _Voucher.ApprovedBy = null;
                        _Voucher.VoucherStatus = "Draft";
                    }
                    _Voucher.ExchangeRate = objVoucher.ExchangeRate;
                    _Voucher.CBAccountId = objVoucher.CBAccountId;
                    _Voucher.Particulars = objVoucher.Particulars;
                    _Voucher.ModifiedBy = SessionHelper.UserID;
                    _Voucher.ModifiedOn = DateTime.Now;
                    db.Entry(_Voucher).State = EntityState.Modified;
                    db.SaveChanges();
                    res = true;
                }
            }
            return res;
        }

        public static Int64 JV_VoucherDetail_Insert(OneDbContext db, Int64 VoucherId, string VoucherType, VoucherDetail objVoucherDetail)
        {
            if (!string.IsNullOrEmpty(objVoucherDetail.AccountId))
            {
                decimal amount = objVoucherDetail.Debit;
                VoucherDetail _VoucherDetail = objVoucherDetail;
                _VoucherDetail.VoucherId = VoucherId;
                _VoucherDetail.TransactionId = GetTransactionId(db, VoucherId);
                _VoucherDetail.TransactionType = GetTransactionType(VoucherType);
                _VoucherDetail.AccountId = objVoucherDetail.AccountId;
                if (objVoucherDetail.Debit > 0)
                {
                    _VoucherDetail.TransactionType = "Dr";
                    _VoucherDetail.Debit = objVoucherDetail.Debit;
                    _VoucherDetail.Credit = 0;
                }
                else
                {
                    _VoucherDetail.TransactionType = "Dr";
                    _VoucherDetail.Credit = objVoucherDetail.Credit;
                    _VoucherDetail.Debit = 0;
                }
                _VoucherDetail.ChequeNo = objVoucherDetail.ChequeNo;
                _VoucherDetail.ChequeDate = objVoucherDetail.ChequeDate;
                _VoucherDetail.ChequeClearDate = objVoucherDetail.ChequeClearDate;
                _VoucherDetail.Narration = objVoucherDetail.Narration == null ? "" : objVoucherDetail.Narration;
                _VoucherDetail.CostGroupId = objVoucherDetail.CostGroupId;
                db.VoucherDetails.Add(_VoucherDetail);
                db.SaveChanges();
            }
            return VoucherId;
        }

        public static bool JV_VoucherDetail_Update(OneDbContext db, VoucherDetail objVoucherDetail)
        {
            bool res = false;
            if (objVoucherDetail.VoucherDetailId > 0)
            {
                VoucherDetail _VoucherDetail = db.VoucherDetails.Where(u => u.VoucherDetailId == objVoucherDetail.VoucherDetailId).FirstOrDefault();
                if (_VoucherDetail != null)
                {
                    _VoucherDetail.AccountId = objVoucherDetail.AccountId;
                    if (objVoucherDetail.Debit > 0)
                    {
                        _VoucherDetail.TransactionType = "Dr";
                        _VoucherDetail.Debit = objVoucherDetail.Debit;
                        _VoucherDetail.Credit = 0;
                    }
                    else
                    {
                        _VoucherDetail.TransactionType = "Dr";
                        _VoucherDetail.Credit = objVoucherDetail.Credit;
                        _VoucherDetail.Debit = 0;
                    }
                    _VoucherDetail.ChequeNo = objVoucherDetail.ChequeNo;
                    _VoucherDetail.ChequeDate = objVoucherDetail.ChequeDate;
                    _VoucherDetail.ChequeClearDate = objVoucherDetail.ChequeClearDate;
                    _VoucherDetail.Narration = objVoucherDetail.Narration == null ? "" : objVoucherDetail.Narration;
                    _VoucherDetail.CostGroupId = objVoucherDetail.CostGroupId;
                    db.Entry(_VoucherDetail).State = EntityState.Modified;
                    db.SaveChanges();
                    res = true;
                }
            }
            return res;
        }



        public static string p_DeleteVoucherDetailsExceptfirst(OneDbContext db, long? VoucherId)
        {
            var result = db.Database.ExecuteSqlCommand($@"--billing
update Finance.BillItems set VoucherDetailId =null 
where VoucherDetailId IN(select  VoucherDetailId from Finance.VoucherDetails where VoucherId = @VoucherId)
update Finance.SupplierInvoicePayments set VoucherDetailId =null 
where VoucherDetailId IN(select  VoucherDetailId from Finance.VoucherDetails where VoucherId = @VoucherId)
--pos
update POS.PurchaseInvoiceProducts set VoucherDetailId =null 
where VoucherDetailId IN(select  VoucherDetailId from Finance.VoucherDetails where VoucherId = @VoucherId)
--inv
update INV.ClearingBillItems set VoucherDetailId =null 
where VoucherDetailId IN(select  VoucherDetailId from Finance.VoucherDetails where VoucherId =@VoucherId)

update INV.FreightBillItems set VoucherDetailId =null 
where VoucherDetailId IN(select  VoucherDetailId from Finance.VoucherDetails where VoucherId =@VoucherId)

delete from Finance.VoucherDetails 
where VoucherId=@VoucherId and 
VoucherDetailId<>(select top 1 VoucherDetailId from Finance.VoucherDetails where VoucherId = @VoucherId order by voucherdetailid asc)",
                new SqlParameter("@VoucherId", GetDBNullOrValue(VoucherId))).ToString();
            return result;
        }
        #endregion

        #region COA

        public static string CreateCategorySingleAccount(OneDbContext db, string NewAccTITLE, string autokey, bool IsControlAcc)
        {
            var _parentAccount = db.Accounts.Where(u => u.autokey == autokey).FirstOrDefault();
            if (_parentAccount != null)
            {
                Account _Account = new Account();
                long? newAcc = GetNewAccoundID(db, _parentAccount.autokey);
                if (newAcc != null)
                {
                    Guid gu = Guid.NewGuid();
                    _Account.ParentAccountId = _parentAccount.autokey;
                    _Account.ParentId = _parentAccount.ACCOUNT_ID;
                    _Account.BranchId = SessionHelper.BranchId;
                    _Account.autokey = gu.ToString();
                    _Account.ACCOUNT_ID = Convert.ToInt64(newAcc);
                    _Account.TITLE = NewAccTITLE;
                    _Account.DEPTH = GetAccountDepth(db, _parentAccount, _parentAccount.autokey);
                    _Account.LINEAGE = GetAccountLineage(db, _parentAccount, _parentAccount.autokey);
                    _Account.CONTROLACCOUNT = IsControlAcc;
                    _Account.ISTRANSACTION = !IsControlAcc;
                    _Account.GroupNo = 1;
                    db.Accounts.Add(_Account);
                    db.SaveChanges();
                    return _Account.autokey;
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public static string CreateNewAccountByParentAccountCode(OneDbContext db, string NewAccTITLE, string parAccTitle, bool IsControl)
        {
            var _parentAccount = db.Accounts.Where(u => u.autokey == parAccTitle).FirstOrDefault();
            if (_parentAccount != null)
            {
                Account _Account = new Account();
                long? newAcc = GetNewAccoundID(db, _parentAccount.autokey);
                if (newAcc != null)
                {
                    Guid gu = Guid.NewGuid();
                    _Account.ParentAccountId = _parentAccount.autokey;
                    _Account.ParentId = _parentAccount.ACCOUNT_ID;
                    _Account.BranchId = SessionHelper.BranchId;
                    _Account.autokey = gu.ToString();
                    _Account.ACCOUNT_ID = Convert.ToInt64(newAcc);
                    _Account.TITLE = NewAccTITLE;
                    _Account.DEPTH = GetAccountDepth(db, _parentAccount, _parentAccount.autokey);
                    _Account.LINEAGE = GetAccountLineage(db, _parentAccount, _parentAccount.autokey);
                    if (IsControl)
                    {
                        _Account.CONTROLACCOUNT = true;
                        _Account.ISTRANSACTION = false;
                    }
                    else
                    {
                        _Account.CONTROLACCOUNT = false;
                        _Account.ISTRANSACTION = true;
                    }
                    _Account.GroupNo = 1;
                    db.Accounts.Add(_Account);
                    //db.SaveChanges();
                    return _Account.autokey;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public static string[] CreateCategoryNewAccounts(OneDbContext db, string NewAccTITLE, string parAccTitle)
        {
            string[] acc = new string[2];
            //var _parentAccount = db.Accounts.Where(u => (u.TITLE == null ? "" : u.TITLE.ToUpper()) == parAccTitle.ToUpper() && u.CONTROLACCOUNT == true).FirstOrDefault();
            var _parentAccount = db.Accounts.Where(u => u.autokey == parAccTitle).FirstOrDefault();
            if (_parentAccount != null)
            {
                Account _Account = new Account();
                long? newAcc = GetNewAccoundID(db, _parentAccount.autokey);
                if (newAcc != null)
                {
                    Guid gu = Guid.NewGuid();
                    _Account.ParentAccountId = _parentAccount.autokey;
                    _Account.ParentId = _parentAccount.ACCOUNT_ID;
                    _Account.BranchId = SessionHelper.BranchId;
                    _Account.autokey = gu.ToString();
                    _Account.ACCOUNT_ID = Convert.ToInt64(newAcc);
                    _Account.TITLE = NewAccTITLE;
                    _Account.DEPTH = GetAccountDepth(db, _parentAccount, _parentAccount.autokey);
                    _Account.LINEAGE = GetAccountLineage(db, _parentAccount, _parentAccount.autokey);
                    _Account.CONTROLACCOUNT = true;
                    _Account.ISTRANSACTION = false;
                    _Account.GroupNo = 1;
                    db.Accounts.Add(_Account);
                    db.SaveChanges();
                    acc[0] = _Account.autokey;

                    //Transaction account
                    newAcc = null;
                    Account _TAccount = new Account();
                    newAcc = GetNewAccoundID(db, _Account.autokey);
                    if (newAcc != null)
                    {
                        gu = Guid.NewGuid();
                        _TAccount.ParentAccountId = _Account.autokey;
                        _TAccount.ParentId = _Account.ACCOUNT_ID;
                        _TAccount.BranchId = SessionHelper.BranchId;
                        _TAccount.autokey = gu.ToString();
                        _TAccount.ACCOUNT_ID = Convert.ToInt64(newAcc);
                        _TAccount.TITLE = NewAccTITLE;
                        _TAccount.DEPTH = GetAccountDepth(db, _Account, _Account.autokey);
                        _TAccount.LINEAGE = GetAccountLineage(db, _Account, _Account.autokey);
                        _TAccount.CONTROLACCOUNT = false;
                        _TAccount.ISTRANSACTION = true;
                        _TAccount.GroupNo = 1;
                        db.Accounts.Add(_TAccount);
                        acc[1] = _TAccount.autokey;

                    }
                    return acc;
                }
                return null;
            }
            else
            {
                return null;
            }
        }
        public static string p_mnl_UnPostAMPurchaseInvoicesToAccount(OneDbContext db, int? UserId, string InvoiceIds)
        {
            var result = db.Database.ExecuteSqlCommand("update Finance.Vouchers  set IsPosted=0,VoucherStatus='Draft', PostedBy=null, PostedOn=null, ModifiedBy=@ModifiedBy, Modifiedon=@Modifiedon where VoucherId in(select VoucherId from AM.PurchaseInvoices where PurchaseInvoiceId in (" + InvoiceIds + ") and IsCancelled = 0 and IsPosted = 1) update AM.PurchaseInvoices set IsAccountPosted = 0, AccountPostedBy = null, AccountPostedOn = null, ModifiedBy = @ModifiedBy, Modifiedon = @Modifiedon where PurchaseInvoiceId in(" + InvoiceIds + ") and IsCancelled = 0 and IsPosted = 1",
                new SqlParameter("@ModifiedBy", GetDBNullOrValue(UserId)),
                new SqlParameter("@Modifiedon", GetDBNullOrValue(DateTime.Now))
                ).ToString();
            return result;
        }
        #endregion

        #region Products
        public static IQueryable<v_mnl_AMIssueItemCode_Result> v_mnl_AMIssueItemCodeList(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_AMIssueItemCode_Result>($@"SELECT        ii.IssuedItemId, pipd.DetailId, pipd.ItemCode, iid.Quantity, item.ItemId, item.IsConsumable
FROM            AM.IssuedItems AS ii INNER JOIN
						 AM.IssuedItemDetails AS iid ON ii.IssuedItemId = iid.IssuedItemId INNER JOIN
						 AM.PurchaseInvoiceProductDetails AS pipd ON iid.PIPDetailId = pipd.DetailId INNER JOIN
						 AM.Items AS item ON pipd.ItemId = item.ItemId
						 where ii.BranchId={SessionHelper.BranchId} and ItemCode not in(SELECT        pipd.ItemCode
FROM            AM.ReturnIssueDetails AS iidnew INNER JOIN
						 AM.PurchaseInvoiceProductDetails AS pipd ON iidnew.PIPDetailId = pipd.DetailId INNER JOIN
						 AM.ReturnIssue as iinew ON iidnew.ReturnIssueId = iinew.ReturnIssueId and iidnew.BranchId = iinew.BranchId
						 where iidnew.BranchId= ii.BranchId)").AsQueryable();
            return result;
        }

        public static IQueryable<v_mnl_AMIssueItemCode_Result> v_mnl_AMIssueItemExistCodeList(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_AMIssueItemCode_Result>($@"SELECT        ii.IssuedItemId, pipd.DetailId, pipd.ItemCode, iid.Quantity, item.ItemId, item.IsConsumable
FROM            AM.IssuedItems AS ii INNER JOIN
						 AM.IssuedItemDetails AS iid ON ii.IssuedItemId = iid.IssuedItemId INNER JOIN
						 AM.PurchaseInvoiceProductDetails AS pipd ON iid.PIPDetailId = pipd.DetailId INNER JOIN
						 AM.Items AS item ON pipd.ItemId = item.ItemId
						 where ii.BranchId={SessionHelper.BranchId}").AsQueryable();
            return result;
        }
        public static void InsertInvoiceProductDetails(OneDbContext db, long purchaseInvoiceProductId, int itemId, decimal quantity, bool IsConsumable)
        {
            var item = db.AMItems.Find(itemId);
            var category = db.AMCategories.Find(item.CategoryId);
            string newName = category.ShortName + "-" + item.ShortName + "-";
            //var previousEntries = db.AMPurchaseInvoiceProductDetails.Where(u => u.PurchaseInvoiceProductId == purchaseInvoiceProductId && u.ItemId == itemId).ToList();
            //db.AMPurchaseInvoiceProductDetails.RemoveRange(previousEntries);
            //db.SaveChanges();
            var temp = db.AMPurchaseInvoiceProductDetails.Where(s => s.ItemCode.Contains(newName)).Select(s => s.ItemCode).Max();
            if (IsConsumable)
            {
                var entity = new AMPurchaseInvoiceProductDetail();
                entity.PurchaseInvoiceProductId = purchaseInvoiceProductId;
                if (string.IsNullOrEmpty(temp))
                {
                    entity.ItemCode = newName + String.Format("{0:D5}", 1);
                    entity.ItemId = item.ItemId;
                    entity.Qty = (int)quantity;
                }
                else
                {
                    Regex re = new Regex(@"\d+");
                    Match result = re.Match(temp);
                    int numaricPart = Convert.ToInt32(result.Value);
                    entity.ItemCode = newName + String.Format("{0:D5}", numaricPart + 1);
                    entity.ItemId = item.ItemId;
                    entity.Qty = (int)quantity;
                }
                db.AMPurchaseInvoiceProductDetails.Add(entity);
            }
            else
            {
                for (int i = 0; i < quantity; i++)
                {
                    var entity = new AMPurchaseInvoiceProductDetail();
                    entity.PurchaseInvoiceProductId = purchaseInvoiceProductId;
                    if (string.IsNullOrEmpty(temp))
                    {
                        entity.ItemCode = newName + String.Format("{0:D5}", i + 1);
                        entity.ItemId = item.ItemId;
                        entity.Qty = 1;
                    }
                    else
                    {
                        Regex re = new Regex(@"\d+");
                        Match result = re.Match(temp);
                        int numaricPart = Convert.ToInt32(result.Value);
                        entity.ItemCode = newName + String.Format("{0:D5}", numaricPart + i + 1);
                        entity.ItemId = item.ItemId;
                        entity.Qty = 1;
                    }
                    db.AMPurchaseInvoiceProductDetails.Add(entity);
                }
            }
            db.SaveChanges();
        }
        public static Int32 GetNewProductCode_AM(OneDbContext db, int CategoryId)
        {
            var result = db.Database.SqlQuery<Int32>($@"Declare @CategoryCode varchar(10),@ProductCode varchar(10)
select @CategoryCode = Cast(CategoryCode as varchar(10)) from AM.Categories where CategoryId = @CategoryId
														 select @ProductCode = Max(ProductCode) from AM.Items where CategoryId = @CategoryId and ProductCode like @CategoryCode + '%'
if (@ProductCode is null)
Begin
 set @ProductCode = RIGHT(CAST(@CategoryCode AS VARCHAR(20)) + '000', 6)
end

select cast(@ProductCode as int) + 1",
                         new SqlParameter("@CategoryId", GetDBNullOrValue(CategoryId))
                         ).FirstOrDefault();
            return result;
        }

        public static int p_mnl_GetNextCategoryCode_AM(OneDbContext db)
        {
            var _maxCategoryCode = db.AMCategories.Max(u => u.CategoryCode);
            if (_maxCategoryCode.HasValue)
            {
                _maxCategoryCode++;
            }
            else
            {
                _maxCategoryCode = 11;
            }

            return (int)_maxCategoryCode;
        }

        public static string ResetProductCode_AM(OneDbContext db, string CategoryId)
        {
            var result = db.Database.ExecuteSqlCommand($@"update AM.Items set ProductCode='' where CategoryId IN ({CategoryId})").ToString();
            return result;
        }
        public static string ResetCategoryCode_AM(OneDbContext db, string CategoryId)
        {
            var result = db.Database.ExecuteSqlCommand($@"update AM.Categories set CategoryCode=null where BranchId={SessionHelper.BranchId} and CategoryId IN ({CategoryId})").ToString();
            return result;
        }

        public static int p_mnl_GetAndSetCategoryCode_AM(OneDbContext db, int CategoryId)
        {
            int? CategoryCode = db.AMCategories.Where(u => u.CategoryId == CategoryId).Select(u => u.CategoryCode).FirstOrDefault();
            if (!CategoryCode.HasValue)
            {
                CategoryCode = p_mnl_GetNextCategoryCode_AM(db);
                var _PosCategories = db.AMCategories.Where(u => u.CategoryId == CategoryId).FirstOrDefault();
                if (_PosCategories != null)
                {
                    _PosCategories.CategoryCode = CategoryCode;
                    db.SaveChanges();
                }
            }
            return (int)CategoryCode;
        }

        public static void InsertPurchaseInvoiceProductDetails(OneDbContext db, string PurchaseInvoiceIds)
        {
            long? pid = null;
            foreach (var items in PurchaseInvoiceIds.Split(','))
            {
                pid = Convert.ToInt64(items);
                if (pid != -1)
                {
                    var purchaseInvoice = db.InvPurchaseInvoices.Where(u => u.PurchaseInvoiceId == pid && u.BranchId == SessionHelper.BranchId).Select(u => u.PurchaseInvoiceId).FirstOrDefault();
                    if (purchaseInvoice > 0)
                    {
                        var det = db.AMPurchaseInvoiceProducts.Where(u => u.PurchaseInvoiceId == purchaseInvoice).ToList();
                        if (det.Count > 0)
                        {
                            foreach (var detitem in det)
                            {
                                var item = db.AMItems.Find(detitem.ItemId);
                                var category = db.AMCategories.Find(item.CategoryId);
                                string newName = category.ShortName + "-" + item.ShortName + "-";
                                var temp = db.AMPurchaseInvoiceProductDetails.Where(s => s.ItemCode.Contains(newName)).Select(s => s.ItemCode).Max();
                                if (item.IsConsumable)
                                {
                                    var entity = new AMPurchaseInvoiceProductDetail();
                                    entity.PurchaseInvoiceProductId = detitem.PurchaseInvoiceProductId;
                                    if (string.IsNullOrEmpty(temp))
                                    {
                                        entity.ItemCode = newName + String.Format("{0:D5}", 1);
                                        entity.ItemId = item.ItemId;
                                        entity.Qty = (int)detitem.Quantity;
                                    }
                                    else
                                    {
                                        Regex re = new Regex(@"\d+");
                                        Match result = re.Match(temp);
                                        int numaricPart = Convert.ToInt32(result.Value);
                                        entity.ItemCode = newName + String.Format("{0:D5}", numaricPart + 1);
                                        entity.ItemId = item.ItemId;
                                        entity.Qty = (int)detitem.Quantity;
                                    }
                                    db.AMPurchaseInvoiceProductDetails.Add(entity);
                                }
                                else
                                {
                                    for (int i = 0; i < detitem.Quantity; i++)
                                    {
                                        var entity = new AMPurchaseInvoiceProductDetail();
                                        entity.PurchaseInvoiceProductId = detitem.PurchaseInvoiceProductId;
                                        if (string.IsNullOrEmpty(temp))
                                        {
                                            entity.ItemCode = newName + String.Format("{0:D5}", i + 1);
                                            entity.ItemId = item.ItemId;
                                            entity.Qty = 1;
                                        }
                                        else
                                        {
                                            Regex re = new Regex(@"\d+");
                                            Match result = re.Match(temp);
                                            int numaricPart = Convert.ToInt32(result.Value);
                                            entity.ItemCode = newName + String.Format("{0:D5}", numaricPart + i + 1);
                                            entity.ItemId = item.ItemId;
                                            entity.Qty = 1;
                                        }
                                        db.AMPurchaseInvoiceProductDetails.Add(entity);
                                    }
                                }

                                try
                                {
                                    db.SaveChanges();
                                }
                                catch (DbEntityValidationException)
                                {

                                }
                            }
                        }
                    }
                }
            }
        }

        public static void DeletePurchaseInvoiceProductDetails(OneDbContext db, string PurchaseInvoiceIds)
        {
            long? pid = null;
            foreach (var items in PurchaseInvoiceIds.Split(','))
            {
                pid = Convert.ToInt64(items);
                var purchaseInvoice = db.AMPurchaseInvoices.Where(u => u.PurchaseInvoiceId == pid && u.BranchId == SessionHelper.BranchId).Select(u => u.PurchaseInvoiceId).ToArray();
                if (purchaseInvoice != null)
                {
                    var purchaseInvoiceProduct = db.AMPurchaseInvoiceProducts.Where(u => purchaseInvoice.Contains(u.PurchaseInvoiceId)).Select(u => (long?)u.PurchaseInvoiceProductId).ToArray();
                    if (purchaseInvoiceProduct != null)
                    {
                        var purchaseInvoiceProductDetail = db.AMPurchaseInvoiceProductDetails.Where(u => purchaseInvoiceProduct.Contains(u.PurchaseInvoiceProductId)).ToList();
                        if (purchaseInvoiceProductDetail != null)
                        {
                            db.AMPurchaseInvoiceProductDetails.RemoveRange(purchaseInvoiceProductDetail);
                        }
                    }
                }
            }
            db.SaveChanges();
        }

        #endregion


        #region Purchase Invoice 
        public static List<v_mnl_PurchaseInvoices_Result> v_mnl_AMPurchaseInvoices(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_PurchaseInvoices_Result>($@"
                                                SELECT pu.PurchaseInvoiceId
                                    	,pu.PurchaseOrderId
                                    	,pu.PurchaseInvoiceDate
                                    	,pu.SupplierId
                                    	,pu.Discount
                                    	,pu.Description
                                    	,pu.IsPosted
                                    	,pu.PostedOn
                                    	,pu.PostedBy
                                    	,pu.IsCancelled
                                    	,pu.CancelledOn
                                    	,pu.CancelledBy
                                    	,pu.CreatedOn
                                    	,pu.CreatedBy
                                    	,pu.ModifiedOn
                                    	,pu.ModifiedBy
                                    	,pu.CurrencyId
                                    	,pu.ExchangeRate
                                    	,pu.MovedToWarehouse
                                    	,pu.VoucherId
                                    	,pu.TotalAmount
                                    	,pu.LabourCharges
                                    	,pu.OtherCharges
                                    	,pu.FareCharges
                                    	,ISNULL(pu.NetTotal, 0) AS NetTotal
                                    	,pu.BranchId
                                    	,pu.Version
                                    	,pu.IsApplyTax
                                    	,ISNULL(pu.Received, 0) AS ReceivedAmount
                                    	,Membership.Users.Username AS PostedName
                                    	,User_2.Username AS CancelledName
                                    	,User_1.Username AS ModifiedName
                                    	,User_3.Username AS CreatedName
                                    	,cl.Name AS ClientName
                                    	,CONVERT(DECIMAL(18, 2), 0) AS Paid
                                    	,pu.IsAccountPosted
                                    	,pu.AccountPostedOn
                                    	,pu.AccountPostedBy
                                    	,User_5.Username AS AccountPostedName
                                    	--,pu.IsCreatedFromOpenningStock
                                    	,(
                                    		SELECT TOP 1 WareHouseId
                                    		FROM Inv.PurchaseInvoiceProducts AS det
                                    		WHERE det.PurchaseInvoiceId = pu.PurchaseInvoiceId
                                    		) AS WareHouseId
                                    FROM Inv.PurchaseInvoices AS pu
                                    LEFT OUTER JOIN Membership.Users AS User_5 ON pu.AccountPostedBy = User_5.UserID
                                    LEFT OUTER JOIN Membership.Users AS User_3 ON pu.CreatedBy = User_3.UserID
                                    LEFT OUTER JOIN Membership.Users AS User_2 ON pu.CancelledBy = User_2.UserID
                                    LEFT OUTER JOIN Membership.Users AS User_1 ON pu.ModifiedBy = User_1.UserID
                                    LEFT OUTER JOIN Membership.Users ON pu.PostedBy = Membership.Users.UserID
                                    LEFT OUTER JOIN Client.Clients AS cl ON pu.SupplierId = cl.ClientId
                                    --WHERE pu.IsCreatedFromOpenningStock = 0").ToList();
            return result;
        }

        #region Post / Unpost purchase
        public static string p_mnl_PostUnPostAMAssetOpening(OneDbContext db, string Command, int? UserId, string InvoiceIds, DateTime? Date)
        {
            var result = db.Database.ExecuteSqlCommand($@"if (@Command = 'Post') 
BEGIN 	
	Insert into AM.WarehouseProducts(WarehouseId, ItemId, Quantity, TransferDate, TransferType, StockPrice, StockValue, 
	TransferMethod, InvoiceId,BranchId,CreatedOn) 
	SELECT        
    sid.WareHouseId, sid.ItemId, sid.Quantity, si.PurchaseInvoiceDate, 'In' AS Expr2, sid.UnitPrice, p.Price, 
	'Asset Stock' AS Expr3, sid.PurchaseInvoiceId,{SessionHelper.BranchId},GetDate() 
	FROM AM.PurchaseInvoiceProducts AS sid 
	INNER JOIN AM.Items AS p ON sid.ItemId = p.ItemId 
	INNER JOIN AM.PurchaseInvoices AS si ON sid.PurchaseInvoiceId = si.PurchaseInvoiceId 
	WHERE(si.PurchaseInvoiceId IN({InvoiceIds}) and si.IsCancelled = 0 and si.IsPosted = 0) 
	update AM.PurchaseInvoices  set IsPosted=1, PostedBy=@UserId, PostedOn=@Date where PurchaseInvoiceId in({InvoiceIds}) 
	and IsCancelled=0 and IsPosted=0  and IsAccountPosted=0  
END 
if (@Command = 'Unpost') 
BEGIN 
	delete wp from AM.WarehouseProducts as wp inner join AM.PurchaseInvoices as pinv on pinv.PurchaseInvoiceId= InvoiceId 
	where TransferMethod = 'Asset Stock' and pinv.PurchaseInvoiceId in({InvoiceIds}) and IsCancelled = 0 and IsPosted = 1  
	update AM.PurchaseInvoices  set IsPosted=0, PostedBy=null, PostedOn=null where PurchaseInvoiceId in({InvoiceIds}) 
	and IsCancelled=0 and IsPosted=1  and IsAccountPosted=0   	
END",
                new SqlParameter("@Command", GetDBNullOrValue(Command)),
                new SqlParameter("@UserId", GetDBNullOrValue(UserId)),
                new SqlParameter("@Date", GetDBNullOrValue(Date))
                ).ToString();
            return result;
        }
        #endregion

        public static string DeleteInvoiceProductDetails(OneDbContext db, long PurchaseInvoiceId)
        {
            //var previousEntries = db.AMPurchaseInvoiceProductDetails.Where(u => u.PurchaseInvoiceProductId == purchaseInvoiceProductId && u.ItemId == itemId).ToList();
            //db.AMPurchaseInvoiceProductDetails.RemoveRange(previousEntries);
            //db.SaveChanges();
            var result = db.Database.ExecuteSqlCommand($@"delete from AM.PurchaseInvoiceProductDetails where PurchaseInvoiceProductId in(select PurchaseInvoiceProductId from AM.PurchaseInvoiceProducts where PurchaseInvoiceId={PurchaseInvoiceId})").ToString();
            return result;
        }
        public static IQueryable<v_mnl_AMItemPosition_Result> v_mnl_AMItemPositionList(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_AMItemPosition_Result>($@"
select 
PurchaseInvoiceProductDetailId, ReturnIssueId as InvoiceId, BranchId, ReturnIssueDate as InvoiceDate, 
'' as EmpName, null as DepartmentId, '' as DepartmentName, '' as CategoryName,ItemName,IsConsumable,ItemCode,
Qty, null as RoomId,'' as Location, 'Returned' AS Method,
null  as EmployeeId, ItemId,PurchaseInvoiceProductDetailId AS ItemCodeDetailId,
ConditionTypeId,ConditionName,
ReturnIssueDetailId 
	from (
		SELECT pi.ReturnIssueId, pi.BranchId, pi.ReturnIssueDate, pip.ReturnIssueDetailId,
		pipd.DetailId as PurchaseInvoiceProductDetailId,pipd.ItemId, item.ItemName,item.IsConsumable,pipd.ItemCode,
		pip.Quantity as Qty, con.ConditionTypeId,con.Name AS ConditionName
		FROM            
		AM.ReturnIssue AS pi 
		INNER JOIN AM.ReturnIssueDetails AS pip ON pi.ReturnIssueId = pip.ReturnIssueId 
		INNER JOIN AM.PurchaseInvoiceProductDetails AS pipd ON pip.PIPDetailId = pipd.DetailId 
		INNER JOIN AM.Items AS item ON pipd.ItemId = item.ItemId 
		LEFT OUTER JOIN AM.ConditionTypes AS con ON pip.ConditionTypeId = con.ConditionTypeId
		WHERE        
		(pi.BranchId={SessionHelper.BranchId}) and
		pi.IsPosted=0
) as tt

UNION

select * from 
(
	select
	PurchaseInvoiceProductDetailId,InvoiceId, BranchId, InvoiceDate, 
	EmpName, DepartmentId,DepartmentName, CategoryName, ItemName,IsConsumable, ItemCode,
	ABS((Qty-ConsumedQuantity) - ReturnedQty) as Qty, RoomId,Location,case when ConsumedQuantity>0 then 'InStock' else Method end Method, EmployeeId,
	ItemId, ItemCodeDetailId, ConditionTypeId, ConditionName,PurchaseInvoiceProductId 
	from (
	SELECT  
	pipd.DetailId as PurchaseInvoiceProductDetailId,cast(ii.IssuedItemId as bigint) as InvoiceId, ii.BranchId, ii.IssueDate as InvoiceDate,emp.EmpName, dep.DepartmentId,dep.DepartmentName, cat.CategoryName, item.ItemName,item.IsConsumable, pipd.ItemCode, pipd.Qty,(case when item.IsConsumable=1 then iid.Quantity else 0 end)ConsumedQuantity,room.RoomId,CAST(building.BuildingName + '>' + floor.FloorName + '>' + room.RoomDoorNo AS varchar) AS Location, 'Issued' AS Method, emp.EmployeeId,
	item.ItemId, pipd.DetailId AS ItemCodeDetailId, con.ConditionTypeId, con.Name as ConditionName,
	pipd.PurchaseInvoiceProductId
	,ISNULL((SELECT  SUM(ipipd.Qty)
				FROM  
				AM.ReturnIssue AS ii 
				INNER JOIN AM.ReturnIssueDetails AS iid ON ii.ReturnIssueId = iid.ReturnIssueId 
				INNER JOIN AM.PurchaseInvoiceProductDetails AS ipipd ON iid.PIPDetailId = ipipd.DetailId
				where 
				pipd.DetailId = ipipd.DetailId and
				pipd.ItemId = ipipd.ItemId and
				ii.BranchId=ii.BranchId and
				ii.IsPosted=1),0) as ReturnedQty
	FROM  AM.IssuedItems AS ii 
	INNER JOIN AM.IssuedItemDetails AS iid ON ii.IssuedItemId = iid.IssuedItemId 
	LEFT OUTER JOIN HR.Departments AS dep ON ii.DepartmentId = dep.DepartmentId 
	LEFT OUTER JOIN AM.ConditionTypes AS con ON iid.ConditionTypeId = con.ConditionTypeId 
	LEFT OUTER JOIN AM.Categories AS cat 
	INNER JOIN AM.Items AS item ON cat.CategoryId = item.CategoryId 
	LEFT OUTER JOIN AM.PurchaseInvoiceProductDetails AS pipd ON item.ItemId = pipd.ItemId ON iid.PIPDetailId = pipd.DetailId AND iid.PIPDetailId = pipd.DetailId AND iid.PIPDetailId = pipd.DetailId 
	LEFT OUTER JOIN HR.Employees AS emp ON iid.EmployeeId = emp.EmployeeId 
	LEFT OUTER JOIN Company.Floors AS floor 
	LEFT OUTER JOIN Company.Buildings AS building ON floor.BuildingId = building.BuildingId 
	RIGHT OUTER JOIN Company.Rooms AS room ON floor.FloorId = room.FloorId ON iid.RoomId = room.RoomId
	WHERE        
	(ii.BranchId={SessionHelper.BranchId}) 
	--and
	--ii.IsPosted=1
	) as tt
) as kk where Qty>0

UNION
select 
PurchaseInvoiceProductDetailId, PurchaseInvoiceId as InvoiceId, BranchId, PurchaseInvoiceDate as InvoiceDate, 
'' as EmpName, null as DepartmentId, '' as DepartmentName, '' as CategoryName,ItemName,IsConsumable,ItemCode,
Qty, null as RoomId,'' as Location,'InStock' AS Method, null  as EmployeeId, ItemId,
PurchaseInvoiceProductDetailId AS ItemCodeDetailId,ConditionTypeId,ConditionName,
PurchaseInvoiceProductId 
from(
	select 
	PurchaseInvoiceId, BranchId, PurchaseInvoiceDate, PurchaseInvoiceProductId, PurchaseInvoiceProductDetailId, 
	ItemId, ItemName,IsConsumable,ItemCode, InStockQty,IssueQty,ReturnQty, ABS((ReturnQty+InStockQty)-IssueQty) as Qty,ConditionName,ConditionTypeId 
	from (
		SELECT        
		pi.PurchaseInvoiceId, pi.BranchId, pi.PurchaseInvoiceDate, pip.PurchaseInvoiceProductId,
		pipd.DetailId as PurchaseInvoiceProductDetailId,pipd.ItemId, item.ItemName,item.IsConsumable,pipd.ItemCode,
		pipd.Qty as InStockQty, con.ConditionTypeId,con.Name AS ConditionName,
		ISNULL((SELECT  SUM(ipipd.Qty)
		FROM AM.IssuedItems AS ii 
		INNER JOIN AM.IssuedItemDetails AS iid ON ii.IssuedItemId = iid.IssuedItemId 
		INNER JOIN AM.PurchaseInvoiceProductDetails AS ipipd ON iid.PIPDetailId = ipipd.DetailId
		where 
		pipd.DetailId = ipipd.DetailId and
		pipd.ItemId = ipipd.ItemId and
		ii.BranchId=pi.BranchId
		),0) as IssueQty,ISNULL((
		 SELECT  SUM(ipipd.Qty)
		FROM            
		AM.ReturnIssue AS ii 
		INNER JOIN AM.ReturnIssueDetails AS iid ON ii.ReturnIssueId = iid.ReturnIssueId 
		and ii.BranchId = iid.BranchId  
		INNER JOIN AM.PurchaseInvoiceProductDetails AS ipipd ON iid.PIPDetailId = ipipd.DetailId
		 where pipd.DetailId = ipipd.DetailId and 
		 pipd.ItemId = ipipd.ItemId and
		 ii.BranchId=pi.BranchId and ii.IsPosted=1
		),0) as ReturnQty
		FROM  AM.PurchaseInvoices AS pi 
		INNER JOIN AM.PurchaseInvoiceProducts AS pip ON pi.PurchaseInvoiceId = pip.PurchaseInvoiceId 
		INNER JOIN AM.PurchaseInvoiceProductDetails AS pipd ON pip.PurchaseInvoiceProductId = pipd.PurchaseInvoiceProductId
		INNER JOIN AM.Items AS item ON pipd.ItemId = item.ItemId 
		LEFT OUTER JOIN AM.ConditionTypes AS con ON pip.ConditionTypeId = con.ConditionTypeId
		WHERE        
		(pi.BranchId={SessionHelper.BranchId}) and pi.IsPosted=1
	) as tt
) as jj 
where Qty>0


").AsQueryable();
            return result;
        }
        public static IQueryable<v_mnl_AMPurchaseItemCode_Result> v_mnl_AMPurchaseItemCodeList(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_AMPurchaseItemCode_Result>($@"
SELECT       
ii.PurchaseInvoiceId, pipd.DetailId, pipd.ItemCode,
case when item.IsConsumable=1 then  isnull((iid.Quantity-iii.Quantity),0) else isnull(iid.Quantity,0) end Quantity,
--iid.Quantity,
item.ItemId,
item.IsConsumable
FROM            
AM.PurchaseInvoices AS ii 
INNER JOIN AM.PurchaseInvoiceProducts AS iid ON ii.PurchaseInvoiceId = iid.PurchaseInvoiceId 
INNER JOIN AM.PurchaseInvoiceProductDetails AS pipd ON iid.PurchaseInvoiceProductId = pipd.PurchaseInvoiceProductId 
INNER JOIN AM.Items AS item ON pipd.ItemId = item.ItemId
left join am.IssuedItemDetails iii on iii.PIPDetailId=pipd.DetailId
where ii.BranchId={SessionHelper.BranchId} and
ItemCode not in(
				SELECT
				case when max(pipd.Qty)-sum(iidnew.Quantity)>0 then '' else
				pipd.ItemCode end ItemCode
				FROM            
				AM.IssuedItemDetails AS iidnew 
				INNER JOIN AM.PurchaseInvoiceProductDetails AS pipd ON iidnew.PIPDetailId = pipd.DetailId 
				INNER JOIN AM.IssuedItems as iinew ON iidnew.IssuedItemId = iinew.IssuedItemId and
				iidnew.BranchId = iinew.BranchId
						 where iidnew.BranchId= ii.BranchId 
						 group by pipd.ItemCode)").AsQueryable();
            return result;
        }
        public static IQueryable<ItemCodes> GetItemCodes(OneDbContext db, int ProductId, int Status, int InStock, int BranchId)
        {
            var query = @"SELECT ItemReg.ItemRegisterId
                    	,ItemReg.ProductId
                    	,ItemReg.ItemCode
                    	,item.ProductName
                    	,ItemReg.Qty Quantity
                    FROM am.ItemRegister ItemReg
                    INNER JOIN inv.Products AS item ON ItemReg.ProductId = item.ProductId
                    WHERE ItemReg.ProductId = @ProductId
                    	AND ItemReg.[Status] = @Status
                    	AND ItemReg.InStock = @InStock
                    	AND ItemReg.BranchId = @BranchId";

            var result = db.Database.SqlQuery<ItemCodes>(query,

                 new SqlParameter("@ProductId", GetDBNullOrValue(ProductId)),
                 new SqlParameter("@Status", GetDBNullOrValue(Status)),
                 new SqlParameter("@InStock", GetDBNullOrValue(InStock)),
                 new SqlParameter("@BranchId", GetDBNullOrValue(BranchId))
                 ).AsQueryable();
            return result;
        }
        #endregion

        #region Sale 
        public static List<v_mnl_TopTenProducts_Result> v_mnl_WeeklySaleDashboardGraph(OneDbContext db, short? BranchId)
        {
            var result = db.Database.SqlQuery<v_mnl_TopTenProducts_Result>($@"DECLARE @MinDate DATE = DATEADD(day, -6, GETDATE()), @MaxDate DATE = getdate();
Declare @tbl table(SaleInvoiceDate date, NetTotal decimal)
declare @Datetbl table(WeekDate date)

insert into @Datetbl
SELECT  TOP(DATEDIFF(DAY, @MinDate, @MaxDate) + 1) Date = DATEADD(DAY, ROW_NUMBER() OVER(ORDER BY a.object_id) - 1, @MinDate)
FROM    sys.all_objects a

insert into @tbl
select SaleInvoiceDate, SUM(NetTotal) from POS.SaleInvoices
where SaleInvoiceDate in (SELECT  WeekDate from @Datetbl) and BranchId=@BranchId
group by SaleInvoiceDate

insert into @tbl
SELECT WeekDate,0 from @Datetbl
where WeekDate not in(select SaleInvoiceDate from @tbl)

--select SaleInvoiceDate, NetTotal, (
--  DATENAME(dw, CAST(DATEPART(m, Cast(GETDATE() as date)) AS VARCHAR)
--  + '/'
--  + CAST(DATEPART(d, Cast(SaleInvoiceDate as date)) AS VARCHAR)
--  + '/'
--  + CAST(DATEPART(yy, Cast(GETDATE() as date)) AS VARCHAR))
--  ) as Day from @tbl order by SaleInvoiceDate asc
  select SaleInvoiceDate, NetTotal, DATENAME(DW, Cast(SaleInvoiceDate as date))  as Day from @tbl order by SaleInvoiceDate asc",
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId))
                         ).ToList();
            return result;
        }
        #endregion

        [Obsolete("use opening balances,yearlybalances table deleted", true)]
        public static List<p_mnl__AccountsBalances_Result> p_mnl__AccountsBalances(OneDbContext db, int? FiscalYearId, short? BranchId)
        {
            var result = db.Database.SqlQuery<p_mnl__AccountsBalances_Result>("SELECT  va.autokey, va.TITLE AS Title, va.ACCOUNT_ID, va.CONTROLACCOUNT, (SELECT(yb.OBDebitAmount - yb.OBCreditAmount)  from Finance.YearlyBalances as yb where yb.FiscalYearId = @FiscalYearId and BranchId = va.BranchId and yb.AccountId = va.autokey) as OpeningBalance, (SELECT        SUM(vd.Debit) FROM            Finance.VoucherDetails AS vd INNER JOIN                          Finance.Vouchers as v ON vd.VoucherId = v.VoucherId WHERE(vd.AccountId = va.autokey) and v.IsPosted = 1 and v.IsCancelled = 0 and v.FiscalYearId = @FiscalYearId and v.BranchId = va.BranchId) as Debit, (SELECT        SUM(vd.Credit) FROM            Finance.VoucherDetails AS vd INNER JOIN                         Finance.Vouchers as v ON vd.VoucherId = v.VoucherId WHERE(vd.AccountId = va.autokey) and v.IsPosted = 1 and v.IsCancelled = 0 and v.FiscalYearId = @FiscalYearId and v.BranchId = va.BranchId) as Credit FROM Finance.Accounts AS va where va.BranchId = @BranchId",
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId)),
                         new SqlParameter("@FiscalYearId", GetDBNullOrValue(FiscalYearId))
                         ).ToList();
            return result;
        }

        public class p_mnl__AccountsBalances_Result
        {
            public long ACCOUNT_ID { get; set; }
            public string TITLE { get; set; }
            public string autokey { get; set; }
            public string LINEAGE { get; set; }
            public Nullable<decimal> OpeningBalance { get; set; }
            public Nullable<decimal> Debit { get; set; }
            public Nullable<decimal> Credit { get; set; }
            public bool CONTROLACCOUNT { get; set; }
        }

        public static List<p_mnl__OpeningStock_Result> p_mnl__OpeningStock(OneDbContext db, int? WareHouseId, short? BranchId)
        {
            var result = db.Database.SqlQuery<p_mnl__OpeningStock_Result>("INSERT INTO Shop.OpeningStock (ProductId, BranchId, WareHouseId, Stock) SELECT pr.ProductId, pr.BranchId, @warehouseid, 0 FROM Shop.Products as pr WHERE  pr.BranchId = @BranchId EXCEPT SELECT op.ProductId, op.BranchId, op.warehouseid, 0 FROM Shop.OpeningStock as op WHERE op.BranchId = @BranchId and op.warehouseid = @warehouseid SELECT        op.OpeningStockId, op.ProductId, op.Stock, op.WareHouseId, op.BranchId, op.CreatedOn, op.CreatedBy, op.ModifiedOn, op.ModifiedBy, pr.ProductName,pr.ProductCode,pr.Barcode FROM            Shop.OpeningStock AS op INNER JOIN Shop.Products AS pr ON op.ProductId = pr.ProductId WHERE op.BranchId = @BranchId and op.warehouseid = @warehouseid",
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId)),
                         new SqlParameter("@WareHouseId", GetDBNullOrValue(WareHouseId))
                         ).ToList();
            return result;
        }

        public class p_mnl__OpeningStock_Result
        {
            public int OpeningStockId { get; set; }
            public int ProductId { get; set; }
            public decimal? Stock { get; set; }
            public int WareHouseId { get; set; }
            public short BranchId { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string ItemName { get; set; }
            public string ShortName { get; set; }
            public string Barcode { get; set; }
        }

        public class v_mnl_Clients_Result
        {
            public int ClientId { get; set; }
            public string PresentableName { get; set; }
            public string Name { get; set; }
            public bool? Gender { get; set; }
            public string MaritalStatus { get; set; }
            public string FatherName { get; set; }
            public string HusbandName { get; set; }
            public string NIC { get; set; }
            public string Address { get; set; }
            public string PhoneNo { get; set; }
            public string MobileNo { get; set; }
            public short? ProfessionId { get; set; }
            public decimal? MonthlyIncome { get; set; }
            public string Email { get; set; }
            public short? CountryId { get; set; }
            public short? CityId { get; set; }
            public short? NationalityId { get; set; }
            public int? ClientTypeId { get; set; }
            public int? GroupId { get; set; }
            public string AccountId { get; set; }
            public bool IsClient { get; set; }
            public bool IsSupplier { get; set; }
            public bool IsAgent { get; set; }
            public byte[] Photo { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public short BranchId { get; set; }
            public string WorkAddress { get; set; }
            public string WorkPhone { get; set; }
            public string Remarks { get; set; }
            public string VehicleRegistrationNo { get; set; }
            public string PassportNo { get; set; }
            public DateTime? VisaExpiryDate { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string PermanentAddress { get; set; }
            public bool ByDefault { get; set; }
            public string autokey { get; set; }
            public string TITLE { get; set; }
            public long? ACCOUNT_ID { get; set; }
            public string ClientTypeName { get; set; }
            public string ClientGroupName { get; set; }
            public string CountryName { get; set; }
            public string ProfessionName { get; set; }
            public string NationalityName { get; set; }
            public string CityName { get; set; }
            //public long? ParentId { get; set; }
            //public string LINEAGE { get; set; }
            //public short? DEPTH { get; set; }
            //public string AccountCode { get; set; }
            //public bool? Credit { get; set; }
            //public double? OpeningBalance { get; set; }
            //public DateTime? OBDate { get; set; }
            //public long? OBVoucherId { get; set; }
            //public bool? CONTROLACCOUNT { get; set; }
            //public bool? ISTRANSACTION { get; set; }
            //public short? CurrencyId { get; set; }
            //public decimal? ExchangeRate { get; set; }
            //public bool BYDEFAULT1 { get; set; }
            //public string DESCN { get; set; }
            //public bool? DBSTATUS { get; set; }
            //public int? ModifiedBy1 { get; set; }
            //public string IP { get; set; }
            //public long? Prev__ID { get; set; }
            //public long? OldAccountCode { get; set; }
            //public bool? IsLocked { get; set; }
            //public int? GroupNo { get; set; }
            //public short? ControlAccountRef { get; set; }
            //public int? StatementSetupId { get; set; }

        }

        public static List<v_mnl_Clients_Result> v_mnl_Clients(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_Clients_Result>("SELECT        cl.ClientId, cl.PresentableName, cl.Name, cl.Address, cl.PhoneNo, cl.MobileNo, cl.CreatedOn, cl.NIC, '' AS PassportNo, cl.Email, cl.NationalityId, cl.CountryId, cl.CityId, cl.ClientTypeId, cl.AccountId, cl.GroupId, cl.IsSupplier, co.CountryName, ci.CityName, cl.Photo, acc.TITLE, cg.ClientGroupName, pro.ProfessionName, acc.ACCOUNT_ID, cl.IsClient, cl.IsAgent, cl.BranchId, cl.Remarks, cl.VehicleRegistrationNo, Client.Types.ClientTypeName, cl.ProfessionId,                          Data.Nationalities.Nationality AS NationalityName, cl.MonthlyIncome, cl.CreatedBy, cl.ModifiedOn, cl.ModifiedBy, cl.DateOfBirth, cl.PermanentAddress, cl.VisaExpiryDate, cl.WorkPhone, cl.WorkAddress, cl.Gender, cl.MaritalStatus, cl.HusbandName, cl.FatherName, cl.ByDefault FROM            Client.Clients AS cl INNER JOIN Client.Groups AS cg ON cl.GroupId = cg.ClientGroupId INNER JOIN                          Client.Types ON cl.ClientTypeId = Client.Types.ClientTypeId AND cl.ClientTypeId = Client.Types.ClientTypeId LEFT OUTER JOIN Finance.Accounts AS acc ON cl.AccountId = acc.autokey LEFT OUTER JOIN Data.Profession AS pro ON cl.ProfessionId = pro.ProfessionId LEFT OUTER JOIN                          Data.Nationalities ON cl.NationalityId = Data.Nationalities.NationalityId AND cl.NationalityId = Data.Nationalities.NationalityId AND cl.NationalityId = Data.Nationalities.NationalityId AND cl.NationalityId = Data.Nationalities.NationalityId LEFT OUTER JOIN Data.Countries AS co ON cl.CountryId = co.CountryId LEFT OUTER JOIN                          Data.Cities AS ci ON cl.CityId = ci.CityId").ToList();
            return result;
        }

        public class v_mnl_SaleInvoices_Result
        {
            public long SaleInvoiceId { get; set; }
            public int ClientId { get; set; }
            public DateTime SaleInvoiceDate { get; set; }
            public short CurrencyId { get; set; }
            public decimal ExchangeRate { get; set; }
            public string DealingPerson { get; set; }
            public string Description { get; set; }
            public bool IsPosted { get; set; }
            public int? PostedBy { get; set; }
            public DateTime? PostedOn { get; set; }
            public bool IsCancelled { get; set; }
            public DateTime? CancelledOn { get; set; }
            public int? CancelledBy { get; set; }
            public decimal Discount { get; set; }
            public int? ModifiedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            public long? VoucherId { get; set; }
            public decimal? TotalAmount { get; set; }
            public decimal? LabourCharges { get; set; }
            public decimal? OtherCharges { get; set; }
            public decimal? FareCharges { get; set; }
            public decimal? NetTotal { get; set; }
            public decimal? Received { get; set; }
            public short? BranchId { get; set; }
            public bool IsApplyTax { get; set; }
            public bool IsAccountPosted { get; set; }
            public string AccountPostedName { get; set; }
            public decimal? ReceivedAmount { get; set; }
            public string PostedName { get; set; }
            public string CancelledName { get; set; }
            public string ModifiedName { get; set; }
            public string CreatedName { get; set; }
            public string ClientName { get; set; }
            public decimal? Paid { get; set; }
            public bool? IsCheked { get; set; }
        }

        public static List<v_mnl_SaleInvoices_Result> v_mnl_SaleInvoices(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_SaleInvoices_Result>("SELECT        si.SaleInvoiceId, si.ClientId, si.SaleInvoiceDate, si.CurrencyId, si.ExchangeRate, si.DealingPerson, si.Description, si.IsPosted, si.PostedBy, si.PostedOn, si.IsCancelled, si.CancelledOn, si.CancelledBy, si.Discount,                        si.ModifiedBy, si.ModifiedOn, si.CreatedOn, si.CreatedBy, si.Version, si.VoucherId, si.TotalAmount, si.LabourCharges, si.OtherCharges, si.FareCharges, ISNULL(si.NetTotal, 0) as NetTotal, si.Received, si.BranchId, si.IsApplyTax,  (SELECT        ISNULL(SUM(Amount), 0) AS Expr1                                FROM            Shop.ClientInvoicePayments AS re                                WHERE(SaleInvoiceId = si.SaleInvoiceId)) AS ReceivedAmount,                        Membership.[Users].Username AS PostedName, User_2.Username AS CancelledName, User_1.Username AS ModifiedName,                          User_3.Username AS CreatedName, cl.Name AS ClientName, convert(decimal(18,2),0) as Paid FROM            Shop.SaleInvoices AS si LEFT OUTER JOIN                          Membership.[Users] AS User_3 ON si.CreatedBy = User_3.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_2 ON si.CancelledBy = User_2.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_1 ON si.ModifiedBy = User_1.UserID LEFT OUTER JOIN                          Membership.[Users] ON si.PostedBy = Membership.[Users].UserID LEFT OUTER JOIN                          Client.Clients AS cl ON si.ClientId = cl.ClientId").ToList();
            return result;
        }

        public class v_mnl_SalesReports_Result
        {
            public long SaleInvoiceId { get; set; }
            public int ClientId { get; set; }
            public DateTime SaleInvoiceDate { get; set; }
            public short CurrencyId { get; set; }
            public decimal ExchangeRate { get; set; }
            public string DealingPerson { get; set; }
            public string Description { get; set; }
            public bool IsPosted { get; set; }
            public int? PostedBy { get; set; }
            public DateTime? PostedOn { get; set; }
            public bool IsCancelled { get; set; }
            public DateTime? CancelledOn { get; set; }
            public int? CancelledBy { get; set; }
            public decimal Discount { get; set; }
            public int? ModifiedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            public long? VoucherId { get; set; }
            public decimal? TotalAmount { get; set; }
            public decimal? LabourCharges { get; set; }
            public decimal? OtherCharges { get; set; }
            public decimal? FareCharges { get; set; }
            public decimal? NetTotal { get; set; }
            public decimal? Received { get; set; }
            public short? BranchId { get; set; }
            public bool IsApplyTax { get; set; }
            public decimal? InvoiceTotal { get; set; }

            public int SaleInvoiceProductId { get; set; }
            public int ProductId { get; set; }
            public string ManufacturerProductNo { get; set; }
            public decimal? OrgWidth { get; set; }
            public decimal? OrgLength { get; set; }
            public decimal? CalWidth { get; set; }
            public decimal? CalLength { get; set; }
            public decimal? CalDigit { get; set; }
            public decimal Quantity { get; set; }
            public decimal? SqFeet { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal? LineTotal { get; set; }
            public decimal Tax { get; set; }
            public DateTime? ExpiryDate { get; set; }
            public string Warranty { get; set; }
            public int? WareHouseId { get; set; }
            public string CategoryName { get; set; }
            public string ProductName { get; set; }
            public int ProductGroupId { get; set; }
            public int CategoryId { get; set; }
            public int BrandId { get; set; }
            public string ProductCode { get; set; }
            public decimal? CostPrice { get; set; }
            public decimal? SalePrice { get; set; }
            public decimal? OpeningStock { get; set; }
            public string Size { get; set; }
            public string Dimensions { get; set; }
            public string Weight { get; set; }
            public string Colour { get; set; }
            public decimal? RecentUnitPrice { get; set; }
            public decimal? UnitsPerPack { get; set; }
            public bool MovedToWarehouse { get; set; }
            public string IP { get; set; }
            public string Barcode { get; set; }
            public int? UnitID { get; set; }
            public string Type { get; set; }
            public decimal? Width { get; set; }
            public decimal? Length { get; set; }
        }

        public static List<v_mnl_SalesReports_Result> v_mnl_SaleReportByProducts(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_SalesReports_Result>("SELECT Shop.SaleInvoices.BranchId,        Shop.Products.ProductId,shop.Products.CategoryId, Shop.Categories.CategoryName, Shop.Products.ProductName , Shop.SaleInvoices.SaleInvoiceDate, Shop.SaleInvoiceProducts.Quantity,Shop.SaleInvoiceProducts.Discount , Shop.SaleInvoiceProducts.UnitPrice,                        Shop.SaleInvoiceProducts.LineTotal, Shop.SaleInvoices.SaleInvoiceId, Shop.SaleInvoiceProducts.NetTotal,Shop.SaleInvoices.NetTotal as InvoiceTotal FROM            Shop.Products INNER JOIN                          Shop.SaleInvoiceProducts ON Shop.Products.ProductId = Shop.SaleInvoiceProducts.ProductId INNER JOIN Shop.SaleInvoices ON Shop.SaleInvoiceProducts.SaleInvoiceId =  Shop.SaleInvoices.SaleInvoiceId INNER JOIN                          Shop.Categories ON Shop.Products.CategoryId = Shop.Categories.CategoryId").ToList();
            return result;
        }

        public static string p_mnl_TransferAccounts(OneDbContext db, Int64? FromAccountId, Int64? ToAccountId)
        {
            var result = db.Database.ExecuteSqlCommand("UPDATE Finance.VoucherDetails SET AccountId = (SELECT autokey FROM Finance.Accounts a	WHERE ACCOUNT_ID = @p1	AND BranchId = @p0) WHERE (	AccountId = ( SELECT autokey FROM Finance.Accounts a WHERE ACCOUNT_ID = @p2 AND BranchId = @p0 ) )",
                         new SqlParameter("@p0", GetDBNullOrValue(branch_ID)),
                         new SqlParameter("@p1", GetDBNullOrValue(ToAccountId)),
                         new SqlParameter("@p2", GetDBNullOrValue(FromAccountId))).ToString();
            return result;
        }



        public static List<v_mnl_PurchaseInvoices_Result> v_mnl_PurchaseInvoices(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_PurchaseInvoices_Result>("SELECT        pu.PurchaseInvoiceId, pu.PurchaseOrderId, pu.PurchaseInvoiceDate, pu.SupplierId, pu.Discount, pu.Description, pu.IsPosted, pu.PostedOn, pu.PostedBy, pu.IsCancelled, pu.CancelledOn, pu.CancelledBy, pu.CreatedOn,                          pu.CreatedBy, pu.ModifiedOn, pu.ModifiedBy, pu.CurrencyId, pu.ExchangeRate, pu.MovedToWarehouse, pu.VoucherId, pu.TotalAmount, pu.LabourCharges, pu.OtherCharges, pu.FareCharges, ISNULL(pu.NetTotal, 0) AS NetTotal, pu.BranchId,                          pu.Version, pu.IsApplyTax, (SELECT        ISNULL(SUM(Amount), 0) AS Expr1                              FROM            Shop.SupplierInvoicePayments AS re                                WHERE(PurchaseInvoiceId = pu.PurchaseInvoiceId)) AS ReceivedAmount, Membership.[Users].Username AS PostedName, User_2.Username AS CancelledName, User_1.Username AS ModifiedName,                          User_3.Username AS CreatedName, cl.Name AS ClientName, convert(decimal(18,2),0) as Paid FROM            Shop.PurchaseInvoices AS pu LEFT OUTER JOIN Membership.[Users] AS User_3 ON pu.CreatedBy = User_3.UserID LEFT OUTER JOIN      Membership.[Users] AS User_2 ON pu.CancelledBy = User_2.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_1 ON pu.ModifiedBy = User_1.UserID LEFT OUTER JOIN                          Membership.[Users] ON pu.PostedBy = Membership.[Users].UserID LEFT OUTER JOIN                          Client.Clients AS cl ON pu.SupplierId = cl.ClientId").ToList();
            return result;
        }

        public class v_mnl_PurchaseInvoiceProducts_Result
        {
            public long PurchaseInvoiceId { get; set; }
            public int SupplierId { get; set; }
            public DateTime PurchaseInvoiceDate { get; set; }
            public short CurrencyId { get; set; }
            public decimal ExchangeRate { get; set; }
            public string DealingPerson { get; set; }
            public string Description { get; set; }
            public bool IsPosted { get; set; }
            public int? PostedBy { get; set; }
            public DateTime? PostedOn { get; set; }
            public bool IsCancelled { get; set; }
            public DateTime? CancelledOn { get; set; }
            public int? CancelledBy { get; set; }
            public decimal Discount { get; set; }
            public int? ModifiedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            public long? VoucherId { get; set; }
            public decimal? TotalAmount { get; set; }
            public decimal? LabourCharges { get; set; }
            public decimal? OtherCharges { get; set; }
            public decimal? FareCharges { get; set; }
            public decimal? NetTotal { get; set; }
            public decimal? Received { get; set; }
            public short? BranchId { get; set; }
            public bool IsApplyTax { get; set; }
            public decimal? InvoiceTotal { get; set; }

            public int PurchaseInvoiceProductId { get; set; }
            public int ProductId { get; set; }
            public string ManufacturerProductNo { get; set; }
            public decimal? OrgWidth { get; set; }
            public decimal? OrgLength { get; set; }
            public decimal? CalWidth { get; set; }
            public decimal? CalLength { get; set; }
            public decimal? CalDigit { get; set; }
            public decimal Quantity { get; set; }
            public decimal? SqFeet { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal? LineTotal { get; set; }
            public decimal Tax { get; set; }
            public DateTime? ExpiryDate { get; set; }
            public string Warranty { get; set; }
            public int? WareHouseId { get; set; }
            public string CategoryName { get; set; }
            public string ProductName { get; set; }
            public int ProductGroupId { get; set; }
            public int CategoryId { get; set; }
            public int BrandId { get; set; }
            public string ProductCode { get; set; }
            public decimal? CostPrice { get; set; }
            public decimal? SalePrice { get; set; }
            public decimal? OpeningStock { get; set; }
            public string Size { get; set; }
            public string Dimensions { get; set; }
            public string Weight { get; set; }
            public string Colour { get; set; }
            public decimal? RecentUnitPrice { get; set; }
            public decimal? UnitsPerPack { get; set; }
            public bool MovedToWarehouse { get; set; }
            public string IP { get; set; }
            public string Barcode { get; set; }
            public int? UnitID { get; set; }
            public string Type { get; set; }
            public decimal? Width { get; set; }
            public decimal? Length { get; set; }
        }

        public static List<v_mnl_PurchaseInvoiceProducts_Result> v_mnl_PurchaseInvoiceProducts(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_PurchaseInvoiceProducts_Result>("SELECT        Inv.BranchId, Shop.Products.ProductId, Shop.Products.CategoryId, Shop.Categories.CategoryName, Shop.Products.ProductName, Inv.PurchaseInvoiceDate, det.Quantity, det.Discount, det.UnitPrice, det.LineTotal, Inv.PurchaseInvoiceId, det.NetTotal, Inv.NetTotal AS InvoiceTotal FROM            Shop.Products INNER JOIN  Shop.PurchaseInvoiceProducts AS det ON Shop.Products.ProductId = det.ProductId INNER JOIN  Shop.PurchaseInvoices AS Inv ON det.PurchaseInvoiceId = Inv.PurchaseInvoiceId INNER JOIN Shop.Categories ON Shop.Products.CategoryId = Shop.Categories.CategoryId").ToList();
            return result;
        }



        public static List<v_mnl_FormRights_Result> v_mnl_FormRights(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>("SELECT Form.FormID, Form.MenuText, FormRights.FormRightName, GroupRights.GroupId, Form.ParentForm, Membership.UserGroups.UserGroupName, Membership.UserGroups.BranchId, GroupRights.Allowed, Form.ControllerName, Form.isActive, Form.FormName, Form.FormURL, FormRights.FormRightId, GroupRights.GroupRightId, Form.IsMenuItem, Form.MenuItemPriority, Form.Icon, Form.PageType, Form.ModuleId FROM Membership.Form AS Form INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId").ToList();
            return result;
        }



        public static List<v_mnl_Vouchers_Result> v_mnl_Vouchers(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_Vouchers_Result>("SELECT        CAST(0 AS bit) AS IsCheked, _vouchers.VoucherId, _vouchers.VoucherType, _vouchers.VoucherName, _vouchers.TransactionDate, _vouchers.DebitAmount, _vouchers.CreditAmount, _vouchers.Balance, _vouchers.CreatedOn,                         _vouchers.CreatedBy, _vouchers.ModifiedOn, _vouchers.ModifiedBy, _vouchers.Particulars, _createdby.UserID AS _CreatedByUserId, _modifiedby.UserID AS _ModifiedByUserId, _vouchers.VoucherStatus, _vouchers.IsPosted, _vouchers.IsCancelled, _vouchers.IsApproved, _vouchers.CurrencyId, _vouchers.ExchangeRate, cu.CurrencyName, _postedBy.UserID AS _postedByUserId, _approvedBy.UserID AS _approvedByUserId, _cancelledBy.UserID AS _cancelledByUserId, _vouchers.ApprovedBy, _vouchers.PostedBy, _vouchers.CancelledBy, CASE _vouchers.isApproved WHEN 1 THEN 'Yes' ELSE 'No' END AS _IsApproved, CASE _vouchers.isPosted WHEN 1 THEN 'Yes' ELSE 'No' END AS _IsPosted, CASE _vouchers.isCancelled WHEN 1 THEN 'Yes' ELSE 'No' END AS _IsCancelled, _approvedBy.Username AS _ApprovedByName, _postedBy.Username AS _PostedByName, _createdby.Username AS _CreatedByName, _cancelledBy.Username AS _CancelledByName, _modifiedby.Username AS _ModifiedByName, _vouchers.CBAccountId, _vouchers.CBAccountId AS AccountId, _vouchers.BranchId, _vouchers.ProjectId, _vouchers.PostedOn, _vouchers.CancelledOn FROM            Finance.Vouchers AS _vouchers INNER JOIN Finance.Currencies AS cu ON _vouchers.CurrencyId = cu.CurrencyId LEFT OUTER JOIN                         Membership.[Users] AS _createdby ON _createdby.UserID = _vouchers.CreatedBy LEFT OUTER JOIN                         Membership.[Users] AS _cancelledBy ON _vouchers.CancelledBy = _cancelledBy.UserID LEFT OUTER JOIN                         Membership.[Users] AS _approvedBy ON _vouchers.ApprovedBy = _approvedBy.UserID LEFT OUTER JOIN                         Membership.[Users] AS _postedBy ON _vouchers.PostedBy = _postedBy.UserID LEFT OUTER JOIN                         Membership.[Users] AS _modifiedby ON _modifiedby.UserID = _vouchers.ModifiedBy").ToList();
            return result;
        }

        public class v_mnl_FillYearlyBalances_Result
        {
            public long? RowNo { get; set; }
            public int YearlyBalanceId { get; set; }
            public int FiscalYearId { get; set; }
            public string AccountId { get; set; }
            public decimal OBDebitAmount { get; set; }
            public decimal OBCreditAmount { get; set; }
            public decimal TransactionDebitAmount { get; set; }
            public decimal TransactionCreditAmount { get; set; }
            public short? BranchId { get; set; }
            public string AccountCode { get; set; }
            public string TITLE { get; set; }
            public short? DEPTH { get; set; }
        }
        [Obsolete("use opening balances,yearlybalances table deleted", true)]
        public static List<v_mnl_FillYearlyBalances_Result> v_mnl_FillYearlyBalances(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_FillYearlyBalances_Result>("SELECT       ROW_NUMBER() over(order by Accounts.AccountCode) as RowNo, YearlyBalances.YearlyBalanceId, YearlyBalances.FiscalYearId, YearlyBalances.AccountId, YearlyBalances.OBDebitAmount, YearlyBalances.OBCreditAmount, YearlyBalances.TransactionDebitAmount,                          YearlyBalances.TransactionCreditAmount, YearlyBalances.BranchId, Accounts.AccountCode, Accounts.TITLE, Accounts.DEPTH FROM            Finance.YearlyBalances AS YearlyBalances INNER JOIN                          Finance.Accounts AS Accounts ON YearlyBalances.AccountId = Accounts.autokey WHERE(1 = Accounts.ISTRANSACTION) AND(0 = Accounts.CONTROLACCOUNT) AND(Accounts.ParentId IS NOT NULL)").ToList();
            return result;
        }

        public class v_mnl_BankAccounts_Result
        {
            public string AccountId { get; set; }
            public string AccountTitle { get; set; }
            public string AccountNo { get; set; }
            public string BankName { get; set; }
            public string BranchName { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Fax { get; set; }
            public string Email { get; set; }
            public string PDCReceivable { get; set; }
            public string PDCPayable { get; set; }
            public int? LastChequeNo { get; set; }
            public int? ChequeNoLength { get; set; }
            public string PDCReceivableTitle { get; set; }
            public string PDCPayableTitle { get; set; }
            public short? BranchId { get; set; }

        }

        public static List<v_mnl_BankAccounts_Result> v_mnl_BankAccounts(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_BankAccounts_Result>("SELECT        Finance.BankAccounts.AccountId, Finance.BankAccounts.AccountTitle, Finance.BankAccounts.AccountNo, Finance.BankAccounts.BankName, Finance.BankAccounts.BranchName, Finance.BankAccounts.Address, Finance.BankAccounts.Phone, Finance.BankAccounts.Fax, Finance.BankAccounts.Email, Finance.BankAccounts.PDCReceivable, Finance.BankAccounts.PDCPayable, Finance.BankAccounts.LastChequeNo, Finance.BankAccounts.ChequeNoLength, Accounts_1.TITLE AS PDCReceivableTitle, Accounts_2.TITLE AS PDCPayableTitle, Finance.Accounts.BranchId FROM            Finance.BankAccounts INNER JOIN Finance.Accounts ON Finance.BankAccounts.AccountId = Finance.Accounts.autokey LEFT OUTER JOIN Finance.Accounts AS Accounts_2 ON Finance.BankAccounts.PDCPayable = Accounts_2.autokey LEFT OUTER JOIN Finance.Accounts AS Accounts_1 ON Finance.BankAccounts.PDCReceivable = Accounts_1.autokey").ToList();
            return result;
        }

        public class p_mnl_Account__Search_Result
        {
            public long ACCOUNT_ID { get; set; }
            public string TITLE { get; set; }
            public string autokey { get; set; }
            public string LINEAGE { get; set; }
            public string Locked { get; set; }
            public bool IsLocked { get; set; }
            public string IsRoot { get; set; }
            public string OB { get; set; }
            public Nullable<int> Children { get; set; }
            public Nullable<long> ParentId { get; set; }
            public Nullable<short> DEPTH { get; set; }
            public Nullable<short> BranchId { get; set; }
            public bool CONTROLACCOUNT { get; set; }
            public string AccountCode { get; set; }
            public string TITLES { get; set; }
            public bool ISTRANSACTION { get; set; }

        }

        //public static List<p_mnl_Account__Search_Result> p_mnl_Account__Search(OneDbContext db, bool? TreeView, long? ParentId, string Lineage, long? AccountCode, long? BranchId, bool? ByDefault, bool ControlAccountsOnly, long? ControlAccountRefType)
        //{
        //    var result = db.Database.SqlQuery<p_mnl_Account__Search_Result>("IF @ParentId IS NULL AND @ControlAccountRefType IS NOT NULL SELECT TOP 1 @ParentId = ACCOUNT_ID FROM Finance.Accounts        WHERE ControlAccountRef = @ControlAccountRefType PRINT @ParentId SET @TreeView = ISNULL(@TreeView, 1); SET @Lineage = ISNULL(@Lineage, '%'); SELECT ACCOUNT_ID, TITLE, autokey, LINEAGE, Locked, IsLocked, IsRoot, OB, Children, ParentId, DEPTH, BranchId, CONTROLACCOUNT, AccountCode, TITLES, ISTRANSACTION FROM (" + accountQuery + ") AS a WHERE(BranchId = @BranchId) AND(ParentId IS NOT NULL)        AND(ParentId = ISNULL(@ParentId, ParentId)) AND(LINEAGE LIKE @Lineage + '%') AND(ACCOUNT_ID = ISNULL(@AccountCode, ACCOUNT_ID)) AND(IsLocked = ISNULL(@ByDefault, IsLocked)) AND CONTROLACCOUNT = ISNULL(@ControlAccountsOnly, CONTROLACCOUNT) ORDER BY LINEAGE + CAST(ACCOUNT_ID AS VARCHAR); ",
        //        new SqlParameter("@TreeView", GetDBNullOrValue(TreeView)),
        //                 new SqlParameter("@ParentId", GetDBNullOrValue(ParentId)),
        //                 new SqlParameter("@Lineage", GetDBNullOrValue(Lineage)),
        //                 new SqlParameter("@AccountCode", GetDBNullOrValue(AccountCode)),
        //                 new SqlParameter("@BranchId", GetDBNullOrValue(BranchId)),
        //                 new SqlParameter("@ByDefault", GetDBNullOrValue(ByDefault)),
        //                 new SqlParameter("@ControlAccountsOnly", GetDBNullOrValue(ControlAccountsOnly)),
        //                 new SqlParameter("@ControlAccountRefType", GetDBNullOrValue(ControlAccountRefType))
        //        ).ToList();
        //    return result;
        //}
        public static List<p_mnl_Account__Search_Result> p_mnl_Account__Search(OneDbContext db, bool? TreeView, long? ParentId, string Lineage, long? AccountCode, long? BranchId, bool? ByDefault, bool ControlAccountsOnly, long? ControlAccountRefType)
        {
            var result = db.Database.SqlQuery<p_mnl_Account__Search_Result>(@"IF @ParentId IS NULL
	AND @ControlAccountRefType IS NOT NULL
	SELECT TOP 1 @ParentId = ACCOUNT_ID
	FROM Finance.Accounts
	WHERE ControlAccountRef = @ControlAccountRefType

PRINT @ParentId

SET @TreeView = ISNULL(@TreeView, 1);
SET @Lineage = ISNULL(@Lineage, '%');

SELECT ACCOUNT_ID
	, TITLE
	, autokey
	, LINEAGE
	, Locked
	, IsLocked
	, IsRoot
	, OB
	, Children
	, ParentId
	, DEPTH
	, BranchId
	, CONTROLACCOUNT
	, AccountCode
	, TITLES
	, ISTRANSACTION
FROM Finance.v__Accounts AS a
WHERE (BranchId = @BranchId)
	AND (ParentId IS NOT NULL)
	AND (ParentId = ISNULL(@ParentId, ParentId))
	AND (LINEAGE LIKE @Lineage + '%')
	AND (ACCOUNT_ID = ISNULL(@AccountCode, ACCOUNT_ID))
	AND (IsLocked = ISNULL(@ByDefault, IsLocked))
	AND CONTROLACCOUNT = ISNULL(@ControlAccountsOnly, CONTROLACCOUNT)
ORDER BY LINEAGE + CAST(ACCOUNT_ID AS VARCHAR);
",
                new SqlParameter("@TreeView", GetDBNullOrValue(TreeView)),
                         new SqlParameter("@ParentId", GetDBNullOrValue(ParentId)),
                         new SqlParameter("@Lineage", GetDBNullOrValue(Lineage)),
                         new SqlParameter("@AccountCode", GetDBNullOrValue(AccountCode)),
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId)),
                         new SqlParameter("@ByDefault", GetDBNullOrValue(ByDefault)),
                         new SqlParameter("@ControlAccountsOnly", GetDBNullOrValue(ControlAccountsOnly)),
                         new SqlParameter("@ControlAccountRefType", GetDBNullOrValue(ControlAccountRefType))
                ).ToList();
            return result;
        }


        public class p_mnl_dbo_Account__Search_Result
        {
            public string TitleS { get; set; }
            public string autokey { get; set; }
            public string LINEAGE { get; set; }
            public string Locked { get; set; }
            public bool IsLocked { get; set; }
            public string IsRoot { get; set; }
            public string OB { get; set; }
            public Nullable<int> Children { get; set; }
            public Nullable<long> ParentId { get; set; }
            public Nullable<short> DEPTH { get; set; }
            public long ACCOUNT_ID { get; set; }
            public Nullable<short> BranchId { get; set; }
            public bool CONTROLACCOUNT { get; set; }
            public string AccountCode { get; set; }
            public string TITLE { get; set; }
            public string AccountTitle { get; set; }
            public bool ISTRANSACTION { get; set; }
            public short ControlAccountRef { get; set; }

        }

        static string accountQuery = "SELECT        CASE 1 WHEN 1 THEN REPLICATE('-', (a.DEPTH - 1) * 2) ELSE '' END + CASE WHEN CONTROLACCOUNT = 1 THEN '[C]' WHEN ISTRANSACTION = 1 THEN '[T]' ELSE '[]' END + ' [ ' + CAST(a.ACCOUNT_ID AS VARCHAR(20)) + ' ] ' + UPPER(a.TITLE) AS TitleS, a.autokey, a.LINEAGE, CASE WHEN a.BYDEFAULT = 'True' THEN 'lock.png' ELSE 'Spacer.gif' END AS Locked, a.BYDEFAULT AS IsLocked, CASE WHEN[ACCOUNT_ID] = 0 THEN 'False' ELSE 'True' END AS IsRoot, a.LINEAGE + CAST(a.ACCOUNT_ID AS VARCHAR(24)) AS OB,ch.Children, a.ParentId, a.DEPTH, a.ACCOUNT_ID, a.BranchId, a.CONTROLACCOUNT, a.AccountCode, a.TITLE, a.TITLE AS AccountTitle, a.ISTRANSACTION, ControlAccountRef FROM            Finance.Accounts AS a LEFT OUTER JOIN (SELECT        COUNT(*) AS Children, ParentId FROM[Finance].[Accounts] AS aa GROUP BY ParentId) AS ch ON a.ACCOUNT_ID = ch.ParentId";
        public static List<p_mnl_dbo_Account__Search_Result> p_mnl_dbo_Account__Search(OneDbContext db, bool? TreeView, long? ParentId, string Lineage, long? AccountCode, long? BranchId)
        {
            var result = db.Database.SqlQuery<p_mnl_dbo_Account__Search_Result>("SET @TreeView = ISNULL(@TreeView,1) SET @Lineage = ISNULL(@Lineage, '%') SELECT * FROM (" + accountQuery + ") a WHERE a.BranchId = @BranchId AND a.ParentId IS NOT NULL AND ParentId = ISNULL(@ParentId, ParentId) AND a.Lineage LIKE @Lineage + '%' AND a.ACCOUNT_ID = ISNULL(@AccountCode, ACCOUNT_ID) ORDER BY Lineage + CAST(a.ACCOUNT_ID AS VARCHAR)",
                new SqlParameter("@TreeView", GetDBNullOrValue(TreeView)),
                         new SqlParameter("@ParentId", GetDBNullOrValue(ParentId)),
                         new SqlParameter("@Lineage", GetDBNullOrValue(Lineage)),
                         new SqlParameter("@AccountCode", GetDBNullOrValue(AccountCode)),
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId))
                ).ToList();
            return result;
        }

        public class p_mnl_CostGroup_Search_Result
        {
            public int CostGroupId { get; set; }
            public string CostGroupName { get; set; }
        }

        public static List<p_mnl_CostGroup_Search_Result> p_mnl_CostGroup_Search(OneDbContext db, short? BranchId)
        {
            var result = db.Database.SqlQuery<p_mnl_CostGroup_Search_Result>("SELECT CostGroupId,CAST(CostGroupCode as varchar) + ' ' +CostGroupName AS CostGroupName FROM Finance.CostGroups WHERE (1 = 1) AND (BranchId = @BranchId) AND ParentId IS NOT NULL",
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId))
                ).ToList();
            return result;
        }

        public class p_mnl_Account_GetCashAndBankAccounts_Result
        {
            public Nullable<long> ACCOUNT_ID { get; set; }
            public string TITLE { get; set; }
            public string autokey { get; set; }
            public string Lineage { get; set; }
            public string Locked { get; set; }
            public Nullable<bool> IsLocked { get; set; }
            public string IsRoot { get; set; }
            public string OB { get; set; }
            public Nullable<int> Children { get; set; }
            public Nullable<long> ParentId { get; set; }
            public Nullable<short> Depth { get; set; }
            public Nullable<short> BranchId { get; set; }
            public Nullable<bool> CONTROLACCOUNT { get; set; }
            public Nullable<long> AccountCode { get; set; }
            public string TITLES { get; set; }
            public Nullable<bool> ISTRANSACTION { get; set; }
        }

        public static List<p_mnl_Account_GetCashAndBankAccounts_Result> p_mnl_Account_GetCashAndBankAccounts(OneDbContext db, bool? GetCashAccounts, bool? GetBankAccounts, short? BranchId, bool? ShowOnFrontDeskOnly)
        {
            var result = db.Database.SqlQuery<p_mnl_Account_GetCashAndBankAccounts_Result>($@"
								DECLARE @TBL TABLE (
								[ACCOUNT_ID] BIGINT
								,[TITLE] NVARCHAR(500)
								,[autokey] VARCHAR(36)
								,[Lineage] VARCHAR(500)
								,[Locked] VARCHAR(10)
								,[IsLocked] BIT
								,[IsRoot] VARCHAR(5)
								,[OB] VARCHAR(524)
								,[Children] INT
								,[ParentId] BIGINT
								,[Depth] SMALLINT
								,[BranchId] SMALLINT
								,[CONTROLACCOUNT] BIT
								,[AccountCode] BIGINT
								,[TITLES] NVARCHAR(500)
								,ISTRANSACTION BIT
								)

							IF @GetCashAccounts = 1
							BEGIN
								INSERT INTO @TBL
								SELECT ACCOUNT_ID
									,TITLE
									,autokey
									,LINEAGE
									,IsLocked
									,IsLocked
									,0
									,''
									,0
									,ParentId
									,DEPTH
									,BranchId
									,CONTROLACCOUNT
									,ACCOUNT_ID
									,'[' + CAST(ACCOUNT_ID AS VARCHAR(20)) + '] ' + UPPER(TITLE) AS TitleS
									,ISTRANSACTION
								FROM Finance.CashAccounts ca
								INNER JOIN Finance.Accounts acc ON ca.CashAccountId = acc.autokey
								WHERE acc.BranchId = @BranchId
							END

							IF @GetBankAccounts = 1
							BEGIN
								INSERT INTO @TBL
								SELECT ACCOUNT_ID
									,TITLE
									,autokey
									,LINEAGE
									,IsLocked
									,IsLocked
									,0
									,''
									,0
									,ParentId
									,DEPTH
									,acc.BranchId
									,CONTROLACCOUNT
									,ACCOUNT_ID
									,'[' + CAST(ACCOUNT_ID AS VARCHAR(20)) + '] ' + UPPER(TITLE) AS TitleS
									,ISTRANSACTION
								FROM Finance.BankAccounts ca
								INNER JOIN Finance.Accounts acc ON ca.AccountId = acc.autokey
								WHERE acc.BranchId = @BranchId
							END
							SELECT *
							FROM @TBL [t]",
                         new SqlParameter("@GetCashAccounts", GetDBNullOrValue(GetCashAccounts)),
                         new SqlParameter("@GetBankAccounts", GetDBNullOrValue(GetBankAccounts)),
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId)),
                         new SqlParameter("@ShowOnFrontDeskOnly", GetDBNullOrValue(ShowOnFrontDeskOnly))
                ).ToList();
            return result;
        }
        [Obsolete("use opening balances,yearlybalances table deleted", true)]
        public static string p_mnl_FillYearlyBalances(OneDbContext db, int? FiscalYearId, short? BranchId, DateTime? StartDate, DateTime? EndDate)
        {
            var result = db.Database.ExecuteSqlCommand("INSERT INTO Finance.YearlyBalances (FiscalYearId, AccountId, BranchId) SELECT @FiscalYearId AS Expr1, autokey, BranchId FROM Finance.Accounts WHERE 1 = 1 AND BranchId = @BranchId EXCEPT SELECT FiscalYearId, AccountId, BranchId FROM Finance.YearlyBalances WHERE FiscalYearId = @FiscalYearId INSERT INTO Finance.MonthlyBalances(Month, Year, YearlyBalanceId, AccountId, BranchId, FiscalYearId) SELECT MONTH(cl.d), YEAR(cl.d), YearlyBalanceId, AccountId, @BranchId, @FiscalYearId FROM Finance.YearlyBalances acc CROSS JOIN Helper.Calendar cl WHERE 1 = 1 AND FiscalYearId = @FiscalYearId    AND cl.d BETWEEN @StartDate        AND @EndDate EXCEPT SELECT MONTH, YEAR, YearlyBalanceId, AccountId, BranchId, FiscalYearId FROM MonthlyBalances WHERE FiscalYearId = @FiscalYearId",
                         new SqlParameter("@FiscalYearId", GetDBNullOrValue(FiscalYearId)),
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId)),
                         new SqlParameter("@EndDate", GetDBNullOrValue(EndDate)),
                         new SqlParameter("@StartDate", GetDBNullOrValue(StartDate))
                ).ToString();
            return result;
        }

        public static List<v_mnl_UpcomingEvent_Result> v_mnl_UpcomingEventList(OneDbContext db, short BranchId)
        {
            var result = db.Database.SqlQuery<v_mnl_UpcomingEvent_Result>($@"SELECT TOP(25) eve.BranchId, eve.EventId, eve.EventDate, clt.Name AS ClientName, evtype.Title AS EventTitle, meal.Title as MealTitle, loc.Title AS LocationTitle
FROM            Event.Events AS eve INNER JOIN
						 Client.Clients AS clt ON eve.ClientId = clt.ClientId INNER JOIN
						 Event.EventTypes AS evtype ON eve.EventTypeId = evtype.EventTypeId INNER JOIN
						 Event.Meals AS meal ON eve.MealId = meal.MealId INNER JOIN
						 Event.Locations AS loc ON eve.LocationId = loc.LocationId
WHERE        (eve.BranchId = @BranchId) AND (eve.StatusId = 1)", new SqlParameter("@BranchId", BranchId)).ToList();
            return result;
        }

        public static string p_mnl_FillFormRights(OneDbContext db)
        {
            var result = db.Database.ExecuteSqlCommand("DECLARE @TBL TABLE( [Action] VARCHAR(50)); INSERT INTO @TBL ( [Action] ) SELECT 'Can Create' UNION SELECT 'Can Read' UNION SELECT 'Can Update' UNION  SELECT 'Can Delete'; INSERT INTO[Membership].[FormRights] ([FormRightName], [FormId] ) SELECT[t].[Action], [FormId] FROM @TBL AS[t] CROSS JOIN[Membership].[Form] EXCEPT SELECT FormRightName, FormId FROM Membership.FormRights;").ToString();
            return result;
        }

        //public class v_mnl_UserBranches_Result
        //{
        //    public int UserBranchId { get; set; }
        //    public int UserId { get; set; }
        //    public short BranchId { get; set; }
        //    public Nullable<bool> Active { get; set; }
        //    public bool DefaultBranch { get; set; }
        //    public string Name { get; set; }
        //}

        //public static List<v_mnl_UserBranches_Result> v_mnl_UserBranches(OneDbContext db)
        //{
        //    var result = db.Database.SqlQuery<v_mnl_UserBranches_Result>("SELECT        Membership.UserBranches.UserBranchId, Membership.UserBranches.UserId, Membership.UserBranches.BranchId, Membership.UserBranches.Active, Membership.UserBranches.DefaultBranch, Company.Branches.Name FROM            Company.Branches INNER JOIN Membership.UserBranches ON Company.Branches.BranchId = Membership.UserBranches.BranchId").ToList();
        //    return result;
        //}

        //public class p_mnl_DashboardMenus_Result
        //{
        //    public string MenuText { get; set; }
        //    public string FormUrl { get; set; }
        //    public string Icon { get; set; }
        //}

        //public static List<p_mnl_DashboardMenus_Result> p_mnl_DashboardMenus(OneDbContext db, int? GroupId, string URL)
        //{
        //    var result = db.Database.SqlQuery<p_mnl_DashboardMenus_Result>("Declare @ParentForm int set @ParentForm = (select top 1 ParentForm from Membership.Form where FormUrl like @URL) if (@ParentForm is null) Begin SELECT        MenuText,FormUrl,Icon FROM            Membership.Form AS Form INNER JOIN  Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId where ParentForm = 1 and isActive = 'Yes' and Allowed = 1 and IsMenuItem = 1 and FormRightName = 'Can Read' and groupid = @GroupId order by MenuItemPriority END ELSE BEGIN SELECT        MenuText,FormUrl,Icon FROM            Membership.Form AS Form INNER JOIN  Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId where ParentForm = @ParentForm and isActive = 'Yes' and Allowed = 1 and FormRightName = 'Can Read' and groupid = @GroupId order by MenuItemPriority END",
        //                 new SqlParameter("@GroupId", GetDBNullOrValue(GroupId)),
        //                 new SqlParameter("@URL", GetDBNullOrValue(URL))).ToList();
        //    return result;
        //}

        //public class p_mnl_QuickLinks_Result
        //{
        //    public string MenuText { get; set; }
        //    public string FormUrl { get; set; }
        //}

        //public static List<p_mnl_QuickLinks_Result> p_mnl_QuickLinks(OneDbContext db, int? GroupId, string URL)
        //{
        //    var result = db.Database.SqlQuery<p_mnl_QuickLinks_Result>("Declare @ParentForm int set @ParentForm = (select top 1 ParentForm from Membership.Form where FormUrl like @URL) if (@ParentForm is null) Begin SELECT MenuText,FormUrl FROM Membership.Form AS Form INNER JOIN  Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId where ParentForm = 1 and isActive = 'Yes' and Allowed = 1 and IsMenuItem = 1 and FormRightName = 'Can Read' and groupid = @GroupId order by MenuItemPriority END ELSE BEGIN SELECT MenuText,FormUrl,Icon FROM Membership.Form AS Form INNER JOIN  Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId where ParentForm = @ParentForm and isActive = 'Yes' and Allowed = 1 and FormRightName = 'Can Read' and groupid = @GroupId order by MenuItemPriority END",
        //                 new SqlParameter("@GroupId", GetDBNullOrValue(GroupId)),
        //                 new SqlParameter("@URL", GetDBNullOrValue(URL))).ToList();
        //    return result;
        //}
        public static string t_VoucherDetail(OneDbContext db, Int64? VoucherId)
        {
            var result = db.Database.ExecuteSqlCommand("Declare @DebitAmount DECIMAL(18, 2), @CreditAmount DECIMAL(18, 2), @VoucherType VARCHAR(5), @Autokey VARCHAR(36) SELECT @DebitAmount = SUM(vd.Debit), @CreditAmount = SUM(vd.Credit) FROM Finance.VoucherDetails AS vd WHERE VoucherId = @VoucherId AND vd.TransactionId <> 1 SELECT @VoucherType = VoucherType FROM Finance.VoucherDetails AS vd INNER JOIN Finance.Vouchers AS v ON v.VoucherId = vd.VoucherId WHERE v.VoucherId = @VoucherId IF @VoucherType IN('CP', 'BP', 'CR', 'BR', 'FV', 'CRF', 'BRF', 'SV', 'CPS', 'BPS', 'CPL', 'BPL', 'CRI', 'BRI','PI','PR','SI','SR','SIP','PIP') BEGIN UPDATE Finance.VoucherDetails SET TransactionType = CASE WHEN @VoucherType IN('CR', 'BR', 'FV', 'CRF', 'BRF', 'SV', 'CRI', 'BRI', 'SI', 'PR','PIP') THEN 'Dr' ELSE 'Cr' END, Debit = ISNULL(CASE WHEN @VoucherType IN('CR', 'BR', 'FV', 'CRF', 'BRF', 'SV', 'CRI', 'BRI', 'SI', 'PR','PIP') THEN ABS(@DebitAmount - @CreditAmount) ELSE 0 END, 0), Credit = ISNULL(CASE WHEN @VoucherType IN('CP', 'BP', 'CPS', 'BPS', 'CPL', 'BPL','PI','SR','SIP') THEN ABS(@DebitAmount - @CreditAmount) ELSE 0 END, 0) FROM Finance.VoucherDetails AS vd WHERE VoucherId = @VoucherId AND vd.TransactionId = 1 END SELECT @DebitAmount = SUM(vd.Debit), @CreditAmount = SUM(vd.Credit) FROM Finance.VoucherDetails AS vd WHERE VoucherId = @VoucherId IF @DebitAmount IS NOT NULL AND @CreditAmount IS NOT NULL BEGIN UPDATE Finance.Vouchers SET DebitAmount = @DebitAmount, CreditAmount = @CreditAmount FROM Finance.Vouchers AS v WHERE VoucherId = @VoucherId END",
                         new SqlParameter("@VoucherId", GetDBNullOrValue(VoucherId))).ToString();
            return result;
        }

        //public static bool SetCounterSession(OneDbContext db, string SaleCounterSessionId)
        //{
        //    if (!string.IsNullOrEmpty(SaleCounterSessionId))
        //    {
        //        var session = db.SaleCounterSessions.Include("SMSaleCounter").Where(u => u.SaleCounterSessionId == SaleCounterSessionId).FirstOrDefault();
        //        if (session != null)
        //        {
        //            SessionHelper.ClientIdAMCounterSession = session.SMSaleCounter.ClientId;
        //            SessionHelper.CashAccountIdAMCounterSession = session.SMSaleCounter.CashAccountId;
        //            SessionHelper.BankAccountIdAMCounterSession = session.SMSaleCounter.BankAccountId;
        //            SessionHelper.WareHouseIdAMCounterSession = session.SMSaleCounter.WareHouseId;
        //            SessionHelper.SaleCounterSessionIdAMCounterSession = session.SaleCounterSessionId;
        //            SessionHelper.CounterNameAMCounterSession = session.SMSaleCounter.Title;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        public static string t_POSClientInvoicePayment(OneDbContext db, Int64? InvoiceId, short? BranchId)
        {
            var result = db.Database.ExecuteSqlCommand("if @InvoiceId is not null begin Update AM.SaleInvoices set Received = (select ISNULL(SUM(Amount), 0) from AM.ClientInvoicePayments where BranchId = @BranchId and SaleInvoiceId = @InvoiceId) where BranchId = @BranchId and SaleInvoiceId = @InvoiceId end", new SqlParameter("@InvoiceId", GetDBNullOrValue(InvoiceId)),
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId))).ToString();
            return result;
        }
        public class v_mnl_VoucherTransactions
        {
            public long VoucherDetailId { get; set; }
            public string AccountId { get; set; }
        }
        public class v_mnl_SaleReturns_Result
        {
            public int SaleReturnId { get; set; }
            public int ClientId { get; set; }
            public DateTime SaleReturnDate { get; set; }
            public short CurrencyId { get; set; }
            public decimal ExchangeRate { get; set; }
            public string DealingPerson { get; set; }
            public string Description { get; set; }
            public bool IsPosted { get; set; }
            public int? PostedBy { get; set; }
            public DateTime? PostedOn { get; set; }
            public bool IsCancelled { get; set; }
            public DateTime? CancelledOn { get; set; }
            public int? CancelledBy { get; set; }
            public decimal Discount { get; set; }
            public int? ModifiedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            public long? VoucherId { get; set; }
            public decimal? TotalAmount { get; set; }
            public decimal? LabourCharges { get; set; }
            public decimal? OtherCharges { get; set; }
            public decimal? FareCharges { get; set; }
            public decimal? NetTotal { get; set; }
            public decimal? Received { get; set; }
            public short? BranchId { get; set; }
            public bool IsApplyTax { get; set; }

            public decimal? ReceivedAmount { get; set; }
            public string PostedName { get; set; }
            public string CancelledName { get; set; }
            public string ModifiedName { get; set; }
            public string CreatedName { get; set; }
            public string ClientName { get; set; }
            public decimal? Paid { get; set; }
            public bool? IsCheked { get; set; }
        }
        public class v_mnl_PurchaseReturns_Result
        {
            public long PurchaseReturnId { get; set; }
            public DateTime PurchaseReturnDate { get; set; }
            public int SupplierId { get; set; }
            public decimal Discount { get; set; }
            public string Description { get; set; }
            public bool IsPosted { get; set; }
            public DateTime? PostedOn { get; set; }
            public int? PostedBy { get; set; }
            public bool IsCancelled { get; set; }
            public DateTime? CancelledOn { get; set; }
            public int? CancelledBy { get; set; }
            public DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public short? CurrencyId { get; set; }
            public decimal? ExchangeRate { get; set; }
            public long? VoucherId { get; set; }
            public decimal? TotalAmount { get; set; }
            public decimal? LabourCharges { get; set; }
            public decimal? OtherCharges { get; set; }
            public decimal? FareCharges { get; set; }
            public decimal? NetTotal { get; set; }
            public short? BranchId { get; set; }
            public bool IsApplyTax { get; set; }

            public decimal? ReceivedAmount { get; set; }
            public string PostedName { get; set; }
            public string CancelledName { get; set; }
            public string ModifiedName { get; set; }
            public string CreatedName { get; set; }
            public string ClientName { get; set; }
            public decimal? Paid { get; set; }
            public bool? IsCheked { get; set; }
        }
        public class v_mnl_Damages_Result
        {
            public int DamageId { get; set; }
            public DateTime DamageDate { get; set; }
            public string Description { get; set; }
            public bool IsPosted { get; set; }
            public int? PostedBy { get; set; }
            public DateTime? PostedOn { get; set; }
            public bool IsCancelled { get; set; }
            public DateTime? CancelledOn { get; set; }
            public int? CancelledBy { get; set; }
            public int? ModifiedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public DateTime CreatedOn { get; set; }
            public int CreatedBy { get; set; }
            public long? VoucherId { get; set; }
            public decimal? NetTotal { get; set; }
            public decimal? Received { get; set; }
            public short? BranchId { get; set; }
            public bool IsApplyTax { get; set; }

            public decimal? ReceivedAmount { get; set; }
            public string PostedName { get; set; }
            public string CancelledName { get; set; }
            public string ModifiedName { get; set; }
            public string CreatedName { get; set; }
            public decimal? Paid { get; set; }
            public bool? IsCheked { get; set; }
        }
        public class v_mnl_TrialBalance_Result
        {
            public long ACCOUNTID { get; set; }
            public string TITLE { get; set; }
            public string PrintableName { get; set; }
            public decimal? Debit { get; set; }
            public decimal? Credit { get; set; }
            public string Lineage { get; set; }
        }
        public class v_mnl_GeneralLedger_Result
        {
            public long AccountCode { get; set; }
            public string AccountTitle { get; set; }
            public long? ParentId { get; set; }
            public DateTime? TransactionDate { get; set; }
            public string VoucherName { get; set; }
            public decimal? Debit { get; set; }
            public decimal? Credit { get; set; }
            public short? BranchId { get; set; }
            public string Narration { get; set; }
            public string autokey { get; set; }
            public string OB { get; set; }
            public int? VoucherCurrencyId { get; set; }
            public decimal? VoucherExchangeRate { get; set; }
            public int BaseCurrency { get; set; }
            public decimal? BaseDebit { get; set; }
            public decimal? BaseCredit { get; set; }
            public bool CONTROLACCOUNT { get; set; }
            public bool? IsPosted { get; set; }
            public bool? IsCancelled { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? FiscalYearId { get; set; }
            public decimal? ToDebit { get; set; }
            public decimal? ToCredit { get; set; }
        }
        public class v_mnl_BalanceSheet_Result
        {
            public string AccountTypeId { get; set; }
            public string AccountTypeName { get; set; }
            public string Title { get; set; }
            public string PrintableTitle { get; set; }
            public string AccountId { get; set; }
            public decimal? Amount { get; set; }
            public string ParentId { get; set; }
        }
        public class v_mnl_IncomeStatement_Result
        {
            public Int16? AccountTypeId { get; set; }
            public string AccountTypeName { get; set; }
            public string Title { get; set; }
            public string PrintableTitle { get; set; }
            public string AccountId { get; set; }
            public decimal? Amount1 { get; set; }
            public decimal? Amount2 { get; set; }
            public string AccountPrintableName { get; set; }
        }
        public class v_mnl_VoucherDetails_Result
        {
            public long VoucherId { get; set; }
            public string VoucherType { get; set; }
            public string VoucherName { get; set; }
            public DateTime TransactionDate { get; set; }
            public decimal DebitAmount { get; set; }
            public decimal CreditAmount { get; set; }
            public decimal? Balance { get; set; }
            public DateTime CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public string Particulars { get; set; }
            public string VoucherStatus { get; set; }
            public bool IsApproved { get; set; }
            public int? ApprovedBy { get; set; }
            public bool IsPosted { get; set; }
            public int? PostedBy { get; set; }
            public bool IsCancelled { get; set; }
            public int? CancelledBy { get; set; }
            public short? CurrencyId { get; set; }
            public decimal? ExchangeRate { get; set; }
            public short BranchId { get; set; }
            public string CBAccountId { get; set; }
            public int? ProjectId { get; set; }
            public string IP { get; set; }
            public DateTime? PostedOn { get; set; }
            public DateTime? CancelledOn { get; set; }
            public int FiscalYearId { get; set; }
            public string TITLE { get; set; }
            public long VoucherDetailId { get; set; }
            public short TransactionId { get; set; }
            public long AccountId { get; set; }
            public char TransactionType { get; set; }
            public decimal Debit { get; set; }
            public decimal Credit { get; set; }
            public string ChequeNo { get; set; }
            public DateTime? ChequeDate { get; set; }
            public DateTime? ChequeClearDate { get; set; }
            public string Narration { get; set; }
            public int? CostGroupId { get; set; }
        }
        public static List<v_mnl_TranferMethodsTypes_Result> p_mnl_POSTranferMethods(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_TranferMethodsTypes_Result>("SELECT distinct TransferMethod from AM.WarehouseProducts").ToList();
            return result;
        }
        public class v_mnl_TranferMethodsTypes_Result
        {
            public string TransferType { get; set; }
            public string TransferMethod { get; set; }
        }

        public static List<v_mnl_TranferMethodsTypes_Result> p_mnl_TranferTypes(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_TranferMethodsTypes_Result>("SELECT distinct TransferType from AM.WarehouseProducts").ToList();
            return result;
        }

        public static List<v_mnl_TranferMethodsTypes_Result> p_mnl_POSTranferTypes(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_TranferMethodsTypes_Result>("SELECT distinct TransferType from AM.WarehouseProducts").ToList();
            return result;
        }
        public static List<v_mnl_Openningstock_Result> p_mnl_POSOpenningstock(OneDbContext db, int? WarehouseId, DateTime? FromDate, DateTime? ToDate)
        {
            var result = db.Database.SqlQuery<v_mnl_Openningstock_Result>($@"
         select 
        ItemId,ShortName,ItemName,CategoryId,CategoryName,WarehouseName,
        SalePrice,CostPrice,
        sum(OS)OS,sum(Pur)Pur,sum(PR)PR,sum(Sale)Sale,sum(SR)SR,Sum(Damage)Damage
        from
	        (SELECT 
	        i.ItemId,i.ShortName, i.ItemName, i.CategoryId,
	        c.CategoryName,w.warehouseName,ISNULL(wp.StockPrice,0) CostPrice,isnull(wp.StockValue,0)SalePrice,
	        case when wp.TransferMethod='Opening Stock' then sum(isnull(wp.Quantity,0)) else 0 end as OS, 	
	        case when wp.TransferMethod='Purchase' then sum(isnull(wp.Quantity,0)) else 0 end as Pur, 			
	        case when wp.TransferMethod='PurchaseReturn' then sum(isnull(wp.Quantity,0)) else 0 end as PR, 
	        case when wp.TransferMethod='Sale' then sum(isnull(wp.Quantity,0)) else 0 end as Sale,		
	        case when wp.TransferMethod='Sale Return' then sum(isnull(wp.Quantity,0)) else 0 end as SR, 		
	        case when wp.TransferMethod='Damage' then sum(isnull(wp.Quantity,0)) else 0 end as Damage 
	        FROM am.WarehouseProducts wp
	        join am.Items i on i.ItemId=wp.ItemId
	        join am.Categories c on c.CategoryId=i.CategoryId
	        join am.Warehouses w on w.WarehouseId=wp.WarehouseId
	        where 
	        wp.WarehouseId='{WarehouseId}' And
	        wp.BranchId='{SessionHelper.BranchId}' And
	        wp.TransferDate between cast('{FromDate.ToddMMMyyyy()}' as date) and cast('{ToDate.ToddMMMyyyy()}' as date)
	        group by 
	        i.ItemId,i.ShortName, i.ItemName, i.CategoryId,
	        wp.StockPrice,wp.StockValue, c.CategoryName,wp.TransferMethod,w.WarehouseName)t1
        group by 
        ItemId,ShortName,ItemName,CategoryId,SalePrice,CategoryName,WarehouseName, 
        SalePrice,CostPrice"
            , new SqlParameter("@WarehouseId", GetDBNullOrValue(WarehouseId))
            , new SqlParameter("@FromDate", GetDBNullOrValue(FromDate))
            , new SqlParameter("@ToDate", GetDBNullOrValue(ToDate))).ToList();
            return result;
        }

        public static List<p_mnl_dbo_Account__Search_Result> p_mnl_AllAccounts(OneDbContext db, long? BranchId)
        {
            var result = db.Database.SqlQuery<p_mnl_dbo_Account__Search_Result>("SELECT * FROM (" + v__Accounts + ") as v where ACCOUNT_ID <> 0 and BranchId=@BranchId order by ob",
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId))
                ).ToList();
            return result;
        }

        public static List<p_mnl__OpeningStock_Result> p_mnl__POSOpeningStock(OneDbContext db, int? WareHouseId, short? BranchId)
        {
            var result = db.Database.SqlQuery<p_mnl__OpeningStock_Result>("INSERT INTO AM.OpeningStocks (ProductId, BranchId, WareHouseId, Stock) SELECT pr.ItemId, pr.BranchId, @warehouseid, 0 FROM AM.Items as pr  WHERE  pr.BranchId = @BranchId  EXCEPT SELECT op.ProductId, op.BranchId, op.warehouseid, 0  FROM AM.OpeningStocks as op  WHERE op.BranchId = @BranchId and op.warehouseid = @warehouseid  SELECT op.OpeningStockId, op.ProductId, op.Stock, op.WareHouseId, op.BranchId, op.CreatedOn, op.CreatedBy, op.ModifiedOn, op.ModifiedBy, pr.ItemName,pr.ShortName FROM AM.OpeningStocks AS op INNER JOIN AM.Items AS pr ON op.ProductId = pr.ItemId  WHERE op.BranchId = @BranchId and op.warehouseid = @warehouseid",
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId)),
                         new SqlParameter("@WareHouseId", GetDBNullOrValue(WareHouseId))
                         ).ToList();
            return result;
        }
        public static string InsertNewAccount(OneDbContext db, string ParentAccountId, string TITLE, string Type, string Group)
        {
            //id is parent accoount
            if (string.IsNullOrEmpty(ParentAccountId))
            {
                return null;
            }

            Account _Account = new Account();
            long? newAcc = GetNewAccoundID(db, ParentAccountId);
            Account _parentAccount = db.Accounts.Where(u => u.autokey == ParentAccountId).FirstOrDefault();

            if (newAcc != null)
            {
                if (_parentAccount != null)
                {
                    Guid gu = Guid.NewGuid();
                    _Account.ParentAccountId = _parentAccount.autokey;
                    _Account.ParentId = _parentAccount.ACCOUNT_ID;
                    _Account.BranchId = branch_ID;
                    _Account.autokey = gu.ToString();
                    _Account.ACCOUNT_ID = Convert.ToInt64(newAcc);
                    _Account.TITLE = TITLE;
                    _Account.DEPTH = GetAccountDepth(db, _parentAccount, ParentAccountId);
                    _Account.LINEAGE = GetAccountLineage(db, _parentAccount, ParentAccountId);

                    if (Type == "Control")
                    {
                        _Account.CONTROLACCOUNT = true;
                        _Account.ISTRANSACTION = false;
                    }
                    else
                    {
                        _Account.CONTROLACCOUNT = false;
                        _Account.ISTRANSACTION = true;
                    }
                    if (Group == "BalanceSheet")
                    {
                        _Account.GroupNo = 1;
                    }
                    else
                    {
                        _Account.GroupNo = 0;
                    }

                    db.Accounts.Add(_Account);
                    db.SaveChanges();
                    return _Account.autokey;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static string GetAccountLineage(OneDbContext db, Account _parentAccount, string autoKey)
        {
            string lineage = null;
            if (_parentAccount != null)
            {
                lineage = _parentAccount.LINEAGE + _parentAccount.ACCOUNT_ID.ToString() + "/";
            }
            return lineage;
        }

        public static long? GetNewAccoundID(OneDbContext db, string autoKey)
        {
            long? newAcc = null;
            if (string.IsNullOrEmpty(autoKey))
            {
                return newAcc;
            }

            Account _parentAccount = db.Accounts.Where(u => u.autokey == autoKey).FirstOrDefault();
            if (_parentAccount != null)
            {
                newAcc = db.Accounts.Where(u => u.ParentId == _parentAccount.ACCOUNT_ID && u.BranchId == branch_ID).Max(u => (long?)u.ACCOUNT_ID);

                if (newAcc == null)
                {
                    newAcc = Convert.ToInt64(_parentAccount.ACCOUNT_ID.ToString() + "0001");
                }
                else
                {
                    newAcc++;
                }
            }
            return newAcc;
        }

        public static short? GetAccountDepth(OneDbContext db, Account _parentAccount, string autoKey)
        {
            short? Depth = null;
            if (_parentAccount != null)
            {
                _parentAccount.DEPTH++;
                Depth = _parentAccount.DEPTH;
            }
            return Depth;
        }
        public static List<v_mnl_SalesReports_Result> v_mnl_SaleReportByPOSProducts(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_SalesReports_Result>("SELECT AM.SaleInvoices.BranchId, AM.Products.ProductId,AM.Products.CategoryId, AM.Categories.CategoryName, AM.Products.ProductName ,AM.SaleInvoices.SaleInvoiceDate, AM.SaleInvoiceProducts.Quantity,AM.SaleInvoiceProducts.Discount , AM.SaleInvoiceProducts.UnitPrice, AM.SaleInvoiceProducts.LineTotal, AM.SaleInvoices.SaleInvoiceId, AM.SaleInvoiceProducts.NetTotal,AM.SaleInvoices.NetTotal as InvoiceTotal FROM AM.Products INNER JOIN AM.SaleInvoiceProducts ON AM.Products.ProductId = AM.SaleInvoiceProducts.ProductId INNER JOIN AM.SaleInvoices ON AM.SaleInvoiceProducts.SaleInvoiceId =  AM.SaleInvoices.SaleInvoiceId INNER JOIN AM.Categories ON AM.Products.CategoryId = AM.Categories.CategoryId").ToList();
            return result;
        }
        public static string tPOSClientRefundInvoice(OneDbContext db, Int64? InvoiceId, short? BranchId)
        {
            var result = db.Database.ExecuteSqlCommand("if @InvoiceId is not null begin Update AM.SaleReturns set Received = (select ISNULL(SUM(Amount), 0) from AM.ClientRefundInvoices where BranchId = @BranchId and SaleReturnId = @InvoiceId) where BranchId = @BranchId and SaleReturnId = @InvoiceId end", new SqlParameter("@InvoiceId", GetDBNullOrValue(InvoiceId)),
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId))).ToString();
            return result;
        }
        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
            {
                return Source;
            }

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }
        public static string p_mnl_UnPostPOSSaleInvoicesToAccount(OneDbContext db, int? UserId, string InvoiceIds)
        {
            var result = db.Database.ExecuteSqlCommand("update Finance.Vouchers set VoucherStatus='Draft', IsPosted=0, PostedBy=null, PostedOn=null, ModifiedBy=@ModifiedBy, Modifiedon=@Modifiedon where VoucherId in(select VoucherId from AM.SaleReturns where SaleInvoiceId in (" + InvoiceIds + ") and IsCancelled = 0 and IsPosted = 1 Union select InventoryVoucherId from AM.SaleReturns where SaleInvoiceId in (" + InvoiceIds + ") and IsCancelled = 0 and IsPosted = 1) update AM.SaleReturns set IsAccountPosted = 0, AccountPostedBy = null, AccountPostedOn = null, ModifiedBy = @ModifiedBy, Modifiedon = @Modifiedon where SaleInvoiceId in(" + InvoiceIds + ") and IsCancelled = 0 and IsPosted = 1",
                new SqlParameter("@ModifiedBy", GetDBNullOrValue(UserId)),
                new SqlParameter("@Modifiedon", GetDBNullOrValue(DateTime.Now))
                ).ToString();
            return result;
        }
        public static List<v_mnl_SaleReturns_Result> v_mnl_POSSaleReturns(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_SaleReturns_Result>("SELECT si.SaleReturnId, si.ClientId, si.SaleReturnDate, si.CurrencyId, si.ExchangeRate, si.DealingPerson, si.Description, si.IsPosted, si.PostedBy, si.PostedOn, si.IsCancelled, si.CancelledOn, si.CancelledBy, si.Discount, si.ModifiedBy, si.ModifiedOn, si.CreatedOn, si.CreatedBy, si.Version, si.VoucherId, si.TotalAmount, si.LabourCharges, si.OtherCharges, si.FareCharges, ISNULL(si.NetTotal, 0) as NetTotal, si.Received, si.BranchId, si.IsApplyTax, ISNULL(si.Received,0) as ReceivedAmount,                        Membership.[Users].Username AS PostedName, User_2.Username AS CancelledName, User_1.Username AS ModifiedName,                          User_3.Username AS CreatedName, cl.Name AS ClientName, convert(decimal(18,2),0) as Paid FROM            AM.SaleReturns AS si LEFT OUTER JOIN                          Membership.[Users] AS User_3 ON si.CreatedBy = User_3.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_2 ON si.CancelledBy = User_2.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_1 ON si.ModifiedBy = User_1.UserID LEFT OUTER JOIN                          Membership.[Users] ON si.PostedBy = Membership.[Users].UserID LEFT OUTER JOIN                          Client.Clients AS cl ON si.ClientId = cl.ClientId").ToList();
            return result;
        }
        public static string p_mnl_PostUnPostPOSSaleReturns(OneDbContext db, string Command, int? UserId, string InvoiceIds, DateTime? Date)
        {
            var result = db.Database.ExecuteSqlCommand("if (@Command = 'Post') BEGIN update AM.SaleReturns set IsPosted = 1, PostedBy = @UserId, PostedOn = @Date where SaleReturnId in(" + InvoiceIds + ") and IsCancelled=0 and IsPosted=0 	Insert into AM.WarehouseProducts (WarehouseId,ProductId,Quantity,TransferDate,TransferType,StockPrice,StockValue,TransferMethod,InvoiceId) SELECT        sid.WareHouseId, sid.ProductId, sid.SqFeet, GETDATE() AS Expr1, 'In' AS Expr2, sid.UnitPrice, p.CostPrice, 'SaleReturn' AS Expr3, sid.SaleReturnId FROM            AM.SaleReturnProducts AS sid INNER JOIN AM.Products AS p ON sid.ProductId = p.ProductId INNER JOIN AM.SaleReturns AS si ON sid.SaleReturnId = si.SaleReturnId WHERE(si.SaleReturnId IN(" + InvoiceIds + ") and si.IsCancelled = 0 and si.IsPosted = 1) END if (@Command = 'Unpost') BEGIN update AM.SaleReturns set IsPosted = 0, PostedBy = null, PostedOn = null where SaleReturnId in(" + InvoiceIds + ") and IsCancelled=0 and IsPosted=1 delete from AM.WarehouseProducts where TransferMethod='SaleReturn' and InvoiceId in(" + InvoiceIds + ")  END if (@Command = 'Cancel') BEGIN update AM.SaleReturns set IsCancelled = 1, CancelledBy = @UserId, CancelledOn = @Date where SaleReturnId in(" + InvoiceIds + ") and IsPosted=0 END",
                new SqlParameter("@Command", GetDBNullOrValue(Command)),
                new SqlParameter("@UserId", GetDBNullOrValue(UserId)),
                new SqlParameter("@Date", GetDBNullOrValue(Date))
                ).ToString();
            return result;
        }
        public static string p_mnl_PostUnPostPOSSaleInvoices(OneDbContext db, string Command, int? UserId, string InvoiceIds, DateTime? Date)
        {
            var result = db.Database.ExecuteSqlCommand("if (@Command = 'Post') BEGIN update AM.SaleReturns set IsPosted = 1, PostedBy = @UserId, PostedOn = @Date where SaleInvoiceId in(" + InvoiceIds + ") and IsCancelled=0 and IsPosted=0 and IsAccountPosted=0 	Insert into AM.WarehouseProducts (WarehouseId,ProductId,Quantity,TransferDate,TransferType,StockPrice,StockValue,TransferMethod,InvoiceId) SELECT        sid.WareHouseId, sid.ProductId, sid.Quantity, GETDATE() AS Expr1, 'Out' AS Expr2, sid.UnitPrice, p.CostPrice, 'Sale' AS Expr3, sid.SaleInvoiceId FROM            AM.SaleInvoiceProducts AS sid INNER JOIN AM.Products AS p ON sid.ProductId = p.ProductId INNER JOIN AM.SaleReturnsAS si ON sid.SaleInvoiceId = si.SaleInvoiceId WHERE(si.SaleInvoiceId IN(" + InvoiceIds + ") and si.IsCancelled = 0 and si.IsPosted = 1 and si.IsAccountPosted=0) END if (@Command = 'Unpost') BEGIN update AM.SaleReturns set IsPosted = 0, PostedBy = null, PostedOn = null where SaleInvoiceId in(" + InvoiceIds + ") and IsCancelled=0 and IsPosted=1 and IsAccountPosted=0 delete wp from AM.WarehouseProducts as wp inner join AM.SaleReturns as sinv on sinv.SaleInvoiceId= InvoiceId where TransferMethod = 'Sale' and sinv.SaleInvoiceId in(" + InvoiceIds + ") and IsCancelled = 0 and IsPosted = 1  and IsAccountPosted = 0  END if (@Command = 'Cancel') BEGIN update AM.SaleReturns set IsCancelled = 1, CancelledBy = @UserId, CancelledOn = @Date where SaleInvoiceId in(" + InvoiceIds + ") and IsPosted=0 and IsAccountPosted=0 END",
                new SqlParameter("@Command", GetDBNullOrValue(Command)),
                new SqlParameter("@UserId", GetDBNullOrValue(UserId)),
                new SqlParameter("@Date", GetDBNullOrValue(Date))
                ).ToString();
            return result;
        }
        public static List<v_mnl_SaleInvoices_Result> v_mnl_POSSaleInvoices(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_SaleInvoices_Result>("SELECT        si.SaleInvoiceId, si.ClientId, si.SaleInvoiceDate, si.CurrencyId, si.ExchangeRate, si.DealingPerson, si.Description, si.IsPosted, si.PostedBy, si.PostedOn, si.IsCancelled, si.CancelledOn, si.CancelledBy, si.Discount,                        si.ModifiedBy, si.ModifiedOn, si.CreatedOn, si.CreatedBy, si.Version, si.VoucherId, si.TotalAmount, si.LabourCharges, si.OtherCharges, si.FareCharges, ISNULL(si.NetTotal, 0) as NetTotal, si.Received, si.BranchId, si.IsApplyTax, ISNULL(si.Received,0) as ReceivedAmount,                        Membership.[Users].Username AS PostedName, User_2.Username AS CancelledName, User_1.Username AS ModifiedName,                          User_3.Username AS CreatedName, cl.Name AS ClientName, convert(decimal(18,2),0) as Paid,IsAccountPosted,User_4.Username as AccountPostedName FROM            AM.SaleInvoices AS si LEFT OUTER JOIN Membership.[Users] AS User_4 ON si.AccountPostedBy = User_4.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_3 ON si.CreatedBy = User_3.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_2 ON si.CancelledBy = User_2.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_1 ON si.ModifiedBy = User_1.UserID LEFT OUTER JOIN                          Membership.[Users] ON si.PostedBy = Membership.[Users].UserID LEFT OUTER JOIN                          Client.Clients AS cl ON si.ClientId = cl.ClientId").ToList();
            return result;
        }
        public static List<v_mnl_PurchaseInvoiceProducts_Result> v_mnl_POSPurchaseInvoiceProducts(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_PurchaseInvoiceProducts_Result>("SELECT        Inv.BranchId, AM.Products.ProductId, AM.Products.CategoryId, AM.Categories.CategoryName, AM.Products.ProductName, Inv.PurchaseInvoiceDate, det.Quantity, det.Discount, det.UnitPrice, det.LineTotal, Inv.PurchaseInvoiceId, det.NetTotal, Inv.NetTotal AS InvoiceTotal FROM            AM.Products INNER JOIN  AM.PurchaseInvoiceProducts AS det ON AM.Products.ProductId = det.ProductId INNER JOIN  AM.PurchaseInvoices AS Inv ON det.PurchaseInvoiceId = Inv.PurchaseInvoiceId INNER JOIN AM.Categories ON AM.Products.CategoryId = AM.Categories.CategoryId").ToList();
            return result;
        }
        public static List<v_mnl_PurchaseInvoices_Result> v_mnl_POSPurchaseInvoices(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_PurchaseInvoices_Result>("SELECT pu.PurchaseInvoiceId, pu.PurchaseOrderId, pu.PurchaseInvoiceDate, pu.SupplierId, pu.Discount, pu.Description, pu.IsPosted, pu.PostedOn, pu.PostedBy, pu.IsCancelled, pu.CancelledOn, pu.CancelledBy, pu.CreatedOn, pu.CreatedBy, pu.ModifiedOn, pu.ModifiedBy, pu.CurrencyId, pu.ExchangeRate, pu.MovedToWarehouse, pu.VoucherId, pu.TotalAmount, pu.LabourCharges, pu.OtherCharges, pu.FareCharges, ISNULL(pu.NetTotal, 0) AS NetTotal, pu.BranchId, pu.Version, pu.IsApplyTax, ISNULL(pu.Received,0) AS ReceivedAmount, Membership.[Users].Username AS PostedName, User_2.Username AS CancelledName, User_1.Username AS ModifiedName, User_3.Username AS CreatedName, cl.Name AS ClientName, convert(decimal(18,2),0) as Paid, pu.IsAccountPosted, pu.AccountPostedOn, pu.AccountPostedBy, User_5.Username AS AccountPostedName,pu.IsCreatedFromOpenningStock FROM AM.PurchaseInvoices AS pu LEFT OUTER JOIN Membership.[Users] as User_5  ON pu.AccountPostedBy = User_5.UserID LEFT OUTER JOIN Membership.[Users] AS User_3 ON pu.CreatedBy = User_3.UserID LEFT OUTER JOIN      Membership.[Users] AS User_2 ON pu.CancelledBy = User_2.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_1 ON pu.ModifiedBy = User_1.UserID LEFT OUTER JOIN                          Membership.[Users] ON pu.PostedBy = Membership.[Users].UserID LEFT OUTER JOIN                          Client.Clients AS cl ON pu.SupplierId = cl.ClientId").ToList();
            return result;
        }

        public static List<v_mnl_Damages_Result> v_mnl_POSDamages(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_Damages_Result>("SELECT si.DamageId, si.DamageDate, si.Description, si.IsPosted, si.PostedBy, si.PostedOn, si.IsCancelled, si.CancelledOn, si.CancelledBy, si.ModifiedBy, si.ModifiedOn, si.CreatedOn, si.CreatedBy, si.VoucherId, si.TotalAmount, si.BranchId, Membership.[Users].Username AS PostedName, User_2.Username AS CancelledName, User_1.Username AS ModifiedName,                          User_3.Username AS CreatedName, convert(decimal(18,2),0) as Paid FROM            AM.Damages AS si LEFT OUTER JOIN                          Membership.[Users] AS User_3 ON si.CreatedBy = User_3.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_2 ON si.CancelledBy = User_2.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_1 ON si.ModifiedBy = User_1.UserID LEFT OUTER JOIN                          Membership.[Users] ON si.PostedBy = Membership.[Users].UserID ").ToList();
            return result;
        }
        public static string p_mnl_PostUnPostPOSDamages(OneDbContext db, string Command, int? UserId, string InvoiceIds, DateTime? Date)
        {
            var result = db.Database.ExecuteSqlCommand("if (@Command = 'Post') BEGIN update AM.Damages set IsPosted = 1, PostedBy = @UserId, PostedOn = @Date where DamageId in(" + InvoiceIds + ") and IsCancelled=0 and IsPosted=0 	Insert into AM.WarehouseProducts (WarehouseId,ProductId,Quantity,TransferDate,TransferType,StockPrice,StockValue,TransferMethod,InvoiceId) SELECT        sid.WareHouseId, sid.ProductId, sid.Quantity, GETDATE() AS Expr1, 'Out' AS Expr2, sid.UnitPrice, p.CostPrice, 'Damage' AS Expr3, sid.DamageId FROM            AM.DamageProducts AS sid INNER JOIN AM.Products AS p ON sid.ProductId = p.ProductId INNER JOIN AM.Damages AS si ON sid.DamageId = si.DamageId WHERE(si.DamageId IN(" + InvoiceIds + ") and si.IsCancelled = 0 and si.IsPosted = 1) END if (@Command = 'Unpost') BEGIN update AM.Damages set IsPosted = 0, PostedBy = null, PostedOn = null where DamageId in(" + InvoiceIds + ") and IsCancelled=0 and IsPosted=1 delete from AM.WarehouseProducts where TransferMethod='Damage' and InvoiceId in(" + InvoiceIds + ")  END if (@Command = 'Cancel') BEGIN update AM.Damages set IsCancelled = 1, CancelledBy = @UserId, CancelledOn = @Date where DamageId in(" + InvoiceIds + ") and IsPosted=0 END",
                new SqlParameter("@Command", GetDBNullOrValue(Command)),
                new SqlParameter("@UserId", GetDBNullOrValue(UserId)),
                new SqlParameter("@Date", GetDBNullOrValue(Date))
                ).ToString();
            return result;
        }
        public static string t_POSSupplierInvoicePayment(OneDbContext db, Int64? InvoiceId, short? BranchId, short? ModuleId)
        {
            var result = db.Database.ExecuteSqlCommand("if @InvoiceId is not null begin Update AM.PurchaseInvoices set Received = (select ISNULL(SUM(Amount), 0) from Client.SupplierInvoicePayments where BranchId = @BranchId and PurchaseInvoiceId = @InvoiceId and ModuleId=@ModuleId) where BranchId = @BranchId and PurchaseInvoiceId = @InvoiceId end",
                         new SqlParameter("@InvoiceId", GetDBNullOrValue(InvoiceId)),
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId)),
                         new SqlParameter("@ModuleId", GetDBNullOrValue(ModuleId))).ToString();
            return result;
        }
        public static string p_mnl_PostUnPostAMPurchaseInvoices(OneDbContext db, string Command, int? UserId, string InvoiceIds, DateTime? Date)
        {
            var result = db.Database.ExecuteSqlCommand($@"
if (@Command = 'Post') 
BEGIN 
    update AM.PurchaseInvoices  
    set IsPosted=1, 
    PostedBy=@UserId, PostedOn=@Date 
    where PurchaseInvoiceId in({ InvoiceIds }) 
    and IsCancelled=0 and IsPosted=0  
    and IsAccountPosted=0 
    and BranchId = { branch_ID } 

    Insert into AM.WarehouseProducts(WarehouseId, ItemId, Quantity, TransferDate
    , TransferType, StockPrice, StockValue,TransferMethod, InvoiceId, BranchId) 
    SELECT sid.WareHouseId, sid.ItemId, pipd.Qty, si.PurchaseInvoiceDate
        , 'In' AS TransferType, sid.UnitPrice, p.Price AS CostPrice, 
        'Purchase' AS TransferMethod, sid.PurchaseInvoiceId,si.BranchId
	FROM AM.PurchaseInvoiceProducts AS sid 
	INNER JOIN AM.PurchaseInvoices AS si ON sid.PurchaseInvoiceId = si.PurchaseInvoiceId 
	INNER JOIN AM.PurchaseInvoiceProductDetails as pipd ON sid.PurchaseInvoiceProductId = pipd.PurchaseInvoiceProductId 
	INNER JOIN AM.Items AS p ON pipd.ItemId = p.ItemId
	WHERE (si.PurchaseInvoiceId IN ({ InvoiceIds })) 
	AND (si.IsCancelled = 0) 
	AND (si.IsPosted = 1) 
	AND (si.IsAccountPosted = 0) 
	AND (si.BranchId = { branch_ID })
END
if (@Command = 'Unpost') 
BEGIN 
	update AM.PurchaseInvoices  set IsPosted=0, PostedBy=null, PostedOn=null 
	where PurchaseInvoiceId in({ InvoiceIds }) and IsCancelled=0 and IsPosted=1  and IsAccountPosted=0 and BranchId = { branch_ID }   
	delete wp from AM.WarehouseProducts as wp inner join AM.PurchaseInvoices as pinv on pinv.PurchaseInvoiceId= InvoiceId 
	where TransferMethod = 'Purchase' and pinv.PurchaseInvoiceId in({ InvoiceIds }) and IsCancelled = 0 and IsPosted = 0  
	and IsAccountPosted = 0   and pinv.BranchId = { branch_ID } 
END 
if (@Command = 'Cancel') 
BEGIN 
	update AM.PurchaseInvoices  set IsCancelled=1, CancelledBy=@UserId, CancelledOn=@Date 
	where PurchaseInvoiceId in({ InvoiceIds }) and IsPosted=0  and IsAccountPosted=0 and BranchId = { branch_ID } 
	delete wp from AM.WarehouseProducts as wp inner join AM.PurchaseInvoices as pinv on pinv.PurchaseInvoiceId= InvoiceId 
	where TransferMethod = 'Purchase' and pinv.PurchaseInvoiceId in({ InvoiceIds }) and IsCancelled = 1 and IsPosted = 0  
	and IsAccountPosted = 0   and pinv.BranchId = { branch_ID } 
END",
                new SqlParameter("@Command", GetDBNullOrValue(Command)),
                new SqlParameter("@UserId", GetDBNullOrValue(UserId)),
                new SqlParameter("@Date", GetDBNullOrValue(Date))
                ).ToString();
            return result;
        }
        public static List<v_mnl_PurchaseReturns_Result> v_mnl_POSPurchaseReturns(OneDbContext db)
        {
            var result = db.Database.SqlQuery<v_mnl_PurchaseReturns_Result>("SELECT pu.PurchaseReturnId, pu.PurchaseReturnDate, pu.SupplierId, pu.Discount, pu.Description, pu.IsPosted, pu.PostedOn, pu.PostedBy, pu.IsCancelled, pu.CancelledOn, pu.CancelledBy, pu.CreatedOn,                          pu.CreatedBy, pu.ModifiedOn, pu.ModifiedBy, pu.CurrencyId, pu.ExchangeRate, pu.VoucherId, pu.TotalAmount, pu.LabourCharges, pu.OtherCharges, pu.FareCharges, ISNULL(pu.NetTotal, 0) AS NetTotal, pu.BranchId, pu.Version, pu.IsApplyTax, ISNULL(pu.Received,0) AS ReceivedAmount, Membership.[Users].Username AS PostedName, User_2.Username AS CancelledName, User_1.Username AS ModifiedName, User_3.Username AS CreatedName, cl.Name AS ClientName, convert(decimal(18,2),0) as Paid FROM AM.PurchaseReturns AS pu LEFT OUTER JOIN Membership.[Users] AS User_3 ON pu.CreatedBy = User_3.UserID LEFT OUTER JOIN      Membership.[Users] AS User_2 ON pu.CancelledBy = User_2.UserID LEFT OUTER JOIN                          Membership.[Users] AS User_1 ON pu.ModifiedBy = User_1.UserID LEFT OUTER JOIN                          Membership.[Users] ON pu.PostedBy = Membership.[Users].UserID LEFT OUTER JOIN                          Client.Clients AS cl ON pu.SupplierId = cl.ClientId").ToList();
            return result;
        }
        public static string p_mnl_PostUnPostPOSPurchaseReturns(OneDbContext db, string Command, int? UserId, string InvoiceIds, DateTime? Date)
        {
            var result = db.Database.ExecuteSqlCommand(
            $@"if (@Command = 'Post')
                BEGIN
                update AM.PurchaseReturns
                set
                IsPosted = 1, PostedBy = @UserId,
                PostedOn = @Date
                where PurchaseReturnId in({ InvoiceIds })
                and IsCancelled = 0
                and IsPosted = 0

                Insert into AM.WarehouseProducts
                (WarehouseId, ItemId, Quantity, TransferDate, TransferType, StockPrice
                , StockValue, TransferMethod, InvoiceId
                , BranchId, CreatedOn, CreatedBy, UserLogId)
                SELECT
                sid.WareHouseId, sid.ItemId, sid.Quantity, si.PurchaseReturnDate AS Expr1, 'Out' AS Expr2,
                sid.UnitPrice, p.Price, 'PurchaseReturn' AS Expr3, sid.PurchaseReturnId
                ,'{SessionHelper.BranchId}',GETDATE(),@UserId,'{SessionHelper.UserLogId}'
                FROM AM.PurchaseReturnProducts AS sid
                INNER JOIN AM.Items AS p ON sid.ItemId = p.ItemId
                INNER JOIN AM.PurchaseReturns AS si ON sid.PurchaseReturnId = si.PurchaseReturnId
                WHERE(si.PurchaseReturnId IN({ InvoiceIds })
                and si.IsCancelled = 0
                and si.IsPosted = 1)
                END
                if (@Command = 'Unpost')
                BEGIN
                update AM.PurchaseReturns
                set
                IsPosted = 0, PostedBy = null, PostedOn = null
                where PurchaseReturnId in({ InvoiceIds })
                and IsCancelled = 0
                and IsPosted = 1

                delete
                from AM.WarehouseProducts
                where TransferMethod = 'Purchase Return'
                and InvoiceId in({ InvoiceIds })
                END

                if (@Command = 'Cancel')
                BEGIN
                update AM.PurchaseReturns
                set
                IsCancelled = 0, CancelledBy = @UserId,
                CancelledOn = @Date
                where PurchaseReturnId in({ InvoiceIds })
                and IsPosted = 0
                END",
            new SqlParameter("@Command", GetDBNullOrValue(Command)),
            new SqlParameter("@UserId", GetDBNullOrValue(UserId)),
            new SqlParameter("@Date", GetDBNullOrValue(Date))
            ).ToString();
            return result;
        }
        public static string p_mnl_UnPostPOSPurchaseInvoicesToAccount(OneDbContext db, int? UserId, string InvoiceIds)
        {
            var result = db.Database.ExecuteSqlCommand("update Finance.Vouchers  set IsPosted=0,VoucherStatus='Draft', PostedBy=null, PostedOn=null, ModifiedBy=@ModifiedBy, Modifiedon=@Modifiedon where VoucherId in(select VoucherId from AM.PurchaseInvoices where PurchaseInvoiceId in (" + InvoiceIds + ") and IsCancelled = 0 and IsPosted = 1) update AM.PurchaseInvoices set IsAccountPosted = 0, AccountPostedBy = null, AccountPostedOn = null, ModifiedBy = @ModifiedBy, Modifiedon = @Modifiedon where PurchaseInvoiceId in(" + InvoiceIds + ") and IsCancelled = 0 and IsPosted = 1",
                new SqlParameter("@ModifiedBy", GetDBNullOrValue(UserId)),
                new SqlParameter("@Modifiedon", GetDBNullOrValue(DateTime.Now))
                ).ToString();
            return result;
        }

        public static string t_POSSupplierRefundInvoice(OneDbContext db, Int64? InvoiceId, short? BranchId)
        {
            var result = db.Database.ExecuteSqlCommand("if @InvoiceId is not null begin Update AM.PurchaseReturns set Received = (select ISNULL(SUM(Amount), 0) from AM.SupplierRefundInvoices where BranchId = @BranchId and PurchaseReturnId = @InvoiceId) where BranchId = @BranchId and PurchaseReturnId = @InvoiceId end", new SqlParameter("@InvoiceId", GetDBNullOrValue(InvoiceId)),
                         new SqlParameter("@BranchId", GetDBNullOrValue(BranchId))).ToString();
            return result;
        }






        #region Membership
        public static void ResetSessionHelper_Branch(OneDbContext db, Branch _branch, int branchId, int userId)
        {
            ClearAMCounterSession();
            ClearPosCounterSession();
            ClearResCounterSession();
            if (_branch == null)
            {
                _branch = db.Branches.Where(u => u.BranchId == branchId).FirstOrDefault();
            }
            else
            {
                _branch = db.Branches.Where(u => u.BranchId == branchId).FirstOrDefault();
            }
            if (_branch != null)
            {
                SessionHelper.BranchId = _branch.BranchId;
                SessionHelper.BranchName = _branch.Name;
                SessionHelper.BranchCode = _branch.BranchCode;
                SessionHelper.BranchAddress = _branch.AddressLine1;
                SessionHelper.BranchPhone = _branch.PhoneNumber;
                SessionHelper.CompanyName = _branch.Info.CompanyName;
                SessionHelper.IsMasterBranch = _branch.IsMasterBranch;
                if (!string.IsNullOrEmpty(_branch.CompanyName))
                {
                    SessionHelper.CompanyName = _branch.CompanyName;
                }
                SessionHelper.NTN = _branch.NTN;
                SessionHelper.GSTN = _branch.GSTN;
                CheckAndCreateLogoImages(db, _branch, branchId);
                SessionHelper.CompanyId = _branch.SettingId;
            }
            //Academics
            //Session session = null;
            //session = (from s in db.Sessions
            //           join us in db.UserSessions on s.SessionId equals us.SessionId
            //           where us.UserId == userId && us.Default == true && us.Assigned == true && s.BranchId == SessionHelper.BranchId
            //           select s).FirstOrDefault();

            //if (session != null)
            //{
            //    SessionHelper.CurrentSession = session?.SessionName ?? string.Empty;
            //    SessionHelper.CurrentSessionId = session?.SessionId ?? Guid.Empty;
            //    SessionHelper.SessionYearEndDate = session.EndTime;
            //    SessionHelper.SessionYearStartDate = session.StartTime;

            //    var term = db.Terms
            //        .Where(s => s.SessionId == session.SessionId && s.BranchId == SessionHelper.BranchId && s.IsActive == true)
            //        .Select(s => new
            //        {
            //            TermName = s.TermName
            //        }).FirstOrDefault();

            //    SessionHelper.CurrentTerm = term?.TermName;
            //}
            //else
            //{
            //    SessionHelper.CurrentSession = "";
            //    SessionHelper.CurrentSessionId = Guid.Empty;
            //}
            //Finance
            var fiscal = db.FiscalYears.Where(u => u.Active == true && u.BranchId == branchId).OrderByDescending(u => u.FiscalYearId).FirstOrDefault();
            if (fiscal != null)
            {
                SessionHelper.FiscalYearId = fiscal.FiscalYearId;
                SessionHelper.FiscalYearName = fiscal.FiscalYearName;
                SessionHelper.FiscalStartDate = fiscal.StartDate;
                SessionHelper.FiscalEndDate = fiscal.EndDate;
            }
            //FrontDesk
            //Areas.FrontDesk.Models.FrontDeskSetting _FrontDeskSetting = db.FrontDeskSettings.Where(u => u.BranchId == branchId).FirstOrDefault();
            //if (_FrontDeskSetting != null)
            //{
            //    SessionHelper.IsRestaurantAutomation = _FrontDeskSetting.IsRestaurantAutomation;
            //    SessionHelper.GST = _FrontDeskSetting.GST;
            //    SessionHelper.KotServiceId = _FrontDeskSetting.KotServiceId;
            //    SessionHelper.RoomServiceId = _FrontDeskSetting.RoomServiceId;
            //    SessionHelper.GstAccountId_FD = _FrontDeskSetting.GstAccountId;
            //    SessionHelper.DiscountAccountId_FD = _FrontDeskSetting.DiscountAccountId;
            //    SessionHelper.CommisionAccountId = _FrontDeskSetting.CommisionAccountId;
            //    SessionHelper.IsIndividualItemAccounts_FrontDesk = _FrontDeskSetting.IsIndividualItemAccounts;
            //    if (_FrontDeskSetting.RoomService != null)
            //    {
            //        SessionHelper.TaxForRoomService = _FrontDeskSetting.RoomService.TaxPercentage;
            //    }
            //}
            //POS
            var _AccountSetting = db.AccountSettings.Where(u => u.BranchId == branchId).FirstOrDefault();
            if (_AccountSetting != null)
            {
                SessionHelper.IsIndividualItemAccounts_POS = _AccountSetting.IsIndividualItemAccounts;
                SessionHelper.GST_POS = _AccountSetting.Tax;
                SessionHelper.GstAccountId_POS = _AccountSetting.TaxAccountId;
                SessionHelper.DiscountAccountId_POS = _AccountSetting.DiscountAccountId;
                SessionHelper.CommisionAccountId_POS = _AccountSetting.CommisionAccountId;
                SessionHelper.IsHideHeaderFooterInPrint_POS = _AccountSetting.IsHideHeaderFooterInPrint;
            }
            //Res Setting
            //var ResSetting = db.ResAccountSettings.Where(u => u.BranchId == branchId).FirstOrDefault();
            //if (ResSetting != null)
            //{
            //    SessionHelper.IsIndividualItemAccounts_Res = ResSetting.IsIndividualItemAccounts;
            //    SessionHelper.GstTax_Res = ResSetting.Tax;
            //    SessionHelper.ServiceCharges_Res = ResSetting.ServiceCharges;
            //    SessionHelper.ServiceChargesAccountId_Res = ResSetting.ServiceChargesAccountId;
            //    SessionHelper.GstTaxAccountId_Res = ResSetting.TaxAccountId;
            //    SessionHelper.DiscountAccountId_Res = ResSetting.DiscountAccountId;
            //    SessionHelper.RestaurantInvoicePrintNote = ResSetting.RestaurantInvoicePrintNote;
            //}
            ////Shop Setting
            //var ShopSetting = db.ShopAccountSettings.Where(u => u.BranchId == branchId).FirstOrDefault();
            //if (ShopSetting != null)
            //{
            //    SessionHelper.IsIndividualItemAccounts_Shop = ShopSetting.IsIndividualItemAccounts;
            //    SessionHelper.GstTax_Shop = ShopSetting.Tax;
            //    SessionHelper.ServiceCharges_Shop = ShopSetting.ServiceCharges;
            //    SessionHelper.ServiceChargesAccountId_Shop = ShopSetting.ServiceChargesAccountId;
            //    SessionHelper.GstTaxAccountId_Shop = ShopSetting.TaxAccountId;
            //    SessionHelper.DiscountAccountId_Shop = ShopSetting.DiscountAccountId;
            //}
            //AM Setting
            var AMSetting = db.AMNatures.Where(u => u.BranchId == branchId).FirstOrDefault();
            if (AMSetting != null)
            {
                SessionHelper.IsIndividualItemAccounts_AM = AMSetting.CreateItemAccounts;
            }
        }
        public static string p_mnl_FillGroupRights(OneDbContext db, int? GroupId)
        {
            var result = db.Database.ExecuteSqlCommand("INSERT INTO [Membership].[GroupRights] ( [GroupId], [FormRightId],[Allowed]) SELECT @GroupId AS[GroupId] , [fr].[FormRightId],1 FROM[Membership].[FormRights] AS[fr] EXCEPT SELECT[GroupId] , [FormRightId],1 FROM[Membership].[GroupRights]",
                         new SqlParameter("@GroupId", GetDBNullOrValue(GroupId))
                ).ToString();
            return "";
        }

        public static FormAndGroupRightsViewModel GetFormAndGroupRights(OneDbContext db, int groupId, bool? allowed, string actionName, string url)
        {
            var vm = new FormAndGroupRightsViewModel();
            vm.FormRights = v_mnl_FormRights(db, groupId, allowed, actionName, url);
            vm.GroupRights = v_mnl_DashboardViews(db, groupId, true, "Can Read", url);
            return vm;
        }
        public static IQueryable<v_mnl_UserBranches_Result> v_mnl_UserBranches(OneDbContext db)
        {
            //var result = db.Database.SqlQuery<v_mnl_UserBranches_Result>("SELECT        Membership.UserBranches.UserBranchId, Membership.UserBranches.UserId, Membership.UserBranches.BranchId, Membership.UserBranches.Active, Membership.UserBranches.DefaultBranch, Company.Branches.Name FROM            Company.Branches INNER JOIN Membership.UserBranches ON Company.Branches.BranchId = Membership.UserBranches.BranchId").ToList();

            var result = db.UserBranches.Select(p => new v_mnl_UserBranches_Result
            {
                UserBranchId = p.UserBranchId,
                Active = p.Active,
                BranchId = p.BranchId,
                UserId = p.UserId,
                DefaultBranch = p.DefaultBranch,
                Name = p.Branch.Name,
                PhoneNumber = p.Branch.PhoneNumber,
                EmailAddress = p.Branch.EmailAddress,
                RegPrefix = p.Branch.RegPrefix,
                BranchCode = p.Branch.BranchCode,
                AddressLine1 = p.Branch.AddressLine1,
                BranchLogoMini = p.Branch.BranchLogoMini,
                BranchLogoSmall = p.Branch.BranchLogoSmall,
                BranchLogoLarge = p.Branch.BranchLogoLarge,
                CompanyName = p.Branch.CompanyName,
                NTN = p.Branch.NTN,
                GSTN = p.Branch.GSTN,
                IsMasterBranch = p.Branch.IsMasterBranch
            });

            return result;
        }
        public static List<v_mnl_FormRights_Result> v_mnl_FormRights(OneDbContext db, int groupId, bool? allowed, string actionName, string url)
        {
            var allowedStr = $"AND Allowed = '{ allowed }'";
            if (!allowed.HasValue)
            {
                allowedStr = "";
            }

            var actionNameStr = $"AND FormRightName = '{actionName}'";
            if (string.IsNullOrWhiteSpace(actionName))
            {
                actionNameStr = "";
            }

            var urlStr = $"AND FormURL = '{ url } '";
            if (string.IsNullOrWhiteSpace(url))
            {
                urlStr = "";
            }
            //string masterMenuCondition = string.Empty;
            //if (!SessionHelper.IsMasterBranch)
            //{
            //    masterMenuCondition = "And IsMasterMenu  != 1";
            //} 

            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"
SELECT Form.FormID
	, Form.MenuText
	, FormRights.FormRightName
	, GroupRights.GroupId
	, Form.ParentForm
	, Membership.UserGroups.UserGroupName
	, GroupRights.Allowed
	, Form.ControllerName
	, Form.PageDescription
	, Form.isActive
	, Form.FormName
	, Form.FormURL
	, FormRights.FormRightId
	, GroupRights.GroupRightId
	, Form.IsMenuItem
	, Form.MenuItemPriority
	, Form.Icon
	, Form.PageType
	, Form.ModuleId
	, Form.IsDashboardPart
FROM Membership.Forms AS Form
INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId 
WHERE Membership.UserGroups.UserGroupId = {groupId} { allowedStr } { actionNameStr } {urlStr} ").ToList();
            return result;
        }



        public static List<p_mnl_QuickLinks_Result> p_mnl_QuickLinks(OneDbContext db, int? GroupId, string URL)
        {
            var result = db.Database.SqlQuery<p_mnl_QuickLinks_Result>($@"DECLARE @ParentForm BIGINT

SET @ParentForm = (
		SELECT TOP 1 ParentForm
		FROM Membership.Forms
		WHERE FormUrl LIKE @URL
		)

IF (@ParentForm IS NULL)
BEGIN
	SELECT MenuText, FormUrl,IsDashboardPart,FaIcon
	FROM Membership.Forms AS Form
	INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
	INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
	INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
	WHERE ParentForm = 1
		AND isActive = 'Yes'
		AND Allowed = 1
		AND IsMenuItem = 1
		AND FormRightName = 'Can Read'
		AND groupid = @GroupId
	ORDER BY MenuItemPriority
END
ELSE
BEGIN
	SELECT MenuText, FormUrl, Icon,IsDashboardPart,FaIcon
	FROM Membership.Forms AS Form
	INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
	INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
	INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
	WHERE ParentForm = @ParentForm
		AND isActive = 'Yes'
		AND Allowed = 1
		AND FormRightName = 'Can Read'
		AND groupid = @GroupId
	ORDER BY MenuItemPriority
END",
                         new SqlParameter("@GroupId", GetDBNullOrValue(GroupId)),
                         new SqlParameter("@URL", GetDBNullOrValue(URL))).ToList();
            return result;
        }
        public static List<p_mnl_QuickLinks_Result> p_mnl_QuickLinks_Main_Dashboard(OneDbContext db, int? GroupId, string URL)
        {
            var result = db.Database.SqlQuery<p_mnl_QuickLinks_Result>($@"
BEGIN
	SELECT MenuText, FormUrl,IsDashboardPart,FaIcon
	FROM Membership.Forms AS Form
	INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
	INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
	INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
	WHERE ParentForm = 1
		AND isActive = 'Yes'
		AND Allowed = 1
		AND IsMenuItem = 1
		AND FormRightName = 'Can Read'
		AND groupid = @GroupId
        AND FormName not like '%Dashboard%'
	ORDER BY MenuItemPriority
END",
                         new SqlParameter("@GroupId", GetDBNullOrValue(GroupId)),
                         new SqlParameter("@URL", GetDBNullOrValue(URL))).ToList();
            return result;
        }

        public static List<ViewModels.p_mnl_DashboardMenus_Result> p_mnl_DashboardMenus(OneDbContext db, int? GroupId, string URL)
        {
            var result = db.Database.SqlQuery<p_mnl_DashboardMenus_Result>(@"DECLARE @ParentForm INT

SET @ParentForm = (
		SELECT TOP 1 ParentForm
		FROM Membership.Forms
		WHERE FormUrl LIKE @URL
		)

IF (@ParentForm IS NULL)
BEGIN
	SELECT MenuText, FormUrl, Icon,IsDashboardPart,FaIcon
	FROM Membership.Forms AS Form
	INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
	INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
	INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
	WHERE ParentForm = 1
		AND isActive = 'Yes'
		AND Allowed = 1
		AND IsMenuItem = 1
		AND FormRightName = 'Can Read'
		AND groupid = @GroupId
	ORDER BY MenuItemPriority
END
ELSE
BEGIN
	SELECT MenuText, FormUrl, Icon,IsDashboardPart,FaIcon
	FROM Membership.Forms AS Form
	INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
	INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
	INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
	WHERE ParentForm = @ParentForm
		AND isActive = 'Yes'
		AND Allowed = 1
		AND FormRightName = 'Can Read'
		AND groupid = @GroupId
	ORDER BY MenuItemPriority
END
",
                         new SqlParameter("@GroupId", GetDBNullOrValue(GroupId)),
                         new SqlParameter("@URL", GetDBNullOrValue(URL))).ToList();
            return result;
        }
        public static IQueryable<v_mnl_FormRights_Result> v_mnl_SingleFormRights(OneDbContext db, int groupId, string url)
        {
            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"
DECLARE @FormId INT
DECLARE @GroupId INT = '{groupId}'
DECLARE @FormURL VARCHAR(4000) = '{url}'

SELECT @FormId = FormId
FROM Membership.Forms
WHERE FormURL = @FormURL

SELECT Form.FormID
	, Form.MenuText
	, FormRights.FormRightName
	, GroupRights.GroupId
	, Form.ParentForm
	, Membership.UserGroups.UserGroupName
	, GroupRights.Allowed
	, Form.ControllerName
	, Form.PageDescription
	, Form.isActive
	, Form.FormName
	, Form.FormURL
	, FormRights.FormRightId
	, GroupRights.GroupRightId
	, Form.IsMenuItem
	, Form.MenuItemPriority
	, Form.Icon
	, Form.PageType
	, Form.ModuleId
	, Form.IsDashboardPart
FROM Membership.Forms AS Form
INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
WHERE (Membership.UserGroups.UserGroupId = @GroupId)
	AND Form.ParentForm = @FormId
	AND FormRightName = 'Can Read'
").AsQueryable();
            return result;
        }
        public static List<v_mnl_FormRights_Result> v_mnl_DashboardViews(OneDbContext db, int groupId, bool? allowed, string actionName, string url)
        {
            var allowedStr = $"AND Allowed = '{ allowed }'";
            if (!allowed.HasValue)
            {
                allowedStr = "";
            }

            var actionNameStr = $"AND FormRightName = '{actionName}'";
            if (string.IsNullOrWhiteSpace(actionName))
            {
                actionNameStr = "";
            }

            var urlStr = $"AND FormURL = '{ url } '";
            if (string.IsNullOrWhiteSpace(url))
            {
                urlStr = "";
            }

            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"
DECLARE @parentform bigint
SELECT TOP (1) @parentform = FormID FROM Membership.Forms WHERE 1 = 1 {urlStr}
SELECT Form.FormID
	, Form.MenuText
	, FormRights.FormRightName
	, GroupRights.GroupId
	, Form.ParentForm
	, UserGroup.UserGroupName
	, GroupRights.Allowed
	, Form.ControllerName
	, Form.PageDescription
	, Form.isActive
	, Form.FormName
	, Form.FormURL
	, FormRights.FormRightId
	, GroupRights.GroupRightId
	, Form.IsMenuItem
	, Form.MenuItemPriority
	, Form.Icon
	, Form.PageType
	, Form.ModuleId
	, Form.IsDashboardPart
,Form.IsAction
FROM Membership.Forms AS Form
INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
INNER JOIN Membership.UserGroups AS UserGroup ON GroupRights.GroupId = UserGroup.UserGroupId
WHERE Form.IsDashboardPart = 1 AND ParentForm = @parentform AND UserGroup.UserGroupId = {groupId} {actionNameStr} {allowedStr}").ToList();
            return result;
        }
        public static List<v_mnl_FormRights_Result> v_mnl_GroupFormRights(OneDbContext db, int ModuleId, int UserGroupId)
        {
            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"SELECT Form.FormID
	, Form.MenuText
	, FormRights.FormRightName
	, GroupRights.GroupId
	, Form.ParentForm
	, Membership.UserGroups.UserGroupName
	, GroupRights.Allowed
	, Form.ControllerName
	, Form.isActive
	, Form.FormName
	, Form.FormURL
	, FormRights.FormRightId
	, GroupRights.GroupRightId
	, Form.IsMenuItem
	, Form.MenuItemPriority
	, Form.Icon
	, Form.PageType
	, Form.ModuleId
FROM Membership.Forms AS Form
INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
INNER JOIN Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
WHERE Form.ModuleId = @FormID
	AND (
		isActive = 'Yes'
		OR Form.isActive = 'Active'
		)
	AND UserGroupId = @UserGroupId
ORDER BY form.formid
	, MenuItemPriority", new SqlParameter("@FormID", GetDBNullOrValue(ModuleId))
                , new SqlParameter("@UserGroupId", GetDBNullOrValue(UserGroupId))).ToList();
            return result;
        }
        public static List<v_mnl_FormRights_Result> v_mnl_GroupFormRights(OneDbContext db, int UserGroupId)
        {
            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"
declare @Modules table(ModuleId int, ModuleName varchar(50)) 
insert into @Modules(ModuleId, ModuleName)
select ModuleId, MenuText from Membership.Forms where isActive = 'Yes' and ParentForm = 1

SELECT Form.FormID, Form.MenuText, FormRights.FormRightName, GroupRights.GroupId, Form.ParentForm, Membership.UserGroups.UserGroupName,
GroupRights.Allowed, Form.ControllerName, Form.isActive, Form.FormName, Form.FormURL, FormRights.FormRightId, GroupRights.GroupRightId,
Form.IsMenuItem, Form.MenuItemPriority, Form.Icon, Form.PageType, Form.ModuleId,
(select top 1 '(' + ModuleName + ')' from @Modules as mf where mf.ModuleId = Form.ModuleId) as Module,
Form.IsDashboardPart ,Form.PageDescription,Form.IsAction
FROM Membership.Forms AS Form INNER JOIN
Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId INNER JOIN
Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId INNER JOIN
Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
where(isActive = 'Yes' or Form.isActive = 'Active') and UserGroupId = @UserGroupId
order by form.formid, MenuItemPriority"
                , new SqlParameter("@UserGroupId", GetDBNullOrValue(UserGroupId))).ToList();
            return result;
        }
        public static List<v_mnl_FormRights_Result> v_mnl_GroupFormRightsQueryable(OneDbContext db, int UserGroupId)
        {
            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"declare @Modules table(ModuleId int, ModuleName varchar(50)) 
insert into @Modules(ModuleId, ModuleName)
select ModuleId, MenuText from Membership.Forms where isActive = 'Yes' and ParentForm = 1

SELECT Form.FormID, Form.MenuText, FormRights.FormRightName, GroupRights.GroupId, Form.ParentForm, Membership.UserGroups.UserGroupName,
GroupRights.Allowed, Form.ControllerName, Form.isActive, Form.FormName, Form.FormURL, FormRights.FormRightId, GroupRights.GroupRightId,
Form.IsMenuItem, Form.MenuItemPriority, Form.Icon, Form.PageType, Form.ModuleId,
(select top 1 '(' + ModuleName + ')' from @Modules as mf where mf.ModuleId = Form.ModuleId) as Module,
Form.IsDashboardPart ,Form.PageDescription
FROM Membership.Forms AS Form INNER JOIN
Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId INNER JOIN
Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId INNER JOIN
Membership.UserGroups ON GroupRights.GroupId = Membership.UserGroups.UserGroupId
where(isActive = 'Yes' or Form.isActive = 'Active') and UserGroupId = @UserGroupId
order by form.formid, MenuItemPriority"
                , new SqlParameter("@UserGroupId", GetDBNullOrValue(UserGroupId))).ToList();
            return result;
        }
        public static int InsertInMembershipModules(OneDbContext db, Module ex)
        {
            int result = 0;
            var IfExists = db.Modules.Where(s => s.ModuleName == ex.ModuleName).Any();
            if (!IfExists)
            {
                db.Modules.Add(ex);
                result = db.SaveChanges();
            }
            return result;
        }
        public static int InsertInMembershipForms(OneDbContext db, Form ex, short BranchId)
        {
            var result = InsertInMembershipFormsWithStaticFormId(db, ex, BranchId);
            return result;
        }
        public static int InsertInMembershipFormsWithStaticFormId(OneDbContext db, Form ex, short BranchId)
        {
            int result = 0;
            var ismenutext = ex.IsMenuItem == true ? 1 : 0;
            var isdashboard = ex.IsDashboardPart == true ? 1 : 0;
            var showondesktop = ex.ShowOnDesktop == true ? 1 : 0;
            var ismaster = ex.IsMasterMenu == true ? 1 : 0;
            var isquicklink = ex.IsQuickLink == true ? 0 : 0;
            var IsAction = ex.IsAction == true ? 1 : 0;
            var isreport = ex.IsReport == true ? 1 : 0;
            var isgroupreport = ex.IsGroupReportHead == true ? 1 : 0;
            if (ex.ParentForm > 0)
            {
                var query = $@"
                --showOnDesktop is 0 when Permission and Isquicklink is also zero and IsDashboard part is 1
                DECLARE @nextFormId BIGINT
	                , @FormName VARCHAR(max)
	                , @ParentId BIGINT
	                , @FormUrl VARCHAR(max)
	                , @MenuText VARCHAR(max)
	                , @IsMenuItem BIT
	                , @MenuItemPriority SMALLINT
	                , @isActive VARCHAR(max)
	                , @IsDashboardPart BIT
	                , @ShowOnDesktop BIT
	                , @PageDescription NVARCHAR(max)
	                , @ModuleId INT
	                , @IsMasterMenu BIT
	                , @IsAjaxRequest BIT
	                , @IsHideChilds BIT
	                , @IsAction BIT
	                , @PageType NVARCHAR(max)
	                , @IsQuickLink BIT
	                , @IsReport BIT
	                , @IsGroupReportHead BIT
                    , @DefaultReportFileName NVARCHAR(max)
                    , @Area nvarchar(50)
                    , @Controller nvarchar(50)
                    , @Action nvarchar(50)";
                if (ex.FormID > 0)
                {
                    query += $@"SET @nextFormId = {ex.FormID}";
                }
                else
                {
                    query += $@"
                                SELECT @nextFormId = ISNULL((
			                                SELECT Max(FormID)
			                                FROM Membership.Forms
			                                WHERE ParentForm = {ex.ParentForm}
			                                ), (
			                                CONCAT (
				                                '{ex.ParentForm}'
				                                , '00'
				                                )
			                                ))

                                SET @nextFormId = @nextFormId + 1

                                PRINT @nextFormId";
                }
                query += $@"
                            SET @FormName = '{ex.FormName}'
                            SET @ParentId = '{ex.ParentForm}'
                            SET @FormUrl = '{ex.FormURL}'
                            SET @MenuText = '{ex.MenuText}'
                            SET @IsMenuItem = '{ismenutext}'
                            SET @MenuItemPriority = '{ex.MenuItemPriority}'
                            SET @isActive = '{ex.isActive}'
                            SET @IsDashboardPart = '{isdashboard}'
                            SET @PageDescription = '{ex.PageDescription}'
                            SET @ShowOnDesktop = '{showondesktop}'
                            SET @ModuleId = {ex.ModuleId}
                            SET @IsMasterMenu = {ismaster}
                            SET @IsAjaxRequest = 0
                            SET @IsHideChilds = 0
                            SET @IsAction = '{IsAction}'
                            SET @IsReport = '{isreport}'
                            SET @IsGroupReportHead = '{isgroupreport}'
                            SET @PageType = '{ex.PageType}'
                            SET @IsQuickLink = '{isquicklink}'
                            SET @DefaultReportFileName = '{ex.DefaultReportFileName}'
                            SET @Area = '{ex.Area}'
                            SET @Controller = '{ex.Controller}'
                            SET @Action = '{ex.Action}'

                            PRINT @nextFormId

                            IF NOT EXISTS (
		                            SELECT FormName
		                            FROM Membership.Forms
		                            WHERE FormName = @FormName
			                            AND ParentForm = @ParentId
		                            )
                            BEGIN TRY
	                            BEGIN TRANSACTION
	                            INSERT INTO Membership.Forms (
		                            FormID
		                            , FormName
		                            , FormURL
		                            , ParentForm
		                            , IsMenuItem
		                            , MenuText
		                            , MenuItemPriority
		                            , isActive
		                            , ShowOnDesktop
		                            , ModuleId
		                            , IsDashboardPart
		                            , PageDescription
		                            , IsMasterMenu
		                            , IsAjaxRequest
		                            , IsHideChilds
		                            , IsAction
		                            , PageType
		                            , IsQuickLink
		                            , IsReport
		                            , IsGroupReportHead
		                            , DefaultReportFileName
		                            , Area
		                            , Controller
		                            , Action
		                            )
	                            VALUES (
		                            @nextFormId
		                            , @FormName
		                            , @FormUrl
		                            , @ParentId
		                            , @IsMenuItem
		                            , @MenuText
		                            , @MenuItemPriority
		                            , @isActive
		                            , @ShowOnDesktop
		                            , @ModuleId
		                            , @IsDashboardPart
		                            , @PageDescription
		                            , @IsMasterMenu
		                            , @IsAjaxRequest
		                            , @IsHideChilds
		                            , @IsAction
		                            , @PageType
		                            , @IsQuickLink
		                            , @IsReport
		                            , @IsGroupReportHead
		                            , @DefaultReportFileName
		                            , @Area
		                            , @Controller
		                            , @Action
		                            )


                                    --IF (@IsReport = 1)
                                    --BEGIN
	                                -- IF NOT EXISTS (
			                        --           SELECT FormId
			                        --           FROM Membership.Reports
			                        --           WHERE FormId = ''
			                        --           )
	                                --   BEGIN
		                            --       INSERT INTO Membership.Reports (
			                        --           FormId
			                        --           , ReportFileName
                                    --           ,BranchId
			                        --           )
		                            --       VALUES (
			                        --           @nextFormId
			                        --           , @FormName
                                    --           ,{BranchId}
			                        --           )
	                                --   END
                                    --END              

                            DECLARE @TBL TABLE ([Action] VARCHAR(50));
	                            INSERT INTO @TBL ([Action])
	                            SELECT 'Can Create'
	
	                            UNION
	
	                            SELECT 'Can Read'
	
	                            UNION
	
	                            SELECT 'Can Update'
	
	                            UNION
	
	                            SELECT 'Can Delete';

	                            INSERT INTO [Membership].[FormRights] (
		                            [FormRightName]
		                            , [FormId]
		                            )
	                            SELECT [t].[Action]
		                            , [FormId]
	                            FROM @TBL AS [t]
	                            CROSS JOIN [Membership].[Forms]
	
	                            EXCEPT
	
	                            SELECT FormRightName
		                            , FormId
	                            FROM Membership.FormRights;

	                            INSERT INTO [Membership].[GroupRights] (
		                            [GroupId]
		                            , [FormRightId]
		                            , [Allowed]
		                            )
	                            SELECT (
			                            SELECT TOP 1 UserGroupId
			                            FROM Membership.UserGroups
			                            WHERE UserGroupName LIKE 'admin'
			                            ) AS [GroupId]
		                            , [fr].[FormRightId]
		                            , 1
	                            FROM [Membership].[FormRights] AS [fr]
	
	                            EXCEPT
	
	                            SELECT [GroupId]
		                            , [FormRightId]
		                            , 1
	                            FROM [Membership].[GroupRights]

	                            COMMIT
                            END TRY

                            BEGIN CATCH
	                            ROLLBACK
                            END CATCH
                        ";
                result = db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, query);
            }
            return result;
        }
        public static int UpdateMembershipForms(OneDbContext db, Form ex)
        {
            int result = 0;
            var ismenutext = ex.IsMenuItem == false ? 0 : 1;
            var isdashboard = ex.IsDashboardPart == false ? 0 : 1;
            var showondesktop = ex.ShowOnDesktop == false ? 0 : 1;
            var ismaster = ex.IsMasterMenu == false ? 0 : 1;
            var isquicklink = ex.IsQuickLink == false ? 0 : 1;
            var IsAction = ex.IsAction == false ? 0 : 1;
            if (ex.ParentForm > 0)
            {
                result = db.Database.ExecuteSqlCommand($@"
              
                DECLARE @ParentId int,@FormUrl Varchar(max), @MenuText varchar(max)
                ,@IsMenuItem bit,@MenuItemPriority smallint,@isActive varchar(max),@IsDashboardPart bit,@ShowOnDesktop bit
                ,@PageDescription nvarchar(max),@ModuleId int,@IsMasterMenu bit,@IsGroupReportHead bit
                ,@IsAjaxRequest bit,@IsHideChilds bit,@IsAction bit,@PageType nvarchar(max),@IsQuickLink bit

                set @ParentId='{ex.ParentForm}'
                set @FormUrl='{ex.FormURL}'
                set @MenuText='{ex.MenuText}'
                set @IsMenuItem='{ismenutext}'
                set @MenuItemPriority='{ex.MenuItemPriority}'
                set @isActive='{ex.isActive}'
                set @IsDashboardPart='{isdashboard}'
                set @PageDescription=''
                set @ShowOnDesktop='{showondesktop}'
                set @ModuleId={ex.ModuleId}
                set @IsMasterMenu={ismaster}
                set @IsGroupReportHead=0
                set @IsAjaxRequest=0
                set @IsHideChilds=0
                set @IsAction='{IsAction}'
                set @PageType='{ex.PageType}'
                set @IsQuickLink='{isquicklink}'
				Update
				Membership.Forms 
				set 
				FormURL = @FormUrl, 
				ParentForm = @ParentId,
				IsMenuItem = @IsMenuItem,
				MenuText = @MenuText,
				MenuItemPriority = @MenuItemPriority, 
				isActive = @isActive, 
				ShowOnDesktop = @ShowOnDesktop, 
				ModuleId  = @ModuleId, 
				IsDashboardPart = @IsDashboardPart, 
				PageDescription = @PageDescription, 
				IsMasterMenu = @IsMasterMenu,
				IsGroupReportHead = @IsGroupReportHead,
				IsAjaxRequest = @IsAjaxRequest, 
				IsHideChilds = @IsHideChilds, 
				IsAction = @IsAction,
				PageType = @PageType,
				IsQuickLink = @IsQuickLink
	            where FormID = {ex.FormID}");
            }
            return result;
        }
        public static bool DeleteGroupoRightsAndForms(OneDbContext db, int id, bool status)
        {
            if (id > 0)
            {
                var dltrows = db.Database.ExecuteSqlCommand($@"
                delete gr from Membership.GroupRights gr
                join Membership.FormRights fr on fr.FormRightId=gr.FormRightId
                where fr.FormId={id}
                delete Membership.FormRights where FormId={id}
                delete Membership.Forms where FormID={id}
                ");
                if (dltrows > 0)
                {
                    status = true;
                }
            }

            return status;
        }





        public static string CheckAppConstraints(OneDbContext db, string Module)
        {
            string msg = "";
            var fiscal = db.FiscalYears.Where(u => u.Active == true && u.BranchId == SessionHelper.BranchId).OrderByDescending(u => u.FiscalYearId).FirstOrDefault();
            if (fiscal == null)
            {
                msg = "* Fiscal Year not found <a href='/Finance/Setup/FiscalYears'>Click here</a><br>";
            }
            switch (Module)
            {
                //#region Res
                //case "Res":
                //    var _AccountSettings = db.ResAccountSettings.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                //    if (_AccountSettings == null)
                //    {
                //        msg += "* Restaurant setting not found <a href='/Res/Setup/AccountSettings'>Click here</a><br>";
                //    }
                //    else
                //    {
                //        msg += _AccountSettings.ServiceChargesAccountId == null ? "* Service charges account not found <a href='/Res/Setup/AccountSettings'>Click here</a><br>" : "";
                //        msg += _AccountSettings.TaxAccountId == null ? "* Tax account not found <a href='/Res/Setup/AccountSettings'>Click here</a><br>" : "";
                //        msg += _AccountSettings.DiscountAccountId == null ? "* Discount account not found <a href='/Res/Setup/AccountSettings'>Click here</a><br>" : "";
                //    }
                //    var _reswarehouse = db.ResWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                //    if (_reswarehouse == null)
                //    {
                //        msg += "* Restaurant's warehouse not found <a href='/Res/Setup/ManageWarehouses'>Click here</a><br>";
                //    }
                //    break;
                //#endregion
                //#region FrontDesk
                //case "FrontDesk":
                //    var _FrontDeskSetting = db.FrontDeskSettings.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                //    if (_FrontDeskSetting == null)
                //    {
                //        msg += "* FrontDesk setting not found <a href='/FrontDesk/Setup/Setting'>Click here</a><br>";
                //    }
                //    else
                //    {
                //        msg += _FrontDeskSetting.GstAccountId == null ? "* GST account not found in setting<a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.TaxParentAccountName == null ? "* Tax parent account not found in setting <a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.DiscountAccountId == null ? "* Discount account not found in setting<a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.ServiceAccountHeadName == null ? "* Service parent account not found in setting<a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.CommisionAccountId == null ? "* Commision parent account not found in setting<a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.RoomServiceId == null ? "* Room service not found in setting <a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //        msg += _FrontDeskSetting.KotServiceId == null ? "* KOT service not found in setting <a href='/FrontDesk/Setup/Setting'>Click here</a><br>" : "";
                //    }
                //    var _fdwarehouse = db.ResWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                //    if (_fdwarehouse == null)
                //    {
                //        msg += "* Front Desk's warehouse not found <a href='/FrontDesk/Setup/ManageWarehouses'>Click here</a><br>";
                //    }
                //    break;
                //#endregion
                #region CashSale
                case "CashSale":
                    msg += SessionHelper.ClientIdResCounterSession == null ? "* Cash Sale Client not found in Counter session<a href='/POS/Setup/ManageCounters'>Click here</a><br>" : "";
                    msg += SessionHelper.CashAccountIdResCounterSession == null ? "* Cash account not found in Counter session <a href='/POS/Setup/ManageCounters'>Click here</a><br>" : "";
                    msg += SessionHelper.BankAccountIdResCounterSession == null ? "* Bank account not found in Counter session<a href='/POS/Setup/ManageCounters'>Click here</a><br>" : "";
                    msg += SessionHelper.WareHouseIdResCounterSession == null ? "* Warehouse not found in Counter session<a href='/POS/Setup/ManageCounters'>Click here</a><br>" : "";
                    msg += SessionHelper.SaleCounterSessionIdResCounterSession == null ? "* Sale Counter Session not found<a href='/POS/Setup/ManageCounters'>Click here</a><br>" : "";
                    break;
                #endregion
                #region HRPayroll
                case "HRPayroll":
                    var _AccountSettingss = db.AccountSettings.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                    if (_AccountSettingss == null)
                    {
                        msg += "* Human resource setting not found <a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>";
                    }
                    else
                    {
                        msg += _AccountSettingss.EmpParentAccountId == null ? "* Employee parent account not found <a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.HRBasicSalaryAccountId == null ? "* Basic salary account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.HRAllowancesParentAccountId == null ? "* Allowances parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.HRDeductionsParentAccountId == null ? "* Deductions parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.EmpAdvancePaymentParentAccountId == null ? "* Employee Advance payment parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.EmpSecurityParentAccountId == null ? "* Employee Security parent account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                        msg += _AccountSettingss.EmpAdvancePaymentDefaultAccountId == null ? "* Employee Advance payment default account not found<a href='/HRPayroll/Payroll/AccountSettings'>Click here</a><br>" : "";
                    }
                    var _AllowancesAndDeductions = db.AllowancesAndDeductions.Where(u => u.BranchId == SessionHelper.BranchId);
                    if (!_AllowancesAndDeductions.Any())
                    {
                        msg += "* Allowances And Deductions not found<br>";
                    }
                    else
                    {
                        foreach (var item in _AllowancesAndDeductions)
                        {
                            msg += item.Account == null ? "* " + item.AllowanceDeductionName + "'s account not found<a href='/HRPayroll/Setup/AllowancesAndDeduction'>Click here</a><br>" : "";
                        }
                    }
                    break;
                #endregion
                //#region Shop
                //case "Shop":
                //    var _warehouse = db.Warehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                //    if (_warehouse == null)
                //    {
                //        msg += "* Shop's warehouse not found <a href='/Shop/Setup/ManageWarehouses'>Click here</a><br>";
                //    }
                //    break;
                //#endregion
                #region POS
                case "POS":
                    var _poswarehouse = db.InvWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                    if (_poswarehouse == null)
                    {
                        msg += "* POS's warehouse not found <a href='/POS/Setup/ManageWarehouses'>Click here</a><br>";
                    }
                    break;
                #endregion
                #region AM
                case "AM":
                    var _AMAccountSettings = db.AMNatures.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                    if (_AMAccountSettings == null)
                    {
                        msg += "* Account setting not found <a href='/AM/Setup/AccountSettings'>Click here</a><br>";
                    }
                    else
                    {
                        msg += _AMAccountSettings.InventoryParentAccountId == null ? "* Inventory Parent Account not found <a href='/AM/Setup/AccountSettings'>Click here</a><br>" : "";
                        msg += _AMAccountSettings.COGIParentAccountId == null ? "* COGI Parent Account not found <a href='/AM/Setup/AccountSettings'>Click here</a><br>" : "";
                        msg += _AMAccountSettings.DepreciationParentAccountId == null ? "* Depreciation Parent Account not found <a href='/AM/Setup/AccountSettings'>Click here</a><br>" : "";
                    }
                    var _AMwarehouse = db.AMWarehouses.Where(u => u.BranchId == SessionHelper.BranchId).FirstOrDefault();
                    if (_AMwarehouse == null)
                    {
                        msg += "* Warehouse not found <a href='/AM/Setup/ManageWarehouses'>Click here</a><br>";
                    }
                    break;
                    #endregion

            }
            return msg;
        }
        #endregion

        #region Helper Methods
        public static void SessionNotes(OneDbContext db)
        {
            var GeneralSettings = db.POSGeneralSettings.ToList();
            if (GeneralSettings.Count() > 0)
            {
                SessionHelper.PurchaseNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper() == "PurchaseInvoiceNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
                SessionHelper.SaleNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper().ToUpper() == "SaleInvoiceNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
                SessionHelper.DeliveryChallanNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper().ToUpper() == "DeliveryChallanNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
                SessionHelper.WorkOrderNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper().ToUpper() == "WorkOrderNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
                SessionHelper.SaleReturnNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper().ToUpper() == "SaleReturnNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
                SessionHelper.PurchaseReturnNote = GeneralSettings.Where(u => u.SettingName.Replace(" ", "").ToUpper().ToUpper() == "PurchaseReturnNote".ToUpper()).Select(u => u.SettingValue).FirstOrDefault();
            }
        }
        public static bool ClearPosCounterSession()
        {
            SessionHelper.ClientIdPOSCounterSession = null;
            SessionHelper.CashAccountIdPOSCounterSession = null;
            SessionHelper.BankAccountIdPOSCounterSession = null;
            SessionHelper.WareHouseIdPOSCounterSession = null;
            SessionHelper.SaleCounterSessionIdPOSCounterSession = null;
            SessionHelper.CounterNamePOSCounterSession = null;
            return true;
        }
        public static bool ClearResCounterSession()
        {
            SessionHelper.ClientIdResCounterSession = null;
            SessionHelper.CashAccountIdResCounterSession = null;
            SessionHelper.BankAccountIdResCounterSession = null;
            SessionHelper.WareHouseIdResCounterSession = null;
            SessionHelper.SaleCounterSessionIdResCounterSession = null;
            SessionHelper.CounterNameResCounterSession = null;
            return true;
        }
        public static bool ClearAMCounterSession()
        {
            SessionHelper.ClientIdAMCounterSession = null;
            SessionHelper.CashAccountIdAMCounterSession = null;
            SessionHelper.BankAccountIdAMCounterSession = null;
            SessionHelper.WareHouseIdAMCounterSession = null;
            SessionHelper.SaleCounterSessionIdAMCounterSession = null;
            SessionHelper.CounterNameAMCounterSession = null;
            return true;
        }

        internal static void CheckAndCreateLogoImages(OneDbContext db, Branch _branch, int BranchId)
        {
            if (_branch == null)
            {
                _branch = db.Branches.Where(u => u.BranchId == BranchId).FirstOrDefault();
            }
            if (_branch != null)
            {
                if (_branch.BranchLogoLarge != null && _branch.BranchLogoLarge.Length > 0)
                {
                    string targetFolder = HttpContext.Current.Server.MapPath("~/uploads/Logos");
                    string targetPath = System.IO.Path.Combine(targetFolder, "BranchLogo" + _branch.BranchId + ".png");
                    var stream = new System.IO.MemoryStream(_branch.BranchLogoLarge);
                    System.IO.File.WriteAllBytes(targetPath, stream.ToArray());
                }
            }
        }
        #endregion

        #region Extension Method


        public static Dictionary<int, string> GetEnumDictionary<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T is not an Enum type");
            }
            return System.Enum.GetValues(typeof(T))
               .Cast<T>()
               .ToDictionary(t => (int)(object)t, t => t.ToString().Replace("_", " "));
        }

        // Required For Getting Exception messsages
        public static string GetExceptionMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            var str = string.Join(Environment.NewLine, messages.LastOrDefault());
            if (str.Contains("conflicted"))
            {
                string newString = str.Substring(str.LastIndexOf("table"));//Remove string till "table"
                newString = newString.Substring(0, newString.IndexOf(","));//Remove string till comma
                newString = newString.Replace("table ", "");//Remove tbale keyword
                newString = newString.Substring(newString.LastIndexOf("."));//Remove schema name
                newString = newString.Replace(".", "");//Remove dot
                string[] split = Regex.Split(newString, @"(?<!^)(?=[A-Z])");//Insert Space Before UpperCase
                if (str.ToUpper().Contains("Insert".ToUpper()))
                {
                    str = $"Insertion failed, Conflict in '{ string.Join(" ", split)}'";
                }
                else
                {
                    str = $"Deletion failed, It is being used in '{ string.Join(" ", split)}'";
                }
            }
            return String.Join(Environment.NewLine, str);
        }
        #endregion

        #region ---- Issuance ----

        public static List<FixedAssetRegVM> GetItemRegisterIssue(OneDbContext db, int InStock, int Status, int BranchId)
        {
            var Oldquery = @"SELECT 
                        --isnull(trans.TransferHistoryId, 0) TransferHistoryId,
                        itemReg.ItemRegisterId
                       	,itemReg.ItemCode
                        ,itemReg.ProductId
                       	,pro.ProductName
                       	,itemReg.Qty Quantity
                       	,itemReg.[Value]
                       	,itemReg.DateOfEntry AS IssueDate
                        ,itemReg.[Status]
                       	,ISNULL(trans.DepartmentId, 0) DepartmentId
                       	,ISNULL(trans.EmployeeId, 0) EmployeeId
                       	,ISNULL(trans.LocationId, 0) LocationId
                       	,isnull(trans.[Description], '') [Description]
                       	,isnull(trans.ConditionTypeId, itemReg.ConditionTypeId) ConditionTypeId
                       FROM am.ItemRegister itemReg
                       INNER JOIN inv.Products pro on pro.ProductId=itemReg.ProductId
                       LEFT JOIN am.TransferHistory trans ON trans.ItemRegisterId = itemReg.ItemRegisterId and trans.BranchId=itemReg.BranchId
                       WHERE 
                       	 itemReg.[Status] = @Status
                       --AND itemReg.instock = @InStock
                       	AND itemReg.BranchId=@BranchId

                        GROUP BY itemReg.ItemRegisterId
                    	,itemReg.ItemCode
                        itemReg.ProductId
                    	,pro.ProductName
                    	,itemReg.Qty
                    	,itemReg.[Value]
                    	,itemReg.DateOfEntry
                    	,itemReg.[Status]
                    	,trans.DepartmentId
                    	,trans.EmployeeId
                    	,trans.LocationId
                    	,trans.Description
                    	,trans.ConditionTypeId
                    	,itemReg.ConditionTypeId    ";


            var newQuery = $@"SELECT 
                        --isnull(trans.TransferHistoryId, 0) TransferHistoryId,
                        itemReg.ItemRegisterId
                       	,itemReg.ItemCode
                        ,itemReg.ProductId
						,cat.CategoryId
						,cat.CategoryName
                       	,pro.ProductName
                       	,itemReg.Qty Quantity
                       	,itemReg.[Value]
                       	,itemReg.DateOfEntry AS IssueDate
                        ,itemReg.[Status]
                       	,ISNULL(itemReg.CurrentdepartmentId, 0) DepartmentId
						,isnull(dep.DepartmentName,'') DepartmentName
                       	,ISNULL(itemReg.EmployeeId, 0) EmployeeId
						,isnull(emp.EmpName,'') EmployeeName
                       	,ISNULL(itemReg.CurrentLocationId, 0) LocationId
						,isnull(loc.RoomDoorNo,'') LocationName
                       	,isnull(trans.[Description], '') [Description]
                        ,'' [Description]
                       	,isnull(trans.ConditionTypeId, itemReg.ConditionTypeId) ConditionTypeId
                       FROM am.ItemRegister itemReg
                       INNER JOIN inv.Products pro on pro.ProductId=itemReg.ProductId
					   LEFT JOIN inv.Categories cat on cat.CategoryId=pro.CategoryId
                       LEFT JOIN am.TransferHistory trans ON trans.ItemRegisterId = itemReg.ItemRegisterId and trans.BranchId=itemReg.BranchId
					   LEFT JOIN Hr.Departments dep on itemReg.CurrentdepartmentId=dep.DepartmentId
					   LEFT JOIN Hr.Employees emp on itemReg.EmployeeId=emp.EmployeeId
					   LEFT JOIN Company.Rooms loc on itemReg.CurrentLocationId=loc.RoomId


                       WHERE 
                       	 itemReg.[Status] = @Status
                       --AND itemReg.instock = @InStock
                       	AND itemReg.BranchId=@BranchId

                        GROUP BY itemReg.ItemRegisterId
                    	,itemReg.ItemCode
                        ,itemReg.ProductId
						,cat.CategoryId
						,cat.CategoryName
                    	,pro.ProductName
                    	,itemReg.Qty
                    	,itemReg.[Value]
                    	,itemReg.DateOfEntry
                    	,itemReg.[Status]
                    	,itemReg.CurrentdepartmentId
						,dep.DepartmentName
						,emp.EmpName
						,loc.RoomDoorNo
                    	,itemReg.EmployeeId
                    	,itemReg.CurrentLocationId
                    	,trans.Description
                    	,trans.ConditionTypeId
                    	,itemReg.ConditionTypeId ";
            try
            {
                var result = db.Database.SqlQuery<FixedAssetRegVM>(newQuery,
               new SqlParameter("@InStock", GetDBNullOrValue(InStock)),
               new SqlParameter("@Status", GetDBNullOrValue(Status)),
               new SqlParameter("@BranchId", GetDBNullOrValue(BranchId))
               ).ToList();
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }


        #endregion

    }
}