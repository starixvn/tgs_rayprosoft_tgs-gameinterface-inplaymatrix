namespace Tgs.GameInterface.InplayMatrix.Core.Extensions;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo _TimeZone = TimeZoneInfo.CreateCustomTimeZone(id: "eastern time", baseUtcOffset: TimeSpan.FromHours(+8), "China Standard Time", "eastern time");

    public static string DatetimeConvertToString(this DateTimeOffset date)
    {
        return TimeZoneInfo.ConvertTime(date.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd HH.mm.ss");
    }

    public static DateTimeOffset StringConvertToDatetime(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return DateTime.UnixEpoch;
        }
        var date = DateTime.Parse(str);

        return new DateTimeOffset(date, _TimeZone.GetUtcOffset(date));
    }
}
