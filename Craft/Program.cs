using Craft.Core;

namespace Craft
{
    public class Program
    {
        public static async Task Main()
        {
            Engine engine = new Engine();
            await engine.StartAsync();
        }
    }
}