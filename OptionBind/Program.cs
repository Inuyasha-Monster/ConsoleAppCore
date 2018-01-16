using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OptionBind
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                // 覆盖默认配置
                //.ConfigureAppConfiguration(config =>
                //{
                //    config.AddJsonFile("appsettings.json", false, false);
                //})
                .UseStartup<Startup>()
                .Build();
    }
}
