using CarDealer.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CarDealer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var host = (Application.Current as App).Host;
            var viewModel = host.Services.GetRequiredService<InventoryViewModel>();

            DataContext = viewModel;
        }
    }
}
