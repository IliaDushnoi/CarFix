using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFix.Classes
{
    public class Car
    {
        private int id;
        private string brand;
        private string model;
        private string year;
        private string vin;
        private string mileage;

        public Car(int id, string brand, string model, string year, string vin, string mileage)
        {
            this.id = id;
            this.brand = brand;
            this.model = model;
            this.year = year;
            this.vin = vin;
            this.mileage = mileage;
        }

        public int Id { get => id; set => id = value; }
        public string Brand { get => brand; set => brand = value; }
        public string Model { get => model; set => model = value; }
        public string Year { get => year; set => year = value; }
        public string Vin { get => vin; set => vin = value; }
        public string Mileage { get => mileage; set => mileage = value; }
    }
}
