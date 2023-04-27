namespace Tgs.GameInterface.InplayMatrix.Core.Extensions;

public static class StringExtension
{
    public static string NoLongerThan(this string str, int chars)
        => string.IsNullOrEmpty(str) || str.Length <= chars ? str : str[..chars];

    public static string ToPlayerCode(this string playerId, string Prefix)
        => playerId.Replace($"{Prefix}_", string.Empty);

    public static string ToPlayerid(this string playerCode, string Prefix)
            => $"{Prefix}_{playerCode}";
}
