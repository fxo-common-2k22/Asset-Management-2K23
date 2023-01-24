using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class v_mnl_Vouchers_Result
    {
        public bool? IsCheked { get; set; }
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
        public int? _CreatedByUserId { get; set; }
        public int? _ModifiedByUserId { get; set; }
        public string VoucherStatus { get; set; }
        public bool IsPosted { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsApproved { get; set; }
        public short? CurrencyId { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string CurrencyName { get; set; }
        public int? _postedByUserId { get; set; }
        public int? _approvedByUserId { get; set; }
        public int? _cancelledByUserId { get; set; }
        public int? ApprovedBy { get; set; }
        public int? PostedBy { get; set; }
        public int? CancelledBy { get; set; }
        public string _IsApproved { get; set; }
        public string _IsPosted { get; set; }
        public string _IsCancelled { get; set; }
        public string _ApprovedByName { get; set; }
        public string _PostedByName { get; set; }
        public string _CreatedByName { get; set; }
        public string _CancelledByName { get; set; }
        public string _ModifiedByName { get; set; }
        public string CBAccountId { get; set; }
        public string AccountId { get; set; }
        public short BranchId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? PostedOn { get; set; }
        public DateTime? CancelledOn { get; set; }


    }
}