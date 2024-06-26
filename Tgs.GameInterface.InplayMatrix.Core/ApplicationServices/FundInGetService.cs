using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

internal class FundInGetService(
	IGameApiClient client) : IGetService<FundInQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client
			 .TransactionPerformTransferAsync(
				 apiUrl: query.ApiUrl,
				 merchantCode: query.MerchantCode,
				 playerId: query.PlayerCode.ToPlayerid(query.MerchantPrefix),
				 productWallet: query.ProductWallet,
				 transactionId: query.TransactionId,
				 amount: -query.Amount,
				 cancellationToken: cancellationToken)
			 .ConfigureAwait(false);

		return new FundResult(
			TransactionId: query.TransactionId.ToString(),
			Resp: result.Resp);
	}
}
