using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FAPP.Model;
using System.Web.Mvc;

namespace FAPP.Areas.AM.ViewModels
{
    public class ManageAMItemsViewModel
    {
        public AMItem Item { get; set; }
        public string SearchItem { get; set; }
        public bool SearchFixedRadioBtn { get; set; }
        public bool SearchConsumableRadioBtn { get; set; }
        public int DepriciationId { get; set; }
        public IEnumerable<AMItem> Items { get; set; }
        public List<ValueAndText> Accounts { get; set; }
        public int ItemId { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Only Two Letters Required..")]
        public string ShortName { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name="Nature")]
        public int? NatureId { get; set; }
        public SelectList Units { get; set; }
        public SelectList Categoires { get; set; }
        public SelectList Natures { get; set; }
        [Required(ErrorMessage = "*")]
        public string EditName { get; set; }
        [Required(ErrorMessage = "*")]
        public decimal? EditPrice { get; set; }
        [Required(ErrorMessage = "*")]
        public string EditNature { get; set; }
        public string Description { get; set; }
        public string IsConsumable { get; set; }

        public bool IsInvoice { get; set; }
        public bool IsBill { get; set; }

        public int? CategoryId { get; set; }
        public int UnitId { get; set; }
      
    }
    public class Natures
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}