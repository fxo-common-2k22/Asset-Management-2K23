using FAPP.Model;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace FAPP.Areas.AM.ViewModels.PrimaryData
{
    public class FinancePrimaryData
    {
        private OneDbContext db = null;

        public FinancePrimaryData(OneDbContext dbContext)
        {
            this.db = dbContext;
        }

        public void ImportAll(ref PrimaryDataViewModel vm)
        {
            //using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    GetOldStatmentSetup(ref vm);
                    ImportStatementSetup(ref vm);
                    GetOldChartOfAccounts(ref vm);
                    ImportChartOfAccounts(ref vm);
                    GetOldCurrencies(ref vm);
                    ImportCurrencies(ref vm);
                    GetOldCurrencyValues(ref vm);
                    ImportCurrencyValues(ref vm);
                    GetOldFiscalYear(ref vm);
                    ImportFiscalYear(ref vm);
                    GetOldTaxSlabs(ref vm);
                    ImportTaxSlabs(ref vm);
                    GetOldVoucherTypes(ref vm);
                    ImportVoucherTypes(ref vm);
                    GetOldVouchers(ref vm);
                    ImportVouchersAndDetails(ref vm);
                    //GetOldYearlyBalances(ref vm);
                    //ImportYearlyBalances(ref vm);
                    GetOldBankAccounts(ref vm);
                    ImportBankAccounts(ref vm);
                    //trans.Commit();
                    var importWizard = db.ImportWizard.FirstOrDefault();
                    if (importWizard == null)
                    {
                        importWizard = new ImportWizard
                        {
                            FinanceImported = true,
                        };
                        db.ImportWizard.Add(importWizard);
                    }
                    else
                    {
                        importWizard.FinanceImported = true;
                    }
                    db.SaveChanges();
                }
                catch (Exception x)
                {
                    vm.Error += x.GetExceptionMessages();
                    //trans.Rollback();
                }
            }

        }

        private void GetOldCurrencies(ref PrimaryDataViewModel vm)
        {

            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Finance.Currencies";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Currencies = DataReaderMapToList<PrimaryDataViewModel.OldCurreny>(reader)
                        .Select(s => new Currency
                        {
                            CurrencyId = s.CurrencyId,
                            CurrencyName = s.CurrencyName,
                            CurrencySymbol = s.CurrencySymbol,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        public void ImportCurrencies(ref PrimaryDataViewModel vm)
        {
            db.Currencies.AddOrUpdate(s => s.CurrencyId, vm.Currencies.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.Currencies.Count()} Currencies imported successfully.";
        }

        private void GetOldCurrencyValues(ref PrimaryDataViewModel vm)
        {

            var currencies = vm.Currencies;
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Finance.CurrencyValues";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.CurrencyValues = DataReaderMapToList<PrimaryDataViewModel.OldCurrencyValue>(reader)
                        .Select(s => new CurrencyValue
                        {
                            CurrencyValueId = s.CurrencyValueId,
                            CurrencyId = s.CurrencyId,
                            Value = s.Value,
                            RecordedOn = s.RecordedOn,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        public void ImportCurrencyValues(ref PrimaryDataViewModel vm)
        {
            db.CurrencyValues.AddOrUpdate(s => s.CurrencyValueId, vm.CurrencyValues.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.CurrencyValues.Count()} Currency Values imported successfully.";
        }

        private void GetOldCostGroups(ref PrimaryDataViewModel vm)
        {

            var currencies = vm.Currencies;
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Finance.CostGroups";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.CostGroups = DataReaderMapToList<PrimaryDataViewModel.OldCostGroup>(reader)
                        .Select(s => new CostGroup
                        {
                            CostGroupId = s.CostGroupId,
                            BranchId = s.BranchId,
                            CostGroupCode = s.CostGroupCode,
                            CostGroupName = s.CostGroupName,
                            Depth = s.Depth,
                            Lineage = s.Lineage,
                            CostGroupType = s.CostGroupType,
                            ParentId = s.ParentId,
                            ParentCode = s.ParentCode,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        public void ImportCostGroups(ref PrimaryDataViewModel vm)
        {
            db.CostGroups.AddOrUpdate(s => s.CostGroupId, vm.CostGroups.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.CostGroups.Count()} Cost Groups imported successfully.";
        }

        private void GetOldStatmentSetup(ref PrimaryDataViewModel vm)
        {

            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Finance.StatementSetup";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.StatementSetups = DataReaderMapToList<PrimaryDataViewModel.OldStatementSetup>(reader)
                        .Select(s => new StatementSetup
                        {
                            StatementSetupId = s.StatementSetupId,
                            ReportType = s.ReportType,
                            HeadingReportOrder = s.HeadingReportOrder,
                            HeadingNature = s.HeadingNature,
                            HeadingName = s.HeadingName,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        public void ImportStatementSetup(ref PrimaryDataViewModel vm)
        {
            db.StatementSetups.AddOrUpdate(s => s.StatementSetupId, vm.StatementSetups.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.StatementSetups.Count()} Statement Setup imported successfully.";
        }

        private void GetOldChartOfAccounts(ref PrimaryDataViewModel vm)
        {

            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Finance.Accounts";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Accounts = DataReaderMapToList<PrimaryDataViewModel.OldAccount>(reader).Select(s => new Account
                    {
                        autokey = s.autokey,
                        TITLE = s.TITLE,
                        ACCOUNT_ID = s.ACCOUNT_ID,
                        BranchId = s.BranchId,
                        ParentAccountId = s.ParentAccountId,
                        ParentId = s.ParentId,
                        LINEAGE = s.LINEAGE,
                        DEPTH = s.DEPTH,
                        AccountCode = s.AccountCode,

                        CONTROLACCOUNT = s.CONTROLACCOUNT,
                        ISTRANSACTION = s.ISTRANSACTION,
                        CurrencyId = s.CurrencyId,
                        BYDEFAULT = s.BYDEFAULT,
                        DESCN = s.DESCN,
                        DBSTATUS = s.DBSTATUS,
                        ModifiedBy = s.ModifiedBy,
                        OldAccountCode = s.OldAccountCode,
                        IsLocked = s.IsLocked,
                        GroupNo = s.GroupNo,
                        ControlAccountRef = s.ControlAccountRef,
                        StatementSetupId = s.StatementSetupId,
                        AccountGroupId = s.AccountGroupId,
                    }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        public void ImportChartOfAccounts(ref PrimaryDataViewModel vm)
        {
            db.Accounts.AddOrUpdate(s => s.autokey, vm.Accounts.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.Accounts.Count()} Accounts imported successfully.";
        }

        private void GetOldFiscalYear(ref PrimaryDataViewModel vm)
        {

            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Finance.FiscalYears";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.FiscalYears = DataReaderMapToList<PrimaryDataViewModel.OldFiscalYear>(reader)
                        .Select(s => new FiscalYear
                        {
                            FiscalYearName = s.FiscalYearName,
                            Prev__ID = s.Prev__ID,
                            StartDate = s.StartDate,
                            Active = s.Active,
                            BranchId = s.BranchId,
                            Closed = s.Closed,
                            EndDate = s.EndDate,
                            FiscalYearId = s.FiscalYearId,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        public void ImportFiscalYear(ref PrimaryDataViewModel vm)
        {
            db.FiscalYears.AddOrUpdate(s => s.FiscalYearId, vm.FiscalYears.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.FiscalYears.Count()} Fiscal Year imported successfully.";
        }

        private void GetOldTaxSlabs(ref PrimaryDataViewModel vm)
        {

            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Finance.TaxSlabs";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.TaxSlabs = DataReaderMapToList<PrimaryDataViewModel.OldTaxSlab>(reader)
                        .Select(s => new TaxSlab
                        {
                            FiscalYearId = s.FiscalYearId,
                            FixedAmount = s.FixedAmount,
                            FromAmount = s.FromAmount,
                            TaxPercentage = s.TaxPercentage,
                            TaxSlabId = s.TaxSlabId,
                            TaxSlabName = s.TaxSlabName,
                            ToAmount = s.ToAmount,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        public void ImportTaxSlabs(ref PrimaryDataViewModel vm)
        {
            db.TaxSlabs.AddOrUpdate(s => s.TaxSlabId, vm.TaxSlabs.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.TaxSlabs.Count()} Tax Slabs imported successfully.";
        }

        private void GetOldVoucherTypes(ref PrimaryDataViewModel vm)
        {

            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Finance.VoucherTypes";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.VoucherTypes = DataReaderMapToList<PrimaryDataViewModel.OldVoucherType>(reader)
                        .Select(s => new VoucherType
                        {
                            VoucherTypeId = s.VoucherTypeId,
                            VoucherTypeName = s.VoucherTypeName,
                            VoucherTypeNo = s.VoucherTypeNo,
                            //AutoPost = s.AutoPost,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        public void ImportVoucherTypes(ref PrimaryDataViewModel vm)
        {
            db.VoucherTypes.AddOrUpdate(s => s.VoucherTypeId, vm.VoucherTypes.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.VoucherTypes.Count()} Voucher Types imported successfully.";
        }

        private void GetOldVouchers(ref PrimaryDataViewModel vm)
        {

            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            try
            {
                string query = $@"Select * from Finance.Vouchers";
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Vouchers = DataReaderMapToList<PrimaryDataViewModel.OldVoucher>(reader)
                        .Select(s => new Voucher
                        {
                            VoucherId = s.VoucherId,
                            VoucherType = s.VoucherType,
                            VoucherName = s.VoucherName,
                            TransactionDate = s.TransactionDate,
                            DebitAmount = s.DebitAmount,
                            CreditAmount = s.CreditAmount,
                            Balance = s.Balance,
                            CreatedOn = s.CreatedOn,
                            CreatedBy = s.CreatedBy,
                            ModifiedOn = s.ModifiedOn,
                            ModifiedBy = s.ModifiedBy,
                            Particulars = s.Particulars,
                            VoucherStatus = s.VoucherStatus,
                            IsApproved = s.IsApproved,
                            ApprovedBy = s.ApprovedBy,
                            IsPosted = s.IsPosted,
                            PostedBy = s.PostedBy,
                            IsCancelled = s.IsCancelled,
                            CancelledBy = s.CancelledBy,
                            CurrencyId = s.CurrencyId,
                            ExchangeRate = s.ExchangeRate,
                            BranchId = s.BranchId,
                            CBAccountId = s.CBAccountId,
                            ProjectId = s.ProjectId,

                            PostedOn = s.PostedOn,
                            CancelledOn = s.CancelledOn,
                            FiscalYearId = s.FiscalYearId,
                            TFeeVoucherId = s.TFeeVoucherId,
                            TFeeMonth = s.TFeeMonth,
                            TFeeReceiptId = s.TFeeReceiptId,
                        }).ToList();
                }
                command.Dispose();

                string query1 = $@"Select * from Finance.VoucherDetails";
                SqlCommand command1 = new SqlCommand(query1, cnn);
                using (IDataReader reader = command1.ExecuteReader())
                {
                    vm.VoucherDetails = DataReaderMapToList<PrimaryDataViewModel.OldVoucherDetail>(reader)
                        .Select(s => new VoucherDetail
                        {
                            VoucherDetailId = s.VoucherDetailId,
                            VoucherId = s.VoucherId,
                            TransactionId = s.TransactionId,
                            AccountId = s.AccountId,
                            TransactionType = s.TransactionType,
                            Debit = s.Debit,
                            Credit = s.Credit,
                            ChequeNo = s.ChequeNo,
                            ChequeDate = s.ChequeDate,
                            ChequeClearDate = s.ChequeClearDate,
                            Narration = s.Narration,
                            CostGroupId = s.CostGroupId,
                            VoucherTypeABBR = s.VoucherTypeABBR,
                            VDBranchId = s.VDBranchId,
                            TIFeeVoucherId = s.TIFeeVoucherId,
                            TIFeeMonth = s.TIFeeMonth,
                            TIFeeTypeId = s.TIFeeTypeId,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        public void ImportVouchersAndDetails(ref PrimaryDataViewModel vm)
        {
            db.Vouchers.AddOrUpdate(s => s.VoucherId, vm.Vouchers.ToArray());
            db.VoucherDetails.AddOrUpdate(s => s.VoucherDetailId, vm.VoucherDetails.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.Vouchers.Count()} Vouchers imported successfully.";
        }
        [Obsolete("use opening balances,yearlybalances table delted", true)]
        private void GetOldYearlyBalances(ref PrimaryDataViewModel vm)
        {

            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Finance.YearlyBalances";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    //vm.YearlyBalances = DataReaderMapToList<PrimaryDataViewModel.OldYearlyBalance>(reader)
                    //    .Select(s => new YearlyBalance
                    //    {
                    //        YearlyBalanceId = s.YearlyBalanceId,
                    //        FiscalYearId = s.FiscalYearId,
                    //        AccountId = s.AccountId,
                    //        OBDebitAmount = s.OBDebitAmount,
                    //        OBCreditAmount = s.OBCreditAmount,
                    //        TransactionDebitAmount = s.TransactionDebitAmount,
                    //        TransactionCreditAmount = s.TransactionCreditAmount,
                    //        BranchId = s.BranchId,
                    //    }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        //public void ImportYearlyBalances(ref PrimaryDataViewModel vm)
        //{
        //    db.YearlyBalances.AddOrUpdate(s => s.YearlyBalanceId, vm.YearlyBalances.ToArray());
        //    db.SaveChanges();
        //    vm.Messages += $"{vm.YearlyBalances.Count()} Yearly Balances imported successfully.";
        //}

        private void GetOldBankAccounts(ref PrimaryDataViewModel vm)
        {

            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from Finance.BankAccounts";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.BankAccounts = DataReaderMapToList<PrimaryDataViewModel.OldBankAccount>(reader)
                        .Select(s => new BankAccount
                        {
                            AccountId = s.AccountId,
                            AccountTitle = s.AccountTitle,
                            AccountNo = s.AccountNo,
                            BankName = s.BankName,
                            BranchName = s.BranchName,
                            Address = s.Address,
                            Phone = s.Phone,
                            Fax = s.Fax,
                            Email = s.Email,
                            PDCReceivable = s.PDCReceivable,
                            PDCPayable = s.PDCPayable,
                            LastChequeNo = s.LastChequeNo,
                            ChequeNoLength = s.ChequeNoLength,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();

            }
            catch (Exception ex)
            {
                vm.Error = ex.GetExceptionMessages();
            }
        }

        public void ImportBankAccounts(ref PrimaryDataViewModel vm)
        {
            db.BankAccounts.AddOrUpdate(s => s.AccountId, vm.BankAccounts.ToArray());
            db.SaveChanges();
            vm.Messages += $"{vm.BankAccounts.Count()} Bank Accounts imported successfully.";
        }

        public List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
    }
}