# Test Automation Repository README

## Overview
Welcome to our Test Automation Repository! This framework is designed to automate testing for Mobile App UIs, Web UIs, and API endpoints. Powered by a robust suite of technologies including Selenium, Appium, NUnit, .NET Core, Bootstrap, and SQLServer, our framework offers a comprehensive testing experience.

## Features
- **Versatility**: Test Mobile App UIs, Web UIs, and API endpoints seamlessly.
- **Independent Projects**: The API, WebUI, and AppUI projects operate independently of one another. You can work on and execute them separately.
- **Configuration**: Each project comes with its own `appsettings.json` allowing for easy configuration and setup.
- **Parallel or Sequential Testing**: Depending on your needs, execute tests in series or in parallel.
- **Detailed Reports**: When tests are run locally, detailed reports with screenshots are generated for a thorough examination.

## Getting Started

1. **Prerequisites**:
   - Ensure you have .NET Core SDK installed.
   - SQLServer should be set up for database-related tests.
   - Appropriate drivers for Selenium and Appium based on your OS and browsers/devices.

2. **Configuration**:
   - Navigate to the `appsettings.json` of the respective project you're interested in (API, WebUI, AppUI).
   - Update the settings as per your environment and requirements.

3. **Running Tests**:
   - Use NUnit to execute tests.
   - For parallel execution, use the respective NUnit flags.
   - For local execution, ensure you're set up to generate reports. Screenshots will be captured for UI-based tests.

4. **Viewing Reports**:
   - After executing tests locally, navigate to the reports directory to view detailed results with screenshots.

## Feedback and Contributions
Your feedback is invaluable. If you encounter any issues or have suggestions, please open an issue or submit a pull request. We're always looking to improve!

## Technologies Used
- **Web UI Testing**: Selenium, Bootstrap
- **Mobile App UI Testing**: Appium
- **API Testing**: NUnit with .NET Core
- **Database**: SQLServer

Thank you for using our Test Automation Repository. Happy testing!
