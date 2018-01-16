using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OptionBind
{
    public class Startup
    {
        public IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ClassTest>(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseMvcWithDefaultRoute();

            //app.Run(async (context) =>
            //{
            //    context.Response.ContentType = "text/plain;charset=utf-8";


            //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //    var classTest = new ClassTest();
            //    this.Configuration.Bind(classTest);

            //    await context.Response.WriteAsync($"{Encoding.GetEncoding("gb2312").ToString()}");


            //    await context.Response.WriteAsync($"{classTest.ClassName}");
            //    await context.Response.WriteAsync($"{classTest.No}");

            //    await context.Response.WriteAsync($"{classTest.Students[0].Name}");
            //    await context.Response.WriteAsync($"{classTest.Students[0].Age}");

            //    await context.Response.WriteAsync($"{classTest.Students[1].Name}");
            //    await context.Response.WriteAsync($"{classTest.Students[1].Age}");

            //    await context.Response.WriteAsync($"{classTest.Students[2].Name}");
            //    await context.Response.WriteAsync($"{classTest.Students[2].Age}");

            //    await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
