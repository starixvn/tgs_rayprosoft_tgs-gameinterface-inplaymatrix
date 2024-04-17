namespace Tgs.GameInterface.InplayMatrix.ViewModels;

public record PlayerBalanceViewModel(
	Uri ApiUrl,
	string MerchantCode,
	string MerchantPrefix,
	string PlayerCode,
	int ProductWallet);
