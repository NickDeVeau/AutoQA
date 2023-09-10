
## RCDAppTests

RCDAppTests is an Appium testing framework designed to execute mobile tests on multiple devices (both Android and iOS) using BrowserStack. The framework leverages NUnit for its testing capabilities.

### Overview:

- **Models**: Contains data models representing different entities used throughout the framework, like the `Device` model.
  
- **Driver**: Responsible for initiating connections to devices, either Android or iOS, on BrowserStack. Handles parallel device execution.
  
- **BaseTest**: Base test class which other test classes should inherit from. It sets up and tears down the drivers for tests.

### How To Use:

1. **Configuration**:
   - Configuration details are placed in `appsettings.json`.
   - Add your BrowserStack `UserName` and `AccessKey` to `BrowserStackConfig`.
   - Add the paths to your Android and iOS apps in `BrowserStackConfig`.
   - Define the devices you want to test on under the `Devices` section. 

2. **Writing Tests**:
   - Each test class should inherit from the `BaseTest` class.
   - Within tests, use the `drivers` list to access initialized drivers for each device.
   - Handle platform-specific logic using:
     ```csharp
     if (driver is AndroidDriver<AndroidElement>)
     {
         // Android-specific logic
     }
     else if (driver is IOSDriver<IOSElement>)
     {
         // iOS-specific logic
     }
     ```

3. **Execution**:
   - Use NUnit to run tests. Since drivers for all devices are initialized in parallel, tests will run across all devices specified in the configuration.

### Important Classes:

- **Device**: Represents a device with properties like `DeviceName`, `OsVersion`, `PlatformName`, and inferred `DeviceType`.

- **Driver**: Contains logic for creating and managing connections to devices. Uses the configuration in `appsettings.json` to determine devices, BrowserStack settings, and other capabilities.

- **BaseTest**: Provides the setup (`SetUp`) and cleanup (`TearDown`) logic for tests. Every test suite should inherit from this base class to get drivers initialized for all devices.

### Important Notes:

- **XPaths and Selectors**: When writing tests, be cautious with XPaths as they can be brittle. Try to use accessibility IDs or other more stable selectors. If XPaths are necessary, ensure you have platform-specific locators if the apps differ between Android and iOS.

- **Parallel Execution**: The framework is designed to run tests on all devices in parallel, reducing execution time when testing on multiple devices.

- **Configuration**: Make sure to keep the `appsettings.json` file secure as it contains sensitive data like BrowserStack credentials.

### To-Do:

- Implement actual test logic replacing placeholders.
- Expand the framework to incorporate more advanced features or utilities as needed.
  