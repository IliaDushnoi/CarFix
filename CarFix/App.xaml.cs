using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CarFix
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<Classes.Client> AllClients = new List<Classes.Client>();
        public static List<Classes.Car> AllCars = new List<Classes.Car>();
        public static List<Classes.Service> AllServices = new List<Classes.Service>();
        public static List<Classes.Order> AllOrders = new List<Classes.Order>();
        public static List<Classes.Part> AllParts = new List<Classes.Part>();
    }
}
