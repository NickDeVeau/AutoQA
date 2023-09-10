using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace APITesting.TestSuites
{
    public class SiteContact : BaseTestAPI
    {
        public int siteContactId;

        [Test,Order(1)]
        public async Task PostSiteContact()
        {
            var endpoint = "/api/admin/SiteContact";

            var requestForm = new Dictionary<string, object>
            {
                {"siteId", "25"},
                {"siteContactName", "TestContact"},
                {"siteContactPhone", "123-123-1234"},
                {"siteContactEmail", "user1.innovation@redcross.org"},
                {"userName","TestUserName" },
                {"isSmsEnabled", "True"},
                {"siteContactTypeId", "1"},
            };


            await API.PostToEndpoint(endpoint, requestForm);
        }

        [Test, Order(2)]
        public async Task GetSiteContactsBySiteId()
        {
            var endpoint = "/api/admin/SiteContact/GetSiteContactsBySiteId";

            var parameters = new Dictionary<string, string>
            {
                //Not sure what values to put here.
                { "siteId", "3" }

            };

            var definitions = new Dictionary<string, object>
            {
                { "siteContactName", "John Ryan" },
                { "siteId", "3" }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test, Order(3)]
        public async Task GetSiteContactTypes()
        {
            var endpoint = "/api/admin/SiteContact/siteContactTypes";

            var parameters = new Dictionary<string, string>
            {
                //None
            };

            var definitions = new Dictionary<string, object>
            {
                     {"description", "Transportation Coordinator" },
                     {"statusCode",200 }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test, Order(5)]
        public async Task GetSiteContact()
        {
            var endpoint = "/api/admin/SiteContact";

            var parameters = new Dictionary<string, string>
            {
                {"id", $"{siteContactId}" }
            };

            var definitions = new Dictionary<string, object>
            {
                {"siteContactName", "TestContact"}
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }


        [Test, Order(6)]
        public async Task PutSiteContact()
        {
            var endpoint = "/api/admin/SiteContact";

            var requestForm = new Dictionary<string, object>
            {
                { "siteId",25},
                { "siteContactName", "TestContact" },
                { "siteContactPhone", "123-123-2234"},
                {"siteContactEmail","user1.innovation@redcross.org" },
                { "isSmsEnabled", "false"},
                {"userName","TestUserName" },
                { "siteContactTypeId", 3},
                {"siteContactID",siteContactId }
            };


            await API.PutToEndpoint(endpoint, requestForm);

        }

        [Test, Order(7)]
        public async Task DeleteSiteContact()
        {
            var endpoint = "/api/admin/SiteContact";

            var parameters = new Dictionary<string, string>
                    {
                        { "id", $"{siteContactId}" }
                    };

            await API.DeleteFromEndpoint(endpoint, parameters);
        }

        [Test, Order(4)]
        public async Task GetSiteContactsId()
        {
            var endpoint = "/api/Contact/GetSiteContactsBySiteId";

            var parameters = new Dictionary<string, string>
    {
        { "siteId", "25" }
    };

            // Use the API to access the endpoint with the given parameters
            var content = await API.GetJsonFromEndpoint(endpoint, parameters);

            // Parse the string into a JObject
            JObject jObject = JObject.Parse(content);

            // Get the content array
            JArray? contentArray = (JArray?)jObject["content"];

            // Find the first object that matches the criteria
            JObject? targetObject = contentArray?.Children<JObject>()
                                         .FirstOrDefault(o => o["siteContactName"]?.ToString() == "TestContact");

            // If the target object was found
            if (targetObject != null)
            {
                // Get the siteContactId from the target object
                string? siteContactIdString = targetObject["siteContactId"]?.ToString();

                // If the siteContactId was found and is a valid integer
                if (siteContactIdString != null && int.TryParse(siteContactIdString, out int siteContactIdGet))
                {
                    // Assign the siteContactId to the external variable
                    siteContactId = siteContactIdGet;
                }
            }
        }
    }
}
