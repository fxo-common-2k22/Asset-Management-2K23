using FAPP.AM.Models;
using FAPP.Areas.AM.Helpers;
using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using FAPP.Helpers;
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
    [Error]
    [SessionAuthorize]
    public class BaseController : Controller
    {
        public string Messages { get; set; }
        public string Error { get; set; }
        public string Warnings { get; set; }

        private new string Url = "";
        private string AreaName = "";
        private string ControllerName = "";
        private string ActionName = "";
        private string ParentUrl = "";
        bool IsPostRequest = false;
        public string ip = "";
        public short module_ID = Convert.ToInt16(SessionHelper.ModuleID);
        public OneDbContext db;

        public BaseController()
        {
            db = new OneDbContext();
            //ViewBag.Forms = GetAllForms("");
        }



        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    //base.OnActionExecuted(filterContext);
        //    if (Request.IsAjaxRequest())
        //    {

        //    }
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ip = Request.UserHostAddress;
            //string url = "";

            //var form = ProceduresModel.v_mnl_GroupFormRights(db, SessionHelper.UserGroupId);
            AreaName = filterContext.RequestContext.RouteData.DataTokens["area"] == null ? "" : filterContext.RequestContext.RouteData.DataTokens["area"].ToString();
            //ControllerName = filterContext.RequestContext.RouteData.Values["Controller"] == null ? "" : filterContext.RequestContext.RouteData.Values["Controller"].ToString();
            //ActionName = filterContext.RequestContext.RouteData.Values["Action"] == null ? "" : filterContext.RequestContext.RouteData.Values["Action"].ToString();

            //if (!string.IsNullOrEmpty(AreaName))
            //Url += "/" + AreaName;
            //if (!string.IsNullOrEmpty(ControllerName))
            //    Url += "/" + ControllerName;
            //if (!string.IsNullOrEmpty(ActionName))
            //    Url += "/" + ActionName;

            //if (Request.IsAjaxRequest())
            //{
            //url = Request.UrlReferrer.LocalPath;
            //}
            //if (Request.IsAjaxRequest())
            //{
            //    if (string.IsNullOrEmpty(ParentUrl.Trim('/')))
            //        return;
            //    if (!IsPostRequest)
            //        return;
            //}

            if (!string.IsNullOrEmpty(AreaName))
            {

                if (AreaName == "AM")
                {
                    // Auto Request Type Insertion
                    //var requestTypesExist = db.AMRequestTypes.Any();
                    //if (!requestTypesExist)
                    //{
                    //    var requestType = new AMRequestTypes
                    //    {
                    //        RequestTypeId = 1,
                    //        RequestTypeName = "",
                    //        CreatedBy = SessionHelper.UserId,
                    //        CreatedOn = DateTime.Now
                    //    };
                    //    db.AMRequestTypes.Add(requestType);
                    //    db.SaveChanges();
                    //}

                    // Auto Depreciation Types Insertion
                    var DepTypeExist = db.AMDepreciationTypes.Any();
                    if (!DepTypeExist)
                    {
                        var depTypesList = new List<AMDepreciationTypes>();
                        depTypesList.Add(new AMDepreciationTypes
                        {
                            DepreciationTypeId = 1,
                            DepreciationTypeName = "Straight-line",
                            CreatedBy = SessionHelper.UserId,
                            CreatedOn = DateTime.Now
                        });
                        depTypesList.Add(new AMDepreciationTypes
                        {
                            DepreciationTypeId = 2,
                            DepreciationTypeName = "Double declining balance",
                            CreatedBy = SessionHelper.UserId,
                            CreatedOn = DateTime.Now
                        });
                        depTypesList.Add(new AMDepreciationTypes
                        {
                            DepreciationTypeId = 3,
                            DepreciationTypeName = "Units of production",
                            CreatedBy = SessionHelper.UserId,
                            CreatedOn = DateTime.Now
                        });

                        db.AMDepreciationTypes.AddRange(depTypesList);
                        db.SaveChanges();
                    }



                }
            }
            SessionHelper.CurrentUrl = Url;
            //var form = ProceduresModel.v_mnl_FormRights(db, SessionHelper.UserGroupId.Value, null, "", Url).ToList();
            var formGroupRights = ProceduresModel.GetFormAndGroupRights(db, SessionHelper.UserGroupId.Value, null, "", Url);
            var form = formGroupRights.FormRights.ToList();
            ViewBag.FormGroupRights = formGroupRights.GroupRights.ToList();
            var currentAction = form.Where(u => u.FormURL == Url && u.FormRightName == "Can Read").FirstOrDefault();
            if (currentAction == null)
            {
                currentAction = new v_mnl_FormRights_Result();
            }
            
            // old applicaiton code for rights 
            //var CreateAction = form.Where(u => u.FormURL == Url && u.FormRightName == "Can Create").FirstOrDefault();
            //ViewBag.CanCreate = CreateAction == null ? false : CreateAction.Allowed;

            //var DeleteAction = form.Where(u => u.FormURL == Url && u.FormRightName == "Can Delete").FirstOrDefault();
            //ViewBag.CanDelete = DeleteAction == null ? false : DeleteAction.Allowed;

            //var UpdateAction = form.Where(u => u.FormURL == Url && u.FormRightName == "Can Update").FirstOrDefault();
            //ViewBag.CanUpdate = UpdateAction == null ? false : UpdateAction.Allowed;

            ViewBag.Title = db.AppForms.Where(x => x.FormURL == Url).Select(x => x.FormName).FirstOrDefault(); //currentAction.FormName;// form.Where(u => u.FormURL == Url && u.FormRightName == "Can Read").Select(u => u.FormName).FirstOrDefault();
           
            //Get rights of the page based on new rights mechanism
            var formId = db.AppForms.Where(x => x.FormURL == Url && x.ApplicationModule.ApplicationId == 16).Select(x => x.FormId).FirstOrDefault();
            ViewBag.FormActionGroupRights = db.FormActionGroupRights.Where(x => x.FormAction.FormId == formId && x.UserGroupId == SessionHelper.UserGroupId && x.Allowed == true).ToList();

            //
            var Audit = new Audit();
            Audit.AuditDate = DateTime.Now;
            Audit.Operation = "View";
            Audit.ModifiedBy = SessionHelper.UserId;
            Audit.Area = System.Web.HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"] != null ? System.Web.HttpContext.Current.Request.RequestContext.RouteData.DataTokens["Area"].ToString() : "";
            Audit.Controller = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString();
            Audit.Action = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["Action"].ToString();
            Audit.BranchId = SessionHelper.BranchId;
            Audit.Url = Request.RawUrl;
            Audit.IP = Request.UserHostAddress;
            Audit.UserLogId = SessionHelper.UserLogId;
            db.Audits.Add(Audit);
            db.SaveChanges();
            if (!form.Any())
            {
                return;
            }

            if (currentAction != null)
            {
                if (!currentAction.Allowed)
                {
                    //if (Request.IsAjaxRequest())
                    //{
                    //    filterContext.HttpContext.Response.StatusCode = 403;
                    //    filterContext.Result = new JsonResult
                    //    {
                    //        Data = new { Error = "NotAuthorized", LogOnUrl = "/Error/NotAuthorized" },
                    //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    //    };
                    //    filterContext.HttpContext.Response.End();
                    //}

                    //else
                    //    filterContext.Result = new RedirectResult("/Error/NotAuthorized");
                }
            }

            if (Url.Contains("Academics") || Url.Contains("Fee"))
            {
                if (SessionHelper.CurrentSessionId == Guid.Empty)
                {
                    //if ((this.db.SessionDefaultSettings?.Any() ?? false))
                    //{
                    //    filterContext.Result = new RedirectToRouteResult(
                    //                                new RouteValueDictionary
                    //                                {
                    //                                    {"Area", "Academics"},
                    //                                    {"controller", "Sessions"},
                    //                                    {"action", "Create"}
                    //                                }
                    //                            );
                    //}
                    //else
                    //{
                    //    filterContext.Result = new RedirectResult("/Global/Students/Index#sessionTab");
                    //}
                }
            }





            //else
            //    filterContext.Result = new RedirectResult("/Error/NotAuthorized");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        //public static async System.Threading.Tasks.Task<Term> GetCurrentTermByDateAndStage(DateTime date, Guid? stageId = null)
        //{
        //    using (var db = new OneDbContext())
        //    {
        //        var query = db.Terms.Where(s => s.SessionId == SessionHelper.CurrentSessionId &&
        //            s.BranchId == SessionHelper.BranchId &&
        //            (s.StartDate.HasValue && date > s.StartDate.Value) &&
        //            (s.EndDate.HasValue && date < s.EndDate.Value));
        //        if (stageId.HasValue && stageId.Value != Guid.Empty)
        //        {
        //            query = query.Where(s => s.TermsStages.Select(w => w.StageId).Contains(stageId.Value));
        //        }
        //        return await query.FirstOrDefaultAsync();
        //    }
        //}

        public JsonResult GetJsonResult(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public string DbValdationExceptionMessages(System.Data.Entity.Validation.DbEntityValidationException ex)
        {
            // Retrieve the error messages as a list of strings.
            var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            // Join the list to a single string.
            var fullErrorMessage = string.Join("; ", errorMessages);
            TempData["Error"] = fullErrorMessage;

            // Combine the original exception message with the new one.
            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
            // Throw a new DbEntityValidationException with the improved exception message.
            //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            return fullErrorMessage;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.RouteData.Values["area"] != null)
            {
                AreaName = requestContext.RouteData.Values["area"].ToString();
                 SessionHelper.ModuleID = module_ID = Areas.AM.Helpers.Utilities.GetModuleId(db, AreaName);
            }

            if (requestContext.RouteData.DataTokens["area"] != null)
            {
                AreaName = requestContext.RouteData.DataTokens["area"].ToString();
                 SessionHelper.ModuleID = module_ID = Areas.AM.Helpers.Utilities.GetModuleId(db, AreaName);
            }

            Url = "";
            ControllerName = requestContext.RouteData.Values["controller"].ToString();
            ActionName = requestContext.RouteData.Values["action"].ToString();

            if (!string.IsNullOrEmpty(AreaName))
            {
                Url += "/" + AreaName;
            }

            if (!string.IsNullOrEmpty(ControllerName))
            {
                Url += "/" + ControllerName;
            }

            if (!string.IsNullOrEmpty(ActionName))
            {
                Url += "/" + ActionName;
            }

            if (Request.HttpMethod == "POST")
            {
                IsPostRequest = true;
            }

            if (Request.IsAjaxRequest())
            {
                ParentUrl = Request.UrlReferrer.LocalPath;
                if (string.IsNullOrEmpty(ParentUrl.Trim('/')))
                {
                    return;
                }

                if (!IsPostRequest)
                {
                    return;
                }
            }
            if (ControllerContext.IsChildAction)
            {
                return;
            }

            var bm = new DAL.FormAddition();
            if (requestContext.RouteData.Values["area"] != null)
            {
                AreaName = requestContext.RouteData.Values["area"].ToString();
            }

            if (requestContext.RouteData.DataTokens["area"] != null)
            {
                AreaName = requestContext.RouteData.DataTokens["area"].ToString();
            }

            Url = "";
            ControllerName = requestContext.RouteData.Values["controller"].ToString();
            ActionName = requestContext.RouteData.Values["action"].ToString();

            if (!string.IsNullOrEmpty(AreaName))
            {
                Url += "/" + AreaName;
            }

            if (!string.IsNullOrEmpty(ControllerName))
            {
                Url += "/" + ControllerName;
            }

            if (!string.IsNullOrEmpty(ActionName))
            {
                Url += "/" + ActionName;
            }

            if (SessionHelper.UserGroupId.HasValue)
            {
                //bm.AddForm(db, ControllerName, ActionName, AreaName, (int)SessionHelper.UserGroupId, Url, Request.IsAjaxRequest(), ParentUrl);
            }
        }

        public string GetModelStateErrors()
        {
            return string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue // Use this value to set your maximum size for all of your Requests
            };
        }

        public async System.Threading.Tasks.Task RecordActionHistoryLog(int ModuleId, string ParentUrl, string ParentRecordType, string ParentRecordId, string RecordType, string RecordId, string Url, string ActionType, string Description, int? UserGroupId = null, long? FormId = null, string MessageType = null, string AlertColor = null, DateTime? ValidityFromDate = null, DateTime? ValidityToDate = null, bool Closed = false, bool Show = false, bool PopUp = false)
        {
            var Entity = new ActionHistoryLog();
            Entity.BranchId = SessionHelper.BranchId;
            Entity.ModuleId = ModuleId;
            Entity.ActionType = ActionType;
            Entity.Description = Description;
            Entity.ParentType = ParentRecordType;
            Entity.ParentRecordId = ParentRecordId;
            Entity.ParentURL = ParentUrl;
            Entity.RecordType = RecordType;
            Entity.RecordId = RecordId;
            Entity.URL = Url;
            Entity.CreatedBy = SessionHelper.UserId;
            Entity.CreatedOn = DateTime.Now;
            Entity.CreatedByIP = SessionHelper.IP;
            Entity.UserGroupId = UserGroupId;
            Entity.FormId = FormId;
            Entity.MessageType = MessageType;
            Entity.AlertColor = AlertColor;
            Entity.ValidityFromDate = ValidityFromDate;
            Entity.ValidityToDate = ValidityToDate;
            Entity.Closed = Closed;
            Entity.Show = Show;
            Entity.PopUp = PopUp;
            db.ActionHistoryLogs.Add(Entity);
            await db.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateMembershipFormUrl(long FormId, string NewFormUrl)
        {
            var form = await db.Forms.Where(s => s.FormID == FormId && s.FormURL != NewFormUrl).FirstOrDefaultAsync();
            if (form != null)
            {
                form.FormURL = NewFormUrl;
                await db.SaveChangesAsync();
            }
        }
    }
}
