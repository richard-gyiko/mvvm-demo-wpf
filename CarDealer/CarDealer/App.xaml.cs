using CarDealer.ViewModel;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows;

namespace CarDealer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal IHost Host { get; }

        public App()
        {
            Host = new HostBuilder()
                 .ConfigureAppConfiguration((context, configurationBuilder) =>
                 {
                     configurationBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath);
                     configurationBuilder.AddJsonFile("appsettings.json", optional: true);
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddValidatorsFromAssembly(typeof(ObservableObject).Assembly);
                    services.AddTransient<InventoryViewModel>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddDebug();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await Host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (Host)
            {
                await Host.StopAsync();
            }

            base.OnExit(e);
        }
    }
}
