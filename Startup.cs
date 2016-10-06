using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Halcyon.Web.HAL.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;

namespace CustomerOrdersApi
{
    public class Startup
    {

        public Startup(IHostingEnvironment env) {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Console.WriteLine("sss");
            Console.WriteLine(Configuration["AppSettings:BaseUrls:Shipping"]);
            Console.WriteLine("sss2");
            Console.WriteLine(Configuration.ToString());
        }
        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var outputSettings = JsonSerializerSettingsProvider.CreateSerializerSettings();

// Configuration.GetSection("BaseUrls").Value("Shipping");

            services.AddOptions()
                // .Configure<AppSettings>(Configuration.GetSection("AppSettings"))
                // .Configure<AppSettings>(appSettings =>
                //     {
                //         appSettings.BaseUrls = new BaseUrls()
                //         {
                //             Shipping = new Uri(Configuration["AppSettings:BaseUrls:Shipping"]),
                //             Payment = new Uri(Configuration["AppSettings:BaseUrls:Payment"]),
                //         };
                //     })
                .AddMvc()
                .AddMvcOptions(c => {
                    c.OutputFormatters.Add(new JsonHalOutputFormatter(
                        outputSettings,
                        halJsonMediaTypes: new string[] { "application/hal+json", "application/vnd.example.hal+json", "application/vnd.example.hal.v1+json" }
                    ));
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
