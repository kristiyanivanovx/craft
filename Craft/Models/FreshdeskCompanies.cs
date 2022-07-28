using Newtonsoft.Json;

namespace Craft.Models
{
    public class FreshdeskCompanies
    {
        [JsonProperty(PropertyName = "companies")]
        public List<FreshdeskCompany> Companies { get; set; }
    }
}
