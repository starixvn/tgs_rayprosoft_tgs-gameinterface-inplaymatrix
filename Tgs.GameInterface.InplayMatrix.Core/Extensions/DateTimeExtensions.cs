namespace Tgs.GameInterface.InplayMatrix.Core.Extensions;

public static class DateTimeExtensions
{
	public static string DatetimeConvertToString(this DateTimeOffset date)
	{
		return date.ToUniversalTime().AddHours(+8).ToString("yyyy-MM-dd HH.mm.ss");
	}

	public static DateTimeOffset StringConvertToDatetime(this string str)
	{
		return DateTimeOffset.Parse(str);
	}
}
