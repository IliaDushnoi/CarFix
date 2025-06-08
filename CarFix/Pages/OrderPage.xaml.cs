using CarFix.Classes;
using CarFix.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public OrderPage()
        {
            InitializeComponent();
            dgOrder.ItemsSource = App.AllOrders;
            dgOrder.Items.Refresh();
        }
        public void RefreshOrderData()
        {
            dgOrder.ItemsSource = null;
            dgOrder.ItemsSource = App.AllOrders;
        }

        private void dgOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Classes.Order order = dgOrder.SelectedItem as Classes.Order;
            if (order != null)
            {
                AddEditOrderWindow addEditOrderWindow = new AddEditOrderWindow(order);
                addEditOrderWindow.OwnerPage = this;
                addEditOrderWindow.ShowDialog();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshOrderData();
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrder.Items.Count == 0)
            {
                MessageBox.Show("Ошибка: Данные заказов не загружены!",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            AddEditOrderWindow addEditOrderWindow = new AddEditOrderWindow();
            addEditOrderWindow.OwnerPage = this;
            addEditOrderWindow.ShowDialog();
        }

        private void btnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            Classes.Order order = dgOrder.SelectedItem as Classes.Order;
            if (order != null)
            {
                AddEditOrderWindow addEditOrderWindow = new AddEditOrderWindow(order);
                addEditOrderWindow.OwnerPage = this;
                addEditOrderWindow.ShowDialog();
            }
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            Classes.Order order = dgOrder.SelectedItem as Classes.Order;
            if (order != null)
            {

                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить эти данные?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Данные удалены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    App.AllOrders.Remove(order);
                }
                else
                {
                    MessageBox.Show("Удаление отмененно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            RefreshOrderData();
        }

        private void btnExportOrder_Click(object sender, RoutedEventArgs e)
        {
            if (dgOrder.Items.Count == 0)
            {
                MessageBox.Show("Ошибка: В таблице Заказы нечего сохранять!",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            string fileName = "order.json";
            string json = JsonSerializer.Serialize(App.AllOrders, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(fileName, json);
            MessageBox.Show("Данные успешно сохранены!", "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnImportOrder_Click(object sender, RoutedEventArgs e)
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
                    if (!fileName.Equals("order.json", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Название файла должно быть order.json",
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
                    var importedOrders = JsonSerializer.Deserialize<List<Order>>(json, options);

                    if (importedOrders == null || !importedOrders.Any())
                    {
                        MessageBox.Show("Файл не содержит данных или имеет неверный формат",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Warning);
                        return;
                    }

                    // Обработка импортированных данных
                    foreach (var order in importedOrders)
                    {
                        App.AllOrders.Add(order);
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
            RefreshOrderData();
        }

        private void btnSearchOrder_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
