using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TechZoneProject.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        // Основные данные заказа
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        // Состав или примечания
        public string OrderDetails { get; set; }
    }
}