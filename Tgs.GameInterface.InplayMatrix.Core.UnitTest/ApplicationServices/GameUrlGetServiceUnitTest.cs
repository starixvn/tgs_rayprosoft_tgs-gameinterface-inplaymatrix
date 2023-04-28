using NSubstitute;
using Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.GameInterface.InplayMatrix.Core.Models;

namespace Tgs.GameInterface.InplayMatrix.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process_Mobile_False()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameUrlQuery(
            ApiUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            PlayerCode: "4ucmqplxb029",
            ProductWallet: 401,
            GameCode: "ESPORTSBULL",
            IpAddress: "127.0.0.1",
            IsMobile: false,
            Language: "ZH-CN");

        var result = new GameNewLaunchGameResult(
            IsPlayerExist: true,
            GameUrl: "https://localhost/");
        _ = fakeGameApiClient
            .GameNewLaunchGameAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                merchantCode: Arg.Is(query.MerchantCode),
                playerId: Arg.Is(query.PlayerCode.ToPlayerid(query.MerchantPrefix)),
                gameCode: Arg.Is(query.GameCode),
                language: Arg.Is(query.Language),
                ipAddress: Arg.Is(query.IpAddress),
                productWallet: Arg.Is(query.ProductWallet),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new GameUrlGetService(
            fakeGameApiClient);

        _ = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        _ = await fakeGameApiClient
            .Received(0)
            .GameNewLaunchMobileGameAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                merchantCode: Arg.Is(query.MerchantCode),
                playerId: Arg.Is(query.PlayerCode.ToPlayerid(query.MerchantPrefix)),
                gameCode: Arg.Is(query.GameCode),
                language: Arg.Is(query.Language),
                ipAddress: Arg.Is(query.IpAddress),
                productWallet: Arg.Is(query.ProductWallet),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }

    [Fact]
    public async Task Normal_Process_Mobile_True()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameUrlQuery(
            ApiUrl: new Uri("http://operatorapi.staging.imaegisapi.com/"),
            MerchantCode: "OfL5H2hKOIP9tHp9ZIoqYeggJtFC39zQ",
            MerchantPrefix: "IM1S3",
            PlayerCode: "4ucmqplxb029",
            ProductWallet: 401,
            GameCode: "ESPORTSBULL",
            IpAddress: "127.0.0.1",
            IsMobile: true,
            Language: "ZH-CN");

        var result = new GameNewLaunchGameResult(
            IsPlayerExist: true,
            GameUrl: "https://localhost/");
        _ = fakeGameApiClient
            .GameNewLaunchMobileGameAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                merchantCode: Arg.Is(query.MerchantCode),
                playerId: Arg.Is(query.PlayerCode.ToPlayerid(query.MerchantPrefix)),
                gameCode: Arg.Is(query.GameCode),
                language: Arg.Is(query.Language),
                ipAddress: Arg.Is(query.IpAddress),
                productWallet: Arg.Is(query.ProductWallet),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new GameUrlGetService(
            fakeGameApiClient);

        _ = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        _ = await fakeGameApiClient
            .Received(0)
            .GameNewLaunchGameAsync(
                apiUrl: Arg.Is(query.ApiUrl),
                merchantCode: Arg.Is(query.MerchantCode),
                playerId: Arg.Is(query.PlayerCode.ToPlayerid(query.MerchantPrefix)),
                gameCode: Arg.Is(query.GameCode),
                language: Arg.Is(query.Language),
                ipAddress: Arg.Is(query.IpAddress),
                productWallet: Arg.Is(query.ProductWallet),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
