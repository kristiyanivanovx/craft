using Craft.Commands.Contracts;
using Craft.Core;
using Craft.Models;
using Craft.Services;

namespace Craft.Commands
{
    public class CreateFreshdeskContactCommand : ICommand
    {
        private Engine engine;
        
        public CreateFreshdeskContactCommand(Engine engine)
        {
            this.engine = engine;
        }
        
        public async Task<string> ExecuteAsync(IList<string> parameters)
        {
            string gitHubUsername;
            string freshdeskSubdomain;

            try
            {
                gitHubUsername = parameters[0];
                freshdeskSubdomain = parameters[1];
            }
            catch
            {
                string message = string.Format(this.engine.FailedToParseCommandParametersMessage, "CreateFreshdeskContact") + this.engine.ProvideDataMessage;
                throw new ArgumentException(message);
            }
            
            string? gitHubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            string? freshdeskApiKey = Environment.GetEnvironmentVariable("FRESHDESK_TOKEN");

            if (gitHubToken == null || freshdeskApiKey == null)
            {
                string message = this.engine.ProvideEnvironmentVariablesMessage +  Environment.NewLine + this.engine.EnvironmentVariablesSyntax;
                throw new ArgumentException(message);
            }
            
            this.engine.GitHubService = new GitHubService(this.engine.RequestService, gitHubToken, "https://api.github.com/users/");
            GitHubUser gitHubUser = await this.engine.GitHubService.GetUserByUsernameAsync(gitHubUsername);
            
            this.engine.FreshdeskService = new FreshdeskService(this.engine.RequestService, freshdeskSubdomain, freshdeskApiKey, "/api/v2/");
            FreshdeskCompany? company = await this.engine.FreshdeskService.GetOrCreateCompanyByNameAsync(gitHubUser?.Company?.Replace("@", ""));
            FreshdeskUser freshdeskUser = this.engine.FreshdeskService.CreateFreshdeskUser(gitHubUser, company);
            string result = await this.engine.FreshdeskService.CreateContactAsync(freshdeskUser);

            return this.engine.CreatedMessage + result;
        }
    }
}

