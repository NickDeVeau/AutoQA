using NUnit.Framework;
using UITesting.Pages;

namespace UITesting.TestSuites
{

    [TestFixture, Order(2)]
    [Category("Home")]
    public class Home : BaseTestUI
    {
        [Test, Order(3)]
        [Category("Sanity")]
        [Category("Regression")]
        public void VerifyDistributionSite()
        {            
            string[] siteNames =
            {
                _configuration.GetSection("AppInfo:Site").Value
            };

            pgHome.Validate_SiteName(siteNames);         
        }

        [Test, Order(5)]
        [Category("Regression")]
        [Category("Pipeline")]
        public void VerifyMenuItems()
        {            
            string[] menuItems = { "Overview", "Dispatches", "Driver newsfeed", 
                // "Directories", "Locations", "Users", "Regional Contacts", "Vehicles",
                // "Settings", "Admin Portal",
                // "Reporting", " Metrics", "Order History",
                // "Submit feedback"                
            };

            pgHome.Validate_MenuItems(menuItems);            
        }

        [Test, Order(4)]
        [Category("Regression")]
        public void VerifyPages()
        {            
            string[] pageItems = {"Overview", "Dispatches", "Driver newsfeed", "Submit feedback"};

            pgHome.ValidateMenuFunctionality(pageItems);            
        }
    }
}

