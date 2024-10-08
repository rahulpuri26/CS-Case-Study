using System;
using Microsoft.Extensions.Configuration;

namespace FManage.Utilities
{
    internal static class DbConnUtil
    {
        private static IConfiguration _iconfiguration;

        static DbConnUtil()
        {
            GetAppSettingsFile();
        }

        private static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())// sets the base path where configuration file is located
                         .AddJsonFile("appsettings.json");// configuration should include  the configuration file named as appsettings,json 
            _iconfiguration = builder.Build();//build creates an Iconfiguration object which has data from Appsettings file



        }

        public static string GetConnString()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            IConfiguration configuration = builder.Build();
            return configuration.GetConnectionString("LocalConnectionString");
        }
    }
}

