using Products.Business.Common;

namespace Products.Business.RequestDto
{
    public class ProductUpdateRequest
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public CategoryOptions Category { get; set; }
        public decimal? Price { get; set; }
        public int? QuantityInStock { get; set; }
    }
}
