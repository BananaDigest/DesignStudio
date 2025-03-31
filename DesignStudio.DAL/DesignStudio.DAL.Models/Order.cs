using System;

namespace DesignStudio.DAL.Models
{
    // Сутність, що описує замовлення послуг дизайну
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }  // Ім’я клієнта
        public DateTime OrderDate { get; set; }     // Дата замовлення
        public bool IsTurnkey { get; set; }         // Чи замовлено «дизайн під ключ»

        public virtual ICollection<DesignService> DesignServices { get; set; } = new List<DesignService>();
    }
}