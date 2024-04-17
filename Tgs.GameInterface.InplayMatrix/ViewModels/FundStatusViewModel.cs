namespace Tgs.GameInterface.InplayMatrix.ViewModels;

public record FundStatusViewModel(
	Uri ApiUrl,
	string MerchantCode,
	string MerchantPrefix,
	string PlayerCode,
	int ProductWallet,
	string TransactionId);
