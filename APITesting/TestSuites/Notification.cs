using NUnit.Framework;

namespace APITesting.TestSuites
{
    public class Notification : BaseTestAPI
    {
        [Test]
        public async Task Post()
        {
            var endpoint = "/api/Notification/SendTestNotification";

            var parameters = new Dictionary<string, string>
                    {
                        {"tagId", "87100CA9-A662-40C8-92C4-4AF2F6329D8F" },
                        {"title","TestTitle" },
                        {"message","TestMessage"}
                    };

            await API.PostToEndpointWithParams(endpoint, parameters);
        }
    }
}

