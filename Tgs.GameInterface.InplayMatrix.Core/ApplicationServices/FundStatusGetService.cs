using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

internal class FundStatusGetService : IGetService<FundStatusQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundStatusGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
        => _client
        .TransactionCheckTransferStatus(
            query.ApiUrl,
            query.MerchantCode,
            query.PlayerCode.ToPlayerid(query.MerchantPrefix),
            query.ProductWallet,
            query.TransactionId,
            cancellationToken: cancellationToken);
}
