using Newtonsoft.Json;

namespace Craft.Models
{
    public class FreshdeskUser
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        // public string? AvatarUrl { get; set; }
        
        [JsonProperty("description")]
        public string? Description { get; set; }
        
        [JsonProperty("company_id")]
        public long? CompanyId { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }
        
        [JsonProperty("address")]
        public string? Address { get; set; }
    }
}
