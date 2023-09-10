using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using AventStack.ExtentReports;

namespace UITesting.Pages
{
    public class Contact : BaseTestUI
    {
        public string ContactType { get; }
        public string ContactName { get; }
        public string ContactPhone { get; }
        public string ContactSMS { get; }

        public Contact(string name)
        {
            try
            {
                IWebDriver driver = Driver.Instance;

                // Find the table element that contains the rows
                IWebElement table = driver.FindElement(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div/div/div/table"));

                // Find the row element with the specific "Name" text
                IWebElement row = table.FindElement(By.XPath(".//tr[td[contains(text(), '" + name + "')]]"));

                // Extract the site, name, and audience members from the row
                IWebElement typeCell = row.FindElement(By.XPath(".//td[3]")); // 1st visible column is Type
                IWebElement nameCell = row.FindElement(By.XPath(".//td[4]")); // 2nd visible column is Name
                IWebElement phoneCell = row.FindElement(By.XPath(".//td[6]")); // 3rd visible column is Phone
                IWebElement smsCell = row.FindElement(By.XPath(".//td[7]")); // 4th visible column is SMS

                ContactType = typeCell.Text;
                ContactName = nameCell.Text;
                ContactPhone = phoneCell.Text;
                ContactSMS = smsCell.Text;
            }
            catch (NoSuchElementException)
            {
                // Contact table or row not found, initialize with default values
                ContactType = null;
                ContactName = null;
                ContactPhone = null;
                ContactSMS = null;
            }
        }


        public void DeleteContact()
        {
            try
            {
                IWebDriver driver = Driver.Instance;

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(waitTime));

               

                // Find the row element with the specific contact name
                IWebElement row = wait.Until(ExpectedConditions.ElementExists(By.XPath($".//tr[td[contains(text(), '{ContactName}')]]")));  // replace 'ContactName' with your actual contact name

                IWebElement btnDeleteContact = wait.Until(ExpectedConditions.ElementToBeClickable(row.FindElement(By.XPath(".//td[@class='k-command-cell' and @role='gridcell']/button[contains(@class,'k-grid-Delete')]"))));
                btnDeleteContact.Click();

                // Assuming your confirmation pop-up also uses Kendo UI and has a "Yes" button
                IWebElement btnConfirmDelete = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[contains(text(), 'Yes')]")));
                btnConfirmDelete.Click();
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

    }
    public class pgRegionalContacts : BaseTestUI
    {
        public static void ValidateRegContactPage(string siteName)
        {          
            try
            {
                pgLogin.Login(
                 _configuration.GetSection("UserInfo:sEmail").Value,
                 _configuration.GetSection("UserInfo:sPwd").Value
                );

                pgHome.selectDistributionSite(siteName);

                pgHome.OpenMenuPage("Regional Contacts");

                IWebDriver _driver = Driver.Instance;

                IWebElement infoText = _driver.FindElement(By.XPath("//main/div/div/div/p"));
                IWebElement headerText = _driver.FindElement(By.XPath("//main/div/div/div/h5"));
                IWebElement addNewBtn = _driver.FindElement(By.ClassName("k-grid-add"));

                Assert.IsTrue(addNewBtn.Displayed);
                Assert.AreEqual(headerText.Text, "Manage Regional Contacts");
                Assert.IsTrue(infoText.Text.Contains("These are the contacts provided to the drivers for your distribution site, " + siteName));                
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static void ValidateRegionalContact(string contactType, string contactName, string email, string contactNumber, string contactSMS)
        {
            IWebDriver _driver = Driver.Instance;

            pgLogin.Login(
              _configuration.GetSection("UserInfo:sEmail").Value,
              _configuration.GetSection("UserInfo:sPwd").Value
            );

            pgHome.selectDistributionSite("5365_Baltimore MD Distribution Site");
            pgHome.OpenMenuPage("Regional Contacts");

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));

            wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div/div/div/table")));

            wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[2]/div/div/div[2]/main/div/div/div/div/table/tbody/tr")));

            if (CheckContact(contactName))
            {
                GetContact(contactName).DeleteContact();
                AddRegionContact(contactType,contactName, email, contactNumber,contactSMS);
            }
            else
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                AddRegionContact(contactType,contactName, email, contactNumber,contactSMS );
            }
        }


        public static void AddRegionContact(string contactType, string contactName, string email,string phoneNumber, string contactSMS)
        {
            try
            {
                IWebDriver _driver = Driver.Instance;

                var addContactPath = "/html/body/div[2]/div/div/div[2]/main/div/div/div/div/div[1]/button";
                var dpnContactPath = "/html/body/div[2]/div/div/div[2]/main/div/div/div/div/table/tbody/tr/td[3]/span";
                var contactListPath = "/html/body/div[6]/div/div/div[2]/ul";
                var inputFieldId = "SiteContactName";
                var emailFieldId = "SiteContactEmail";
                var numberFieldId = "SiteContactPhone";
                var dpnSMSPath = "/html/body/div[2]/div/div/div[2]/main/div/div/div/div/table/tbody/tr/td[7]/span[1]";
                var SMSListPath = "/html/body/div[7]/div/div/div[2]/ul";
                var submitPath = "/html/body/div[2]/div/div/div[2]/main/div/div/div/div/table/tbody/tr[1]/td[8]/button[1]";

                

               
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTime));

                IWebElement btnAddContact = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(addContactPath)));
                btnAddContact.Click();

                IWebElement dpnContact = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(dpnContactPath)));
                dpnContact.Click();

                IWebElement contactParent = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(contactListPath)));
                IList<IWebElement> contactOptions = contactParent.FindElements(By.XPath(".//*"));

                foreach (IWebElement option in contactOptions)
                {
                    if (option.Text.Contains(contactType))
                    {
                        option.Click();
                        break;
                    }
                }

                IWebElement nameField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id(inputFieldId)));
                nameField.Clear();
                nameField.SendKeys(contactName);

                IWebElement emailField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id(emailFieldId)));
                emailField.Clear();
                emailField.SendKeys(email);

                IWebElement numberField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id(numberFieldId)));
                numberField.Clear();
                numberField.Click();
                numberField.SendKeys(phoneNumber);

                IWebElement dropdown = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(dpnSMSPath)));
                dropdown.Click();

                IWebElement SMSParent = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SMSListPath)));
                IList<IWebElement> SMSOptions = SMSParent.FindElements(By.XPath(".//*"));

                foreach (IWebElement option in SMSOptions)
                {
                    if (option.Text.Contains(contactSMS))
                    {
                        option.Click();
                        break;
                    }
                }
                IWebElement btnSubmit = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(submitPath)));
                btnSubmit.Click();
            }
            catch
            {
                test.Log(Status.Fail, test.AddScreenCaptureFromPath(BaseTestUI.TakeScreenshot(TestContext.CurrentContext.Test.Name), TestContext.CurrentContext.Test.Name) + "Test Failed: " + string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace));
                throw;
            }
        }

        public static Contact GetContact(string name)
        {
            IWebDriver driver = Driver.Instance;
            return new Contact(name);
        }

        public static bool CheckContact(string name)
        {
            IWebDriver driver = Driver.Instance;
            Contact contact = new Contact(name);
            return contact.ContactName != null;
        }

        

    }
}
