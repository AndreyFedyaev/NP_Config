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

namespace NP_Config
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        WorkPage WP1 = new WorkPage();
        WorkPage WP2 = new WorkPage();
        WorkPage WP3 = new WorkPage();
        WorkPage WP4 = new WorkPage();
        WorkPage WP5 = new WorkPage();
        WorkPage WP6 = new WorkPage();
        WorkPage WP7 = new WorkPage();
        WorkPage WP8 = new WorkPage();
        WorkPage WP9 = new WorkPage();
        WorkPage WP10 = new WorkPage();
        WorkPage WP11 = new WorkPage();
        WorkPage WP12 = new WorkPage();
        WorkPage WP13 = new WorkPage();
        WorkPage WP14 = new WorkPage();
        WorkPage WP15 = new WorkPage();
        WorkPage WP16 = new WorkPage();

        private void NP1_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP1);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP2_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP2);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP3_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP3);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP4_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP4);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP5_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP5);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP6_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP6);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP7_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP7);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP8_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP8);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP9_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP9);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP10_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP10);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP11_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP11);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP12_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP12);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP13_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP13);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP14_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP14);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP15_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP15);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }

        private void NP16_Click(object sender, RoutedEventArgs e)
        {
            WorkMainPage_Frame.NavigationService.Navigate(WP16);
            WorkMainPage_Frame.NavigationService.RemoveBackEntry();
        }
    }
}
