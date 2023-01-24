using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class p_mnl_DashboardMenus_Result
    {
        public string MenuText { get; set; }
        public string FormUrl { get; set; }
        public string Icon { get; set; }
        public bool IsDashboardPart { get; set; }
        public string FaIcon { get; set; }
    }
}