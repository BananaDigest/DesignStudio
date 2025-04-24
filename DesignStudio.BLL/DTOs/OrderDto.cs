using System;
using System.Collections.Generic;

namespace DesignStudio.BLL.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public bool IsTurnkey { get; set; }
        public string? DesignRequirement { get; set; }
        public string? DesignDescription { get; set; }
        public List<DesignServiceDto> Services { get; set; } = new();
    }
}
