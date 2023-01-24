
using System;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
namespace FAPP.DAL
{
    public class Error : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            //if(filterContext.RequestContext.HttpContext.is)
            Exception ex = filterContext.Exception;
            if (!SessionHelper.ShowExceptionToUser)
                filterContext.ExceptionHandled = true;
            string url = "";
            var AreaName = filterContext.RequestContext.RouteData.DataTokens["area"] == null ? "" : filterContext.RequestContext.RouteData.DataTokens["area"].ToString();
            var ControllerName = filterContext.RequestContext.RouteData.Values["Controller"] == null ? "" : filterContext.RequestContext.RouteData.Values["Controller"].ToString();
            var ActionName = filterContext.RequestContext.RouteData.Values["Action"] == null ? "" : filterContext.RequestContext.RouteData.Values["Action"].ToString();
            if (!string.IsNullOrEmpty(AreaName))
                url += "/" + AreaName;
            if (!string.IsNullOrEmpty(ControllerName))
                url += "/" + ControllerName;
            if (!string.IsNullOrEmpty(ActionName))
                url += "/" + ActionName;
            // SendMail("zeeshanoffice77@gmail.com", ex, "Error in " + url);            
            SaveErrorLog(ex, "Error in " + url);
            if (!SessionHelper.ShowExceptionToUser)
                filterContext.Result = new RedirectResult("/Error/Index");
            base.OnException(filterContext);
            return;
        }

        protected bool SendMail(string To, Exception ex, string subject)
        {
            try
            {
                SmtpClient emailClient = new SmtpClient();
                var user = "alert@ezschoolerp.com";
                var pass = "Q6b8g@b5";
                emailClient.Host = "mail.ezschoolerp.com";
                emailClient.Port = 25;
                emailClient.EnableSsl = false;

                MailMessage message = new MailMessage(user, To, subject, "");
                message.Body = "<font size=5 color=red><b>" + ex.Message + "</b></font><br /> " + ex.StackTrace;

                message.IsBodyHtml = true;
                var SMTPUserInfo = new System.Net.NetworkCredential(user, pass);
                emailClient.UseDefaultCredentials = false;
                emailClient.Credentials = SMTPUserInfo;
                emailClient.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        protected bool SaveErrorLog(Exception ex, string subject)
        {
            try
            {
                using (var db = new Model.OneDbContext())
                {
                    Model.ErrorLogs _Logs = new Model.ErrorLogs();
                    _Logs.Subject = subject;
                    _Logs.Exception = "<font size=5 color=red><b>" + ex.Message + "</b></font><br /> " + ex.StackTrace; ;
                    _Logs.Message = ProceduresModel.GetExceptionMessages(ex);
                    _Logs.UserId = SessionHelper.UserID;
                    _Logs.BranchId = (short)SessionHelper.BranchId;
                    _Logs.IP = SessionHelper.IP;
                    _Logs.ExceptionDate = DateTime.Now;
                    db.ErrorLogs.Add(_Logs);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }

    public class FormAddition
    {
        public void AddForm(Model.OneDbContext dc, string controller, string action, string area, int userGroupId, string Url, bool IsAjaxRequest, string ParentUrl)
        {
            //var form = dc.Forms.FirstOrDefault(p => p.Area == area && p.Controller == controller && p.Action == action);
            //var form = dc.Forms.Where(p => p.FormUrl.ToLower() == Url).FirstOrDefault();
            var form = GetFrom(dc, controller, action, area, Url);

            if (form == null)
            {
                form = new Model.Form();

                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

                form.FormName = myTI.ToTitleCase(Regex.Replace(action, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 "));

                form.Area = FirstCharToUpper(area);
                form.Controller = FirstCharToUpper(controller);
                form.Action = FirstCharToUpper(action);

                var parentfrom = dc.Forms.Where(u => u.FormURL.ToLower().Contains(area.ToLower() == "" ? controller.ToLower() : area.ToLower())).Select(u => u.ParentForm).FirstOrDefault();
                form.ParentForm = parentfrom;
                if (IsAjaxRequest)
                {
                    parentfrom = dc.Forms.Where(u => u.FormURL.ToLower() == ParentUrl.ToLower()).Select(u => u.FormID).FirstOrDefault();
                }
                //if (!parentfrom.HasValue)
                //{
                //    parentfrom = dc.Forms.Where(u => u.Controller.ToLower() == area.ToLower()).Select(u => u.ParentForm).FirstOrDefault();
                //}
                //switch (area)
                //{
                //    case "Finance":
                //        form.ParentForm = 2000;
                //        form.Section = "Finance";
                //        form.Module = "Finance";
                //        break;
                //    case "FixedAsset":
                //        form.ParentForm = 3000;
                //        form.Section = "Fixed Assets";
                //        form.Module = "Fixed Assets";
                //        break;
                //    case "HR":
                //        form.ParentForm = 4000;
                //        form.Section = "HR";
                //        form.Module = "HR";
                //        break;
                //    case "Settings":
                //        form.ParentForm = 5000;
                //        form.Section = "Setup";
                //        form.Module = "Setup";
                //        break;
                //    case "Inventory":
                //        form.ParentForm = 7000;
                //        form.Section = "Inventory";
                //        form.Module = "Inventory";
                //        break;
                //    default:
                //        form.ParentForm = 6000;
                //        form.Section = "User Management";
                //        form.Module = "User Management";
                //        break;
                //}

                if (!string.IsNullOrWhiteSpace(controller))
                    controller = "/" + FirstCharToUpper(controller);

                if (!string.IsNullOrWhiteSpace(area))
                    area = "/" + FirstCharToUpper(area);

                if (!string.IsNullOrWhiteSpace(action))
                    action = "/" + FirstCharToUpper(action);

                form.FormURL = $"{area}{controller}{action}";
                var urlWOA = $"{area}{controller}";
                var urlWOAIndex = $"{area}{controller}/Index";

                if (!string.IsNullOrWhiteSpace(action.Trim('/')) && action.Trim('/').ToUpper() != "INDEX")
                {
                    form.ParentForm = dc.Forms.FirstOrDefault(p => p.FormURL.ToLower() == urlWOA.ToLower() || p.FormURL.ToLower() == urlWOAIndex.ToLower())?.FormID;
                }
                string ch = "";

                var formId = form.ParentForm ?? 0;
                do
                {
                    formId = dc.Forms.Where(u => u.FormID.ToString().StartsWith(form.ParentForm.ToString().Substring(0, 1))).Max(u => u.FormID);
                    ch = formId.ToString().Substring(1, formId.ToString().Length - 1);
                    formId = System.Convert.ToInt32(form.ParentForm.ToString().Substring(0, 1) + System.String.Format("{0:D3}", System.Convert.ToInt32(ch) + 1));
                } while (dc.Forms.Any(p => p.FormID == formId));

                //formId++;
                form.FormID = formId;
                form.MenuItemPriority = (short)formId;
                form.IsMenuItem = false;
                form.ShowOnDesktop = false;
                form.IsDashboardPart = false;
                form.IsGroupReportHead = false;
                form.IsHideChilds = false;
                form.IsAjaxRequest = IsAjaxRequest;

                dc.Forms.Add(form);
                dc.SaveChanges();

                ProceduresModel.p_mnl_FillGroupRights(dc, userGroupId);
                dc.SaveChanges();
            }
        }

        public static string FirstCharToUpper(string chars)
        {
            if (System.String.IsNullOrEmpty(chars))
                return string.Empty;
            return chars.First().ToString().ToUpper() + chars.Substring(1);
        }

        public Model.Form GetFrom(Model.OneDbContext db, string controller, string action, string area, string Url)
        {
            var form = db.Forms.Where(p => p.FormURL.ToLower() == Url.ToLower()).FirstOrDefault();
            if (form == null)
                form = db.Forms.Where(p => p.Area.ToLower() == area.ToLower() && p.Controller.ToLower() == controller.ToLower() && p.Action.ToLower() == action.ToLower()).FirstOrDefault();
            if (form == null)
            {
                if (!string.IsNullOrEmpty(area))
                    Url += "/" + area;
                if (!string.IsNullOrEmpty(controller))
                    Url += "/" + controller;
                if (!string.IsNullOrEmpty(action))
                {
                    if (action.ToLower() == "index")
                        action = "";
                    else
                        Url += "/" + action;
                }
                form = db.Forms.Where(p => p.FormURL.ToLower() == Url.ToLower()).FirstOrDefault();
            }
            return form;
        }

    }
}