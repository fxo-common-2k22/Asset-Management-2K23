using FAPP.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static FAPP.ViewModel.Common.ProceduresModel;

namespace FAPP.ViewModel.Common.Finance
{
    public class VoucherAndReceiptModel
    {
        public string action { get; set; }
        public Int64? FromAccountId { get; set; }
        public Int64? ToAccountId { get; set; }
        public Int64? VoucherNo { get; set; }
        public string Message { get; set; }
        public bool All { get; set; }
        public bool Posted { get; set; }
        public bool Reconciled { get; set; }
        public bool WithBalance { get; set; }
        public bool UnPosted { get; set; }
        public bool Cancelled { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Search { get; set; }
        public string VoucherTypeId { get; set; }
        public int Count { get; set; }
        public string CashBankAccount { get; set; }
        public Int64? VoucherId { get; set; }
        public Voucher Voucher { get; set; }
        public List<VoucherDetail> VoucherDetail { get; set; }
        public List<v_mnl_Vouchers_Result> v__Vouchers { get; set; }
        public IPagedList<v_mnl_Vouchers_Result> v__VouchersList { get; set; }
        public Voucher VoucherToCopy { get; set; }
        public int? CurrentPage { get; set; }


        public string searchBoxAccountId { get; set; }
        public string searchBoxOtherAccountId { get; set; }
        public string AmountInWords { get; set; }
    }
}