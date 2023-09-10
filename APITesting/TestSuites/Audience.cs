using NUnit.Framework;

namespace APITesting.TestSuites
{
    public class Audience : BaseTestAPI
    {
        public int audienceId;

        [Test, Order(1)]
        
        public async Task PostAudience()
        {
            try
            {
                var postEndpoint = "/api/Audience";
                var getEndpoint = "/api/Audience/siteAudiences";

                var getParameters = new Dictionary<string, string>
            {
                { "siteId", "25" },
            };

                var getDefinitions = new Dictionary<string, object>
            {
                { "name", "APITEST" }
            };

                var postRequestForm = new Dictionary<string, object>
            {
                { "siteId", "25" },
                { "audienceName", "APITEST"},
                {"userName","string"},
                { "memberIds",new List<int> {53}}
            };

                var defExists = await API.VerifyDefinitionExists(getEndpoint, getParameters, getDefinitions, false);
                if (defExists != true)
                {

                    await API.PostToEndpoint(postEndpoint, postRequestForm);
                }

                audienceId = Int32.Parse(await API.FindNestedValue(getEndpoint, getParameters, "name", "APITEST", "id", true));
            }
            catch (Exception ex)
            {
                // Log the exception, or even rethrow it with additional information:
                throw new Exception("An error occurred during PostAudience.", ex);
            }
        }

        [Test, Order(2)]
        
        public async Task GetSiteAudiences()
        {
            var endpoint = "/api/Audience/siteAudiences";

            var parameters = new Dictionary<string, string>
            {
                { "siteId", "25" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "statusCode", 200 }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions, true);

            audienceId = int.Parse(await API.FindNestedValue(endpoint,parameters, "name", "APITEST", "id"));

        }

        [Test, Order(3)]
        
        public async Task PutAudience()
        {
            var endpoint = "/api/Audience";

            var requestForm = new Dictionary<string, object>
            {
                { "audienceId", audienceId },
                {"audienceName", "APITEST" },
                {"userName","string"},
                { "memberIds",new List<int> {53,52}}
            };


            await API.PutToEndpoint(endpoint, requestForm);
        }

        //[Test, Order(4)]
        //public async Task DeleteAudience()
        //{
        //    var endpoint = "/api/Audience";

        //    var requestForm = new Dictionary<string, string>
        //    {
        //        { "id", $"{audienceId}" }
        //    };

        //    await API.DeleteFromEndpoint(endpoint, requestForm);
        //}
    }
}

