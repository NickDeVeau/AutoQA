using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using AventStack.ExtentReports;
using System.Text.RegularExpressions;

namespace UITesting.Pages
{
    public class pgDispatches : BaseTestUI
    {

        public static int Get_Row_Count()
        {
            try
            {
                IWebDriver _driver = Driver.Instance;

                var totalItems = 0;
                var input = _driver.FindElement(By.XPath("//*[@id=\"dispatchesGrid\"]/div/span")).Text;
                string pattern = @"\d+"; // Matches one or more digit

                MatchCollection matches = Regex.Matches(input, pattern);

                if (matches.Count > 0)
                {
                    // Get the last match (which represents the last number in the string)
                    string lastNumberString = matches[matches.Count - 1].Value;

                    if (int.TryParse(lastNumberString, out int lastNumber))
                    {
                        totalItems = lastNumber;
                    }
                }
                return totalItems;
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }


        public static void Validate_DispatchItems(string siteName)
        {
            try
            {
                pgLogin.Login(
                    _configuration.GetSection("UserInfo:sEmail").Value,
                    _configuration.GetSection("UserInfo:sPwd").Value
                );

                pgHome.selectDistributionSite(siteName);

                pgHome.OpenMenuPage("Dispatches");

                IWebDriver _driver = Driver.Instance;

                var recentDispatchPath = "//*[@id=\"dispatchesStrip-tab-2\"]";
                var counterPath = "/html/body/div[1]/div/div/div[2]/main/div/div/div/div/div[3]/div[1]/div/span";

                WebDriverWait dispatchWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));

                IWebElement btnRecentDispatch = dispatchWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(recentDispatchPath)));
                btnRecentDispatch.Click();

                By locator = By.XPath(counterPath);
                IList<IWebElement> elements = _driver.FindElements(locator);

                if (elements.Count > 0)
                {
                    int count = Get_Row_Count();

                    string input = dispatchWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(counterPath))).Text;

                    string pattern = @"\d+"; // Matches one or more digits

                    MatchCollection matches = Regex.Matches(input, pattern);

                    if (matches.Count > 0)
                    {
                        // Get the last match (which represents the last number in the string)
                        string lastNumberString = matches[matches.Count - 1].Value;

                        if (int.TryParse(lastNumberString, out int lastNumber))
                        {
                            Assert.IsTrue(lastNumber == count);
                        }
                    }
                }
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static List<string> Get_Row_Status()
        {
            try
            {
                List<string> status = new List<string>();
                string recentDispatchPath = "//*[@id=\"dispatchesStrip-tab-2\"]";
                string nextPagePath = "//*[@id=\"dispatchesGrid\"]/div/a[4]";
                IWebDriver _driver = Driver.Instance;

                WebDriverWait dispatchWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                IWebElement btnRecentDispatch = dispatchWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(recentDispatchPath)));

                btnRecentDispatch.Click();

                WebDriverWait nextPageWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                IWebElement btnNextPage = nextPageWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(nextPagePath)));

                WebDriverWait gridWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                IWebElement gridElement = gridWait.Until(ExpectedConditions.ElementIsVisible(By.Id("dispatchesGrid")));

                WebDriverWait rowWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                IList<IWebElement> rowElements = rowWait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("tbody > tr")));
                List<string> elementTexts = new List<string>();

                int pages = Get_Row_Count() / 10;

                for (int i = 0; i < pages; i++)
                {
                    foreach (IWebElement rowElement in rowElements)
                    {
                        IList<IWebElement> columnElements = rowElement.FindElements(By.TagName("td"));
                        string elementText = columnElements[5].Text.ToLower();

                        elementTexts.Add(elementText);
                    }
                    status.AddRange(elementTexts);
                    btnNextPage.Click();
                }
                return status;
            }
            catch
            { 
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void Validate_Status(string siteName)
        {
            try
            {
                IWebDriver _driver = Driver.Instance;

                var counterPath = "/html/body/div[1]/div/div/div[2]/main/div/div/div/div/div[3]/div[1]/div/span";

                //By locator = By.XPath(counterPath);
                //IList<IWebElement> elements = _driver.FindElements(locator);

                //if (elements.Count <= 0)
                //{
                //    return;
                //}
                pgLogin.Login
                (
                    _configuration.GetSection("UserInfo:sEmail").Value,
                    _configuration.GetSection("UserInfo:sPwd").Value
                 );

                pgHome.selectDistributionSite(siteName);

                pgHome.OpenMenuPage("Dispatches");

                var status = Get_Row_Status().ToArray();

                bool isMatched = false;  // Initialize the flag as false

                foreach (string item in status)
                {
                    switch (item.ToLower())
                    {
                        case "accepted":
                            isMatched = true;
                            break;
                        case "cancelled":
                            isMatched = true;
                            break;
                        case "no response":
                            isMatched = true;
                            break;
                        case "awaiting":
                            isMatched = true;
                            break;
                        case "rejected":
                            isMatched = true;
                            break;
                        default:
                            break;
                    }
                }
                if(status.Length == 0)
                {
                    isMatched = true;
                }
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }
        
    
        public static void Validate_DispatchView(int itemNumber, string siteName)
        {
            if (itemNumber != -1)
            {
                try
                {
                    pgLogin.Login
                    (
                        _configuration.GetSection("UserInfo:sEmail").Value,
                        _configuration.GetSection("UserInfo:sPwd").Value
                    );

                    pgHome.selectDistributionSite(siteName);

                    pgHome.OpenMenuPage("Dispatches");

                    string recentDispatchPath = "//*[@id=\"dispatchesStrip-tab-2\"]";
                    IWebDriver _driver = Driver.Instance;

                    WebDriverWait dispatchWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                    IWebElement btnRecentDispatch = dispatchWait.Until(ExpectedConditions.ElementIsVisible(By.XPath(recentDispatchPath)));

                    btnRecentDispatch.Click();

                    WebDriverWait gridWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                    IWebElement gridElement = gridWait.Until(ExpectedConditions.ElementIsVisible(By.Id("dispatchesGrid")));

                    IList<IWebElement> rowElements = gridElement.FindElements(By.CssSelector("tbody > tr"));

                    IList<IWebElement> columnElements = rowElements[itemNumber].FindElements(By.TagName("td"));

                    var time = columnElements[0].Text;
                    char delimiter = '\n';
                    int index = time.IndexOf(delimiter);
                    if (index >= 0)
                    {
                        string trimmedString = time.Substring(0, index);
                        time = trimmedString;
                    }

                    var destination = columnElements[1].Text;
                    var type = columnElements[2].Text;
                    var status = columnElements[5].Text;

                    WebDriverWait viewWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                    IWebElement viewtempElement = viewWait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".k-grid-View")));

                    IWebElement viewElement = rowElements[itemNumber].FindElement(By.CssSelector(".k-grid-View"));
                    viewElement.Click();

                    WebDriverWait timeWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                    IWebElement timeElement = timeWait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"dispatchDetailsSection\"]/div[6]/div/p[2]")));

                    var sTime = timeElement.Text;
                    Assert.IsTrue(sTime.Contains(time));

                    WebDriverWait destinationWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                    IWebElement destinationElement = destinationWait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"dispatchDetailsSection\"]/div[5]/div/p[2]")));
                    Assert.IsTrue(destinationElement.Text.Contains(destination));

                    WebDriverWait typeWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                    IWebElement typeElement = typeWait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"dispatchDetailsSection\"]/div[2]/div/p[2]")));
                    Assert.IsTrue(typeElement.Text.Contains(type));

                    WebDriverWait statusWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));
                    IWebElement statusElement = statusWait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"dispatchDetails\"]/div/div[1]/div/div/span")));
                    Assert.IsTrue(statusElement.Text.Contains(status));
                }
                catch (Exception ex)
                {
                    test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                    throw;
                }
            }

        }
    }
}
