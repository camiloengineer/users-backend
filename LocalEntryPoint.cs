using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace User.Backend.Api
{
    /// <summary>
    /// The Main function can be used to run the ASP.NET Core application locally using the Kestrel webserver.
    /// </summary>
    public class LocalEntryPoint
    {
        public static void Main(string[] args)
        { 
            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Error()
            .WriteTo.Console()
            .CreateLogger();

            try
            {
                Log.Information("Iniciando la aplicación");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error al inicar la aplicacion");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .UseSerilog()
                .UseContentRoot(Directory.GetCurrentDirectory())
                 .ConfigureAppConfiguration((hostContext, config) =>
                 {
                     config.AddEnvironmentVariables();
                     var configuration = config.Build();

                     Log.Logger = new LoggerConfiguration()
                     .ReadFrom.Configuration(configuration)
                     .Enrich.FromLogContext()
                     .MinimumLevel.Error()
                     .WriteTo.Console()
                     .CreateLogger();
                 })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddSerilog(dispose: true);
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.PreferHostingUrls(true);
                });   
    }
}
