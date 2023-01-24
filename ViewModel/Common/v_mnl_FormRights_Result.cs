using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.ViewModel.Common
{
    public class v_mnl_FormRights_Result
    {
        public long FormID { get; set; }
        public string MenuText { get; set; }
        public string FormRightName { get; set; }
        public int GroupId { get; set; }
        public long? ParentForm { get; set; }
        public string UserGroupName { get; set; }
        public short BranchId { get; set; }
        public bool Allowed { get; set; }
        public string ControllerName { get; set; }
        public string isActive { get; set; }
        public string FormName { get; set; }
        public string FormURL { get; set; }
        public int FormRightId { get; set; }
        public int GroupRightId { get; set; }
        public bool IsMenuItem { get; set; }
        public short MenuItemPriority { get; set; }
        public string Icon { get; set; }
        public string PageType { get; set; }
        public int ModuleId { get; set; }
        public string Module { get; set; }
        public string PageDescription { get; set; }
        public bool IsDashboardPart { get; set; }
        public bool IsAction { get; set; }
        public bool IsHideChilds { get; set; }
    }
}