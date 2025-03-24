namespace Portfolio.Api.Models.Entities
{
    public class UserStock
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int Quantity { get; set; }


        public int CatalogId { get; set; }
        public StockCatalog StockCatalog { get; set; }
    }
}
