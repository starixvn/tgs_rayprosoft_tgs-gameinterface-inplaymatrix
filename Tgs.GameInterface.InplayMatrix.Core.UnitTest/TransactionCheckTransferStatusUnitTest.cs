using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.InplayMatrix.Core.Exceptions;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest;

public class TransactionCheckTransferStatusUnitTest
{
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    [Fact]
    public async Task Extensions()
    {
        var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
        var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
        var playerId = "IM1S3_4ucmqplxb029";
        var productWallet = 401;
        var transactionId = "95c3e7e9116f42bab0d51dcc1ec6ce67";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.UtcNow;
        _ = fakeSystemClock
            .CurrentTime
            .Returns(currentTime);

        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            ProductWallet = productWallet,
            TransactionId = transactionId,
        };

        var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

        var response = new
        {
            Code = "515",
            Message = "Transaction id is not found for this player in this product wallet."
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/Transaction/CheckTransferStatus")
                .WithContent(parameters.ToJsonString())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeSystemClock,
            fakeHttpClient);

        _ = await Assert.ThrowsAsync<GameInterfaceException>(() => client
                .TransactionCheckTransferStatus(
                      apiUrl: apiUrl,
                      merchantCode: merchantCode,
                      playerId: playerId,
                      productWallet: productWallet,
                      transactionId: transactionId)
                .AsTask())
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Normal_Process_Deposit()
    {
        var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
        var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
        var playerId = "IM1S3_4ucmqplxb029";
        var productWallet = 401;
        var transactionId = "b1499a7472ba45f0abf5e8180527d45e";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.UtcNow;
        _ = fakeSystemClock
            .CurrentTime
            .Returns(currentTime);

        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            ProductWallet = productWallet,
            TransactionId = transactionId,
        };

        var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

        var response = new
        {
            Code = "0",
            Message = "Successful.",
            Status = "Approved"
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/Transaction/CheckTransferStatus")
                .WithContent(parameters.ToJsonString())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client
            .TransactionCheckTransferStatus(
                apiUrl: apiUrl,
                merchantCode: merchantCode,
                playerId: playerId,
                productWallet: productWallet,
                transactionId: transactionId)
            .ConfigureAwait(false);

        Assert.True(actual);
    }

    [Fact]
    public async Task Normal_Process_Withdrawal()
    {
        var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
        var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
        var playerId = "IM1S3_4ucmqplxb029";
        var productWallet = 401;
        var transactionId = "95c3e7e9116f42bab0d51dcc1ec6ce6e";

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeSystemClock = Substitute.For<ISystemClock>();

        var currentTime = DateTimeOffset.UtcNow;
        _ = fakeSystemClock
            .CurrentTime
            .Returns(currentTime);

        var request = new
        {
            MerchantCode = merchantCode,
            PlayerId = playerId,
            ProductWallet = productWallet,
            TransactionId = transactionId,
        };

        var parameters = JsonSerializer.SerializeToNode(request, _jsonOptions)!;

        var response = new
        {
            Code = "0",
            Message = "Successful.",
            Status = "Approved"
        };
        var content = JsonSerializer.Serialize(response);

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/Transaction/CheckTransferStatus")
                .WithContent(parameters.ToJsonString())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeSystemClock,
            fakeHttpClient);

        var actual = await client
            .TransactionCheckTransferStatus(
                apiUrl: apiUrl,
                merchantCode: merchantCode,
                playerId: playerId,
                productWallet: productWallet,
                transactionId: transactionId)
            .ConfigureAwait(false);

        Assert.True(actual);
    }
}
