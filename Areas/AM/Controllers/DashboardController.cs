using FAPP.Areas.AM.BLL;
using FAPP.Areas.AM.ViewModels;
using FAPP.DAL;
using System;
using System.Web.Mvc;

namespace FAPP.Areas.AM.Controllers
{
    public class DashboardController : FAPP.Controllers.BaseController
    {
        //private OneDbContext db = new OneDbContext();
        // GET: POS/Dashboard
        public ActionResult Index()
        {
            GraphsModel graphsModel = new GraphsModel();
            graphsModel.Action = ControllerContext.RouteData.Values["action"].ToString();
            graphsModel.Contoller = ControllerContext.RouteData.Values["Controller"].ToString();
            graphsModel.Area = ControllerContext.RouteData.DataTokens["area"].ToString();
            graphsModel.Module = ControllerContext.RouteData.DataTokens["area"].ToString();
            DateTime today = DateTime.Now.Date;
            graphsModel.Url = Request.RawUrl;
            graphsModel = DashboardGraphBLL.FetchGraphsData(db, graphsModel);
            DashboardGraphBLL.GetDataForDashboardItems(db, graphsModel, SessionHelper.BranchId, today, SessionHelper.UserId);
            var formGroupRights = AMProceduresModel.GetFormAndGroupRights(db, SessionHelper.UserGroupId.Value, null, "", "/AM/Dashboard/Index");
            graphsModel.v_mnl_FormRights = formGroupRights.GroupRights;
            return View(graphsModel);
        }
    }
}