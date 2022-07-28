using System.Text;
using Craft.Models;
using Craft.Resolvers;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Craft.Services
{
    public class FreshdeskService
    {
        public FreshdeskService(RequestService requestService, string domain, string apiKey, string apiPathSuffix)
        {
            this.RequestService = requestService;
            this.Domain = domain;
            this.ApiKey = this.FormatAuthorizationToken(apiKey);
            this.ApiPath = "https://" + domain + ".freshdesk.com" + apiPathSuffix;
        }

        public RequestService RequestService { get; init; }

        public string Domain { get; set; }

        public string ApiKey { get; set; }

        public string ApiPath { get; set; }

        public string BuildUserJson(FreshdeskUser user)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseClassResolver();
            string userJson = JsonConvert.SerializeObject(user, Formatting.Indented, settings);
            return userJson;
        }

        public FreshdeskUser CreateFreshdeskUser(GitHubUser gitHubUser, FreshdeskCompany company)
        {
            FreshdeskUser freshdeskUser = new FreshdeskUser
            {
                // avatar = avatar_url, 
                Name = gitHubUser?.Name, 
                Email = gitHubUser?.Email ?? $"none_provided_{Guid.NewGuid().ToString().Substring(0, 6)}@example.com",
                Description = gitHubUser?.Bio,
                CompanyId = company?.Id,
                Address = gitHubUser?.Location,
            };

            return freshdeskUser;
        }
        
        public string BuildCompanyJson(string companyName)
        {
            object companyJsonObject = new
            {
                name = companyName, 
            };
            
            string companyJson = JsonSerializer.Serialize(companyJsonObject);
            
            return companyJson;
        }

        public async Task<string> CreateContactAsync(FreshdeskUser user)
        {
            string freshdeskUserJson = this.BuildUserJson(user);
            
            HttpResponseMessage freshdeskResponse = await this.RequestService.CreateRequestAsync(
                this.ApiPath + "contacts", 
                "POST",
                this.ApiKey, 
                freshdeskUserJson);
            
            string result = this.RequestService.ParseResponse(freshdeskResponse);

            return result;
        }

        public async Task<FreshdeskCompany?> GetOrCreateCompanyByNameAsync(string? name)
        {
            // if the user doesn't have a company, return early
            if (name == null)
            {
                return null;
            }
            
            // get all companies
            HttpResponseMessage freshdeskCompaniesResponse = await this.RequestService.CreateRequestAsync(
                this.ApiPath + "companies/autocomplete?name=" + name, 
                "GET",
                this.ApiKey);
            
            string companiesData = this.RequestService.ParseResponse(freshdeskCompaniesResponse);

            // if the company is null (we do not have it created yet in Freshdesk), create it
            FreshdeskCompany? mappedCompany = CreateCompaniesFromData(companiesData).Companies.FirstOrDefault();
            if (mappedCompany == null)
            {
                string companyAsJson = BuildCompanyJson(name);
                HttpResponseMessage freshdeskCompanyResponse = await this.RequestService.CreateRequestAsync(
                    this.ApiPath + "companies", 
                    "POST",
                    this.ApiKey, 
                    companyAsJson);
                
                string companyData = this.RequestService.ParseResponse(freshdeskCompanyResponse);
                mappedCompany = CreateCompanyFromData(companyData);
            }
            
            return new FreshdeskCompany { Id = mappedCompany.Id, Name = mappedCompany.Name };
        }

        public FreshdeskCompanies CreateCompaniesFromData(string data)
            => JsonConvert.DeserializeObject<FreshdeskCompanies>(data);
        
        public FreshdeskCompany? CreateCompanyFromData(string data)
            =>  JsonConvert.DeserializeObject<FreshdeskCompany>(data);
        
        public string FormatAuthorizationToken(string token)
            => "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(token + ":X"));
    }
}