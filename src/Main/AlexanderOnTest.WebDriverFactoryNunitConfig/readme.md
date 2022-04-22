# Selenium.WebDriver.WebDriverFactoryNunitConfig

NUnit 3 based configuration library for [Selenium.WebDriver.NetCoreWebDriverFactory](https://www.nuget.org/packages/Selenium.WebDriver.NetCoreWebDriverFactory/)

[WebDriverSettings.cs](https://github.com/AlexanderOnTest/NetCoreWebDriverFactory/blob/v4_0_0/src/Main/AlexanderOnTest.WebDriverFactoryNunitConfig/TestSettings/WebDriverSettings.cs) provides static properties that can be used, including in NUnit 3 `[OneTimeSetup]` methods to configure the WebDriver that you wish to use.

Individual configurations are loaded from an applied runsettings file or assigned a default *EXCEPT* where overrides apply.

## Overrides
#### The complete WebDriverConfiguration 
`WebDriverSettings.WebDriverConfiguration` will return a configuration loaded from the json file at 
- "My Documents/Config_WebDriver.json" (Windows) or 
- "/Config_WebDriver.json" (Mac / Linux)

if present, otherwise returning one based on the individual configurations

#### GridUri
`WebDriverSettings.GridUri` will return a Uri generated according to the following priority

*1* loaded from the json file at
- "My Documents/Config_GridUri.json" (Windows) 
- "/Config_GridUri.json" (Mac / Linux)
 
if present

*2* the value defined in
- "My Documents/Config_WebDriver.json" (Windows) or
- "/Config_WebDriver.json" (Mac / Linux)

if present 

*3* the value defined in the runsetting `gridUri`

*4* if no value is found - the Selenium Grid 4 default Uri "http://localhost:4444"

## Example runsettings
````
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
  <TestRunParameters>
    <Parameter name="browserType" value="Chrome" />
    <Parameter name="isLocal" value="true" />
    <Parameter name="headless" value="false" />
    <Parameter name="windowSize" value="Qhd" />
    <Parameter name="languageCulture" value="es-ES" />
  </TestRunParameters>
</RunSettings>
````

## Example Config_GridUri.json
`` "http://localhost:4445/" ``

## Example Config_WebDriver.json
``
{
"Browser": "SAFARI",
"IsLocal": false,
"PlatformType": "MAC",
"WindowSize": "FHd",
"Headless": false,
"GridUri": "http://localhost:4445/",
"LanguageCulture": ""
}
``