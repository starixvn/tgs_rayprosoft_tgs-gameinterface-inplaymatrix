using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, GameUrlResult>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameUrlResult> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
    {
        var result = query.IsMobile
            ? await _client
                .GameNewLaunchMobileGameAsync(
                    apiUrl: query.ApiUrl,
                    merchantCode: query.MerchantCode,
                    playerId: query.PlayerCode.ToPlayerid(query.MerchantPrefix),
                    gameCode: query.GameCode,
                    language: query.Language,
                    ipAddress: query.IpAddress,
                    productWallet: query.ProductWallet,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false)
            : await _client
                .GameNewLaunchGameAsync(
                    apiUrl: query.ApiUrl,
                    merchantCode: query.MerchantCode,
                    playerId: query.PlayerCode.ToPlayerid(query.MerchantPrefix),
                    gameCode: query.GameCode,
                    language: query.Language,
                    ipAddress: query.IpAddress,
                    productWallet: query.ProductWallet,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        return new GameUrlResult(
            IsPlayerExist: result.IsPlayerExist,
            GameUrl: result.GameUrl);
    }
}
