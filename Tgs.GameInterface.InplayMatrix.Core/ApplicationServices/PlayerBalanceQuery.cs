namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string MerchantCode,
	string MerchantPrefix,
	string PlayerCode,
	int ProductWallet);
