namespace Tgs.GameInterface.InplayMatrix.Core.Extensions;

public static class DateTimeExtensions
{
    public static string DatetimeConvertToString(this DateTimeOffset date)
    {
        var _timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "China Standard Time", baseUtcOffset: TimeSpan.FromHours(+8), "China Standard Time", "China Standard Time");
        return TimeZoneInfo.ConvertTime(date.ToUniversalTime(), _timeZone).ToString("yyyy-MM-dd HH.mm.ss");
    }

    public static DateTimeOffset StringConvertToDatetime(this string str)
    {
        return DateTimeOffset.Parse(str);
    }
}
