using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class v_mnl_UserBranches_Result
    {
        public int UserBranchId { get; set; }
        public int UserId { get; set; }
        public short BranchId { get; set; }
        public Nullable<bool> Active { get; set; }
        public bool DefaultBranch { get; set; }
        [Display(Name = "Branch Name")]
        public string Name { get; set; }
        [Display(Name = "Branch Code")]
        public short BranchCode { get; set; }
        [Display(Name = "Address")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }
        [Display(Name = "Prefix")]
        public string RegPrefix { get; set; }
        public byte[] BranchLogoMini { get; set; }
        public byte[] BranchLogoSmall { get; set; }
        public byte[] BranchLogoLarge { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public string NTN { get; set; }
        public string GSTN { get; set; }
        [UIHint("ActiveInactiveIcons")]
        public bool IsMasterBranch { get; set; }
    }
}