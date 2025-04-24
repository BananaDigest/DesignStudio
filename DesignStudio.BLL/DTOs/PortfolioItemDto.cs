namespace DesignStudio.BLL.DTOs
{
    public class PortfolioItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DesignServiceDto? Service { get; set; }
    }
}
