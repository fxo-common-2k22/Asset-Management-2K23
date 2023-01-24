using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FAPP.Helpers
{
    public static class MembershipHelper
    {
        public static void DeleteMembershipFormById(OneDbContext db,long FormId) {
            try
            {
                db.Database.ExecuteSqlCommand($@"Declare @FormId bigint = {FormId} 
            delete gr from Membership.GroupRights gr
            join Membership.FormRights fr on fr.FormRightId=gr.FormRightId
            where fr.FormId=@FormId
            delete Membership.FormRights where FormId=@FormId
            delete Membership.Forms where FormID=@FormId");
            }
            catch {

            }
        }
    }
}