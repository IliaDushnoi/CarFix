using CarFix.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
using Path = System.IO.Path;

namespace CarFix.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();
            dgService.ItemsSource = App.AllServices;
            dgService.Items.Refresh();
        }
        public void RefreshServiceData()
        {
            dgService.ItemsSource = null;
            dgService.ItemsSource = App.AllServices;
        }

        private void dgService_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Classes.Service service = dgService.SelectedItem as Classes.Service;
            if (service != null)
            {
                Windows.AddEditServiceWindow addEditServiceWindow = new Windows.AddEditServiceWindow(service);
                addEditServiceWindow.OwnerPage = this;
                addEditServiceWindow.ShowDialog();
            }
        }

        private void btnAddService_Click(object sender, RoutedEventArgs e)
        {
            if (dgService.Items.Count == 0)
            {
                MessageBox.Show("Ошибка: Данные услуг не загружены!",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            Windows.AddEditServiceWindow addEditServiceWindow = new Windows.AddEditServiceWindow();
            addEditServiceWindow.OwnerPage = this;
            addEditServiceWindow.ShowDialog();
        }

        private void btnEditService_Click(object sender, RoutedEventArgs e)
        {
            Classes.Service service = dgService.SelectedItem as Classes.Service;
            if (service != null)
            {
                Windows.AddEditServiceWindow addEditServiceWindow = new Windows.AddEditServiceWindow(service);
                addEditServiceWindow.OwnerPage = this;
                addEditServiceWindow.ShowDialog();
            }
        }

        private void btnDeleteService_Click(object sender, RoutedEventArgs e)
        {
            Classes.Service service = dgService.SelectedItem as Classes.Service;
            if (service != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить эти данные?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Данные удалены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    App.AllServices.Remove(service);
                }
                else
                {
                    MessageBox.Show("Удаление отмененно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                dgService.ItemsSource = null;
                dgService.ItemsSource = App.AllServices;
            }
        }

        private void btnExportService_Click(object sender, RoutedEventArgs e)
        {
            if (dgService.Items.Count == 0)
            {
                MessageBox.Show("Ошибка: В таблице Услуги нечего сохранять!",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            string fileName = "service.json";
            string json = JsonSerializer.Serialize(App.AllServices, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(fileName, json);
            MessageBox.Show("Данные успешно сохранены!", "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnImportService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "JSON files (*.json)|*.json",
                    Title = "Выберите файл для импорта"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    // Проверка имени файла
                    string fileName = Path.GetFileName(openFileDialog.FileName);
                    if (!fileName.Equals("service.json", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Название файла должно быть service.json",
                                        "Ошибка",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                        return;
                    }

                    // Чтение файла
                    string json = File.ReadAllText(openFileDialog.FileName);

                    // Настройки десериализации
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() },
                        NumberHandling = JsonNumberHandling.AllowReadingFromString,
                        ReadCommentHandling = JsonCommentHandling.Skip
                    };

                    // Десериализация
                    var importedServices = JsonSerializer.Deserialize<List<Service>>(json, options);

                    if (importedServices == null || !importedServices.Any())
                    {
                        MessageBox.Show("Файл не содержит данных или имеет неверный формат",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Warning);
                        return;
                    }

                    // Обработка импортированных данных
                    foreach (var service in importedServices)
                    {
                        App.AllServices.Add(service);
                    }

                    MessageBox.Show($"Данные успешно загружены!",
                                  "Импорт завершен",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
            }
            catch (JsonException jsonEx)
            {
                MessageBox.Show($"Ошибка формата JSON: {jsonEx.Message}",
                               "Ошибка",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при импорте: {ex.Message}",
                               "Ошибка",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
            dgService.ItemsSource = null;
            dgService.ItemsSource = App.AllServices;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgService.Items.Refresh();
        }
    }
}
