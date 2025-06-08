using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFix.Classes
{
    public class Service
    {
        private int id;
        private string name;
        private string description;
        private string price;

        public Service(int id, string name, string description, string price)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.price = price;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string Price { get => price; set => price = value; }
    }
}
