using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class Branches
    {
        public int UserBranchId { get; set; }
        public int UserId { get; set; }
        public short BranchId { get; set; }
        public string BranchName { get; set; }
        public bool Active { get; set; }
        public bool DefaultBranch { get; set; }
        public int ModifiedBy { get; set; }
    }
}