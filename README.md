# The American Red Cross - Test Automation

## Introduction
The American Red Cross Test Automation project is an initiative to ensure the robustness of our applications and platforms by testing both UI and API components. Our testing frameworks include Selenium for UI, log4net for logging, Extent Reports for visual reports, and NUnit as our testing framework. Our repository, "Test Automation" on Azure DevOps, aims to create a transparent and repeatable testing process.

## Getting Started

### Installation Process
1. Clone the repository from Azure DevOps.
2. Ensure that your environment has the required tools such as Selenium, log4net, NUnit, and Extent Reports.
3. Follow the build steps as outlined in the subsequent section.

### Software Dependencies
- **Testing Framework**: NUnit (v3.13.3) combined with Microsoft.NET.Test.Sdk.
- **UI Testing**: Selenium.WebDriver (v4.11.0) with ChromeDriver support (v115.0.5790.17000).
- **Logging**: log4net (v2.0.15).
- **Test Reporting**: ExtentReports (v4.1.0).
- **Configuration Management**: Microsoft.Extensions.Configuration (v7.0.0) and Microsoft.Extensions.Configuration.Json (v7.0.0) for JSON-based configurations.
- **JSON Handling**: Newtonsoft.Json (v13.0.3).
- **Test Coverage**: coverlet.collector (v6.0.0) for generating code coverage.
- **Selenium Utilities**: DotNetSeleniumExtras.WaitHelpers (v3.11.0) for additional WebDriver functionalities.

### Latest Releases
- Please check our Azure DevOps page for the latest releases.

### API References
- TODO: Add links or references to your API documentation.

## Build and Test
To build the solution and run the tests, follow these steps:
1. Open the solution in your favorite IDE (e.g., Visual Studio).
2. Build the solution to fetch the required dependencies.
3. Run the tests using the NUnit test runner.