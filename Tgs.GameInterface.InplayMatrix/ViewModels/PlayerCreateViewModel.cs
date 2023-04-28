namespace Tgs.GameInterface.InplayMatrix.ViewModels;

public record PlayerCreateViewModel(
    Uri ApiUrl,
    string MerchantCode,
    string MerchantPrefix,
    string PlayerCode,
    string Currency,
    string Password);
