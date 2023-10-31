using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.InplayMatrix.Core.Exceptions;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest;

public class PlayerRegisterUnitTest
{
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    [Fact]
    public async Task Invalid_Password()
    {
        var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
        var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
        var playerId = "IM1S3_4ucmqplxb029";
        var currency = "CNY";
        var password = "TgsP@ssw0rd";

        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            Currency = currency,
            Password = password,
        };

        var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

        var response = new
        {
            Code = "524",
            Message = "Invalid Password."
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/Player/Register")
                .WithContent(parameters.ToJsonString())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeSystemClock,
            fakeHttpClient);

        _ = await Assert.ThrowsAsync<GameInterfaceException>(
            () => client.PlayerRegisterAsync(
                 apiUrl: apiUrl,
                 merchantCode: merchantCode,
                 playerId: playerId,
                 currency: currency,
                 password: password)
            .AsTask());
    }

    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
        var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
        var playerId = "IM1S3_4ucmqplxb029";
        var currency = "CNY";
        var password = "1q2w3e4r";

        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            Currency = currency,
            Password = password,
        };

        var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

        var response = new
        {
            Code = "0",
            Message = "Successful.",
            Currency = "CNY"
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/Player/Register")
                .WithContent(parameters.ToJsonString())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeSystemClock,
            fakeHttpClient);

        await client.PlayerRegisterAsync(
            apiUrl: apiUrl,
            merchantCode: merchantCode,
            playerId: playerId,
            currency: currency,
            password: password);
    }

    [Fact]
    public async Task Player_already_exists()
    {
        var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
        var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
        var playerId = "IM1S3_4ucmqplxb029";
        var currency = "CNY";
        var password = "1q2w3e4r";

        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            Currency = currency,
            Password = password,
        };

        var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

        var response = new
        {
            Code = "503",
            Message = "Player already exists.",
            Currency = "-"
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/Player/Register")
                .WithContent(parameters.ToJsonString())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeSystemClock = Substitute.For<ISystemClock>();
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeSystemClock,
            fakeHttpClient);

        await client.PlayerRegisterAsync(
            apiUrl: apiUrl,
            merchantCode: merchantCode,
            playerId: playerId,
            currency: currency,
            password: password);
    }
}
