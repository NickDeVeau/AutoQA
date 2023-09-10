using NUnit.Framework;

namespace APITesting.TestSuites
{
	public class Newsfeed : BaseTestAPI
	{
        [Test]
        public async Task GetNewsfeedForMember()
        {
            var endpoint = "/api/Newsfeed/GetNewsfeedForMember";

            var parameters = new Dictionary<string, string>
            {
                { "memberId", "21707" }
            };

            var keys = new List<string>
            {
                "createdTimeUtc",
                "text"
            };

            await API.AssertKeysNotNull(endpoint, parameters,keys);
        }
    }
}

