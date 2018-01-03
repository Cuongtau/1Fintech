using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Pay1Fintech.Configuration;

namespace Pay1Fintech.Configuration
{
    public class ConfigEnvironment
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        public ConfigEnvironment(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = GetAppConfiguration(env);
        }

        private IConfigurationRoot GetAppConfiguration(IHostingEnvironment env)
        {
            return AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
        }
    }
}
