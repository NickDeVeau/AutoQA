using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;


[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
//[assembly: Parallelizable(ParallelScope.All)]

namespace UITesting
{

    [TestFixture]
    public class BaseTestUI
    {
        public static IConfiguration _configuration;
        public static HttpClient client;
        protected static ExtentReports _extent;

        public static int waitTime;
        public static string appBaseUrl;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(BaseTestUI));
        private static string currentReportFolder; // Keep track of the current test run folder
        public static Dictionary<string, ExtentTest> _testCases = new Dictionary<string, ExtentTest>();
        public static ExtentTest test;

        private static string testRunTimestamp; // Add this line to store the timestamp of the test run

        static BaseTestUI()
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

            if (string.IsNullOrEmpty(testRunTimestamp)) // Only create the folder if it hasn't been created before
            {
                testRunTimestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); // Store the timestamp of the test run

                currentReportFolder = Path.Combine(baseReportFolder, $"Testrun_{testRunTimestamp}");
                Directory.CreateDirectory(currentReportFolder); // Creates new folder for the current test run
                BaseUtilities.SetupLogger(currentReportFolder, _configuration.GetValue<bool>("Logging:WriteToFile"));

                BaseUtilities.DeleteOlderReportFolders(baseReportFolder); // Deletes older report folders

                var htmlReporter = new ExtentHtmlReporter(Path.Combine(currentReportFolder, "index.html"));
                htmlReporter.Config.Theme = Theme.Standard;

                _extent = new ExtentReports();
                _extent.AttachReporter(htmlReporter);

                log.Info("ExtentReports configured and ready for use");
            }

            waitTime = Int32.Parse(_configuration.GetSection("AppInfo:WaitTime").Value);
            appBaseUrl = _configuration.GetSection("AppInfo:Url").Value; // Get website URL

            log.Info("HttpClient initialized with a 2-minute timeout");
        }


        [SetUp]
        public void Setup()
        {
            _testCases[TestContext.CurrentContext.Test.FullName] = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
            test = _testCases[TestContext.CurrentContext.Test.FullName];

            log.Info($"Setting up test: {TestContext.CurrentContext.Test.Name}");

            string fullName = TestContext.CurrentContext.Test.FullName;
            string[] splitName = fullName.Split('.');
            string testName = splitName[splitName.Length - 2] + "." + splitName[splitName.Length - 1];

            Driver.Initialize(testName);

            // Maximize browser window
            Driver.Instance.Manage().Window.Maximize();

            // Navigate to the URL
            Driver.Instance.Navigate().GoToUrl(appBaseUrl);

            // Set zoom level to 50%
           

            Driver.Instance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);


        }

        [TearDown]
        public void Teardown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                ? ""
                : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.StackTrace);
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    log.Error($"Test failed: {TestContext.CurrentContext.Test.Name}");
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

            _testCases[TestContext.CurrentContext.Test.FullName].Log(logstatus, "Test ended with " + logstatus + stacktrace);
            Driver.CloseDriver();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _extent.Flush();
            log.Info("Finished all tests and flushed reports");
        }

        // Takes screenshot of the current browser state
        public static string TakeScreenshot(string testName)
        {
            var js = (IJavaScriptExecutor)Driver.Instance;
            js.ExecuteScript("var body = document.body; var p = document.createElement('p'); p.textContent = 'URL: ' + window.location.href; body.insertBefore(p, body.firstChild);");

            var projectDirPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var testSuiteName = TestContext.CurrentContext.Test.ClassName.Split('.').Last();

            // Modified path to save the screenshots within the TestRun folders.
            var screenshotDirPath = Path.Combine(projectDirPath, "Reports", $"Testrun_{testRunTimestamp}", "Screenshots", testSuiteName);
            Directory.CreateDirectory(screenshotDirPath);

            var screenshotFileName = $"{testName}_{DateTime.Now:MMddHHmmss}.png";
            var screenshotFilePath = Path.Combine(screenshotDirPath, screenshotFileName);

            var screenshot = ((ITakesScreenshot)Driver.Instance).GetScreenshot();
            screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);

            test.Info("URL: " + Driver.Instance.Url);

            return screenshotFilePath;
        }

        
    }
}