using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace UITesting
{
    public static class UI
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void ClickElement(IWebDriver driver, By locator)
        {
            try
            {
                var element = driver.FindElement(locator);
                element.Click();
                Log.Info($"Clicked element with locator: {locator}");
            }
            catch (Exception e)
            {
                Log.Error($"Failed to click element with locator: {locator}. Exception: {e.Message}");
            }
        }

        public static void EnterText(IWebDriver driver, By locator, string text)
        {
            try
            {
                var element = driver.FindElement(locator);
                element.Clear();
                element.SendKeys(text);
                Log.Info($"Entered text: '{text}' into element with locator: {locator}");
            }
            catch (Exception e)
            {
                Log.Error($"Failed to enter text: '{text}' into element with locator: {locator}. Exception: {e.Message}");
            }
        }

        public static string GetText(IWebDriver driver, By locator)
        {
            try
            {
                var element = driver.FindElement(locator);
                Log.Info($"Retrieved text: '{element.Text}' from element with locator: {locator}");
                return element.Text;
            }
            catch (Exception e)
            {
                Log.Error($"Failed to retrieve text from element with locator: {locator}. Exception: {e.Message}");
                return string.Empty;
            }
        }

        public static void HoverOverElement(IWebDriver driver, By locator)
        {
            try
            {
                var element = driver.FindElement(locator);
                var actions = new Actions(driver);
                actions.MoveToElement(element).Perform();
                Log.Info($"Hovered over element with locator: {locator}");
            }
            catch (Exception e)
            {
                Log.Error($"Failed to hover over element with locator: {locator}. Exception: {e.Message}");
            }
        }

        public static bool IsElementPresent(IWebDriver driver, By locator)
        {
            try
            {
                driver.FindElement(locator);
                Log.Info($"Element with locator: {locator} is present");
                return true;
            }
            catch (NoSuchElementException)
            {
                Log.Info($"Element with locator: {locator} is not present");
                return false;
            }
        }

        public static void WaitForElementToBeVisible(IWebDriver driver, By locator, int timeoutInSeconds)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(locator));
                Log.Info($"Element with locator: {locator} became visible after waiting");
            }
            catch (WebDriverTimeoutException e)
            {
                Log.Error($"Element with locator: {locator} did not become visible after waiting for {timeoutInSeconds} seconds. Exception: {e.Message}");
            }
        }

        public static void SelectFromDropdownByValue(IWebDriver driver, By dropdownLocator, string value)
        {
            try
            {
                var selectElement = new SelectElement(driver.FindElement(dropdownLocator));
                selectElement.SelectByValue(value);
                Log.Info($"Selected value '{value}' from dropdown with locator: {dropdownLocator}");
            }
            catch (Exception e)
            {
                Log.Error($"Failed to select value '{value}' from dropdown with locator: {dropdownLocator}. Exception: {e.Message}");
            }
        }
    }
}
