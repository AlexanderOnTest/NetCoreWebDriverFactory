using System.Drawing;
using OpenQA.Selenium;

namespace AlexanderOnTest.NetCoreWebDriverFactory.Utils
{
    /// <summary>
    /// Interface for a WebDriver resizing service
    /// </summary>
    public interface IWebDriverReSizer
    {
        /// <summary>
        /// Set the WebDriver to the requested Browser size.
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="windowSize"></param>
        /// <param name="windowCustomSize"></param>
        /// <returns></returns>
        IWebDriver SetWindowSize(
            IWebDriver driver, 
            WindowSize windowSize, 
            Size windowCustomSize = new Size());
    }
}