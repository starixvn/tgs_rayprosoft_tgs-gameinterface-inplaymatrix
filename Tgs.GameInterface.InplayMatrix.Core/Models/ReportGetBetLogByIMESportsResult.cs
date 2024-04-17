namespace Tgs.GameInterface.InplayMatrix.Core.Models;

public record ReportGetBetLogByIMESportsResult(
	string GameID,
	string BetId,
	DateTimeOffset WagerCreationDateTime,
	DateTimeOffset LastUpdatedDate,
	string PlayerId,
	decimal StakeAmount,
	decimal WinLoss,
	string IsSettled,
	string IsResettled,
	string IsCancelled,
	DateTimeOffset? SettlementDateTime,
	string AdditionalData);
