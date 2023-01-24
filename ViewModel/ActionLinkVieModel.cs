using System.ComponentModel.DataAnnotations;

namespace FAPP.ViewModel
{
    public class ActionLinksViewModel
    {
        public ActionLinksViewModel()
        {
            IsMenuItem = true;
            IsActive = true;
            FontIcon = Icon;
            Target = "_self";
            IsAjaxRequest = false;
            ShowOnDesktop = true;
        }
        
        public int FormId { get; set; }

        [Required]
        [StringLength(50)]
        public string FormName { get; set; }
        [StringLength(500)]
        public string FormUrl { get; set; }
        public int? ParentForm { get; set; }
        [Display(Description = "Icon class saved in icons file")]
        [StringLength(50)]
        public string Icon { get; set; }

        [Display(Description = "Font icon e.g. font-awesome or glyphicons")]
        [StringLength(200)]
        public string FontIcon { get; set; }
        public bool IsMenuItem { get; set; }
        public bool IsQuickLink { get; set; }
        [StringLength(10)]
        public string Target { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Section { get; set; }
        public int? MenuPriority { get; set; }

        [StringLength(30)]
        public string Module { get; set; }

        [StringLength(30)]
        public string Area { get; set; }

        [StringLength(30)]
        public string Controller { get; set; }

        [StringLength(30)]
        public string Action { get; set; }

        public bool? IsAjaxRequest { get; set; }

        public bool? ShowOnDesktop { get; set; }

        public int? ModuleId { get; set; }

        public bool IsHideChilds { get; set; }

        public bool IsGroupReportHead { get; set; }

        //[ForeignKey("ParentForm")]
        //public virtual ActionLinks Parent { get; set; }

        //[ForeignKey("ModuleId")]
        //public virtual Module ActionModule { get; set; }

        //public virtual ICollection<ActionLinkRights> UserGroupRights { get; set; }
    }

}