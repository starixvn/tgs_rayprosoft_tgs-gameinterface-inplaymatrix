using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.InplayMatrix.Core.Exceptions;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.GameInterface.InplayMatrix.Core.Models;

namespace Tgs.GameInterface.InplayMatrix.Core;

internal class GameApiClient : IGameApiClient
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
    private readonly ILogger<GameApiClient> _logger;
    private readonly ISystemClock _systemClock;

    public GameApiClient(
        ILogger<GameApiClient> logger,
        ISystemClock systemClock,
        HttpClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameNewLaunchGameResult> GameNewLaunchGameAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        string gameCode,
        string language,
        string ipAddress,
        int productWallet,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            GameCode = gameCode,
            Language = language,
            IpAddress = ipAddress,
            ProductWallet = productWallet
        };

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "Game/NewLaunchGame",
                parameters: JsonSerializer.SerializeToNode(request, _jsonOptions)!,
                logPrefix: "NewLaunchGame",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !jsonNode["Code"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase)
            ? !jsonNode["Code"]!.GetValue<string>().Equals("504", StringComparison.OrdinalIgnoreCase)
                ? throw new GameInterfaceException(jsonNode["Message"]!.GetValue<string>(), jsonNode.ToJsonString(_jsonOptions))
                : new GameNewLaunchGameResult(
                    IsPlayerExist: false,
                    GameUrl: string.Empty)
            : new GameNewLaunchGameResult(
                IsPlayerExist: true,
                GameUrl: jsonNode["GameUrl"]!.GetValue<string>());
    }

    public async ValueTask<GameNewLaunchGameResult> GameNewLaunchMobileGameAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        string gameCode,
        string language,
        string ipAddress,
        int productWallet,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            GameCode = gameCode,
            Language = language,
            IpAddress = ipAddress,
            ProductWallet = productWallet
        };

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "Game/NewLaunchMobileGame",
                parameters: JsonSerializer.SerializeToNode(request, _jsonOptions)!,
                logPrefix: "NewLaunchMobileGame",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !jsonNode["Code"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase)
            ? !jsonNode["Code"]!.GetValue<string>().Equals("504", StringComparison.OrdinalIgnoreCase)
                ? throw new GameInterfaceException(jsonNode["Message"]!.GetValue<string>(), jsonNode.ToJsonString(_jsonOptions))
                : new GameNewLaunchGameResult(
                    IsPlayerExist: false,
                    GameUrl: string.Empty)
            : new GameNewLaunchGameResult(
                IsPlayerExist: true,
                GameUrl: jsonNode["GameUrl"]!.GetValue<string>());
    }

    public ValueTask<bool> PlayerCheckExistsAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public async ValueTask<PlayerGetBalanceResult> PlayerGetBalanceAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        int productWallet,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            ProductWallet = productWallet,
        };

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "Player/GetBalance",
                parameters: JsonSerializer.SerializeToNode(request, _jsonOptions)!,
                logPrefix: "GetBalance",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !jsonNode["Code"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase)
            ? !jsonNode["Code"]!.GetValue<string>().Equals("504", StringComparison.OrdinalIgnoreCase)
                ? throw new GameInterfaceException(jsonNode["Message"]!.GetValue<string>(), jsonNode.ToJsonString(_jsonOptions))
                : new PlayerGetBalanceResult(
                    IsPlayerExist: false,
                    Balance: 0,
                    Currency: string.Empty,
                    WalletStatus: string.Empty,
                    ThresholdConfig: string.Empty)
            : new PlayerGetBalanceResult(
            IsPlayerExist: true,
            Balance: decimal.Parse(jsonNode["Balance"]!.GetValue<string>()),
            Currency: jsonNode["Currency"]!.GetValue<string>(),
            WalletStatus: jsonNode["WalletStatus"]!.GetValue<string>(),
            ThresholdConfig: jsonNode["ThresholdConfig"]!.GetValue<string>());
    }

    public async ValueTask PlayerRegisterAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        string currency,
        string password,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            Currency = currency,
            Password = password,
        };

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "Player/Register",
                parameters: JsonSerializer.SerializeToNode(request, _jsonOptions)!,
                logPrefix: "Register",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!jsonNode["Code"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase) &&
            !jsonNode["Code"]!.GetValue<string>().Equals("503", StringComparison.OrdinalIgnoreCase))
        {
            throw new GameInterfaceException(jsonNode["Message"]!.GetValue<string>(), jsonNode.ToJsonString(_jsonOptions));
        }
    }

    public async ValueTask PlayerResetPasswordAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        string password,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            Password = password,
        };

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "Player/ResetPassword",
                parameters: JsonSerializer.SerializeToNode(request, _jsonOptions)!,
                logPrefix: "ResetPassword",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!jsonNode["Code"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase))
        {
            throw new GameInterfaceException(jsonNode["Message"]!.GetValue<string>(), jsonNode.ToJsonString(_jsonOptions));
        }
    }

    public ValueTask<ReportGetBetLogResult> ReportGetBetLogAsync(
        Uri apiUrl,
        string merchantCode,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        int Page,
        int productWallet,
        DateFilterType dateFilterType,
        BetStatus betStatus,
        CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async ValueTask<bool> TransactionCheckTransferStatus(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        int productWallet,
        string transactionId,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            ProductWallet = productWallet,
            TransactionId = transactionId,
        };

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "Transaction/CheckTransferStatus",
                parameters: JsonSerializer.SerializeToNode(request, _jsonOptions)!,
                logPrefix: "PerformTransfer",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !jsonNode["Code"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase)
            ? throw new GameInterfaceException(jsonNode["Message"]!.GetValue<string>(), jsonNode.ToJsonString(_jsonOptions))
            : true;
    }

    public async ValueTask<TransactionPerformTransferResult> TransactionPerformTransferAsync(
        Uri apiUrl,
        string merchantCode,
        string playerId,
        int productWallet,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            ProductWallet = productWallet,
            TransactionId = transactionId,
            Amount = amount,
            RequestTime = _systemClock.CurrentTime.DatetimeConvertToString()
        };

        var jsonNode = await PostAsync(
                apiUrl: apiUrl,
                path: "Transaction/PerformTransfer",
                parameters: JsonSerializer.SerializeToNode(request, _jsonOptions)!,
                logPrefix: "PerformTransfer",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !jsonNode["Code"]!.GetValue<string>().Equals("0", StringComparison.OrdinalIgnoreCase)
            ? throw new GameInterfaceException(jsonNode["Message"]!.GetValue<string>(), jsonNode.ToJsonString(_jsonOptions))
            : new TransactionPerformTransferResult(
                TransactionId: transactionId,
                Res: jsonNode.ToJsonString(_jsonOptions));
    }

    private async ValueTask<JsonNode> PostAsync(
        Uri apiUrl,
        string path,
        JsonNode parameters,
        string logPrefix,
        CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();
        var requestUri = new Uri(apiUrl, path);
        var content = new StringContent(parameters.ToJsonString(), Encoding.UTF8, "application/json");
        var httpResponseMessage = await _client
            .PostAsync(
                requestUri: requestUri,
                content: content,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };
        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }
}
