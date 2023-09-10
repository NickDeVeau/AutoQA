using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace APITesting.TestSuites
{
    public class DispatchRequest : BaseTestAPI
    {
        [Test]
        
        [Ignore("In Development")]
        public async Task PostDispatchRequest()
        {
            var endpoint = "/api/DispatchRequest/AddDispatchRequest";

            var requestForm = new Dictionary<string, object>
                    {
                        { "", "" },
                        {"","" }
                    };

            await API.PostToEndpoint(endpoint, requestForm);
        }

        [Test]
        
        [Ignore("In Development")]
        public async Task GetDispatchRequests()
        {
            var endpoint = "/api/DispatchRequest/GetDispatchRequestDetails";

            var parameters = new Dictionary<string, string>
            {
                { "memberId", "" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "", "" }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        
        [Ignore("In Development")]
        public async Task GetDispatchRequestDetails()
        {
            var endpoint = "/api/DispatchRequest/GetDispatchRequestDetails";

            var parameters = new Dictionary<string, string>
            {
                { "DispatchRequestId", "" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "", "" }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        
        [Ignore("In Development")]
        public async Task GetAcceptRequests()
        {
            var endpoint = "/api/DispatchRequest/GetAcceptRequest";

            var parameters = new Dictionary<string, string>
            {
                { "MemberId", "" },
                {"isPushNotificationRequered","false" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "", "" }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        
        [Ignore("In Development")]
        public async Task GetDispatchRequestStatusForOrder()
        {
            var endpoint = "/api/DispatchRequest/GetAcceptRequest";

            var parameters = new Dictionary<string, string>
            {
                { "orderId", "" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "", "" }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        
        [Ignore("In Development")]
        public async Task GetRecentDispatches()
        {
            var endpoint = "/api/DispatchRequest/GetDispatchRequestStatusForOrder";

            var parameters = new Dictionary<string, string>
            {
                { "siteId", "" },
                { "dayOffset", "" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "", "" }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        
        [Ignore("In Development")]
        public async Task GetDispatchDetails()
        {
            var endpoint = "/api/DispatchRequest/admin/GetDispatchDetails";

            var parameters = new Dictionary<string, string>
            {
                { "dispatchRequestId", "" }
            };

            var definitions = new Dictionary<string, object>
            {
                { "", "" }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }

        [Test]
        
        [Ignore("In Development")]
        public async Task GetUserDistanceForDispatch()
        {
            var endpoint = "/api/DispatchRequest/CheckUserDistanceForDispatch";

            var parameters = new Dictionary<string, string>
            {
                { "dispatchRequestId", "" },
                { "longitude", "" },
                { "latitude", "" },
            };

            var definitions = new Dictionary<string, object>
            {
                { "", "" }
            };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }
    }
}

