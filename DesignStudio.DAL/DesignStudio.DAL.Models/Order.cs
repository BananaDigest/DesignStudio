using System;

namespace DesignStudio.DAL.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public DateTime OrderDate { get; set; }

        // Прапорець, що визначає тип замовлення (під ключ або з переліку)
        public bool IsTurnkey { get; set; }

        // Статус замовлення (за замовчуванням "В процесі")
        public OrderStatus Status { get; set; } = OrderStatus.InProcess;

        // Для замовлень "під ключ"
        public string DesignRequirement { get; set; }
        public string DesignDescription { get; set; }

        // Замовлення з переліку може містити послуги
        public virtual ICollection<DesignService> DesignServices { get; set; } = new List<DesignService>();
    }
}