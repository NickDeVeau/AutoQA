using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace APITesting.TestSuites
{
    public class SiteSettings : BaseTestAPI
    {
        [Test]
        
        public async Task GetSiteSettings()
        {
            var endpoint = "/api/admin/SiteSettings";

            var parameters = new Dictionary<string, string>
            {
                { "siteId", "25" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "statusCode", 200 }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        
        [Ignore("Under Development")]
        public async Task PostSiteSettings()
        {
            var endpoint = "/api/admin/SiteSettings";

            var requestForm = new Dictionary<string, object>
            {
                {"siteSettingId", "9"},
                {"siteId", "25"},
                {"siteSettingTypeId", "0"},
                {"autoDispatch", "True"},
                {"responseSTAT", "1"},
                {"responseASAP", "1"},
                {"responseOther", "1"},
                {"maxDriveSTAT", "2"},
                {"maxDriveASAP", "2"},
                {"maxDriveOther", "2"},
            };

            await API.PostToEndpoint(endpoint, requestForm);
        }

    }
}
