namespace Tgs.GameInterface.InplayMatrix.Core.Models;

public record PlayerGetBalanceResult(
	bool IsPlayerExist,
	decimal Balance,
	string Currency,
	string WalletStatus,
	string ThresholdConfig);
