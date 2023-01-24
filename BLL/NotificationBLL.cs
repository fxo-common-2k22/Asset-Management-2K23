using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FAPP.Helpers;
namespace FAPP.BLL
{
    public static class NotificationBLL
    {
        public static string AddNotificationWithStudentReference(OneDbContext db, Guid SessionId, string NotificationTitle, string NotificationType, short BranchId, string Remarks, int? AdminUserId, int? UserId, int? UserLogId, string IP, string LinkedRecordId, string LinkedModuleId, string LinkedRecordType, string TicketId)
        {
            try
            {
                //string MobileNosCSV = string.Join(",", MobileNos);
                string query = $@"
                    INSERT INTO [Media].[Conversations]
                       ([ConversationId]
                       ,[Closed]    
                       ,[TicketId]
                       ,[AdminUserId]
                       ,[ConversationUserMobile]
                       ,[ConversationTitle]
                       ,[IsNotification]
                       ,[ConversationType]
                       ,[BranchId]
                       ,[ConversationRemarks]
                       ,[LinkedRecordId]
                       ,[LinkedRecordType]
                       ,[LinkedModuleId]
                       ,[ConversationDate]
                       ,[CreatedOn]
                       ,[CreatedBy]
                       ,[CreatedByIP]
                       ,[UserLogId]
                       ,[IP]
                       ,[IsRead]
                       ,[LinkedProfileId]
                       ,[LinkedProfileName]
                       ,[LinkedProfileType]
                       )
                 SELECT (
		ROW_NUMBER() OVER (
			ORDER BY t1.StudentId
			) + ISNULL((
				SELECT Max(ConversationId)
				FROM Media.Conversations
				), 0)
		)
	,0
	,'{TicketId}'
	,'{AdminUserId}'
	,t2.FamilyMobile
	,'{NotificationTitle}'
	,1
	,'{NotificationType}'
	,{BranchId}
	,'{Remarks}'
	,'{LinkedRecordId}'
	,'{LinkedRecordType}'
	,{LinkedModuleId}
	,'{DateTime.Today.ToddMMMyyyy()}'
	,getDate()
	,{UserId}
	,'{IP}'
	,'{UserLogId}'
	,'{IP}'
	,0
	,t1.StudentId
	,t1.FullName
	,'Student'
FROM Academics.Students t1
INNER JOIN Academics.StudentSessions t3 ON t1.StudentId = t3.StudentId
	AND t3.Active = 1
	AND t3.SessionActive = 1
INNER JOIN Academics.Families t2 ON t1.FamilyId = t2.FamilyId
WHERE t3.SessionId = '{SessionId}'
	AND t3.BranchId = {BranchId}
	AND t1.Active = 1 AND t2.FamilyMobile is not null AND LEN(t2.FamilyMobile) = 12";
                var RowsInserted = db.Database.ExecuteSqlCommand(query);
                return $@"{RowsInserted} Notifications Added.";
            }
            catch (Exception)
            {
                return "No Notifications are Added Due to Error. Please Contact Administrator.";
            }
        }
        public static string AddNotificationsByGroupId(OneDbContext db, Guid GroupId, string NotificationTitle, string NotificationType, short BranchId, string Remarks, int? AdminUserId, int? UserId, int? UserLogId, string IP, string LinkedRecordId, string LinkedModuleId, string LinkedRecordType, string TicketId)
        {
            try
            {
                //string MobileNosCSV = string.Join(",", MobileNos);
                string query = $@"
                    INSERT INTO [Media].[Conversations]
                       ([ConversationId]
                       ,[Closed]    
                       ,[TicketId]
                       ,[AdminUserId]
                       ,[ConversationUserMobile]
                       ,[ConversationTitle]
                       ,[IsNotification]
                       ,[ConversationType]
                       ,[BranchId]
                       ,[ConversationRemarks]
                       ,[LinkedRecordId]
                       ,[LinkedRecordType]
                       ,[LinkedModuleId]
                       ,[ConversationDate]
                       ,[CreatedOn]
                       ,[CreatedBy]
                       ,[CreatedByIP]
                       ,[UserLogId]
                       ,[IP]
                       ,[IsRead]
                       ,[LinkedProfileId]
                       ,[LinkedProfileName]
                       ,[LinkedProfileType]
                       )
                 SELECT (
		ROW_NUMBER() OVER (
			ORDER BY t1.StudentId
			) + ISNULL((
				SELECT Max(ConversationId)
				FROM Media.Conversations
				), 0)
		)
	,0
	,'{TicketId}'
	,'{AdminUserId}'
	,t2.FamilyMobile
	,@Title
	,1
	,'{NotificationType}'
	,{BranchId}
	,@Remarks
	,'{LinkedRecordId}'
	,'{LinkedRecordType}'
	,{LinkedModuleId}
	,'{DateTime.Today.ToddMMMyyyy()}'
	,getDate()
	,{UserId}
	,'{IP}'
	,'{UserLogId}'
	,'{IP}'
	,0
	,t1.StudentId
	,t1.FullName
	,'Student'
FROM Academics.Students t1
INNER JOIN Academics.StudentSessions t3 ON t1.StudentId = t3.StudentId
	AND t3.Active = 1
	AND t3.SessionActive = 1
INNER JOIN Academics.Families t2 ON t1.FamilyId = t2.FamilyId
WHERE t3.GroupId = '{GroupId}'
	AND t1.Active = 1 AND t2.FamilyMobile is not null  AND LEN(t2.FamilyMobile) = 12";
                var RowsInserted = db.Database.ExecuteSqlCommand(query, new SqlParameter("@Title", ProceduresModel.GetDBNullOrValue(NotificationTitle)), new SqlParameter("@Remarks", ProceduresModel.GetDBNullOrValue(Remarks)));
                return $@"{RowsInserted} Notifications Added.";
            }
            catch (Exception)
            {
                return "No Notifications are Added Due to Error. Please Contact Administrator.";
            }
        }

        public static async Task<int> AddSingleNotificationByStudentId(OneDbContext db, string NotificationTitle, string NotificationType, short BranchId, string Remarks, int? AdminUserId, int? UserId, int? UserLogId, string IP, string LinkedRecordId, string LinkedModuleId, string LinkedRecordType, string TicketId, int StudentId)
        {
            try
            {
                string query = $@"
                    INSERT INTO [Media].[Conversations]
                       ([ConversationId]
                       ,[Closed]
                       ,[TicketId]
                       ,[AdminUserId]
                       ,[ConversationUserMobile]
                       ,[ConversationTitle]
                       ,[IsNotification]
                       ,[ConversationType]
                       ,[BranchId]
                       ,[ConversationRemarks]
                       ,[LinkedRecordId]
                       ,[LinkedRecordType]
                       ,[LinkedModuleId]
                       ,[ConversationDate]
                       ,[CreatedOn]
                       ,[CreatedBy]
                       ,[CreatedByIP]
                       ,[UserLogId]
                       ,[IP]
                       ,[IsRead]
                       ,[LinkedProfileId]
                       ,[LinkedProfileName]
                       ,[LinkedProfileType]
                       )
                  Select
                       (ROW_NUMBER() OVER(ORDER BY t1.studentId) +  ISNULL((select Max(ConversationId) from Media.Conversations),0))
                               ,0
                               ,'{TicketId}'
                               ,'{AdminUserId}'
                               ,t2.FamilyMobile
                               ,'{NotificationTitle}'
                               ,1
                               ,'{NotificationType}'
                               ,1001
                               ,'{Remarks}'
                               ,'{LinkedRecordId}'
                               ,'{LinkedRecordType}'
                               ,1
                               ,'{DateTime.Today.ToddMMMyyyy()}'
                               ,getDate()
                               ,10010036
                               ,'{IP}'
                               ,'{UserLogId}'
                               ,'{IP}'
                               ,0
							   ,t1.StudentId
							   ,t1.FullName
							   ,'StudentId'
				               from Academics.Students t1 
							   Inner Join Academics.Families t2 on t1.FamilyId = t2.FamilyId
							   where t1.StudentId = {StudentId}
								AND t2.FamilyMobile is not null
								AND LEN(t2.FamilyMobile) = 12
                                AND t2.DeviceID is not null";
                var RowsInserted = db.Database.ExecuteSqlCommand(query);
                if (RowsInserted > 0)
                {
                    var DeviceId = db.Database.SqlQuery<string>($@"select DeviceID from Academics.Students t1 
							   Inner Join Academics.Families t2 on t1.FamilyId = t2.FamilyId
							   where t1.StudentId = {StudentId}
								AND t2.FamilyMobile is not null
								AND LEN(t2.FamilyMobile) = 12
								AND t2.DeviceID is not null").FirstOrDefault();
                    if (!string.IsNullOrEmpty(DeviceId))
                    {
                        await FirebaseBLL.PushSingleNotification(NotificationTitle, Remarks, DeviceId);
                    }
                }
                return RowsInserted;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}