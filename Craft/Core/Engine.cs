using Craft.Commands.Contracts;
using Craft.Core.Contracts;
using Craft.Core.Factories;
using Craft.Core.Providers;
using Craft.Services;

namespace Craft.Core
{
    public class Engine : IEngine
    {
        private const string TerminationCommand = "Exit";
        private const string WelcomeMessage = "Welcome to Craft!";
        private const string InvalidDataMessage = "Invalid data provided!";

        internal string EnvironmentVariablesSyntax =
            "Use the following environment variables: GITHUB_TOKEN for storing the GitHub PAT and FRESHDESK_TOKEN for storing the Freshdesk API Key.";
        internal string ProvideEnvironmentVariablesMessage = "Please, provide both your GitHub Personal Access Token and Freshdesk API Key as environment variables.";
        internal string ProvideDataMessage = "In order to create a new Freshdesk contact, please provide the GitHub username of the target user and your Freshdesk subdomain in the following format: " +
                                             "CreateFreshdeskContact [GitHub username] [Freshdesk subdomain]";

        internal string CreatedMessage = "Created a new Freshdesk contact! " + Environment.NewLine;
        internal string FailedToParseCommandParametersMessage = "Failed to parse \"{0}\" command parameters. ";

        public Engine()
        {
            this.Parser = new CommandParser();
            this.CommandFactory = new CommandFactory(this);
            this.RequestService = new RequestService();
        }
                
        public GitHubService GitHubService { get; set; }
        
        public FreshdeskService FreshdeskService { get; set; }
        
        public RequestService RequestService { get; set; }
        
        public ICommandFactory CommandFactory { get; set; }
        
        public IParser Parser { get; set; }

        public async Task StartAsync()
        {
            this.PrintInformation();
            
            while (true)
            {
                try
                {
                    string commandAsString = Console.ReadLine();
                    if (commandAsString.ToLower() == TerminationCommand.ToLower())
                    {
                        break;
                    }

                    string? message = await this.ProcessCommandAsync(commandAsString);
                    Console.WriteLine(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task<string?> ProcessCommandAsync(string commandAsString)
        {
            if (string.IsNullOrWhiteSpace(commandAsString))
            {
                var message = InvalidDataMessage + Environment.NewLine + ProvideDataMessage;
                throw new ArgumentNullException(commandAsString, message);
            }

            string commandName = this.Parser.ParseCommand(commandAsString);
            IList<string> commandParameters = this.Parser.ParseParameters(commandAsString);
            
            ICommand? command = this.CommandFactory.Create(commandName);
            string? executionResult = await command?.ExecuteAsync(commandParameters);

            return executionResult;
        }
        
        private void PrintInformation()
        {
            Console.WriteLine(WelcomeMessage); 
            Console.WriteLine(ProvideDataMessage);
            Console.WriteLine(ProvideEnvironmentVariablesMessage);
            Console.WriteLine(EnvironmentVariablesSyntax);
        }
    }
}

