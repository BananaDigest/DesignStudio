namespace DesignStudio.DAL.Models
{
    // Сутність, яка описує послугу дизайну
    public class DesignService
    {
        public int DesignServiceId { get; set; }
        public string Name { get; set; }          // Назва послуги
        public string Description { get; set; }   // Опис послуги
        public decimal Price { get; set; }        // Вартість послуги

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<PortfolioItem> PortfolioItems { get; set; } = new List<PortfolioItem>();
    }
}
