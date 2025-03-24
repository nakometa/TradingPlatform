using System.Collections.Concurrent;

namespace Shared.Contracts
{
    public class PriceCache : IPriceCache
    {
        private readonly ConcurrentDictionary<string, decimal> _prices = new();

        public void UpdatePrice(string ticker, decimal price)
        {
            _prices.AddOrUpdate(ticker, price, (key, oldValue) => price);
        }

        public bool TryGetPrice(string ticker, out decimal price)
        {
            return _prices.TryGetValue(ticker, out price);
        }
    }
}
