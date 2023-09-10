using Microsoft.VisualStudio.CodeCoverage;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;

namespace APITesting.TestSuites
{
	public class OpenShifts : BaseTestAPI
	{
        [Test, Order(1)]
        public async Task PostSignUpForShifts()
        {
            var endpoint = "/api/OpenShifts/SignUpForShift";

            var requestForm = new Dictionary<string, object>
            {
                { "shiftId", "3" },
                {"userId","3" }
            };

            //await API.PostToEndpoint(endpoint, requestForm);
        }

        [Test]
        public async Task GetOpenShiftsForMember()
        {
            var endpoint = "/api/OpenShifts/GetOpenShiftsForMember";

            string today = DateTime.Today.ToString("MM/dd/yyyy");

            var parameters = new Dictionary<string, string>
            {
                { "siteId", "25" },
                { "memberId", "21707" },
                { "date", $"{today}" }
            };

            var definitions = new List<string>
            {
                {"shiftId" }
            };

            //await API.AssertKeysNotNull(endpoint, parameters, definitions);
        }

        [Test]
        public async Task GetOpenShiftById()
        {
            var endpoint = "/api/OpenShifts/GetOpenShiftById";

            var parameters = new Dictionary<string, string>
            {
                { "shiftId", "3" }
            };

            var definitions = new Dictionary<string, object>
            {
                {"message", "Open shift not found"}
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        public async Task GetDatesWithOpenShifts()
        {
            var endpoint = "/api/OpenShifts/GetDatesWithOpenShifts";

            var parameters = new Dictionary<string, string>
            {
                //Not sure what values to put here.
                { "siteId", "3" },
                {"memberId","53" }

            };

            var definitions = new Dictionary<string, object>
            {
                {"message", "Open shift not found"}
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }
    }
}

