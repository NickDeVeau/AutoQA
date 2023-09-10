using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace UITesting
{
    public static class UI
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void EnterText(this IWebElement control, string value,bool submitAtEnd = false)
        {
            control.Clear();
            control.SendKeys(value);
            control.SendKeys(Keys.Tab);
            if (submitAtEnd)
            {
                control.SendKeys(Keys.Return);
            }
        }





        public static string RgbaToHex(string rgba)
        {
            var match = Regex.Match(rgba, @"rgba?\((\d+),\s*(\d+),\s*(\d+)(?:,\s*(\d+(?:\.\d+)?))?\)");
            var red = byte.Parse(match.Groups[1].Value);
            var green = byte.Parse(match.Groups[2].Value);
            var blue = byte.Parse(match.Groups[3].Value);
            var alpha = match.Groups[4].Success ? (int)(float.Parse(match.Groups[4].Value) * 255) : 255;
            return $"#{red:X2}{green:X2}{blue:X2}";
        }

        public static IWebElement WaitForDropdownOption(IWebElement dropdownOption, int timeoutInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds));

            // This condition waits for the dropdown option to be both displayed and enabled.
            return wait.Until(drv => dropdownOption.Displayed && dropdownOption.Enabled ? dropdownOption : null);
        }


        public static IWebElement WaitForElement(IWebElement element, int timeoutInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutInSeconds));
            return wait.Until(drv => element.Displayed ? element : null);
        }

        public static IReadOnlyList<IWebElement> GetRowsFromGrid(IWebElement gridElement)
        {
            return gridElement.FindElements(By.TagName("tr"));
        }

        // Overloaded method that accepts a locator for the grid and fetches rows
        public static IReadOnlyList<IWebElement> GetRowsFromGrid(By gridLocator, IWebDriver driver)
        {
            var gridElement = driver.FindElement(gridLocator);
            return gridElement.FindElements(By.TagName("tr"));
        }
    

    public static IWebElement GetColumnFromRow(this IWebElement row, int columnIndex)
        {
            try
            {
                return row.FindElement(By.XPath($"td[{columnIndex+1}]"));
            }
            catch (NoSuchElementException ex)
            {
                log.Error($"Error accessing column {columnIndex} from the given row.", ex);
                return null;
            }
        }

        public static void WaitForElementTextToChange(IWebElement element, string oldText, int timeoutSeconds = 10)
        {
            WebDriverWait wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(drv => !element.Text.Equals(oldText));
        }

        public static List<IWebElement> GetDropdownOptions(this IWebElement element, bool closeAtEnd = false)
        {
            List<IWebElement> optionsText = new List<IWebElement>();

            if (element.TagName.ToLower() == "select")
            {
                SelectElement selectElement = new SelectElement(element);
                optionsText.AddRange(selectElement.Options);
            }
            else if (element.GetAttribute("class").Contains("k-combobox"))
            {
                var button = element.FindElement(By.CssSelector(".k-input-button"));
                button.Click();

                // Use aria-controls attribute to find the associated listbox
                string ariaControls = element.FindElement(By.CssSelector(".k-input-inner")).GetAttribute("aria-controls");
                var optionElements = Driver.Instance.FindElements(By.CssSelector($"#{ariaControls} li"));

                optionsText.AddRange(optionElements);

                if (closeAtEnd)
                {
                    button.Click(); // Optionally, click again to close the ComboBox
                }
            }
            else if (element.GetAttribute("class").Contains("k-dropdownlist"))
            {
                var button = element.FindElement(By.CssSelector(".k-input-button"));
                button.Click();

                // Use aria-controls attribute to find the associated listbox
                string ariaControls = element.GetAttribute("aria-controls");
                var optionElements = Driver.Instance.FindElements(By.CssSelector($"#{ariaControls} li"));

                optionsText.AddRange(optionElements);

                if (closeAtEnd)
                {
                    button.Click(); // Optionally, click again to close the DropDownList
                }
            }
            else
            {
                throw new ArgumentException("The provided element is neither a standard dropdown, a recognized ComboBox, nor a DropDownList.");
            }

            return optionsText;
        }




        public static bool IsGridSortedDescending(this IList<IWebElement> rows, int columnIndex)
        {
            List<string> columnData = new List<string>();

            foreach (var row in rows)
            {
                string cellValue = row.FindElement(By.XPath($"td[{columnIndex}]")).Text;
                columnData.Add(cellValue);
            }

            List<string> sortedColumnData = new List<string>(columnData);
            sortedColumnData.Sort((a, b) => b.CompareTo(a));

            return sortedColumnData.SequenceEqual(columnData);
        }

      

        public static bool IsGridSortedAscending(this IList<IWebElement> rows, int columnIndex)
        {
            List<string> columnData = new List<string>();

            foreach (var row in rows)
            {
                string cellValue = row.FindElement(By.XPath($"td[{columnIndex}]")).Text;
                columnData.Add(cellValue);
            }

            List<string> sortedColumnData = new List<string>(columnData);
            sortedColumnData.Sort();

            return sortedColumnData.SequenceEqual(columnData);
        }

        public static void ClickLinkByText(this IWebDriver driver, string linkText)
        {
            driver.FindElement(By.LinkText(linkText)).Click();
        }

        public static void LogMessage(string message)
        {
            log.Info(message);
        }

        public static void ScrollToElement(this IWebDriver driver, IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public static void ScrollToTop(this IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);");
        }

        public static void ScrollToBottom(this IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }

        public static bool ElementExists(this IWebDriver driver, By selector)
        {
            return driver.FindElements(selector).Count > 0;
        }

        public static bool IsElementVisible(this IWebDriver driver, By selector)
        {
            try
            {
                return driver.FindElement(selector).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static void SwitchToFrame(this IWebDriver driver, By frameSelector)
        {
            IWebElement frame = driver.FindElement(frameSelector);
            driver.SwitchTo().Frame(frame);
        }

        public static void SwitchToDefaultContent(this IWebDriver driver)
        {
            driver.SwitchTo().DefaultContent();
        }

        public static void TakeScreenshot(this IWebDriver driver, string path)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
        }

        public static void HoverOverElement(this IWebDriver driver, IWebElement element)
        {
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
        }

        public static void DragAndDrop(this IWebDriver driver, IWebElement sourceElement, IWebElement targetElement)
        {
            Actions action = new Actions(driver);
            action.DragAndDrop(sourceElement, targetElement).Perform();
        }

        public static void WaitForPageLoad(this IWebDriver driver, int timeoutInSeconds)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(d =>
                ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static void RefreshPage(this IWebDriver driver)
        {
            driver.Navigate().Refresh();
        }
    }
}
