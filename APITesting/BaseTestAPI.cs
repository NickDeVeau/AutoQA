using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using NUnit.Framework.Interfaces;
using System.Net.NetworkInformation;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

[assembly: NonParallelizable]

namespace APITesting
{

    [TestFixture]
    public class BaseTestAPI
    {
        public static IConfiguration _configuration;
        public static HttpClient client;
        protected static ExtentReports _extent;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(BaseTestAPI));
        private static string currentReportFolder; // Keep track of the current test run folder
        private static Dictionary<string, ExtentTest> _testCases = new Dictionary<string, ExtentTest>();

        [OneTimeSetUp]
        public static void OneTimeSetup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(2) // sets a timeout of 2 minutes
            };

            var baseReportFolder = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Reports");

            if (currentReportFolder == null) // Check if the currentReportFolder is not already set
            {
                currentReportFolder = Path.Combine(baseReportFolder, $"Testrun_{DateTime.Now:yyyyMMddHHmmss}");
                Directory.CreateDirectory(currentReportFolder); // Creates new folder for the current test run
                BaseUtilities.SetupLogger(currentReportFolder); // Set up log4net logger
                BaseUtilities.DeleteOlderReportFolders(baseReportFolder); // Deletes older report folders

                var htmlReporter = new ExtentHtmlReporter(Path.Combine(currentReportFolder, "index.html"));
                htmlReporter.Config.Theme = Theme.Standard;

                _extent = new ExtentReports();
                _extent.AttachReporter(htmlReporter);

                log.Info("ExtentReports configured and ready for use");
            }

            log.Info("HttpClient initialized with a 2-minute timeout");
        }


        [SetUp]
        public void Setup()
        {
            _testCases[TestContext.CurrentContext.Test.FullName] = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            log.Info($"Setting up test: {TestContext.CurrentContext.Test.Name}");
        }

        [TearDown]
        public void Teardown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;

            // Get the exception message
            var exceptionMessage = TestContext.CurrentContext.Result.Message;

            // Combine the exception message and the stack trace
            var combinedMessage = exceptionMessage + "\n" + TestContext.CurrentContext.Result.StackTrace;

            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    log.Error($"Test failed: {TestContext.CurrentContext.Test.Name}. Error: {exceptionMessage}"); // log the exception message here
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    log.Warn($"Test inconclusive: {TestContext.CurrentContext.Test.Name}");
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    log.Info($"Test skipped: {TestContext.CurrentContext.Test.Name}");
                    break;
                default:
                    logstatus = Status.Pass;
                    log.Info($"Test passed: {TestContext.CurrentContext.Test.Name}");
                    break;
            }

            _testCases[TestContext.CurrentContext.Test.FullName].Log(logstatus, "Test ended with " + logstatus + ": " + combinedMessage);
        }



        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _extent.Flush();
            log.Info("Finished all tests and flushed reports");
        }
    }
}
