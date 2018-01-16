using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CookieAuthDemo.Data;
using CookieAuthDemo.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CookieAuthDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var dbContext = services.GetRequiredService<ApplicationDbContext>();

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                try
                {
                    dbContext.Database.Migrate();
                    if (!dbContext.Users.Any())
                    {
                        var user = new ApplicationUser()
                        {
                            UserName = "djlnet",
                            Email = "394922860@qq.com",
                            NormalizedEmail = "admin"
                        };
                        var result = userManager.CreateAsync(user, "Djl394922860=").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception("default user init error:" + result.Errors.Select(x => x.Description).Aggregate((x, y) => $"{x} {y}"));
                        }
                    }

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
