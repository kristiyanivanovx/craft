using Craft.Core.Contracts;

namespace Craft.Core.Providers
{
    public class CommandParser  : IParser
    {
        public string ParseCommand(string fullCommand)
        {
            string commandName = fullCommand.Split(' ')[0];
            return commandName;
        }

        public IList<string> ParseParameters(string fullCommand)
        {
            List<string> commandParts = fullCommand.Split(' ').ToList();
            if (!commandParts.Any())
            {
                return new List<string>();
            }
            
            commandParts.RemoveAt(0);

            return commandParts;
        }
    }
}