namespace Order.Api.Models.DTOs
{
    public class CreateOrderDto
    {
        public string Ticker { get; set; }

        public int Quantity { get; set; }

        public string Side { get; set; }
    }
}
