using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

internal class PlayerCreateCommandService : ICommandService<PlayerCreateCommand>
{
    private readonly IGameApiClient _client;

    public PlayerCreateCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => _client.PlayerRegisterAsync(
                apiUrl: command.ApiUrl,
                merchantCode: command.MerchantCode,
                playerId: command.PlayerCode.ToPlayerid(command.MerchantPrefix),
                currency: command.Currency,
                password: command.Password,
                cancellationToken: cancellationToken);
}
