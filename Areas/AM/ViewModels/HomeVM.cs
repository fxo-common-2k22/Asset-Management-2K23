using FAPP.Model;
using System;
using System.Collections.Generic;

namespace FAPP.Areas.AM.ViewModel
{
    public class HomeVM
    {
        public List<Audit> Audits { get; set; }
    }


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

    //public class UserAppVM
    //{
    //    public string ApplicationTitle { get; set; }
    //    public string AppUrl { get; set; }
    //    public string AppIcon { get; set; }
    //}
}