using NUnit.Framework;

namespace APITesting.TestSuites
{
	public class Member : BaseTestAPI
	{

        [Test,Order(1)]
        
        public async Task PostMembersByEmailAndType()
        {
            var endpoint = "/api/Member/LoginMemberByEmailAndType";

            var requestForm = new Dictionary<string, object>
            {
                { "email", "user1.innovation@redcross.org" },
                {"userType","3" }
            };

            await API.PostToEndpoint(endpoint, requestForm);
        }

        [Test, Order(2)]
        
        public async Task GetMemberById()
        {
            var endpoint = "/api/Member/GetMemberById";

            var parameters = new Dictionary<string, string>
            {
                { "memberId", "53" }
            };

            var definitions = new Dictionary<string, object>
            {
                {"lastName","Kovtanyuk"},
                {"memberId", 53 }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test, Order(3)]
        
        public async Task GetMemberByEmail()
        {
            var endpoint = "/api/Member/GetMemberByEmail";

            var parameters = new Dictionary<string, string>
            {
                { "email", "user1.innovation@redcross.org" }
            };

            var definitions = new Dictionary<string, object>
            {
                {"userName", "Testaccone"},
                { "email", "user1.innovation@redcross.org" }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test, Order(4)]
        
        public async Task GetMemberByEmailAndType()
        {
            var endpoint = "/api/Member/GetMemberByEmailAndType";

            var parameters = new Dictionary<string, string>
            {
                { "email", "user1.innovation@redcross.org" },
                {"userType","3"}
            };

            var definitions = new Dictionary<string, object>
            {
                {"userName", "Testaccone"},
                { "email", "user1.innovation@redcross.org" },
                {"userType", 3}
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }
    }
}

