# Craft

1. You need your GitHub personal access token stored in GITHUB_TOKEN environmental variable and Freshdesk API key stored in FRESHDESK_TOKEN environmental variable.
- Linux - export VARNAME="my value"
- Windows/macOS/Linux - https://www3.ntu.edu.sg/home/ehchua/programming/howto/Environment_Variables.html
- Or set directly in IDE

![](Documentation/EnvironmentVariables.png)

2. You need an `launchSettings.json` file in the `Properties` directory, in the `Craft.Tests` project in order for the unit tests to work properly
- Use the following template
```json
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "Craft.Tests": {
      "commandName": "Project",
      "environmentVariables": {
        "FRESHDESK_TOKEN": "Your Token",
        "GITHUB_TOKEN": "Your Token"
      }
    }
  }
}
```