using CarFix.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFix.Classes
{
    public class Order
    {
        private int id;
        private string clientName;
        private DateTime? createdDate = DateTime.Now;
        private DateTime? completedDate;
        private string service;
        private string status;
        private string totalAmount;

        public Order(int id, string clientName, DateTime? createdDate, DateTime? completedDate, string service, string status, string totalAmount)
        {
            this.id = id;
            this.clientName = clientName;
            this.createdDate = createdDate;
            this.completedDate = completedDate;
            this.service = service;
            this.status = status;
            this.totalAmount = totalAmount;
        }

        public int Id { get => id; set => id = value; }
        public string ClientName { get => clientName; set => clientName = value; }
        public DateTime? CreatedDate { get => createdDate; set => createdDate = value; }
        public DateTime? CompletedDate { get => completedDate; set => completedDate = value; }
        public string Service { get => service; set => service = value; }
        public string Status { get => status; set => status = value; }
        public string TotalAmount { get => totalAmount; set => totalAmount = value; }
    }
}
