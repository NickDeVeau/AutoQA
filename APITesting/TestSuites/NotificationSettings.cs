using Microsoft.VisualStudio.CodeCoverage;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;

namespace APITesting.TestSuites
{
    public class NotificationSettings : BaseTestAPI
    {
        [Test]
        public async Task PostUserNotificationSettings()
        {
            var endpoint = "/api/NotificationSettings/SetUserNotificationSettings";

            var requestForm = new Dictionary<string, object>
            {
                {"notificationsDisabled", "True"},
                {"dispatchDisabled", "True"},
                {"remindersDisabled", "True"},
                {"memberId", "21707"},
                {"snoozeInMinutes", "0"},
                {"notificationsEnd", "7/20/2023 7:18:50 PM"},
            };

            await API.PostToEndpoint(endpoint, requestForm);
        }

        [Test]
        public async Task GetUserNotificationSettings()
        {
            var endpoint = "/api/NotificationSettings/GetUserNotificationSettings";

            var parameters = new Dictionary<string, string>
            {
                { "memberId", "21707" }
            };

            var definitions = new Dictionary<string, object>
           {
                {"notificationSettingId", 89},
                {"memberId", 21707},
                {"notificationsDisabled", true},
                {"dispatchDisabled", true},
                {"remindersDisabled", true},
           };

            await API.VerifyDefinitionExists(endpoint, parameters, definitions);
        }
    }
}

