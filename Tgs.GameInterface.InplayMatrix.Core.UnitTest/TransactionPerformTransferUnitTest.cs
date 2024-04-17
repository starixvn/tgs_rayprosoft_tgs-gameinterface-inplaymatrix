using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest;

public class TransactionPerformTransferUnitTest
{
	private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

	[Fact]
	public async Task Normal_Process_Deposit()
	{
		var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
		var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
		var playerId = "IM1S3_4ucmqplxb029";
		var productWallet = 401;
		var transactionId = Guid.NewGuid().ToString("N");
		var amount = 10;

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
			Amount = amount,
			RequestTime = currentTime.DatetimeConvertToString()
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
		_ = handler.Expect(HttpMethod.Post, "/Transaction/PerformTransfer")
				.WithContent(parameters.ToJsonString())
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.TransactionPerformTransferAsync(
			apiUrl: apiUrl,
			merchantCode: merchantCode,
			playerId: playerId,
			productWallet: productWallet,
			transactionId: transactionId,
			amount: amount);

		Assert.NotNull(actual);
		Assert.Equal(transactionId, actual.TransactionId);
	}

	[Fact]
	public async Task Normal_Process_Withdrawal()
	{
		var apiUrl = new Uri("http://operatorapi.staging.imaegisapi.com/");
		var merchantCode = "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ";
		var playerId = "IM1S3_4ucmqplxb029";
		var productWallet = 401;
		var transactionId = Guid.NewGuid().ToString("N");
		var amount = -10;

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
			Amount = amount,
			RequestTime = currentTime.DatetimeConvertToString()
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
		_ = handler.Expect(HttpMethod.Post, "/Transaction/PerformTransfer")
				.WithContent(parameters.ToJsonString())
				.Respond(HttpStatusCode.OK, new StringContent(content));

		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeSystemClock,
			fakeHttpClient);

		var actual = await client.TransactionPerformTransferAsync(
			apiUrl: apiUrl,
			merchantCode: merchantCode,
			playerId: playerId,
			productWallet: productWallet,
			transactionId: transactionId,
			amount: amount);

		Assert.NotNull(actual);
		Assert.Equal(transactionId, actual.TransactionId);
	}
}
