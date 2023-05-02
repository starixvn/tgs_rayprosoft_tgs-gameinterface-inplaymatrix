using NSubstitute;
using Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.GameInterface.InplayMatrix.Core.Models;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest.ApplicationServices;

public class GameRecordByIMESportsQueryServiceUnitTest
{
    [Fact]
    public async Task Normal_Process_Cancelled()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordByIMESportsQuery(
            ReportUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            StartDate: DateTimeOffset.Parse("2023-04-27T15:56:00+08:00"),
            EndDate: DateTimeOffset.Parse("2023-04-27T16:00:00+08:00"),
            Page: 1);

        var results = new ReportGetBetLogByIMESportsResult[]{
            new ReportGetBetLogByIMESportsResult(
                GameID: "ESPORTSBULL",
                BetId: "432368343",
                WagerCreationDateTime: "2023-04-27 15:54:50 +08:00".StringConvertToDatetime(),
                LastUpdatedDate: "2023-04-27 15:59:47 +08:00".StringConvertToDatetime(),
                PlayerId: "IM1S3_4ucmqplxb029",
                StakeAmount: 5m,
                WinLoss: 0m,
                IsSettled: "1",
                IsResettled: "0",
                IsCancelled: "1",
                SettlementDateTime:  "2023-04-27 15:59:47 +08:00".StringConvertToDatetime(),
                AdditionalData: "{\"Provider\":\"ESPORTSBULL_ESPORTS\",\"GameId\":\"ESPORTSBULL\",\"BetId\":\"432368343\",\"WagerCreationDateTime\":\"2023-04-27 15:54:50 +08:00\",\"LastUpdatedDate\":\"2023-04-27 15:59:47 +08:00\",\"PlayerId\":\"IM1S3_4ucmqplxb029\",\"ProviderPlayerId\":\"IM1S3IM1S3_4ucmqplxb029\",\"Currency\":\"CNY\",\"StakeAmount\":\"5.000000\",\"WinLoss\":\"0.000000\",\"OddsType\":\"EURO\",\"TotalOdds\":\"13.952\",\"WagerType\":\"Single\",\"Platform\":\"Web\",\"IsSettled\":\"1\",\"IsResettled\":\"0\",\"IsCancelled\":\"1\",\"SettlementDateTime\":\"2023-04-27 15:59:47 +08:00\",\"Tolerance\":\"0\",\"FlagCategory\":\"\",\"DetailItems\":[{\"EventName\":\"eSuba VS Team GO\",\"EventDateTime\":\"2023-04-29 00:30:00 +08:00\",\"CompetitionName\":\"EMEA Masters Spring 2023\",\"HomeTeamName\":\"eSuba\",\"AwayTeamName\":\"Team GO\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":45,\"SportsName\":\"LEAGUE OF LEGENDS\",\"BetType\":\"Game 1 Win (Live Odds)\",\"EventId\":\"12929913\",\"EventType\":\"BO7\",\"GameOrder\":\"1\",\"Selection\":\"eSuba\",\"Odds\":\"13.952\",\"IsSettled\":\"1\",\"IsResettled\":\"0\",\"IsWagerItemCancelled\":\"1\",\"WagerItemCancelType\":\"2\",\"ResultStatus\":\"\",\"BaseTierID\":\"1\",\"BaseTierCode\":\"1\",\"BaseTierName\":\"Tier1\"}]}"
                )
        };

        _ = fakeGameApiClient
             .ReportGetBetLogByIMESportsAsync(
                 reportUrl: Arg.Is(query.ReportUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 page: Arg.Is(query.Page),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordByIMESportsQueryService(
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
                query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordByIMESportsResult>();

        Assert.True(actual.Any());
        Assert.Equal("432368343", actual.Single().ExternalRoundId);
        Assert.Equal("ESPORTSBULL", actual.Single().ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.Single().PlayerCode);
        Assert.True(actual.Single().IsSettled);
        Assert.True(actual.Single().IsCancelled);
        Assert.False(actual.Single().IsResettled);
        Assert.Equal(DateTimeOffset.Parse("2023-04-27T15:59:47+08:00"), actual.Single().ExternalClosedOn);
        Assert.Equal(5m, actual.Single().Bet);
        Assert.Equal(0m, actual.Single().Settle);
        Assert.Equal(0m, actual.Single().WinLoss);
    }

    [Fact]
    public async Task Normal_Process_ParlayAll()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordByIMESportsQuery(
            ReportUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            StartDate: DateTimeOffset.Parse("2023-04-27T16:33:00+08:00"),
            EndDate: DateTimeOffset.Parse("2023-04-27T16:43:00+08:00"),
            Page: 1);

        var results = new ReportGetBetLogByIMESportsResult[]{
            new ReportGetBetLogByIMESportsResult(
                GameID: "ESPORTSBULL",
                BetId: "432368345",
                WagerCreationDateTime: "2023-04-27 16:33:07 +08:00".StringConvertToDatetime(),
                LastUpdatedDate: "2023-04-27 16:33:07 +08:00".StringConvertToDatetime(),
                PlayerId: "IM1S3_4ucmqplxb029",
                StakeAmount: 5m,
                WinLoss: 0m,
                IsSettled: "0",
                IsResettled: "0",
                IsCancelled: "0",
                SettlementDateTime: null,
                AdditionalData: "{\"Provider\":\"ESPORTSBULL_ESPORTS\",\"GameId\":\"ESPORTSBULL\",\"BetId\":\"432368345\",\"WagerCreationDateTime\":\"2023-04-27 16:33:07 +08:00\",\"LastUpdatedDate\":\"2023-04-27 16:33:07 +08:00\",\"PlayerId\":\"IM1S3_4ucmqplxb029\",\"ProviderPlayerId\":\"IM1S3IM1S3_4ucmqplxb029\",\"Currency\":\"CNY\",\"StakeAmount\":\"5.000000\",\"WinLoss\":\"0.000000\",\"OddsType\":\"EURO\",\"TotalOdds\":\"34.24\",\"WagerType\":\"ParlayAll\",\"Platform\":\"Web\",\"IsSettled\":\"0\",\"IsResettled\":\"0\",\"IsCancelled\":\"0\",\"SettlementDateTime\":\"\",\"Tolerance\":\"255\",\"FlagCategory\":\"\",\"DetailItems\":[{\"ParlaySign\":\"\",\"EventName\":\"eSuba VS Team GO\",\"EventDateTime\":\"2023-04-29 00:00:00 +08:00\",\"CompetitionName\":\"EMEA Masters Spring 2023\",\"ParlayWagerCreationDateTime\":\"2023-04-27 16:33:07 +08:00\",\"HomeTeamName\":\"eSuba\",\"AwayTeamName\":\"Team GO\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":45,\"SportsName\":\"LEAGUE OF LEGENDS\",\"BetType\":\"BO7 Series Win\",\"EventId\":\"12929906\",\"EventType\":\"BO7\",\"GameOrder\":\"0\",\"Selection\":\"eSuba\",\"Odds\":\"20.0\",\"IsSettled\":\"0\",\"IsResettled\":\"0\",\"IsWagerItemCancelled\":\"0\",\"WagerItemCancelType\":\"0\",\"ResultStatus\":\"\",\"BaseTierID\":\"1\",\"BaseTierCode\":\"1\",\"BaseTierName\":\"Tier1\"},{\"ParlaySign\":\"\",\"EventName\":\"Astralis Talent VS Illuminar Gaming\",\"EventDateTime\":\"2023-04-27 17:00:00 +08:00\",\"CompetitionName\":\"CCT Central Europe Series #6\",\"ParlayWagerCreationDateTime\":\"2023-04-27 16:33:07 +08:00\",\"HomeTeamName\":\"Astralis Talent\",\"AwayTeamName\":\"Illuminar Gaming\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":47,\"SportsName\":\"CS:GO\",\"BetType\":\"Game 3 Win\",\"EventId\":\"12957194\",\"EventType\":\"BO3\",\"GameOrder\":\"3\",\"Selection\":\"Astralis Talent\",\"Odds\":\"1.712\",\"IsSettled\":\"0\",\"IsResettled\":\"0\",\"IsWagerItemCancelled\":\"0\",\"WagerItemCancelType\":\"0\",\"ResultStatus\":\"\",\"BaseTierID\":\"0\",\"BaseTierCode\":\"0\",\"BaseTierName\":\"Other\"}]}"
                )
        };

        _ = fakeGameApiClient
             .ReportGetBetLogByIMESportsAsync(
                 reportUrl: Arg.Is(query.ReportUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 page: Arg.Is(query.Page),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordByIMESportsQueryService(
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
                query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordByIMESportsResult>();

        Assert.True(actual.Any());
        Assert.Equal("432368345", actual.Single().ExternalRoundId);
        Assert.Equal("ESPORTSBULL", actual.Single().ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.Single().PlayerCode);
        Assert.False(actual.Single().IsSettled);
        Assert.False(actual.Single().IsCancelled);
        Assert.False(actual.Single().IsResettled);
        Assert.Null(actual.Single().ExternalClosedOn);
    }

    [Fact]
    public async Task Normal_Process_ParlayAll_12929906_Settlement_Win()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordByIMESportsQuery(
            ReportUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            StartDate: DateTimeOffset.Parse("2023-04-27T16:33:00+08:00"),
            EndDate: DateTimeOffset.Parse("2023-04-27T16:43:00+08:00"),
            Page: 1);

        var results = new ReportGetBetLogByIMESportsResult[]{
            new ReportGetBetLogByIMESportsResult(
                GameID: "ESPORTSBULL",
                BetId: "432368345",
                WagerCreationDateTime: "2023-04-27 16:33:07 +08:00".StringConvertToDatetime(),
                LastUpdatedDate: "2023-04-27 17:02:03 +08:00".StringConvertToDatetime(),
                PlayerId: "IM1S3_4ucmqplxb029",
                StakeAmount: 5m,
                WinLoss: 0m,
                IsSettled: "0",
                IsResettled: "0",
                IsCancelled: "0",
                SettlementDateTime: null,
                AdditionalData: "{\"Provider\":\"ESPORTSBULL_ESPORTS\",\"GameId\":\"ESPORTSBULL\",\"BetId\":\"432368345\",\"WagerCreationDateTime\":\"2023-04-27 16:33:07 +08:00\",\"LastUpdatedDate\":\"2023-04-27 17:02:03 +08:00\",\"PlayerId\":\"IM1S3_4ucmqplxb029\",\"ProviderPlayerId\":\"IM1S3IM1S3_4ucmqplxb029\",\"Currency\":\"CNY\",\"StakeAmount\":\"5.000000\",\"WinLoss\":\"0.000000\",\"OddsType\":\"EURO\",\"TotalOdds\":\"34.24\",\"WagerType\":\"ParlayAll\",\"Platform\":\"Web\",\"IsSettled\":\"0\",\"IsResettled\":\"0\",\"IsCancelled\":\"0\",\"SettlementDateTime\":\"\",\"Tolerance\":\"255\",\"FlagCategory\":\"\",\"DetailItems\":[{\"ParlaySign\":\"\",\"EventName\":\"eSuba VS Team GO\",\"EventDateTime\":\"2023-04-29 00:00:00 +08:00\",\"CompetitionName\":\"EMEA Masters Spring 2023\",\"ParlayWagerCreationDateTime\":\"2023-04-27 16:33:07 +08:00\",\"HomeTeamName\":\"eSuba\",\"AwayTeamName\":\"Team GO\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"FTScore\":[{\"Option\":\"eSuba\",\"Outcome\":1},{\"Option\":\"Team GO\",\"Outcome\":0}],\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":45,\"SportsName\":\"LEAGUE OF LEGENDS\",\"BetType\":\"BO7 Series Win\",\"EventId\":\"12929906\",\"EventType\":\"BO7\",\"GameOrder\":\"0\",\"Selection\":\"eSuba\",\"Odds\":\"20.0\",\"IsSettled\":\"1\",\"IsResettled\":\"0\",\"IsWagerItemCancelled\":\"0\",\"WagerItemCancelType\":\"0\",\"ResultStatus\":\"1\",\"BaseTierID\":\"1\",\"BaseTierCode\":\"1\",\"BaseTierName\":\"Tier1\"},{\"ParlaySign\":\"\",\"EventName\":\"Astralis Talent VS Illuminar Gaming\",\"EventDateTime\":\"2023-04-27 17:00:00 +08:00\",\"CompetitionName\":\"CCT Central Europe Series #6\",\"ParlayWagerCreationDateTime\":\"2023-04-27 16:33:07 +08:00\",\"HomeTeamName\":\"Astralis Talent\",\"AwayTeamName\":\"Illuminar Gaming\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":47,\"SportsName\":\"CS:GO\",\"BetType\":\"Game 3 Win\",\"EventId\":\"12957194\",\"EventType\":\"BO3\",\"GameOrder\":\"3\",\"Selection\":\"Astralis Talent\",\"Odds\":\"1.712\",\"IsSettled\":\"0\",\"IsResettled\":\"0\",\"IsWagerItemCancelled\":\"0\",\"WagerItemCancelType\":\"0\",\"ResultStatus\":\"\",\"BaseTierID\":\"0\",\"BaseTierCode\":\"0\",\"BaseTierName\":\"Other\"}]}"
                )
        };

        _ = fakeGameApiClient
             .ReportGetBetLogByIMESportsAsync(
                 reportUrl: Arg.Is(query.ReportUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 page: Arg.Is(query.Page),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordByIMESportsQueryService(
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
                query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordByIMESportsResult>();

        Assert.True(actual.Any());
        Assert.Equal("432368345", actual.Single().ExternalRoundId);
        Assert.Equal("ESPORTSBULL", actual.Single().ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.Single().PlayerCode);
        Assert.False(actual.Single().IsSettled);
        Assert.False(actual.Single().IsCancelled);
        Assert.False(actual.Single().IsResettled);
        Assert.Null(actual.Single().ExternalClosedOn);
    }

    [Fact]
    public async Task Normal_Process_ParlayAll_Settlement_Win()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordByIMESportsQuery(
            ReportUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            StartDate: DateTimeOffset.Parse("2023-04-27T16:33:00+08:00"),
            EndDate: DateTimeOffset.Parse("2023-04-27T16:43:00+08:00"),
            Page: 1);

        var results = new ReportGetBetLogByIMESportsResult[]{
            new ReportGetBetLogByIMESportsResult(
                GameID: "ESPORTSBULL",
                BetId: "432368345",
                WagerCreationDateTime: "2023-04-27 16:33:07 +08:00".StringConvertToDatetime(),
                LastUpdatedDate: "2023-04-27 17:29:08 +08:00".StringConvertToDatetime(),
                PlayerId: "IM1S3_4ucmqplxb029",
                StakeAmount: 5m,
                WinLoss: 166.199997m,
                IsSettled: "1",
                IsResettled: "0",
                IsCancelled: "0",
                SettlementDateTime:  "2023-04-27 17:29:08 +08:00".StringConvertToDatetime(),
                AdditionalData: "{\"Provider\":\"ESPORTSBULL_ESPORTS\",\"GameId\":\"ESPORTSBULL\",\"BetId\":\"432368345\",\"WagerCreationDateTime\":\"2023-04-27 16:33:07 +08:00\",\"LastUpdatedDate\":\"2023-04-27 17:29:08 +08:00\",\"PlayerId\":\"IM1S3_4ucmqplxb029\",\"ProviderPlayerId\":\"IM1S3IM1S3_4ucmqplxb029\",\"Currency\":\"CNY\",\"StakeAmount\":\"5.000000\",\"WinLoss\":\"166.199997\",\"OddsType\":\"EURO\",\"TotalOdds\":\"34.24\",\"WagerType\":\"ParlayAll\",\"Platform\":\"Web\",\"IsSettled\":\"1\",\"IsResettled\":\"0\",\"IsCancelled\":\"0\",\"SettlementDateTime\":\"2023-04-27 17:29:08 +08:00\",\"Tolerance\":\"255\",\"FlagCategory\":\"\",\"DetailItems\":[{\"ParlaySign\":\"\",\"EventName\":\"eSuba VS Team GO\",\"EventDateTime\":\"2023-04-29 00:00:00 +08:00\",\"CompetitionName\":\"EMEA Masters Spring 2023\",\"ParlayWagerCreationDateTime\":\"2023-04-27 16:33:07 +08:00\",\"HomeTeamName\":\"eSuba\",\"AwayTeamName\":\"Team GO\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"FTScore\":[{\"Option\":\"eSuba\",\"Outcome\":1},{\"Option\":\"Team GO\",\"Outcome\":0}],\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":45,\"SportsName\":\"LEAGUE OF LEGENDS\",\"BetType\":\"BO7 Series Win\",\"EventId\":\"12929906\",\"EventType\":\"BO7\",\"GameOrder\":\"0\",\"Selection\":\"eSuba\",\"Odds\":\"20.0\",\"IsSettled\":\"1\",\"IsResettled\":\"0\",\"IsWagerItemCancelled\":\"0\",\"WagerItemCancelType\":\"0\",\"ResultStatus\":\"1\",\"BaseTierID\":\"1\",\"BaseTierCode\":\"1\",\"BaseTierName\":\"Tier1\"},{\"ParlaySign\":\"\",\"EventName\":\"Astralis Talent VS Illuminar Gaming\",\"EventDateTime\":\"2023-04-27 17:00:00 +08:00\",\"CompetitionName\":\"CCT Central Europe Series #6\",\"ParlayWagerCreationDateTime\":\"2023-04-27 16:33:07 +08:00\",\"HomeTeamName\":\"Astralis Talent\",\"AwayTeamName\":\"Illuminar Gaming\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"FTScore\":[{\"Option\":\"Astralis Talent\",\"Outcome\":1},{\"Option\":\"Illuminar Gaming\",\"Outcome\":0}],\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":47,\"SportsName\":\"CS:GO\",\"BetType\":\"Game 3 Win\",\"EventId\":\"12957194\",\"EventType\":\"BO3\",\"GameOrder\":\"3\",\"Selection\":\"Astralis Talent\",\"Odds\":\"1.712\",\"IsSettled\":\"1\",\"IsResettled\":\"0\",\"IsWagerItemCancelled\":\"0\",\"WagerItemCancelType\":\"0\",\"ResultStatus\":\"1\",\"BaseTierID\":\"0\",\"BaseTierCode\":\"0\",\"BaseTierName\":\"Other\"}]}"
                )
        };

        _ = fakeGameApiClient
             .ReportGetBetLogByIMESportsAsync(
                 reportUrl: Arg.Is(query.ReportUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 page: Arg.Is(query.Page),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordByIMESportsQueryService(
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
                query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordByIMESportsResult>();

        Assert.True(actual.Any());
        Assert.Equal("432368345", actual.Single().ExternalRoundId);
        Assert.Equal("ESPORTSBULL", actual.Single().ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.Single().PlayerCode);
        Assert.True(actual.Single().IsSettled);
        Assert.False(actual.Single().IsCancelled);
        Assert.False(actual.Single().IsResettled);
        Assert.Equal(DateTimeOffset.Parse("2023-04-27T17:29:08+08:00"), actual.Single().ExternalClosedOn);
        Assert.Equal(5m, actual.Single().Bet);
        Assert.Equal(171.199997m, actual.Single().Settle);
        Assert.Equal(166.199997m, actual.Single().WinLoss);
    }

    [Fact]
    public async Task Normal_Process_Resettled()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordByIMESportsQuery(
            ReportUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            StartDate: DateTimeOffset.Parse("2023-04-27T15:56:00+08:00"),
            EndDate: DateTimeOffset.Parse("2023-04-27T16:00:00+08:00"),
            Page: 1);

        var results = new ReportGetBetLogByIMESportsResult[]{
            new ReportGetBetLogByIMESportsResult(
                GameID: "ESPORTSBULL",
                BetId: "432368343",
                WagerCreationDateTime: "2023-04-27 15:54:50 +08:00".StringConvertToDatetime(),
                LastUpdatedDate: "2023-04-27 16:19:37 +08:00".StringConvertToDatetime(),
                PlayerId: "IM1S3_4ucmqplxb029",
                StakeAmount: 5m,
                WinLoss: 64.760002m,
                IsSettled: "1",
                IsResettled: "1",
                IsCancelled: "0",
                SettlementDateTime: "2023-04-27 16:19:37 +08:00".StringConvertToDatetime(),
                AdditionalData: "{\"Provider\":\"ESPORTSBULL_ESPORTS\",\"GameId\":\"ESPORTSBULL\",\"BetId\":\"432368343\",\"WagerCreationDateTime\":\"2023-04-27 15:54:50 +08:00\",\"LastUpdatedDate\":\"2023-04-27 16:19:37 +08:00\",\"PlayerId\":\"IM1S3_4ucmqplxb029\",\"ProviderPlayerId\":\"IM1S3IM1S3_4ucmqplxb029\",\"Currency\":\"CNY\",\"StakeAmount\":\"5.000000\",\"WinLoss\":\"64.760002\",\"OddsType\":\"EURO\",\"TotalOdds\":\"13.952\",\"WagerType\":\"Single\",\"Platform\":\"Web\",\"IsSettled\":\"1\",\"IsResettled\":\"1\",\"IsCancelled\":\"0\",\"SettlementDateTime\":\"2023-04-27 16:19:37 +08:00\",\"Tolerance\":\"0\",\"FlagCategory\":\"\",\"DetailItems\":[{\"EventName\":\"eSuba VS Team GO\",\"EventDateTime\":\"2023-04-29 00:30:00 +08:00\",\"CompetitionName\":\"EMEA Masters Spring 2023\",\"HomeTeamName\":\"eSuba\",\"AwayTeamName\":\"Team GO\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"FTScore\":[{\"Option\":\"eSuba\",\"Outcome\":1},{\"Option\":\"Team GO\",\"Outcome\":0}],\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":45,\"SportsName\":\"LEAGUE OF LEGENDS\",\"BetType\":\"Game 1 Win (Live Odds)\",\"EventId\":\"12929913\",\"EventType\":\"BO7\",\"GameOrder\":\"1\",\"Selection\":\"eSuba\",\"Odds\":\"13.952\",\"IsSettled\":\"1\",\"IsResettled\":\"1\",\"IsWagerItemCancelled\":\"0\",\"WagerItemCancelType\":\"0\",\"ResultStatus\":\"1\",\"BaseTierID\":\"1\",\"BaseTierCode\":\"1\",\"BaseTierName\":\"Tier1\"}]}"
                )
        };

        _ = fakeGameApiClient
             .ReportGetBetLogByIMESportsAsync(
                 reportUrl: Arg.Is(query.ReportUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 page: Arg.Is(query.Page),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordByIMESportsQueryService(
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
                query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordByIMESportsResult>();

        Assert.True(actual.Any());
        Assert.Equal("432368343", actual.Single().ExternalRoundId);
        Assert.Equal("ESPORTSBULL", actual.Single().ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.Single().PlayerCode);
        Assert.True(actual.Single().IsSettled);
        Assert.False(actual.Single().IsCancelled);
        Assert.True(actual.Single().IsResettled);
        Assert.Equal(DateTimeOffset.Parse("2023-04-27T16:19:37+08:00"), actual.Single().ExternalClosedOn);
        Assert.Equal(5m, actual.Single().Bet);
        Assert.Equal(69.760002m, actual.Single().Settle);
        Assert.Equal(64.760002m, actual.Single().WinLoss);
    }

    [Fact]
    public async Task Normal_Process_Settlement_Losee()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordByIMESportsQuery(
            ReportUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            StartDate: DateTimeOffset.Parse("2023-04-27T14:12:00+08:00"),
            EndDate: DateTimeOffset.Parse("2023-04-27T14:13:00+08:00"),
            Page: 1);

        var results = new ReportGetBetLogByIMESportsResult[]{
            new ReportGetBetLogByIMESportsResult(
                GameID: "ESPORTSBULL",
                BetId: "432368314",
                WagerCreationDateTime: "2023-04-27 10:17:30 +08:00".StringConvertToDatetime(),
                LastUpdatedDate: "2023-04-27 10:17:30 +08:00".StringConvertToDatetime(),
                PlayerId: "IM1S3_4ucmqplxb029",
                StakeAmount: 5m,
                WinLoss: -5m,
                IsSettled: "1",
                IsResettled: "0",
                IsCancelled: "0",
                SettlementDateTime: "2023-04-27 14:12:34 +08:00".StringConvertToDatetime(),
                AdditionalData: "{\"Provider\":\"ESPORTSBULL_ESPORTS\",\"GameId\":\"ESPORTSBULL\",\"BetId\":\"432368314\",\"WagerCreationDateTime\":\"2023-04-27 10:17:30 +08:00\",\"LastUpdatedDate\":\"2023-04-27 14:12:34 +08:00\",\"PlayerId\":\"IM1S3_4ucmqplxb029\",\"ProviderPlayerId\":\"IM1S3IM1S3_4ucmqplxb029\",\"Currency\":\"CNY\",\"StakeAmount\":\"5.000000\",\"WinLoss\":\"-5.000000\",\"OddsType\":\"EURO\",\"TotalOdds\":\"1.093\",\"WagerType\":\"Single\",\"Platform\":\"Web\",\"IsSettled\":\"1\",\"IsResettled\":\"0\",\"IsCancelled\":\"0\",\"SettlementDateTime\":\"2023-04-27 14:12:34 +08:00\",\"Tolerance\":\"255\",\"FlagCategory\":\"\",\"DetailItems\":[{\"EventName\":\"herO VS KeeN\",\"EventDateTime\":\"2023-04-27 17:30:00 +08:00\",\"CompetitionName\":\"Global StarCraft II League Season 1\",\"HomeTeamName\":\"herO\",\"AwayTeamName\":\"KeeN\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"FTScore\":[{\"Option\":\"herO\",\"Outcome\":0},{\"Option\":\"KeeN\",\"Outcome\":3}],\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":53,\"SportsName\":\"STARCRAFT2\",\"BetType\":\"BO3 Series Win\",\"EventId\":\"12933529\",\"EventType\":\"BO3\",\"GameOrder\":\"0\",\"Selection\":\"herO\",\"Odds\":\"1.093\",\"IsSettled\":\"1\",\"IsResettled\":\"0\",\"IsWagerItemCancelled\":\"0\",\"WagerItemCancelType\":\"0\",\"ResultStatus\":\"2\",\"BaseTierID\":\"0\",\"BaseTierCode\":\"0\",\"BaseTierName\":\"Other\"}]}"
                )
        };

        _ = fakeGameApiClient
             .ReportGetBetLogByIMESportsAsync(
                 reportUrl: Arg.Is(query.ReportUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 page: Arg.Is(query.Page),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordByIMESportsQueryService(
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
                query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordByIMESportsResult>();

        Assert.True(actual.Any());
        Assert.Equal("432368314", actual.Single().ExternalRoundId);
        Assert.Equal("ESPORTSBULL", actual.Single().ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.Single().PlayerCode);
        Assert.True(actual.Single().IsSettled);
        Assert.False(actual.Single().IsCancelled);
        Assert.False(actual.Single().IsResettled);
        Assert.Equal(DateTimeOffset.Parse("2023-04-27T14:12:34+08:00"), actual.Single().ExternalClosedOn);
        Assert.Equal(5m, actual.Single().Bet);
        Assert.Equal(0m, actual.Single().Settle);
        Assert.Equal(-5m, actual.Single().WinLoss);
    }

    [Fact]
    public async Task Normal_Process_Settlement_Win()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordByIMESportsQuery(
            ReportUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            StartDate: DateTimeOffset.Parse("2023-04-27T14:12:00+08:00"),
            EndDate: DateTimeOffset.Parse("2023-04-27T14:13:00+08:00"),
            Page: 1);

        var results = new ReportGetBetLogByIMESportsResult[]{
            new ReportGetBetLogByIMESportsResult(
                GameID: "ESPORTSBULL",
                BetId: "432368324",
                WagerCreationDateTime: "2023-04-27 13:40:17 +08:00".StringConvertToDatetime(),
                LastUpdatedDate: "2023-04-27 14:08:13 +08:00".StringConvertToDatetime(),
                PlayerId: "IM1S3_4ucmqplxb029",
                StakeAmount: 5m,
                WinLoss: 3.005000m,
                IsSettled: "1",
                IsResettled: "0",
                IsCancelled: "0",
                SettlementDateTime: "2023-04-27 14:08:13 +08:00".StringConvertToDatetime(),
                AdditionalData: "{\"Provider\":\"ESPORTSBULL_ESPORTS\",\"GameId\":\"ESPORTSBULL\",\"BetId\":\"432368324\",\"WagerCreationDateTime\":\"2023-04-27 13:40:17 +08:00\",\"LastUpdatedDate\":\"2023-04-27 14:08:13 +08:00\",\"PlayerId\":\"IM1S3_4ucmqplxb029\",\"ProviderPlayerId\":\"IM1S3IM1S3_4ucmqplxb029\",\"Currency\":\"CNY\",\"StakeAmount\":\"5.000000\",\"WinLoss\":\"3.005000\",\"OddsType\":\"EURO\",\"TotalOdds\":\"1.601\",\"WagerType\":\"Single\",\"Platform\":\"Web\",\"IsSettled\":\"1\",\"IsResettled\":\"0\",\"IsCancelled\":\"0\",\"SettlementDateTime\":\"2023-04-27 14:08:13 +08:00\",\"Tolerance\":\"255\",\"FlagCategory\":\"\",\"DetailItems\":[{\"EventName\":\"Team WE Academy VS Joy Dream\",\"EventDateTime\":\"2023-04-27 14:00:00 +08:00\",\"CompetitionName\":\"LDL 2023 Split 2\",\"HomeTeamName\":\"Team WE Academy\",\"AwayTeamName\":\"Joy Dream\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"FTScore\":[{\"Option\":\"Team WE Academy\",\"Outcome\":3},{\"Option\":\"Joy Dream\",\"Outcome\":0}],\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":45,\"SportsName\":\"LEAGUE OF LEGENDS\",\"BetType\":\"BO3 Series Win\",\"EventId\":\"12954756\",\"EventType\":\"BO3\",\"GameOrder\":\"0\",\"Selection\":\"Team WE Academy\",\"Odds\":\"1.601\",\"IsSettled\":\"1\",\"IsResettled\":\"0\",\"IsWagerItemCancelled\":\"0\",\"WagerItemCancelType\":\"0\",\"ResultStatus\":\"1\",\"BaseTierID\":\"2\",\"BaseTierCode\":\"2\",\"BaseTierName\":\"Tier2\"}]}"
                )
        };

        _ = fakeGameApiClient
             .ReportGetBetLogByIMESportsAsync(
                 reportUrl: Arg.Is(query.ReportUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 page: Arg.Is(query.Page),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordByIMESportsQueryService(
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
                query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordByIMESportsResult>();

        Assert.True(actual.Any());
        Assert.Equal("432368324", actual.Single().ExternalRoundId);
        Assert.Equal("ESPORTSBULL", actual.Single().ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.Single().PlayerCode);
        Assert.True(actual.Single().IsSettled);
        Assert.False(actual.Single().IsCancelled);
        Assert.False(actual.Single().IsResettled);
        Assert.Equal(DateTimeOffset.Parse("2023-04-27T14:08:13+08:00"), actual.Single().ExternalClosedOn);
        Assert.Equal(5m, actual.Single().Bet);
        Assert.Equal(8.005m, actual.Single().Settle);
        Assert.Equal(3.005000m, actual.Single().WinLoss);
    }

    [Fact]
    public async Task Normal_Process_Unsettlement()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordByIMESportsQuery(
            ReportUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            StartDate: DateTimeOffset.Parse("2023-04-27T10:17:00+08:00"),
            EndDate: DateTimeOffset.Parse("2023-04-27T10:27:00+08:00"),
            Page: 1);

        var results = new ReportGetBetLogByIMESportsResult[]{
            new ReportGetBetLogByIMESportsResult(
                GameID: "ESPORTSBULL",
                BetId: "432368314",
                WagerCreationDateTime: "2023-04-27 10:17:30 +08:00".StringConvertToDatetime(),
                LastUpdatedDate: "2023-04-27 10:17:30 +08:00".StringConvertToDatetime(),
                PlayerId: "IM1S3_4ucmqplxb029",
                StakeAmount: 5m,
                WinLoss: 0m,
                IsSettled: "0",
                IsResettled: "0",
                IsCancelled: "0",
                SettlementDateTime: null,
                AdditionalData: "{\"Provider\":\"ESPORTSBULL_ESPORTS\",\"GameId\":\"ESPORTSBULL\",\"BetId\":\"432368314\",\"WagerCreationDateTime\":\"2023-04-27 10:17:30 +08:00\",\"LastUpdatedDate\":\"2023-04-27 10:17:30 +08:00\",\"PlayerId\":\"IM1S3_4ucmqplxb029\",\"ProviderPlayerId\":\"IM1S3IM1S3_4ucmqplxb029\",\"Currency\":\"CNY\",\"StakeAmount\":\"5.000000\",\"WinLoss\":\"0.000000\",\"OddsType\":\"EURO\",\"TotalOdds\":\"1.093\",\"WagerType\":\"Single\",\"Platform\":\"Web\",\"IsSettled\":\"0\",\"IsResettled\":\"0\",\"IsCancelled\":\"0\",\"SettlementDateTime\":\"\",\"Tolerance\":\"255\",\"FlagCategory\":\"\",\"DetailItems\":[{\"EventName\":\"herO VS KeeN\",\"EventDateTime\":\"2023-04-27 17:30:00 +08:00\",\"CompetitionName\":\"Global StarCraft II League Season 1\",\"HomeTeamName\":\"herO\",\"AwayTeamName\":\"KeeN\",\"FavTeam\":\"\",\"Period\":\"\",\"HomeTeamHTScore\":\"0\",\"AwayTeamHTScore\":\"0\",\"HomeTeamFTScore\":\"0\",\"AwayTeamFTScore\":\"0\",\"WagerHomeTeamScore\":\"0\",\"WagerAwayTeamScore\":\"0\",\"MarketType\":\"\",\"Handicap\":\"0.00\",\"CancelReason\":\"\",\"SportsID\":53,\"SportsName\":\"STARCRAFT2\",\"BetType\":\"BO3 Series Win\",\"EventId\":\"12933529\",\"EventType\":\"BO3\",\"GameOrder\":\"0\",\"Selection\":\"herO\",\"Odds\":\"1.093\",\"IsSettled\":\"0\",\"IsResettled\":\"0\",\"IsWagerItemCancelled\":\"0\",\"WagerItemCancelType\":\"0\",\"ResultStatus\":\"\",\"BaseTierID\":\"0\",\"BaseTierCode\":\"0\",\"BaseTierName\":\"Other\"}]}"
                )
        };

        _ = fakeGameApiClient
             .ReportGetBetLogByIMESportsAsync(
                 reportUrl: Arg.Is(query.ReportUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 startDate: Arg.Is(query.StartDate),
                 endDate: Arg.Is(query.EndDate),
                 page: Arg.Is(query.Page),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(results.ToAsyncEnumerable());

        var client = new GameRecordByIMESportsQueryService(
            fakeGameApiClient);

        var actual = await client
            .QueryAsync(
                query: query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordByIMESportsResult>();

        Assert.True(actual.Any());
        Assert.Equal("432368314", actual.Single().ExternalRoundId);
        Assert.Equal("ESPORTSBULL", actual.Single().ExternalGameId);
        Assert.Equal("4ucmqplxb029", actual.Single().PlayerCode);
        Assert.False(actual.Single().IsSettled);
        Assert.False(actual.Single().IsCancelled);
        Assert.False(actual.Single().IsResettled);
        Assert.Null(actual.Single().ExternalClosedOn);
    }
}
