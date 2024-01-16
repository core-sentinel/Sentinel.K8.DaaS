using Microsoft.Extensions.Configuration;

namespace Sentinel.Tests.Helper
{
    public class ConfigurationHelper
    {
        public static string? GetRootFolder()
        {
            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            // var path = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            var dllfolder = Directory.GetParent(path)?.Parent?.Parent?.Parent?.FullName;

            return dllfolder;
        }


        public static IConfiguration GetConfiguration(string? relativePathToAppsettingsfile)
        {
            if (string.IsNullOrWhiteSpace(relativePathToAppsettingsfile))
            {
                relativePathToAppsettingsfile = "./appsettings.json";
            }

            var path = System.AppDomain.CurrentDomain.BaseDirectory;
            // var path = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            var dllfolder = Directory.GetParent(path)?.Parent?.Parent?.Parent?.FullName;
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .SetBasePath(dllfolder)
                .AddJsonFile(relativePathToAppsettingsfile)
                .Build();
            return config;
        }
    }
}
