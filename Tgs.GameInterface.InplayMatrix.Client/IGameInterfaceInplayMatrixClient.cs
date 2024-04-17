using Tgs.GameInterface.InplayMatrix.Protos;

namespace Tgs.GameInterface.InplayMatrix.Client;

public interface IGameInterfaceInplayMatrixClient
{
	ValueTask<string> CheckingServiceAsync(
		string machineName,
		CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundInAsync(
		Uri apiUrl,
		string merchantCode,
		string merchantPrefix,
		string playerCode,
		int productWallet,
		string transactionId,
		decimal amount,
		CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundOutAsync(
		Uri apiUrl,
		string merchantCode,
		string merchantPrefix,
		string playerCode,
		int productWallet,
		string transactionId,
		decimal amount,
		CancellationToken cancellationToken = default);

	ValueTask<bool> FundStatusAsync(
		Uri apiUrl,
		string merchantCode,
		string merchantPrefix,
		string playerCode,
		int productWallet,
		string transactionId,
		CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameRecordByIMESportsInfo> GameRecordByIMESportsAsync(
		Uri reportUrl,
		string merchantCode,
		string merchantPrefix,
		DateTimeOffset startDate,
		DateTimeOffset endDate,
		int page,
		CancellationToken cancellationToken = default);

	ValueTask<GameUrlReply> GameUrlAsync(
		Uri apiUrl,
		string merchantCode,
		string merchantPrefix,
		string playerCode,
		int productWallet,
		string gameCode,
		string ipAddress,
		bool isMobile,
		string language,
		CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
		Uri apiUrl,
		string merchantCode,
		string merchantPrefix,
		string playerCode,
		int productWallet,
		CancellationToken cancellationToken = default);

	ValueTask PlayerCreateAsync(
		Uri apiUrl,
		string merchantCode,
		string merchantPrefix,
		string playerCode,
		string currency,
		string password,
		CancellationToken cancellationToken = default);
}
