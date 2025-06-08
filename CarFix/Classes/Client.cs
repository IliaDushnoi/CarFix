using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarFix.Classes
{
    public class Client
    {
        private int iD;
        private string fullName;
        private string phoneNumber;
        private string email;
        private DateTime? registrationDate;
        private string carVIN;

     

        public Client(int iD, string fullName, string phoneNumber, string email, DateTime? registrationDate, string carVIN)
        {
            
            ID = iD;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Email = email;
            RegistrationDate = DateTime.Now;
            CarVIN = carVIN;
        }

        public int ID { get => iD; set => iD = value; }
        public string FullName { get => fullName; set => fullName = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Email { get => email; set => email = value; }
        public DateTime? RegistrationDate { get => registrationDate; set => registrationDate = value; }
        public string CarVIN { get => carVIN; set => carVIN = value; }


    }
}
