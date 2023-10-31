using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest;

public class PlayerGetBalanceUnitTest
{
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
        var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
        var playerId = "IM1S3_4ucmqplxb029";
        var productWallet = 401;

        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = "IM1S3_4ucmqplxb029",
            ProductWallet = productWallet
        };

        var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

        var response = new
        {
            Code = "0",
            Message = "Successful.",
            Balance = "0.0000",
            Currency = "CNY",
            WalletStatus = "Normal",
            ThresholdConfig = "Inactive"
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/Player/GetBalance")
                .WithContent(parameters.ToJsonString())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client.PlayerGetBalanceAsync(
            apiUrl: apiUrl,
            merchantCode: merchantCode,
            playerId: playerId,
            productWallet: productWallet);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
    }

    [Fact]
    public async Task This_Player_Does_Not_Exist()
    {
        var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
        var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
        var playerId = "IM1S3_4ucmqplxb029";
        var productWallet = 401;

        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = "IM1S3_4ucmqplxb029",
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
        _ = handler.Expect(HttpMethod.Post, "/Player/GetBalance")
                .WithContent(parameters.ToJsonString())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client.PlayerGetBalanceAsync(
            apiUrl: apiUrl,
            merchantCode: merchantCode,
            playerId: playerId,
            productWallet: productWallet);

        Assert.NotNull(actual);
        Assert.False(actual.IsPlayerExist);
    }
}
