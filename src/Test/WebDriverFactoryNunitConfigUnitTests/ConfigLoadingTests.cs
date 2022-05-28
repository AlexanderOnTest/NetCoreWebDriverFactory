using System.IO;
using System.Threading.Tasks;
using AlexanderOnTest.NetCoreWebDriverFactory.Config;
using AlexanderOnTest.WebDriverFactoryNunitConfig.TestSettings;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using static System.Environment;

namespace WebDriverFactoryNunitConfigUnitTests;

[Category("CI")]
public class ConfigLoadingTests
{
    private const string GridUriConfigFileName = "Config_GridUri.json";
    private const string SampleGridUriConfig = "\"http://from.Config.Grid.Uri:31767\"";
    private const string InvalidGridUriConfig = "\"http://\"";
    private const string WebDriverConfigFileName = "Config_WebDriver.json";
    private const string SampleWebDriverConfig =
        "{\"Browser\":\"CHROME\",\"IsLocal\":false,\"PlatformType\":\"LINUX\",\"WindowSize\":\"FHd\",\"Headless\":true,\"GridUri\":\"http://from.webdriver.config:30884\",\"LanguageCulture\":\"\"}";
    private string _previousGridUriConfigFile;
    private string _previousWebDriverConfigFile;

    [OneTimeSetUp]
    public async Task GetCurrentConfigs()
    {
        _previousGridUriConfigFile = await ReadConfig(GridUriConfigFileName);
        _previousWebDriverConfigFile = await ReadConfig(WebDriverConfigFileName);
    }
    
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public async Task GridUriReadsFromGridUriConfigWithNoWebDriverConfig()
    {
        await WriteConfig(WebDriverConfigFileName, null);
        await WriteConfig(GridUriConfigFileName, SampleGridUriConfig);

        WebDriverSettings.GridUri.Should().Be("http://from.config.grid.uri:31767/");
    }

    [Test]
    public async Task GridUriReadsFromWebDriverConfigIfNoGridUriConfig()
    {
        await WriteConfig(WebDriverConfigFileName, SampleWebDriverConfig);
        await WriteConfig(GridUriConfigFileName, null);

        WebDriverSettings.GridUri.Should().Be("http://from.webdriver.config:30884/");
    }
    
    [Test]
    public async Task GridUriReadsFromGridUriConfigInsteadOfWebDriverConfig()
    {
        await WriteConfig(WebDriverConfigFileName, SampleWebDriverConfig);
        await WriteConfig(GridUriConfigFileName, SampleGridUriConfig);

        WebDriverSettings.GridUri.Should().Be("http://from.config.grid.uri:31767/");
    }
    
    
    [Test]
    public async Task GridUriReturnsLocalhostIfNoConfigs()
    {
        await WriteConfig(WebDriverConfigFileName, null);
        await WriteConfig(GridUriConfigFileName, null);

        WebDriverSettings.GridUri.Should().Be("http://localhost:4444");
    }

    [Test]
    public async Task OverriddenWebDriverConfigReturnsCorrectGridUriIfNoGridUriConfig()
    {
        await WriteConfig(WebDriverConfigFileName, SampleWebDriverConfig);
        await WriteConfig(GridUriConfigFileName, null);

        WebDriverSettings.OverriddenWebDriverConfiguration.GridUri.Should().Be("http://from.webdriver.config:30884/");
    }

    [Test]
    public async Task OverriddenWebDriverConfigReturnsGridUriValueFromGridUriConfigIfPresent()
    {
        await WriteConfig(WebDriverConfigFileName, SampleWebDriverConfig);
        await WriteConfig(GridUriConfigFileName, SampleGridUriConfig);

        WebDriverSettings.OverriddenWebDriverConfiguration.GridUri.Should().Be("http://from.config.grid.uri:31767/");
    }

    [Test]
    public async Task OverriddenWebDriverConfigReturnsLocalhostIfNoConfigsPresent()
    {
        await WriteConfig(WebDriverConfigFileName, null);
        await WriteConfig(GridUriConfigFileName, null);

        WebDriverSettings.OverriddenWebDriverConfiguration.GridUri.Should().Be("http://localhost:4444");
    }

    [Test]
    public async Task OverriddenWebDriverConfigReturnsExpectedValuesIfNoConfigsPresent()
    {
        await WriteConfig(WebDriverConfigFileName, null);
        await WriteConfig(GridUriConfigFileName, null);

        WebDriverSettings.OverriddenWebDriverConfiguration.Should().BeEquivalentTo(WebDriverSettings.WebDriverConfiguration) ;
    }

    [Test]
    public async Task OverriddenWebDriverConfigReturnsExpectedValuesIfOnlyGridUriConfigPresent()
    {
        await WriteConfig(WebDriverConfigFileName, null);
        await WriteConfig(GridUriConfigFileName, SampleGridUriConfig);

        var expectedConfig = WebDriverSettings.WebDriverConfiguration as WebDriverConfiguration;
        expectedConfig.GridUri = new ("http://from.config.grid.uri:31767/");
        
        WebDriverSettings.OverriddenWebDriverConfiguration.Should().BeEquivalentTo(expectedConfig) ;
    }

    [Test]
    public async Task OverriddenWebDriverConfigReturnsLocalConfigIfInvalidGridUriConfigPresent()
    {
        await WriteConfig(WebDriverConfigFileName, SampleWebDriverConfig);
        await WriteConfig(GridUriConfigFileName, InvalidGridUriConfig);

        var expectedConfig = WebDriverSettings.WebDriverConfiguration as WebDriverConfiguration;
        expectedConfig.IsLocal = true;
        expectedConfig.PlatformType = PlatformType.Any;
        expectedConfig.Headless = false;
        
        WebDriverSettings.OverriddenWebDriverConfiguration.Should().BeEquivalentTo(expectedConfig) ;
    }

    [OneTimeTearDown]
    public async Task RestorePreviousConfigs()
    {
        await WriteConfig(GridUriConfigFileName, _previousGridUriConfigFile);
        await WriteConfig(WebDriverConfigFileName, _previousWebDriverConfigFile);
    }
    
    private async Task<string> ReadConfig(string configFileName)
    {
        string configFilePath = Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), configFileName);
        if (!string.IsNullOrEmpty(configFilePath) && File.Exists(configFilePath))
        {
            using (StreamReader file = File.OpenText(configFilePath))
            {
                return file.ReadToEnd();
            }
        }
        return null;
    }
    
    private async Task WriteConfig(string configFileName, string configToWrite)
    {
        string configFilePath = Path.Combine(GetFolderPath(SpecialFolder.MyDocuments), configFileName);
        if (string.IsNullOrEmpty(configToWrite) && File.Exists(configFilePath))
        {
            File.Delete(configFilePath);
        }
        else
        {
            await File.WriteAllTextAsync(configFilePath, configToWrite);
        }
    }
}