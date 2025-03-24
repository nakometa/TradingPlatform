namespace Portfolio.Api.Models.DTOs
{
    public class UserStockDto
    {
        public string Ticker { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal Total => Quantity * PricePerUnit;
    }
}
