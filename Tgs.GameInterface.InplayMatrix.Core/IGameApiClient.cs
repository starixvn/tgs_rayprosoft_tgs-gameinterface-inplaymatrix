using Tgs.GameInterface.InplayMatrix.Core.Models;

namespace Tgs.GameInterface.InplayMatrix.Core;

public interface IGameApiClient
{
    public ValueTask<GameNewLaunchGameResult> GameNewLaunchGameAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        string gameCode,
        string language,
        string ipAddress,
        int productWallet,
        CancellationToken cancellationToken = default);

    public ValueTask<GameNewLaunchGameResult> GameNewLaunchMobileGameAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        string gameCode,
        string language,
        string ipAddress,
        int productWallet,
        CancellationToken cancellationToken = default);

    public ValueTask<bool> PlayerCheckExistsAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        CancellationToken cancellationToken = default);

    public ValueTask<PlayerGetBalanceResult> PlayerGetBalanceAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        int productWallet,
        CancellationToken cancellationToken = default);

    public ValueTask PlayerRegisterAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        string currency,
        string password,
        CancellationToken cancellationToken = default);

    public ValueTask PlayerResetPasswordAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        string password,
        CancellationToken cancellationToken = default);

    public IAsyncEnumerable<ReportGetBetLogByIMESportsResult> ReportGetBetLogByIMESportsAsync(
        Uri reportUrl,
        string merchantCode,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        int page = 1,
        CancellationToken cancellationToken = default);

    public ValueTask<bool> TransactionCheckTransferStatus(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        int productWallet,
        string transactionId,
        CancellationToken cancellationToken = default);

    public ValueTask<TransactionPerformTransferResult> TransactionPerformTransferAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        int productWallet,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default);
}
