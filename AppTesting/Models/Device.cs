namespace RCDAppTests.Models
{
    public class Device
    {
        public string? DeviceName { get; set; }
        public string? OsVersion { get; set; }
        public string? PlatformName { get; set; }
        public Driver.DeviceType DeviceType { get; set; }

        public void InferDeviceType()
        {
            if (PlatformName?.ToLower() == "android")
            {
                DeviceType = Driver.DeviceType.Android;
            }
            else if (PlatformName?.ToLower() == "ios")
            {
                DeviceType = Driver.DeviceType.iOS;
            }
        }
    }
}
