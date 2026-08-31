using Products.Business.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Business.ResponseDto
{
    public class ProductResponse
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public CategoryOptions Category { get; set; }
        public decimal? Price { get; set; }
        public int? QuantityInStock { get; set; }
    }
}
