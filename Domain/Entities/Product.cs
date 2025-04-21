using Domain.Common;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        // ارتباط با Category
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
