namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string MerchantCode,
	string MerchantPrefix,
	string PlayerCode,
	int ProductWallet,
	string TransactionId);
