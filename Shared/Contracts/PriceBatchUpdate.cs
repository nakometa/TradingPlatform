namespace Shared.Contracts
{
    public record PriceBatchUpdate
    {
        public List<PriceUpdate> Prices { get; init; } = new();
        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
    }
}
