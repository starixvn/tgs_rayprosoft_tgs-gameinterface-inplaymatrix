using NSubstitute;
using Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundStatusQuery(
            ApiUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            PlayerCode: "4ucmqplxb029",
            ProductWallet: 401,
            TransactionId: Guid.NewGuid().ToString("N"));

        _ = fakeGameApiClient
             .TransactionCheckTransferStatus(
                 apiUrl: Arg.Is(query.ApiUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 playerId: Arg.Is(query.PlayerCode.ToPlayerid(query.MerchantPrefix)),
                 productWallet: Arg.Is(query.ProductWallet),
                 transactionId: Arg.Is(query.TransactionId),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(true);

        var client = new FundStatusGetService(
            fakeGameApiClient);

        var actual = await client.GetAsync(query);

        Assert.True(actual);
    }
}
