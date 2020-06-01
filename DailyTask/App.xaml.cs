using DailyTask.View;
using DailyTask.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DailyTask
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// https://marcominerva.wordpress.com/2019/11/07/update-on-using-hostbuilder-dependency-injection-and-service-provider-with-net-core-3-0-wpf-applications/
    /// </summary>
    public partial class App : Application
    {
        //private readonly IHost host;
        //todo. wangjc. singleton!
        public static IHost IOC;

        public App()
        {
            IOC = Host.CreateDefaultBuilder()
                   .ConfigureServices((context, services) =>
                   {
                       ConfigureServices(context.Configuration, services);
                   })
                   .ConfigureLogging(logBuilder =>
                   {
                       logBuilder.SetMinimumLevel(LogLevel.Information);
                       logBuilder.AddNLog("nLog.config");
                   })
                   .Build();
        }

        private void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<View.NewRecordView>();
            services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
            services.AddScoped<ISampleService, SampleService>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await IOC.StartAsync();

            var mainWindow = IOC.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (IOC)
            {
                await IOC.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}
