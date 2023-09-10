using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace APITesting.TestSuites
{
    public class GeneralAvailability : BaseTestAPI
    {

        [Test, Order(1)]
        
        public async Task DeleteUserAvailability()
        {
            // Define User details and endpoint
            var memberId = "21707";
            var deleteEndpoint = "/api/GeneralAvailability/DeleteUserAvailability";
            var getEndpoint = "/api/GeneralAvailability/GetUserAvailability";
            var parameters = new Dictionary<string, string> { { "memberId", memberId } };

            // Get the content from the endpoint
            string content = await API.GetJsonFromEndpoint(getEndpoint, parameters);

            // Parse the JSON string into a JObject
            JObject jsonResponse = JObject.Parse(content);

            // Extract the 'content' array from the JSON response
            JArray memberAvailabilityArray = (JArray)jsonResponse["content"];

            // Check if availability exists for day 1
            var day1Availability = memberAvailabilityArray.Children<JObject>()
                .FirstOrDefault(o => o["dayOfWeek"].Value<int>() == 1);

            // If day 1 availability exists, delete it
            if (day1Availability != null)
            {
                var memberAvailabilityId = day1Availability["memberAvailabilityId"].Value<int>();

                // Form the request body for delete endpoint
                var deleteBody = new
                {
                    memberAvailabilityId = memberAvailabilityId,
                    memberId = int.Parse(memberId),
                    dayOfWeek = 1
                };

                await API.DeleteFromEndpointWithBody(deleteEndpoint, deleteBody);


                await API.DeleteFromEndpointWithBody(deleteEndpoint, deleteBody);

                // Get the content from the endpoint again to verify deletion
                content = await API.GetJsonFromEndpoint(getEndpoint, parameters);

                // Parse the JSON string into a JObject
                jsonResponse = JObject.Parse(content);

                // Extract the 'content' array from the JSON response
                memberAvailabilityArray = (JArray)jsonResponse["content"];

                // Verify the deleted item is not in the list anymore
                day1Availability = memberAvailabilityArray.Children<JObject>()
                    .FirstOrDefault(o => o["dayOfWeek"].Value<int>() == 1);

                Assert.IsNull(day1Availability, "Failed to delete availability for day 1");
            }
            else
            {
                Assert.Pass("No availability for day 1 to delete");
            }
        }


        [Test, Order(2)]
        
        public async Task PostAndGetUserAvailability()
        {
            // Define User details and endpoint
            var memberId = "21707";
            var postEndpoint = "/api/GeneralAvailability/AddUserAvailability";
            var getEndpoint = "/api/GeneralAvailability/GetUserAvailability";
            var parameters = new Dictionary<string, string> { { "memberId", memberId } };

            // Get the content from the endpoint
            string content = await API.GetJsonFromEndpoint(getEndpoint, parameters);

            // Parse the JSON string into a JObject
            JObject jsonResponse = JObject.Parse(content);

            // Extract the 'content' array from the JSON response
            JArray memberAvailabilityArray = (JArray)jsonResponse["content"];

            // Extract all 'dayOfWeek' values from the 'content' array
            List<int> daysOfWeek = memberAvailabilityArray.Children<JObject>()
                .Select(o => o["dayOfWeek"].Value<int>())
                .Distinct() // This will ensure there are no duplicate days
                .ToList();

            // Create a list of all days of the week
            var allDaysOfWeek = Enumerable.Range(1, 6).ToList();

            // Check if any day of the week is missing from the availability list
            var missingDays = allDaysOfWeek.Except(daysOfWeek);

            // If any missing days, pick the first one and post it
            if (missingDays.Any())
            {
                var missingDay = missingDays.First();

                var requestForm = new Dictionary<string, object>
                {
                    {"memberId", memberId},
                    {"dayOfWeek", missingDay.ToString()},
                    {"startTime", "01:02:00"},
                    {"endTime", "22:02:00"},
                };

                await API.PostToEndpoint(postEndpoint, requestForm);
                Assert.Pass($"Added missing availability for day of the week: {missingDay}");
            }
            else
            {
                Assert.Pass("All days of the week exist in the availability list");
            }
        }

        [Test,Order(3)]
        
        public async Task GetUserGeneralAvailability()
        {
            var endpoint = "/api/GeneralAvailability/GetUserAvailability";

            var parameters = new Dictionary<string, string>
            {
                //Not sure what values to put here.
                { "memberId", "21707" }
            };

            var definitions = new Dictionary<string, object>
            {
                {"statusCode", 200},
                {"memberId", 21707 }
            };



            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }
 
     
    }
}
