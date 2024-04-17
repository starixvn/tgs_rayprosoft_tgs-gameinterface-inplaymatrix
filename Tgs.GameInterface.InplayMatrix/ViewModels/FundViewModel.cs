namespace Tgs.GameInterface.InplayMatrix.ViewModels;

public record FundViewModel(
	Uri ApiUrl,
	string MerchantCode,
	string MerchantPrefix,
	string PlayerCode,
	int ProductWallet,
	string TransactionId,
	decimal Amount);
