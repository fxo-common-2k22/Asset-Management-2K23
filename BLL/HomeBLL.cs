using FAPP.Helpers;
using FAPP.Model;
using FAPP.Models;
using FAPP.ViewModel;
using FAPP.ViewModel.Common;
using FAPP.ViewModel.Common.Student;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FAPP.BLL
{
    public static class HomeBLL
    {
        [Obsolete("use opening balances,yearlybalances table delted", true)]
        public static List<v_mnl_TopTenProducts_Result> v_mnl_financedashboard_Graph(OneDbContext db, int? FiscalYearId, short? BranchId)
        {
            var result = db.Database.SqlQuery<v_mnl_TopTenProducts_Result>($@"
					DECLARE @CashIn DECIMAL(18, 2), @CashOut DECIMAL(18, 2), @OB DECIMAL(18, 2)

					SELECT @OB = SUM(OBDebitAmount - OBCreditAmount)
					FROM Finance.YearlyBalances
					WHERE (FiscalYearId = @FiscalYearId)
						AND (BranchId = @BranchId)

					SELECT @CashIn = SUM(vd.Debit), @CashOut = SUM(vd.Credit)
					FROM Finance.VoucherDetails AS vd
					INNER JOIN Finance.Vouchers AS v ON vd.VoucherId = v.VoucherId
					WHERE (v.IsPosted = 1)
						AND (v.IsCancelled = 0)
						AND (v.FiscalYearId = @FiscalYearId)
						AND (v.BranchId = @BranchId)

					SELECT 'OB' AS Type, CASE 
							WHEN @OB > 0
								THEN @OB
							ELSE @OB * - 1
							END AS [NetTotal], CASE 
							WHEN @OB > 0
								THEN 'OB(Dr)'
							WHEN @OB = 0
								THEN 'Cash Bal'
							ELSE 'OB(Cr)'
							END AS ProductName

					UNION ALL

					SELECT 'Cash In' AS Type, CASE 
							WHEN @CashIn > 0
								THEN @CashIn
							ELSE @CashIn * - 1
							END AS [NetTotal], CASE 
							WHEN @CashIn > 0
								THEN 'Cash In(Dr)'
							WHEN @CashIn = 0
								THEN 'Cash Bal'
							ELSE 'Cash In(Cr)'
							END AS ProductName

					UNION ALL

					SELECT 'Cash Out' AS Type, CASE 
							WHEN @CashOut > 0
								THEN @CashOut
							ELSE @CashOut * - 1
							END AS [NetTotal], CASE 
							WHEN @CashOut > 0
								THEN 'Cash Out(Dr)'
							WHEN @CashOut = 0
								THEN 'Cash Bal'
							ELSE 'Cash Out(Cr)'
							END AS ProductName

					UNION ALL

					SELECT 'Cash Bal' AS Type, CASE 
							WHEN (@CashIn - @CashOut) > 0
								THEN (@CashIn - @CashOut)
							ELSE (@CashIn - @CashOut) * - 1
							END AS [NetTotal], CASE 
							WHEN @CashIn - @CashOut > 0
								THEN 'Cash Bal(Dr)'
							WHEN @CashIn - @CashOut = 0
								THEN 'Cash Bal'
							ELSE 'Cash Bal(Cr)'
							END AS ProductName
",
                         new SqlParameter("@BranchId", Utilities.GetDBNullOrValue(BranchId)),
                         new SqlParameter("@FiscalYearId", Utilities.GetDBNullOrValue(FiscalYearId))
                         ).ToList();
            return result;
        }

        public static List<v_mnl_UpcomingEvent_Result> v_mnl_UpcomingEventList(OneDbContext db,short BranchId)
        {
            var result = db.Database.SqlQuery<v_mnl_UpcomingEvent_Result>($@"SELECT TOP(25) eve.BranchId, eve.EventId, eve.EventDate, clt.Name AS ClientName, evtype.Title AS EventTitle, meal.Title as MealTitle, loc.Title AS LocationTitle
FROM            Event.Events AS eve INNER JOIN
						 Client.Clients AS clt ON eve.ClientId = clt.ClientId INNER JOIN
						 Event.EventTypes AS evtype ON eve.EventTypeId = evtype.EventTypeId INNER JOIN
						 Event.Meals AS meal ON eve.MealId = meal.MealId INNER JOIN
						 Event.Locations AS loc ON eve.LocationId = loc.LocationId
WHERE        (eve.BranchId = @BranchId) AND (eve.StatusId = 1)", new SqlParameter("@BranchId", BranchId)).ToList();
            return result;
        }

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
}