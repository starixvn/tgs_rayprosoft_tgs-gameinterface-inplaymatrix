namespace Tgs.GameInterface.InplayMatrix.Core;

public interface ISystemClock
{
	DateTimeOffset CurrentTime { get; }
}
