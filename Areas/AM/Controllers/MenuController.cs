using FAPP.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FAPP.Areas.AM.Controllers
{
    public class MenuController : BaseController
    {
        // GET: AM/Menu
        public ActionResult Index()
        {
            /// hiding child of Reports,Budgets and Reconcilation
            Int64[] Ids = new Int64[] { 13003, 13014, 13026, 13036, 13027, 13013, 13684, 13686 };
            var objform = db.Forms.Where(x => Ids.Contains(x.FormID)).ToList();
            try
            {
                if (objform != null && objform.Count() > 0)
                {
                    objform.ForEach(x => x.isActive = "No");
                }


                var ReportsMenu = db.Forms.FirstOrDefault(x => x.FormID == 1300102);
                if (ReportsMenu != null)
                {
                    ReportsMenu.IsHideChilds = true;
                }

                var issue = db.Forms.FirstOrDefault(x => x.FormID == 13012);
                if (issue != null)
                {
                    issue.FormName = issue.MenuText = "Issuance / Return / Damage";
                    issue.FormURL = "/AM/Issue/IssueItems";
                }

                var issueMenu = db.Forms.FirstOrDefault(x => x.FormID == 13010);
                if (issueMenu != null)
                {
                    issueMenu.MenuText = issueMenu.FormName = "Issuance";
                }

                //var Manageissue = db.Forms.FirstOrDefault(x => x.FormID == 13013);
                //if (Manageissue != null)
                //{
                //    Manageissue.isActive = "No";
                //}

                var depreciationTypes = db.AMDepreciationTypes.Any();
                var UserId = FAPP.DAL.SessionHelper.UserID;
                if (!depreciationTypes)
                {
                    db.AMDepreciationTypes.Add(new FAPP.AM.Models.AMDepreciationTypes
                    {
                        DepreciationTypeName = "Straight-line",
                        CreatedBy = UserId,
                        CreatedOn = DateTime.Now
                    });

                    db.AMDepreciationTypes.Add(new FAPP.AM.Models.AMDepreciationTypes
                    {
                        DepreciationTypeName = "Double declining balance",
                        CreatedBy = UserId,
                        CreatedOn = DateTime.Now
                    });
                    db.AMDepreciationTypes.Add(new FAPP.AM.Models.AMDepreciationTypes
                    {
                        DepreciationTypeName = "Units of production",
                        CreatedBy = UserId,
                        CreatedOn = DateTime.Now
                    });
                }


                // Condition Types

                var checkConditionType = db.AMConditionTypes.Any();
                if (!checkConditionType)
                {
                    db.AMConditionTypes.Add(new Model.AMConditionType
                    {
                        ConditionTypeId = 1,
                        Name = "New"

                    });

                    db.AMConditionTypes.Add(new Model.AMConditionType
                    {
                        ConditionTypeId = 2,
                        Name = "Used"

                    });
                    db.AMConditionTypes.Add(new Model.AMConditionType
                    {
                        ConditionTypeId = 3,
                        Name = "Damage"

                    });
                }

                db.SaveChanges();

            }
            catch (Exception)
            {

            }

            return View();
        }
    }
}