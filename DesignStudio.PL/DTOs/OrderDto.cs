using System;
using System.Collections.Generic;

namespace DesignStudio.PL.DTOs
{
    public class OrderDto { public int Id { get; set; } public string CustomerName { get; set; } public string Phone { get; set; } public bool IsTurnkey { get; set; } public string DesignRequirement { get; set; } public string DesignDescription { get; set; } public List<DesignServiceDto> Services { get; set; } public DateTime OrderDate { get; set; } }
}