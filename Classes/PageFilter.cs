using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAPP.Classes
{
    public class PageFilter
    {
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        public string FromDateS { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        public string ToDateS { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        public string SearchBy { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public bool ShowFromDate { get; set; }
        public bool ShowToDate { get; set; }
        public bool ShowSearchBy { get; set; }
        public bool ShowDate { get; set; }
    }
}