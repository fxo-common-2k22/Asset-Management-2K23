using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
namespace FAPP.Controllers
{
    public class MembershipController : BaseController
    {
        // GET: Membership
        public JsonResult InsertInMemberShipModules(Module ex)
        {
            int result = 1;
            try
            {
                result = ProceduresModel.InsertInMembershipModules(db, ex);
                result = ex.ModuleId;
                var studentArea = db.Forms.Where(s => s.FormID == 2011130550).FirstOrDefault();
                if (studentArea != null) {
                    studentArea.FormURL = "/Students/Dashboard";
                    db.SaveChanges();
                }
                var CommunicationCenter = db.Forms.Where(s => s.FormName == "Communication Center").FirstOrDefault();
                if (CommunicationCenter != null) {
                    CommunicationCenter.FormURL = "/Academics/Communication/Manage";
                    CommunicationCenter.ParentForm = 2011130550;
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand($@"delete from Membership.Forms where ParentForm = {CommunicationCenter.FormID}");
                }
                var studentManagement = db.Forms.Where(s => s.FormID == 20110303).FirstOrDefault();
                if (studentManagement != null) {
                    studentManagement.FormURL = "/Students/Students/Manage";
                    db.SaveChanges();
                }
                var studentReports = db.Forms.Where(s => s.FormID == 20110606).FirstOrDefault();
                if (studentReports != null) {
                    studentReports.FormURL = "/Students/StudentReports";
                    db.SaveChanges();
                }
                var studentAllReports = db.Forms.Where(s => s.ParentForm == 20110606).ToList();
                if (studentAllReports != null)
                {
                    studentAllReports.ForEach(x => x.FormURL = x.FormURL.Replace("/Academics/", "/Students/"));
                    db.SaveChanges();
                }
                var ApplicantForms = db.Forms.Where(s => s.FormID == 20110600 || s.FormID == 20110601 || s.FormID == 20110602 || s.FormID == 20110603 || s.FormID == 20110604).ToList();
                if (ApplicantForms != null)
                {
                    ApplicantForms.ForEach(x => x.FormURL = x.FormURL.Replace("/Academics/", "/Students/"));
                    db.SaveChanges();
                }
               var studentEdit = db.Forms.Where(s => s.FormID == 20110302).FirstOrDefault();
                if (studentEdit != null) {
                    studentEdit.FormURL = "/Students/Students/Edit";
                    db.SaveChanges();
                }
               
                var formList = db.Forms.Where(s => s.ParentForm == 201001).Where(s => s.FormID == 201103 || s.FormID == 201105 || s.FormID == 201106 || s.FormID == 201117 || s.FormID == 201108 || s.FormID == 201111 || s.FormID == 201115 || s.FormID == 201116).ToList();
                if (formList != null)
                {
                    formList.ForEach(s => s.ParentForm = 2011130550);
                    db.SaveChanges();
                }
                var certificateFormList = db.Forms.Where(s => s.ParentForm == 110010 || s.ParentForm == 201115).ToList();
                if (certificateFormList != null)
                {
                    certificateFormList.ForEach(s => s.isActive = "No");
                    db.SaveChanges();
                }
                var teachingStaffForms = db.Forms.Where(s => s.ParentForm == 201112).Where(s => s.FormID == 20111214 || s.FormID == 20111215 || s.FormID == 20111216 || s.FormID == 20111208).ToList();
                if (teachingStaffForms != null)
                {
                    teachingStaffForms.ForEach(s => s.ParentForm = 201001);
                    db.SaveChanges();
                }
                var ptmForm = db.Forms.Where(s => s.FormID == 20111215).FirstOrDefault();
                if (ptmForm != null) {
                    ptmForm.FormURL = "/Academics/PTM";
                    db.SaveChanges();
                }
                var resource = db.Forms.Where(s => s.FormID == 20111214).FirstOrDefault();
                if (resource != null)
                {
                    resource.FormName = "E-Learning/Resources";
                    resource.MenuText = "E-Learning/Resources";
                    db.SaveChanges();
                }
                var ptm = db.Forms.Where(s => s.FormID == 20111215).FirstOrDefault();
                if (ptm != null)
                {
                    ptm.FormName = "Parent Teacher Meeting";
                    ptm.MenuText = "Parent Teacher Meeting";
                    db.SaveChanges();
                }
                var diary = db.Forms.Where(s => s.FormID == 20111216).FirstOrDefault();
                if (diary != null)
                {
                    diary.FormName = "Diary Management";
                    diary.MenuText = "Diary Management";
                    db.SaveChanges();
                }
                var resultSetup = db.Forms.Where(s => s.FormID == 20110403).FirstOrDefault();
                if (resultSetup != null)
                {
                    resultSetup.FormURL = "/Academics/SessionManagement/ResultRating";
                    db.SaveChanges();
                }
                var ExamSetup = db.Forms.Where(s => s.FormID == 20111001).FirstOrDefault();
                if (ExamSetup != null)
                {
                    ExamSetup.FormURL = "/Academics/SessionManagement/ResultRating";
                    db.SaveChanges();
                }
                var applicantSetup = db.Forms.Where(s => s.FormID == 20110605).FirstOrDefault();
                if (applicantSetup != null)
                {
                    applicantSetup.FormURL = "/Academics/SessionManagement/Applicant";
                    db.SaveChanges();
                }
                var StudentManagementSetup = db.Forms.Where(s => s.FormID == 20110305).FirstOrDefault();
                if (StudentManagementSetup != null)
                {
                    StudentManagementSetup.FormURL = "/Academics/SessionManagement/Groups";
                    db.SaveChanges();
                }
                var StudentsBulkActions = db.Forms.Where(s => s.FormID == 201111).FirstOrDefault();
                if (StudentsBulkActions != null)
                {
                    StudentsBulkActions.FormURL = "/Students/BulkActions/Index";
                    db.SaveChanges();
                }
                var StudentsBulkActionsDashboard = db.Forms.Where(s => s.FormID == 20111100).FirstOrDefault();
                if (StudentsBulkActionsDashboard != null)
                {
                    StudentsBulkActionsDashboard.FormURL = "/Students/BulkActions/Index";
                    db.SaveChanges();
                }
                var StudentsBulkActionsImport = db.Forms.Where(s => s.FormID == 20111101).FirstOrDefault();
                if (StudentsBulkActionsImport != null)
                {
                    StudentsBulkActionsImport.FormURL = "/Students/BulkActions/ImportStudents";
                    db.SaveChanges();
                }

                var StudentsBulkActionsGroupTeacherAssgn = db.Forms.Where(s => s.FormID == 20111103).FirstOrDefault();
                if (StudentsBulkActionsGroupTeacherAssgn != null)
                {
                    StudentsBulkActionsGroupTeacherAssgn.FormURL = "/Students/BulkActions/Groups";
                    db.SaveChanges();
                }
                var DiscountCategory = db.Forms.Where(s => s.ParentForm == 201001).Where(s => s.FormID == 2011120681).FirstOrDefault();
                if (DiscountCategory != null) {
                    DiscountCategory.ParentForm = 301004;
                    DiscountCategory.FormName = "Discount Category";
                    DiscountCategory.MenuText = "Discount Category";
                    db.SaveChanges();
                }
                var studentViewForm = db.Forms.Where(s => s.FormID == 20111102).FirstOrDefault();
                if (studentViewForm != null)
                {
                    studentViewForm.FormURL = "/Students/StudentView/Index";
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                result = 0;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetMainMemberShipForms() {
            int result = 1;
            try
            {
                var DashboardForm = db.Forms.Where(s => s.FormID == 1).FirstOrDefault();
                if (DashboardForm != null)
                {
                    DashboardForm.isActive = "No";
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                result = 0;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}