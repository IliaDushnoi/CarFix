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
    /// Логика взаимодействия для AddEditCarWindow.xaml
    /// </summary>
    public partial class AddEditCarWindow : Window
    {
        public CarPage OwnerPage { get; set; }
        Classes.Car currentCar;
        public AddEditCarWindow()
        {
            InitializeComponent();
            currentCar = new Classes.Car(0, "", "", "", "", "");
        }
        public AddEditCarWindow(Classes.Car car)
        {
            InitializeComponent();
            currentCar = car;

            tbBrand.Text = car.Brand;
            tbModel.Text = car.Model;
            tbYear.Text = car.Year.ToString();
            tbVIN.Text = car.Vin;
            tbMileage.Text = car.Mileage.ToString();
        }

        private void btnSaveClient_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbBrand.Text))
            {
                MessageBox.Show("Марка автомобиля обязательна",
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(tbModel.Text))
            {
                MessageBox.Show("Модель автомобиля обязательна",
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(tbVIN.Text))
            {
                MessageBox.Show("VIN номер обязателен",
                              "Ошибка валидации",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return;
            }
            else if (tbVIN.Text.Length != 17)
            {
                MessageBox.Show("VIN номер должен содержать ровно 17 символов",
                      "Ошибка валидации",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning);
                return;
            }
            else if (tbMileage.Text.Length > 6)
            {
                MessageBox.Show("Пробег не может быть больше 999.999км",
                      "Ошибка валидации",
                      MessageBoxButton.OK,
                      MessageBoxImage.Warning);
                return;
            }

            if (currentCar.Id == 0)
            {
                currentCar.Brand = tbBrand.Text;
                currentCar.Model = tbModel.Text;
                currentCar.Year = tbYear.Text;
                currentCar.Vin = tbVIN.Text;
                currentCar.Mileage = tbMileage.Text;
                if (App.AllCars.Count == 0)
                {
                    currentCar.Id = 1;
                }
                else
                {
                    currentCar.Id = App.AllCars.OrderByDescending(a => a.Id).First().Id+ 1;
                }
                App.AllCars.Add(currentCar);
            }
            else
            {
                currentCar.Brand = tbBrand.Text;
                currentCar.Model = tbModel.Text;
                currentCar.Year = tbYear.Text;
                currentCar.Vin = tbVIN.Text;
                currentCar.Mileage = tbMileage.Text;
            }


            Close();

            OwnerPage?.RefreshCarData();

            MessageBox.Show("Данные об автомобиле успешно добавлены/изменены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
