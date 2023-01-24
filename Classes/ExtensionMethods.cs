using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FAPP
{
    public class PagedListExtended<T> : BasePagedList<T>
    {
        private PagedListExtended()
        {
        }

        public static async Task<IPagedList<T>> Create(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            var list = new PagedListExtended<T>();
            await list.Init(superset, pageNumber, pageSize);
            return list;
        }

        async Task Init(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, "PageNumber cannot be below 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "PageSize cannot be less than 1.");
            }

            TotalItemCount = superset == null ? 0 : await superset.CountAsync();
            PageSize = pageSize;
            PageNumber = pageNumber;
            PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;
            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < PageCount;
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber >= PageCount;
            FirstItemOnPage = (PageNumber - 1) * PageSize + 1;
            var num = FirstItemOnPage + PageSize - 1;
            LastItemOnPage = num > TotalItemCount ? TotalItemCount : num;
            if (superset == null || TotalItemCount <= 0)
            {
                return;
            }

            Subset.AddRange(pageNumber == 1 ? await superset.Skip(0).Take(pageSize).ToListAsync() : await superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync());
        }
    }

    public static class PagedListExtendedExtensions
    {
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, int pageNumber, int pageSize)
        {
            return await PagedListExtended<T>.Create(superset, pageNumber, pageSize);
        }
    }


    public static class ExtensionMethods
    {
        public static Dictionary<int, string> GetEnumDictionary<T>()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T is not an Enum type");
            }
            return System.Enum.GetValues(typeof(T))
               .Cast<T>()
               .ToDictionary(t => (int)(object)t, t => t.ToString().Replace("_", " "));
        }

        // Required For Getting Exception messsages
        public static string GetExceptionMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            var str = string.Join(Environment.NewLine, messages.LastOrDefault());
            if (str.Contains("conflicted"))
            {
                string newString = str.Substring(str.LastIndexOf("table"));//Remove string till "table"
                newString = newString.Substring(0, newString.IndexOf(","));//Remove string till comma
                newString = newString.Replace("table ", "");//Remove tbale keyword
                newString = newString.Substring(newString.LastIndexOf("."));//Remove schema name
                newString = newString.Replace(".", "");//Remove dot
                string[] split = Regex.Split(newString, @"(?<!^)(?=[A-Z])");//Insert Space Before UpperCase
                if (str.ToUpper().Contains("Insert".ToUpper()))
                {
                    str = $"Insertion failed, Conflict in '{ string.Join(" ", split)}'";
                }
                else
                {
                    str = $"Deletion failed, It is being used in '{ string.Join(" ", split)}'";
                }
            }
            return String.Join(Environment.NewLine, str);
        }



        public static string GetPropertyName<T>(System.Linq.Expressions.Expression<Func<T>> propertyLambda)
        {
            var me = propertyLambda.Body as System.Linq.Expressions.MemberExpression;
            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }
            return me.Member.Name;
        }



        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem) where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }






        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex(@"FROM\s+(?<table>.+)\s+AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }

        public static Exception InnermostException(this Exception e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            while (e.InnerException != null)
            {
                e = e.InnerException;
            }

            return e;
        }

        public static void MatchAndMap<TSource, TDestination>(this TSource source, TDestination destination)
           where TSource : class, new()
           where TDestination : class, new()
        {
            if (source != null && destination != null)
            {
                List<PropertyInfo> sourceProperties = source.GetType().GetProperties().ToList<PropertyInfo>();
                List<PropertyInfo> destinationProperties = destination.GetType().GetProperties().ToList<PropertyInfo>();

                foreach (PropertyInfo sourceProperty in sourceProperties)
                {
                    PropertyInfo destinationProperty = destinationProperties.Find(item => item.Name == sourceProperty.Name);

                    if (destinationProperty != null)
                    {
                        try
                        {
                            destinationProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }

        }
        public static TDestination MapProperties<TDestination>(this object mapSource)
         where TDestination : class, new()
        {
            var destination = Activator.CreateInstance<TDestination>();
            MatchAndMap(mapSource, destination);

            return destination;
        }

        public static IHtmlString YesNoRadioFor(this HtmlHelper helper, string content)
        {
            string LableStr = $"<label style=\"background-color:blue;color:red;font-size:24px\">{content}</label>";
            return new HtmlString(LableStr);

        }
        public static string YesNoString(this HtmlHelper html, bool? val)
        {
            return Convert.ToBoolean(val) == true ? "Yes" : "No";
        }
        public static string TruncateLongString(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.Substring(0, Math.Min(str.Length, maxLength));
        }

        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static string ToMMMyyyyString(this DateTime dt)
        {
            return string.Format("{0:MMM yyyy}", dt);
        }

        public static string ToddMMMyyyyString(this DateTime dt)
        {
            return string.Format("{0:dd MMM yyyy}", dt);
        }

        static GregorianCalendar _gc = new GregorianCalendar();

        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        static int GetWeekOfYear(this DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
        public static int ToZeroAndOne(this bool dt)
        {
            return dt == true ? 1 : 0;
        }

        public static bool ContainsOnlyNumbers(this string str)
        {
            return str.All(char.IsDigit);
        }

        public static string RemoveDigit(this string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, @"[\d-]", string.Empty);
            //return new (@"^\\d+|\\d+$").Replace(input, "");
        }

        public static long ToLong(this string input)
        {
            return Convert.ToInt64(input);
            //return new (@"^\\d+|\\d+$").Replace(input, "");
        }

        public static IEnumerable<string> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage + " " + v.Exception).ToList();

        }

        public static string NumberWithComma<T>(this T input) => $"{input:n}";

        public static string GetFirstTwoCharacters(this string input) => input.Count() > 1 ? input.Trim().Substring(0, 2) : input;

        public static IEnumerable<T> Select<T>(this System.Data.IDataReader reader, Func<System.Data.IDataReader, T> projection)
        {
            while (reader.Read())
            {
                yield return projection(reader);
            }
        }
    }

    public static class DateTimeExtensionMethods
    {
        public static DateTime ToDateTime(this string s, string format = "ddMMyyyy", string cultureString = "en-GB")
        {
            try
            {
                var r = DateTime.ParseExact(
                    s: s,
                    format: format,
                    provider: CultureInfo.GetCultureInfo(cultureString));
                return r;
            }
            catch (FormatException)
            {
                throw;
            }
            catch (CultureNotFoundException)
            {
                throw; // Given Culture is not supported culture
            }
        }
        //return new (@"^\\d+|\\d+$").Replace(input, "");
        public static List<string> SplitStringByChar(this string input, char character)
        {
            return input.Replace(" ", string.Empty).Split(character).ToList<string>();
        }

        public static IEnumerable<DateTime> Range(this DateTime startDate, DateTime endDate)
        {
            List<DateTime> months = new List<DateTime>();
            DateTime iterator;
            DateTime limit;

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
                months.Add(new DateTime(iterator.Year, iterator.Month, 1));
                iterator = iterator.AddMonths(1);
            }
            return months.OrderBy(s => s);
            //return Enumerable.Range(0, (endDate - startDate).Days + 1).Select(d => startDate.AddDays(d));
        }

        public static string ToyyyyMMdd(this DateTime? date)
        {
            return string.Format("{0:yyyy-MM-dd}", date);
        }

        public static string ToddMMyyyy(this DateTime? date)
        {
            return string.Format("{0:dd-MM-yyyy}", date);
        }

        public static string ToddMMMyyyy(this DateTime? date)
        {
            return string.Format("{0:dd-MMM-yyyy}", date);
        }

        public static string ToMMYYYYString(this DateTime? date)
        {
            return string.Format("{0:MM-yyyy}", date);
        }

        public static string ToMMMYYYYString(this DateTime? date)
        {
            return string.Format("{0:MMM-yyyy}", date);
        }
    }
    public static class GuidExtensions
    {
        public static string GetStringNull(this Guid val)
        {
            return val == Guid.Empty ? " = null " : string.Empty;
        }
    }
}