using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.ViewModel.Common
{
    public class AttendanceStatus
    {
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Leave { get; set; }
        public int Holiday { get; set; }
    }
}