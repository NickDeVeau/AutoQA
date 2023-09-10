using NUnit.Framework;
using UITesting.Pages;
using UITesting;

namespace UITesting.TestSuites
{
    [TestFixture, Order(4)]
    [Category("RegionalContacts")]
    public class RegionalContacts : BaseTestUI
    {        
        [Test]
        
        [Category("Sanity")]
        public void VerifyRegionaContactPage()
        {
            var siteName = "";

            pgRegionalContacts.ValidateRegContactPage(_configuration.GetSection("AppInfo:Site").Value);
        }
        [Test]
        
        [Category("Regression")]
        public void VerifyAddNewRegionalContacts()
        {
            string siteName = "";
            string contactType = "Other";
            string FName = "Automation";
            string LName = "Test";
            string email = _configuration.GetSection("UserInfo:sEmail").Value;
            string phone = "9998887777";
            var sms = "No";

            pgRegionalContacts.ValidateRegionalContact(contactType, (FName+" "+LName), email, phone, sms);
        }

        [Test]
        
        [Category("Regression")]
       
        public void VerifyEditRegionalContacts()
        {
            string siteName = "";
            string contactType = "Other";
            string FName = "Automation";
            string LName = "Test";
            string email = "user1.innovation@redcross.org";
            string phone = "9998887777";
            var sms = "No";

            pgRegionalContacts.ValidateRegionalContact(contactType, (FName + " " + LName), email, phone, sms);
        }


    }
}

