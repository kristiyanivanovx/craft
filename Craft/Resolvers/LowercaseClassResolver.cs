using Newtonsoft.Json.Serialization;

namespace Craft.Resolvers
{
    public class LowercaseClassResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}