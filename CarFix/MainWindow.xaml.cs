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

namespace CarFix
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



        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            Pages.ClientPage page = new Pages.ClientPage();
            mainFrame.Navigate(page);
        }

        private void btnCar_Click(object sender, RoutedEventArgs e)
        {
            Pages.CarPage page = new Pages.CarPage();
            mainFrame.Navigate(page);
        }

        private void btnService_Click(object sender, RoutedEventArgs e)
        {
            Pages.ServicePage page = new Pages.ServicePage();
            mainFrame.Navigate(page);
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            Pages.OrderPage page = new Pages.OrderPage();
            mainFrame.Navigate(page);
        }

        private void btnPart_Click(object sender, RoutedEventArgs e)
        {
            Pages.PartPage page = new Pages.PartPage();
            mainFrame.Navigate(page);
        }
    }
}
