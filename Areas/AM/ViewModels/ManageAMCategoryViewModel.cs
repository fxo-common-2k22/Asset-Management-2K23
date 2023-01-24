using FAPP.Areas.AM.ViewModels;
using FAPP.Model;
using PagedList;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FAPP.Areas.AM.ViewModels
{
    public class ManageAMCategoryViewModel
    {
        public AMCategory Category { get; set; }
        public string Search { get; set; }
        public IEnumerable<AMCategory> Categories { get; set; }
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Only Two Letters Required..")]
        public string ShortName { get; set; }
        public int? ParentCategory { get; set; }
        public int? NatureId { get; set; }
        public SelectList Natures { get; set; }
        public SelectList ParentCategories { get; set; }
        public IEnumerable<ValueAndText> AccountDDL { get; set; }
        public List<string> AccountList { get; set; }

        public bool DisableNatureDDL { get; set; }
        [Display(Name = "Fixed Assets Items")]
        public string FixedAutokey { get; set; }
        [Display(Name = "Consumable Items")]
        public string ConsumeableAutokey { get; set; }



        public IPagedList<AMCategory> AMCategoryPagedList { get; set; }
        public AMCategory AMCategory { get; set; }
        public List<AMCategory> AMCategoryList { get; set; }
    }


}