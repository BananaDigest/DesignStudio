namespace DesignStudio.DAL.Models
{
    public class PortfolioItem
    {
        public int PortfolioItemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }

        // Nullable — бо для замовлення під ключ зв'язку може не бути
        public int? DesignServiceId { get; set; }
        public virtual DesignService? DesignService { get; set; }
    }
}
