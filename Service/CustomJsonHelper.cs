using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace FAPP.Service
{
    public static class CustomJsonHelper
    {
        public static string RenderPartialViewToString(ControllerContext context, string viewName, object model)
        {
            var controller = context.Controller;
            var partialView = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            var stringBuilder = new StringBuilder();
            using (var stringWriter = new StringWriter(stringBuilder))
            {
                using (var htmlWriter = new HtmlTextWriter(stringWriter))
                {
                    controller.ViewData.Model = model;
                    partialView.View.Render(new ViewContext(controller.ControllerContext, partialView.View, controller.ViewData, new TempDataDictionary(), htmlWriter), htmlWriter);
                }
            }
            return stringBuilder.ToString();
        }

        public static string convertMinutesToHours(double tt)
        {
            return convertMinutesToHours_Base(tt);
        }

        public static string convertMinutesToHours(this decimal? tt)
        {
            return convertMinutesToHours_Base(tt);
        }

        private static string convertMinutesToHours_Base(object val)
        {
            string result = "";
            decimal tt = Math.Abs(Convert.ToDecimal(val));
            if (tt >= 60)
            {
                if (tt <= 0)
                    result = "0:0";
                else
                    result = ((int)(tt / 60)).ToString() +":"+ ((int)(tt % 60)).ToString();
            }
            else
                result = "0:" + (Convert.ToInt16(tt)).ToString();
            return result;
        }

        public static decimal convertMinutesToHoursInDecimal(object val)
        {
            string result = convertMinutesToHours_Base(val);
            return Convert.ToDecimal(result.Replace(':','.'));
        }

        public static decimal getEmployeeDeductionAddition(string type, List<Model.HREmployeeAttendanceRule> rulelist, decimal minutes)
        {
            decimal sal = 0;
            if (rulelist != null)
            {
                var _obj = rulelist.Where(u => u.IsActive && minutes >= u.FromMin && minutes <= u.ToMin).FirstOrDefault();
                if (_obj == null)
                    _obj = rulelist.Where(u => u.IsActive).OrderByDescending(o => o.ToMin).FirstOrDefault();
                if (_obj == null)
                    _obj = new Model.HREmployeeAttendanceRule();
                if (type == "Deduction")
                    sal = _obj.DeductionRate;
                else
                    sal = _obj.OverTimeRate;
            }
            return sal;
        }

        public static double getAdjustmentTimeInMinutes(TimeSpan ArrivalTime, TimeSpan shiftStartTime)
        {
            return (ArrivalTime - shiftStartTime).TotalMinutes;
        }

        public static double getTimeSpendWithinShiftTimeInMinutes(DateTime shiftStartTime, DateTime shiftEndTime, DateTime ArrivalTime, DateTime departureTime)
        {
            return TimeSpan.FromTicks((Math.Max(ArrivalTime.Ticks, shiftStartTime.Ticks) - Math.Min(departureTime.Ticks, shiftEndTime.Ticks))).TotalMinutes;
            //return (Math.Max(ArrivalTime.TimeOfDay.TotalMinutes, shiftStartTime.TimeOfDay.TotalMinutes) - Math.Min(departureTime.TimeOfDay.TotalMinutes, shiftEndTime.TimeOfDay.TotalMinutes));
        }
        public static double getTimeSpendWithinShiftTimeInMinutes2(DateTime shiftStartTime, DateTime shiftEndTime, DateTime ArrivalTime, DateTime departureTime)
        {
            return TimeSpan.FromTicks((Math.Min(departureTime.Ticks, shiftEndTime.Ticks)- Math.Max(ArrivalTime.Ticks, shiftStartTime.Ticks))).TotalMinutes;
            //return (Math.Max(ArrivalTime.TimeOfDay.TotalMinutes, shiftStartTime.TimeOfDay.TotalMinutes) - Math.Min(departureTime.TimeOfDay.TotalMinutes, shiftEndTime.TimeOfDay.TotalMinutes));
        }

        public static string getTimeSpendWithinShiftTimeInHours(DateTime shiftStartTime, DateTime shiftEndTime, DateTime ArrivalTime, DateTime departureTime)
        {
            return convertMinutesToHours_Base(getTimeSpendWithinShiftTimeInMinutes(shiftStartTime, shiftEndTime, ArrivalTime, departureTime));
        }

        public static double getTimeSpendInMinutes(DateTime ArrivalTime, DateTime departureTime)
        {
            if ((departureTime - ArrivalTime).TotalMinutes > 0)
                return (departureTime - ArrivalTime).TotalMinutes;
            else
                return 0;
        }

        public static string getTimeSpendInHours(DateTime ArrivalTime, DateTime departureTime)
        {
            return convertMinutesToHours_Base(getTimeSpendInMinutes(ArrivalTime, departureTime));
        }

        

        public static System.Web.Routing.RouteValueDictionary ConditionalDisable(bool disabled, object htmlAttributes = null)
        {
            var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            if (disabled)
                dictionary.Add("disabled", "disabled");

            return dictionary;
        }

        
    }
}