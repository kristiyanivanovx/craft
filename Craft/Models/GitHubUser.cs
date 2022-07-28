using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Craft.Models
{
    public class GitHubUser
    {
        public string? Name { get; set; }

        [JsonProperty(PropertyName = "Avatar_Url")]
        public string? AvatarUrl { get; set; }
        
        public string? Bio { get; set; }
        
        public string? Blog { get; set; }
        
        public string? Company { get; set; }

        public string? Email { get; set; }
        
        public string? Location { get; set; }
    }
}