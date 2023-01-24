using FAPP.Model;
using System;
using System.Linq;

namespace FAPP.Helpers
{
    /// <summary>
    /// This helper class should include functions related to finance module only
    /// </summary>
    public class FinanceHelper
    {
        /// <summary>
        /// Get FiscalYearId based on date and branchid,if not then return null
        /// </summary>
        /// <param name="db">DbContext instance</param>
        /// <param name="BranchId">Your BranchId</param>
        /// <param name="Date">Date to check FiscalYear</param>
        /// <returns>int FiscalYearId</returns>
        public static int? GetFiscalYearForAutoVoucherPosting(OneDbContext db, short BranchId, DateTime Date,out string Error,out string Success)
        {
            Error = string.Empty;
            Success = string.Empty;

            
            var result= db.Database.SqlQuery<int?>(GetFiscalYearId(BranchId, Date)).FirstOrDefault();
            if(result>0)
            {
                Success = "Success! Fiscal Year Found";
            }
            else
            {
                Error = "Error! Fiscal Year Not Found";
            }
            return result;
        }

        private static string GetFiscalYearId(short BranchId, DateTime Date)
        {
            var sql= $@"
                    SELECT FiscalYearId
                    FROM Finance.FiscalYears fy
                    WHERE fy.BranchId = {BranchId}
	                    AND cast('{Date.ToddMMMyyyy()}' AS DATE) BETWEEN fy.StartDate
		                    AND fy.EndDate";
            return sql;
        }
    }
}