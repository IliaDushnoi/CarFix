using CarFix.Classes;
using CarFix.Windows;
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
    /// Логика взаимодействия для CarPage.xaml
    /// </summary>
    public partial class CarPage : Page
    {
        public CarPage()
        {
            InitializeComponent();

            dgCar.ItemsSource = App.AllCars;
            dgCar.Items.Refresh();
        }
        public void RefreshCarData()
        {
            dgCar.ItemsSource = null;
            dgCar.ItemsSource = App.AllCars;
        }

        private void dgCar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Classes.Car car = dgCar.SelectedItem as Classes.Car;
            if (car != null)
            {
                AddEditCarWindow addEditCarWindow = new AddEditCarWindow(car);
                addEditCarWindow.OwnerPage = this;
                addEditCarWindow.ShowDialog();
            }
        }

        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            if (dgCar.Items.Count == 0)
            {
                MessageBox.Show("Ошибка: Данные автомобилей не загружены!",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            Windows.AddEditCarWindow addEditCarWindow = new Windows.AddEditCarWindow();
            addEditCarWindow.OwnerPage = this;
            addEditCarWindow.ShowDialog();
        }

        private void btnEditCar_Click(object sender, RoutedEventArgs e)
        {
            Classes.Car car = dgCar.SelectedItem as Classes.Car;
            if (car != null)
            {
                AddEditCarWindow addEditCarWindow = new AddEditCarWindow(car);
                addEditCarWindow.OwnerPage = this;
                addEditCarWindow.ShowDialog();
            }
        }

        private void btnDeletCar_Click(object sender, RoutedEventArgs e)
        {
            Classes.Car car = dgCar.SelectedItem as Classes.Car ;
            if (car != null)
            {

                MessageBoxResult result = MessageBox.Show(
                    "Вы точно хотите удалить эти данные?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MessageBox.Show("Данные удалены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    App.AllCars.Remove(car);
                }
                else
                {
                    MessageBox.Show("Удаление отмененно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            dgCar.ItemsSource = null;
            dgCar.ItemsSource = App.AllCars;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dgCar.Items.Refresh();
        }

        private void btnExportCar_Click(object sender, RoutedEventArgs e)
        {
            if (dgCar.Items.Count == 0)
            {
                MessageBox.Show("Ошибка: В таблице Автомобили нечего сохранять!",
                              "Внимание",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            string fileName = "car.json";
            string json = JsonSerializer.Serialize(App.AllCars, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(fileName, json);
            MessageBox.Show("Данные успешно сохранены!", "Экспорт завершен", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnImportCar_Click(object sender, RoutedEventArgs e)
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
                    if (!fileName.Equals("car.json", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Название файла должно быть car.json",
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
                    var importedCars = JsonSerializer.Deserialize<List<Car>>(json, options);

                    if (importedCars == null || !importedCars.Any())
                    {
                        MessageBox.Show("Файл не содержит данных или имеет неверный формат",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Warning);
                        return;
                    }

                    // Обработка импортированных данных
                    foreach (var car in importedCars)
                    {
                        App.AllCars.Add(car);
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
            RefreshCarData();
        }

        private void btnSearchCar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
