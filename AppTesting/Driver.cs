using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using RCDAppTests.Models;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Appium.iOS;

namespace RCDAppTests
{
    public class Driver
    {
        private static IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        private static string? BrowserStackUserName = Configuration["BrowserStackConfig:UserName"];
        private static string? BrowserStackAccessKey = Configuration["BrowserStackConfig:AccessKey"];
        private static string? AndroidAppPath = Configuration["BrowserStackConfig:AndroidAppPath"];
        private static string? iOSAppPath = Configuration["BrowserStackConfig:iOSAppPath"];
        private static string? ProjectName = Configuration["BrowserStackConfig:ProjectName"];
        private static string? BuildName = $"TestRun_{DateTime.Now:yyyyMMdd_HHmmss}";
        private static string? BuildIdentifier = Configuration["BrowserStackConfig:BuildIdentifier"];
        private static string? Source = Configuration["BrowserStackConfig:Source"];
        private static bool? BrowserStackLocal = bool.Parse(Configuration["BrowserStackConfig:BrowserStackLocal"] ?? "false");
        private static bool? Debug = bool.Parse(Configuration["BrowserStackConfig:Debug"] ?? "false");
        private static bool? NetworkLogs = bool.Parse(Configuration["BrowserStackConfig:NetworkLogs"] ?? "false");
        private static string? ConsoleLogs = Configuration["BrowserStackConfig:ConsoleLogs"];

        private static List<Device> Devices = Configuration.GetSection("Devices").Get<List<Device>>();

        static Driver()
        {
            foreach (var device in Devices)
            {
                device.InferDeviceType();
            }
        }

        public enum DeviceType
        {
            Android,
            iOS
        }

        public static IWebDriver CreateDriver(int DeviceIndex = 0)
        {
            if (DeviceIndex < 0 || DeviceIndex >= Devices.Count)
                throw new ArgumentOutOfRangeException(nameof(DeviceIndex));

            var Device = Devices[DeviceIndex];

            string? AppPath;
            if (Device.DeviceType == DeviceType.Android)
            {
                AppPath = Configuration["BrowserStackConfig:AndroidAppPath"];
            }
            else
            {
                AppPath = Configuration["BrowserStackConfig:iOSAppPath"];
            }

            AppiumOptions appiumOptions = new AppiumOptions();

            appiumOptions.AddAdditionalCapability("browserstack.user", BrowserStackUserName);
            appiumOptions.AddAdditionalCapability("browserstack.key", BrowserStackAccessKey);
            appiumOptions.AddAdditionalCapability("app", AppPath);
            appiumOptions.AddAdditionalCapability("project", ProjectName);
            appiumOptions.AddAdditionalCapability("build", BuildName);
            appiumOptions.AddAdditionalCapability("buildIdentifier", BuildIdentifier);
            appiumOptions.AddAdditionalCapability("source", Source);
            appiumOptions.AddAdditionalCapability("browserstack.local", BrowserStackLocal);
            appiumOptions.AddAdditionalCapability("debug", Debug);
            appiumOptions.AddAdditionalCapability("networkLogs", NetworkLogs);
            appiumOptions.AddAdditionalCapability("consoleLogs", ConsoleLogs);
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, Device.PlatformName);
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, Device.DeviceName);
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, Device.OsVersion);

            try
            {
                if (Device.DeviceType == DeviceType.Android)
                {
                    return new AndroidDriver<AndroidElement>(new Uri("http://hub-cloud.browserstack.com/wd/hub"), appiumOptions);
                }
                else if (Device.DeviceType == DeviceType.iOS)
                {
                    appiumOptions.AddAdditionalCapability("automationName", "XCUITest");
                    return new IOSDriver<IOSElement>(new Uri("http://hub-cloud.browserstack.com/wd/hub"), appiumOptions);
                }
                else
                {
                    throw new NotSupportedException($"Device type '{Device.DeviceType}' is not supported.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing driver for device: {Device.DeviceName}, OS: {Device.OsVersion}, Platform: {Device.PlatformName}. Error: {ex.Message}");
                throw;
            }
        }

        public static List<IWebDriver> RunAllDevicesInParallel()
        {
            var drivers = new List<IWebDriver>();
            var tasks = new List<Task<IWebDriver>>();

            for (int i = 0; i < Devices.Count; i++)
            {
                int deviceIndex = i;
                tasks.Add(Task.Run(() => CreateDriver(deviceIndex)));
            }

            Task.WhenAll(tasks).ContinueWith(t =>
            {
                foreach (var task in tasks)
                {
                    drivers.Add(task.Result);
                }
            }).Wait();

            return drivers;
        }
    }
}
