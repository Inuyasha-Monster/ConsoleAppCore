using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacMvcCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AutofacMvcCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //services.AddScoped<IMessageService, MessageService>();

            // Add Autofacs
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            containerBuilder.RegisterType<MessageService>().As<IMessageService>();

            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            applicationLifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine("ApplicationStarted");
            });
            applicationLifetime.ApplicationStopped.Register(() =>
            {
                Console.WriteLine("ApplicationStopped");
            });
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                Console.WriteLine("ApplicationStopping");
            });

            app.UseMvc();

            app.UseMvcWithDefaultRoute();
        }
    }
}
