using System;

public static class IntEM
{
    public static string BytesToString(this long len)
    {
        decimal lenD = len * 1M;
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        int order = 0;
        while (lenD >= 1024 && order < sizes.Length - 1)
        {
            order++;
            lenD = lenD / 1024;
        }
        return lenD.ToString("N2") + " " + sizes[order];
    }
    public static string NumberWithComma<T>(this T input) => $"{input:n}";

    public static string ToCurrencyFormat (this decimal de)
    {
        return string.Format("{0:N2}",de);
    }
}

public static class DateTimeEM
{
    //public static DateTime FirstDayOfMonth(this DateTime dt);
    //public static string GetElapsedTime(this DateTime ElapsedDate);
    //public static int GetHalf(this DateTime dt);
    //public static int GetIso8601WeekOfYear(this DateTime time);
    //public static DateTime GetLastDayOfMonth(DateTime dt);
    //public static int GetQuarter(this DateTime dt);
    //public static int GetWeekOfYear(this DateTime time);
    //public static DateTime ToDateTime(this TimeSpan value);
    public static string ToddMMMyyyy(this DateTime date)
    {
        return date.ToString("dd-MMM-yyyy");
    }
    //public static long? ToJsonTicks(this DateTime? value);
    //public static long ToJsonTicks(this DateTime value);

    public static string ToMMMyyyyString(this DateTime date)
    {
        return date.ToString("MMM yyyy");
    }
    public static DateTime GetLastDayOfMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month));
    }
    public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month,1);
    }
}