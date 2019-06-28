using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Tokens;
using TestDemo.WebAPI.Controllers;
using TestDemo.Data;
using TestDemo.Business;
using TestDemo.Business.GlobalErrorHandling;
using Microsoft.EntityFrameworkCore;
using TestDemo.Data.Entities;
using NLog;
using NLog.Extensions.Logging;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace TestDemo.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            //loggerFactory.ConfigureNLog(string.Concat(Directory.GetCurrentDirectory(), "/nNlog.config"));

            #region Code for Loading Nlog.config file location

            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/Nlog.config"));

            #endregion
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Code for register Default MSSQL server Connection String

            services.AddDbContext<AppDataContext>(opt =>
            opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #endregion

            //, x => x.MigrationsAssembly("TestDemo.Data")

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region Swagger Functionalities Code for registering that service
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My First API",
                    Description = "My First ASP.NET Core 2.0 Web API",
                    TermsOfService = "None",
                    Contact = new Contact()
                    {
                        Name = "Amar Singh",
                        Email = "amar.singh40@gmail.com",
                        Url = "https://www.google.com/"
                    }
                });

                //c.SwaggerDoc("v2", new Info
                //{
                //    Version = "v2",
                //    Title = "My Second API",
                //    Description = "My Second Web API",
                //    TermsOfService = "Copy Right Law",
                //    Contact = new Contact()
                //    {
                //        Name = "Amit Choudhary",
                //        Email = "Amitchoudhary@gmail.com",
                //        Url = "https://www.google.com/"
                //    }

                //});
            });
            #endregion

            #region Enable Autentication Service for JWT 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";

            }).AddJwtBearer("JwtBearer", jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = TokenController.Signin_key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });
            #endregion

            #region register classes and interface for Dependency Injection
            services.AddTransient<IRepository<Employee>, Respository<Employee>>();

            services.AddSingleton<ILoggerManager, LoggerManager>();
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            #region Handling Excpetion Globally either by commented code or customized ConfigureGlobalExcpetionFilter

            app.ConfigureGlobalExcpetionFilter(logger);
            //app.UseExceptionHandler(
            //    options =>
            //    {
            //        options.Run(
            //            async context =>
            //            {
            //                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //                context.Response.ContentType = "text/html";
            //                var ex = context.Features.Get<IExceptionHandlerFeature>();
            //                if (ex != null)
            //                {
            //                    var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace }";
            //                    await context.Response.WriteAsync(err).ConfigureAwait(false);
            //                }
            //            }
            //            );
            //    }
            //    );            
            #endregion




            app.UseHttpsRedirection();
            app.UseMvc();

            #region use Swagger With WebAPI for UI represenation
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API V2");
            });
            #endregion

            #region Enabling Autentication

            app.UseAuthentication();
            #endregion
        }
    }
}
