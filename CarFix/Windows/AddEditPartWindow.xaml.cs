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
    /// Логика взаимодействия для AddEditPartWindow.xaml
    /// </summary>
    public partial class AddEditPartWindow : Window
    {
        public PartPage OwnerPage { get; set; }
        Classes.Part currentPart;
        public AddEditPartWindow()
        {
            InitializeComponent();
            currentPart = new Classes.Part(0, "", "", "", 0);
        }
        public AddEditPartWindow(Classes.Part part)
        {
            InitializeComponent();
            currentPart = part;

            tbPartName.Text = part.Name;
            tbPartDescription.Text = part.Description;
            tbPartPrice.Text = part.Price;
            tbQuantityInStock.Text = part.QuantityInStock.ToString();
        }

        private void btnSavePart_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbPartName.Text))
            {
                MessageBox.Show("Обязательно укажите название запчасти!",
                                 "Ошибка валидации",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(tbPartPrice.Text))
            {
                MessageBox.Show("Обязательно укажите стоимость запчасти!",
                                 "Ошибка валидации",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(tbQuantityInStock.Text))
            {
                MessageBox.Show("Обязательно укажите количество запчастей на складе! \n(Можно указать 0)",
                                 "Ошибка валидации",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Warning);
                return;
            }
            if (currentPart.Id == 0)
            {
                currentPart.Name = tbPartName.Text;
                currentPart.Description = tbPartDescription.Text;
                currentPart.Price = tbPartPrice.Text;
                currentPart.QuantityInStock = int.Parse(tbQuantityInStock.Text);
                if (App.AllParts.Count == 0)
                {
                    currentPart.Id = 1;
                }
                else
                {
                    currentPart.Id = App.AllParts.OrderByDescending(a => a.Id).First().Id + 1;
                }
                App.AllParts.Add(currentPart);
            }
            else
            {
                currentPart.Name = tbPartName.Text;
                currentPart.Description = tbPartDescription.Text;
                currentPart.Price = tbPartPrice.Text;
                currentPart.QuantityInStock = int.Parse(tbQuantityInStock.Text);
            }

            Close();

            OwnerPage?.RefreshPartData();

            MessageBox.Show("Данные успешно добавлены/изменены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
