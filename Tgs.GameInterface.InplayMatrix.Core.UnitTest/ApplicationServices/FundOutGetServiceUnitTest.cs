using NSubstitute;
using Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.GameInterface.InplayMatrix.Core.Models;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundOutQuery(
            ApiUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            PlayerCode: "4ucmqplxb029",
            ProductWallet: 401,
            TransactionId: Guid.NewGuid().ToString("N"),
            Amount: 1m);

        var result = new TransactionPerformTransferResult(
            TransactionId: query.TransactionId,
            Res: "{\"Code\":\"0\",\"Message\":\"Successful.\",\"Status\":\"Approved\"}");
        _ = fakeGameApiClient
             .TransactionPerformTransferAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 playerId: Arg.Is(query.PlayerCode.ToPlayerid(query.MerchantPrefix)),
                 productWallet: Arg.Is(query.ProductWallet),
                 transactionId: Arg.Is(query.TransactionId),
                 amount: Arg.Is(query.Amount),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(result);

        var client = new FundOutGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(query.TransactionId.ToString(), actual.TransactionId);
    }
}
