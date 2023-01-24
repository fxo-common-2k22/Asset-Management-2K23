using FAPP.BLL;
using FAPP.Classes.Messaging.DTOS;
using FAPP.DAL;
using FAPP.dbviews;
using FAPP.Helpers;
using FAPP.Model;
using FAPP.Service;
using FAPP.ViewModel;
using PagedList;
using SmartFormat;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Z.EntityFramework.Plus;
using StudentNumUpdateToolViewModel = FAPP.ViewModel.StudentNumUpdateToolViewModel;

namespace FAPP.Controllers
{
    public class MessagingController : BaseController
    {
        private static int pageNo = 1;
        public static Guid _GroupId;
        public static SearchMessageVM SearchMessage;
        private short _BranchId = SessionHelper.BranchId;
        private Guid _SessionId = SessionHelper.CurrentSessionId;

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
            var smsQueue = db.SmsQueues.Where(q => typesIds.Contains(q.TemplateTypeId));
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

        public async Task<ActionResult> NumCorrectionTool()
        {
            var vm = new StudentNumUpdateToolViewModel();
            await vm.FillDD(SessionHelper.CurrentSessionId);
            var correct =
                db.Students.Count(p => p.Active == true && p.FatherMobileNumber.Length == 12);
            TempData["correctNum"] = correct;
            var wrong =
                db.Students.Count(p => p.Active == true && p.FatherMobileNumber.Length < 12);
            TempData["invalidNum"] = wrong;
            TempData["noNum"] = db.Students.Count(p => p.Active == true && p.FatherMobileNumber == null);
            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> NumCorrectionTool(int studentId, string mobNo)
        {
            string message = string.Empty;
            string status = "success";
            var correct =
                db.Students.Count(p => p.Active == true && p.FatherMobileNumber.Length == 12);
            TempData["correctNum"] = correct;
            var wrong =
                db.Students.Count(p => p.Active == true && p.FatherMobileNumber.Length <= 12);
            TempData["invalidNum"] = wrong;
            var student = await db.Students.FindAsync(studentId);
            if (student != null)
            {
                student.FatherMobileNumber = mobNo.Trim();
                try
                {
                    await db.SaveChangesAsync();
                    message = $"Updated successfully.";
                }
                catch (Exception)
                {
                    message = $"Failed.";
                    status = "error";
                }
            }
            var result = new
            {
                Status = status,
                Message = message,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> FixAll()
        {
            string message = string.Empty;
            string status = "success";
            try
            {
                db.Students
                    .Where(p => p.Active == true && p.FatherMobileNumber.Length >= 10)
                    .Update(p => new Student()
                    {
                        FatherMobileNumber = p.FatherMobileNumber.Trim().Replace("-", "").Replace(" ", "")
                    });

                db.Students
                    .Where(p => p.Active == true && p.FatherMobileNumber.StartsWith("03"))
                    .Update(p => new Student()
                    {
                        FatherMobileNumber = "92" + p.FatherMobileNumber.Remove(0, 1)
                    });

                db.Students
                    .Where(p => p.Active == true && p.FatherMobileNumber.StartsWith("3"))
                    .Update(p => new Student()
                    {
                        FatherMobileNumber = "92" + p.FatherMobileNumber
                    });
                await db.SaveChangesAsync();
                message = $"Updated successfully.";
            }
            catch (Exception)
            {
                message = $"Failed.";
                status = "error";
            }

            var result = new
            {
                Status = status,
                Message = message,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //over load function
        public async Task<ActionResult> LoadStudents(StudentNumUpdateToolViewModel vm)
        {
            await vm.FillDD(SessionHelper.CurrentSessionId);
            vm.Students = vm.GetStudents(vm.GroupId, SessionHelper.BranchId);
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_StudentsList", vm),
                GridId = "studentsList",
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Batch(int? id, int? page)
        {
            var dds = new Dropdowns();
            ViewBag.ModuleName = db.Modules.Where(s => s.ModuleId == id).Select(s => s.ModuleName).FirstOrDefault();
            ViewBag.StatusDD = dds.GetPendingSentFailedDD();
            ViewBag.TypesDD = await dds.GetTemplateTypesDD(id);
            page = page ?? 1;
            pageNo = page.Value;

            if (SearchMessage == null)
            {
                SearchMessage = new SearchMessageVM();
            }
            SearchMessage.Batch = null;
            var vm = new MessagingVM();
            vm.ModuleId = id;
            vm = SearchBatch(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult SearchBatches(MessagingVM vm)
        {
            SearchMessage = vm.SearchMessage;
            vm = SearchBatch(vm);
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialSmsBatchGrid", vm),
                Messages = "Records has been loaded successfully.",
                GridId = "smsGrid",
                //LoadFile = "/scripts/eakroko.min.js"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public MessagingVM SearchBatch(MessagingVM vm)
        {
            // var list = db.SmsQueues.Include(a => a.Template).Include(a=>a.TemplateType);
            var blist = from b in db.SmsQueues
                        join t in db.Templates on b.TemplateId equals t.TemplateId
                        join tt in db.TemplateTypes on b.TemplateTypeId equals tt.TemplateTypeId
                        where
                        b.Batch == (SearchMessage.Batch.HasValue ? SearchMessage.Batch : b.Batch) && b.BranchId == SessionHelper.BranchId && (
                        b.ScheduledOnDate == (SearchMessage.Date.HasValue ? DbFunctions.TruncateTime(SearchMessage.Date) : DbFunctions.TruncateTime(b.ScheduledOnDate)) || b.ScheduledOn == (SearchMessage.Date.HasValue ? DbFunctions.TruncateTime(SearchMessage.Date) : DbFunctions.TruncateTime(b.ScheduledOn))) &&
                        b.TemplateTypeId == (SearchMessage.TemplateTypeId.HasValue ? SearchMessage.TemplateTypeId : b.TemplateTypeId) &&
                        b.ModuleId == (vm.ModuleId.HasValue ? vm.ModuleId.Value : b.ModuleId)
                        select new SmsBatchVM
                        {
                            BatchNo = b.Batch,
                            BatchDate = (DateTime)(b.ScheduledOnDate ?? b.ScheduledOn),
                            MessageStatus = b.MessageStatus,
                            SMSReference = b.SmsReference,
                            //Sent = db.SmsQueues.Where(s => s.Batch == b.Batch)
                            //    .Count(s => s.MessageStatus.Contains("Sent")),
                            //Failed = db.SmsQueues.Where(s => s.Batch == b.Batch)
                            //    .Count(s => s.MessageStatus.Contains("Failed")),
                            //Delivered = db.SmsQueues.Where(s => s.Batch == b.Batch)
                            //    .Count(s => s.DeliveredStatus == true),
                            //Total = db.SmsQueues.Count(s => s.Batch == b.Batch),
                            SentBy = db.Users
                                .FirstOrDefault(u => u.UserID == (b.UserId.HasValue ? b.UserId : u.UserID))
                                .Username,
                            Template = b.Template,
                            TemplateType = b.TemplateType
                        };
            var list = blist.ToList();
            var group = list.GroupBy(b => new { b.BatchNo, b.BatchDate, b.Template, b.TemplateType, b.SentBy })
                .Select(s => new SmsBatchVM
                {
                    BatchNo = s.Key.BatchNo,
                    BatchDate = s.Key.BatchDate,
                    Sent = s.Where(f => f.BatchNo == s.Key.BatchNo).Count(f => f.MessageStatus.Contains("Sent")),
                    Failed = s.Where(f => f.BatchNo == s.Key.BatchNo).Count(f => f.MessageStatus.Contains("Failed")),
                    Delivered = s.Where(f => f.BatchNo == s.Key.BatchNo).Count(f => f.MessageStatus.Contains("Delivered")),
                    Pending = s.Where(f => f.BatchNo == s.Key.BatchNo).Count(f => f.MessageStatus.Contains("Pending")),
                    Canceled = s.Where(f => f.BatchNo == s.Key.BatchNo).Count(f => f.MessageStatus.Contains("Canceled")),
                    Total = s.Count(f => f.BatchNo == s.Key.BatchNo),
                    SentBy = s.Key.SentBy,
                    SMSReference = s.FirstOrDefault().SMSReference,
                    TemplateType = s.Key.TemplateType,
                    Template = s.Key.Template
                });
            vm.SmsBatchList = group.OrderByDescending(s => s.BatchDate).ThenByDescending(s => s.BatchNo).ToPagedList(pageNo, 25);
            return vm;
        }
        // GET: Messaging
        public async Task<ActionResult> Index(int? id, int? page, int? batch)
        {
            var dds = new Dropdowns();
            page = page ?? 1;
            pageNo = page.Value;
            ViewBag.TypesDD = await dds.GetTemplateTypesDD(id);
            ViewBag.BatchDD = await dds.GetSmsQueuesDD();
            ViewBag.StatusDD = dds.GetPendingSentFailedDD();
            if (SearchMessage == null)
            {
                SearchMessage = new SearchMessageVM();
            }
            //if (batch.HasValue)
            //{
                SearchMessage.Batch = batch;
            //}
            var vm = new MessagingVM();
            vm.ModuleId = id;
            vm.BatchNo = batch;
            GetChartData(ref vm);
            vm = Search(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult SearchMsgs(MessagingVM vm)
        {
            SearchMessage = vm.SearchMessage;
            SearchMessage.Batch = vm.BatchNo;
            vm = Search(vm);
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialMsgsQueGrid", vm),
                Messages = "Records has been loaded successfully.",
                GridId = "smsGrid",
                //LoadFile = "/scripts/eakroko.min.js"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateSMSQueue(string SMSQueueIds, string Command, long? BatchNo)
        {
            string error = string.Empty;
            string message = string.Empty;
            if (!string.IsNullOrEmpty(SMSQueueIds))
            {
                try
                {
                    if (Command == "Cancel")
                    {
                        var query = $@"Update Media.SmsQueue
                                set MessageStatus = 'Canceled'
                                where SmsQueueId in ({SMSQueueIds})";
                        db.Database.ExecuteSqlCommand(query);
                    }
                    else
                    {

                        var query = $@"Update Media.SmsQueue
                                set MessageStatus = 'Pending'
                                where SmsQueueId in ({SMSQueueIds})";
                        db.Database.ExecuteSqlCommand(query);
                    }
                    message = "Queue Updated Successfully!";
                }
                catch (Exception ex)
                {
                    error = "Failed! " + ex.Message;
                }
            }
            var vm = new MessagingVM();
            SearchMessage.Batch = BatchNo;
            vm = Search(vm);
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialMsgsQueGrid", vm),
                Messages = message,
                GridId = "smsGrid",
                Error = error,
                ModalId = "ConfirmBoxModal"
                //LoadFile = "/scripts/eakroko.min.js"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateSMSQueueByBatchIds(string SMSBatchIds)
        {
            string error = string.Empty;
            string message = string.Empty;
            if (!string.IsNullOrEmpty(SMSBatchIds))
            {
                try
                {
                    var query = $@"Update Media.SmsQueue
                                set MessageStatus = 'Pending'
                                where Batch in ({SMSBatchIds})
                                AND MessageStatus = 'Failed'";
                    db.Database.ExecuteSqlCommand(query);
                    message = "Queue Updated Successfully!";
                }
                catch (Exception ex)
                {
                    error = "Failed! " + ex.Message;
                }
            }

            var vm = new MessagingVM();
            vm = SearchBatch(vm);
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialSMSBatchGrid", vm),
                Messages = message,
                GridId = "smsGrid",
                Error = error,
                ModalId = "ConfirmBoxModal"
                //LoadFile = "/scripts/eakroko.min.js"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public MessagingVM Search(MessagingVM vm)
        {
            var list = db.SmsQueues
                .Include(a => a.Module)
                .Include(a => a.User);
            if (vm.ModuleId.HasValue && vm.ModuleId.Value > 0)
            {
                list = list.Where(s => s.ModuleId == vm.ModuleId.Value);
            }
            if (SearchMessage.Batch != null)
            {
                list = list.Where(a => a.Batch == SearchMessage.Batch);
            }
            if (SearchMessage.TemplateTypeId != null)
            {
                list = list.Where(a => a.TemplateTypeId == SearchMessage.TemplateTypeId);
            }
            if (SearchMessage.Status != null)
            {
                list = list.Where(a => a.MessageStatus == SearchMessage.Status);
            }
            if (SearchMessage.MobileNo != null)
            {
                list = list.Where(a => a.ReceiverMobile.Contains(SearchMessage.MobileNo));
            }
            vm.SmsQuesList = list.OrderByDescending(s => s.MessageId).ToPagedList(pageNo, 100);//100
            return vm;
        }

        public async Task<ActionResult> SmsDetails(int id)
        {
            var vm = new MessagingVM();
            var sms = await db.SmsQueues.Include(a => a.Module).Include(a => a.User).FirstOrDefaultAsync(s => s.SmsQueueId == id);

            vm.SmsQueue = sms;
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialSmsDetails", vm),
                TargetId = "smsDetailModalontent",
                ModalId = "smsDetailModal",
                Messages = "",
                //LoadFile = "/scripts/eakroko.min.js"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //id = template & templateTypeId = ttid
        public async Task<ActionResult> AddEdit(Guid? id, int? ttid)
        {
            var vm = new MessagingVM();
            vm.Template = new Template();
            if (id.HasValue)
            {
                vm.Template = await db.Templates.Include(c => c.TemplateType).FirstOrDefaultAsync(c => c.TemplateId == id);
                if (vm.Template != null)
                {
                    vm.Template.TemplateTypeId = Convert.ToInt16(SessionHelper.TemplateTypeId);
                    vm.TemplateType = vm.Template.TemplateType;
                }
            }
            else
            {
                vm.Template = new Template();
                vm.TemplateType = db.TemplateTypes.Find(ttid);//SessionHelper.TemplateTypeId
                vm.Template.TemplateTypeId = Convert.ToInt16(ttid);//SessionHelper.TemplateTypeId
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
        public async Task<ActionResult> AddEdit(MessagingVM vm)
        {
            string message = string.Empty;
            string error = string.Empty;

            try
            {
                if (vm.Template.TemplateId == Guid.Empty)
                {
                    vm.Template.TemplateId = Guid.NewGuid();
                    vm.Template.SMSTemplate = "p";
                    vm.Template.CreatedOn = DateTime.Now;
                    vm.Template.CreatedBy = SessionHelper.UserId;
                    vm.Template.BranchId = Convert.ToInt16(SessionHelper.BranchId);
                    vm.Template.ByDefault = false;
                    vm.Template.IP = Request.UserHostAddress;
                    db.Templates.Add(vm.Template);
                    //var addLnk = new CertificateActionLinks();
                    //addLnk.CertificateId = vm.Certificate.CertificateId;
                    //addLnk.FormId = SessionHelper.FormId;
                    //var link = db.CertificateActionLinks.Add(addLnk);
                }
                else
                {
                    var update = db.Templates.Find(vm.Template.TemplateId);
                    update.SMSTemplate = vm.Template.SMSTemplate;
                    update.SMSTemplateHeader = vm.Template.SMSTemplateHeader;
                    update.SMSTemplateFooter = vm.Template.SMSTemplateFooter;
                    update.Description = vm.Template.Description;
                    update.MailSubject = vm.Template.MailSubject;
                    update.MailTemplateHeader = vm.Template.MailTemplateHeader;
                    update.MailTemplate = vm.Template.MailTemplate;
                    update.MailTemplateFooter = vm.Template.MailTemplateFooter;
                    update.FaIcon = vm.Template.FaIcon;
                    update.ModifiedBy = SessionHelper.UserId;
                    update.ModifiedOn = DateTime.Now;
                    db.Entry(update).State = EntityState.Modified;
                    vm.Template = update;
                }
                await db.SaveChangesAsync();
                message = "Template Saved successfully...";
                var type = await db.TemplateTypes.FindAsync(vm.Template.TemplateTypeId);
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
                PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/Messaging/_MessagingTemplateFrm.cshtml", vm),
                GridId = "msgGrid",
                Url = Url.Action("AddEdit", "Messaging", new { area = "", id = vm.Template.TemplateId }),
                Reset = "false",
                Messages = message,
                Error = error
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private async Task<MessagingVM> InitMessagesViewModel(MessagingVM vm)
        {
            foreach (var filter in vm.Template.TemplateType.TemplateTypeFilters)
            {
                if (filter.FilterName == "GroupId")
                {

                    var groupsdd = await db.AcademicGroups.Select(s => new SelectListItem { Text = s.GroupClassName + "-" + s.GroupSectionName, Value = s.GroupId.ToString() }).ToListAsync();
                    TempData["GroupsDD"] = groupsdd;
                }
            }
            var tempdd = await db.Templates11.Select(s => new SelectListItem { Text = s.TemplateTitle, Value = s.ReportTemplateId.ToString() }).ToListAsync();
            ViewBag.TemplatesDD = tempdd;
            return vm;
        }

        public async Task<ActionResult> Send(Guid? id)
        {
            var vm = new MessagingVM();
            var dds = new Dropdowns();
            if (SessionHelper.ModuleId == 1 || SessionHelper.ModuleId == 2 || SessionHelper.ModuleId == 7)
            {
                ViewBag.GroupsDD = await dds.GetGroupsDD();
            }

            if (SessionHelper.ModuleId == 9)
            {
                ViewBag.DepartmentsDD = await dds.GetDepartmentsDD(); ;
            }

            if (id.HasValue)
            {
                vm.SendMessage = new SendMessageVM();

                var template = await db.Templates.Include(c => c.TemplateType).Include(c => c.TemplateType.TemplateTypeFilters).Include(c => c.TemplateType.Module).FirstOrDefaultAsync(c => c.TemplateId == id);
                vm.Template = template;
                vm.ModuleId = template.TemplateType.ModuleId;
                vm.TemplateType = template.TemplateType;
                TempData["TypeId"] = template.TemplateTypeId;
                TempData["MTempId"] = template.TemplateId;
                TempData["IsSent"] = "Send";
                vm.SendMessage.TemplateId = template.TemplateId;
                vm.SendMessage.Message = template.SMSTemplate;
                vm.SendMessage.NotificationTitle = template.SMSTemplateHeader;
            }
            //vm.ModuleId = id;
            vm.SendMessage.ScheduledOn = DateTime.Now;
            //await GetUrlSettings();
            return View(vm);
        }
        [HttpPost]
        public async Task<ActionResult> Send(MessagingVM vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            try
            {
                var template = await db.Templates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).FirstOrDefaultAsync(m => m.TemplateId == vm.SendMessage.TemplateId);
                if (!string.IsNullOrWhiteSpace(vm.SendMessage.MobNos))
                {
                    if (!template.SMSTemplate.Contains("{"))
                    {
                        if (template.TemplateType.ModuleId == 1 || template.TemplateType.ModuleId == 2 || template.TemplateType.ModuleId == 7)//module id is always static
                        {
                            template.SMSTemplate = vm.SendMessage.Message;
                            //var studentsSession = await db.StudentSessions.Include(s => s.Student).Where(s => s.GroupId == vm.SendMessage.GroupId).Select(s => s.Student.StudentId).ToListAsync();
                            // var studentList = await db.Students.Where(s => studentsSession.Contains(s.StudentId)).ToListAsync();
                            var studentList = db.Database.SqlQuery<v__Students>(@"SELECT * FROM Academics.v__Students")
                                .Where(q => q.GroupId == vm.SendMessage.GroupId).ToList();
                            var noList = vm.SendMessage.MobNos.Split(',');
                            var batch = (await db.SmsQueues.MaxAsync(q => (long?)q.Batch) ?? 0) + 1;
                            var batchDate = DateTime.Now;
                            foreach (var student in studentList)
                            {
                                //var student = studentList.FirstOrDefault(n => n.FatherMobileNumber == no);
                                if (!string.IsNullOrWhiteSpace(student.FatherMobileNumber))
                                {
                                    var smsQue = new SmsQueue();
                                    smsQue.Batch = batch;
                                    smsQue.BatchDate = batchDate;
                                    smsQue.TemplateId = template.TemplateId;
                                    smsQue.TemplateTypeId = template.TemplateTypeId;
                                    smsQue.ReceiverMobile = student.FatherMobileNumber;
                                    smsQue.MessageBody = PrepareStudentSMS(template, student);
                                    smsQue.UserId = SessionHelper.UserId;
                                    smsQue.ModuleId = template.TemplateType.ModuleId;
                                    smsQue.MessageStatus = "Pending";
                                    smsQue.DeliveredStatus = false;
                                    smsQue.ProfileId = student.StudentId.ToString();
                                    smsQue.SentOn = null;
                                    smsQue.MessageId = null;
                                    smsQue.ScheduledOn = vm.SendMessage.ScheduledOn;
                                    smsQue.ScheduledOnDate = vm.SendMessage.ScheduledOn.Value.Date;
                                    smsQue.EAID = null;
                                    smsQue.CreatedOn = DateTime.Now;
                                    smsQue.ModifiedOn = null;
                                    smsQue.BranchId = Convert.ToInt16(SessionHelper.BranchId);
                                    db.SmsQueues.Add(smsQue);
                                }
                            }
                        }

                        if (template.TemplateType.ModuleId == 9)
                        {
                            template.SMSTemplate = vm.SendMessage.Message;
                            var empList = db.Database.SqlQuery<v__Employees>(@"SELECT * FROM HR.v__Employees")
                                .Where(q => q.DepartmentId == vm.SendMessage.DepartmentId).ToList();
                            var noList = vm.SendMessage.MobNos.Split(',');
                            long batch = 1;
                            var batchList = db.SmsQueues.Select(q => q.Batch).ToList();
                            if (batchList.Any())
                            {
                                var max = batchList.Max();
                                batch = max;
                                batch++;
                            }
                            var batchDate = DateTime.Now;
                            foreach (var emp in empList)
                            {
                                //var student = studentList.FirstOrDefault(n => n.FatherMobileNumber == no);
                                if (!string.IsNullOrWhiteSpace(emp.Mobile))
                                {
                                    var smsQue = new SmsQueue();
                                    smsQue.Batch = batch;
                                    smsQue.BatchDate = batchDate;
                                    smsQue.TemplateId = template.TemplateId;
                                    smsQue.TemplateTypeId = template.TemplateTypeId;
                                    smsQue.ReceiverMobile = emp.Mobile;
                                    smsQue.MessageBody = PrepareEmployeeSMS(template, emp);
                                    smsQue.UserId = SessionHelper.UserId;
                                    smsQue.ModuleId = template.TemplateType.ModuleId;
                                    smsQue.MessageStatus = "Pending";
                                    smsQue.DeliveredStatus = false;
                                    smsQue.ProfileId = emp.EmployeeId.ToString();
                                    smsQue.SentOn = null;
                                    smsQue.MessageId = null;
                                    smsQue.ScheduledOn = vm.SendMessage.ScheduledOn;
                                    smsQue.ScheduledOnDate = vm.SendMessage.ScheduledOn.Value.Date;
                                    smsQue.EAID = null;
                                    smsQue.CreatedOn = DateTime.Now;
                                    smsQue.ModifiedOn = null;
                                    smsQue.BranchId = Convert.ToInt16(SessionHelper.BranchId);
                                    db.SmsQueues.Add(smsQue);
                                }
                            }
                        }
                        await db.SaveChangesAsync();
                        message = "SMS Saved successfully...";
                    }
                    else
                    {
                        error = "This SMS Cannot be sent from here, you can send it from its menu!";
                    }
                }
                else
                {
                    error = "Select Filter to send SMS";
                }

                if (SessionHelper.ModuleId == 1 || SessionHelper.ModuleId == 2 || SessionHelper.ModuleId == 7)
                {
                    var groupsdd = await db.AcademicGroups.Select(s => new SelectListItem { Text = s.GroupClassName + "-" + s.GroupSectionName, Value = s.GroupId.ToString() }).ToListAsync();
                    ViewBag.GroupsDD = groupsdd;
                }
                if (SessionHelper.ModuleId == 9)
                {
                    var depsdd = await db.Departments.Select(s => new SelectListItem { Text = s.DepartmentName, Value = s.DepartmentId.ToString() }).ToListAsync();
                    ViewBag.DepartmentsDD = depsdd;
                }
                vm.SendMessage = new SendMessageVM();
                vm.SendMessage.ScheduledOn = DateTime.Now;
                vm.Template = template;
                vm.TemplateType = template.TemplateType;
                TempData["TypeId"] = template.TemplateTypeId;
                TempData["MTempId"] = template.TemplateId;
                TempData["IsSent"] = "Send";
                vm.SendMessage.TemplateId = template.TemplateId;
                vm.SendMessage.Message = template.SMSTemplate;
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    error = "Something went wrong! Try again Later.";
                }
            }
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/Messaging/_SendMessage.cshtml", vm),
                GridId = "msgGrid",
                //Url = Url.Action("AddEdit", "Messaging", new { area = "", id = vm.Template.TemplateId }),
                Reset = "false",
                Messages = message,
                Error = error,
                ShowNotDismissableAlerts = true,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SendBulkSMS(MessagingVM vm)
        {
            string message = string.Empty;
            string error = string.Empty;
            long BatchNo = 0;

            var template = await db.Templates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).FirstOrDefaultAsync(m => m.TemplateId == vm.SendMessage.TemplateId);
            if (vm.Command == "SMS")
            {
                if (!string.IsNullOrWhiteSpace(vm.SendMessage.MobNos))
                {
                    if (!template.SMSTemplate.Contains("{"))
                    {

                        if (template.TemplateType.ModuleId == 1 || template.TemplateType.ModuleId == 2 || template.TemplateType.ModuleId == 7)//module id is always static
                        {
                            template.SMSTemplate = vm.SendMessage.Message;
                            //var studentsSession = await db.StudentSessions.Include(s => s.Student).Where(s => s.GroupId == vm.SendMessage.GroupId).Select(s => s.Student.StudentId).ToListAsync();
                            // var studentList = await db.Students.Where(s => studentsSession.Contains(s.StudentId)).ToListAsync();
                            //var studentList = db.Database.SqlQuery<v__Students>(@"SELECT * FROM Academics.v__Students")
                            //    .Where(q => q.GroupId == vm.SendMessage.GroupId).ToList();
                            var noList = vm.SendMessage.MobNos.Split(',');
                            var batch = (await db.SmsQueues.MaxAsync(q => (long?)q.Batch) ?? 0) + 1;
                            BatchNo = batch;
                            var batchDate = DateTime.Now;
                            foreach (var item in noList)
                            {
                                try
                                {
                                    var studentId = db.Students.Where(d => d.FatherMobileNumber == item.Trim()).Select(s => s.StudentId).FirstOrDefault();
                                    if (!string.IsNullOrWhiteSpace(item))
                                    {
                                        var smsQue = new SmsQueue();
                                        smsQue.Batch = batch;
                                        smsQue.BatchDate = batchDate;
                                        smsQue.TemplateId = template.TemplateId;
                                        smsQue.TemplateTypeId = template.TemplateTypeId;
                                        smsQue.ReceiverMobile = item.Trim();
                                        smsQue.MessageBody = PrepareStudentBulkSMS(template, studentId);
                                        smsQue.UserId = SessionHelper.UserId;
                                        smsQue.ModuleId = template.TemplateType.ModuleId;
                                        smsQue.MessageStatus = "Pending";
                                        smsQue.DeliveredStatus = false;
                                        smsQue.ProfileId = studentId.ToString();
                                        smsQue.SentOn = null;
                                        smsQue.MessageId = null;
                                        smsQue.ScheduledOn = vm.SendMessage.ScheduledOn;
                                        smsQue.ScheduledOnDate = vm.SendMessage.ScheduledOn.Value.Date;

                                        smsQue.Remarks = vm.SendMessage.Remarks;
                                        smsQue.EAID = null;
                                        smsQue.CreatedOn = DateTime.Now;
                                        smsQue.ModifiedOn = null;
                                        smsQue.BranchId = Convert.ToInt16(SessionHelper.BranchId);
                                        db.SmsQueues.Add(smsQue);
                                        await db.SaveChangesAsync();
                                    }
                                }
                                catch (Exception)
                                {
                                    continue;
                                }
                            }
                            message = "SMS Scheduled successfully...";

                        }

                        if (template.TemplateType.ModuleId == 9)
                        {
                            template.SMSTemplate = vm.SendMessage.Message;
                            var empList = db.Database.SqlQuery<v__Employees>(@"SELECT * FROM HR.v__Employees")
                                .Where(q => q.DepartmentId == vm.SendMessage.DepartmentId).ToList();
                            var noList = vm.SendMessage.MobNos.Split(',');
                            long batch = 1;
                            var batchList = db.SmsQueues.Select(q => q.Batch).ToList();
                            if (batchList.Any())
                            {
                                var max = batchList.Max();
                                batch = max;
                                batch++;
                            }
                            BatchNo = batch;
                            var batchDate = DateTime.Now;
                            foreach (var emp in empList)
                            {
                                try
                                {
                                    //var student = studentList.FirstOrDefault(n => n.FatherMobileNumber == no);
                                    if (!string.IsNullOrWhiteSpace(emp.Mobile))
                                    {
                                        var smsQue = new SmsQueue();
                                        smsQue.Batch = batch;
                                        smsQue.BatchDate = batchDate;
                                        smsQue.TemplateId = template.TemplateId;
                                        smsQue.TemplateTypeId = template.TemplateTypeId;
                                        smsQue.ReceiverMobile = emp.Mobile;
                                        smsQue.MessageBody = PrepareEmployeeSMS(template, emp);
                                        smsQue.UserId = SessionHelper.UserId;
                                        smsQue.ModuleId = template.TemplateType.ModuleId;
                                        smsQue.MessageStatus = "Pending";
                                        smsQue.DeliveredStatus = false;
                                        smsQue.ProfileId = emp.EmployeeId.ToString();
                                        smsQue.SentOn = null;
                                        smsQue.MessageId = null;
                                        smsQue.ScheduledOn = vm.SendMessage.ScheduledOn;
                                        smsQue.ScheduledOnDate = vm.SendMessage.ScheduledOn.Value.Date;
                                        smsQue.Remarks = vm.SendMessage.Remarks;
                                        smsQue.EAID = null;
                                        smsQue.CreatedOn = DateTime.Now;
                                        smsQue.ModifiedOn = null;
                                        smsQue.BranchId = Convert.ToInt16(SessionHelper.BranchId);
                                        db.SmsQueues.Add(smsQue);
                                        await db.SaveChangesAsync();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    continue;
                                }

                            }
                            message = "SMS Scheduled successfully...";

                        }

                    }
                    else
                    {
                        error = "This SMS Cannot be sent from here, you can send it from its menu!";
                    }
                }
                else
                {
                    error = "Select Filter to send SMS";
                }
            }
            else
            {
                if (vm.SendMessage.GroupId != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(vm.SendMessage.NotificationTitle))
                    {
                        if (!template.SMSTemplate.Contains("{"))
                        {
                            message = NotificationBLL.AddNotificationsByGroupId(db, vm.SendMessage.GroupId, vm.SendMessage.NotificationTitle, "Notification", SessionHelper.BranchId, vm.SendMessage.Message, null, SessionHelper.UserId, SessionHelper.UserLogId, SessionHelper.IP, null, template.TemplateType.ModuleId.ToString(), "Student", null);
                            var DeviceIds = FirebaseBLL.GetDeviceIdsBYGroup(db, vm.SendMessage.GroupId);
                            if (DeviceIds != null)
                            {
                                await FirebaseBLL.NotificationToList(DeviceIds, vm.SendMessage.NotificationTitle, vm.SendMessage.Message);
                            }
                        }
                        else
                        {
                            error = "This Notification Cannot be sent from here, Its contains '{'!";
                        }
                    }
                    else
                    {
                        error = "Notification Title is Required.";
                    }
                }
                else
                {
                    error = "Select Filter to send Notification";
                }
            }
            if (SessionHelper.ModuleId == 1 || SessionHelper.ModuleId == 2 || SessionHelper.ModuleId == 7)
            {
                var groupsdd = await db.AcademicGroups.Select(s => new SelectListItem { Text = s.GroupClassName + "-" + s.GroupSectionName, Value = s.GroupId.ToString() }).ToListAsync();
                ViewBag.GroupsDD = groupsdd;
            }
            if (SessionHelper.ModuleId == 9)
            {
                var depsdd = await db.Departments.Select(s => new SelectListItem { Text = s.DepartmentName, Value = s.DepartmentId.ToString() }).ToListAsync();
                ViewBag.DepartmentsDD = depsdd;
            }
            vm.SendMessage = new SendMessageVM();
            vm.SendMessage.ScheduledOn = DateTime.Now;
            vm.Template = template;
            vm.TemplateType = template.TemplateType;
            TempData["TypeId"] = template.TemplateTypeId;
            TempData["MTempId"] = template.TemplateId;
            TempData["IsSent"] = "Send";
            vm.SendMessage.TemplateId = template.TemplateId;
            vm.SendMessage.Message = template.SMSTemplate;


            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/Messaging/_SendMessage.cshtml", vm),
                GridId = "msgGrid",
                //Url = Url.Action("AddEdit", "Messaging", new { area = "", id = vm.Template.TemplateId }),
                Reset = "false",
                Messages = message,
                ModalId = "GeneralConfirmBoxModal",
                Error = error,
                ShowNotDismissableAlerts = true,
                BatchNo = BatchNo
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string PrepareEmployeeSMS(Template temp, v__Employees employee)
        {
            var content = temp.SMSTemplate;
            var connection =
                System.Configuration.ConfigurationManager.
                    ConnectionStrings["OneDbContext"].ConnectionString;
            using (var _conn = new SqlConnection(connection))
            {
                _conn.Open();
                string _sql = string.Format("SELECT {0} FROM {1} where EmployeeId={2}", temp.TemplateType.ViewField, temp.TemplateType.TemplateView, employee.EmployeeId);
                SqlCommand _cmd = new SqlCommand(_sql, _conn);
                SqlDataReader _dr = _cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (_dr.Read())
                {
                    var dict1 = new Dictionary<string, string>();
                    for (int i = 0; i < _dr.FieldCount; i++)
                    {
                        var fieldName = _dr.GetName(i);
                        var fieldValue = _dr[i].ToString();
                        //content.Replace( fieldName , fieldValue);
                        if (!dict1.ContainsKey(fieldName))
                        {
                            dict1.Add(fieldName, fieldValue);
                        }
                        //model.CustomReport.Fields.Add(_dr.GetName(i));
                    }
                    content = Smart.Format(content, dict1);
                    break;
                }
                _dr.Close();
            }
            return content;
        }

        public ActionResult SearchEMployeeMobNo(int id)
        {
            bool IssSuccess = false;
            var vm = new MessagingVM();
            vm.SendMessage = new SendMessageVM();
            var empListMobNos = db.Database.SqlQuery<v__Employees>(@"SELECT * FROM HR.v__Employees")
                .Where(q => q.DepartmentId == id && q.Mobile != null && q.Mobile.Length == 12 && q.Mobile.StartsWith("92")).Select(z => z.Mobile).ToList();
            if (empListMobNos.Any())
            {
                //vm.SendMessage.MobNos = string.Join(",", students);
                foreach (var no in empListMobNos)
                {
                    if (!string.IsNullOrWhiteSpace(no))
                    {
                        vm.SendMessage.MobNos += no + ",";
                    }
                }
                IssSuccess = true;
                vm.SendMessage.MobNos = vm.SendMessage.MobNos.TrimEnd(',');//.Substring(0, vm.SendMessage.MobNos.Length - 1)
            }
            var result = new
            {

                Messages = "Mobile Numbers has been loaded successfully.",
                GridId = "mobNoGrid",
                MobNo = vm.SendMessage.MobNos,
                IssSuccess = IssSuccess,
                //LoadFile = "/scripts/eakroko.min.js"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string PrepareStudentSMS(Template temp, v__Students student)
        {
            var content = temp.SMSTemplate;
            var connection =
                System.Configuration.ConfigurationManager.
                    ConnectionStrings["OneDbContext"].ConnectionString;
            using (var _conn = new SqlConnection(connection))
            {
                _conn.Open();
                string _sql = string.Format("SELECT {0} FROM {1} where StudentId={2}", temp.TemplateType.ViewField, temp.TemplateType.TemplateView, student.StudentId);
                SqlCommand _cmd = new SqlCommand(_sql, _conn);
                SqlDataReader _dr = _cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (_dr.Read())
                {
                    var dict1 = new Dictionary<string, string>();
                    for (int i = 0; i < _dr.FieldCount; i++)
                    {
                        var fieldName = _dr.GetName(i);
                        var fieldValue = _dr[i].ToString();
                        //content.Replace( fieldName , fieldValue);
                        if (!dict1.ContainsKey(fieldName))
                        {
                            dict1.Add(fieldName, fieldValue);
                        }
                        //model.CustomReport.Fields.Add(_dr.GetName(i));
                    }
                    content = Smart.Format(content, dict1);
                    break;
                }
                _dr.Close();
            }
            return content;
        }

        private string PrepareStudentBulkSMS(Template temp, int studentId)
        {
            var content = temp.SMSTemplate;
            var connection =
                System.Configuration.ConfigurationManager.
                    ConnectionStrings["OneDbContext"].ConnectionString;
            using (var _conn = new SqlConnection(connection))
            {
                _conn.Open();
                string _sql = string.Format("SELECT {0} FROM {1} where StudentId={2}", temp.TemplateType.ViewField, temp.TemplateType.TemplateView, studentId);
                SqlCommand _cmd = new SqlCommand(_sql, _conn);
                SqlDataReader _dr = _cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (_dr.Read())
                {
                    var dict1 = new Dictionary<string, string>();
                    for (int i = 0; i < _dr.FieldCount; i++)
                    {
                        var fieldName = _dr.GetName(i);
                        var fieldValue = _dr[i].ToString();
                        //content.Replace( fieldName , fieldValue);
                        if (!dict1.ContainsKey(fieldName))
                        {
                            dict1.Add(fieldName, fieldValue);
                        }
                        //model.CustomReport.Fields.Add(_dr.GetName(i));
                    }
                    content = Smart.Format(content, dict1);
                    break;
                }
                _dr.Close();
            }
            return content;
        }

        public async Task<ActionResult> SearchStudentsParentMobNo(Guid id)
        {
            bool IssSuccess = false;
            var vm = new MessagingVM();
            vm.SendMessage = new SendMessageVM();
            var students = await db.StudentSessions.Include(s => s.Student).Where(s => s.GroupId == id && s.Student.FatherMobileNumber != null && s.Student.FatherMobileNumber.Length == 12 && s.Student.FatherMobileNumber.StartsWith("92")).Select(s => s.Student.FatherMobileNumber).Distinct().ToListAsync();
            if (students.Any())
            {
                //vm.SendMessage.MobNos = string.Join(",", students);
                //foreach (var no in students)
                //{
                //    if (!string.IsNullOrWhiteSpace(no))
                //    {
                //        vm.SendMessage.MobNos += no + ",";
                //    }
                //}
                vm.SendMessage.MobNos = string.Join(", ", students);

                IssSuccess = true;
                //vm.SendMessage.MobNos = vm.SendMessage.MobNos.TrimEnd(',');//.Substring(0, vm.SendMessage.MobNos.Length - 1)
            }
            var result = new
            {

                Messages = "Mobile Numbers has been loaded successfully.",
                GridId = "mobNoGrid",
                MobNo = vm.SendMessage.MobNos,
                IssSuccess = IssSuccess,
                ////LoadFile = "/scripts/eakroko.min.js"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Templates(int id)//id: ModuleId
        {
            TempData["TemplateTypes"] = db.TemplateTypes.Where(c => c.ModuleId == id).ToList();
            SessionHelper.ModuleId = id;
            var vm = new MessagingVM();
            vm.ModuleId = id;
            ViewBag.ModuleName = db.Modules.Where(s => s.ModuleId == id).Select(s => s.ModuleName).FirstOrDefault();
            GetChartData(ref vm);
            return View(vm);
        }

        #region Notification Settings
        public ActionResult NotificationSettings(int id)//id: ModuleId
        {
            var vm = new MessagingVM();
            vm.EmailTemplateDD = db.Templates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module)
                .Where(c => c.TemplateType.ModuleId == id)
                .Select(s => new SelectListItem
                {
                    Text = s.Title,
                    Value = s.TemplateId.ToString()
                }).ToList();
            vm.SMSTemplateDD = db.Templates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module)
                .Where(c => c.TemplateType.ModuleId == id)
                .Select(s => new SelectListItem
                {
                    Text = s.Title,
                    Value = s.TemplateId.ToString()
                }).ToList();
            vm.NotificationSettingsList = db.NotificationSettings.Include(s => s.Module).Include(s => s.SMSTemplate).Include(s => s.EmailTemplate).Where(c => c.ModuleId == id).ToList();
            SessionHelper.ModuleId = id;
            vm.ModuleId = id;
            ViewBag.ModuleName = db.Modules.Where(s => s.ModuleId == id).Select(s => s.ModuleName).FirstOrDefault();
            return View(vm);
        }

        [HttpPost]
        public JsonResult AddNotificationSettings(int? id)
        {
            var vm = new MessagingVM();
            string message = string.Empty;
            string error = string.Empty;
            vm.NotificationSettingsList = new List<NotificationSetting>();
            vm.NotificationSettingsList.Add(new NotificationSetting() { SettingName = "StudentAbsentee", SMSTemplateId = null, EmailTemplateId = null, ModuleId = 1, Description = "No Description Required.", BranchId = SessionHelper.BranchId });
            vm.NotificationSettingsList.Add(new NotificationSetting() { SettingName = "FeeDefaulter", SMSTemplateId = null, EmailTemplateId = null, ModuleId = 7, Description = "No Description Required.", BranchId = SessionHelper.BranchId });
            vm.NotificationSettingsList.Add(new NotificationSetting() { SettingName = "VoucherDue", SMSTemplateId = null, EmailTemplateId = null, ModuleId = 7, Description = "No Description Required.", BranchId = SessionHelper.BranchId });
            vm.NotificationSettingsList.Add(new NotificationSetting() { SettingName = "VoucherPayment", SMSTemplateId = null, EmailTemplateId = null, ModuleId = 7, Description = "No Description Required.", BranchId = SessionHelper.BranchId });
            vm.NotificationSettingsList.Add(new NotificationSetting() { SettingName = "BulkTemplate", SMSTemplateId = null, EmailTemplateId = null, ModuleId = 1, Description = "No Description Required.", BranchId = SessionHelper.BranchId });
            vm.NotificationSettingsList.Add(new NotificationSetting() { SettingName = "LoginCode", SMSTemplateId = null, EmailTemplateId = null, ModuleId = 14, Description = "No Description Required.", BranchId = SessionHelper.BranchId });


            vm.NotificationSettingsList.Add(new NotificationSetting() { SettingName = "Reservation Sms", SMSTemplateId = null, EmailTemplateId = null, ModuleId = 19, Description = "No Description Required.", BranchId = SessionHelper.BranchId });
            vm.NotificationSettingsList.Add(new NotificationSetting() { SettingName = "Reservation Payment Sms", SMSTemplateId = null, EmailTemplateId = null, ModuleId = 19, Description = "No Description Required.", BranchId = SessionHelper.BranchId });
            foreach (var item in vm.NotificationSettingsList)
            {
                var notificationSetting = db.NotificationSettings.Where(s => s.SettingName == item.SettingName).FirstOrDefault();
                if (notificationSetting != null)
                {
                    continue;
                }
                else
                {
                    db.NotificationSettings.Add(item);
                }
            }
            try
            {
                db.SaveChanges();
                message = "Settings were updated successfully.";
                TempData["Success"] = "Settings were updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                error = "Settings updation failed.";
            }
            vm.EmailTemplateDD = db.Templates
                 .Select(s => new SelectListItem
                 {
                     Value = s.TemplateId.ToString(),
                     Text = s.Title
                 }).ToList();
            vm.SMSTemplateDD = db.Templates
               .Select(s => new SelectListItem
               {
                   Value = s.TemplateId.ToString(),
                   Text = s.Title
               }).ToList();
            vm.NotificationSettingsList = db.NotificationSettings.Include(s => s.Module).Include(s => s.SMSTemplate).Include(s => s.EmailTemplate).Where(c => c.ModuleId == id).ToList();

            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialNotificationsGrid", vm),
                Messages = message,
                GridId = "notificationGrid",
                Errors = error
                //LoadFile = "/scripts/eakroko.min.js"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddEditNotificatinAdminUsers(int id)
        {
            var vm = new MessagingVM();
            vm.NotificationSettingId = id;
            vm.UserGroupsDD = db.UserGroups.Select(u => new SelectListItem()
            {
                Text = u.UserGroupName,
                Value = u.UserGroupId.ToString()
            }).ToList();
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialNotificationAdminUsersModal", vm),
                TargetId = "addNotificationSettingAdminUserModalContent",
                ModalId = "addNotificationSettingAdminUserModal"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> AddEditNotificatinAdminUsers(MessagingVM vm)
        {
            string messages = string.Empty;
            string error = string.Empty;
            try
            {
                if (vm.NotificationSettingId != null)
                {
                    if (vm.NotificationSettingAdminUsers.Count() > 0)
                    {
                        foreach (var item in vm.NotificationSettingAdminUsers)
                        {
                            var Entity = await db.NotificationAdminUsers.Where(s => s.UserId == item.UserId && s.NotificationSettingId == vm.NotificationSettingId).FirstOrDefaultAsync();
                            Entity.Active = item.IsSelected;
                            await db.SaveChangesAsync();
                        }
                        messages = "Admin Users Updated Successfully.";
                    }
                    else
                    {
                        error = "Please Select Users!";
                    }
                }
                else
                {
                    error = "Notification Setting Not Found!";
                }
            }
            catch (Exception ex)
            {
                error = "Failed!" + ex.Message;
            }
            var result = new
            {
                Messages = messages,
                Error = error,
                ModalId = "addNotificationSettingAdminUserModal"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetAdminUsersByUserGroupId(int NotificationSettingId, int UserGroupId)
        {
            var vm = new MessagingVM();
            await AddRemainingUsersToNotificationAdminUsers(UserGroupId, NotificationSettingId);
            vm.NotificationSettingAdminUsers = await db.NotificationAdminUsers.Include(s => s.User).Where(s => s.NotificationSettingId == NotificationSettingId && s.User.UserGroupId == UserGroupId).Select(s => new NotificationSettingAdminUserVM
            {
                UserId = s.UserId,
                UserName = s.User.DisplayName ?? s.User.Username,
                IsSelected = s.Active
            }).ToListAsync();
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(ControllerContext, "_PartialNotificationSettingAdminUserGrid", vm),
                GridId = "notificationAdminUsersGrid",
                Reset = "false"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private async System.Threading.Tasks.Task AddRemainingUsersToNotificationAdminUsers(int UserGroupId, int NotificationSettingId)
        {
            await db.Database.ExecuteSqlCommandAsync($@"INSERT INTO [Media].[NotificationAdminUsers] (
	            [NotificationAdminUserId]
	            ,[UserId]
	            ,[NotificationSettingId]
	            ,[Active]
	            ,[CreatedOn]
	            ,[CreatedBy]
	            ,[CreatedByIP]
	            ,[UserLogId]
	            ,[IP]
	            )
            SELECT (
		            ISNULL((
				            SELECT Max(NotificationAdminUserId)
				            FROM Media.NotificationAdminUsers
				            ), 0) + ROW_NUMBER() OVER (
			            ORDER BY UserID ASC
			            )
		            )
	            ,UserID
	            ,{NotificationSettingId}
	            ,0
	            ,GETDATE()
	            ,{SessionHelper.UserId}
	            ,'{SessionHelper.IP}'
	            ,'{SessionHelper.UserLogId}'
	            ,'{SessionHelper.IP}'
            FROM Membership.Users
            WHERE UserGroupId = {UserGroupId}
	            AND UserID NOT IN (
		            SELECT UserId
		            FROM Media.NotificationAdminUsers Where NotificationSettingId = {NotificationSettingId}
		            )");
        }
        #endregion

        [HttpPost]
        public JsonResult AddTemplateTypes(int? id)
        {
            var vm = new MessagingVM();
            string message = string.Empty;
            string error = string.Empty;
            vm.TemplateTypeList = new List<TemplateType>();
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 1, TemplateTypeName = "General", TemplateView = "Academics.v__Students", TemplateViewKey = "StudentId", ModuleId = 1, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "StudentId,RegistrationNo,Name,RegistrationDate,SessionName,ClassName,SectionName,RollNumber,ActiveInClass,StudentSessionId,SessionId,ClassId,SectionId,MobileNumber,DateOfBirth,Address,FatherMobileNumber,FatherIdNumber,FatherEmail,Active,RequestedClassName,SchoolLeavingDate,ReasonForLeaving,MotherName,MotherMobileNumber,MotherIdNumber,MotherEmail,GuardianName,GuardianMobileNumber,GuardianIdNumber,GuardianEmail,BloodGroup,Type,BranchName,BranchId,ActiveInBranch,DateOfAssignment,ClassOrder,Password,ProfileId,GRNo,GaurdianRelationId,SessionActive,IsOrphan,GroupId,School,Branch,Disease,Instructions,MedicalProblem,ChronicalMedicalProblems,TBHistory,DiabetesHistory,EpilespsyHistory,OthersHistory,Allergies,Medication,Email,FatherName,Gender,FatherAnnualIncome,DiscountName,DiscountRate,StudentSecurityFeeAmount,StudentSecurityFeeVoucherId,EmployeeId,StaffChild,HouseTypeId,AdmissionClass,RFID,SendPresentSMSAuto,SendAbsentSMSAuto,LastSchoolAttended,ReligionName,Photo,PlaceOfBirth,PhoneNumber,PhoneNo,AdmissionDate,FamilyRef,FamilyId" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 8, TemplateTypeName = "Defaulter", TemplateView = "Fee.v__Defaulters", TemplateViewKey = "StudentId", ModuleId = 7, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "RegistrationNumber,Name,SessionName,GroupName,FatherMobile,PrevBalance,CurrBalance,SessionId,ClassId,SectionId,StudentId,Active,FatherName,NetBalance,GRNo" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 9, TemplateTypeName = "Result", TemplateView = "dbo.v__Exam__Details", TemplateViewKey = "ExamDetailId", ModuleId = 1, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "ExamId,StudentId,FullName,RollNumber,FatherName,DateOfBirth,BranchName,Active,SessionId,ClassId,SectionId,SessionName,ClassName,SectionName,ExamTermId,ExamSubjectId,SubSubjectId,MaxMarks,ObtainedMarks,TermName,SubjectName,SubSubjectName,GradeName,Gender,Photo,AttendanceStatus,StageId,GRNo,Published,DeclarationDate,HighestPercentage,LowestPercentage,ShortFormSubject,SubjectCode,SubSubjectShortForm,TermPercentage,ExamDate,Weightage,ExamType,ResultStatus,TotalPercentage,TotalObtWeighted,ExamTypeId,GroupId,ActiveInSection,AcadGroupId,ExamNo,TeacherId,ExamDetailId,FatherMobileNumber,Remarks,StudentSessionId,RegistrationNumber,SubjectPassMarks,Grade,SubjectGrade,GradeRemarks,SubjectGradeRemarks,ERNO,SubjectPassMarksPercentage,Itr,MarksPercentage,PassPercentage,STTRGrade,OverallRemarks,HouseCode,HouseOrder,EmpName,IsPass" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 10, TemplateTypeName = "Attendance", TemplateView = "Academics.v__Attendance", TemplateViewKey = "StudentId", ModuleId = 1, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "StudentId,StudentName,RegNo,Gender,GRNo,AttendanceDate,Status,Arrival,Departure,HalfDay" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 11, TemplateTypeName = "Detailed Result", TemplateView = "ER.v__Results", TemplateViewKey = "StudentId, GroupId, ExamTermId, ExamTypeId", ModuleId = 1, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "ExamId,StudentId,FullName,RollNumber,FatherName,DateOfBirth,BranchName,Active,RegistrationNumber,SessionId,ClassId,SectionId,SessionName,ClassName,SectionName,ExamTermId,ExamSubjectId,SubSubjectId,MaxMarks,ObtainedMarks,TermName,SubjectName,SubSubjectName,GradeName,Gender,AttendanceStatus,StageId,GRNo,Published,DeclarationDate,HighestPercentage,LowestPercentage,ShortFormSubject,SubjectCode,SubSubjectShortForm,TermPercentage,ExamDate,Weightage,ExamType,ResultStatus,TotalPercentage,TotalObtWeighted,ExamTypeId,GroupId,ActiveInSection,AcadGroupId,ExamNo,TeacherId,ExamDetailId,FatherMobileNumber,Remarks,StudentSessionId,TotalObtainedMarks,TotalMaxMarks,TotalFPercentage,TTGrade,Photo" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 12, TemplateTypeName = "Payment", TemplateView = "Fee.v__VoucherDetails", TemplateViewKey = "FeeVoucherId", ModuleId = 7, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "FeeVoucherId,StudentId,NAME,SessionName,SectionName,ClassName,DueDate,DefaultStatus,SessionId,ClassId,SectionId,GroupName,IssueDate,Amount,Fine,Discount,PaidAmount,VoucherStatus,NetAmount,PayableAmount,BranchId,FeeMonth,Duration,RegistrationNumber,FatherName" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 13, TemplateTypeName = "General", TemplateView = "Fee.v__VoucherDetails", TemplateViewKey = "FeeVoucherId", ModuleId = 7, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "FeeVoucherId,StudentId,NAME,SessionName,SectionName,ClassName,DueDate,DefaultStatus,SessionId,ClassId,SectionId,GroupName,IssueDate,Amount,Fine,Discount,PaidAmount,VoucherStatus,NetAmount,PayableAmount,BranchId,FeeMonth,Duration,RegistrationNumber,FatherName" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 14, TemplateTypeName = "PTM", TemplateView = "Academics.v__PTM", TemplateViewKey = "StudentId", ModuleId = 1, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "TeacherId,Title,Date,Description,EmpName,StartTime,EndTime,RoomName,ClassName,StudentId" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 15, TemplateTypeName = "Family", TemplateView = "Academics.v__Families", TemplateViewKey = "FamilyId", ModuleId = 1, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "FamilyId,FatherName,FatherLastName,FatherMobileNumber,FatherEmail,FatherIdNumber,FatherQualification,FatherProfessionId,FatherAnnualIncome,MotherName,MotherLastName,MotherMobileNumber,MotherEmail,MotherIdNumber,MotherQualification,MotherProfessionId,GuardianName,GuardianLastName,GuardianMobileNumber,GuardianEmail,GuardianIdNumber,GuardianQualification,GuardianProfessionId,GaurdianRelationId,ModifiedBy,ModifiedOn,CreatedBy,CreatedOn,BranchId,Password,MotherProfession,FatherProfession,GuardianProfessoin,GuardianRelationName" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 16, TemplateTypeName = "Employee", TemplateView = "HR.v__Employees", TemplateViewKey = "EmployeeId", ModuleId = 9, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "EmployeeId,EmpName,RegNo,FatherName,DateOfBirth,NIC,Mobile,Phone,Email,AppointmentDate,DateOfJoining,Address,Salary,Photo" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 17, TemplateTypeName = "Contacts", TemplateView = "Client.v__Clients", TemplateViewKey = "ClientId", ModuleId = 18, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "ClientId,PresentableName,Name,Gender,MaritalStatus,FatherName,HusbandName,NIC,Address,PhoneNo,MobileNo,MonthlyIncome,Email" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 18, TemplateTypeName = "Student", TemplateView = "Academics.v__AllStudents_MRC", TemplateViewKey = "StudentId", ModuleId = 1, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "StudentId,RegistrationNo,Name,RegistrationDate,SessionName,ClassName,SectionName,RollNumber,ActiveInClass,StudentSessionId,SessionId,ClassId,SectionId,MobileNumber,DateOfBirth,Address,FatherMobileNumber,FatherIdNumber,FatherEmail,Active,RequestedClassName,SchoolLeavingDate,ReasonForLeaving,MotherName,MotherMobileNumber,MotherIdNumber,MotherEmail,GuardianName,GuardianMobileNumber,GuardianIdNumber,GuardianEmail,BloodGroup,Type,BranchName,BranchId,ActiveInBranch,DateOfAssignment,ClassOrder,Password,ProfileId,GRNo,GaurdianRelationId,SessionActive,IsOrphan,GroupId,School,Branch,Disease,Instructions,MedicalProblem,ChronicalMedicalProblems,TBHistory,DiabetesHistory,EpilespsyHistory,OthersHistory,Allergies,Medication,Email,FatherName,Gender,FatherAnnualIncome,DiscountName,DiscountRate,StudentSecurityFeeAmount,StudentSecurityFeeVoucherId,EmployeeId,StaffChild,HouseTypeId,AdmissionClass,RFID,SendPresentSMSAuto,SendAbsentSMSAuto,LastSchoolAttended,ReligionName,Photo,PlaceOfBirth,PhoneNumber,PhoneNo,AdmissionDate,FamilyRef,FamilyId" });
            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 19, TemplateTypeName = "LoginCode", TemplateView = "Membership.v__UserLoginInfo", TemplateViewKey = "UserID", ModuleId = 14, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = "UserID,MobileNo,isActive,LoginCode,DisplayName,BranchId,UserGroupId,Username" });


            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 20, TemplateTypeName = "Reservation", TemplateView = "FrontDesk.V__Reservations", TemplateViewKey = "ReservationId", ModuleId = 19, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = vm.GetReservationViewFeilds() });

            vm.TemplateTypeList.Add(new TemplateType() { TemplateTypeId = 21, TemplateTypeName = "Reservation Payment", TemplateView = "FrontDesk.V__ReservationPayments", TemplateViewKey = "InvoicePaymentId", ModuleId = 19, DetailTemplateView = null, FooterTemplateView = null, DetailTemplateViewKey = null, FooterTemplateViewKey = null, ViewField = vm.GetReservationPaymentsFeilds() });
            try
            {
                foreach (var item in vm.TemplateTypeList)
                {
                    var templateType = db.TemplateTypes.Where(s => s.TemplateTypeId == item.TemplateTypeId).FirstOrDefault();
                    if (templateType != null)
                    {
                        templateType.TemplateTypeName = item.TemplateTypeName;
                        templateType.TemplateView = item.TemplateView;
                        templateType.TemplateViewKey = item.TemplateViewKey;
                        templateType.ModuleId = item.ModuleId;
                        templateType.FooterTemplateView = item.FooterTemplateView;
                        templateType.DetailTemplateViewKey = item.DetailTemplateViewKey;
                        templateType.FooterTemplateViewKey = item.FooterTemplateViewKey;
                        templateType.ViewField = item.ViewField;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.TemplateTypes.Add(item);
                        db.SaveChanges();
                    }
                }
                message = "Settings were updated successfully.";

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            var result = new
            {
                Messages = message,
                Errors = error
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> SaveInfo(int NotificationSettingId, Guid? SMSTemplateId, Guid? EmailTemplateId, bool SendMobileNotification, bool SendMessage, bool SendEmail)

        {

            string message = string.Empty;
            string status = "success";
            var NotificationSettings = await db.NotificationSettings.Where(s => s.NotificationSettingId == NotificationSettingId).FirstOrDefaultAsync();
            if (NotificationSettings != null)
            {
                NotificationSettings.SMSTemplateId = SMSTemplateId;
                NotificationSettings.EmailTemplateId = EmailTemplateId;
                NotificationSettings.SendMobileNotification = SendMobileNotification;
                NotificationSettings.SendMessage = SendMessage;
                NotificationSettings.SendEmail = SendEmail;
                try
                {
                    await db.SaveChangesAsync();
                    message = $"Updated successfully.";
                }
                catch (Exception ex)
                {

                    message = $"Failed";
                    status = "error";
                }
            }
            var result = new
            {
                Status = status,
                Message = message,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async System.Threading.Tasks.Task GetUrlSettings()
        {
            var result = await db.UrlSettings
                .Select(s => new
                {
                    s.URLStudent,
                    s.URLEmployee,
                    s.URLapi
                }).FirstOrDefaultAsync();
            SessionHelper.UrlStudent = result.URLStudent;
            SessionHelper.UrlEmployee = result.URLEmployee;
            SessionHelper.Urlapi = result.URLapi;
        }

        #region Messaging
        public ActionResult GetDashboardMessaging(int mid, int ttid)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            // SessionHelper.TemplateTypeId = ttid;
            //var links = db.CertificateActionLinks.Where(c => c.FormId == fid).Select(c=>c.CertificateId).ToList();
            var templatesList = db.Templates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).Where(c => c.TemplateTypeId == ttid).ToList();
            var typeslist = db.TemplateTypes.Where(v => v.ModuleId == mid && v.TemplateTypeId == ttid).ToList();
            //var certificates = db.Certificates.Include(c=>c.TemplateType).ToList(); 
            //var modules = db.Modules.ToList();
            ViewBag.TypesList = typeslist;
            var actionList = new List<ActionLinksViewModel>();
            //foreach (var type in typeslist)
            //{
            //   var certificatesList = certificates.Where(x => x.TemplateTypeId == type.TemplateTypeId).ToList();
            //   var module = modules.First(m=>m.ModuleId== type.ModuleId);

            foreach (var temp in templatesList)
            {
                var url = "";
                if (!string.IsNullOrWhiteSpace(temp.TemplateType.Module.ModuleName))
                {
                    url = "/" + "Messaging/Send/" + temp.TemplateType.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" + temp.TemplateType.TemplateTypeName.Trim().Replace(" ", String.Empty) + "/" + temp.TemplateId;
                }
                else
                {
                    url = "/Messaging/Send/" + temp.TemplateType.TemplateTypeName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" + temp.TemplateId;
                }
                var al = new ActionLinksViewModel { FormId = 1, FormName = temp.Title, FormUrl = url, ParentForm = temp.TemplateType.TemplateTypeId, IsMenuItem = true, IsQuickLink = true, ShowOnDesktop = false, Section = "Messaging", FontIcon = String.IsNullOrWhiteSpace(temp.FaIcon) ? "https://png.icons8.com/color/60/000000/report-card.png" : "fa " + temp.FaIcon + " fa-3x", Module = temp.TemplateType.Module.ModuleName, Area = temp.TemplateType.Module.ModuleName, Controller = "Messaging", Action = "Send", MenuPriority = 0 };
                actionList.Add(al);
            }
            var firstOrDefault = db.TemplateTypes.Find(ttid);
            if (firstOrDefault != null)
            {
                // SessionHelper.TemplateTypeId = firstOrDefault.TemplateTypeId;
                //var certificate = certificatesList.FirstOrDefault();
                var url1 = "/Messaging/AddEdit/" + firstOrDefault.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" +
                           firstOrDefault.TemplateTypeName.Trim().Replace(" ", String.Empty) + "?ttid=" + ttid;
                var al1 = new ActionLinksViewModel { FormId = 1, FormName = "Add New", FormUrl = url1, ParentForm = firstOrDefault.TemplateTypeId, IsMenuItem = true, IsQuickLink = true, ShowOnDesktop = false, Section = "Messaging", FontIcon = String.IsNullOrWhiteSpace("fa-plus-square-o") ? "https://png.icons8.com/color/60/000000/report-card.png" : "fa fa-plus" + " fa-3x", Module = firstOrDefault.Module.ModuleName, Area = firstOrDefault.Module.ModuleName, Controller = "Messaging", Action = "AddEdit", MenuPriority = 0 };
                actionList.Add(al1);
            }
            // }
            SessionHelper.ModuleId = mid;
            SessionHelper.ReturnUrl = HttpContext.Request.RawUrl;
            return PartialView("_PartialDashboardMessaging_", actionList);
        }

        public ActionResult GetMessagesLinks(int ttid, string isSent)
        {
            var templatesList = db.Templates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).Where(c => c.TemplateTypeId == ttid).ToList();
            var actionList = new List<ActionLinksViewModel>();
            var modules = db.Modules.ToList();
            string action = "Send";
            if (isSent == "Sent")
            {
                action = "Sent";
            }
            foreach (var item in templatesList)
            {
                var module = item.TemplateType.Module;
                var al = new ActionLinksViewModel { FormId = item.TemplateTypeId, FormName = item.Title, FormUrl = "/" + "Messaging/" + action + "/" + module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" + item.TemplateType.TemplateTypeName.Trim().Replace(" ", String.Empty) + "/" + item.TemplateId, ParentForm = item.TemplateTypeId, IsMenuItem = true, IsQuickLink = true, ShowOnDesktop = false, Section = item.TemplateId.ToString(), FontIcon = "https://png.icons8.com/color/60/000000/report-card.png", Module = module.ModuleName, Area = module.ModuleName, Controller = "Messaging", Action = "Send", MenuPriority = 0 };
                actionList.Add(al);
            }

            return PartialView("_PartialMessagesQuickLinks", actionList);
        }
        #endregion

        [HttpPost]
        public async Task<JsonResult> SendBulkSMSToAll(DateTime? ScheduledOn, string Remarks, Guid TemplateId, string MessageBody)
        {
            string message = string.Empty;
            string error = string.Empty;
            long BatchNo = 0;
            if (TemplateId != Guid.Empty)
            {
                var template = await db.Templates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).FirstOrDefaultAsync(m => m.TemplateId == TemplateId);
                var batch = (await db.SmsQueues.MaxAsync(q => (long?)q.Batch) ?? 0) + 1;
                BatchNo = batch;
                var batchDate = DateTime.Now;
                if (ScheduledOn == null)
                {
                    ScheduledOn = DateTime.Today;
                }
                var query = $@"Insert INTO Media.SmsQueue (ReceiverMobile,Batch,BatchDate,TemplateId,TemplateTypeId,MessageBody,UserId,ModuleId,MessageStatus,DeliveredStatus,SmsReference,ScheduledOn,ScheduledOnDate,CreatedOn,BranchId,TryCount)

                            select Distinct FatherMobileNumber, {batch},'{batchDate.ToddMMMyyyy()}','{TemplateId}','{template.TemplateTypeId}','{MessageBody}','{SessionHelper.UserId}','{template.TemplateType.ModuleId}','Pending','0','{Remarks}','{ScheduledOn.ToddMMMyyyy()}','{ScheduledOn.ToddMMMyyyy()}','{DateTime.Now.ToddMMMyyyy()}','{SessionHelper.BranchId}',0 from Academics.Students st
                            INNER JOIN Academics.StudentSessions ss ON st.StudentId = ss.StudentId
                            where ss.BranchId = {SessionHelper.BranchId} AND ss.SessionId = '{SessionHelper.CurrentSessionId}'
                            AND FatherMobileNumber is not null AND ss.Active = 1 AND ss.SessionActive = 1 AND st.Active = 1
                            AND LEN(FatherMobileNumber) = 12 AND LEFT(FatherMobileNumber,2) = '92' 
                            AND FatherMobileNumber NOT LIKE '%[^0-9]%' AND FatherMobileNumber Not in (select ReceiverMobile From Media.SmsQueue Where TemplateId = '{TemplateId}' AND ScheduledOnDate = '{ScheduledOn.ToddMMMyyyy()}')";
                try
                {
                    await db.Database.ExecuteSqlCommandAsync(query);
                    message = "SMS Scheduled Successfully.";
                }
                catch (Exception ex)
                {
                    error = "Failed! " + ex.Message;
                }
            }
            else
            {
                error = "Template not Selected.";
            }
            var result = new
            {
                Messages = message,
                Error = error,
                ShowNotDismissableAlerts = true,
                BatchNo = BatchNo,
                Reset = "false",
                ModalId = "ConfirmBoxModal"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> SendBulkNotificationToAll(string NotificationTitle, Guid TemplateId, string MessageBody)
        {
            string message = string.Empty;
            string error = string.Empty;
            long BatchNo = 0;
            if (TemplateId != Guid.Empty)
            {
                message = NotificationBLL.AddNotificationWithStudentReference(db, SessionHelper.CurrentSessionId, NotificationTitle, "Notification", SessionHelper.BranchId, MessageBody, null, SessionHelper.UserId, SessionHelper.UserLogId, SessionHelper.IP, null, SessionHelper.ModuleId.ToString(), "Student", null);
                var DeviceIds = FirebaseBLL.GetDeviceIdsBySession(db, SessionHelper.CurrentSessionId, SessionHelper.BranchId);
                if (DeviceIds != null)
                {
                    await FirebaseBLL.NotificationToList(DeviceIds, NotificationTitle, MessageBody);
                }
            }
            else
            {
                error = "Template not Selected.";
            }
            var result = new
            {
                Messages = message,
                Error = error,
                ShowNotDismissableAlerts = true,
                BatchNo = BatchNo,
                Reset = "false",
                ModalId = "ConfirmBoxModalNotification"
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudentStrengthForMessaging(Guid TemplateId)
        {
            int totalCount;
            try
            {
                var query = $@" select Count(Distinct FatherMobileNumber)
                                from Academics.Students st
                            INNER JOIN Academics.StudentSessions ss ON st.StudentId = ss.StudentId
                            where ss.BranchId = {SessionHelper.BranchId} AND ss.SessionId = '{SessionHelper.CurrentSessionId}'
                            AND FatherMobileNumber is not null AND ss.Active = 1 AND ss.SessionActive = 1 AND st.Active = 1
                            AND LEN(FatherMobileNumber) = 12 AND LEFT(FatherMobileNumber,2) = '92' 
                            AND FatherMobileNumber NOT LIKE '%[^0-9]%' AND FatherMobileNumber Not in (select ReceiverMobile From Media.SmsQueue Where TemplateId = '{TemplateId}' AND ScheduledOnDate = '{DateTime.Today.ToddMMMyyyy()}')";
                totalCount = db.Database.SqlQuery<int>(query).FirstOrDefault();
            }
            catch (Exception ex)
            {
                totalCount = 0;
            }
            return Json(totalCount, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudentStrengthForNotification()
        {
            int totalCount;
            try
            {
                var query = $@" SELECT 
		Count(*)
FROM Academics.Students t1
INNER JOIN Academics.StudentSessions t3 ON t1.StudentId = t3.StudentId
	AND t3.Active = 1
	AND t3.SessionActive = 1
INNER JOIN Academics.Families t2 ON t1.FamilyId = t2.FamilyId
WHERE t3.SessionId = '{SessionHelper.CurrentSessionId}'
	AND t3.BranchId = {SessionHelper.BranchId}
	AND t1.Active = 1 AND t2.FamilyMobile is not null AND LEN(t2.FamilyMobile) = 12";
                totalCount = db.Database.SqlQuery<int>(query).FirstOrDefault();
            }
            catch (Exception ex)
            {
                totalCount = 0;
            }
            return Json(totalCount, JsonRequestBehavior.AllowGet);
        }
    }
}