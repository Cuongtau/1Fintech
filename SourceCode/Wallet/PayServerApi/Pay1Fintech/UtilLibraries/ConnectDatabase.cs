using Microsoft.Extensions.Configuration;
using System.IO;

namespace UtilLibraries
{
    public class ConnectDatabase
    {
        private readonly IConfigurationRoot _configuration;
        public string ConnectString { get; private set; }
        public ConnectDatabase()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false).Build();
            ConnectString = _configuration.GetConnectionString("Default");
        }
    }
}
