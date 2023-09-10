using NUnit.Framework;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UITesting.Pages
{
    public class pgLogin : BaseTestUI
    {
        public static IWebElement txtEmail;
        public static IWebElement txtPassword;
        public static IWebElement btnLogin;
        public static IWebDriver _driver = Driver.Instance;

        // Gets the login controls on the screen.
        public static void GetControlsOnScreen_Login()
        {
            try
            {
                _driver = Driver.Instance;

                txtEmail = _driver.FindElement(By.Id("username"));
                txtPassword = _driver.FindElement(By.Id("password"));
                btnLogin = _driver.FindElement(By.ClassName("ping-buttons"));
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        // Performs the login action.
        public static void Login(string sEmail, string sPwd)
        {
            string txtRedCrossDeliversPath = "/html/body/div[2]/header/div/div[1]/h6";

            GetControlsOnScreen_Login();

            txtEmail.EnterText(sEmail);
            txtPassword.EnterText(sPwd);

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("ping-buttons")));
            btnLogin.Click();


            Assert.IsTrue(_driver.FindElement(By.XPath(txtRedCrossDeliversPath)).Text == "Red Cross Delivers");
        }

        // Validates the login functionality.
        public static void Validate_Login(string sEmail, string sPwd, string sfullName)
        {
            try
            {
                Login(sEmail, sPwd);

                IWebElement nameElement = _driver.FindElement(By.XPath("/html/body/div[2]/header/div/div[2]/div/div/span[2]"));
                var fullName = nameElement.Text.Trim();
                Assert.AreEqual(sfullName, fullName);

            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        // Validates the logout functionality.
        public static void Validate_Logout()
        {
            try
            {
                pgLogin.Validate_Login(
                    _configuration.GetSection("UserInfo:sEmail").Value,
                    _configuration.GetSection("UserInfo:sPwd").Value,
                    _configuration.GetSection("UserInfo:sFullName").Value
                );

                IWebElement btnAccount = _driver.FindElement(By.ClassName("rc-account"));
                btnAccount.Click();

                IWebElement btnLogout = _driver.FindElement(By.Id("logout"));
                btnLogout.Click();

                IWebElement txtPing = _driver.FindElement(By.ClassName("ping-header"));
                var pingText = txtPing.Text;

                Assert.IsTrue(pingText == "Sign Off Successful");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }
    }
}
