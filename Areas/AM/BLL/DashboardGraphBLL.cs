using FAPP.Areas.AM.BLL;
using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace FAPP.Areas.AM.BLL
{
    public class DashboardGraphBLL
    {
        public static GraphsModel GetDataForDashboardItems(OneDbContext db, GraphsModel graphsModel, short? branchId, DateTime today, int UserId)
        {
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialShowStats").Any())
            {
                graphsModel.logins = db.UserLogs.Where(u => u.UserId == UserId && DbFunctions.TruncateTime(u.LoginTime) == today).Count();
                graphsModel.clients = db.Clients.Where(u => u.BranchId == branchId && u.IsClient).Count();
                graphsModel.suppliers = db.Clients.Where(u => u.BranchId == branchId && u.IsSupplier).Count();
                graphsModel.users = db.Users.Count();
            }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            graphsModel.weeklyStrength = jss.Serialize(graphsModel.WeeklySaleList);
            return graphsModel;
        }

        public static GraphsModel FetchGraphsData(OneDbContext db, GraphsModel ex)
        {
            if (!string.IsNullOrEmpty(ex.Url))
            {
                DateTime today = DateTime.Now.Date;
                var formrights = AMProceduresModel.v_mnl_DashboardViews(db, SessionHelper.UserGroupId.Value, true, "Can Read", ex.Url).ToList();
                if (formrights == null)
                {
                    return ex;
                }

                ex.DashboardConstraint = AMProceduresModel.CheckAppConstraints(db, ex.Module);
                ex.v_mnl_FormRights = formrights;
            }
            return ex;
        }

        public static List<v_mnl_FormRights_Result> v_mnl_DashboardViews(OneDbContext db, int groupId, bool? allowed, string actionName, string url)
        {
            var allowedStr = $"AND Allowed = '{ allowed }'";
            if (!allowed.HasValue)
            {
                allowedStr = "";
            }

            var actionNameStr = $"AND FormRightName = '{actionName}'";
            if (string.IsNullOrWhiteSpace(actionName))
            {
                actionNameStr = "";
            }

            var urlStr = $"AND FormURL = '{ url } '";
            if (string.IsNullOrWhiteSpace(url))
            {
                urlStr = "";
            }

            var result = db.Database.SqlQuery<v_mnl_FormRights_Result>($@"
DECLARE @parentform bigint
SELECT TOP (1) @parentform = FormID FROM Membership.Forms WHERE 1 = 1 {urlStr}
SELECT Form.FormID
	, Form.MenuText
	, FormRights.FormRightName
	, GroupRights.GroupId
	, Form.ParentForm
	, UserGroup.UserGroupName
	, GroupRights.Allowed
	, Form.ControllerName
	, Form.PageDescription
	, Form.isActive
	, Form.FormName
	, Form.FormURL
	, FormRights.FormRightId
	, GroupRights.GroupRightId
	, Form.IsMenuItem
	, Form.MenuItemPriority
	, Form.Icon
	, Form.PageType
	, Form.ModuleId
	, Form.IsDashboardPart
,Form.IsAction
FROM Membership.Forms AS Form
INNER JOIN Membership.FormRights AS FormRights ON Form.FormID = FormRights.FormId
INNER JOIN Membership.GroupRights AS GroupRights ON FormRights.FormRightId = GroupRights.FormRightId
INNER JOIN Membership.UserGroups AS UserGroup ON GroupRights.GroupId = UserGroup.UserGroupId
WHERE Form.IsDashboardPart = 1 AND ParentForm = @parentform AND UserGroup.UserGroupId = {groupId} {actionNameStr} {allowedStr}").ToList();
            return result;
        }

        public static GraphsModel GetDataForDashboardItems(OneDbContext db, GraphsModel graphsModel, short? branchId, DateTime today, int UserId, Guid? SessionId = null)
        {
          //  var res = db.Sessions.Where(u => u.SessionId == SessionHelper.CurrentSessionId).Select(s => new { s.StartTime, s.EndTime }).FirstOrDefault();
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialShowStats").Any())
            {
                graphsModel.logins = db.UserLogs.Where(u => u.UserId == UserId && DbFunctions.TruncateTime(u.LoginTime) == today).Count();
                graphsModel.clients = db.Clients.Where(u => u.BranchId == branchId && u.IsClient).Count();
                graphsModel.suppliers = db.Clients.Where(u => u.BranchId == branchId && u.IsSupplier).Count();
                graphsModel.users = db.Users.Count();
                //SessionManagement Stats
                //var res = db.Sessions.Where(u => u.SessionId == SessionHelper.CurrentSessionId).Select(s => new { s.StartTime, s.EndTime }).FirstOrDefault();
                int daysDiff = 0;
                //if (res != null)
                //{
                //    daysDiff = ((TimeSpan)(res?.EndTime - res?.StartTime)).Days;
                //}
                graphsModel.SessionDays = daysDiff;
                //graphsModel.SessionGroups = db.AcademicGroups.Where(u => u.BranchId == branchId && u.SessionId == SessionHelper.CurrentSessionId).Count();
                //graphsModel.SessionStudents = db.StudentSessions.Where(u => u.BranchId == branchId && u.Group.SessionId == SessionHelper.CurrentSessionId && u.Active == true).Count();
                //graphsModel.SessionTeachers = db.TeachingStaffs.Where(s => s.SessionId == SessionHelper.CurrentSessionId).Count();
            }
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialShowSomething").Any())
            {
                // graphsModel.StudentGendersGraph = StudentGenderPieChart(db);
            }
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialAcademicGenderWiseStrength").Any())
            {
                // graphsModel.StudentGendersGraph = StudentGenderPieChart(db);
            }


            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialStudentStrengthGraph").Any())
            {
                // graphsModel.StudentStrengthGraphDataList = StudentStrength(db);//Academic Dashboard BLL
            }
            //Attendance Graph Stats
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialAcademicAttendanceStatsGraph").Any())
            {
                // graphsModel.StudentAttendanceStatsGraphList = AttendanceGraphStats(db, branchId);//Academic Dashboard BLL
            }
            //Attendance PieChart
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialAcademicsAttendancePieChartGraph").Any())
            {
                // graphsModel.StudentAttendancePieChart = StudentAttendancePieChart(db, branchId);
            }
            //Teachers Available
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialAcademicsAttendanceAvailableTeachers").Any())
            {
                //graphsModel.Teachers = db.Database.SqlQuery<Teachers>($@"
                //select 
                //EAID,er.EmployeeId,e.EmpName
                //from 
                //hr.EmployeeAttendances er
                //join hr.Employees e on e.EmployeeId=er.EmployeeId and er.AttendanceStatus='Present'
                //and AttendanceDate=cast (getdate() as date) 
                //join Academics.TeachingStaff ts  on ts.EmployeeId=er.EmployeeId and SessionId='{SessionHelper.CurrentSessionId}'").ToList();
            }

            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialStudentAdmissionWithdrawl").Any())
            {
                // graphsModel.StudentAdmissionWithDrawlList = studentAdmissionWithdrawl(db);//Academic Dashboard BLL
            }
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialAcademicAttendanceStats").Any())
            {
                //    graphsModel.AttendanceStatus = db.Database.SqlQuery<AttendanceStatus>($@"
                //select Isnull(sum(Absent),0)Absent,Isnull(sum(Present),0)Present,isnull(sum(Leave),0)Leave,isnull(sum(Holiday),0)Holiday from (
                //    select 
                //    case when sa.AttendanceStatus='Present'
                //    then count(*) else 0 end as [Present],
                //    case when sa.AttendanceStatus='Absent' 
                //    then count(*) else 0 end as [Absent],
                //    case when sa.AttendanceStatus='Leave' 
                //    then count(*) else 0 end as Leave,
                //    case when sa.AttendanceStatus='Holiday'
                //    then count(*) else 0 end as [Holiday]
                //    from Academics.StudentAttendance sa
                //    join Academics.Groups g on g.GroupId=sa.GroupId
                //    where AttendanceDate=cast((GETDATE()) as date) and sa.BranchId=g.branchid 
                //    group by sa.AttendanceStatus)p").FirstOrDefault();
            }
            //if (res != null)
            //{
            //    graphsModel.SessionStartTime = res?.StartTime.ToShortDateString();
            //    graphsModel.SessionEndTime = res?.EndTime.ToShortDateString();
            //}
            JavaScriptSerializer jss = new JavaScriptSerializer();
            graphsModel.weeklyStrength = jss.Serialize(graphsModel.WeeklySaleList);
            return graphsModel;
        }

    }
}