using NUnit.Framework;

namespace APITesting.TestSuites
{
	public class MyShifts : BaseTestAPI
	{
        [Test]
        
        public async Task GetShiftDetails()
        {
            var endpoint = "/api/MyShifts/GetShiftDetails";

            var parameters = new Dictionary<string, string>
            {
                { "shiftId", "19122" }
            };

            var definitions = new List<string>
            {
                {
                    "siteName"
                }
            };

            await API.AssertKeysNotNull(endpoint, parameters, definitions);
        }

        [Test]
        
        public async Task GetLaterShifts()
        {
            var endpoint = "/api/MyShifts/GetLaterShifts";

            var parameters = new Dictionary<string, string>
            {
                { "memberId", "21707" },
                {"siteId", "25"}
            };

            var definitions = new Dictionary<string, object>
            {
                { "statusCode", 204}
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        
        public async Task GetTodayShifts()
        {
            var endpoint = "/api/MyShifts/GetTodayShifts";

            var parameters = new Dictionary<string, string>
            {
                { "memberId", "21707" },
                {"siteId","25" }
            };

            var definitions = new Dictionary<string, object>
            {
                {"statusCode", 200},
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        [Ignore("error")]
        
        public async Task GetActiveShift()
        {
            var endpoint = "/api/MyShifts/GetActiveShift";

            var parameters = new Dictionary<string, string>
            {
                { "memberId", "21707" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "statusCode", 204 }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }
    }
}

