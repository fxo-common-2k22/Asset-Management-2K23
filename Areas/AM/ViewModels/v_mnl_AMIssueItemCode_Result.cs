using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class v_mnl_AMIssueItemCode_Result
    {
        public int IssuedItemId { get; set; }
        public int DetailId { get; set; }
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
        public int ItemId { get; set; }
        public bool IsConsumable { get; set; }
    }
}