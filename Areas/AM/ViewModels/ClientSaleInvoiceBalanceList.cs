using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class ClientSaleInvoiceBalanceList
    {
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? Discount { get; set; }
        public decimal? NetTotal { get; set; }
        public decimal? Received { get; set; }
        public decimal? Balance { get; set; }
        public Int32? TotalsalesInvoices { get; set; }
        public short BranchId { get; set; }
        public int? TotalPayments { get; set; }
    }
}