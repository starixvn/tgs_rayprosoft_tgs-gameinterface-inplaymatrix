using System.Net;
using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

internal class GameUrlGetService(
    IGameApiClient client) : IGetService<GameUrlQuery, GameUrlResult>
{
    public async ValueTask<GameUrlResult> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
    {
        var iPAddress = IPAddress.Parse(query.IpAddress);
        var result = query.IsMobile
            ? await client
                .GameNewLaunchMobileGameAsync(
                    apiUrl: query.ApiUrl,
                    merchantCode: query.MerchantCode,
                    playerId: query.PlayerCode.ToPlayerid(query.MerchantPrefix),
                    gameCode: query.GameCode,
                    language: query.Language,
                    ipAddress: iPAddress.MapToIPv4().ToString(),
                    productWallet: query.ProductWallet,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false)
            : await client
                .GameNewLaunchGameAsync(
                    apiUrl: query.ApiUrl,
                    merchantCode: query.MerchantCode,
                    playerId: query.PlayerCode.ToPlayerid(query.MerchantPrefix),
                    gameCode: query.GameCode,
                    language: query.Language,
                    ipAddress: iPAddress.MapToIPv4().ToString(),
                    productWallet: query.ProductWallet,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        return new GameUrlResult(
            IsPlayerExist: result.IsPlayerExist,
            GameUrl: result.GameUrl);
    }
}
