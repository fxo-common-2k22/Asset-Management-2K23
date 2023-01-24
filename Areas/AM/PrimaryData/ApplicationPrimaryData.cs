using FAPP.Areas.AM.BLL;
using FAPP.Areas.AM.ViewModels;
using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Module = FAPP.Model.Module;
using Task = System.Threading.Tasks.Task;
using PrimaryDataViewModel = FAPP.Areas.AM.ViewModels.PrimaryDataViewModel;
namespace FAPP.Areas.AM.PrimaryData
{
    public class ApplicationPrimaryData
    {
        public OneDbContext _context = null;

        public ApplicationPrimaryData(OneDbContext db)
        {
            this._context = db;
        }

        public void AddUpdateAppModule()
        {
            _context.Modules.AddOrUpdate(p => p.ModuleId,
                new Module { ModuleId = 1, ModuleName = "Academics" },
                new Module { ModuleId = 2, ModuleName = "Fee" },
                new Module { ModuleId = 3, ModuleName = "HR" }
                );
        }

        public void AddUpdateClientTypes()
        {
            _context.Types.AddOrUpdate(p => p.ClientTypeId,
                new FAPP.Model.Type { ClientTypeId = 1, ClientTypeName = "Individual" },
                new FAPP.Model.Type { ClientTypeId = 2, ClientTypeName = "Organization" }
                );
        }

        public void AddUpdateHouseType()
        {
            _context.HouseTypes.AddOrUpdate(p => p.HouseTypeId,
                  new HouseType { HouseTypeName = "Rented" },
                  new HouseType { HouseTypeName = "Owned" }
                  );
        }

        public void AddUpdateVoucherType()
        {
            _context.VoucherTypes.AddOrUpdate(p => p.VoucherTypeId,
                new VoucherType { VoucherTypeId = "PIP", VoucherTypeName = "Purchase Invoice Payment", VoucherTypeNo = "57" },
                new VoucherType { VoucherTypeId = "MIV", VoucherTypeName = "Material Issuance Voucher", VoucherTypeNo = "58" },
                new VoucherType { VoucherTypeId = "FGV", VoucherTypeName = "Finishgood Voucher", VoucherTypeNo = "59" },
                new VoucherType { VoucherTypeId = "EBV", VoucherTypeName = "Expense Bill Voucher", VoucherTypeNo = "63" },
                new VoucherType { VoucherTypeId = "ARF", VoucherTypeName = "Advance Receipt Fee", VoucherTypeNo = "65" },
                new VoucherType { VoucherTypeId = "EPV", VoucherTypeName = "Expense Payment Voucher", VoucherTypeNo = "64" },
                new VoucherType { VoucherTypeId = "IRV", VoucherTypeName = "Inventory Return Voucher", VoucherTypeNo = "66" }
                );
        }

        public void AddUpdateResOrderType()
        {
            //context.ResOrderTypes.AddOrUpdate(p => p.OrderTypeId,
            //    new Areas.Res.Models.ResOrderType { OrderTypeId = 1, TypeName = "Dine In", IsActive = true },
            //    new Areas.Res.Models.ResOrderType { OrderTypeId = 2, TypeName = "Take away", IsActive = true },
            //    new Areas.Res.Models.ResOrderType { OrderTypeId = 3, TypeName = "Delivery", IsActive = true }
            //    );

        }

        //public void AddUpdateResOrderStatuses()
        //{
        //    _context.ResOrderStatuses.AddOrUpdate(p => p.OrderStatusId,
        //         new Areas.Res.Models.ResOrderStatuses { OrderStatusId = 1, OrderStatus = "Reserved" },
        //         new Areas.Res.Models.ResOrderStatuses { OrderStatusId = 2, OrderStatus = "Confirmed" },
        //         new Areas.Res.Models.ResOrderStatuses { OrderStatusId = 3, OrderStatus = "Cancelled" },
        //         new Areas.Res.Models.ResOrderStatuses { OrderStatusId = 4, OrderStatus = "Served" },
        //         new Areas.Res.Models.ResOrderStatuses { OrderStatusId = 5, OrderStatus = "Completed" }
        //         );
        //}

        //public void AddUpdateResItemOrderStatuses()
        //{
        //    _context.ResItemOrderStatuses.AddOrUpdate(p => p.ItemOrderStatusId,
        //       new Areas.Res.Models.ResItemOrderStatuses { ItemOrderStatusId = 1, ItemOrderStatus = "Pending" },
        //       new Areas.Res.Models.ResItemOrderStatuses { ItemOrderStatusId = 2, ItemOrderStatus = "Ordered" },
        //       new Areas.Res.Models.ResItemOrderStatuses { ItemOrderStatusId = 3, ItemOrderStatus = "Sent to kitchen" },
        //       new Areas.Res.Models.ResItemOrderStatuses { ItemOrderStatusId = 4, ItemOrderStatus = "Preparing" },
        //       new Areas.Res.Models.ResItemOrderStatuses { ItemOrderStatusId = 5, ItemOrderStatus = "Ready" },
        //       new Areas.Res.Models.ResItemOrderStatuses { ItemOrderStatusId = 6, ItemOrderStatus = "Served" },
        //       new Areas.Res.Models.ResItemOrderStatuses { ItemOrderStatusId = 7, ItemOrderStatus = "Cancelled" },
        //       new Areas.Res.Models.ResItemOrderStatuses { ItemOrderStatusId = 8, ItemOrderStatus = "Rejected" }
        //       );
        //}

        public void AddUpdateAMRequestStatus()
        {
            _context.AMRequestStatus.AddOrUpdate(p => p.StatusId,
                new FAPP.Model.AMRequestStatus { StatusId = 1, StatusName = "Draft" },
                new FAPP.Model.AMRequestStatus { StatusId = 2, StatusName = "Pending" },
                new FAPP.Model.AMRequestStatus { StatusId = 3, StatusName = "Approved" }
                );
        }

        public void AddUpdateAMConditionType()
        {
            _context.AMConditionTypes.AddOrUpdate(p => p.ConditionTypeId,
                new FAPP.Model.AMConditionType { ConditionTypeId = 1, Name = "New" },
                new FAPP.Model.AMConditionType { ConditionTypeId = 2, Name = "Used" },
                new FAPP.Model.AMConditionType { ConditionTypeId = 3, Name = "Damaged" }

                );
        }

        public void AddUpdatePlacementType()
        {

            _context.PlacementTypes.AddOrUpdate(p => p.PlacementTypeId,
                new PlacementType { PlacementTypeId = 1, PlacementTypeName = "Appointment", Action = "NA" },
                new PlacementType { PlacementTypeId = 2, PlacementTypeName = "Joined", Action = "Activate" },
                new PlacementType { PlacementTypeId = 3, PlacementTypeName = "Retirement", Action = "Deactivate" },
                new PlacementType { PlacementTypeId = 4, PlacementTypeName = "Transfer", Action = "Activate" },
                new PlacementType { PlacementTypeId = 5, PlacementTypeName = "Terminated", Action = "Deactivate" },
                new PlacementType { PlacementTypeId = 6, PlacementTypeName = "Resigned", Action = "Deactivate" },
                new PlacementType { PlacementTypeId = 7, PlacementTypeName = "Promote", Action = "Activate" },
                new PlacementType { PlacementTypeId = 8, PlacementTypeName = "Demote", Action = "Activate" },
                new PlacementType { PlacementTypeId = 9, PlacementTypeName = "Reappointed", Action = "Activate" },
                new PlacementType { PlacementTypeId = 10, PlacementTypeName = "Applicant", Action = "NA" }
                );
        }

        public void AddUpdateRoomStatus()
        {
            //context.RoomStatuses.AddOrUpdate(p => p.RoomStatusId,
            //    new Areas.FrontDesk.Models.RoomStatus { RoomStatusId = 11, RoomStatusName = "Reserved", RoomStatusType = "Room", BackColor = "Black" }
            //    );
        }

        public void AddUpdateWeekDays()
        {
            _context.WeekDays.AddOrUpdate(p => p.DayId,
                new WeekDay { DayName = "Monday" },
                new WeekDay { DayName = "Tuesday" },
                new WeekDay { DayName = "Wednesday" },
                new WeekDay { DayName = "Thursday" },
                new WeekDay { DayName = "Friday" },
                new WeekDay { DayName = "Saturday" },
                new WeekDay { DayName = "Sunday" }
            );
        }

        public void AddUpdateTemplateTypes()
        {
            _context.TemplateTypes.AddOrUpdate(p => p.TemplateTypeId,
                new TemplateType { ModuleId = 1, DetailTemplateView = "", FooterTemplateView = "", DetailTemplateViewKey = "", FooterTemplateViewKey = "", TemplateTypeId = 1, TemplateTypeName = "General", TemplateView = "Academics.v__Students", TemplateViewKey = "StudentId", ViewField = "RollNumber,ClassName,RegistrationNo,School,Name,ClassName,RegistrationDate,SessionName,SectionName,FatherName" },
                new TemplateType { ModuleId = 2, DetailTemplateView = "", FooterTemplateView = "", DetailTemplateViewKey = "", FooterTemplateViewKey = "", TemplateTypeId = 7, TemplateTypeName = "General_", TemplateView = "Fee.v__VoucherDetails", TemplateViewKey = "FeeVoucherId", ViewField = "NAME,SessionName,SectionName,ClassName,DueDate,IssueDate,Amount,PaidAmount" },
                new TemplateType { ModuleId = 2, DetailTemplateView = "", FooterTemplateView = "", DetailTemplateViewKey = "", FooterTemplateViewKey = "", TemplateTypeId = 8, TemplateTypeName = "Defaulter", TemplateView = "Fee.v__Defaulters", TemplateViewKey = "StudentId", ViewField = "" },
                new TemplateType { ModuleId = 1, DetailTemplateView = "", FooterTemplateView = "", DetailTemplateViewKey = "", FooterTemplateViewKey = "", TemplateTypeId = 9, TemplateTypeName = "Result", TemplateView = "dbo.v__Exam__Details", TemplateViewKey = "ExamDetailId", ViewField = "FullName,FatherName,ClassName,SectionName,ObtainedMarks,MaxMarks,SubjectName,SubSubjectName,TermName" },
                new TemplateType { ModuleId = 1, DetailTemplateView = "", FooterTemplateView = "", DetailTemplateViewKey = "", FooterTemplateViewKey = "", TemplateTypeId = 10, TemplateTypeName = "Attendance", TemplateView = "Academics.v__Attendance", TemplateViewKey = "StudentId", ViewField = "StudentName,AttendanceDate,RegNo,Status,Departure" },
                new TemplateType { ModuleId = 1, DetailTemplateView = "", FooterTemplateView = "", DetailTemplateViewKey = "", FooterTemplateViewKey = "", TemplateTypeId = 11, TemplateTypeName = "Detailed Result", TemplateView = "ER.v__Results", TemplateViewKey = "StudentId, GroupId, ExamTermId, ExamTypeId", ViewField = "" },
                new TemplateType { ModuleId = 2, DetailTemplateView = "", FooterTemplateView = "", DetailTemplateViewKey = "", FooterTemplateViewKey = "", TemplateTypeId = 12, TemplateTypeName = "Payment", TemplateView = "Fee.v__VoucherDetails", TemplateViewKey = "FeeVoucherId", ViewField = "" },
                new TemplateType { ModuleId = 2, DetailTemplateView = "", FooterTemplateView = "", DetailTemplateViewKey = "", FooterTemplateViewKey = "", TemplateTypeId = 13, TemplateTypeName = "General", TemplateView = "Fee.v__VoucherDetails", TemplateViewKey = "FeeVoucherId", ViewField = "" },
                new TemplateType { ModuleId = 1, DetailTemplateView = "", FooterTemplateView = "", DetailTemplateViewKey = "", FooterTemplateViewKey = "", TemplateTypeId = 14, TemplateTypeName = "PTM", TemplateView = "Academics.v__PTM", TemplateViewKey = "StudentId", ViewField = "Title,Description,,EmpName,StartTime,EndTime,RoomName,ClassName" }
            );
        }

        //        public void CreateTemplateTypeSqlView()
        //        {
        //            //delete existing view if existed 
        //            _context.Database.ExecuteSqlCommand($@"
        //IF OBJECT_ID('Academics.v__Students', 'V') IS NOT NULL
        //    DROP VIEW [Academics].v__Students

        //IF OBJECT_ID('Fee.v__VoucherDetails', 'V') IS NOT NULL
        //    DROP VIEW [Fee].[v__VoucherDetails]

        //IF OBJECT_ID('dbo.v__FeeVoucher__Durations__CSV', 'V') IS NOT NULL
        //    DROP VIEW [dbo].[v__FeeVoucher__Durations__CSV]

        // --Adding Fee.v__Defaulters View 
        //IF OBJECT_ID('[Fee].[v__Defaulters]', 'V') IS NOT NULL
        //    DROP VIEW [Fee].[v__Defaulters]

        //--Adding dbo.v__Exam__Details View 
        //IF OBJECT_ID('[dbo].[v__Exam__Details]', 'V') IS NOT NULL
        //    DROP VIEW dbo.v__Exam__Details 

        // --Adding Academics.v__Attendance View 
        //IF OBJECT_ID('Academics.v__Attendance', 'V') IS NOT NULL
        //    DROP VIEW Academics.v__Attendance


        // --Adding ER.v__ResultSGTTFooter View 
        //IF OBJECT_ID('ER.v__ResultSGTTFooter', 'V') IS NOT NULL
        //    DROP VIEW ER.v__ResultSGTTFooter 

        // --Adding ER.v__Results View 
        //IF OBJECT_ID('ER.v__Results', 'V') IS NOT NULL
        //    DROP VIEW ER.v__Results

        //  --Adding fee.v__VoucherDetails View 
        //IF OBJECT_ID('Fee.v__VoucherDetails', 'V') IS NOT NULL
        //    DROP VIEW Fee.v__VoucherDetails  

        //   --Adding Academics.v__PTM View 
        //IF OBJECT_ID('Academics.v__PTM', 'V') IS NOT NULL
        //    DROP VIEW Academics.v__PTM     

        //--Adding dbo.v__Exam__TermPercentages View 
        //IF OBJECT_ID('dbo.v__Exam__TermPercentages', 'V') IS NOT NULL
        //    DROP VIEW dbo.v__Exam__TermPercentages   
        //");

        //            #region Academics].[v__Students
        //            _context.Database.ExecuteSqlCommand($@"

        //CREATE VIEW [Academics].[v__Students]
        //AS
        //SELECT        ass.StudentId, sa.RegistrationNo, st.FullName AS Name, st.RegistrationDate, sess.SessionName,
        //                             (SELECT        TOP (1) cl.ClassName
        //                               FROM            Academics.StudentSessions AS s INNER JOIN
        //                                                         Academics.Classes AS cl ON s.ClassId = cl.ClassId
        //                               WHERE        (s.Active = 1) AND (s.StudentId = st.StudentId)) AS ClassName, sec.SectionName, ass.RollNumber, ass.Active AS ActiveInClass, ass.StudentSessionId, 
        //                         ass.SessionId, ass.ClassId, ass.SectionId, st.MobileNumber, st.DateOfBirth, st.Address, st.FatherMobileNumber, st.FatherIdNumber, st.FatherEmail, st.Active, 
        //                         rcl.ClassName AS RequestedClassName, st.SchoolLeavingDate, st.ReasonForLeaving, st.MotherName, st.MotherMobileNumber, st.MotherIdNumber, 
        //                         st.MotherEmail, st.GuardianName, st.GuardianMobileNumber, st.GuardianIdNumber, st.GuardianEmail, st.BloodGroup, ass.Type, br.Name AS BranchName, 
        //                         sa.BranchId, sa.AdmissionDate, sa.Active AS ActiveInBranch, ass.DateOfAssignment, cl.ClassOrder, st.Password, st.ProfileId, st.GRNo, st.GaurdianRelationId, 
        //                         sess.Active AS SessionActive, st.IsOrphan, ass.GroupId, Company.Info.CompanyName AS School, br.Name AS Branch, st.Disease, st.Instructions, 
        //                         st.MedicalProblem, st.ChronicalMedicalProblems, st.TBHistory, st.DiabetesHistory, st.EpilespsyHistory, st.OthersHistory, st.Allergies, st.Medication, st.Email, 
        //                         st.FatherName + ISNULL(' ' + st.FatherLastName, '') AS FatherName, CAST(CASE st.Gender WHEN 'Male' THEN 'True' ELSE 'False' END AS BIT) AS Gender, 
        //                         st.FatherAnnualIncome, Fee.Discounts.DiscountName, Fee.Discounts.DiscountRate, st.StudentSecurityFeeAmount, st.StudentSecurityFeeVoucherId, st.EmployeeId, 
        //                         st.StaffChild, st.HouseTypeId,
        //                             (SELECT        TOP (1) cl.ClassName
        //                               FROM            Academics.StudentSessions AS s INNER JOIN
        //                                                         Academics.Classes AS cl ON s.ClassId = cl.ClassId
        //                               WHERE        (s.Type = 'Admission') AND (s.StudentId = st.StudentId)) AS AdmissionClass, ass.RFID, st.SendPresentSMSAuto, st.SendAbsentSMSAuto, 
        //                         st.LastSchoolAttended, Data.Religions.ReligionName
        //FROM            Academics.StudentSessions AS ass INNER JOIN
        //                         Academics.Classes AS cl ON ass.ClassId = cl.ClassId INNER JOIN
        //                         Academics.Sections AS sec ON ass.SectionId = sec.SectionId INNER JOIN
        //                         Academics.Sessions AS sess ON ass.SessionId = sess.SessionId RIGHT OUTER JOIN
        //                         Company.Info INNER JOIN
        //                         Company.Branches AS br ON Company.Info.SettingId = br.SettingId RIGHT OUTER JOIN
        //                         Fee.Discounts RIGHT OUTER JOIN
        //                         Academics.StudentAdmission AS sa INNER JOIN
        //                         Academics.Students AS st ON sa.StudentId = st.StudentId left outer JOIN
        //                         Data.Religions ON st.ReligionId = Data.Religions.ReligionId ON Fee.Discounts.AccountsFeeDiscountId = st.FeeDiscountId ON br.BranchId = sa.BranchId ON 
        //                         ass.StudentId = st.StudentId LEFT OUTER JOIN
        //                         Academics.Classes AS rcl ON st.RequestedClassId = rcl.ClassId
        //");
        //            #endregion

        //            #region [dbo].[v__FeeVoucher__Durations__CSV]
        //            _context.Database.ExecuteSqlCommand($@"
        //CREATE VIEW [dbo].[v__FeeVoucher__Durations__CSV]
        //AS
        //select DISTINCT FeeVoucherId,  stuff((select ', ' + LEFT(DATENAME(MONTH, FeeMonth), 3) + ' ' + DATENAME(YEAR, FeeMonth) from [Fee].[VoucherDetails] 
        //where feevoucherid = o.FeeVoucherId
        //group by FeeVoucherId, FeeMonth
        //for xml path('')),
        //          1,2,'') [Duration] 
        //FROM [Fee].[VoucherDetails] o
        //");
        //            #endregion

        //            #region [Fee].[v__VoucherDetails]
        //            _context.Database.ExecuteSqlCommand($@"
        //CREATE VIEW [Fee].[v__VoucherDetails]
        //AS
        //SELECT        fv.FeeVoucherId, fv.StudentId, st.FullName AS NAME, ses.SessionName, sec.SectionName, cl.ClassName, fv.DueDate, CASE WHEN VoucherStatus = 'Unpaid' AND 
        //                         GETDATE() > Fee.VoucherDetails.DueDate THEN 'Default [' + CAST(DATEDIFF(DD, Fee.VoucherDetails.DUEDATE, GETDATE()) AS VARCHAR(2)) + ' Days ]' ELSE '' END AS DefaultStatus, ses.SessionId, 
        //                         cl.ClassId, sec.SectionId, cl.ClassName + ' ' + sec.SectionName AS Clasec, fv.IssueDate, fv.TotalAmount AS Amount, fv.FineAmount AS Fine, 
        //                         fv.DiscountAmount AS Discount, fv.PaidAmount, fv.VoucherStatus, fv.NetAmount, fv.PayableAmount, fv.BranchId, Fee.VoucherDetails.FeeMonth, 
        //                         dbo.v__FeeVoucher__Durations__CSV.Duration, Academics.StudentAdmission.RegistrationNo AS RegistrationNumber, st.FatherName
        //FROM            Fee.Vouchers AS fv INNER JOIN
        //                         Academics.Students AS st ON fv.StudentId = st.StudentId INNER JOIN
        //                         Academics.Groups AS ag ON fv.GroupId = ag.GroupId INNER JOIN
        //                         Academics.Sessions AS ses ON ag.SessionId = ses.SessionId INNER JOIN
        //                         Academics.Sections AS sec ON ag.SectionId = sec.SectionId INNER JOIN
        //                         Academics.Classes AS cl ON ag.ClassId = cl.ClassId INNER JOIN
        //                         Fee.VoucherDetails ON fv.FeeVoucherId = Fee.VoucherDetails.FeeVoucherId INNER JOIN
        //                         dbo.v__FeeVoucher__Durations__CSV ON fv.FeeVoucherId = dbo.v__FeeVoucher__Durations__CSV.FeeVoucherId INNER JOIN
        //                         Academics.StudentAdmission ON st.StudentId = Academics.StudentAdmission.StudentId AND st.BranchId = Academics.StudentAdmission.BranchId
        //");
        //            #endregion

        //            #region [Fee].[v__Defaulters]
        //            _context.Database.ExecuteSqlCommand($@"

        //CREATE VIEW [Fee].[v__Defaulters]
        //AS
        //SELECT sa.RegistrationNo AS RegistrationNumber, st.FullName AS Name, ses.SessionName, 
        //     ag.GroupName AS Clasec, st.FatherMobileNumber AS FatherMobile, ISNULL(pb.PA, 0) 
        //     AS PrevBalance, ISNULL(cb.PA, 0) AS CurrBalance, ag.SessionId, ag.ClassId, ag.SectionId, 
        //     st.StudentId, ass.Active, st.FatherName + ' ' + st.FatherLastName AS FatherName, 
        //     ISNULL(pb.PA, 0) + ISNULL(cb.PA, 0) AS NetBalance, st.GRNo
        //FROM Academics.Sessions AS ses INNER JOIN
        //     Academics.Groups AS ag ON ses.SessionId = ag.SessionId INNER JOIN
        //     Academics.StudentSessions AS ass ON ag.GroupId = ass.GroupId INNER JOIN
        //     Academics.Students AS st ON ass.StudentId = st.StudentId AND 
        //     ass.Active = 1 LEFT OUTER JOIN
        //         (SELECT SUM(PayableAmount) AS PA, StudentId
        //       FROM Fee.Vouchers AS FeeVouchers_1
        //       WHERE (dbo.f__GetFirstDayOfMonth(DueDate) = dbo.f__GetFirstDayOfMonth(GETDATE())) 
        //             AND (DueDate < GETDATE()) AND (IsCancelled = 0) AND (VoucherStatus <> 'Paid')
        //       GROUP BY StudentId) AS cb ON st.StudentId = cb.StudentId LEFT OUTER JOIN
        //         (SELECT SUM(PayableAmount) AS PA, StudentId
        //       FROM Fee.Vouchers AS FeeVouchers_2
        //       WHERE (DueDate < DATEADD(DAY, - 1, dbo.f__GetFirstDayOfMonth(GETDATE()))) AND 
        //             (IsCancelled = 0) AND (VoucherStatus <> 'Paid')
        //       GROUP BY StudentId) AS pb ON st.StudentId = pb.StudentId INNER JOIN
        //     Academics.StudentAdmission AS sa ON sa.StudentId = st.StudentId AND 
        //     sa.BranchId = st.BranchId
        //WHERE (ass.Active = 1) AND (pb.PA > 0) OR
        //     (ass.Active = 1) AND (cb.PA > 0)
        //");
        //            #endregion

        //            #region [dbo].[v__Exam__Details]

        //            _context.Database.ExecuteSqlCommand($@"
        //CREATE VIEW [dbo].[v__Exam__TermPercentages]
        //AS
        //SELECT        ex.GroupId, ex.ExamTermId, SUM(ed.ObtainedMarks) / SUM(ex.MaxMarks) * 100 AS TermPercentage, ed.StudentId
        //FROM            [ER].[ResultItems] AS ed INNER JOIN
        //                         [ER].[Results] AS ex ON ed.ExamId = ex.ExamId
        //GROUP BY ex.GroupId, ex.ExamTermId, ed.StudentId
        //HAVING        (SUM(ex.MaxMarks) > 0)
        //");

        //            _context.Database.ExecuteSqlCommand($@"
        //CREATE VIEW [dbo].[v__Exam__Details]
        //AS
        //SELECT DISTINCT 
        //                         exd.ExamId, exd.StudentId, ast.FullName, Academics.StudentSessions.RollNumber, ast.FatherName + ' ' + ast.FatherLastName AS FatherName, ast.DateOfBirth, 
        //                         gr.GroupBranchName AS BranchName, ast.Active, gr.SessionId, gr.ClassId, gr.SectionId, gr.GroupSessionName AS SessionName, 
        //                         gr.GroupClassName AS ClassName, gr.GroupSectionName AS SectionName, ex.ExamTermId, ex.ExamSubjectId, ex.SubSubjectId, ex.MaxMarks, 
        //                         exd.ObtainedMarks, et.TermName, sub.SubjectName, ss.Name AS SubSubjectName,
        //                             (SELECT        TOP (1) GradeName
        //                               FROM            ER.Grades AS eg
        //                               WHERE        (exd.Percentage BETWEEN MinMarks AND MaxMarks) AND (BranchId = gr.BranchId) AND (SessionId = gr.SessionId) AND (StageId = gr.GroupStageId)) 
        //                         AS GradeName, ast.Gender, ast.Photo, exd.AttendanceStatus, gr.GroupStageId AS StageId, ast.GRNo, ex.Published, ex.DeclarationDate, ex.HighestPercentage, 
        //                         ex.LowestPercentage, sub.ShortFormSubject, sub.SubjectCode, ss.ShortForm AS SubSubjectShortForm, dbo.v__Exam__TermPercentages.TermPercentage, 
        //                         ex.ExamDate, et.Weightage, ER.Types.ExamType, exd.ResultStatus, exd.ObtainedMarks / ex.MaxMarks * 100 AS TotalPercentage, 
        //                         exd.ObtainedMarks / ex.MaxMarks * et.Weightage AS TotalObtWeighted, ex.ExamTypeId, ex.GroupId, Academics.StudentSessions.Active AS ActiveInSection, 
        //                         Academics.StudentSessions.GroupId AS AcadGroupId, ex.ExamNo, ex.TeacherId, exd.ExamDetailId, ast.FatherMobileNumber, exd.Remarks, 
        //                         Academics.StudentSessions.StudentSessionId, ast.RegistrationNumber, ex.SubjectPassMarks, exd.Grade, exd.SubjectGrade, exd.GradeRemarks, 
        //                         exd.SubjectGradeRemarks, ER.ExamRegistrations.RollNo AS ERNO, ex.SubjectPassMarksPercentage, exd.Itr, ex.MarksPercentage, ex.PassPercentage
        //FROM            ER.ResultItems AS exd INNER JOIN
        //                         Academics.Students AS ast ON exd.StudentId = ast.StudentId INNER JOIN
        //                         ER.Results AS ex ON exd.ExamId = ex.ExamId INNER JOIN
        //                         Academics.Groups AS gr ON ex.GroupId = gr.GroupId INNER JOIN
        //                         ER.Terms AS et ON ex.ExamTermId = et.ExamTermId INNER JOIN
        //                         ER.Subjects AS sub ON ex.ExamSubjectId = sub.ExamSubjectId INNER JOIN
        //                         ER.SubSubjects AS ss ON ex.SubSubjectId = ss.ExamSubSubjectId INNER JOIN
        //                         ER.Types ON ex.ExamTypeId = ER.Types.ExamTypeId INNER JOIN
        //                         dbo.v__Exam__TermPercentages ON ex.ExamTermId = dbo.v__Exam__TermPercentages.ExamTermId AND 
        //                         exd.StudentId = dbo.v__Exam__TermPercentages.StudentId AND ex.GroupId = dbo.v__Exam__TermPercentages.GroupId INNER JOIN
        //                         Academics.StudentSessions ON ast.StudentId = Academics.StudentSessions.StudentId LEFT OUTER JOIN
        //                         ER.ExamRegistrations ON ex.ExamTypeId = ER.ExamRegistrations.TypeId AND ex.ExamTermId = ER.ExamRegistrations.TermId AND 
        //                         Academics.StudentSessions.StudentSessionId = ER.ExamRegistrations.StudentSessionId
        //");
        //            #endregion

        //            #region Academics.v__Attendance
        //            _context.Database.ExecuteSqlCommand($@"

        //CREATE VIEW [Academics].[v__Attendance]
        //AS
        //SELECT        sa.StudentId, st.FullName AS StudentName, st.RegistrationNumber AS RegNo, CAST(CASE st.Gender WHEN 'Male' THEN 1 ELSE 0 END AS BIT) AS Gender, st.GRNo, 
        //                         sa.AttendanceDate, sa.AttendanceStatus AS Status, sa.Arrival, sa.Departure, sa.HalfDay
        //FROM            Academics.StudentAttendance AS sa INNER JOIN
        //                         Academics.Students AS st ON sa.StudentId = st.StudentId
        //");
        //            #endregion

        //            #region [er].[v__ResultSGTTFooter]
        //            _context.Database.ExecuteSqlCommand($@"
        //CREATE VIEW [ER].[v__ResultSGTTFooter]
        //AS
        //SELECT rd.StudentId, re.GroupId, re.ExamTermId, re.ExamTypeId, SUM(rd.ObtainedMarks) 
        //     AS TotalObtainedMarks, SUM(re.MaxMarks) AS TotalMaxMarks, SUM(rd.ObtainedMarks) 
        //     / SUM(re.MaxMarks) * 100 AS TotalPercentage,
        //         (SELECT TOP (1) GradeName
        //       FROM ER.Grades
        //       WHERE (SessionId = gr.SessionId) AND (StageId = gr.GroupStageId) AND 
        //             (CAST(SUM(rd.ObtainedMarks) / SUM(re.MaxMarks) * 100 AS decimal(9, 2)) BETWEEN 
        //             MinMarks AND MaxMarks)) AS TTGrade
        //FROM ER.ResultItems AS rd INNER JOIN
        //     ER.Results AS re ON rd.ExamId = re.ExamId INNER JOIN
        //     Academics.Groups AS gr ON re.GroupId = gr.GroupId
        //GROUP BY rd.StudentId, re.GroupId, re.ExamTermId, re.ExamTypeId, gr.SessionId, gr.GroupStageId
        //");
        //            #endregion

        //            #region [er].[v__Results]
        //            _context.Database.ExecuteSqlCommand($@"

        //CREATE VIEW [ER].[v__Results]
        //AS
        //SELECT DISTINCT 
        //     exd.ExamId, exd.StudentId, ast.FullName, Academics.StudentSessions.RollNumber, 
        //     ast.FatherName + ' ' + ast.FatherLastName AS FatherName, ast.DateOfBirth, 
        //     Company.Branches.Name AS BranchName, ast.Active, 
        //     Academics.StudentAdmission.RegistrationNo AS RegistrationNumber, gr.SessionId, 
        //     gr.ClassId, gr.SectionId, ses.SessionName, cl.ClassName, sec.SectionName, 
        //     ex.ExamTermId, ex.ExamSubjectId, ex.SubSubjectId, ex.MaxMarks, exd.ObtainedMarks, 
        //     et.TermName, sub.SubjectName, ss.Name AS SubSubjectName,
        //         (SELECT TOP (1) GradeName
        //       FROM ER.Grades AS eg
        //       WHERE (exd.ObtainedMarks / ex.MaxMarks * 100 BETWEEN MinMarks AND 
        //             MaxMarks) AND (BranchId = gr.BranchId) AND (SessionId = gr.SessionId) AND 
        //             (StageId = cl.StageId)) AS GradeName, ast.Gender, exd.AttendanceStatus, cl.StageId, 
        //     ast.GRNo, ex.Published, ex.DeclarationDate, ex.HighestPercentage, ex.LowestPercentage, 
        //     sub.ShortFormSubject, sub.SubjectCode, ss.ShortForm AS SubSubjectShortForm, 
        //     dbo.v__Exam__TermPercentages.TermPercentage, ex.ExamDate, et.Weightage, 
        //     ER.Types.ExamType, exd.ResultStatus, 
        //     ROUND(CAST(exd.ObtainedMarks / ex.MaxMarks * 100 AS decimal(18, 2)), 2) 
        //     AS TotalPercentage, 
        //     exd.ObtainedMarks / ex.MaxMarks * et.Weightage AS TotalObtWeighted, ex.ExamTypeId, 
        //     ex.GroupId, Academics.StudentSessions.Active AS ActiveInSection, 
        //     Academics.StudentSessions.GroupId AS AcadGroupId, ex.ExamNo, ex.TeacherId, 
        //     exd.ExamDetailId, ast.FatherMobileNumber, exd.Remarks, 
        //     Academics.StudentSessions.StudentSessionId, 
        //     ER.v__ResultSGTTFooter.TotalObtainedMarks, ER.v__ResultSGTTFooter.TotalMaxMarks, 
        //     ROUND(CAST(ER.v__ResultSGTTFooter.TotalPercentage AS decimal(18, 2)), 2) 
        //     AS TotalFPercentage, ER.v__ResultSGTTFooter.TTGrade, ast.Photo
        //FROM Academics.StudentAdmission INNER JOIN
        //     Academics.StudentSessions ON 
        //     Academics.StudentAdmission.StudentId = Academics.StudentSessions.StudentId AND 
        //     Academics.StudentAdmission.BranchId = Academics.StudentSessions.BranchId INNER JOIN
        //     ER.ResultItems AS exd INNER JOIN
        //     Academics.Students AS ast ON exd.StudentId = ast.StudentId INNER JOIN
        //     ER.Results AS ex ON exd.ExamId = ex.ExamId INNER JOIN
        //     Academics.Groups AS gr ON ex.GroupId = gr.GroupId INNER JOIN
        //     Academics.Sessions AS ses ON gr.SessionId = ses.SessionId INNER JOIN
        //     Academics.Classes AS cl ON gr.ClassId = cl.ClassId INNER JOIN
        //     Academics.Sections AS sec ON gr.SectionId = sec.SectionId INNER JOIN
        //     ER.Terms AS et ON ex.ExamTermId = et.ExamTermId INNER JOIN
        //     ER.Subjects AS sub ON ex.ExamSubjectId = sub.ExamSubjectId INNER JOIN
        //     ER.SubSubjects AS ss ON ex.SubSubjectId = ss.ExamSubSubjectId INNER JOIN
        //     ER.Types ON ex.ExamTypeId = ER.Types.ExamTypeId INNER JOIN
        //     dbo.v__Exam__TermPercentages ON 
        //     ex.ExamTermId = dbo.v__Exam__TermPercentages.ExamTermId AND 
        //     exd.StudentId = dbo.v__Exam__TermPercentages.StudentId AND 
        //     ex.GroupId = dbo.v__Exam__TermPercentages.GroupId INNER JOIN
        //     Company.Branches ON gr.BranchId = Company.Branches.BranchId INNER JOIN
        //     ER.v__ResultSGTTFooter ON exd.StudentId = ER.v__ResultSGTTFooter.StudentId AND 
        //     ex.GroupId = ER.v__ResultSGTTFooter.GroupId AND 
        //     ex.ExamTermId = ER.v__ResultSGTTFooter.ExamTermId AND 
        //     ex.ExamTypeId = ER.v__ResultSGTTFooter.ExamTypeId ON 
        //     Academics.StudentSessions.GroupId = ex.GroupId AND 
        //     Academics.StudentSessions.StudentId = exd.StudentId
        //");
        //            #endregion


        //            #region [Academics].[v__PTM]
        //            _context.Database.ExecuteSqlCommand($@"

        //CREATE VIEW [Academics].[v__PTM]
        //AS
        //SELECT DISTINCT api.TeacherId, ap.Title, ap.Date, ap.Description, em.EmpName, api.StartTime, api.EndTime, rm.RoomDoorNo As RoomName, ac.ClassName, ass.StudentId
        //FROM            Academics.Ptm AS ap INNER JOIN
        //                         Academics.PtmItems AS api ON ap.PtmId = api.PtmId INNER JOIN
        //                         Company.Rooms AS rm ON api.RoomId = rm.RoomId INNER JOIN
        //                         HR.Employees AS em ON api.TeacherId = em.EmployeeId INNER JOIN
        //                         Academics.Classes AS ac ON api.ClassId = ac.ClassId INNER JOIN
        //                         Academics.StudentSessions AS ass ON ass.ClassId = api.ClassId
        //WHERE        (ass.Active = 1)
        //");
        //            #endregion
        //        }

        public void AddUpdateApplicationPortal()
        {
            _context.ApplicationPortals.AddOrUpdate(p => p.ApplicationPortalId,
                new ApplicationPortal { ApplicationPortalId = 1, Name = "Admin Portal" },
                new ApplicationPortal { ApplicationPortalId = 2, Name = "Employee Portal" }
            );
        }

        public void AddUpdatePaymentModes()
        {
            _context.PaymentModes.AddOrUpdate(p => p.PaymentModeId,
                new PaymentMode { PaymentModeId = 1, PaymentModeName = "Bank" },
                new PaymentMode { PaymentModeId = 2, PaymentModeName = "Cash" },
                new PaymentMode { PaymentModeId = 3, PaymentModeName = "Credit" }
            );
        }

        public void ImportAllOld(ref PrimaryDataViewModel vm)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    GetOldCountries(ref vm);
                    ImportCountries(ref vm);
                    GetOldStates(ref vm);
                    ImportStates(ref vm);
                    GetOldCitites(ref vm);
                    ImportCitites(ref vm);
                    GetOldNationalities(ref vm);
                    ImportNationalities(ref vm);
                    GetOldDocumentTypes(ref vm);
                    ImportDocumentTypes(ref vm);
                    GetOldProfession(ref vm);
                    ImportProfessions(ref vm);
                    GetOldRegions(ref vm);
                    ImportRegions(ref vm);
                    GetOldReligions(ref vm);
                    ImportReligions(ref vm);
                    SaveChanges();
                    trans.Commit();
                    var importWizard = _context.ImportWizard.FirstOrDefault();
                    if (importWizard == null)
                    {
                        importWizard = new ImportWizard
                        {
                            DataImported = true,
                        };
                        _context.ImportWizard.Add(importWizard);
                    }
                    else
                    {
                        importWizard.DataImported = true;
                    }
                    SaveChanges();
                }
                catch (Exception x)
                {
                    vm.Error += x.GetExceptionMessages();
                    trans.Rollback();
                }
            }
        }

        public void GetOldCountries(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from DATA.Countries";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Countries = DataReaderMapToList<PrimaryDataViewModel.OLDCountry>(reader)
                        .Select(s => new Country
                        {
                            CountryId = s.CountryId,
                            CallingCode = s.CallingCode,
                            CountryName = s.CountryName,
                            ISO3166Code = s.ISO3166Code,
                            IdCardNoVE = s.IdCardNoVE,
                            MobileNoVE = s.MobileNoVE,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportCountries(ref PrimaryDataViewModel vm)
        {
            _context.Countries.AddOrUpdate(s => s.CountryId, vm.Countries.ToArray());

            vm.Messages += $"{vm.Countries.Count()} Countries imported successfully.";
        }

        public void GetOldStates(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from DATA.States";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.States = DataReaderMapToList<PrimaryDataViewModel.OldState>(reader)
                        .Select(s => new State
                        {
                            StateId = s.StateId,
                            StateName = s.StateName,
                            CountryId = s.CountryId,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportStates(ref PrimaryDataViewModel vm)
        {
            _context.States.AddOrUpdate(s => s.StateId, vm.States.ToArray());

            vm.Messages += $"{vm.States.Count()} States imported successfully.";
        }

        public void GetOldCitites(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from DATA.Cities";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Cities = DataReaderMapToList<PrimaryDataViewModel.OldCity>(reader)
                        .Select(s => new City
                        {
                            CityId = s.CityId,
                            CityName = s.CityName,
                            CountryId = s.CountryId,
                            CityPriority = s.CityPriority,
                            StateId = s.StateId,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportCitites(ref PrimaryDataViewModel vm)
        {
            _context.Cities.AddOrUpdate(s => s.CityId, vm.Cities.ToArray());

            vm.Messages += $"{vm.Cities.Count()} Cities imported successfully.";
        }

        public void GetOldNationalities(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from DATA.Nationalities";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Nationalities = DataReaderMapToList<PrimaryDataViewModel.OldNationality>(reader)
                        .Select(s => new Nationality
                        {
                            IP = s.IP,
                            ModifiedBy = s.ModifiedBy,
                            Nationality1 = s.Nationality,
                            NationalityId = s.NationalityId
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportNationalities(ref PrimaryDataViewModel vm)
        {
            _context.Nationalities.AddOrUpdate(s => s.NationalityId, vm.Nationalities.ToArray());

            vm.Messages += $"{vm.Nationalities.Count()} Nationalities imported successfully.";
        }

        public void GetOldDocumentTypes(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from DATA.DocumentTypes";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.DocumentTypes = DataReaderMapToList<PrimaryDataViewModel.OldDocumentType>(reader)
                        .Select(s => new DocumentType
                        {
                            DocumentTypeFor = s.DocumentTypeFor,
                            DocumentTypeId = s.DocumentTypeId,
                            DocumentTypeName = s.DocumentTypeName,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportDocumentTypes(ref PrimaryDataViewModel vm)
        {
            _context.DocumentTypes.AddOrUpdate(s => s.DocumentTypeId, vm.DocumentTypes.ToArray());

            vm.Messages += $"{vm.DocumentTypes.Count()} Document Types imported successfully.";
        }

        public void GetOldProfession(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from DATA.Profession";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Professions = DataReaderMapToList<PrimaryDataViewModel.OldProfession>(reader)
                        .Select(s => new Profession
                        {
                            ModifiedBy = s.ModifiedBy,
                            IP = s.IP,
                            ProfessionId = s.ProfessionId,
                            ProfessionName = s.ProfessionName,
                            Prev__ID = s.Prev__ID,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportProfessions(ref PrimaryDataViewModel vm)
        {
            _context.Professions.AddOrUpdate(s => s.ProfessionId, vm.Professions.Where(s => s.ProfessionName != "").ToArray());

            vm.Messages += $"{vm.Professions.Count()} Professions imported successfully.";
        }

        public void GetOldRegions(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from DATA.Regions";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Regions = DataReaderMapToList<PrimaryDataViewModel.OldRegion>(reader)
                        .Select(s => new Region
                        {
                            IP = s.IP,
                            ModifiedBy = s.ModifiedBy,
                            RegionId = s.RegionId,
                            RegionName = s.RegionName,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportRegions(ref PrimaryDataViewModel vm)
        {
            _context.Regions.AddOrUpdate(s => s.RegionId, vm.Regions.ToArray());

            vm.Messages += $"{vm.Regions.Count()} Regions imported successfully.";
        }

        public void GetOldReligions(ref PrimaryDataViewModel vm)
        {
            string connetionString = $"Data Source={vm.Credential.Server};Initial Catalog={vm.Credential.Database};User ID={vm.Credential.UserName};Password={vm.Credential.Password}";
            SqlConnection cnn = new SqlConnection(connetionString);
            string query = $@"Select * from DATA.Religions";
            try
            {
                cnn.Open();
                SqlCommand command = new SqlCommand(query, cnn);
                using (IDataReader reader = command.ExecuteReader())
                {
                    vm.Religions = DataReaderMapToList<PrimaryDataViewModel.OldReligion>(reader)
                        .Select(s => new Religion
                        {
                            ModifiedBy = s.ModifiedBy,
                            IP = s.IP,
                            ReligionId = s.ReligionId,
                            ReligionName = s.ReligionName,
                        }).ToList();
                }
                command.Dispose();
                cnn.Close();
            }
            catch (Exception ex)
            {
                vm.Error += ex.GetExceptionMessages();
            }
        }

        public void ImportReligions(ref PrimaryDataViewModel vm)
        {
            _context.Religions.AddOrUpdate(s => s.ReligionId, vm.Religions.ToArray());

            vm.Messages += $"{vm.Religions.Count()} Religions imported successfully.";
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
    }
}