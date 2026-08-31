using Products.Business.Common;

namespace Products.Business.RequestDto
{
    public class ProductAddRequest
    {
        public string ProductName { get; set; }
        public CategoryOptions Category { get; set; }
        public decimal? Price { get; set; }
        public int? QuantityInStock { get; set; }
    }
}
