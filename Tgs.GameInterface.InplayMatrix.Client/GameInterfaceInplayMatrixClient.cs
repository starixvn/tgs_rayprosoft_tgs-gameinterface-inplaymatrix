using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.InplayMatrix.Protos;

namespace Tgs.GameInterface.InplayMatrix.Client;

internal class GameInterfaceInplayMatrixClient : IGameInterfaceInplayMatrixClient
{
    private readonly GameInterfaceInplayMatrix.GameInterfaceInplayMatrixClient _client;

    public GameInterfaceInplayMatrixClient(GameInterfaceInplayMatrix.GameInterfaceInplayMatrixClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string merchantCode,
        string merchantPrefix,
        string playerCode,
        int productWallet,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default)
        => await _client
        .FundInAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                MerchantCode = merchantCode,
                MerchantPrefix = merchantPrefix,
                PlayerCode = playerCode,
                ProductWallet = productWallet,
                TransactionId = transactionId,
                Amount = amount
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string merchantCode,
        string merchantPrefix,
        string playerCode,
        int productWallet,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default)
        => await _client
        .FundOutAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                MerchantCode = merchantCode,
                MerchantPrefix = merchantPrefix,
                PlayerCode = playerCode,
                ProductWallet = productWallet,
                TransactionId = transactionId,
                Amount = amount
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string merchantCode,
        string merchantPrefix,
        string playerCode,
        int productWallet,
        string transactionId,
        CancellationToken cancellationToken = default)
    {
        var reply = await _client
        .FundStatusAsync(
            request: new FundStatusRequest
            {
                ApiUrl = apiUrl.ToString(),
                MerchantCode = merchantCode,
                MerchantPrefix = merchantPrefix,
                PlayerCode = playerCode,
                ProductWallet = productWallet,
                TransactionId = transactionId
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

        return reply.State;
    }

    public async IAsyncEnumerable<GameRecordByIMESportsInfo> GameRecordByIMESportsAsync(
        Uri reportUrl,
        string merchantCode,
        string merchantPrefix,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        int page,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameRecordByIMESportsRequest
        {
            ReportUrl = reportUrl.ToString(),
            MerchantCode = merchantCode,
            MerchantPrefix = merchantPrefix,
            StartDate = startDate.ToTimestamp(),
            EndDate = endDate.ToTimestamp(),
            Page = page
        };

        var reply = await _client
            .GameRecordByIMESportsAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameRecords = reply.Items
            ?? Enumerable.Empty<GameRecordByIMESportsInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return gameRecord;
        }
    }

    public async ValueTask<GameUrlReply> GameUrlAsync(
        Uri apiUrl,
        string merchantCode,
        string merchantPrefix,
        string playerCode,
        int productWallet,
        string gameCode,
        string ipAddress,
        bool isMobile,
        string language,
        CancellationToken cancellationToken = default)
        => await _client
        .GameUrlAsync(
            request: new GameUrlRequest
            {
                ApiUrl = apiUrl.ToString(),
                MerchantCode = merchantCode,
                MerchantPrefix = merchantPrefix,
                PlayerCode = playerCode,
                ProductWallet = productWallet,
                GameCode = gameCode,
                IpAddress = ipAddress,
                IsMobile = isMobile,
                Language = language,
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string merchantCode,
        string merchantPrefix,
        string playerCode,
        int productWallet,
        CancellationToken cancellationToken = default)
        => await _client
        .PlayerBalanceAsync(
            request: new PlayerBalanceRequest
            {
                ApiUrl = apiUrl.ToString(),
                MerchantCode = merchantCode,
                MerchantPrefix = merchantPrefix,
                PlayerCode = playerCode,
                ProductWallet = productWallet
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string merchantCode,
        string merchantPrefix,
        string playerCode,
        string currency,
        string password,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .PlayerCreateAsync(
            request: new PlayerCreateRequest
            {
                ApiUrl = apiUrl.ToString(),
                MerchantCode = merchantCode,
                MerchantPrefix = merchantPrefix,
                PlayerCode = playerCode,
                Currency = currency,
                Password = password
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
