namespace DesignStudio.API.DTOs
{
    public class OrderDtoApi
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public bool IsTurnkey { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public DateTime OrderDate { get; set; }
    }
}