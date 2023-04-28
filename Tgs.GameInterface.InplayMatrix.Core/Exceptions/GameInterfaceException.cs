namespace Tgs.GameInterface.InplayMatrix.Core.Exceptions;

public class GameInterfaceException : Exception
{
    public GameInterfaceException(string message, string json) : base($"{message}({json})")
    {
    }
}
