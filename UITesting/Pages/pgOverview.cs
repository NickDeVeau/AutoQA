using NUnit.Framework;
using OpenQA.Selenium;
using AventStack.ExtentReports;

using UITesting.TestSuites;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Interactions;

namespace UITesting.Pages
{
	public class pgOverview : Overview
	{
        public static void SetUpOverview()
        {
            IWebDriver driver = Driver.Instance;

            // User login
            pgLogin.Login(
                _configuration.GetSection("UserInfo:sEmail").Value,
                _configuration.GetSection("UserInfo:sPwd").Value);

            // Select the desired distribution site
            pgHome.selectDistributionSite(SiteName);
        }

        //Shift Main
        public static void ValidateShiftName()
        {
            try
            {
                ShiftSearchBar.EnterText(ShiftName,true);

                var rows = UI.GetRowsFromGrid(ShiftGrid).ToArray();

                string[] shiftNames = new string[rows.Length];

                IWebElement targetRow = null;

                for (int i = 0; i < rows.Length;i++)
                {
                    shiftNames[i] = UI.GetColumnFromRow(rows[i],0).Text;
                    targetRow = rows[i];
                }

                Assert.IsTrue(shiftNames.Contains(ShiftName));
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateShiftTypeDropDown()
        {
            var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(waitTime));

            // Wait for dropdown to be clickable and then get its options
            wait.Until(ExpectedConditions.ElementToBeClickable(ShiftTypeDropDown));
            var dropdownOptions = UI.GetDropdownOptions(ShiftTypeDropDown);

            Assert.IsTrue(dropdownOptions.Count == 3);

            // Click the first option
            UI.WaitForElement(dropdownOptions[0],waitTime).Click();

            // Now, since the DOM might have changed after clicking the option, re-fetch the first option before getting the span
            dropdownOptions = UI.GetDropdownOptions(ShiftTypeDropDown);
            var test = dropdownOptions[0].FindElement(By.TagName("span"));

            // Wait for grid to be visible
            wait.Until(ExpectedConditions.ElementToBeClickable(ShiftGrid));


            var rows = UI.GetRowsFromGrid(ShiftGrid);

            string shiftTypeColumn;

            for (int i = 0; i < rows.Count; i++)
            {
                // Wait for the cell's text to be non-empty
                wait.Until(driver => UI.GetColumnFromRow(rows[i], 1).Text != "");
                shiftTypeColumn = UI.GetColumnFromRow(rows[i], 1).Text;
                Assert.IsTrue(shiftTypeColumn == "Scheduled");
            }
        }

        public static void ValidateShiftCalendar()
        {
            try
            {
                // Input and filter by date range
                _driver.FindElement(By.XPath("//div[@id='currentShiftsDateRangePicker']/span/span/input")).Click();
                _driver.FindElement(By.LinkText($"{TodaysDate.Day.ToString()}")).Click();
                _driver.FindElement(By.LinkText($"{TodaysDate.AddDays(1).Day.ToString()}")).Click();
                _driver.FindElement(By.XPath("(//input[@type='text'])[4]")).Click();

                // Search by the formatted date
                string todayDate = DateTime.Now.ToString("MMddyy");
                var searchBox = _driver.FindElement(By.XPath("(//input[@type='text'])[4]"));
                searchBox.SendKeys(ShiftName);

                // Wait until search results appear
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(50));
                wait.Until(drv => drv.FindElements(By.XPath("//div[4]/div/div/div[2]")).Count > 0);
                searchBox.SendKeys(Keys.Enter);

                // Process and validate the search results

                pgOverview.ValidateShiftName();
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateShiftStatusColor()
        {
            try
            {

                var statusBadge = UI.GetColumnFromRow(UI.GetRowsFromGrid(ShiftGrid)[0],5).FindElement(By.TagName("span"));

                string actualColorHex = UI.RgbaToHex(statusBadge.GetCssValue("background-color"));

                switch (statusBadge.Text)
                {
                    case "IN PROGRESS":
                        Assert.AreEqual("#537B35", actualColorHex, $"Background color for {statusBadge.Text} is not correct.");
                        break;

                    case "UPCOMING":
                        Assert.AreEqual("#537B35", actualColorHex, $"Background color for {statusBadge.Text} is not correct.");
                        break;

                    case "EXPIRED":
                        Assert.AreEqual("#537B35", actualColorHex, $"Background color for {statusBadge.Text} is not correct.");
                        break;

                    case "COMPLETE":
                        Assert.AreEqual("#537B35", actualColorHex, $"Background color for {statusBadge.Text} is not correct.");
                        break;

                    default:
                        Assert.Fail($"Unexpected badge text: {statusBadge.Text}");
                        break;
                }
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateShiftDateAndTimeColumn()
        {
            try
            {
                var rows = UI.GetRowsFromGrid(ShiftGrid);

                string dateAndTime;

                for (int i = 0; i < rows.ToArray().Length; i++)
                {
                    dateAndTime = UI.GetColumnFromRow(rows[i], 2).Text;
                    Assert.IsTrue(dateAndTime.Contains(TodaysDate.ToString("dd"))|| dateAndTime.Contains(TodaysDate.AddDays(1).ToString("dd")));
                }

                
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateShiftOrder(string direction)
        {
            try
            {
                var rows = UI.GetRowsFromGrid(ShiftGrid).ToArray();

                

                switch (direction)
                {
                    case "Ascending":
                        Assert.IsTrue(UI.IsGridSortedAscending(rows, 1));
                        break;

                    case "Descending":
                        Assert.IsTrue(UI.IsGridSortedDescending(rows, 1));
                        break;

                    default:
                        Assert.Fail($"Invalid parameter: {direction}");
                        break;
                }

            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateShiftSearchBars()
        {
            try
            {
                //UI.EnterText(DateSearchBar, TodaysDate.ToString("MM/dd/yy"),true);

                var rows = UI.GetRowsFromGrid(ShiftGrid);

                string target;

                //for (int i = 0; i < rows.ToArray().Length; i++)
                //{
                //    target = UI.GetColumnFromRow(rows[i], 2).Text;
                //    Assert.IsTrue(target.Contains(TodaysDate.ToString("MM/dd/yy")));
                //}

                //UI.EnterText(DateSearchBar, TodaysDate.ToString(""), true);

                UI.EnterText(NumberOfMovementsSearchBar, "1", true);

                rows = UI.GetRowsFromGrid(ShiftGrid);

                for (int i = 0; i < rows.ToArray().Length; i++)
                {
                    target = UI.GetColumnFromRow(rows[i], 3).Text;
                    Assert.IsTrue(target.Contains("1"));
                }

                UI.EnterText(NumberOfMovementsSearchBar, "", true);

                UI.EnterText(DriverSearchBar, "Prabu", true);

                rows = UI.GetRowsFromGrid(ShiftGrid);

                for (int i = 0; i < rows.ToArray().Length; i++)
                {
                    target = UI.GetColumnFromRow(rows[i], 4).Text;
                    Assert.IsTrue(target.Contains("Prabu"));
                }

                UI.EnterText(DriverSearchBar, "", true);

                UI.EnterText(ShiftStatusSearchBar, "IN PROGRESS", true);

                rows = UI.GetRowsFromGrid(ShiftGrid);

                for (int i = 0; i < rows.ToArray().Length; i++)
                {
                    target = UI.GetColumnFromRow(rows[i], 5).Text;
                    Assert.IsTrue(target.Contains("IN PROGRESS"));
                }

                UI.EnterText(ShiftStatusSearchBar, "", true);

                UI.EnterText(PaidOrVolunteerSearchBar, "Volunteer", true);

                rows = UI.GetRowsFromGrid(ShiftGrid);

                for (int i = 0; i < rows.ToArray().Length; i++)
                {
                    target = UI.GetColumnFromRow(rows[i], 6).Text;
                    Assert.IsTrue(target.Contains("Volunteer"));
                }

                UI.EnterText(PaidOrVolunteerSearchBar, "", true);
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        //

        //Shift Pop Up View

        public static void OpenViewWindow()
        {
            try
            {
                var rows = UI.GetRowsFromGrid(ShiftGrid);

                for (int i = 0; i < rows.ToArray().Length; i++)
                {
                    var name = UI.GetColumnFromRow(rows[i], 0);

                    if (name.Text == ShiftName)
                    {
                        UI.GetColumnFromRow(rows[i], 7).Click();
                        break;
                    }
                }

                // Wait for the specified element to become visible
                WebDriverWait wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(waitTime));
                wait.Until(driver => ModalView.Displayed);
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void GrabShiftData()
        {
            try
            {
                var rows = UI.GetRowsFromGrid(ShiftGrid);

                for (int i = 0; i < rows.ToArray().Length; i++)
                {
                    var name = UI.GetColumnFromRow(rows[i], 0);

                    if (name.Text == ShiftName)
                    {
                        ShiftDriver = UI.GetColumnFromRow(rows[i], 4).Text;
                        PaidOrVolunteer = UI.GetColumnFromRow(rows[i], 6).Text;
                        ShiftDateTime = UI.GetColumnFromRow(rows[i], 2).Text;
                        NumberOfMovements = UI.GetColumnFromRow(rows[i], 3).Text;
                        ShiftType = UI.GetColumnFromRow(rows[i], 1).Text;
                        ShiftStatus = UI.GetColumnFromRow(rows[i], 5).Text;

                        break;
                    }
                }
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateModalShiftNameAndDate()
        {
            try
            {
                Assert.IsTrue(ModalShiftName.Text == ShiftName);
                Assert.IsTrue(ModalShiftDate.Text == $"{TodaysDate.DayOfWeek.ToString()} {TodaysDate.ToString("MM/dd/yy")} |");
                //Assert.IsTrue(ModalShiftTime.Text == ??);
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateModalMovementCountAndShiftId()
        {
            try
            {
                string shiftIdValue = _driver.FindElement(By.Id("ShiftId")).GetAttribute("value");
                Assert.IsTrue(shiftIdValue.Length == 6);

                string movementsValue = _driver.FindElement(By.Id("MovementsCount")).GetAttribute("value");
                IList<IWebElement> movementTabs = _driver.FindElements(By.CssSelector(".k-tabstrip-items .k-tabstrip-item"));
                int numberOfMovements = movementTabs.Count;
                Assert.AreEqual(movementsValue, numberOfMovements.ToString(), "The number of movement tabs does not match the expected value.");
                Assert.AreEqual(movementsValue, NumberOfMovements, "The number of movement tabs does not match the expected value.");
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateModalShiftStatus()
        {
            try
            {
                
                string badgeText = ModalShiftStatus.Text.Trim();

                string actualColorHex = UI.RgbaToHex(ModalShiftStatus.GetCssValue("background-color"));

                switch (badgeText)
                {
                    case "IN PROGRESS":
                        Assert.AreEqual("#537B35", actualColorHex, $"Background color for {badgeText} is not correct.");
                        break;

                    case "PENDING":
                        Assert.AreEqual("#E8E8E8", actualColorHex, $"Background color for {badgeText} is not correct.");
                        break;

                    case "EXPIRED":
                        Assert.AreEqual("#B2131A", actualColorHex, $"Background color for {badgeText} is not correct.");
                        break;

                    case "COMPLETE":
                        Assert.AreEqual("#537B35", actualColorHex, $"Background color for {badgeText} is not correct.");
                        break;

                    default:
                        Assert.Fail($"Unexpected badge text: {badgeText}");
                        break;
                }

            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateModalMovementGrid()
        {
            try
            {
                //Check driver information screen. verify  driver, role, phone , email details.
                string driverName = _driver.FindElement(By.Id("Name")).GetAttribute("value");
                Assert.That(driverName.Trim(), Is.EqualTo(ShiftDriver.Trim()));

                //Check movements is displayed below driver information . verify movements and vehicle information is displayed.
                //Assert.That(_driver.FindElement(By.XPath("/html/body/div[23]/div[1]/div/div[2]/div[2]/div/div[2]/div[1]/div[1]/div[1]/p")).Text, Is.EqualTo(ShiftName));

                //Check movements grid displays below vehicle information screen. verify all 4 columns
                //Assert.That(driver.FindElement(By.XPath("/html/body/div[23]/div[1]/div/div[2]/div[2]/div/div[2]/div[2]/div[2]/table/tbody/tr[1]/td[2]")).Text, Is.EqualTo("American Red Cross"));
                //Assert.That(driver.FindElement(By.XPath("/html/body/div[23]/div[1]/div/div[2]/div[2]/div/div[2]/div[2]/div[2]/table/tbody/tr[2]/td[2]")).Text, Is.EqualTo("IU HEALTH RILEY HOSPITAL FOR CHILDREN"));
                //Assert.That(driver.FindElement(By.XPath("/html/body/div[23]/div[1]/div/div[2]/div[2]/div/div[2]/div[2]/div[2]/table/tbody/tr[3]/td[2]")).Text, Is.EqualTo("3845_Indianapolis IN Distribution Site"));
                //Assert.That(driver.FindElement(By.XPath("/html/body/div[23]/div[1]/div/div[2]/div[2]/div/div[2]/div[2]/div[2]/table/tbody/tr[4]/td[2]")).Text, Is.EqualTo("American Red Cross"));

                //Check Add a movement in TMS screen is displayed at the bottom of the page. verify movement ID search box with unassigned movements table grid is displayed.
                Assert.That(_driver.FindElement(By.CssSelector(".mt-3 > .font-size-regular")).Text, Is.EqualTo("Add a Movement from TMS"));
                var elements = _driver.FindElements(By.XPath("//span[2]/input"));
                Assert.True(elements.Count > 0);
                Assert.That(_driver.FindElement(By.XPath("//div[2]/div/table/thead/tr/th")).Text, Is.EqualTo("Origin"));
                Assert.That(_driver.FindElement(By.XPath("//div[2]/div/table/thead/tr/th[2]")).Text, Is.EqualTo("Stops"));
                Assert.That(_driver.FindElement(By.XPath("//div[2]/div/table/thead/tr/th[4]")).Text, Is.EqualTo("Service Level"));
                Assert.That(_driver.FindElement(By.XPath("//div[2]/div/table/thead/tr/th[5]")).Text, Is.EqualTo("Pickup by"));
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }
        //

        //Movement
        public static void ValidateMovementName()
        {
            try
            {
                MovementSearchBar.EnterText(MovementName, true);

                var rows = UI.GetRowsFromGrid(MovementsGrid).ToArray();

                string[] movementNames = new string[rows.Length];

                IWebElement targetRow = null;

                for (int i = 0; i < rows.Length; i++)
                {
                    movementNames[i] = UI.GetColumnFromRow(rows[i], 0).Text;
                    targetRow = rows[i];
                }

                Assert.IsTrue(movementNames.Contains(MovementName));
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateMovementTypeDropDown()
        {
            try
            {

                var wait = new WebDriverWait(Driver.Instance, TimeSpan.FromSeconds(waitTime));

                // Wait for dropdown to be clickable and then get its options
                wait.Until(ExpectedConditions.ElementToBeClickable(MovementServiceLevelDropdown));
                var dropdownOptions = UI.GetDropdownOptions(MovementServiceLevelDropdown);

                Assert.IsTrue(dropdownOptions.Count == 3);

                // Click the first option
                UI.WaitForElement(dropdownOptions[0],waitTime).Click();

                // Now, since the DOM might have changed after clicking the option, re-fetch the first option before getting the span
                dropdownOptions = UI.GetDropdownOptions(MovementServiceLevelDropdown);
                var test = dropdownOptions[0].FindElement(By.TagName("span"));

                // Wait for grid to be visible
                wait.Until(ExpectedConditions.ElementToBeClickable(MovementsGrid));


                var rows = UI.GetRowsFromGrid(MovementsGrid);

                string movementTypeColumn;

                for (int i = 0; i < rows.Count; i++)
                {
                    // Wait for the cell's text to be non-empty
                    wait.Until(driver => UI.GetColumnFromRow(rows[i], 4).Text != "");
                    movementTypeColumn = UI.GetColumnFromRow(rows[i], 4).Text;
                    Assert.IsTrue(movementTypeColumn == "Scheduled");
                }
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateMovementCalendar()
        {
            try
            {
                // Input and filter by date range
                _driver.FindElement(By.XPath("//div[@id='currentShiftsDateRangePicker']/span/span/input")).Click();
                _driver.FindElement(By.LinkText($"{TodaysDate.Day.ToString()}")).Click();
                _driver.FindElement(By.LinkText($"{TodaysDate.AddDays(1).Day.ToString()}")).Click();
                _driver.FindElement(By.XPath("(//input[@type='text'])[4]")).Click();

                // Search by the formatted date
                string todayDate = DateTime.Now.ToString("MMddyy");
                var searchBox = _driver.FindElement(By.XPath("(//input[@type='text'])[4]"));
                searchBox.SendKeys(ShiftName);

                // Wait until search results appear
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(50));
                wait.Until(drv => drv.FindElements(By.XPath("//div[4]/div/div/div[2]")).Count > 0);
                searchBox.SendKeys(Keys.Enter);

                // Process and validate the search results

                pgOverview.ValidateShiftName();
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateMovementStatusColor()
        {
            try
            {

                var statusBadge = UI.GetColumnFromRow(UI.GetRowsFromGrid(MovementsGrid)[0], 5).FindElement(By.TagName("span"));

                string actualColorHex = UI.RgbaToHex(statusBadge.GetCssValue("background-color"));

                switch (statusBadge.Text)
                {
                    case "IN PROGRESS":
                        Assert.AreEqual("#537B35", actualColorHex, $"Background color for {statusBadge.Text} is not correct.");
                        break;

                    case "PENDING":
                        Assert.AreEqual("#6d6e70", actualColorHex, $"Background color for {statusBadge.Text} is not correct.");
                        break;

                    case "EXPIRED":
                        Assert.AreEqual("#B2131A", actualColorHex, $"Background color for {statusBadge.Text} is not correct.");
                        break;

                    case "COMPLETE":
                        Assert.AreEqual("#537B35", actualColorHex, $"Background color for {statusBadge.Text} is not correct.");
                        break;

                    default:
                        Assert.Fail($"Unexpected badge text: {statusBadge.Text}");
                        break;
                }
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateMovementDateAndTimeColumn()
        {
            try
            {
                var rows = UI.GetRowsFromGrid(MovementsGrid);

                string dateAndTime;

                for (int i = 0; i < rows.ToArray().Length; i++)
                {
                    dateAndTime = UI.GetColumnFromRow(rows[i], 1).Text;
                    Assert.IsTrue(dateAndTime.Contains(TodaysDate.ToString("dd")) || dateAndTime.Contains(TodaysDate.AddDays(1).ToString("dd")));
                }


            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateMovementOrder(string direction)
        {
            try
            {
                var rows = UI.GetRowsFromGrid(MovementsGrid).ToArray();



                switch (direction)
                {
                    case "Ascending":
                        Assert.IsTrue(UI.IsGridSortedAscending(rows, 1));
                        break;

                    case "Descending":
                        Assert.IsTrue(UI.IsGridSortedDescending(rows, 1));
                        break;

                    default:
                        Assert.Fail($"Invalid parameter: {direction}");
                        break;
                }

            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateMovementSearchBars()
        {
            try
            {
                //UI.EnterText(DateSearchBar, TodaysDate.ToString("MM/dd/yy"),true);

                //var rows = UI.GetRowsFromGrid(ShiftGrid);

                //string target;

                //UI.EnterText(MovementStops, "2", true);

                //rows = UI.GetRowsFromGrid(MovementsGrid);

                //for (int i = 0; i < rows.ToArray().Length; i++)
                //{
                //    target = UI.GetColumnFromRow(rows[i], 3).Text;
                //    Assert.IsTrue(target.Contains("3"));
                //}

                //UI.EnterText(MovementStops, "", true);

                //UI.EnterText(MovementServiceLevel, "Scheduled", true);

                //rows = UI.GetRowsFromGrid(MovementsGrid);

                //for (int i = 0; i < rows.ToArray().Length; i++)
                //{
                //    target = UI.GetColumnFromRow(rows[i], 4).Text;
                //    Assert.IsTrue(target.Contains("Scheduled"));
                //}

                //UI.EnterText(MovementServiceLevel, "", true);

            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }
        //

        public static void ValidateModalDriverEdits()
        {
            try
            {

                UI.WaitForElement(ModalMovementId, waitTime);
                Assert.IsTrue(ModalMovementId.Text.Contains("M-"));

                Assert.IsTrue(ModalServiceLevel.Text == ShiftType);

                UI.WaitForElement(ModalVehicleEditButton, waitTime).Click();

                //First Try
                Thread.Sleep(1000);
                UI.WaitForElement(ModalAssignVehicleDropDown, waitTime);
                var options = UI.GetDropdownOptions(ModalAssignVehicleDropDown);
                Thread.Sleep(1000);
                var vehicle = options[1].Text;
                options[1].Click();
                Thread.Sleep(1000);
                UI.WaitForElement(ModalVehicleEditSaveButton, waitTime).Click();
                Thread.Sleep(1000);
                Assert.IsTrue(ModalVehicle.Text == vehicle);
                //

                UI.WaitForElement(ModalVehicleEditButton, waitTime).Click();

                //Second Try
                Thread.Sleep(1000);
                UI.WaitForElement(ModalAssignVehicleDropDown, waitTime);
                options = UI.GetDropdownOptions(ModalAssignVehicleDropDown);
                Thread.Sleep(1000);
                var vehicleType = options[options.ToArray().Length-1].Text;
                options[1].Click();
                Thread.Sleep(1000);
                UI.WaitForElement(ModalVehicleEditSaveButton, waitTime).Click();
                Thread.Sleep(1000);
                Assert.IsTrue(ModalVehicle.Text == vehicle);
                //
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }

        }

    }
}

