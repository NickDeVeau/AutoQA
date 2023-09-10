using NUnit.Framework;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using System.Threading.Tasks;

namespace RCDAppTests
{
    [TestFixture]
    public class YourTestClass : BaseTest
    {
        [Test]
        public void YourTestMethod()
        {
            Parallel.ForEach(drivers, driver =>
            {
                if (driver is AndroidDriver<AndroidElement>)
                {
                    // Android-specific logic
                }
                else if (driver is IOSDriver<IOSElement>)
                {
                    // iOS-specific logic
                }
            });

        }

    }
}
