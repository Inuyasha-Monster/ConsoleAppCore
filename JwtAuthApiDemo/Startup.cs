using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthApiDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            var jwtSettings = new JwtSettings();

            Configuration.Bind("JwtSettings", jwtSettings);

            services.AddMvc();

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = jwtSettings.ValidIssuer,
                        ValidAudience = jwtSettings.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.IssuerSigningKey))
                    };

                    // 自定义验证逻辑
                    //x.SecurityTokenValidators.Clear();
                    //x.SecurityTokenValidators.Add(new CustomerSecurityTokenValidator());


                    // 自定义token取值逻辑
                    //x.Events = new JwtBearerEvents()
                    //{
                    //    OnMessageReceived = (messageReceivedContext) =>
                    //    {
                    //        var token = messageReceivedContext.Request.Headers["token"].FirstOrDefault();
                    //        messageReceivedContext.Token = token;
                    //        return Task.CompletedTask;
                    //    }
                    //};

                });

            services.AddAuthorization(option =>
            {
                option.AddPolicy("SuperAdmin", x => x.RequireClaim("SuperAdmin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();

            app.UseMvcWithDefaultRoute();
        }
    }
}
