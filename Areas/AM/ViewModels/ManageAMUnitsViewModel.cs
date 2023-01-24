using FAPP.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FAPP.Areas.AM.ViewModels
{
    public class ManageAMUnitsViewModel
    {
        public IEnumerable<AMUnit> Units { get; set; }

        public string Search { get; set; }

        public int UnitId { get; set; }
        [Required(ErrorMessage="*")]
        public string UnitName { get; set; }
    }
}