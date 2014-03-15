using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Shingles
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup_1(object sender, StartupEventArgs e)
        {
            var view = new MainView();

            var viewModel = new MainViewModel(new ShingleResolver());

            view.DataContext = viewModel;

            view.Show();
        }
    }
}
