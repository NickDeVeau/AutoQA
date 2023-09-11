using AventStack.ExtentReports;
using log4net;
using NUnit.Framework;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]

namespace UITesting
{
    [TestFixture]
    public class BaseTestUI
    {
        private readonly ILog Log = LogManager.GetLogger(typeof(BaseTestUI));

        private string _testRunTimestamp;
        private string _currentReportFolder;

        public  HttpClient Client { get; private set; }
        public static readonly int WaitTime = int.Parse(ConfigurationService.Instance.GetValue("AppInfo:WaitTime"));
        public string AppBaseUrl = ConfigurationService.Instance.GetValue("AppInfo:Url");
        protected ExtentReports Extent { get; private set; }
        public Dictionary<string, ExtentTest> TestCases { get; private set; } = new Dictionary<string, ExtentTest>();
        public ExtentTest Test { get; private set; }

        public BaseTestUI()
        {
            InitializeHttpClient();
            InitializeReport();
        }

        [SetUp]
        public void SetUpTest()
        {
            SetUpExtentTest();
            InitializeDriver();
            NavigateToBaseAppUrl();
        }

        [TearDown]
        public void TearDownTest()
        {
            LogTestResult();
            Driver.CloseDriver();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            FlushReports();
        }

        public string TakeScreenshot(string testName)
        {
            AppendUrlToBrowser();
            return CaptureScreenshot(testName);
        }

        private void InitializeHttpClient()
        {
            Client = new HttpClient { Timeout = TimeSpan.FromMinutes(2) };
            Log.Info("HttpClient initialized with a 2-minute timeout");
        }

        private void InitializeReport()
        {
            var baseReportFolder = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "Reports");

            if (string.IsNullOrEmpty(_testRunTimestamp))
            {
                _testRunTimestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                _currentReportFolder = Path.Combine(baseReportFolder, $"Testrun_{_testRunTimestamp}");
                Directory.CreateDirectory(_currentReportFolder);

                var htmlReporter = new ExtentHtmlReporter(Path.Combine(_currentReportFolder, "index.html"))
                {
                    Config = { Theme = Theme.Standard }
                };

                Extent = new ExtentReports();
                Extent.AttachReporter(htmlReporter);

                Log.Info("ExtentReports configured and ready for use");
            }
        }

        private void SetUpExtentTest()
        {
            TestCases[TestContext.CurrentContext.Test.FullName] = Extent.CreateTest(TestContext.CurrentContext.Test.Name);
            Test = TestCases[TestContext.CurrentContext.Test.FullName];
            Log.Info($"Setting up test: {TestContext.CurrentContext.Test.Name}");
        }

        private void InitializeDriver()
        {
            var testName = GetTestNameFromContext();
            Driver.Initialize(testName);

            Driver.Instance.Manage().Window.Maximize();
            Driver.Instance.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        private void NavigateToBaseAppUrl()
        {
            Driver.Instance.Navigate().GoToUrl(AppBaseUrl);
        }

        private void LogTestResult()
        {
            var result = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                ? ""
                : $"<pre>{TestContext.CurrentContext.Result.StackTrace}</pre>";

            Status logstatus;
            switch (result)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    Log.Error($"Test failed: {TestContext.CurrentContext.Test.Name}");
                    break;
                case TestStatus.Inconclusive:
                    logstatus = Status.Warning;
                    Log.Warn($"Test inconclusive: {TestContext.CurrentContext.Test.Name}");
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    Log.Info($"Test skipped: {TestContext.CurrentContext.Test.Name}");
                    break;
                default:
                    logstatus = Status.Pass;
                    Log.Info($"Test passed: {TestContext.CurrentContext.Test.Name}");
                    break;
            }

            TestCases[TestContext.CurrentContext.Test.FullName].Log(logstatus, "Test ended with " + logstatus + stacktrace);
        }

        private void FlushReports()
        {
            Extent.Flush();
            Log.Info("Finished all tests and flushed reports");
        }

        private void AppendUrlToBrowser()
        {
            var js = (IJavaScriptExecutor)Driver.Instance;
            js.ExecuteScript("var body = document.body; var p = document.createElement('p'); p.textContent = 'URL: ' + window.location.href; body.insertBefore(p, body.firstChild);");
            Test.Info("URL: " + Driver.Instance.Url);
        }

        private string CaptureScreenshot(string testName)
        {
            var projectDirPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            var testSuiteName = TestContext.CurrentContext.Test.ClassName.Split('.').Last();
            var screenshotDirPath = Path.Combine(projectDirPath, "Reports", $"Testrun_{_testRunTimestamp}", "Screenshots", testSuiteName);
            Directory.CreateDirectory(screenshotDirPath);

            var screenshotFileName = $"{testName}_{DateTime.Now:MMddHHmmss}.png";
            var screenshotFilePath = Path.Combine(screenshotDirPath, screenshotFileName);

            var screenshot = ((ITakesScreenshot)Driver.Instance).GetScreenshot();
            screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);

            return screenshotFilePath;
        }

        private string GetTestNameFromContext()
        {
            var fullName = TestContext.CurrentContext.Test.FullName;
            var splitName = fullName.Split('.');
            return splitName[splitName.Length - 2] + "." + splitName[splitName.Length - 1];
        }
    }
}
