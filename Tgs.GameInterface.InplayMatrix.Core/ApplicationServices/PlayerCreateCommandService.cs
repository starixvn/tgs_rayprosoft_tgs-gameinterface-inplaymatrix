using Tgs.GameInterface.InplayMatrix.Core.Extensions;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.InplayMatrix.Core.ApplicationServices;

internal class PlayerCreateCommandService(
	IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
	public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
		=> client.PlayerRegisterAsync(
				apiUrl: command.ApiUrl,
				merchantCode: command.MerchantCode,
				playerId: command.PlayerCode.ToPlayerid(command.MerchantPrefix),
				currency: command.Currency,
				password: command.Password,
				cancellationToken: cancellationToken);
}
