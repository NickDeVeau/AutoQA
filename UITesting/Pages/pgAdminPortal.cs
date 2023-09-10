using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using AventStack.ExtentReports;

namespace UITesting.Pages
{
    public class Audience : BaseTestUI
    {
        public string AudienceSite { get; }
        public string AudienceName { get; }
        public string[] AudienceMembers { get; }

        public Audience(string name)
        {
            try
            {
                IWebDriver driver = Driver.Instance;

                // Find the table element that contains the rows
                IWebElement table = driver.FindElement(By.XPath("//div[@id='audiencesGrid']/table"));

                // Find the row element with the specific "Name" text
                IWebElement row = table.FindElement(By.XPath($"//tr[contains(td[2], '{name}')]"));

                // Extract the site, name, and audience members from the row
                IWebElement siteCell = row.FindElement(By.XPath("./td[1]"));
                IWebElement nameCell = row.FindElement(By.XPath("./td[2]"));
                IWebElement membersCell = row.FindElement(By.XPath("./td[3]"));

                AudienceSite = siteCell.Text;
                AudienceName = nameCell.Text;
                AudienceMembers = membersCell.Text.Split(", ");
            }
            catch (NoSuchElementException)
            {
                // Audience table or row not found, initialize with default values
                AudienceSite = null;
                AudienceName = null;
                AudienceMembers = null;
            }
        }

        public void DeleteAudience()
        {
            try
            {
                IWebDriver driver = Driver.Instance;

                var btnConfirmDeletePath = "/html/body/div[5]/div[3]/button[1]";
                var btnDeleteAudiencePath = "/html/body/div[1]/div/div/div[2]/main/div/div/div[2]/div/table/tbody/tr/td[4]/button[2]";

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));

                // Find the table element that contains the rows
                IWebElement table = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@id='audiencesGrid']/table")));

                // Find the row element with the specific audience name
                IWebElement row = wait.Until(ExpectedConditions.ElementExists(By.XPath($"//tr[contains(td[2], '{AudienceName}')]")));

                // Find the delete button within the row and click it
                IWebElement btnDeleteAudience = wait.Until(ExpectedConditions.ElementToBeClickable(row.FindElement(By.CssSelector("button.k-grid-Delete"))));
                btnDeleteAudience.Click();

                // Wait for the confirmation delete button to be clickable
                IWebElement btnConfirmDelete = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(btnConfirmDeletePath)));
                btnConfirmDelete.Click();
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }
    }

    public class pgAdminPortal : BaseTestUI
    {
        public static void ValidateAdminPortSettingsPage(string audienceName, string members, string siteName = null)
        {
            try
            {
                
                pgLogin.Login(_configuration.GetSection("UserInfo:sEmail").Value, _configuration.GetSection("UserInfo:sPwd").Value);

                pgHome.selectDistributionSite(siteName);

                pgHome.OpenMenuPage("Admin portal");

                IWebDriver driver = Driver.Instance;

                IWebElement pgHeaderText = driver.FindElement(By.XPath("//main/h2"));
                IWebElement dsHeaderText = driver.FindElement(By.XPath("//main/div/div/div[1]/h5"));
                IWebElement dsInfoText = driver.FindElement(By.XPath("//main/div/div/div[1]/p[1]"));
                IWebElement dsMaxResTimeText = driver.FindElement(By.XPath("//main/div/div/div[1]/p[2]"));
                IWebElement dsMaxDriTimeText = driver.FindElement(By.XPath("//main/div/div/div[1]/p[3]"));
                IWebElement dsSaveBtn = driver.FindElement(By.XPath("//*[@id=\"dispatchDriveSettingsForm\"]/div[3]/button"));
                IWebElement maHeaderText = driver.FindElement(By.XPath("//main/div/div/div[2]/h5"));
                IWebElement maInfoText = driver.FindElement(By.XPath("//main/div/div/div[2]/p[1]"));
                IWebElement maAllVolText = driver.FindElement(By.XPath("//main/div/div/div[2]/p[2]"));
                IWebElement maUrgDelDriText = driver.FindElement(By.XPath("//main/div/div/div[2]/p[3]"));
                IWebElement addNewAudBtn = driver.FindElement(By.ClassName("k-grid-add"));

                Assert.AreEqual("Admin Portal Settings", pgHeaderText.Text);
                Assert.AreEqual("Dispatch Settings", dsHeaderText.Text);
                Assert.IsTrue(dsInfoText.Text.Contains("* these settings will apply to your entire site"));
                Assert.IsTrue(dsMaxResTimeText.Text.Contains("Max response time for volunteer to accept dispatch request (minutes)"));
                Assert.IsTrue(dsMaxDriTimeText.Text.Contains("Max drive time for nearby volunteer (minutes)"));
                Assert.IsTrue(dsSaveBtn.Displayed, "Dispatch setting Save button missing");

                Assert.AreEqual("Manage Audiences", maHeaderText.Text);
                Assert.IsTrue(maInfoText.Text.Contains("This distribution site has 2 default Audience groups that will be created and maintained automatically by the system:"));
                Assert.IsTrue(maAllVolText.Text.Contains("Includes every active volunteer driver within the base site and is automatically maintained as volunteers are added or subtracted from the system."));
                Assert.IsTrue(maUrgDelDriText.Text.Contains("Every active volunteer driver who currently has indicated one or more segment of time in the Urgent Availability section of the Driver App. This group is the default for all STAT, ASAP, and Urgent shipments."));
                Assert.IsTrue(addNewAudBtn.Displayed, "Add New audience button missing");
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateAddAudience(string audienceName, string[] members, string siteName)
        {
            try
            {
                pgLogin.Login(_configuration.GetSection("UserInfo:sEmail").Value, _configuration.GetSection("UserInfo:sPwd").Value);

                pgHome.selectDistributionSite(siteName);

                pgHome.OpenMenuPage("Admin portal");

                IWebDriver driver = Driver.Instance;

                IWebElement pgHeaderText = driver.FindElement(By.XPath("//main/h2"));
                IWebElement maHeaderText = driver.FindElement(By.XPath("//main/div/div/div[1]/h5"));
                IWebElement addNewAudBtn = driver.FindElement(By.ClassName("k-grid-add"));

                Assert.IsTrue(addNewAudBtn.Displayed, "Add New audience button missing");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));

                IWebElement audiencesGrid = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("audiencesGrid")));

                wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@id='audiencesGrid']/table/tbody/tr")));

                if (CheckAudience(audienceName))
                {
                    GetAudience(audienceName).DeleteAudience();
                    AddNewAudience(audienceName, members);
                }
                else
                {
                    AddNewAudience(audienceName, members);
                }
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void AddNewAudience(string audienceName, string[] members)
        {
            try
            {
                IWebDriver driver = Driver.Instance;
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));

                // Find the "Add New" button and click it
                IWebElement addNewButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@id='audiencesGrid']//button[contains(@class, 'k-grid-add')]")));
                addNewButton.Click();

                // Find the input fields and fill them with the provided values
                IWebElement nameField = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='audiencesGrid']//input[@name='AudienceName']")));
                nameField.SendKeys(audienceName);

                // Find the dropdown arrow element and wait for it to be clickable
                IWebElement dropdownArrow = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='SelectedMembers_taglist']/input")));
                dropdownArrow.Click();

                // Wait for the dropdown animation to finish
                System.Threading.Thread.Sleep(1000);  // Adjust this delay as needed

                // Find the dropdown list and wait for it to be visible
                IWebElement dropdownList = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='SelectedMembers_listbox']")));

                // Wait for the specific dropdown option to be clickable
                foreach (string member in members)
                {
                    bool isOptionSelected = false;

                    // Retry selecting the option if it fails
                    for (int attempt = 1; attempt <= 3; attempt++)
                    {
                        try
                        {
                            IWebElement option = dropdownList.FindElement(By.XPath($"//li[normalize-space()='{member}']"));
                            option.Click();
                            isOptionSelected = true;
                            break;
                        }
                        catch (NoSuchElementException)
                        {
                            // Wait for a short time and retry
                            System.Threading.Thread.Sleep(500);
                        }
                    }

                    // If the option selection fails after multiple attempts, log an error and throw an exception
                    if (!isOptionSelected)
                    {
                        test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                        throw new Exception("Failed to select a member from the dropdown.");
                    }
                }

                // Save the newly added audience row
                IWebElement saveOption = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@id='audiencesGrid']//button[contains(@class, 'k-grid-update')]")));
                saveOption.Click();

                // Wait for the save operation to complete
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@id='audiencesGrid']//button[contains(@class, 'k-grid-update')][not(@disabled)]")));
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }



        public static Audience GetAudience(string site)
        {
            IWebDriver driver = Driver.Instance;
            return new Audience(site);
        }

        public static bool CheckAudience(string site)
        {
            IWebDriver driver = Driver.Instance;
            Audience audience = new Audience(site);
            return audience.AudienceName != null;
        }
    }
}
