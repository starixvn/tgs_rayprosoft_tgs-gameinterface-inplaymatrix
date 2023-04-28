using System.Runtime.CompilerServices;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.GameInterface.InplayMatrix.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

internal class GameRecordByIMESportsQueryService : IQueryService<GameRecordByIMESportsQuery, GameRecordByIMESportsResult>
{
    private readonly IGameApiClient _client;

    public GameRecordByIMESportsQueryService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameRecordByIMESportsResult> QueryAsync(
        GameRecordByIMESportsQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var results = await _client
            .ReportGetBetLogByIMESportsAsync(
                reportUrl: query.ReportUrl,
                merchantCode: query.MerchantCode,
                startDate: query.StartDate,
                endDate: query.EndDate,
                page: query.Page,
                cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<ReportGetBetLogByIMESportsResult>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            var isSettled = result.IsSettled.Equals("1", StringComparison.OrdinalIgnoreCase);
            var isCancelled = result.IsCancelled.Equals("1", StringComparison.OrdinalIgnoreCase);
            yield return new GameRecordByIMESportsResult(
                ExternalRoundId: result.BetId,
                ExternalGameId: result.GameID,
                PlayerCode: result.PlayerId.ToPlayerCode(query.MerchantPrefix),
                IsSettled: isSettled,
                IsCancelled: isCancelled,
                IsResettled: result.IsResettled.Equals("1", StringComparison.OrdinalIgnoreCase),
                ExternalCreatedOn: result.WagerCreationDateTime,
                ExternalClosedOn: result.SettlementDateTime,
                Bet: result.StakeAmount,
                Settle: isSettled && !isCancelled ? result.StakeAmount + result.WinLoss : 0,
                WinLoss: isSettled ? result.WinLoss : 0,
                AdditionalData: result.AdditionalData);
        }
    }
}
