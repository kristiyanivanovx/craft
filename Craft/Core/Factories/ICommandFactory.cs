using Craft.Commands.Contracts;

namespace Craft.Core.Factories
{
    public interface ICommandFactory
    {
        ICommand Create(string commandName);
    }
}