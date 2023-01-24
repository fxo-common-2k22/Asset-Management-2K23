using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.ViewModels
{
    public class v_mnl_AuditLog_Result
    {
        public long AuditId { get; set; }
        public DateTime AuditDate { get; set; }
        public string TableName { get; set; }
        public string Operation { get; set; }
        public string Col { get; set; }
        public string OldVal { get; set; }
        public string NewVal { get; set; }
        public int ModifiedBy { get; set; }
        public string IP { get; set; }
        public string PrimaryKeyValue { get; set; }
        public string Url { get; set; }
        public string Username { get; set; }
    }
}