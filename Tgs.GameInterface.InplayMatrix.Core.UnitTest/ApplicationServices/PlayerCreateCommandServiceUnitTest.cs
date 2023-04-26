using NSubstitute;
using Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest.ApplicationServices;

public class PlayerCreateCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new PlayerCreateCommand(
            ApiUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            PlayerCode: "4ucmqplxb029",
            Currency: "CNY",
            Password: "1q2w3e4r");

        var client = new PlayerCreateCommandService(
            fakeGameApiClient);

        await client
            .ExecuteAsync(
                query)
            .ConfigureAwait(false);

        await fakeGameApiClient
            .PlayerRegisterAsync(
               apiUrl: Arg.Is(query.ApiUrl),
               merchantCode: Arg.Is(query.MerchantCode),
               playerId: Arg.Is(query.PlayerCode.ToPlayerid(query.MerchantPrefix)),
               currency: Arg.Is(query.Currency),
               password: Arg.Is(query.Password),
               cancellationToken: Arg.Any<CancellationToken>())
           .ConfigureAwait(false);
    }
}
