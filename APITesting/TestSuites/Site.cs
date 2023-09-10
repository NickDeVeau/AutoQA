using NUnit.Framework;

namespace APITesting.TestSuites
{
    public class Site : BaseTestAPI
    {
        [Test]
        public async Task GetMemberSitesByemberId()
        {
            var endpoint = "/api/Site/GetMemberSitesByMemberId";

            var parameters = new Dictionary<string, string>
            {
                //Not sure what values to put here.
                { "memberId", "53" }

            };

            var definitions = new List<string>
            {
                {"siteName"}
            };

            await API.AssertKeysNotNull(endpoint, parameters, definitions);
        }

        [Test]
        public async Task GetMemberSitesByMemberEmail()
        {
            var endpoint = "/api/Site/GetMemberSitesByMemberEmail";

            var parameters = new Dictionary<string, string>
            {
                //Not sure what values to put ahere.
                { "email", "user1.innovation@redcross.org" }

            };

            var definitions = new Dictionary<string, object>
            {
                {"siteName","San Diego CA Distribution Site"},
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        public async Task GetsiteMembers()
        {
            var endpoint = "/api/Site/siteMembers";

            var parameters = new Dictionary<string, string>
            {
                //Not sure what values to put here.
                { "siteId", "3" }

            };

            var definitions = new Dictionary<string, object>
            {
                { "firstName", "Sherlock" }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }
    }
}

