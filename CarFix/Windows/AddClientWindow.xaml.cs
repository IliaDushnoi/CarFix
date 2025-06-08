using CarFix.Classes;
using CarFix.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {
        public ClientPage OwnerPage { get; set; }

        public AddClientWindow()
        {
            InitializeComponent();
            currentClient = new Classes.Client(0, "", "", "", null, "");
        }
        public AddClientWindow(Classes.Client client)
        {
            InitializeComponent();
            currentClient = client;

            tbFullNameClient.Text = client.FullName;
            tbPhoneNumberClient.Text = client.PhoneNumber;
            tbEmailClient.Text = client.Email;
            tbVinClient.Text = client.CarVIN;
        }

        Classes.Client currentClient;

        private void btnSaveClient_Click(object sender, RoutedEventArgs e)
        {
            //Валидация 
            //"ФИО обязательно для заполнения"
            //"Номер телефона обязателен"
            //"VIN номер обязателен"

            if (string.IsNullOrEmpty(tbFullNameClient.Text))
            {
                MessageBox.Show("ФИО обязательно для заполнения",
                              "Ошибка валидации",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(tbPhoneNumberClient.Text))
            {
                MessageBox.Show("Номер телефона обязателен",
                              "Ошибка валидации",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(tbVinClient.Text))
            {
                MessageBox.Show("VIN номер обязателен",
                              "Ошибка валидации",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            else if (tbVinClient.Text.Length != 17)
            {
                MessageBox.Show("VIN номер должен содержать ровно 17 символов",
                      "Ошибка валидации",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning);
                return;
            }


            if (currentClient.ID == 0)
            {
                currentClient.FullName = tbFullNameClient.Text;
                currentClient.PhoneNumber = tbPhoneNumberClient.Text;
                currentClient.Email = tbEmailClient.Text;
                currentClient.RegistrationDate = DateTime.Now;
                currentClient.CarVIN = tbVinClient.Text;
                if (App.AllClients.Count == 0)
                {
                    currentClient.ID = 1;
                }
                else
                {
                    currentClient.ID = App.AllClients.OrderByDescending(a => a.ID).First().ID + 1;
                }
                App.AllClients.Add(currentClient);
            }
            else
            {
                currentClient.FullName = tbFullNameClient.Text;
                currentClient.PhoneNumber = tbPhoneNumberClient.Text;
                currentClient.Email = tbEmailClient.Text;
                currentClient.RegistrationDate = DateTime.Now;
                currentClient.CarVIN = tbVinClient.Text;
            }
            

            Close();

            OwnerPage?.RefreshClientData();

            MessageBox.Show("Данные клиента успешно добавлены/изменены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
