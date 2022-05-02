using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StreetLightManager;
using Observer.ViewModels;
using System.Threading;

namespace Observer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = new MainWindowViewModel();
            StreetLightManager.StreetlightManager.Start();
            StreetLightManager.StreetlightManager.DigitsChanged += async () =>
            {
                await Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    (DataContext as MainWindowViewModel)?.UpdateDigitsSegments();
                }));
                
            };

            StreetLightManager.StreetlightManager.ColorChanged += async () =>
            {
                await Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    (DataContext as MainWindowViewModel)?.UpdateColor();
                }));

            };

            RestSharpManager.RestSharpManager.ExternalMessage += async (string s) =>
            {
                await Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    (DataContext as MainWindowViewModel)?.AddMessage(s);
                }));

            };

            
        }
    }
}
