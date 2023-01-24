using FAPP.Areas.AM.Helper;
using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Helpers;
using FAPP.Model;
using FAPP.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FAPP.Controllers
{

    public class CustomReportingController : BaseController
    {
        // GET: Academics/CustomReporting
        public ActionResult Index()
        {
            return View("~/Views/CustomReporting/Index.cshtml");
        }

        public async Task<ActionResult> Templates()
        {
            var vm = new TemplatesViewModel();
            vm.TemplateList = await db.Templates11.ToListAsync();
            return View("~/Views/CustomReporting/Templates.cshtml", vm);
        }

        public async Task<ActionResult> Setting()
        {
            var vm = new TemplatesViewModel();
            vm.Branch = await db.Branches.FindAsync(SessionHelper.BranchId);
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Setting(TemplatesViewModel vm)
        {
            string message = string.Empty;
            try
            {
                var branch = await db.Branches.FindAsync(SessionHelper.BranchId);
                branch.ReportHeader = vm.Branch.ReportHeader;
                branch.ReportFooter = vm.Branch.ReportFooter;
                db.Entry(branch).State = EntityState.Modified;
                await db.SaveChangesAsync();
                message = "Template Settings Saved successfully...";

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    message = ex.Message;
                }
            }
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/CustomReporting/_PartialTemplateSetingFrm.cshtml", vm),
                GridId = "tempGrid",
                Reset = "false",
                Messages = message
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Add(int? id)
        {
            var vm = new TemplatesViewModel();
            if (id.HasValue)
            {
                vm.Template = await db.Templates11.Include(t => t.View).Include(t => t.View.TemplateFilters).FirstOrDefaultAsync(t => t.ReportTemplateId == id);
                vm = InitTemplateVM(vm);
            }
            else
            {
                vm.Template = new Templates1();
                vm.ViewFieldsDD = new List<SelectListItem>();
                vm.SelectedFields = new List<string>();
                vm.View = db.Views.Find(SessionHelper.ViewId);
                vm.Template.ReportViewId = SessionHelper.ViewId;

                //vm.ViewsDD = new List<SelectListItem>();
                //vm.ViewsDD = (from v in db.Views
                //              select new SelectListItem
                //              {
                //                  Text = v.ViewName,
                //                  Value = v.ViewId.ToString()
                //              }).ToList();
            }
            return View("~/Views/CustomReporting/Add.cshtml", vm);
        }

        public async Task<ActionResult> AddUpdateTemplate(int? id)
        {
            var vm = new TemplatesViewModel();
            if (id.HasValue)
            {
                vm.Template = await db.Templates11.Include(t => t.View).Include(t => t.View.TemplateFilters).FirstOrDefaultAsync(t => t.ReportTemplateId == id);
                vm = InitTemplateVM(vm);
            }
            else
            {
                vm.Template = new Templates1();
                vm.ViewsDD = new List<SelectListItem>();
                vm.ViewsDD = (from v in db.Views
                              select new SelectListItem
                              {
                                  Text = v.ViewName,
                                  Value = v.ViewId.ToString()
                              }).ToList();
            }


            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/CustomReporting/_PartialTemplateFrm.cshtml", vm),
                GridId = "tempGrid",
                Reset = "false",
                Messages = ""
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> AddUpdateTemplate(TemplatesViewModel vm)
        {
            string message = string.Empty;
            var addedtemp = vm.Template;
            try
            {
                vm.Template.BranchId = Convert.ToInt16(SessionHelper.BranchId);
                if (vm.SelectedFields != null)
                {
                    vm.Template.SelectedFields = String.Join(",", vm.SelectedFields.ToArray());
                }
                if (vm.Template.ReportTemplateId == 0)
                {
                    // vm.Template.FaIcon = "fa " + vm.Template.FaIcon;
                    db.Templates11.Add(vm.Template);
                }
                else
                {
                    db.Entry(vm.Template).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();
                vm.Template = await db.Templates11.Include(t => t.View).FirstOrDefaultAsync(t => t.ReportTemplateId == vm.Template.ReportTemplateId);
                message = "Report Template Saved successfully...";
                vm = InitTemplateVM(vm);
                //var addLnk = new CustomReportActionLinks();
                //addLnk.ReportTemplateId = vm.Template.ReportTemplateId;
                //addLnk.FormId = SessionHelper.FormId;
                //var link = db.CustomReportActionLinks.Add(addLnk);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                {
                    message = ex.Message;
                }
            }
            var result = new
            {
                PartialView = CustomJsonHelper.RenderPartialViewToString(this.ControllerContext, "~/Views/CustomReporting/_PartialTemplateFrm.cshtml", vm),
                GridId = "tempGrid",
                Reset = "false",
                Messages = message
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Initialize TemplateViewModel
        /// </summary>
        /// <param name="vm">Pre Initialized TemplateViewModel</param>
        /// <returns></returns>
        private TemplatesViewModel InitTemplateVM(TemplatesViewModel vm)
        {
            vm.ViewFieldsDD = new List<SelectListItem>();
            //vm.ViewsDD = new List<SelectListItem>();
            //vm.ViewsDD = (from v in db.Views
            //              select new SelectListItem
            //              {
            //                  Text = v.ViewName,
            //                  Value = v.ViewId.ToString()
            //              }).ToList();
            vm.Fields = new ArrayList();
            vm.SelectedFields = new List<string>();
            var reportView = db.Views.Find(vm.Template.ReportViewId);
            if (!string.IsNullOrWhiteSpace(reportView.ViewFields))
            {
                var vfields = reportView.ViewFields.Split(',');
                for (int i = 0; i < vfields.Count(); i++)
                {
                    if (!string.IsNullOrWhiteSpace(vfields[i]))
                    {
                        vm.Fields.Add(vfields[i]);
                        bool isSelected = false;
                        if (vm.Template.SelectedFields != null)
                        {
                            isSelected = vm.Template.SelectedFields.Contains(vfields[i]) ? true : false;
                        }
                        vm.ViewFieldsDD.Add(new SelectListItem
                        {
                            Selected = isSelected,
                            Text = vfields[i],
                            Value = vfields[i]
                        });
                        if (vm.Template.SelectedFields != null)
                        {
                            if (vm.Template.SelectedFields.Contains(vfields[i]))
                            {
                                vm.SelectedFields.Add(vfields[i]);
                            }
                        }
                    }
                }
            }

            return vm;
        }
        private async Task<TemplatesViewModel> InitTemplatesViewModel(TemplatesViewModel vm)
        {
            var dds = new Dropdowns();
            //foreach (var filter in vm.Template.View.TemplateFilters)
            //{
            //    if (filter.FilterName == "GroupId")
            //    { 
            //        TempData["GroupsDD"] = await dds.GetGroupsDD();
            //    }
            //}
            //vm.GroupsDD = await dds.GetGroupsDD();
            //vm.StagesDD = await dds.GetStagesDD();
            //vm.ClassesDD = await dds.GetClassesDD(SessionHelper.BranchId);
            ViewBag.TemplatesDD = await dds.GetReportingTemplatesDD();
            return vm;
        }
        public async Task<ActionResult> ReportBuilder(int? id)
        {
            var vm = new TemplatesViewModel();
            if (id.HasValue)
            {
                vm.Template = await db.Templates11.Include(t => t.View).Include(t => t.View.TemplateFilters).Include(t => t.View.Module).FirstOrDefaultAsync(f => f.ReportTemplateId == id);
                vm = await InitTemplatesViewModel(vm);
                TempData["ViewId"] = vm.Template.ReportViewId;
                TempData["TempId"] = vm.Template.ReportTemplateId;
                vm.ReportBuilderViewModel = new ReportBuilderViewModel();
                vm.ReportBuilderViewModel.ReportTemplateId = Convert.ToInt32(id);
            }
            return View("~/Views/CustomReporting/ReportBuilder.cshtml", vm);
        }
        [HttpPost]
        public async Task<ActionResult> ReportBuilder(TemplatesViewModel model)
        {
            var template = await db.Templates11.Include(t => t.View).Include(t => t.View.TemplateFilters).FirstOrDefaultAsync(t => t.ReportTemplateId == model.ReportBuilderViewModel.ReportTemplateId);
            model.Template = template;
            TempData["ViewId"] = template.ReportViewId;
            TempData["TempId"] = template.ReportTemplateId;
            model = await InitTemplatesViewModel(model);
            model.CustomReport = new CustomReportVM();
            model.Template = template;
            model.Branch = await db.Branches.FindAsync(SessionHelper.BranchId);
            var connection =
                        System.Configuration.ConfigurationManager.
                        ConnectionStrings["OneDbContext"].ConnectionString;
            using (var _conn = new SqlConnection(connection))
            {
                _conn.Open();
                string _sql = string.Format("SELECT {0} FROM {1} ", template.SelectedFields, template.View.ReportView, model.ReportBuilderViewModel.GroupId);
                _sql = _sql + $@" where BranchId = {SessionHelper.BranchId} AND SessionId = '{SessionHelper.CurrentSessionId}' AND Active = 1 ";

                if (template.View.TemplateFilters != null && template.View.TemplateFilters.Any())
                {
                    int c = 0;
                    foreach (var filter in template.View.TemplateFilters)
                    {
                        if (Request.Form[filter.FilterName] != null)
                        {
                            if (!string.IsNullOrWhiteSpace(Request.Form[filter.FilterName].ToString()))
                            {
                                TempData[filter.FilterName] = Request.Form[filter.FilterName].ToString();
                                if (c == 0)
                                {
                                    if (filter.FilterType == "Dropdown")
                                    {
                                        _sql = _sql + " and " + filter.FilterName + "='" + Request.Form[filter.FilterName].ToString() + "' ";
                                    }
                                    if (filter.FilterType == "Textbox")
                                    {
                                        _sql = _sql + " and " + filter.FilterName + " like '%" + Request.Form[filter.FilterName].ToString().Trim() + "%' ";
                                    }
                                }
                                else
                                {

                                    if (filter.FilterType == "Dropdown")
                                    {
                                        _sql = _sql + " and " + filter.FilterName + "='" + Request.Form[filter.FilterName].ToString() + "' ";
                                    }
                                    if (filter.FilterType == "Textbox")
                                    {
                                        _sql = _sql + " and " + filter.FilterName + " like '%" + Request.Form[filter.FilterName].ToString().Trim() + "%' ";
                                    }
                                }
                                c++;
                            }
                        }
                    }
                }
                if (model.ReportBuilderViewModel.StageId != null)
                {
                    if (model.ReportBuilderViewModel.StageId != Guid.Empty)
                    {
                        _sql = _sql + $@" AND StageId = '{model.ReportBuilderViewModel.StageId}'";
                    }
                }
                if (model.ReportBuilderViewModel.GroupId != null)
                {
                    if (model.ReportBuilderViewModel.GroupId != Guid.Empty)
                    {
                        _sql = _sql + $@" AND GroupId = '{model.ReportBuilderViewModel.GroupId}'";
                    }
                }
                if (model.ReportBuilderViewModel.ClassId != null)
                {
                    if (model.ReportBuilderViewModel.ClassId != Guid.Empty)
                    {
                        _sql = _sql + $@" AND ClassId = '{model.ReportBuilderViewModel.ClassId}'";
                    }
                }
                SqlCommand _cmd = new SqlCommand(_sql, _conn);
                SqlDataReader _dr = _cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                model.CustomReport.Fields = new ArrayList(_dr.FieldCount);
                model.CustomReport.Values = new List<List<string>>();
                for (int i = 0; i < _dr.FieldCount; i++)
                {
                    model.CustomReport.Fields.Add(_dr.GetName(i));
                }
                while (_dr.Read())
                {
                    var list = new List<string>();
                    for (int i = 0; i < _dr.FieldCount; i++)
                    {
                        var value = _dr[i].ToString();
                        list.Add(value);
                    }
                    model.CustomReport.Values.Add(list);
                }
                _dr.Close();
            }
            return View("~/Views/CustomReporting/ReportBuilder.cshtml", model);

        }

        //public async Task<JsonResult> GetGroupsByStage(Guid? stageId)
        //{
        //    if (stageId != null)
        //    {
        //        var groups = await (from g in db.AcademicGroups
        //                             .Include(s => s.Class)
        //                             .Include(s => s.Section)
        //                            where (from c in db.Classes
        //                                   where c.StageId == stageId && c.BranchId == SessionHelper.BranchId
        //                                   select c.ClassId).Contains(g.ClassId)
        //                                   && g.BranchId == SessionHelper.BranchId && g.SessionId == SessionHelper.CurrentSessionId
        //                            orderby g.Class.ClassOrder, g.Section.SectionName
        //                            select new SelectListItem
        //                            {
        //                                Text = g.GroupName,
        //                                Value = g.GroupId.ToString()
        //                            }).ToListAsync();

        //        return Json(groups, JsonRequestBehavior.AllowGet);
        //    }
        //    else {
        //        var groups = await new Dropdowns().GetGroupsDD();

        //        return Json(groups, JsonRequestBehavior.AllowGet);
        //    }
        //}
       
        #region Reports

        public ActionResult GetDashboardReports(int mid, int vid)
        {
            var reportViews = db.Views.Include(v => v.Module).Where(v => v.ModuleId == mid && v.ViewId == vid).ToList();
            ViewBag.ReportViews = reportViews;
            SessionHelper.ViewId = vid;
            //var links = db.CustomReportActionLinks.Where(c => c.FormID == fid).Select(c => c.ReportTemplateId).ToList();
            var templatesList = db.Templates11.Include(c => c.View).Include(c => c.View.Module).Where(c => c.ReportViewId == vid).ToList();

            var actionList = new List<Form>();
            //foreach (var view in templatesList)
            //{
            // var temp = templates.Where(x => x.ReportViewId == view.ViewId).ToList();
            foreach (var item in templatesList)
            {
                var url = "";
                if (!string.IsNullOrWhiteSpace(item.View.Module.ModuleName))
                {
                    url = "/" + "CustomReporting/Builder/" + item.View.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" + item.View.ViewName.Trim().Replace(" ", String.Empty) + "/" + item.ReportTemplateId;
                }
                else
                {
                    url = "/CustomReporting/Builder/" + "/" + item.View.ViewName.Trim().Replace(" ", String.Empty) + "/" + item.ReportTemplateId;
                }
                var al = new Form { FormID = 1, FormName = item.TemplateTitle, FormURL = url, ParentForm = item.View.ViewId, IsMenuItem = true, IsQuickLink = true, ShowOnDesktop = false, Section = "Home", Icon = String.IsNullOrWhiteSpace(item.FaIcon) ? "https://png.icons8.com/color/60/000000/report-card.png" : "fa " + item.FaIcon + " fa-3x", Module = item.View.Module.ModuleName, Area = item.View.Module.ModuleName, Controller = "CustomReporting", Action = "Index", MenuItemPriority = 0 };
                actionList.Add(al);
            }
            var tempplate = db.Views.Find(vid);

            SessionHelper.ViewId = vid;
            if (tempplate != null)
            {
                var url1 = "/CustomReporting/AddEdit/" + tempplate.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" + tempplate.ViewName.Trim().Replace(" ", String.Empty);
                var al2 = new Form { FormID = 1, FormName = "Add New", FormURL = url1, ParentForm = tempplate.ViewId, IsMenuItem = true, IsQuickLink = true, ShowOnDesktop = false, Section = "Home", Icon = String.IsNullOrWhiteSpace("fa fa-plus") ? "https://png.icons8.com/color/60/000000/report-card.png" : "fa fa-plus" + " fa-3x", Module = tempplate.Module.ModuleName, Area = tempplate.Module.ModuleName, Controller = "CustomReporting", Action = "Index", MenuItemPriority = 0 };
                actionList.Add(al2);
            }


            //}
            return PartialView("_PartialDashboardReports", actionList);
        }

        public ActionResult GetReportsLinks(int vid)
        {
            var view = db.Views.Find(vid);
            var templates = db.Templates11.Where(t => t.ReportViewId == vid).ToList();
            var actionList = new List<Form>();

            foreach (var item in templates)
            {
                if (view != null)
                {
                    var al = new Form { FormID = item.ReportTemplateId, FormName = item.TemplateTitle, FormURL = "/" + "CustomReporting/Builder/" + view.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" + item.View.ViewName.Trim().Replace(" ", String.Empty) + "/" + item.ReportTemplateId, ParentForm = view.ViewId, IsMenuItem = true, IsQuickLink = true, ShowOnDesktop = false, Section = "Home", Icon = "https://png.icons8.com/color/60/000000/report-card.png", Module = view.Module.ModuleName, Area = view.Module.ModuleName, Controller = "CustomReporting", Action = "Index", MenuItemPriority = 0 };
                    actionList.Add(al);
                }
            }

            return PartialView("_PartialReportsQuickLinks", actionList);
        }
        #endregion
    }
}