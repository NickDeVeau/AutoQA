using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;

namespace RCDAppTests
{
    [TestFixture]
    public class BaseTest
    {
        public List<IWebDriver> drivers;

        [SetUp]
        public void SetUp()
        {
            drivers = Driver.RunAllDevicesInParallel();
        }

       

        [TearDown]
        public void TearDown()
        {
            foreach (var driver in drivers)
            {
                driver.Quit();
            }
        }
    }
}
