using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.ViewModel.Common
{
    public class QueryResultModel
    {
        public int YearlyBalanceId { get; set; }
        public int FiscalYearId { get; set; }
        public string AccountId { get; set; }
        public decimal OBDebitAmount { get; set; }
        public decimal OBCreditAmount { get; set; }
        public decimal TransactionDebitAmount { get; set; }
        public decimal TransactionCreditAmount { get; set; }
        public decimal CurentBalance { get; set; }
      
    }
}