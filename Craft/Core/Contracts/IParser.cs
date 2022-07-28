namespace Craft.Core.Contracts
{
    public interface IParser
    {
        string ParseCommand(string fullCommand);

        IList<string> ParseParameters(string fullCommand);
    }
}