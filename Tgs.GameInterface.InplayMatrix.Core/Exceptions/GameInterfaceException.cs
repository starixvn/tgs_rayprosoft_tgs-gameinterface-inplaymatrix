namespace Tgs.GameInterface.InplayMatrix.Core.Exceptions;

public class GameInterfaceException(string message, string json) : Exception($"{message}({json})")
{
}
