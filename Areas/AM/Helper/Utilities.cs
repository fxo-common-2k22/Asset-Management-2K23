
using FAPP.DAL;
using FAPP.Model;
using FAPP.Areas.AM.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ProceduresModel = FAPP.Areas.AM.BLL.AMProceduresModel;
public static class AMEnumHelpers
{
    public static IEnumerable<SelectListItem> GetEnumDictionaryTV<T>()
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("T is not an Enum type");
        }

        return Enum.GetValues(typeof(T))
           .Cast<T>()
           .Select(t => new SelectListItem { Text = t.ToString().Replace("_", " "), Value = t.ToString() });
    }

    public static IEnumerable<SelectListItem> GetEnumDictionaryTVNumberValue<T>()
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("T is not an Enum type");
        }

        return Enum.GetValues(typeof(T))
           .Cast<T>()
           .Select(t => new SelectListItem { Text = t.ToString().Replace("_", " "), Value = Convert.ToInt32(t).ToString() });
    }

    public static void Swap(ref int lhs, ref int rhs)
    {
        int temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

    public static List<string> ExtractErrors(ModelStateDictionary modelStateDictionary)
    {
        var modelErrorCollection = (from modelState in modelStateDictionary.Values
                                    where modelState.Errors != null && modelState.Errors.Count > 0
                                    select modelState.Errors)
                                    .SelectMany(item => item)
                                    .Select(modelError => modelError.ErrorMessage)
                                    .ToList();
        return modelErrorCollection;
    }
}

namespace FAPP.Areas.AM.Helpers
{
    public static class Utilities
    {
        public static Int32 TryInt32Parse(Object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            Int32 retVal = 0;
            Int32.TryParse(obj.ToString(), out retVal);
            return retVal;
        }
        public static Double GetTotalDays(DateTime startDate, DateTime endDate)
        {
            return (endDate.Date - startDate.Date).TotalDays;
        }
        public static MvcHtmlString Timeago(this HtmlHelper helper, DateTime dateTime)
        {
            var tag = new TagBuilder("abbr");
            tag.AddCssClass("timeago");
            tag.Attributes.Add("title", dateTime.ToString("s") + "Z");
            tag.SetInnerText(dateTime.ToString());

            return MvcHtmlString.Create(tag.ToString());
        }



        //public static bool SendEmail(EmailModel model)
        //{
        //    if (string.IsNullOrWhiteSpace(model.ToEmail))
        //    {
        //        return false;
        //    }
        //    try
        //    {
        //        MailMessage mail = new MailMessage();
        //        mail.To.Add(model.ToEmail);
        //        mail.From = new MailAddress("no-reply@mysitedemo.us");
        //        mail.Subject = model.EmailSubject;
        //        string Body = model.EMailBody;
        //        mail.Body = Body;
        //        mail.IsBodyHtml = true;
        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "mail.mysitedemo.us";
        //        smtp.Port = 25;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new System.Net.NetworkCredential("no-reply@mysitedemo.us", "Up4fo!61"); // Enter seders User name and password   
        //        smtp.EnableSsl = false;
        //        smtp.Send(mail);
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }            
        //    return true;
        //}

        public static string GetWpIdLabel(int wpid)
        {
            string result = "";
            switch (wpid)
            {
                case 1:
                    result = "Applied";
                    break;
                case 2:
                    result = "Recommended";
                    break;
                case 3:
                    result = "Canceled";
                    break;
                case 4:
                    result = "Approved";
                    break;
                case 5:
                    result = "Rejected";
                    break;
                case 6:
                    result = "Draft";
                    break;
                case 7:
                    result = "Not Recommended";
                    break;

            }
            return result;
        }

        public static List<SelectListItem> GetWpIdListItems()
        {
            List<SelectListItem> statusItems = new List<SelectListItem>();
            statusItems.Add(new SelectListItem
            {
                Text = "Applied",
                Value = "1"
            });
            //statusItems.Add(new SelectListItem
            //{
            //    Text = "Recommended",
            //    Value = "2"
            //});
            statusItems.Add(new SelectListItem
            {
                Text = "Canceled",
                Value = "3"
            });
            statusItems.Add(new SelectListItem
            {
                Text = "Approved",
                Value = "4"
            });
            statusItems.Add(new SelectListItem
            {
                Text = "Rejected",
                Value = "5"
            });
            statusItems.Add(new SelectListItem
            {
                Text = "Draft",
                Value = "6"
            });
            //statusItems.Add(new SelectListItem
            //{
            //    Text = "Recommended by HR Sup",
            //    Value = "7"
            //});
            return statusItems;
        }

        public static string TryStringParse(Object obj)
        {
            return obj == null ? "" : obj.ToString();
        }
        public static string DateFormat(DateTime datetime)
        {
            if (datetime.Date == new DateTime().Date)
            {
                return string.Format("{0:dd/MM/yyyy}", "");
            }
            else
            {
                return string.Format("{0:dd/MM/yyyy}", datetime);
            }

        }
        public static string DateTimeFormat(DateTime datetime)
        {
            if (datetime.Date == new DateTime().Date)
            {
                return string.Format("{0:dd/MM/yyyy hh:mm tt}", "");
            }
            else
            {
                return string.Format("{0:dd/MM/yyyy  hh:mm tt}", datetime);
            }

        }

        public static GraphsModel FetchGraphsData(OneDbContext db, GraphsModel ex)
        {
            if (!string.IsNullOrEmpty(ex.Url))
            {
                DateTime today = DateTime.Now.Date;
                var formrights = ProceduresModel.v_mnl_DashboardViews(db, SessionHelper.UserGroupId.Value, true, "Can Read", ex.Url).ToList();
                if (formrights == null)
                {
                    return ex;
                }

                ex.DashboardConstraint = ProceduresModel.CheckAppConstraints(db, ex.Module);
                ex.v_mnl_FormRights = formrights;
            }
            return ex;
        }
        public static IEnumerable<DateTime> MonthsBetween(
DateTime startDate,
DateTime endDate)
        {
            DateTime iterator;
            DateTime limit;
            var Dattimelist = new List<DateTime>();

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                DateTime date = Convert.ToDateTime("1/" + dateTimeFormat.GetAbbreviatedMonthName(iterator.Month) + "/" + iterator.Year);
                Dattimelist.Add(date);
                iterator = iterator.AddMonths(1);
            }
            return Dattimelist;
        }

        public static object GetDBNullOrValue<T>(this T val)
        {
            bool isDbNull = true;
            var t = typeof(T);
            if (Nullable.GetUnderlyingType(t) != null)
            {
                isDbNull = EqualityComparer<T>.Default.Equals(default(T), val);
            }
            else if (t.IsValueType)
            {
                isDbNull = false;
            }
            else
            {
                isDbNull = val == null;
            }
            return isDbNull ? DBNull.Value : (object)val;
        }

        public static short GetModuleId(OneDbContext db, string moduleName)
        {
            try
            {
                var moduleID = db.Modules.FirstOrDefault(x => x.ModuleName.ToLower().Contains(moduleName.ToLower()))?.ModuleId ?? 0;
                return Convert.ToInt16(moduleID);
            }
            catch (Exception)
            {

                throw;
            }


        }

    }


}