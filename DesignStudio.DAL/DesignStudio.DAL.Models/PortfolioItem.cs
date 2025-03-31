namespace DesignStudio.DAL.Models
{
    // Сутність, що описує роботу в портфоліо
    public class PortfolioItem
    {
        public int PortfolioItemId { get; set; }
        public string Title { get; set; }         // Заголовок роботи
        public string Description { get; set; }   // Опис роботи
        public string ImageUrl { get; set; }        // Посилання на зображення роботи

        public int DesignServiceId { get; set; }
        public virtual DesignService DesignService { get; set; }
    }
}
