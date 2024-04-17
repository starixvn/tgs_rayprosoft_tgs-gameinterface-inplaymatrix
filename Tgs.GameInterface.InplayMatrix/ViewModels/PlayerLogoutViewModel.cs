namespace Tgs.GameInterface.InplayMatrix.ViewModels;

public record PlayerLogoutViewModel(
	Uri ApiUrl,
	string AgentCode,
	string AgentKey,
	string Currency,
	string PlayerCode);
