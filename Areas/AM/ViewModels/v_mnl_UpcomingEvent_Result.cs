using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class v_mnl_UpcomingEvent_Result
    {
        public short BranchId { get; set; }
        public long EventId { get; set; }
        public DateTime EventDate { get; set; }
        public string ClientName { get; set; }
        public string EventTitle { get; set; }
        public string MealTitle { get; set; }
        public string LocationTitle { get; set; }
    }
}