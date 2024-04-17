namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

public record PlayerCreateCommand(
	Uri ApiUrl,
	string MerchantCode,
	string MerchantPrefix,
	string PlayerCode,
	string Currency,
	string Password);
