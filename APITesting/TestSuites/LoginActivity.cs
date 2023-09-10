using NUnit.Framework;

namespace APITesting.TestSuites
{
    public class LoginActivity : BaseTestAPI
    {

        [Test]
        
        public async Task PostLoginActivity()
        {
            var endpoint = "/api/LoginActivity/AddLoginActivity";

            var requestForm = new Dictionary<string, object>
            {
                {"loginActivityId", "87100CA9-A662-40C8-92C4-4AF2F6329D8F"},
                {"requesterTypeId", "1"},
                {"authenticationTypeId", "1"},
                {"activityTypeId", "1"},
                {"email", "nicholas.deveau@redcross.org"},
                {"timeStamp", "7/20/2023 3:33:15 PM"},
            };
            

            await API.PostToEndpoint(endpoint, requestForm);
        }

        [Test]
        
        public async Task PutLoginActivity()
        {
            var endpoint = "/api/LoginActivity/UpdateLoginActivity";

            var requestForm = new Dictionary<string, object>
            {
                {"loginActivityId", "87100CA9-A662-40C8-92C4-4AF2F6329D8F"},
                {"requesterTypeId", "1"},
                {"authentdicationTypeId", "1"},
                {"activityTypeId", "1"},
                {"email", "nicholas.deveau@redcross.org"},
                {"timeStamp", $"7/20/2023 {DateTime.Now:HH:mm} PM"},
            };

            await API.PutToEndpoint(endpoint, requestForm);

        }
    }
}

