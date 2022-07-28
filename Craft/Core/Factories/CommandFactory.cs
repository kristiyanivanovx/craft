using Craft.Commands;
using Craft.Commands.Contracts;

namespace Craft.Core.Factories
{
    public class CommandFactory : ICommandFactory
    {
        private readonly Engine engine;
        
        public CommandFactory(Engine engine)
        {
            this.engine = engine;
        }
        
        public ICommand Create(string commandName)
        {
            if (commandName.ToLower() == "CreateFreshdeskContact".ToLower())
            {
                CreateFreshdeskContactCommand command = new CreateFreshdeskContactCommand(engine);
                return command;
            }
            
            throw new ArgumentException($"Command \"{commandName}\" does not exist!");
        }
    }
}

