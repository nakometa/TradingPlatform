namespace Shared.Contracts
{
    public record OrderResponse
    {
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
