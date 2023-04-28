namespace Tgs.GameInterface.InplayMatrix.Core;

internal class SystemClock : ISystemClock
{
    public DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;
}
