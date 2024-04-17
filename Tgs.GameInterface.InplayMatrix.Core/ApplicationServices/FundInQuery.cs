namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

public record FundInQuery(
	Uri ApiUrl,
	string MerchantCode,
	string MerchantPrefix,
	string PlayerCode,
	int ProductWallet,
	string TransactionId,
	decimal Amount);
