using FAPP.Model;
using PagedList;
using System;
using System.Collections.Generic;
using FAPP.Areas.AM.BLL;
using FAPP.INV.Models;

namespace FAPP.Areas.AM.ViewModels
{
    public class RequestModel
    {
        public bool All { get; set; }
        public bool UnPosted { get; set; }
        public bool Posted { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public bool WithBalance { get; set; }
        public string Search { get; set; }
        public Int64? InvoiceNo { get; set; }
        public string CalDigit { get; set; }
        public InvRequest Request { get; set; }
        public List<InvRequestDetail> RequestDetail { get; set; }
        public IPagedList<InvRequest> RequestPagedList { get; set; }
        public List<AMProceduresModel.v_mnl_Damages_Result> v_mnl_DamagesList { get; set; }
        public InvPurchaseOrder PurchaseOrder { get; set; }
    }
}