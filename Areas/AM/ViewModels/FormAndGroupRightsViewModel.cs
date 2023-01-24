using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class FormAndGroupRightsViewModel
    {
        public List<v_mnl_FormRights_Result> FormRights { get; set; }

        public List<v_mnl_FormRights_Result> GroupRights { get; set; }
    }
}