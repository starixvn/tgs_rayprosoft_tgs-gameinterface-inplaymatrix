using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest;

public class GameNewLaunchMobileGameUnitTest
{
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
        var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
        var playerId = "IM1S3_4ucmqplxb029";
        var gameCode = "ESPORTSBULL";
        var language = "ZH-CN";
        var ipAddress = "127.0.0.1";
        var productWallet = 401;

        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            GameCode = gameCode,
            Language = language,
            IpAddress = ipAddress,
            ProductWallet = productWallet
        };

        var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

        var response = new
        {
            Code = "0",
            Message = "Successful.",
            GameUrl = "https://imes-4zl7f-staging.roshan88.com/mobile.aspx?id=3441&token=e8c2866c-2f3a-43f1-8e90-26d2b5ccfb50&LanguageCode=1"
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/Game/NewLaunchMobileGame")
                .WithContent(parameters.ToJsonString())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client
            .GameNewLaunchMobileGameAsync(
                apiUrl: apiUrl,
                merchantCode: merchantCode,
                playerId: playerId,
                gameCode: gameCode,
                language: language,
                ipAddress: ipAddress,
                productWallet: productWallet)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
        Assert.Equal(response.GameUrl, actual.GameUrl);
    }

    [Fact]
    public async Task This_Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
        var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
        var playerId = "IM1S3_4ucmqplxb029";
        var gameCode = "ESPORTSBULL";
        var language = "ZH-CN";
        var ipAddress = "127.0.0.1";
        var productWallet = 401;

        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            GameCode = gameCode,
            Language = language,
            IpAddress = ipAddress,
            ProductWallet = productWallet
        };

        var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

        var response = new
        {
            Code = "504",
            Message = "Player does not exist."
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/Game/NewLaunchMobileGame")
                .WithContent(parameters.ToJsonString())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client
            .GameNewLaunchMobileGameAsync(
                apiUrl: apiUrl,
                merchantCode: merchantCode,
                playerId: playerId,
                gameCode: gameCode,
                language: language,
                ipAddress: ipAddress,
                productWallet: productWallet)
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.False(actual.IsPlayerExist);
    }
}
