using CarFix.Classes;
using CarFix.Windows;
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
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using Path = System.IO.Path;

namespace CarFix.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();
            dgClient.ItemsSource = App.AllClients;
            dgClient.Items.Refresh();
        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            if (dgClient.Items.Count == 0)
            {
                MessageBox.Show("Ошибка: Данные клиентов не загружены!",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            Windows.AddClientWindow addClientWindow = new Windows.AddClientWindow();
            addClientWindow.OwnerPage = this;
            addClientWindow.ShowDialog();
        }

        private void btnEditClient_Click(object sender, RoutedEventArgs e)
        {
            Classes.Client client = dgClient.SelectedItem as Classes.Client;
            if (client != null)
            {
                AddClientWindow addClientWindow = new AddClientWindow(client);
                addClientWindow.OwnerPage = this;
                addClientWindow.ShowDialog();
            }
        }

        private void btnDeletClient_Click(object sender, RoutedEventArgs e)
        {
            Classes.Client client = dgClient.SelectedItem as Classes.Client;
            if (client != null)
            {
                
                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить эти данные?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Данные удалены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    App.AllClients.Remove(client);
                }
                else
                {
                    MessageBox.Show("Удаление отмененно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            dgClient.ItemsSource = null;
            dgClient.ItemsSource = App.AllClients;
        }

        private void btnExportClient_Click(object sender, RoutedEventArgs e)
        {
            if (dgClient.Items.Count == 0)
            {
                MessageBox.Show("Ошибка: В таблице Клиенты нечего сохранять!",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            string fileName = "client.json";
            string json = JsonSerializer.Serialize(App.AllClients, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(fileName, json);
            MessageBox.Show("Данные успешно сохранены!", "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgClient.Items.Refresh();
        }
        public void RefreshClientData()
        {
            dgClient.ItemsSource = null;
            dgClient.ItemsSource = App.AllClients;
        }

        private void dgClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Classes.Client client = dgClient.SelectedItem as Classes.Client;
            if (client != null)
            {
                AddClientWindow addClientWindow = new AddClientWindow(client);
                addClientWindow.OwnerPage = this;
                addClientWindow.ShowDialog();   
            }
        }

        private void btnImportClient_Click(object sender, RoutedEventArgs e)
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
                    if (!fileName.Equals("client.json", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Название файла должно быть client.json",
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
                    var importedClients = JsonSerializer.Deserialize<List<Client>>(json, options);

                    if (importedClients == null || !importedClients.Any())
                    {
                        MessageBox.Show("Файл не содержит данных или имеет неверный формат",
                                        "Ошибка",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                        return;
                    }

                    // Обработка импортированных данных
                    foreach (var client in importedClients)
                    {
                        App.AllClients.Add(client);
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
            dgClient.ItemsSource = null;
            dgClient.ItemsSource = App.AllClients;
        }

        private void btnSearchClient_Click(object sender, RoutedEventArgs e)
        {
            SearchClientsByName(tbSearchClient.Text);
        }

        // Функция поиска клиентов по имени
        private void SearchClientsByName(string searchText)
        {
            // Если строка поиска пустая - показываем всех клиентов
            if (string.IsNullOrWhiteSpace(searchText))
            {
                dgClient.ItemsSource = App.AllClients;
                return;
            }

            // Ищем клиентов, содержащих введенный текст (без учета регистра)
            var filteredClients = App.AllClients
                .Where(client => client.FullName != null &&
                       client.FullName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            // Обновляем DataGrid
            dgClient.ItemsSource = filteredClients;
        }

        // Обработчик для TextBox поиска (можно привязать к TextChanged)
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchClientsByName(tbSearchClient.Text);
        }
    }
}
