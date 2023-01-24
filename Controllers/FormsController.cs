using FAPP.BLL;
using FAPP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAPP.Controllers
{
        // GET: Forms
        public class FormsController : BaseController
        {
            // GET: Forms
            public const int applicationId = 16;//10 for Inventory Application
            public PartialViewResult GetAllForms()
            {
                if (SessionHelper.MenuList == null || SessionHelper.MenuList.Count() == 0)
                {
                    SessionHelper.MenuList = FormsBLL.GetAllFormsByUserId(db, SessionHelper.UserId, applicationId);
                }
                return PartialView("_PartialHeadMenu", SessionHelper.MenuList);
            }
            public PartialViewResult GetQuickLinks()
            {
                string FormURL = Request.Url.AbsolutePath;
                string DBFormURL = "";
                var arr = FormURL.Split('/');

                for (int i = 1; i < arr.Count(); i++)
                {
                    if (i <= 3)
                    {
                        DBFormURL += "/" + arr[i];
                    }
                    else
                    {
                        break;
                    }
                }
                var links = FormsBLL.GetQuickLinks(db, SessionHelper.UserId, DBFormURL, (int)SessionHelper.UserGroupId, applicationId);
                ViewBag.CurrentFormURL = FormURL;
                return PartialView("_PartialQuicklinks", links);
            }
        }

}