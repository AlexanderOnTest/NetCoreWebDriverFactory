using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace AlexanderOnTest.NetCoreWebDriverFactory.DriverOptionsFactory
{
    /// <summary>
    /// An IDriverOptionsFactory implementation that provides Chrome and Firefox instances with a defined default language
    /// </summary>
    public class CulturedDriverOptionsFactory : DefaultDriverOptionsFactory, IDriverOptionsFactory
    {
        /// <summary>
        /// Construct a new instance of the CulturedDriverOptionsFactory for Chrome, (Chromium based) Edge and Firefox
        /// </summary>
        /// <param name="requestedCulture"></param>
        public CulturedDriverOptionsFactory(CultureInfo requestedCulture) : base(new Dictionary<Type, DriverOptions>())
        {
            ChromeOptions chromeCultureOptions = StaticDriverOptionsFactory.GetChromeOptions(false);
            chromeCultureOptions.AddUserProfilePreference("intl.accept_languages", requestedCulture.ToString());
            DriverOptionsDictionary.Add(typeof(ChromeOptions), chromeCultureOptions);
            
            FirefoxOptions firefoxCultureOptions = StaticDriverOptionsFactory.GetFirefoxOptions();
            firefoxCultureOptions.SetPreference("intl.accept_languages", requestedCulture.ToString());
            DriverOptionsDictionary.Add(typeof(FirefoxOptions), firefoxCultureOptions);

            Microsoft.Edge.SeleniumTools.EdgeOptions edgeCultureOptions = StaticDriverOptionsFactory.GetEdgiumOptions();
            edgeCultureOptions.AddUserProfilePreference("intl.accept_languages", requestedCulture.ToString());
            DriverOptionsDictionary.Add(typeof(Microsoft.Edge.SeleniumTools.EdgeOptions), edgeCultureOptions);
        }
        
        /// <summary>
        /// Return a DriverOptions instance of the correct type configured for a Local WebDriver. Only supported for Chrome and Firefox
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="headless"></param>
        /// <returns></returns>
        T IDriverOptionsFactory.GetLocalDriverOptions<T>(bool headless)
        {
            Type type = typeof(T);
            DriverOptionsDictionary.TryGetValue(type, out DriverOptions driverOptions);
            T options = driverOptions as T;
            
            if (headless)
            {
                if (options is FirefoxOptions)
                {
                    options = AddHeadlessOption(options);
                }
                else
                {
                    Trace.WriteLine("Chromium does not support language profiles in headless operation, running on screen.");
                }
            }
            return options;
        }
    }
}