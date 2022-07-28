using System.Text;
using Craft.Models;
using Newtonsoft.Json;

namespace Craft.Services
{
    public class GitHubService
    {
        public GitHubService(RequestService requestService, string personalAccessToken, string apiPath)
        {
            this.RequestService = requestService;
            this.PersonalAccessToken = this.FormatAuthorizationToken(personalAccessToken);
            this.ApiPath = apiPath;
        }

        public RequestService RequestService { get; init; }

        public string PersonalAccessToken { get; set; }

        public string ApiPath { get; set; }

        public GitHubUser CreateUserFromData(string data)
            => JsonConvert.DeserializeObject<GitHubUser>(data);
    
        public async Task<GitHubUser> GetUserByUsernameAsync(string username)
        {
            HttpResponseMessage userResponse = await this.RequestService.CreateRequestAsync(
                this.ApiPath + username,
                "GET",
                this.PersonalAccessToken,
                null,
                "Awesome-Octocat-App");

            string userDataParsed = this.RequestService.ParseResponse(userResponse);
         
            return this.CreateUserFromData(userDataParsed);
        }

        public string FormatAuthorizationToken(string token)
            => "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(token + ":X"));
    }
}
