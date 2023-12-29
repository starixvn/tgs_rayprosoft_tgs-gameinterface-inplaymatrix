using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;
using Tgs.GameInterface.InplayMatrix.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Controllers;

[Route("im")]
[ApiController]
public class InplayMatrixController : ControllerBase
{
    /// <summary>
    /// Funds the in asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://operatorapi.staging.imaegisapi.com/",
    ///             "merchantCode": "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
    ///             "merchantPrefix": "IM1S3",
    ///             "playerCode": "4ucmqplxb029",
    ///             "productWallet": "401",
    ///             "transactionId":"8E6C596F-EF95-4796-E52C-ED8E3ACE1E66",
    ///             "amount": 1
    ///         }
    /// </remarks>
    [HttpPost("fund/in", Name = "FundIn")]
    public async ValueTask<FundResultViewModel> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundInQuery(
            ApiUrl: source.ApiUrl,
            MerchantCode: source.MerchantCode,
            MerchantPrefix: source.MerchantPrefix,
            PlayerCode: source.PlayerCode,
            ProductWallet: source.ProductWallet,
            TransactionId: source.TransactionId,
            Amount: source.Amount);

        var result = await service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new FundResultViewModel(
            TransactionId: result.TransactionId,
            Res: result.Res);
    }

    /// <summary>
    /// Funds the out asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://operatorapi.staging.imaegisapi.com/",
    ///             "merchantCode": "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
    ///             "merchantPrefix": "IM1S3",
    ///             "playerCode": "4ucmqplxb029",
    ///             "productWallet": "401",
    ///             "transactionId":"0B228B07-2F0C-D074-7871-64C55030B29F",
    ///             "amount": 1
    ///         }
    /// </remarks>
    [HttpPost("fund/out", Name = "FundOut")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
        [FromServices] IGetService<FundOutQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundOutQuery(
            ApiUrl: source.ApiUrl,
            MerchantCode: source.MerchantCode,
            MerchantPrefix: source.MerchantPrefix,
            PlayerCode: source.PlayerCode,
            ProductWallet: source.ProductWallet,
            TransactionId: source.TransactionId,
            Amount: source.Amount);

        var result = await service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new FundResultViewModel(
            TransactionId: result.TransactionId,
            Res: result.Res);
    }

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://operatorapi.staging.imaegisapi.com/",
    ///             "merchantCode": "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
    ///             "merchantPrefix": "IM1S3",
    ///             "playerCode": "4ucmqplxb029",
    ///             "productWallet": "401",
    ///             "transactionId":"0B228B07-2F0C-D074-7871-64C55030B29F"
    ///         }
    /// </remarks>
    [HttpPost("fund/Status", Name = "FundStatus")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
            ApiUrl: source.ApiUrl,
            MerchantCode: source.MerchantCode,
            MerchantPrefix: source.MerchantPrefix,
            PlayerCode: source.PlayerCode,
            ProductWallet: source.ProductWallet,
            TransactionId: source.TransactionId);

        return service.GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Games the record asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "reportUrl": "http://operatorapi-report.staging.imaegisapi.com/",
    ///             "merchantCode": "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
    ///             "merchantPrefix": "IM1S3",
    ///             "startDate": "2023-04-27T00:00:00+08:00",
    ///             "endDate": "2023-04-28T00:00:00+08:00",
    ///             "page": 1
    ///         }
    /// </remarks>
    [HttpPost("game/record/im-eSports")]
    public async IAsyncEnumerable<GameRecordByIMESportsResultViewModel> GameRecordAsync(
        [FromServices] IQueryService<GameRecordByIMESportsQuery, GameRecordByIMESportsResult> service,
        GameRecordByIMESportsViewModel source)
    {
        var query = new GameRecordByIMESportsQuery(
            ReportUrl: source.ReportUrl,
            MerchantCode: source.MerchantCode,
            MerchantPrefix: source.MerchantPrefix,
            StartDate: source.StartDate,
            EndDate: source.EndDate,
            Page: source.Page);

        var results = await service
            .QueryAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ToArrayAsync(
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordByIMESportsResult>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new GameRecordByIMESportsResultViewModel(
                ExternalRoundId: result.ExternalRoundId,
                ExternalGameId: result.ExternalGameId,
                PlayerCode: result.PlayerCode,
                IsSettled: result.IsSettled,
                IsCancelled: result.IsCancelled,
                IsResettled: result.IsResettled,
                ExternalCreatedOn: result.ExternalCreatedOn,
                ExternalClosedOn: result.ExternalClosedOn,
                Bet: result.Bet,
                Settle: result.Settle,
                WinLoss: result.WinLoss,
                AdditionalData: result.AdditionalData);
        }
    }

    /// <summary>
    /// Games the URL asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://operatorapi.staging.imaegisapi.com/",
    ///             "merchantCode": "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
    ///             "merchantPrefix": "IM1S3",
    ///             "playerCode": "4ucmqplxb029",
    ///             "gameCode": "ESPORTSBULL",
    ///             "ipAddress": "127.0.0.1",
    ///             "isMobile": "False"
    ///             "language": "ZH-CN"
    ///         }
    /// </remarks>
    [HttpPost("gameurl")]
    public async ValueTask<GameUrlResultViewModel> GameUrlAsync(
        [FromServices] IGetService<GameUrlQuery, GameUrlResult> service,
        GameUrlQueryViewModel source)
    {
        var query = new GameUrlQuery(
            ApiUrl: source.ApiUrl,
            MerchantCode: source.MerchantCode,
            MerchantPrefix: source.MerchantPrefix,
            PlayerCode: source.PlayerCode,
            ProductWallet: source.ProductWallet,
            GameCode: source.GameCode,
            IpAddress: source.IpAddress,
            IsMobile: source.IsMobile,
            Language: source.Language);

        var result = await service
            .GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new GameUrlResultViewModel(
            IsPlayerExist: result.IsPlayerExist,
            GameUrl: result.GameUrl);
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://operatorapi.staging.imaegisapi.com/",
    ///             "merchantCode": "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
    ///             "merchantPrefix": "IM1S3",
    ///             "playerCode": "4ucmqplxb029",
    ///             "productWallet": 401
    ///         }
    /// </remarks>
    [HttpPost("player/balance", Name = "PlayerBalance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: source.ApiUrl,
            MerchantCode: source.MerchantCode,
            MerchantPrefix: source.MerchantPrefix,
            PlayerCode: source.PlayerCode,
            ProductWallet: source.ProductWallet);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance);
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "http://operatorapi.staging.imaegisapi.com/",
    ///             "merchantCode": "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
    ///             "merchantPrefix": "IM1S3",
    ///             "playerCode": "4ucmqplxb029",
    ///             "currency": "CNY",
    ///             "password":"1q2w3e4r"
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: source.ApiUrl,
            MerchantCode: source.MerchantCode,
            MerchantPrefix: source.MerchantPrefix,
            PlayerCode: source.PlayerCode,
            Currency: source.Currency,
            Password: source.Password);

        return service.ExecuteAsync(
                command: command,
                cancellationToken: HttpContext.RequestAborted);
    }
}
