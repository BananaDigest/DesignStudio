namespace DesignStudio.API.DTOs
{
    public class PortfolioItemDtoApi
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}