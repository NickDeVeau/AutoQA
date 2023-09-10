using NUnit.Framework;
using UITesting.Pages;

namespace UITesting.TestSuites
{
    [TestFixture, Order(5)]
    [Category("AdminPortalSetting")]
    public class AdminPortal : BaseTestUI
    {
        [Test, Order(1)]
        [Category("Sanity")]
        public void VerifyAdminPortSettingPage()
        {
            string siteName = "San Diego CA Distribution Site";
            string audienceName = "Auto Test";
            string members = "Testacctwo Testacctwo";
            //string memberType = "Paid" or "Volunteer"";

            pgAdminPortal.ValidateAdminPortSettingsPage(audienceName, members, siteName);
        }

        [Test]
        [Category("Regression")]
        public void VerifyAddNewAudience()
        {
            string siteName = "1665_Columbus OH Distribution Site";            
            string audienceName = "Test";
            string[] members = new string[] { "Testacctwo Testacctwo", "Prabu S" };

            pgAdminPortal.ValidateAddAudience(audienceName, members, siteName);
        }
    }
}

