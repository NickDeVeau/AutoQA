using NUnit.Framework;

namespace APITesting.TestSuites
{
    public class Overview : BaseTestAPI
    {
        string shiftId;

        [Test]
        public async Task GetRecentShifts()
        {
            var endpoint = "/api/Overview/admin/GetRecentShifts";

            var parameters = new Dictionary<string, string>
            {
                { "siteId", "25" },
                {"from","07/10/2020" },
                {"to","07/20/2020" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "statusCode", 200}
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        public async Task GetUrgentRequests()
        {
            var endpoint = "/api/Overview/admin/GetUrgentRequests";

            var parameters = new Dictionary<string, string>
            {
                { "siteId", "25" },
                {"lastHourOffset","5" },
                {"memberType","2" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "statusCode",200 }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        public async Task GetMovements()
        {
            var endpoint = "/api/Overview/admin/GetMovements";

            var parameters = new Dictionary<string, string>
            {
                { "siteId", "25" },
                { "from", "1/1/2020" },
                { "to", "1/1/2024" }
            };

            var definitions = new List<string>
            {
                { "name" }
            };

            await API.AssertKeysNotNull(endpoint, parameters, definitions);

            shiftId = await API.FindNestedValue(endpoint, parameters, "shiftName", "M-1539596", "shiftId", false);
        }

        [Test]
        public async Task GetOverviewShiftDetails()
        {
            var endpoint = "/api/Overview/admin/GetOverviewShiftDetails";

            var parameters = new Dictionary<string, string>
            {
                { "shiftId", shiftId }
            };

            var definitions = new List<string>
            {
                { "shiftId" }
            };

            await API.AssertKeysNotNull(endpoint, parameters, definitions);
        }
    }
}

