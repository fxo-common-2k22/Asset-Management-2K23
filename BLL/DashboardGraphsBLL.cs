using FAPP.DAL;
using FAPP.Model;
using FAPP.ViewModel.Common;
using FAPP.ViewModel.Common.Student;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace FAPP.BLL
{
    public static class DashboardGraphsBLL
    {
        public static StudentGendersGraphModel StudentGenderPieChart(OneDbContext db)
        {

            var chartSlices = db.Database.SqlQuery<PieChartSlice>($@"
                                                                    SELECT s.Gender AS name, CAST(COUNT(CASE 
                                                                    WHEN Gender = 'Male'
                                                                    THEN 1
                                                                    WHEN Gender = 'Female'
                                                                    THEN 1
                                                                    ELSE 0
                                                                    END) AS DECIMAL) AS y
                                                                    FROM Academics.Students AS s
                                                                    INNER JOIN Academics.StudentSessions AS ss ON ss.StudentId = s.StudentId
                                                                    AND ss.SessionActive = 1
                                                                    AND ss.SessionId = '{DAL.SessionHelper.CurrentSessionId}'
                                                                    GROUP BY s.Gender");
            var StudentGenderPierChart = new StudentGendersGraphModel()
            {
                GenderSlices = chartSlices.ToList()
            };

            return StudentGenderPierChart;
        }
        
        public static StudentAttendancePieGraphModel StudentAttendancePieChart(OneDbContext db, short? BranchId)
        {
            var chartSlices = db.Database.SqlQuery<PieChartSliceWithColor>($@"
            select sa.AttendanceStatus name,
            cast(
            (case when sa.AttendanceStatus='Present'
            then count(*) when sa.AttendanceStatus='Absent' 
            then count(*)  when sa.AttendanceStatus='Leave' 
            then count(*)  when sa.AttendanceStatus='Holiday'
            then count(*) else 0 end) as decimal) y,
            case when sa.AttendanceStatus='Present'
            then 'Green' when sa.AttendanceStatus='Absent' 
            then '#e63a3a'  when sa.AttendanceStatus='Leave' 
            then 'cyan'  when sa.AttendanceStatus='Holiday'
            then 'Borwn' else '' end as color
            from Academics.StudentAttendance sa
            join Academics.Groups g on g.GroupId=sa.GroupId
            where AttendanceDate=cast((GETDATE()) as date) and sa.BranchId='{BranchId}' 
            group by sa.AttendanceStatus
            ").ToList();
            var StudentAttendancePieChart = new StudentAttendancePieGraphModel()
            {
                AttendanceSlices = chartSlices
            };

            return StudentAttendancePieChart;
        }
        public static StudentStrengthSectionWiseModel StudentStrength(OneDbContext db)
        {
            var studentStrength = new StudentStrengthSectionWiseModel();
            var studentStrengths = new List<StudentStrengthSectionWise>();
            var classSections = db.Database.SqlQuery<ClassSectionCount>($@"
                                                                            SELECT  COUNT(*) AS Strength, cl.ClassName, sec.SectionName,ClassOrder
                                                                            FROM Academics.StudentSessions AS ss 
                                                                            INNER JOIN Academics.Classes AS cl ON ss.ClassId = cl.ClassId 
                                                                            INNER JOIN Academics.Sections AS sec ON ss.SectionId = sec.SectionId
                                                                            WHERE        (ss.SessionActive = 1) AND (ss.SessionId = '{DAL.SessionHelper.CurrentSessionId}')
                                                                            GROUP BY cl.ClassName, sec.SectionName,ClassOrder
                                                                            order by ClassOrder").ToList();

            List<string> classes = new List<string>();
            classes = classSections.Select(p => p.ClassName).Distinct().ToList();
            var sections = classSections.Select(p => p.SectionName).Distinct().ToList();
            foreach (var sectionName in sections)
            {
                var student = new StudentStrengthSectionWise();
                student.name = sectionName;
                foreach (var className in classes)
                {
                    var studentCount = classSections.Where(p => p.ClassName == className && p.SectionName == sectionName).Select(p => p.Strength).FirstOrDefault();
                    student.data.Insert(classes.IndexOf(className), studentCount);
                }
                studentStrengths.Add(student);
            }

            studentStrength.Classes = classes;

            studentStrength.StudentStrengthSectionWiseList = studentStrengths;
            return studentStrength;
        }
        public static StudentStrengthSectionWiseModel AttendanceGraphStats(OneDbContext db, short? BranchId)
        {
            var studentStrength = new StudentStrengthSectionWiseModel();
            var studentStrengths = new List<StudentStrengthSectionWise>();
            var classSections = db.Database.SqlQuery<ClassSectionCount>($@"
            select 
            count(*) as Strength,g.GroupName ClassName,sa.AttendanceStatus SectionName,g.GroupClassOrder ClassOrder
            from Academics.StudentAttendance sa
            join Academics.Groups g on g.GroupId=sa.GroupId
            where AttendanceDate=cast((GETDATE()) as date) and sa.BranchId='{BranchId}'
            group by g.GroupName,sa.AttendanceStatus,g.GroupClassOrder").ToList();

            var sections = new List<string>();
            sections.Add("Present");
            sections.Add("Absent");
            sections.Add("Leave");
            List<string> classes = new List<string>();
            classes = classSections.Select(p => p.ClassName).Distinct().ToList();
            //var sections = classSections.Select(p => p.SectionName).Distinct().ToList();
            foreach (var sectionName in sections)
            {
                var student = new StudentStrengthSectionWise();
                student.name = sectionName;
                foreach (var className in classes)
                {
                    var studentCount = classSections.Where(p => p.ClassName == className && p.SectionName == sectionName).Select(p => p.Strength).FirstOrDefault();
                    student.data.Insert(classes.IndexOf(className), studentCount);
                }
                studentStrengths.Add(student);
            }

            studentStrength.Classes = classes;

            studentStrength.StudentStrengthSectionWiseList = studentStrengths;
            return studentStrength;
        }
        public static StudentAdmissionWithdrawlModel studentAdmissionWithdrawl(OneDbContext db)
        {
            var studentAdmissionWithdrawlModel = new StudentAdmissionWithdrawlModel();
            var studentAdmissionWithdrawls = new List<StudentAdmissionWithdrawl>();
            List<DateTime> DaysInCurrentMonths = new List<DateTime>();
            var TotalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            var fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var toDate = fromDate.AddMonths(1).AddDays(-1);

            for (int i = 0; i < TotalDays; i++)
            {
                DaysInCurrentMonths.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1 + i));
            }

            var admissionWithdrawlCounts = db.Database.SqlQuery<AdmissionWithdrawlCount>($@"
                                                                            declare @typee varchar(50)
                                                                            set @typee='Withdrawls'

                                                                            select DateOfAssignment date,Type,count(DateOfAssignment)count
                                                                            from Academics.StudentSessions where Type='Admission' and DateOfDeactivation is null
                                                                            group by DateOfAssignment,Type
                                                                            union all
                                                                            select Date,@typee,count(Date)count
                                                                            from Academics.ActiveInactiveLog where IsDeactive=1
                                                                            group by Date").ToList();
            var Type = new string[] { "Withdrawls", "Admission" };
            foreach (var type in Type)
            {
                var student = new StudentAdmissionWithdrawl();
                student.name = type;
                foreach (var day in DaysInCurrentMonths)
                {
                    var studentCount = admissionWithdrawlCounts.Where(p => p.date.Date == day.Date && p.Type == type).Select(p => p.count).FirstOrDefault();
                    student.data.Insert(DaysInCurrentMonths.IndexOf(day), studentCount);
                }
                studentAdmissionWithdrawls.Add(student);
            }
            studentAdmissionWithdrawlModel.Days = DaysInCurrentMonths.Select(s => string.Format("{0:dd MMM yyyy}", s)).ToList();
            studentAdmissionWithdrawlModel.studentAdmissionWithDrawl = studentAdmissionWithdrawls;
            return studentAdmissionWithdrawlModel;
        }
        public static GraphsModel GetDataForDashboardItems(OneDbContext db, GraphsModel graphsModel, short? branchId, DateTime today, int UserId, Guid? SessionId = null)
        {
            var res = db.Sessions.Where(u => u.SessionId == SessionHelper.CurrentSessionId).Select(s => new { s.StartTime, s.EndTime }).FirstOrDefault();
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialShowStats").Any())
            {
                graphsModel.logins = db.UserLogs.Where(u => u.UserId == UserId && DbFunctions.TruncateTime(u.LoginTime) == today).Count();
                graphsModel.clients = db.Clients.Where(u => u.BranchId == branchId && u.IsClient).Count();
                graphsModel.suppliers = db.Clients.Where(u => u.BranchId == branchId && u.IsSupplier).Count();
                graphsModel.users = db.Users.Count();
                //SessionManagement Stats
                //var res = db.Sessions.Where(u => u.SessionId == SessionHelper.CurrentSessionId).Select(s => new { s.StartTime, s.EndTime }).FirstOrDefault();
                int daysDiff = ((TimeSpan)(res.EndTime - res.StartTime)).Days;
                graphsModel.SessionDays = daysDiff;
                graphsModel.SessionGroups = db.AcademicGroups.Where(u => u.BranchId == branchId && u.SessionId == SessionHelper.CurrentSessionId).Count();
                graphsModel.SessionStudents = db.StudentSessions.Where(u => u.BranchId == branchId && u.Group.SessionId == SessionHelper.CurrentSessionId && u.Active == true).Count();
                graphsModel.SessionTeachers = db.TeachingStaffs.Where(s => s.SessionId == SessionHelper.CurrentSessionId).Count();
            }
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialShowSomething").Any())
            {
                graphsModel.StudentGendersGraph = StudentGenderPieChart(db);
            }
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialAcademicGenderWiseStrength").Any())
            {
                graphsModel.StudentGendersGraph = StudentGenderPieChart(db);
            }
            

            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialStudentStrengthGraph").Any())
            {
                graphsModel.StudentStrengthGraphDataList = StudentStrength(db);//Academic Dashboard BLL
            }
            //Attendance Graph Stats
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialAcademicAttendanceStatsGraph").Any())
            {
                graphsModel.StudentAttendanceStatsGraphList = AttendanceGraphStats(db, branchId);//Academic Dashboard BLL
            }
            //Attendance PieChart
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialAcademicsAttendancePieChartGraph").Any())
            {
                graphsModel.StudentAttendancePieChart = StudentAttendancePieChart(db, branchId);
            }
            //Teachers Available
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialAcademicsAttendanceAvailableTeachers").Any())
            {
                graphsModel.Teachers = db.Database.SqlQuery<Teachers>($@"
                select 
                EAID,er.EmployeeId,e.EmpName
                from 
                hr.EmployeeAttendances er
                join hr.Employees e on e.EmployeeId=er.EmployeeId and er.AttendanceStatus='Present'
                and AttendanceDate=cast (getdate() as date) 
                join Academics.TeachingStaff ts  on ts.EmployeeId=er.EmployeeId and SessionId='{SessionHelper.CurrentSessionId}'").ToList();
            }

            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialStudentAdmissionWithdrawl").Any())
            {
                graphsModel.StudentAdmissionWithDrawlList = studentAdmissionWithdrawl(db);//Academic Dashboard BLL
            }
            if (graphsModel.v_mnl_FormRights.Where(u => u.FormURL == "_PartialAcademicAttendanceStats").Any())
            {
                graphsModel.AttendanceStatus = db.Database.SqlQuery<AttendanceStatus>($@"
            select Isnull(sum(Absent),0)Absent,Isnull(sum(Present),0)Present,isnull(sum(Leave),0)Leave,isnull(sum(Holiday),0)Holiday from (
                select 
                case when sa.AttendanceStatus='Present'
                then count(*) else 0 end as [Present],
                case when sa.AttendanceStatus='Absent' 
                then count(*) else 0 end as [Absent],
                case when sa.AttendanceStatus='Leave' 
                then count(*) else 0 end as Leave,
                case when sa.AttendanceStatus='Holiday'
                then count(*) else 0 end as [Holiday]
                from Academics.StudentAttendance sa
                join Academics.Groups g on g.GroupId=sa.GroupId
                where AttendanceDate=cast((GETDATE()) as date) and sa.BranchId=g.branchid 
                group by sa.AttendanceStatus)p").FirstOrDefault();
            }
            graphsModel.SessionStartTime = res.StartTime.ToShortDateString();
            graphsModel.SessionEndTime = res.EndTime.ToShortDateString();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            graphsModel.weeklyStrength = jss.Serialize(graphsModel.WeeklySaleList);
            return graphsModel;
        }


    }
    internal class ClassSectionCount
    {
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public int Strength { get; set; }
        public Int16 ClassOrder { get; set; }
    }
    internal class AdmissionWithdrawlCount
    {
        public DateTime date { get; set; }
        public string Type { get; set; }
        public int count { get; set; }
    }
    
}