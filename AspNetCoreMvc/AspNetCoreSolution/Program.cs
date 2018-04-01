using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSolution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((webHostBuilder, configBuilder) =>
                {
                    IHostingEnvironment env = webHostBuilder.HostingEnvironment;

                    configBuilder.AddJsonFile("appsettings.json", false, false)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                        .AddJsonFile("App_Config/ConnectionStrings.json", false, true)
                        .AddJsonFile("App_Config/IdentityOptions.json", false, true);
                })
                .Build();
    }
}
