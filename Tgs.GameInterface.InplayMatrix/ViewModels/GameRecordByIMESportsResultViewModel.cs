namespace Tgs.GameInterface.InplayMatrix.ViewModels;

public record GameRecordByIMESportsResultViewModel(
	string ExternalRoundId,
	string ExternalGameId,
	string PlayerCode,
	bool IsSettled,
	bool IsCancelled,
	bool IsResettled,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset? ExternalClosedOn,
	decimal Bet,
	decimal Settle,
	decimal WinLoss,
	string AdditionalData);
