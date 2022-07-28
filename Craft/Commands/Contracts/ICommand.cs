namespace Craft.Commands.Contracts
{
    public interface ICommand
    {
        Task<string> ExecuteAsync(IList<string> parameters);
    }
}

