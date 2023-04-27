namespace Tgs.GameInterface.InplayMatrix.ViewModels;

public record GameUrlQueryViewModel(
    Uri ApiUrl,
    string MerchantCode,
    string MerchantPrefix,
    string PlayerCode,
    int ProductWallet,
    string GameCode,
    string IpAddress,
    bool IsMobile,
    string Language);
