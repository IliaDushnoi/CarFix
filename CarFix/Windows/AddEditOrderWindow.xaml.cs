using CarFix.Classes;
using CarFix.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Логика взаимодействия для AddEditOrderWindow.xaml
    /// </summary>
    public partial class AddEditOrderWindow : Window
    {
        public OrderPage OwnerPage { get; set; }
        Classes.Order currentOrder;
        List<string> clientName = new List<string>();
        List<Service> services = new List<Service>();
        List<string> serviceNames = new List<string>();
        List<string> status = new List<string>() {"Новый", "В работе", "Завершен", "Отменен"};


        public AddEditOrderWindow()
        {
            InitializeComponent();
            LoadClientsFromJson();
            LoadServicesFromJson();
            cmbStatus.ItemsSource = status;
            currentOrder = new Classes.Order(0, "", null, null, "", "", "");

            cmbService.SelectionChanged += CmbServiceName_SelectionChanged;
        }
        public AddEditOrderWindow(Classes.Order order)
        {
            InitializeComponent();
            LoadClientsFromJson();
            LoadServicesFromJson();
            cmbStatus.ItemsSource = status;
            currentOrder = order;
            cmbClientName.ItemsSource = clientName;
            cmbClientName.Text = order.ClientName;
            dpCompletedOrder.Text = order.CompletedDate.ToString();
            cmbService.ItemsSource = serviceNames;
            cmbService.Text = order.Service;
            cmbStatus.Text = order.Status;
            tbTotalAmount.Text = order.TotalAmount;

            cmbService.SelectionChanged += CmbServiceName_SelectionChanged;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cmbClientName.Text))
            {
                MessageBox.Show("ФИО клиента обязательно для заполнения",
                              "Ошибка валидации",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(dpCompletedOrder.Text))
            {
                MessageBox.Show("Установите дату (можно установить текущею)",
                              "Ошибка валидации",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(cmbService.Text))
            {
                MessageBox.Show("Услуга обязательна для заполнения",
                              "Ошибка валидации",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(cmbStatus.Text))
            {
                cmbStatus.Text = "Новый";
            }
            if (currentOrder.Id == 0)
            {
                currentOrder.ClientName = cmbClientName.Text;
                currentOrder.CreatedDate = DateTime.Now;
                currentOrder.CompletedDate = DateTime.Parse(dpCompletedOrder.Text);
                currentOrder.Service = cmbService.Text;
                currentOrder.Status = cmbStatus.Text;
                currentOrder.TotalAmount = tbTotalAmount.Text;
                if (App.AllOrders.Count == 0)
                {
                    currentOrder.Id = 1;
                }
                else
                {
                    currentOrder.Id = App.AllOrders.OrderByDescending(a => a.Id).First().Id + 1;
                }
                App.AllOrders.Add(currentOrder);
            }
            else
            {
                currentOrder.ClientName = cmbClientName.Text;
                currentOrder.CreatedDate = DateTime.Now;
                currentOrder.CompletedDate = DateTime.Parse(dpCompletedOrder.Text);
                currentOrder.Service = cmbService.Text;
                currentOrder.Status = cmbStatus.Text;
                currentOrder.TotalAmount = tbTotalAmount.Text;
            }
            Close();

            OwnerPage?.RefreshOrderData();

            MessageBox.Show("Запись успешно добавлена/изменена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        private void LoadClientsFromJson()
        {
            try
            {
                string jsonFilePath = "client.json"; // Путь к файлу
                if (!File.Exists(jsonFilePath))
                {
                    MessageBox.Show("Файл client.json не найден!");
                    return;
                }

                // Чтение JSON и десериализация
                string jsonData = File.ReadAllText(jsonFilePath);
                List<Client> clients = JsonSerializer.Deserialize<List<Client>>(jsonData);

                // Извлекаем имена и добавляем в список
                clientName = clients.Select(c => c.FullName).ToList();

                // Привязка к ComboBox
                cmbClientName.ItemsSource = clientName;
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Ошибка формата JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}");
            }
        }

        private void LoadServicesFromJson()
        {
            try
            {
                string jsonFilePath = "service.json";

                if (!File.Exists(jsonFilePath))
                {
                    MessageBox.Show("Файл service.json не найден!");
                    return;
                }

                string jsonData = File.ReadAllText(jsonFilePath);
                services = JsonSerializer.Deserialize<List<Service>>(jsonData);

                serviceNames = services.Select(s => s.Name).ToList();
                cmbService.ItemsSource = serviceNames;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки услуг: {ex.Message}");
            }
        }

        private void CmbServiceName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbService.SelectedItem == null) return;

            string selectedServiceName = cmbService.SelectedItem.ToString();

            // Находим соответствующую услугу в списке
            Service selectedService = services.FirstOrDefault(s => s.Name == selectedServiceName);

            if (selectedService != null)
            {
                // Выводим цену в TextBox
                tbTotalAmount.Text = selectedService.Price.ToString();
            }
        }
    }
}
