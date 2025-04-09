using System;
using System.Collections.Generic;

namespace DesignStudio.DAL.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public DateTime OrderDate { get; set; }

        public bool IsTurnkey { get; set; }
        public string? DesignRequirement { get; set; } // Під ключ
        public string? DesignDescription { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.InProcess;

        // З переліку
        public virtual ICollection<DesignService> DesignServices { get; set; } = new List<DesignService>();
    }
}
