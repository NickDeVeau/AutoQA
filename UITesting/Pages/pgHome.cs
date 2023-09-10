using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using AventStack.ExtentReports;

namespace UITesting.Pages
{
    public class pgHome : BaseTestUI
    {
        public static void OpenTabs()
        {
            try
            {
                IWebDriver _driver = Driver.Instance;

                string sideBarButtonPath = "/html/body/div[2]/header/div/div[1]/span";
                IWebElement sideBarButton = _driver.FindElement(By.XPath(sideBarButtonPath));
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));

                sideBarButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(sideBarButtonPath)));

                IWebElement sidebar = _driver.FindElement(By.Id("sidebar-wrapper"));

                IList<IWebElement> childrenElements = sidebar.FindElements(By.CssSelector("a.list-group-item.list-group-item-action"));

                foreach (IWebElement element in childrenElements)
                {
                    string classAttributeValue = element.GetAttribute("class");
                    bool isExpandable = classAttributeValue.Contains("expandable");

                    if (isExpandable)
                    {
                        element.Click();
                    }
                }
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void selectDistributionSite(string siteName)
        {
            try
            {
                // Sets the location in the drop-down list based on the provided location string.

                string dropDownPath = "/html/body/div[2]/header/div/div[2]/div/span";

                IWebDriver _driver = Driver.Instance;

                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                IWebElement siteDropDown = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(dropDownPath)));

                siteDropDown.Click();

                WebDriverWait listWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                IList<IWebElement> listElements = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName("k-list-item")));

                WebDriverWait wait2 = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));

                try
                {
                    wait.Until(driver =>
                    {
                        foreach (IWebElement element in listElements)
                        {
                            if (element.Text.Contains(siteName))
                            {
                                return true;
                            }
                        }
                        return false;
                    });
                }
                catch (WebDriverTimeoutException ex)
                {
                    Console.WriteLine("Desired location not found within the specified timeout.");
                    throw;
                }
                foreach (IWebElement element in listElements)
                {
                    if (element.Text.Contains(siteName))
                    {
                        element.Click();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }
   
        // Click on a menu item in the side navbar and Navigate to the page.
        public static void OpenMenuPage(string pageName)
        {
            try
            {
                OpenTabs();
                
                IWebDriver _driver = Driver.Instance;
                IWebElement sidebar = _driver.FindElement(By.Id("sidebar-wrapper"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].setAttribute('style', 'display: block !important;');", sidebar);

                IWebElement elementLink = _driver.FindElement(By.CssSelector("a[data-side-bar-id='" + pageName + "']"));
                
                elementLink.Click();              
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        // Checks the dropdown menu to verify that all the expected site names are present.
        public static void Validate_SiteName(string[] sSiteNames)
        {
            try
            {
                pgLogin.Login
                (
                    _configuration.GetSection("UserInfo:sEmail").Value,
                    _configuration.GetSection("UserInfo:sPwd").Value
                );

                IWebDriver _driver = Driver.Instance;
                string dropDownPath = "/html/body/div[2]/header/div/div[2]/div/span";
                var correctItems = 0;

                IWebElement siteDropDown = _driver.FindElement(By.XPath(dropDownPath));
                siteDropDown.Click();

                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                siteDropDown = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(dropDownPath)));

                // Wait for the list items to be present
                IReadOnlyCollection<IWebElement> listItems = wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName("k-list-item")));

                foreach (string siteName in sSiteNames)
                {
                    bool isSiteNamePresent = false;
                    foreach (IWebElement listItem in listItems)
                    {
                        if (listItem.Text.Trim() == siteName)
                        {
                            isSiteNamePresent = true;
                            break;
                        }
                    }

                    if (isSiteNamePresent)
                    {
                        correctItems++;
                    }
                }

                if (correctItems != sSiteNames.Length)
                {
                    throw new Exception("Not all site names are present in the dropdown.");
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        // Validates that each of the expected menu items is present in the sidebar menu.
        public static void Validate_MenuItems(string[] menuItems)
        {
            try
            {
                pgLogin.Login
                (
                    _configuration.GetSection("UserInfo:sEmail").Value,
                    _configuration.GetSection("UserInfo:sPwd").Value
                );

                IWebDriver _driver = Driver.Instance;
                _driver.Manage().Window.Maximize();
                
                string sideBarButtonPath = "/html/body/div[2]/header/div/div[1]/span";
                IWebElement sideBarButton = _driver.FindElement(By.XPath(sideBarButtonPath));
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                sideBarButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(sideBarButtonPath)));

                IWebElement sidebar = _driver.FindElement(By.Id("sidebar-wrapper"));

                IList<IWebElement> childrenElements = sidebar.FindElements(By.CssSelector("a.list-group-item.list-group-item-action"));

                foreach (IWebElement element in childrenElements)
                {
                    string classAttributeValue = element.GetAttribute("class");
                    bool isExpandable = classAttributeValue.Contains("expandable");
                    if (isExpandable)
                    {
                        element.Click();
                    }
                }
                for (int i = 0; i < menuItems.Length; i++)
                {
                    WebDriverWait pageWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                    sideBarButton = pageWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.list-group-item[data-side-bar-id='" + menuItems[i] + "']")));

                    if (_driver.FindElement(By.CssSelector("a.list-group-item[data-side-bar-id='" + menuItems[i] + "']")) == null)
                    {
                        throw new Exception(menuItems[i] + " is missing from the menu.");
                    }
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: "+ string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        // Validates the functionality of each menu item by clicking on it and verifying that the corresponding page is displayed.
        public static void ValidateMenuFunctionality(string[] pageItems)
        {
            try
            {
                pgLogin.Login
                (
                    _configuration.GetSection("UserInfo:sEmail").Value,
                    _configuration.GetSection("UserInfo:sPwd").Value
                );

                var leftNavMenu = "sidebar-wrapper";
                var btnNavPath = "/html/body/div[2]/header/div/div[1]/span";

                IWebDriver _driver = Driver.Instance;
                IWebElement sidebar = _driver.FindElement(By.Id(leftNavMenu));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].setAttribute('style', 'display: block !important;');", sidebar);

                var correctItems = 0;

                IWebElement sideBarButton = _driver.FindElement(By.XPath(btnNavPath));
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                sideBarButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(btnNavPath)));

                for (int i = 0; i < pageItems.Length; i++)
                {
                    WebDriverWait pageWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                    sideBarButton = pageWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("a.list-group-item[data-side-bar-id='" + pageItems[i] + "']")));

                    pgHome.OpenMenuPage(pageItems[i]);

                    string pageSource = _driver.PageSource;
                    string txtTitle = _driver.FindElement(By.TagName("main")).Text.ToLower();

                    if (txtTitle.Contains(pageItems[i].ToLower()))
                    {
                        // Success
                    }
                    else
                    {
                        throw new Exception(pageItems[i] + " is not present in the title.");
                    }
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }
    }
}
