using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

internal class FundStatusGetService(
	IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
	public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
		=> client
		.TransactionCheckTransferStatus(
			query.ApiUrl,
			query.MerchantCode,
			query.PlayerCode.ToPlayerid(query.MerchantPrefix),
			query.ProductWallet,
			query.TransactionId,
			cancellationToken: cancellationToken);
}
