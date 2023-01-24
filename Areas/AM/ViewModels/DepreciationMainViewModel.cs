using System;
using FAPP.AM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace FAPP.Areas.AM.ViewModels
{
    public class DepreciationMainViewModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public readonly int pagesize = 200;
        public int DepreciationTypeId { get; set; }
        public int? DepreciationMainId { get; set; }
        public IPagedList<DepreciationMain> Depreciations { get; set; }
        public IPagedList<DepreciationDetail> DepreciationDetails { get; set; }
    }
}