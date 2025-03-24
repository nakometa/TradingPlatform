namespace Shared.Contracts
{
    public record OrderRequest
    {
        public int UserId { get; init; }
        public string Ticker { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public string Side { get; init; } = string.Empty;
    }
}
