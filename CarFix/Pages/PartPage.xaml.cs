using CarFix.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
    /// Логика взаимодействия для PartPage.xaml
    /// </summary>
    public partial class PartPage : Page
    {
        public PartPage()
        {
            InitializeComponent();
            dgPart.ItemsSource = App.AllParts;
            dgPart.Items.Refresh();
        }

        private void dgPart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Classes.Part part = dgPart.SelectedItem as Classes.Part;
            if (part != null)
            {
                Windows.AddEditPartWindow addEditPartWindow = new Windows.AddEditPartWindow(part);
                addEditPartWindow.OwnerPage = this;
                addEditPartWindow.ShowDialog();
            }
        }
        public void RefreshPartData()
        {
            dgPart.ItemsSource = null;
            dgPart.ItemsSource = App.AllParts;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgPart.Items.Refresh();
        }

        private void btnAddPart_Click(object sender, RoutedEventArgs e)
        {
            if (dgPart.Items.Count == 0)
            {
                MessageBox.Show("Ошибка: Данные запчастей не загружены!",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            Windows.AddEditPartWindow addEditPartWindow = new Windows.AddEditPartWindow();
            addEditPartWindow.OwnerPage = this;
            addEditPartWindow.ShowDialog();
        }

        private void btnEditPart_Click(object sender, RoutedEventArgs e)
        {
            Classes.Part part = dgPart.SelectedItem as Classes.Part;
            if (part != null)
            {
                Windows.AddEditPartWindow addEditPartWindow = new Windows.AddEditPartWindow(part);
                addEditPartWindow.OwnerPage = this;
                addEditPartWindow.ShowDialog();
            }
        }

        private void btnDeletePart_Click(object sender, RoutedEventArgs e)
        {
            Classes.Part part = dgPart.SelectedItem as Classes.Part;
            if (part != null)
            {

                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить эти данные?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Данные удалены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    App.AllParts.Remove(part);
                }
                else
                {
                    MessageBox.Show("Удаление отмененно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            dgPart.ItemsSource = null;
            dgPart.ItemsSource = App.AllParts;
        }

        private void btnExportPart_Click(object sender, RoutedEventArgs e)
        {
            if (dgPart.Items.Count == 0)
            {
                MessageBox.Show("Ошибка: В таблице Запчасти нечего сохранять!",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            string fileName = "part.json";
            string json = JsonSerializer.Serialize(App.AllParts, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(fileName, json);
            MessageBox.Show("Данные успешно сохранены!", "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnImportPart_Click(object sender, RoutedEventArgs e)
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
                    if (!fileName.Equals("part.json", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Название файла должно быть part.json",
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
                    var importedPart = JsonSerializer.Deserialize<List<Part>>(json, options);

                    if (importedPart == null || !importedPart.Any())
                    {
                        MessageBox.Show("Файл не содержит данных или имеет неверный формат",
                                        "Ошибка",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                        return;
                    }

                    // Обработка импортированных данных
                    foreach (var part in importedPart)
                    {
                        App.AllParts.Add(part);
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
            dgPart.ItemsSource = null;
            dgPart.ItemsSource = App.AllParts;
        }
    }
}
