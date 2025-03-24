namespace Portfolio.Api.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        public ICollection<UserStock> UserStocks { get; set; } = new List<UserStock>();
    }
}
