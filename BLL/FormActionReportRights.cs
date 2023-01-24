using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FAPP.Model;


namespace FAPP.BLL
{
    public static class FormActionReportRights
    {
        //appliction id must be changed as the application
        private const int applicationId = 16;// this is the applcation id of Asset Management,every applciation as a unique application ID
       
        // The function will be called from teh page to add the rights for that particularpage
        public static void GenericFormActionRight(OneDbContext db, string url, string formActionName)
        {
            var formId = db.AppForms.Where(x => x.FormURL == url && x.ApplicationModule.ApplicationId == applicationId).Select(x => x.FormId).FirstOrDefault();
            if (formId > 0)
            {
                db.Database.ExecuteSqlCommand($@"
            if not exists (select * from system.FormActions where FormId = {formId} and ActionName = '{formActionName}')
            begin 
            INSERT INTO [System].[FormActions]([FormActionId],[ActionName] ,[Active] ,[FormId])
            VALUES ((Select IsNull(Max(FormActionId),0)+1 from System.FormActions),'{formActionName}',1,{formId})
            
            declare @formActionId int = (select Max(FormActionId) from system.FormActions where FormId = {formId})
            insert into system.FormActionGroupRights (FormActionGroupRightId,UserGroupId,FormActionId,Allowed)
            select
            (select ISNULL(max(FormActionGroupRightId),0) from system.FormActionGroupRights)+(ROW_NUMBER() Over(ORDER BY ug.UserGroupId)),
            ug.UserGroupId,
            @formActionId,
            1
            from Membership.UserGroups as ug where UserGroupId not in 
            (select UserGroupId from System.FormActionGroupRights where FormActionId = @formActionId)
            end
             ");
            }
        }

        // The function will be called from teh page to add the reprot formats for that particular page

        public static long GenericFormReportRight(OneDbContext db, string url, string formRightName)
        {
            var formId = db.AppForms.Where(x => x.FormURL == url && x.ApplicationModule.ApplicationId == applicationId).Select(x => x.FormId).FirstOrDefault();
            if (formId > 0)
            {
                db.Database.ExecuteSqlCommand($@"
                        if not exists (select * from system.FormReportFormats where FormId = {formId} and Title = '{formRightName}')
                        begin 
                        INSERT INTO [System].FormReportFormats(FormReportFormatId,Title ,[Active] ,[FormId])
                        VALUES ((Select IsNull(Max(FormReportFormatId),0)+1 from System.FormReportFormats),'{formRightName}',1,{formId})
            
                        declare @formReportformatId int = (select Max(FormReportFormatId) from system.FormReportFormats where FormId = {formId})
                        insert into system.FormReportGroupRights(UserGroupId,FormReportFormatId,Allowed)
                        select
                        --(select ISNULL(max(FormReportGroupRightId),0) from system.FormReportGroupRights)+(ROW_NUMBER() Over(ORDER BY ug.UserGroupId)),
                        ug.UserGroupId,
                        @formReportformatId,
                        1
                        from Membership.UserGroups as ug where UserGroupId not in 
                        (select UserGroupId from System.FormReportGroupRights where FormReportFormatId = @formReportformatId)
                        end
             ");
            }
            return formId;
        }

        //Implementation
        // 1- get form rights in base controller
        // add form action from page load function

    }
}