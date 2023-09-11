using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace UITesting
{
    public class Driver : BaseTestUI
    {
        private static ThreadLocal<IWebDriver> _driver = new ThreadLocal<IWebDriver>();

        public static IWebDriver Instance
        {
            get { return _driver.Value; }
            private set { _driver.Value = value; }
        }

        public static void Initialize(string testName)
        {
            var localChromeDriver = Convert.ToBoolean(ConfigurationService.Instance.GetValue("AppInfo:LocalChromeDriver"));
            if (localChromeDriver)
            {
                Instance = new ChromeDriver();
            }
            else
            {
                ChromeOptions capabilities = new ChromeOptions();
                Dictionary<string, object> browserstackOptions = new Dictionary<string, object>();
                browserstackOptions.Add("os", "Windows");
                browserstackOptions.Add("osVersion", "10");
                browserstackOptions.Add("browserVersion", "latest");
                browserstackOptions.Add("local", "false");
                browserstackOptions.Add("seleniumVersion", "4.10.0");
                browserstackOptions.Add("userName", "nickdeveau_kKg7iN");
                browserstackOptions.Add("accessKey", "xYfgDn2zAP8wyNPpgMUC");
                browserstackOptions.Add("browserName", "Chrome");
                browserstackOptions.Add("sessionName", testName);
                capabilities.AddAdditionalOption("bstack:options", browserstackOptions);

                Instance = new RemoteWebDriver(
                    new Uri("https://hub.browserstack.com/wd/hub/"), capabilities);
            }
        }

        public static void SetZoomLevel(int level)
        {
            var js = (IJavaScriptExecutor)Instance;
            js.ExecuteScript($"document.body.style.zoom='{level.ToString()}%'");
            Thread.Sleep(3000);
        }

        public static void CloseDriver()
        {
            if (Instance != null)
            {
                Instance.Dispose();
                Instance.Quit();
                Instance = null;
            }
        }
    }
}
