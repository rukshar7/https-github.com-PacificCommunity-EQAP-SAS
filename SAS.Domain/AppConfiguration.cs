using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;
namespace SAS.Domain
{
    public class AppConfiguration
    {

        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            GetSASDbConnection = root.GetConnectionString("SASDbConnection");
            GetPacSimsSecurityDbConnection = root.GetConnectionString("PacSimsSecurityDbConnection");

        }


        public string GetSASDbConnection { get; }
        public string GetPacSimsSecurityDbConnection { get; }


    }

}
