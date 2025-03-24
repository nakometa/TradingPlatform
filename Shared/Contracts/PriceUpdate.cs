namespace Shared.Contracts
{
    public class PriceUpdate
    {
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
