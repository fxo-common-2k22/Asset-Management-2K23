using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
namespace FAPP.Controllers
{
    public class PrimaryController : Controller
    {
        private readonly OneDbContext db;

        public PrimaryController()
        {
            db = new OneDbContext();
        }

        public ActionResult Branches()
        {
            var list = ProceduresModel.v_mnl_UserBranches(db)
                .Where(u => u.Active == true && u.UserId == FAPP.DAL.SessionHelper.UserID)
                .ToList();

            return PartialView("_Braches", list);
        }


        public ActionResult FiscalYears(string URL)
        {
            var list = db.FiscalYears
                .Where(u => u.BranchId == SessionHelper.BranchId)
                .OrderByDescending(u => u.FiscalYearId)
                .ToList();

            return PartialView("_FiscalYear", list);
        }

        [ChildActionOnly]
        public ActionResult GetQuicklinks(string area2, string controller, string action)
        {
            string url = "";
            if (!string.IsNullOrEmpty(area2))
            {
                url += "/" + area2;
            }

            if (!string.IsNullOrEmpty(controller))
            {
                url += "/" + controller;
            }

            if (!string.IsNullOrEmpty(action))
            {
                url += "/" + action;
            }

            var isDashboard = db.Forms.Where(s => s.FormURL == url && (s.FormName.Contains("Dashboard") || s.MenuText.Contains("Dashboard"))).Any();
            if (url.Contains("/Index") && isDashboard)
            {
                url = url.Remove(url.LastIndexOf('/'));
            }

            //var d = RouteData.Route["Defaults"];
            //var dd = (Route)RouteTable.Routes["Default"];
            var route = RouteTable.Routes.OfType<Route>().Where(s => s.Defaults != null).FirstOrDefault();
            if (route != null)
            {
                var defaultController = route.Defaults["controller"];
                var defaultAction = route.Defaults["action"];
            }
            //url = url + (!string.IsNullOrEmpty((string)this.RouteData.Values["id"]) ? "/" + (string)this.RouteData.Values["id"] : "");

            return PartialView("_PartialQuicklinks", QuickLinks(url));
        }

        public string QuickLinks(string URL)
        {
            List<p_mnl_QuickLinks_Result> res;
            var ParentFormId = db.Forms.Where(s => s.FormURL == URL).Select(s => s.ParentForm).FirstOrDefault();
            if (ParentFormId == 2)
            {
                res = ProceduresModel.p_mnl_QuickLinks_Main_Dashboard(db, SessionHelper.UserGroupId, URL).ToList();
            }
            else
            {
                res = ProceduresModel.p_mnl_QuickLinks(db, SessionHelper.UserGroupId, URL).ToList();
            }
            string form = "";
            string active = "class='active'";
            if (res != null)
            {
                form += "<div class='subnav'><div class='subnav-title'><a href='#' class='toggle-subnav'><i class='fa fa-angle-down'></i><span>Quick Links</span></a></div><ul class='subnav-menu'>";

                foreach (var item in res.Where(u => !u.IsDashboardPart))
                {
                    item.FaIcon = "glyphicon-circle-arrow-right";
                    if (URL == item.FormUrl)
                    {
                        active = "class='active'";
                    }
                    else
                    {
                        active = "";
                    }

                    form += "<li " + active + "><a class='wrap-text' title='" + item.MenuText + "' href='" + item.FormUrl + "'" + "><span><i class=\"glyphicon " + item.FaIcon + "\"></i></span>&nbsp;" + item.MenuText + "</a></li>";
                }
                form += "</ul></div>";
            }
            return form;
        }

        [ChildActionOnly]
        public ActionResult GetDashboardMenus(string area2, string controller, string action)
        {
            string url = "";
            if (!string.IsNullOrEmpty(area2))
            {
                url += "/" + area2;
            }

            if (!string.IsNullOrEmpty(controller))
            {
                url += "/" + controller;
            }

            if (!string.IsNullOrEmpty(action))
            {
                url += "/" + action;
            }

            return PartialView("_PartialDashboardMenu", DashboardMenu(url));
        }

        public string DashboardMenu(string URL)
        {
            var res = ProceduresModel.p_mnl_DashboardMenus(db, SessionHelper.UserGroupId, URL).ToList();
            string form = "", icon = "";
            if (res != null)
            {
                foreach (var item in res.Where(u => !u.IsDashboardPart))
                {
                    if (!string.IsNullOrEmpty(item.Icon))
                    {
                        icon = item.Icon.Contains("<img") ? item.Icon : $"<img src = '{item.Icon}' width='48' height='48'/>";
                    }
                    if (item.MenuText != "Dashboard")
                    {
                        form += "<li><a id=\"HyperLink1\"title=\"" + item.MenuText + "\" href=" + item.FormUrl + "><span>" + icon + "</span><em>" + item.MenuText + "</em></a></li>";
                    }
                }
            }
            return form;
        }

        [ChildActionOnly]
        public ActionResult GetDashboardQuickLinks(string area2, string controller, string action)
        {
            string url = "";
            if (!string.IsNullOrEmpty(area2))
            {
                url += "/" + area2;
            }

            if (!string.IsNullOrEmpty(controller))
            {
                url += "/" + controller;
            }

            if (!string.IsNullOrEmpty(action))
            {
                url += "/" + action;
            }

            return PartialView("_PartialDashboardMenu", DashboardQuickLinks(url));
        }

        public string DashboardQuickLinks(string URL)
        {
            var res = ProceduresModel.p_mnl_DashboardMenus(db, SessionHelper.UserGroupId, URL).ToList();
            string form = "";
            if (res != null)
            {
                foreach (var item in res.Where(u => !u.IsDashboardPart))
                {
                    //item.Icon = "<i class=\"fa fa-check\"></i>";
                    if (item.MenuText != "Dashboard")
                    {
                        form += "<a class=\"bold\" id=\"HyperLink1\"title=\"" + item.MenuText + "\" href=" + item.FormUrl + "><span><i class=\"glyphicon " + item.FaIcon + "\"></i>&nbsp;&nbsp;" + item.MenuText + "</a><br/>";
                    }
                }
            }
            return form;
        }

        [ChildActionOnly]
        public ActionResult GetHomeDashboardMenus(string url)
        {
            return PartialView("_PartialDashboardMenu", HomeDashboardMenu(url));
        }

        public string HomeDashboardMenu(string URL)
        {
            var res = ProceduresModel.p_mnl_DashboardMenus(db, SessionHelper.UserGroupId, URL).ToList();
            string form = "";
            if (res != null)
            {
                if (res.Count() > 0)
                {
                    foreach (var item in res.Where(u => !u.IsDashboardPart))
                    {
                        if (item.Icon != null)
                        {
                            item.Icon = item.Icon.Replace("width=\"48\"", "width=\"26\"");
                            item.Icon = item.Icon.Replace("height=\"48\"", "height=\"26\"");
                            item.Icon = item.Icon.Replace("width=\"40\"", "width=\"26\"");
                            item.Icon = item.Icon.Replace("height=\"40\"", "height=\"26\"");
                        }
                        //item.Icon = "<i class=\"fa fa-check\"></i>";
                        if (item.MenuText != "Dashboard")
                        {
                            form += "<a class=\"bold\" id=\"HyperLink1\"title=\"" + item.MenuText + "\" href=" + item.FormUrl + "><i class=\"glyphicon " + item.FaIcon + "\"></i>&nbsp;&nbsp;" + item.MenuText + "</a><br/>";
                        }
                    }
                    form = form.Remove(form.Length - 10, 0);
                }
            }
            return form;
        }

        //public string QuickLinkForFamily(StudentModel ex)
        //{
        //    string form = "";

        //    //foreach (var item in ex.Students)
        //    //{
        //    //    href = "/Academics/Student/SearchStudent/" + item.ID;
        //    //    if (href.ToUpper() == Request.RawUrl.ToUpper() || ("/Academics/Student/StudentBilling/" + item.ID).ToUpper() == Request.RawUrl.ToUpper() || ("/Academics/Student/SearchStudent/" + item.ID + "?ActiveTab=Family").ToUpper() == Request.RawUrl.ToUpper() || ("/Academics/Student/SearchStudent/" + item.ID + "?ActiveTab=Timetable").ToUpper() == Request.RawUrl.ToUpper())
        //    //    {
        //    //        liTrue = true;
        //    //        subLi = true;
        //    //    }
        //    //    else
        //    //        subLi = false;
        //    //    if (subLi)
        //    //        li += "<li style=\"background-color: lightgray;\"><a href=" + href + ">" + item.Name + "-" + item.RegistraionNo + "</a></li>";
        //    //    else
        //    //        li += "<li><a href=" + href + ">" + item.Name + "-" + item.ID + "</a></li>";
        //    //}
        //    //href = "/Academics/Student/EditStudents?Href=" + ex.Href;
        //    //if (liTrue)
        //    //{
        //    //    form += "<div class='subnav'><div class='subnav-title'><a href='#' class='toggle-subnav'><i class='fa fa-angle-down'></i><span>" + ex.Href + "</span></a></div><ul class='subnav-menu'><li><a href=" + href + ">Family</a></li><li class='dropdown active'><a href='#' data-toggle='dropdown'>Students</a><ul class='dropdown-menu'>" + li + "</ul></li><li><a href=/Academics/Student/FamilyBilling?Href=" + ex.Href + ">Family Billing</a></li></ul></div>";
        //    //}
        //    //else if (("/Academics/Student/EditStudents?Href=" + ex.Href).ToUpper() == Request.RawUrl.ToUpper() || ("/Academics/Student/EditStudents/?Href=" + ex.Href).ToUpper() == Request.RawUrl.ToUpper())
        //    //{
        //    //    form += "<div class='subnav'><div class='subnav-title'><a href='#' class='toggle-subnav'><i class='fa fa-angle-down'></i><span>" + ex.Href + "</span></a></div><ul class='subnav-menu'><li " + active + "><a href=" + href + ">Family</a></li><li class='dropdown'><a href='#' data-toggle='dropdown'>Students</a><ul class='dropdown-menu'>" + li + "</ul></li><li><a href=/Academics/Student/FamilyBilling?Href=" + ex.Href + ">Family Billing</a></li></ul></div>";
        //    //}
        //    //else if (("/Academics/Student/FamilyBilling?Href=" + ex.Href).ToUpper() == Request.RawUrl.ToUpper())
        //    //{
        //    //    form += "<div class='subnav'><div class='subnav-title'><a href='#' class='toggle-subnav'><i class='fa fa-angle-down'></i><span>" + ex.Href + "</span></a></div><ul class='subnav-menu'><li ><a href=" + href + ">Family</a></li><li class='dropdown'><a href='#' data-toggle='dropdown'>Students</a><ul class='dropdown-menu'>" + li + "</ul></li><li " + active + "><a href=/Academics/Student/FamilyBilling?Href=" + ex.Href + ">Family Billing</a></li></ul></div>";
        //    //}
        //    //else
        //    //{
        //    //    form += "<div class='subnav'><div class='subnav-title'><a href='#' class='toggle-subnav'><i class='fa fa-angle-down'></i><span>" + ex.Href + "</span></a></div><ul class='subnav-menu'><li><a href=" + href + ">Family</a></li><li class='dropdown'><a href='#' data-toggle='dropdown'>Students</a><ul class='dropdown-menu'>" + li + "</ul></li><li><a href=/Academics/Student/FamilyBilling?Href=" + ex.Href + "> Family Billing</a></li></ul></div>";
        //    //}
        //    return form;
        //}

        [ChildActionOnly]
        public ActionResult GetAllMenus(string area2, string controller, string action)
        {
            string url = "";
            if (!string.IsNullOrEmpty(area2))
            {
                url += "/" + area2;
            }

            if (!string.IsNullOrEmpty(controller))
            {
                url += "/" + controller;
            }

            if (!string.IsNullOrEmpty(action))
            {
                url += "/" + action;
            }

            if (string.IsNullOrWhiteSpace(SessionHelper.MenuString))
            {
                SessionHelper.MenuString = GetAllForms(url);
            }

            return PartialView("_PartialHeadMenu", SessionHelper.MenuString);
        }

        public string GetAllForms(string url)
        {
            var forms = ProceduresModel.v_mnl_FormRights(db, SessionHelper.UserGroupId.Value, true, "Can Read", "")
                //.Where(p => p.Allowed == true && p.FormRightName == "Can Read")
                .Select(p => p)
                .OrderBy(r => r.ParentForm)
                .ThenBy(u => u.MenuItemPriority).ToList();
            string output = "", head = "", final = "";
            var ModuleId = forms.Where(u => u.FormURL == url).Select(u => u.ModuleId).FirstOrDefault();
            SessionHelper.ModuleID = ModuleId;
            SessionHelper.IsAMBSActive = forms.Where(u => u.FormURL == "/AMBS/Dashboard").Any();
            SessionHelper.IsAcademicsActive = forms.Where(u => u.FormURL == "/Academics/Dashboard" && u.isActive == "Yes").Any();
            SessionHelper.IsSaleInvoiceForBatchActive = forms.Where(u => u.FormURL == "/POS/Sale/AddEditSaleInvoiceBatch" && u.isActive == "Yes").Any();
            forms = forms.Where(u => u.isActive == "Yes").Where(u => !u.IsDashboardPart).ToList();

            foreach (var item in forms.Where(u => u.IsMenuItem == true))
            {
                if (item.PageType == "NoChild")
                {
                    head = "<li class='menu-li'><a href=\"" + item.FormURL + "\" ><span>" + item.MenuText + "</span></a>";
                    output = "";
                }
                else
                {
                    head = "<li class='menu-li'><a href=\"" + item.FormURL + "\" data-toggle=\"dropdown\" class='dropdown-toggle'><span>" + item.MenuText + "</span><span class=\"caret\"></span></a><ul class=\"dropdown-menu\">";
                    output = "";

                    foreach (var subitem in forms.Where(u => u.ParentForm == item.FormID))
                    {
                        if (subitem.ModuleId == ModuleId)
                        {
                            head = "<li><a href=\"" + item.FormURL + "\" data-toggle=\"dropdown\" class='dropdown-toggle'><span>" + item.MenuText + "</span><span class=\"caret\"></span></a><ul class=\"dropdown-menu\">";
                        }

                        if (forms.Where(u => u.ParentForm == subitem.FormID).Count() > 0)
                        {
                            output += "<li class='dropdown-submenu'><a href=\"" + subitem.FormURL + "\" data-toggle=\"dropdown\">" + subitem.MenuText + "</a><ul class=\"dropdown-menu\">";
                            foreach (var subthen in forms.Where(u => u.ParentForm == subitem.FormID))
                            {
                                if (subthen.ModuleId == ModuleId)
                                {
                                    head = "<li><a href=\"" + item.FormURL + "\" data-toggle=\"dropdown\" class='dropdown-toggle'><span>" + item.MenuText + "</span><span class=\"caret\"></span></a><ul class=\"dropdown-menu\">";
                                }

                                if (forms.Where(u => u.ParentForm == subthen.FormID && subthen.PageType == "HaveChilds").Count() > 0)
                                {
                                    output += "<li class='dropdown-submenu'><a href=\"" + subthen.FormURL + "\" data-toggle=\"dropdown\">" + subthen.MenuText + "</a><ul class=\"dropdown-menu\">";
                                    foreach (var subthenthen in forms.Where(u => u.ParentForm == subthen.FormID))
                                    {
                                        if (subthenthen.ModuleId == ModuleId)
                                        {
                                            head = "<li><a href=\"" + item.FormURL + "\" data-toggle=\"dropdown\" class='dropdown-toggle'><span>" + item.MenuText + "</span><span class=\"caret\"></span></a><ul class=\"dropdown-menu\">";
                                        }

                                        if (subthenthen.MenuText == "Dashboard")
                                        {
                                            output += "<li><a href=\"" + subthenthen.FormURL + "\"><b>" + subthenthen.MenuText + "</b></a></li><li role='presentation' class='divider'></li>";
                                        }
                                        else
                                        {
                                            output += "<li><a href=\"" + subthenthen.FormURL + "\">" + subthenthen.MenuText + "</a></li>";
                                        }
                                    }
                                    output += "</ul></li>";
                                }
                                else
                                {
                                    if (subthen.MenuText == "Dashboard")
                                    {
                                        output += "<li><a href=\"" + subthen.FormURL + "\"><b>" + subthen.MenuText + "</b></a></li><li role='presentation' class='divider'></li>";
                                    }
                                    else
                                    {
                                        output += "<li><a href=\"" + subthen.FormURL + "\">" + subthen.MenuText + "</a></li>";
                                    }
                                }
                            }
                            output += "</ul></li>";
                        }
                        else
                        {
                            if (subitem.MenuText == "Dashboard")
                            {
                                output += "<li><a href=\"" + subitem.FormURL + "\"><b>" + subitem.MenuText + "</b></a></li><li role='presentation' class='divider'></li>";
                            }
                            else
                            {
                                output += "<li><a href=\"" + subitem.FormURL + "\">" + subitem.MenuText + "</a></li>";
                            }
                        }
                    }
                    output += "</ul></li>";
                }
                final += head + output;
            }
            //output += "</ul>";
            return final;
        }

        //public ReportsModel ReportDashboard_Statistics()
        //{
        //    ReportsModel ex = new ReportsModel();
        //    //var todayAttendance = "";
        //    ex.TPresent = "0";
        //    ex.TAbsent = "0";
        //    ex.TTeacher = "0";
        //    ex.TBalance = "0";
        //    return ex;
        //}

        public ActionResult GetGlobalSettings()
        {
            //if (!SessionHelper.IsMasterBranch)
            //{

            //}


            var parentForm = db.Forms.Where(s => s.FormName == "Global Settings").Select(s => s.FormID).FirstOrDefault();

            return PartialView("_PartialGlobalSettings", db.Forms.Where(s => s.ParentForm == parentForm).ToList());
        }

        //public ActionResult GetSessionList()
        //{
        //    var sessions = db.UserSessions.Where(s => s.BranchId == SessionHelper.BranchId && s.Assigned == true && s.UserId == SessionHelper.UserId && s.Session.Active).Select(s => s.Session).ToList();

        //    return PartialView("_PartialSessionsList", sessions);
        //}

        #region Messaging
        public ActionResult GetDashboardMessaging(int mid, int ttid)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            // SessionHelper.TemplateTypeId = ttid;
            //var links = db.CertificateActionLinks.Where(c => c.FormID == fid).Select(c=>c.CertificateId).ToList();
            var templatesList = db.Templates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).Where(c => c.TemplateTypeId == ttid).ToList();
            var typeslist = db.TemplateTypes.Where(v => v.ModuleId == mid && v.TemplateTypeId == ttid).ToList();
            //var certificates = db.Certificates.Include(c=>c.TemplateType).ToList(); 
            //var modules = db.Modules.ToList();
            ViewBag.TypesList = typeslist;
            var actionList = new List<Form>();
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
                var al = new Form
                {
                    FormID = 1,
                    FormName = temp.Title,
                    FormURL = url,
                    ParentForm = temp.TemplateType.TemplateTypeId,
                    IsMenuItem = true,
                    IsQuickLink = true,
                    ShowOnDesktop = false,
                    Section = "Messaging",
                    Icon = String.IsNullOrWhiteSpace(temp.FaIcon) ? "https://png.icons8.com/color/60/000000/report-card.png" : "fa " + temp.FaIcon + " fa-3x",
                    Module = temp.TemplateType.Module.ModuleName,
                    Area = temp.TemplateType.Module.ModuleName,
                    Controller = "Messaging",
                    Action = "Send",
                    MenuItemPriority = 0
                };
                actionList.Add(al);
            }
            var firstOrDefault = db.TemplateTypes.Find(ttid);
            if (firstOrDefault != null)
            {
                // SessionHelper.TemplateTypeId = firstOrDefault.TemplateTypeId;
                //var certificate = certificatesList.FirstOrDefault();
                var url1 = "/Messaging/AddEdit/" + firstOrDefault.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" +
                           firstOrDefault.TemplateTypeName.Trim().Replace(" ", String.Empty) + "?ttid=" + ttid;
                var al1 = new Form
                {
                    FormID = 1,
                    FormName =
                    "Add New",
                    FormURL = url1,
                    ParentForm = firstOrDefault.TemplateTypeId,
                    IsMenuItem = true,
                    IsQuickLink = true,
                    ShowOnDesktop = false,
                    Section = "Messaging",
                    Icon = String.IsNullOrWhiteSpace("fa-plus-square-o") ? "https://png.icons8.com/color/60/000000/report-card.png" : "fa fa-plus" + " fa-3x",
                    Module = firstOrDefault.Module.ModuleName,
                    Area = firstOrDefault.Module.ModuleName,
                    Controller = "Messaging",
                    Action = "AddEdit",
                    MenuItemPriority = 0
                };
                actionList.Add(al1);
            }
            // }
            SessionHelper.ModuleId = mid;
            SessionHelper.ReturnUrl = HttpContext.Request.RawUrl;
            return PartialView("_PartialDashboardMessaging", actionList);
        }

        public ActionResult GetMessagesLinks(int ttid, string isSent)
        {
            var templatesList = db.Templates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).Where(c => c.TemplateTypeId == ttid).ToList();
            var actionList = new List<Form>();
            var modules = db.Modules.ToList();
            string action = "Send";
            if (isSent == "Sent")
            {
                action = "Sent";
            }
            foreach (var item in templatesList)
            {
                var module = item.TemplateType.Module;
                var al = new Form { FormID = item.TemplateTypeId, FormName = item.Title, FormURL = "/" + "Messaging/" + action + "/" + module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" + item.TemplateType.TemplateTypeName.Trim().Replace(" ", String.Empty) + "/" + item.TemplateId, ParentForm = item.TemplateTypeId, IsMenuItem = true, IsQuickLink = true, ShowOnDesktop = false, Section = item.TemplateId.ToString(), Icon = "https://png.icons8.com/color/60/000000/report-card.png", Module = module.ModuleName, Area = module.ModuleName, Controller = "Messaging", Action = "Send", MenuItemPriority = 0 };
                actionList.Add(al);
            }

            return PartialView("_PartialMessagesQuickLinks", actionList);
        }
        #endregion

        #region Certificates
        public ActionResult GetDashboardIssuedCertificates(int mid, int ttid)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SessionHelper.TemplateTypeId = ttid;
            //var links = db.CertificateActionLinks.Where(c => c.FormID == fid).Select(c=>c.CertificateId).ToList();
            var certificatesList = db.Certificates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).Where(c => c.TemplateTypeId == ttid).ToList();
            //var typeslist = db.TemplateTypes.Where(v => v.ModuleId == mid && v.TemplateTypeId == ttid).ToList();
            //var certificates = db.Certificates.Include(c=>c.TemplateType).ToList(); 
            //var modules = db.Modules.ToList();
            //ViewBag.TypesList = typeslist;
            var actionList = new List<Form>();
            //foreach (var type in typeslist)
            //{
            //   var certificatesList = certificates.Where(x => x.TemplateTypeId == type.TemplateTypeId).ToList();
            //   var module = modules.First(m=>m.ModuleId== type.ModuleId);

            foreach (var cert in certificatesList)
            {
                var url = "";
                if (!string.IsNullOrWhiteSpace(cert.TemplateType.Module.ModuleName))
                {
                    url = "/" + "Certificates/Issued/" + cert.TemplateType.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" + cert.TemplateType.TemplateTypeName.Trim().Replace(" ", String.Empty) + "/" + cert.CertificateId;
                }
                else
                {
                    url = "/Certificates/Issued/" + cert.TemplateType.TemplateTypeName.Trim().Replace(" ", String.Empty) + "/" + cert.CertificateId;
                }
                var al = new Form { FormID = 1, FormName = cert.Title, FormURL = url, ParentForm = cert.TemplateType.TemplateTypeId, IsMenuItem = true, IsQuickLink = true, ShowOnDesktop = false, Section = "Home", Icon = String.IsNullOrWhiteSpace(cert.FaIcon) ? "https://png.icons8.com/color/60/000000/report-card.png" : "fa " + cert.FaIcon + " fa-3x", Module = cert.TemplateType.Module.ModuleName, Area = cert.TemplateType.Module.ModuleName, Controller = "Certificates", Action = "Issued", MenuItemPriority = 0 };
                actionList.Add(al);
            }

            // }
            return PartialView("_PartialDashboardIssuedCertificates", actionList);
        }

        public ActionResult GetDashboardCertificates(int mid, int ttid)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            SessionHelper.TemplateTypeId = ttid;
            //var links = db.CertificateActionLinks.Where(c => c.FormID == fid).Select(c=>c.CertificateId).ToList();
            var certificatesList = db.Certificates.Include(c => c.TemplateType).Include(c => c.TemplateType.Module).Where(c => c.TemplateTypeId == ttid).ToList();
            var typeslist = db.TemplateTypes.Where(v => v.ModuleId == mid && v.TemplateTypeId == ttid).ToList();
            //var certificates = db.Certificates.Include(c=>c.TemplateType).ToList(); 
            //var modules = db.Modules.ToList();
            ViewBag.TypesList = typeslist;
            var actionList = new List<Form>();
            //foreach (var type in typeslist)
            //{
            //   var certificatesList = certificates.Where(x => x.TemplateTypeId == type.TemplateTypeId).ToList();
            //   var module = modules.First(m=>m.ModuleId== type.ModuleId);

            foreach (var cert in certificatesList)
            {
                var url = "";
                if (!string.IsNullOrWhiteSpace(cert.TemplateType.Module.ModuleName))
                {
                    url = "/" + "Certificates/Issue/" + cert.TemplateType.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" + cert.TemplateType.TemplateTypeName.Trim().Replace(" ", String.Empty) + "/" + cert.CertificateId;
                }
                else
                {
                    url = "/Certificates/Issue/" + cert.TemplateType.TemplateTypeName.Trim().Replace(" ", String.Empty) + "/" + cert.CertificateId;
                }
                var al = new Form { FormID = 1, FormName = cert.Title, FormURL = url, ParentForm = cert.TemplateType.TemplateTypeId, IsMenuItem = true, IsQuickLink = true, ShowOnDesktop = false, Section = "Home", Icon = String.IsNullOrWhiteSpace(cert.FaIcon) ? "https://png.icons8.com/color/60/000000/report-card.png" : "fa " + cert.FaIcon + " fa-3x", Module = cert.TemplateType.Module.ModuleName, Area = cert.TemplateType.Module.ModuleName, Controller = "Certificates", Action = "Index", MenuItemPriority = 0 };
                actionList.Add(al);
            }
            var firstOrDefault = db.TemplateTypes.Find(ttid);
            if (firstOrDefault != null)
            {
                //SessionHelper.TemplateTypeId = firstOrDefault.TemplateTypeId;
                //var certificate = certificatesList.FirstOrDefault();
                var url1 = Url.Action("AddEdit", "Certificates", new { area = "", module = firstOrDefault.ModuleId, type = firstOrDefault.TemplateTypeId });
                //+ + firstOrDefault.Module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" +
                //           firstOrDefault.TemplateTypeName.Trim().Replace(" ", String.Empty);
                var al1 = new Form
                {
                    FormID = 1,
                    FormName = "Add New",
                    FormURL = url1,
                    ParentForm = firstOrDefault.TemplateTypeId,
                    IsMenuItem = true,
                    IsQuickLink = true,
                    ShowOnDesktop = false,
                    Section = "Home",
                    Icon = string.IsNullOrWhiteSpace("fa-plus") ? "https://png.icons8.com/color/60/000000/report-card.png" : "fa fa-plus" + " fa-3x",
                    Module = firstOrDefault.Module.ModuleName,
                    Area = firstOrDefault.Module.ModuleName,
                    Controller = "Certificates",
                    Action = "Index",
                    MenuItemPriority = 0
                };
                actionList.Add(al1);
            }
            // }
            SessionHelper.ReturnUrl = HttpContext.Request.RawUrl;
            return PartialView("_PartialDashboardCertificates", actionList);
        }

        public ActionResult GetCertificatesLinks(int tid, string isIssued)
        {
            var certificates = db.Certificates.Include(c => c.TemplateType).Where(v => v.TemplateTypeId == tid).ToList();
            var actionList = new List<Form>();
            var modules = db.Modules.ToList();
            string action = "Issue";
            if (isIssued == "Issued")
            {
                action = "Issued";
            }
            foreach (var item in certificates)
            {
                var module = modules.First(m => m.ModuleId == item.TemplateType.ModuleId);
                var al = new Form { FormID = item.CertificateId, FormName = item.Title, FormURL = "/" + "Certificates/" + action + "/" + module.ModuleName.Trim().Replace(" ", String.Empty).Replace("&", String.Empty) + "/" + item.TemplateType.TemplateTypeName.Trim().Replace(" ", String.Empty) + "/" + item.CertificateId, ParentForm = item.TemplateTypeId, IsMenuItem = true, IsQuickLink = true, ShowOnDesktop = false, Section = "Home", Icon = "https://png.icons8.com/color/60/000000/report-card.png", Module = module.ModuleName, Area = module.ModuleName, Controller = "Certificates", Action = "Index", MenuItemPriority = 0 };
                actionList.Add(al);
            }

            return PartialView("_PartialCertificatesQuickLinks", actionList);
        }
        #endregion


    }
}
