using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

internal class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _client;

    public PlayerBalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
            .PlayerGetBalanceAsync(
                apiUrl: query.ApiUrl,
                merchantCode: query.MerchantCode,
                playerId: query.PlayerCode.ToPlayerid(query.MerchantPrefix),
                productWallet: query.ProductWallet,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceResult(
            result.IsPlayerExist,
            result.Balance);
    }
}
