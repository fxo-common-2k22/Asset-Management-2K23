using FAPP.Areas.POS.Models;
using FAPP.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class ReturnIssueModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Search { get; set; }
        public Int64? InvoiceNo { get; set; }
        public AMReturnIssue ReturnIssue { get; set; }
        public List<AMReturnIssueDetail> ReturnIssueDetail { get; set; }
    }
}