using NUnit.Framework;
using OpenQA.Selenium;
using log4net;
using UITesting.Pages;

namespace UITesting.TestSuites
{
    [TestFixture, Order(1)]
    [Category("Login")]
    public class Login : BaseTestUI
    {
        public static IWebDriver _driver = Driver.Instance;

        public static ILog logger = LogManager.GetLogger(typeof(Login));

        [Test, Order(1)]
        [Category("Sanity")]
        [Category("Regression")]
        public void VerifyLogin()
        {
            
            pgLogin.Validate_Login
            (
               _configuration.GetSection("UserInfo:sEmail").Value,
               _configuration.GetSection("UserInfo:sPwd").Value,
               _configuration.GetSection("UserInfo:sFullName").Value
            );
        }

        [Test, Order(2)]
        [Category("Sanity")]
        [Category("Regression")]
        public void VerifyLogout()
        {
            pgLogin.Validate_Logout();
        }
    }
}
