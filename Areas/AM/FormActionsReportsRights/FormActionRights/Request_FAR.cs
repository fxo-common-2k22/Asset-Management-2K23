using FAPP.BLL;
using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Areas.AM.FormActionsReportsRights.FormActionRights
{
    public static class Request_FAR
    {
        public static void ManageRequests_FAR(OneDbContext db, string url, string[] formactionNames)
        {
            foreach (var item in formactionNames)
            {
                FormActionReportRights.GenericFormActionRight(db, url, item);
            }
        }
    }
}