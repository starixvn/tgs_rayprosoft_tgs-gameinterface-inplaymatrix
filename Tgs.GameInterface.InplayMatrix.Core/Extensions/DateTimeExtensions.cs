using System;

namespace Tgs.GameInterface.InplayMatrix.Core.Extensions;

public static class DateTimeExtensions
{
    public static string DatetimeConvertToString(this DateTimeOffset date)
    {
        var timeZone = TimeZoneInfo.CreateCustomTimeZone(id: "china standard time", baseUtcOffset: TimeSpan.FromHours(+8), "china standard time", "china standard time");
        return new DateTimeOffset(date.DateTime, timeZone.GetUtcOffset(date)).ToString("yyyy-MM-dd HH.mm.ss");
    }

    public static DateTimeOffset StringConvertToDatetime(this string str)
    {
        return DateTimeOffset.Parse(str);
    }
}
