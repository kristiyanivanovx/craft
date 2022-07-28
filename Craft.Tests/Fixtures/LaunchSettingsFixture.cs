using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Craft.Tests.Fixtures
{
    public class LaunchSettingsFixture
    {
        public LaunchSettingsFixture()
        {
            string path = Path.Combine("Properties", "launchSettings.json");
            using StreamReader file = File.OpenText(path);
            JsonTextReader reader = new JsonTextReader(file);
            JObject? jObject = JObject.Load(reader);

            List<JProperty>? variables = jObject
                ?.GetValue("profiles")
                ?.SelectMany(profiles => profiles.Children())
                .SelectMany(profile => profile.Children<JProperty>())
                .Where(prop => prop.Name == "environmentVariables")
                .SelectMany(prop => prop.Value.Children<JProperty>())
                .ToList();

            foreach (JProperty variable in variables)
            {
                Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
            }
        }
    }  
}
