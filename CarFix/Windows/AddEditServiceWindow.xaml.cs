using CarFix.Pages;
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
using System.Windows.Shapes;

namespace CarFix.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditServiceWindow.xaml
    /// </summary>
    public partial class AddEditServiceWindow : Window
    {
        public ServicePage OwnerPage { get; set; }
        Classes.Service currentService;
        public AddEditServiceWindow()
        {
            InitializeComponent();
            currentService = new Classes.Service(0, "", "", "");
        }
        public AddEditServiceWindow(Classes.Service service)
        {
            InitializeComponent();
            currentService = service;

            tbNameService.Text = service.Name;
            tbDescription.Text = service.Description;
            tbPrice.Text = service.Price;
        }

        private void btnSaveService_Click(object sender, RoutedEventArgs e)
        {
            if (currentService.Id == 0)
            {
                currentService.Name = tbNameService.Text;
                currentService.Description = tbDescription.Text;
                currentService.Price = tbPrice.Text;
                if (App.AllServices.Count == 0)
                {
                    currentService.Id = 1;
                }
                else
                {
                    currentService.Id = App.AllServices.OrderByDescending(a => a.Id).First().Id + 1;
                }
                App.AllServices.Add(currentService);
            }
            else
            {
                currentService.Name = tbNameService.Text;
                currentService.Description = tbDescription.Text;
                currentService.Price = tbPrice.Text;
            }

            Close();

            OwnerPage?.RefreshServiceData();

            MessageBox.Show("Данные об услуге успешно добавлены/изменены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
