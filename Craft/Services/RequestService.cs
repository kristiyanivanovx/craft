using System.Net;
using System.Text;

namespace Craft.Services
{
    public class RequestService
    {
        public async Task<HttpResponseMessage> CreateRequestAsync(string url, string method, string authorizationHeader, string? json = null, string? userAgentHeader = null)
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
            
            if (userAgentHeader != null)
            {
                client.DefaultRequestHeaders.Add("User-Agent", userAgentHeader);
            }
            
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(method), client.BaseAddress);
            if (json != null)
            {
                request.Content = new StringContent(json, Encoding.UTF8,  "application/json");
            }
            
            HttpResponseMessage response = await client.SendAsync(request);
            return response;
        }
        
        public string ParseResponse(HttpResponseMessage response)
        {
            using HttpContent content = response.Content;
            string jsonResult = content.ReadAsStringAsync().Result;

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                string baseNotFoundMessage =
                    "The requested resource couldn't be found! Please, make sure the GitHub username and the Freshdesk domain are correct!";
                
                string message =
                    string.IsNullOrWhiteSpace(jsonResult) ?
                        baseNotFoundMessage : 
                        baseNotFoundMessage + " Message: " + jsonResult;    
                
                throw new ArgumentException(message);
            }
              
            return jsonResult;
        }
    }
}
