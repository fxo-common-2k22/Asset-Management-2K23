using System.Web;
using System.Web.Mvc;

namespace FAPP.DAL
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["BranchId"] != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var url = filterContext.HttpContext.Request.RawUrl;
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                filterContext.Result = new JsonResult
                {
                    Data = new { Error = "NotAuthorized", LogOnUrl = "/Account/Login?returnUrl=" + url },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
                filterContext.HttpContext.Response.End();
            }
            else
                filterContext.Result = new RedirectResult("/Account/Login?returnUrl=" + url);
        }
    }

    //public class ApiAuthorizeAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(HttpActionContext filterContext)
    //    {
    //        var re = filterContext.Request;
    //        var headers = re.Headers;

    //        tokenId = string.Empty;
    //        tokenValue = string.Empty;
    //        if (headers.Contains("username"))
    //        {
    //            tokenId = headers.GetValues("username").First();
    //        }
    //        if (headers.Contains("password"))
    //        {
    //            tokenValue = headers.GetValues("password").First();
    //        }
    //    }
    //}
}