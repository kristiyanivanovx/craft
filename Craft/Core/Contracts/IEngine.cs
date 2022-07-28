namespace Craft.Core.Contracts
{
    public interface IEngine
    {
        Task StartAsync();

        Task<string?> ProcessCommandAsync(string commandAsString);
    }
}