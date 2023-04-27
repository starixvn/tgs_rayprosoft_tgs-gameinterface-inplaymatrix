using NSubstitute;
using Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.GameInterface.InplayMatrix.Core.Models;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new PlayerBalanceQuery(
            ApiUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            PlayerCode: "4ucmqplxb029",
            ProductWallet: 401);

        var result = new PlayerGetBalanceResult(
            IsPlayerExist: true,
            Balance: 0.0m,
            Currency: "CNY",
            WalletStatus: "Normal",
            ThresholdConfig: "Inactive");
        _ = fakeGameApiClient
             .PlayerGetBalanceAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 merchantCode: Arg.Is(query.MerchantCode),
                 playerId: Arg.Is(query.PlayerCode.ToPlayerid(query.MerchantPrefix)),
                 productWallet: Arg.Is(query.ProductWallet),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(result);

        var client = new PlayerBalanceGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
        Assert.Equal(result.Balance, actual.Balance);
    }
}
