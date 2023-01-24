using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace FAPP.Helpers
{
    public static class HtmlHelpers
    {
        public static string IsActive(this HtmlHelper html, string control, string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "actives" : "";
        }

        public static string IsActiveMenu(this HtmlHelper html, string action, string control)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = control == routeControl &&
                               action == routeAction;

            return returnActive ? "activeMenu" : "";
        }
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }

        //public static object GetDBNullOrValue<T>(this T val)
        //{
        //    bool isDbNull = true;
        //    var t = typeof(T);

        //    if (Nullable.GetUnderlyingType(t) != null)
        //    {
        //        isDbNull = EqualityComparer<T>.Default.Equals(default(T), val);
        //    }
        //    else if (t.IsValueType)
        //    {
        //        isDbNull = false;
        //    }
        //    else
        //    {
        //        isDbNull = val == null;
        //    }

        //    return isDbNull ? DBNull.Value : (object)val;
        //}

        public static RouteValueDictionary DisabledIf(this object htmlAttributes, bool disabled)
        {
            var attributes = new RouteValueDictionary(htmlAttributes);
            if (disabled)
            {
                attributes["disabled"] = "disabled";
            }
            return attributes;
        }

        public static string YesNoString(this HtmlHelper html, bool? val)
        {
            return Convert.ToBoolean(val) == true ? "Yes" : "No";
        }
    }
}