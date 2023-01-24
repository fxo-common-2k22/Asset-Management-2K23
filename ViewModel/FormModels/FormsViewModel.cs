using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.ViewModel.FormModels
{
    public class FormsViewModel
    {
        public long FormID { get; set; }
        public string FormURL { get; set; }
        public string FormName { get; set; }
        public string MenuText { get; set; }
        public string Icon { get; set; }
        public short MenuItemPriority { get; set; }
        public int ApplicationModuleId { get; set; }
        public long? ParentForm { get; set; }
        public string ModuleName { get; set; }
        public bool IsHideChilds { get; set; }
    }
}