using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

internal class PlayerBalanceGetService(
	IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client
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
