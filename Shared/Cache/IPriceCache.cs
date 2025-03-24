namespace Shared.Contracts
{
    public interface IPriceCache
    {
        void UpdatePrice(string ticker, decimal price);
        bool TryGetPrice(string ticker, out decimal price);
    }
}
