using FAPP.Areas.AM.BLL;
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
    public class PRPrimaryData
    {
        public OneDbContext db = null;

        public PRPrimaryData(OneDbContext dbContext)
        {
            this.db = dbContext;
        }

        public void ImportAll(ref PrimaryDataViewModel vm)
        {
            try
            {
                GetOldSalaries(ref vm);
                ImportSalaries(ref vm);
                GetOldSalaryDetails(ref vm);
                ImportSalaryDetails(ref vm);
                GetOldPayments(ref vm);
                ImportPayments(ref vm);
                var importWizard = db.ImportWizard.FirstOrDefault();
                if (importWizard == null)
                {
                    importWizard = new ImportWizard
                    {
                        PRImported = true,
                    };
                    db.ImportWizard.Add(importWizard);
                }
                else
                {
                    importWizard.PRImported = true;
                }
                db.SaveChanges();
            }
            catch (Exception x)
            {
                vm.Error += x.GetExceptionMessages();
            }
        }

        public void GetOldSalaries(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from pr.Salaries";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Salaries = DataReaderMapToList<PrimaryDataViewModel.OldSalary>(reader)
                        .Select(s => new Salary
                        {
                            BranchId = s.BranchId,
                            CreatedBy = s.CreatedBy,
                            CreatedOn = s.CreatedOn,
                            FiscalYearId = s.FiscalYearId,
                            IssueDate = s.IssueDate,
                            ModifiedBy = s.ModifiedBy,
                            ModifiedOn = s.ModifiedOn,
                            SalaryId = new Guid(s.SalaryId.ToString()),
                            SalaryMonth = s.SalaryMonth,
                            //VoucherId = s.VoucherId,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportSalaries(ref PrimaryDataViewModel vm)
        {
            db.Salaries.AddOrUpdate(s => s.SalaryId, vm.Salaries.ToArray());
            db.SaveChanges();
            //Messages += $"{vm.Designations.Count()} Designations imported successfully.";
        }

        public void GetOldSalaryDetails(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from pr.SalaryDetails";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.SalaryDetails = DataReaderMapToList<PrimaryDataViewModel.OldSalaryDetail>(reader)
                        .Select(s => new SalaryDetail
                        {
                            CreatedBy = s.CreatedBy,
                            CreatedOn = s.CreatedOn,
                            EmployeeId = s.EmployeeId,
                            PaidSalary = s.PaidSalary,
                            SalaryDetailId = s.SalaryDetailId,
                            SalaryId = s.SalaryId,
                            Salary = s.Salary,
                            Posted = s.Posted,
                            VoucherDetailId = s.VoucherDetailId,
                            VoucherType = s.VoucherType,
                            TotalGross = s.TotalGross,
                            TotalDebit = s.TotalDebit,
                            TotalCredit = s.TotalCredit,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportSalaryDetails(ref PrimaryDataViewModel vm)
        {
            db.SalaryDetails.AddOrUpdate(s => s.SalaryDetailId, vm.SalaryDetails.ToArray());
            db.SaveChanges();
            //Messages += $"{vm.Designations.Count()} Designations imported successfully.";
        }

        public void GetOldPayments(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from pr.Payments";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Payments = DataReaderMapToList<PrimaryDataViewModel.OldPayment>(reader)
                        .Select(s => new Payment
                        {
                            PaymentId = s.PaymentId,
                            PaymentDate = s.PaymentDate,
                            AccountId = s.AccountId,
                            ChequeNo = s.ChequeNo,
                            Particulars = s.Particulars,
                            SalaryDetailId = s.SalaryDetailId,
                            PaidAmount = s.PaidAmount,
                            Posted = s.Posted,
                            VoucherId = s.VoucherId,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportPayments(ref PrimaryDataViewModel vm)
        {
            db.Payments.AddOrUpdate(s => s.PaymentId, vm.Payments.ToArray());
            db.SaveChanges();
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