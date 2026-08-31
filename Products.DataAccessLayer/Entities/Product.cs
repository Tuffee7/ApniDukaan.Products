using System.ComponentModel.DataAnnotations;

namespace Products.DataLayer.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
        public int? QuantityInStock { get; set; }
    }
}
