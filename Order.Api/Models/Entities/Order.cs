namespace Order.Api.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Ticker { get; set; }

        public int Quantity { get; set; }

        public decimal PricePerUnit { get; set; }

        public decimal Total { get; set; }

        public string Side { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
