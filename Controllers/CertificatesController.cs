using FAPP.BLL.Certificates;
using FAPP.DAL;
using FAPP.Helpers;
using FAPP.Model;
using FAPP.Service;
using FAPP.ViewModel;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FAPP.Controllers
{
    public class CertificatesController : BaseController
    {
        private static SearchIssuedCertificates SearchIssuedCertificates;

        public ActionResult Dashboard()
        {
            var vm = new MessagingVM();
            GetChartData(ref vm);
            return View(vm);
        }

        private void GetChartData(ref MessagingVM vm)
        {
            int? moduleId = vm.ModuleId;
            var types = db.TemplateTypes.Include(m => m.Module)
                .Where(s => s.ModuleId == (moduleId.HasValue && moduleId.Value > 0 ? moduleId.Value : s.ModuleId))
                .ToList();
            var typesIds = types.Select(s => (short?)s.TemplateTypeId).ToList();
            var smsQueue = db.Certificates.Where(q => typesIds.Contains(q.TemplateTypeId));
            vm.SmsChart = new Dictionary<string, int>();
            foreach (var type in types)
            {
                var count = smsQueue.Where(q => q.TemplateTypeId == type.TemplateTypeId).Count();
                if (!vm.SmsChart.ContainsKey(type.TemplateTypeName))
                {
                    vm.SmsChart.Add(type.TemplateTypeName, count);
                }
            }

        }

        //id = moduleid
        public ActionResult Templates(int? id)
        {
            TempData["TemplateTypes"] = db.TemplateTypes.Where(c => c.ModuleId == id).ToList();
            if (id.HasValue)
            {
                SessionHelper.ModuleId = id.Value;
            }
            var vm = new MessagingVM();
            vm.ModuleId = id;
            vm.ModuleName = db.Modules.Where(s => s.ModuleId == id).Select(u => u.ModuleName).FirstOrDefault();
            GetChartData(ref vm);
            return View(vm);
        }
        // GET: Academics/Certificates
        public ActionResult Manage(int? id)
        {
            //ViewBag.TemplateTypeId = id;
            ViewBag.ModuleName = db.Modules.Where(u => u.ModuleId == id).Select(u => u.ModuleName).FirstOrDefault();
            return View(db.Certificates.Where(s => s.TemplateType.ModuleId == id).OrderByDescending(t => t.CreatedOn).OrderByDescending(t => t.ModifiedOn).ToList());
        }

        [HttpPost]
        public async Task<ActionResult> Print(MediaTemplatesViewModel vm)
        {
            vm.Branch = await db.Branches.FindAsync(SessionHelper.BranchId);
            var id = vm.SelectedCertificates.FirstOrDefault();
            if (vm.ModuleId == 9)
            {
                var employeeCert = db.EmployeeCertificates.Find(id);
                vm.Certificate = await db.Certificates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).FirstOrDefaultAsync(c => c.CertificateId == employeeCert.CertificateTypeId);
                vm.TemplateType = vm.Certificate.TemplateType;
                TempData["TypeId"] = vm.Certificate.TemplateTypeId;
                TempData["CertId"] = vm.Certificate.CertificateId;
                TempData["Issued"] = "Issued";
                vm.IssuedEmployeeCertificatesList = await db.EmployeeCertificates.Where(c => vm.SelectedCertificates.Contains(c.CertificateId)).ToListAsync();

            }
            else
            {
                var studentCert = db.StudentCertificates.Find(id);
                vm.Certificate = await db.Certificates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).FirstOrDefaultAsync(c => c.CertificateId == studentCert.CertificateTypeId);
                vm.TemplateType = vm.Certificate.TemplateType;
                TempData["TypeId"] = vm.Certificate.TemplateTypeId;
                TempData["CertId"] = vm.Certificate.CertificateId;
                TempData["Issued"] = "Issued";
                vm.IssuedCertificatesList = await db.StudentCertificates.Where(c => vm.SelectedCertificates.Contains(c.CertificateId)).ToListAsync();

            }
            return View(vm);
        }


        [HttpGet]
        public async Task<ActionResult> DetailsModal(int? id)
        {
            var vm = new MediaTemplatesViewModel();
            if (id.HasValue)
            {

                vm.Certificate = await db.Certificates.Include(c => c.TemplateType).FirstOrDefaultAsync(c => c.CertificateId == id);
                var type = vm.Certificate.TemplateType;
                vm.ViewFields = new List<string>();
                if (!string.IsNullOrWhiteSpace(type.ViewField))
                {
                    var fields = type.ViewField.Split(',');
                    vm.ViewFields = fields.ToList();
                }
            }
            else
            {
                vm.Certificate = new Certificate();
            }
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/Certificates/_PartialCertificateDetails.cshtml", vm),
                TargetId = "frmModalContent",
                ModalId = "frmModal",
                Messages = ""
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> AddUpdateCertificateModal(int? id)
        {
            var vm = new MediaTemplatesViewModel();
            if (id.HasValue)
            {

                vm.Certificate = await db.Certificates.Include(c => c.TemplateType).FirstOrDefaultAsync(c => c.CertificateId == id);
                var type = vm.Certificate.TemplateType;
                vm.ViewFields = new List<string>();
                if (!string.IsNullOrWhiteSpace(type.ViewField))
                {
                    var fields = type.ViewField.Split(',');
                    vm.ViewFields = fields.ToList();
                }
                vm.TemplateTypeList = await db.TemplateTypes.Where(t => t.ModuleId == 1).ToListAsync();
            }
            else
            {
                vm.Certificate = new Certificate();
            }
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/Certificates/_PartialCertificateFrmModal.cshtml", vm),
                TargetId = "frmModalContent",
                ModalId = "frmModal",
                Messages = ""
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> VerifyCertificateRefNo(string no)
        {
            bool isOk = true;
            string msg = string.Empty;
            if (!string.IsNullOrWhiteSpace(no.ToUpper().Trim()))
            {
                var cert = await db.Certificates.Where(c => c.RefNoFormat == no.ToUpper().Trim()).ToListAsync();
                if (cert != null)
                {
                    if (cert.Count > 0)
                    {
                        isOk = false;
                        msg = "Ref No Already Exists, try another";
                    }
                }
            }
            var result = new
            {
                IssSuccess = isOk,
                Error = msg
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> AddEditFrmModal(MediaTemplatesViewModel vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            try
            {

                var update = db.Certificates.Find(vm.Certificate.CertificateId);
                update.Title = vm.Certificate.Title;
                update.RefNoFormat = vm.Certificate.RefNoFormat;
                update.TemplateTypeId = vm.Certificate.TemplateTypeId;
                update.FaIcon = vm.Certificate.FaIcon;
                update.ModifiedBy = SessionHelper.UserId;
                update.ModifiedOn = DateTime.Now;
                db.Entry(update).State = EntityState.Modified;

                await db.SaveChangesAsync();
                message = "Certificate Saved successfully...";
                var type = await db.TemplateTypes.FindAsync(vm.Certificate.TemplateTypeId);
                vm.ViewFields = new List<string>();
                if (!string.IsNullOrWhiteSpace(type.ViewField))
                {
                    var fields = type.ViewField.Split(',');
                    vm.ViewFields = fields.ToList();
                }
                vm.TemplateTypeList = await db.TemplateTypes.Where(t => t.ModuleId == 1).ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    error = ex.Message;
                }
            }
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/Certificates/_PartialCertificateFrmModal.cshtml", vm),
                GridId = "frmModalContent",
                ModalId = "frmModal",

                Reset = "false",
                Messages = message,
                Error = error
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //id: ModuleId, id2 = templateTypeId 

        [Route("Certificates/AddEdit/{module}/{type}/{id}")]
        public async Task<ActionResult> AddEdit(int? module, int? type, int? id)
        {
            var vm = new MediaTemplatesViewModel();
            vm.Certificate = new Certificate();
            if (id.HasValue)
            {
                vm.Certificate = await db.Certificates.Include(c => c.TemplateType).FirstOrDefaultAsync(c => c.CertificateId == id);
                if (vm.Certificate != null)
                {
                    vm.Certificate.TemplateTypeId = Convert.ToInt16(SessionHelper.TemplateTypeId);
                    vm.TemplateType = vm.Certificate.TemplateType;

                }
            }
            else
            {
                vm.Certificate = new Certificate();
                SessionHelper.TemplateTypeId = type ?? 0;
                vm.TemplateType = db.TemplateTypes.Find(SessionHelper.TemplateTypeId);
                vm.Certificate.TemplateTypeId = Convert.ToInt16(SessionHelper.TemplateTypeId);
            }
            vm.ViewFields = new List<string>();
            if (vm.TemplateType != null && !string.IsNullOrWhiteSpace(vm.TemplateType.ViewField))
            {
                var fields = vm.TemplateType.ViewField.Split(',');
                vm.ViewFields = fields.ToList();
            }
            //vm.TemplateTypeList = await db.TemplateTypes.Where(t => t.ModuleId == 1).ToListAsync();
            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> AddEdit(MediaTemplatesViewModel vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            try
            {
                if (vm.Certificate.CertificateId == 0)
                {
                    vm.Certificate.CreatedOn = DateTime.Now;
                    vm.Certificate.CreatedBy = SessionHelper.UserId;
                    vm.Certificate.BranchId = Convert.ToInt16(SessionHelper.BranchId);
                    vm.Certificate.ByDefault = false;
                    vm.Certificate.IP = Request.UserHostAddress;
                    db.Certificates.Add(vm.Certificate);
                    //var addLnk = new CertificateActionLinks();
                    //addLnk.CertificateId = vm.Certificate.CertificateId;
                    //addLnk.FormId = SessionHelper.FormId;
                    //var link = db.CertificateActionLinks.Add(addLnk);
                }
                else
                {
                    var update = db.Certificates.Find(vm.Certificate.CertificateId);
                    update.CertificateContent = vm.Certificate.CertificateContent;
                    update.ModifiedBy = SessionHelper.UserId;
                    update.ModifiedOn = DateTime.Now;
                    update.RefNoFormat = vm.Certificate.RefNoFormat;
                    update.Title = vm.Certificate.Title;
                    update.FaIcon = vm.Certificate.FaIcon;
                    db.Entry(update).State = EntityState.Modified;
                    vm.Certificate = update;
                }
                await db.SaveChangesAsync();
                message = "Certificate Saved successfully...";
                var type = await db.TemplateTypes.FindAsync(vm.Certificate.TemplateTypeId);
                vm.ViewFields = new List<string>();
                if (!string.IsNullOrWhiteSpace(type.ViewField))
                {
                    var fields = type.ViewField.Split(',');
                    vm.ViewFields = fields.ToList();
                }
                vm.TemplateTypeList = await db.TemplateTypes.Where(t => t.ModuleId == 1).ToListAsync();
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    error = ex.Message;
                }
            }
            var firstOrDefault = db.TemplateTypes.Find(vm.Certificate.TemplateTypeId);
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/Certificates/_CertificateTemplateFrm.cshtml", vm),
                GridId = "certGrid",
                Url = "/Certificates/AddEdit/" + firstOrDefault.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" +
                           firstOrDefault.TemplateTypeName.Trim().Replace(" ", String.Empty) + "/" + vm.Certificate.CertificateId,
                Reset = "false",
                Messages = message,
                Error = error,
                RedirectTo = "/Certificates/AddEdit/" + firstOrDefault.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" +
                           firstOrDefault.TemplateTypeName.Trim().Replace(" ", String.Empty) + "/" + vm.Certificate.CertificateId,

            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetMediaTypeFields(int? id)
        {
            var type = await db.TemplateTypes.FindAsync(id);
            var list = new List<string>();
            if (!string.IsNullOrWhiteSpace(type.ViewField))
            {
                var fields = type.ViewField.Split(',');
                list = fields.ToList();
            }

            return PartialView("~/Views/Certificates/_PartialFields.cshtml", list);
        }

        public async Task<ActionResult> Issue(int? id)
        {
            var vm = new MediaTemplatesViewModel();
            var dds = new Dropdowns();
            if (id.HasValue)
            {
                vm.IssueCertificate = new IssueCertificateViewModel();
                var certificate = await db.Certificates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).FirstOrDefaultAsync(c => c.CertificateId == id);
                vm.Certificate = certificate;
                vm.ModuleId = (int)certificate.TemplateType.ModuleId;
                vm.TemplateType = certificate.TemplateType;
                TempData["TypeId"] = certificate.TemplateTypeId;
                TempData["CertId"] = certificate.CertificateId;
                TempData["Issued"] = "Issue";
                vm.IssueCertificate.CertificateId = certificate.CertificateId;
            }

            if (vm.ModuleId == 9)
            {
                ViewBag.DepartmentsDD = await dds.GetDepartmentsDD();
            }
            else
            {
                ViewBag.GroupsDD = await dds.GetGroupsDD();
            }

            //vm.ModuleId = SessionHelper.ModuleId;
            ViewBag.CertsDD = await dds.GetCertificatesDD();
            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> Issue(MediaTemplatesViewModel vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            try
            {
                var certificate = await db.Certificates.Include(c => c.TemplateType).FirstOrDefaultAsync(c => c.CertificateId == vm.IssueCertificate.CertificateId);
                CertificatesBL certificates = new CertificatesBL();
                int lastNo = await certificates.LastIssueCertificateNumber(db, certificate, vm.ModuleId);
                if (vm.ModuleId == 9)
                {
                    foreach (var item in vm.IssueCertificate.SelectedEmployees)
                    {
                        lastNo++;
                        var issueNoLast = (lastNo).ToString().PadLeft(4, '0');
                        certificates.PrepareCertificate(db, vm, certificate, lastNo, item, 9);
                    }
                    await db.SaveChangesAsync();
                    message = "Certificates Issued successfully...";
                }
                else
                {
                    foreach (var item in vm.IssueCertificate.SelectedStudentSessionIds)
                    {
                        lastNo++;
                        var issueNoLast = (lastNo).ToString().PadLeft(4, '0');
                        certificates.PrepareCertificate(db, vm, certificate, lastNo, 0, 1, item);
                    }
                    await db.SaveChangesAsync();
                    message = "Certificates Issued successfully...";
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    error = "Somthing went wrong! Try again Later.";
                }
            }
            if (vm.ModuleId == 9)
            {
                var query = db.Employees.Where(s => s.DepartmentId == vm.IssueCertificate.DepartmentId);
                if (vm.IssueCertificate.IncludeInactive == false)
                {
                    query = query.Where(s => s.Active == true);
                }
                vm.IssueCertificate.Employees = await query.ToListAsync();
                var result = new
                {
                    PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/Certificates/_PartialIssueCertificateEmployees.cshtml", vm),
                    GridId = "certsGrid",
                    Reset = "false",
                    Messages = message,
                    Error = error
                };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var query = db.StudentSessions.Include(s => s.Student).Where(s => s.GroupId == vm.IssueCertificate.GroupId);
                if (vm.IssueCertificate.IncludeInactive == false)
                {
                    query = query.Where(s => s.Active == true && s.SessionActive == true && s.Student.Active == true);
                }
                vm.IssueCertificate.Students = await query.ToListAsync();
                var result = new
                {
                    PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/Certificates/_PartialIssueCertificate.cshtml", vm),
                    GridId = "certsGrid",
                    Reset = "false",
                    Messages = message,
                    Error = error
                };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }



        [HttpPost]
        public async Task<ActionResult> DeleteCertificate(int id)
        {
            string message = string.Empty;
            string error = string.Empty;
            var entity = await db.StudentCertificates.FindAsync(id);
            if (entity != null)
            {

                db.StudentCertificates.Remove(entity);
                try
                {
                    await db.SaveChangesAsync();
                    message = "Certificate has been deleted successfully...";
                }
                catch (Exception)
                {
                    error = "Certificate cannot be deleted because its in use...";
                }

            }
            else
            {
                error = "Certificate not found!";
            }
            string partialView = string.Empty;
            var vm = new MediaTemplatesViewModel();
            var list =  GetIssuedCertificates();
            vm.IssuedCertificatesList = await list.OrderByDescending(c => c.CertificateId).ToListAsync();
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_IssuedCertificatesGrid", vm),
                Messages = message,
                Error = error
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SearchStudents(MediaTemplatesViewModel vm)
        {
            if (vm.ModuleId == 9)
            {
                vm.IssueCertificate.Employees = new List<Employee>();
                vm.IssueCertificate.SelectedEmployees = new List<int>();
                var query = db.Employees.Where(s => s.DepartmentId == vm.IssueCertificate.DepartmentId);
                if (vm.IssueCertificate.IncludeInactive == false)
                {
                    query = query.Where(s => s.Active == true);
                }
                vm.IssueCertificate.Employees = await query.ToListAsync();
                vm.IssueDate = DateTime.Now;

                var result = new
                {
                    PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "~/Views/Certificates/_PartialIssueCertificateEmployees.cshtml", vm),
                    Messages = "Records has been loaded successfully.",
                    GridId = "certsGrid",
                    LoadFile = "/scripts/eakroko.min.js"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                vm.IssueCertificate.Students = new List<StudentSession>();
                vm.IssueCertificate.SelectedStudents = new List<int>();
                var query = db.StudentSessions.Include(s => s.Student).Where(s => s.GroupId == vm.IssueCertificate.GroupId);
                if (vm.IssueCertificate.IncludeInactive == false)
                {
                    query = query.Where(s => s.Student.Active == true && s.Active == true && s.SessionActive == true);
                }
                vm.IssueCertificate.Students = await query.ToListAsync();
                vm.IssueDate = DateTime.Now;
                var result = new
                {
                    PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "~/Views/Certificates/_PartialIssueCertificate.cshtml", vm),
                    Messages = "Records has been loaded successfully.",
                    GridId = "certsGrid",
                    LoadFile = "/scripts/eakroko.min.js"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<ActionResult> Issued(int? id)
        {
            var vm = new MediaTemplatesViewModel();
            var dds = new Dropdowns();
            if (id.HasValue)
            {
                vm.Certificate = await db.Certificates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).FirstOrDefaultAsync(c => c.CertificateId == id);
                vm.ModuleId = (int)vm.Certificate.TemplateType.ModuleId;
            }
            else
            {
                ViewBag.CertsDD = await dds.GetCertificatesDD();
            }
            if (vm.ModuleId == 9)
            {
                ViewBag.DepartmentsDD = await dds.GetDepartmentsDD();
            }
            else
            {
                ViewBag.GroupsDD = await dds.GetGroupsDD();
            }
            //vm.ModuleId = SessionHelper.ModuleId;


            vm.TemplateType = vm.Certificate.TemplateType;
            TempData["TypeId"] = vm.Certificate.TemplateTypeId;
            TempData["CertId"] = vm.Certificate.CertificateId;
            TempData["Issued"] = "Issued";
            vm.SearchIssuedCertificates = new SearchIssuedCertificates();
            vm.SearchIssuedCertificates.CertificateTypeId = id;
            //vm.IssuedCertificatesList = await db.StudentCertificates.Include(c=>c.Student).OrderByDescending(t => t.IssuedOn).ThenByDescending(t => t.CertificateId).ToListAsync();
            vm.SelectedCertificates = new List<long>();
            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> SearchIssued(MediaTemplatesViewModel vm)
        {
            SearchIssuedCertificates = new SearchIssuedCertificates();
            SearchIssuedCertificates = vm.SearchIssuedCertificates;

            if (vm.ModuleId == 9)
            {
                var list = await GetIssuedEmployeeCertificates();
                vm.IssuedEmployeeCertificatesList = await list.OrderByDescending(c => c.CertificateId).ToListAsync();
                var result = new
                {
                    PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_IssuedEmployeeCertificatesGrid", vm),
                    Messages = "Records has been loaded successfully.",
                    GridId = "certGrid",
                    LoadFile = "/scripts/eakroko.min.js"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var list =  GetIssuedCertificates();
                vm.IssuedCertificatesList = await list.OrderByDescending(c => c.CertificateId).ToListAsync();
                var result = new
                {
                    PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_IssuedCertificatesGrid", vm),
                    Messages = "Records has been loaded successfully.",
                    GridId = "certGrid",
                    LoadFile = "/scripts/eakroko.min.js"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        private IQueryable<StudentCertificate> GetIssuedCertificates()
        {
            var query = db.StudentCertificates.Include(s => s.Student).Include(s => s.IssuedByUser).Include(s => s.Group);
            if (SearchIssuedCertificates.GroupId != null)
            {
                query = query.Where(c => c.GroupId == SearchIssuedCertificates.GroupId);
            }
            if (SearchIssuedCertificates.CertificateTypeId != null)
            {
                query = query.Where(c => c.CertificateTypeId == SearchIssuedCertificates.CertificateTypeId);
            }
            if (!string.IsNullOrWhiteSpace(SearchIssuedCertificates.SearchStudent))
            {
                query = query.Where(c => c.IssueNo.Contains(SearchIssuedCertificates.SearchStudent) || c.Student.RollNumber == Convert.ToInt32(SearchIssuedCertificates.SearchStudent) || c.Student.RegistrationNumber == SearchIssuedCertificates.SearchStudent);
            }

            if (SearchIssuedCertificates.StudentName != null)
            {
                query = query.Where(c => c.Student.StudentName.ToLower().Contains(SearchIssuedCertificates.StudentName.ToLower()));
            }
            return  query;
        }
        private async Task<IQueryable<EmployeeCertificate>> GetIssuedEmployeeCertificates()
        {
            var list = db.EmployeeCertificates.Include(s => s.Employee).Include(s => s.Employee.Department).Include(s => s.IssuedByUser);
            if (SearchIssuedCertificates.DepartmentId != null)
            {
                var EmployeeList = await db.Employees.Where(s => s.DepartmentId == SearchIssuedCertificates.DepartmentId)
                    .Select(s => s.EmployeeId).ToListAsync();
                list = list.Where(c => EmployeeList.Contains(c.EmployeeId));
            }
            if (SearchIssuedCertificates.CertificateTypeId != null)
            {
                list = list.Where(c => c.CertificateTypeId == SearchIssuedCertificates.CertificateTypeId);
            }
            if (!string.IsNullOrWhiteSpace(SearchIssuedCertificates.SearchEmployee))
            {
                list = list.Where(c => c.IssueNo.Contains(SearchIssuedCertificates.SearchEmployee) || c.Employee.EmpNo == SearchIssuedCertificates.SearchEmployee || c.Employee.RegNo == SearchIssuedCertificates.SearchEmployee || c.Employee.EmpName.ToLower().Contains(SearchIssuedCertificates.SearchEmployee.ToLower()));
            }
            return list;
        }
    }
}