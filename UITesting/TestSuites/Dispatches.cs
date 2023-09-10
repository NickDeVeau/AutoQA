using NUnit.Framework;
using UITesting.Pages;
using UITesting;

namespace UITesting.TestSuites
{
    [TestFixture, Order(3)]
    [Category("Dispatches")]
    //[Ignore("Ignore test fixture")]
    public class Dispatches : BaseTestUI
    {
        [Test]
        [Ignore("Site In Development")]
        [Category("Regression")]        
        public void VerifyDispatchItems()
        {
            pgDispatches.Validate_DispatchItems(_configuration.GetSection("AppInfo:Site").Value);
        }

        [Test]
        [Category("Sanity")]
        [Category("Regression")]
        //[Ignore("Ignore test")]
        public void VerifyDispatchView()
        {
            int itemNumber = -1;

            pgDispatches.Validate_DispatchView(itemNumber, _configuration.GetSection("AppInfo:Site").Value);
        }

        [Test]
        [Ignore("Site In Development")]
        [Category("Regression")]
        public void VerifyDispatchStatus()
        {            
            pgDispatches.Validate_Status(_configuration.GetSection("AppInfo:Site").Value);
        }
    }
}

