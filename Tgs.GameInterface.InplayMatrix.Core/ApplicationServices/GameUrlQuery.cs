namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

public record GameUrlQuery(
    Uri ApiUrl,
    string MerchantCode,
    string MerchantPrefix,
    string PlayerCode,
    int ProductWallet,
    string GameCode,
    string IpAddress,
    bool IsMobile,
    string Language);
