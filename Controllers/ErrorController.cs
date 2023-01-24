using System.Web.Mvc;

namespace FAPP.Controllers
{
    public class ErrorController : Controller
    {
        [HandleError]
        public ViewResult Index()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            if (Request.RawUrl == "/Error/NotFound?aspxerrorpath=/Academics/Student/SearchFamily")
                return Redirect("/Home/Index");
            else
            {
                Response.StatusCode = 404;  //you may want to set this to 200
                return View();
            }
        }

        public ViewResult NotAuthorized(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}