using Microsoft.Extensions.Configuration;
using System.IO;

public sealed class ConfigurationService
{
    private static readonly ConfigurationService _instance = new ConfigurationService();
    private readonly IConfiguration _configuration;

    static ConfigurationService()
    {
    }

    private ConfigurationService()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static ConfigurationService Instance
    {
        get
        {
            return _instance;
        }
    }

    public string GetValue(string key)
    {
        return _configuration[key];
    }

}
