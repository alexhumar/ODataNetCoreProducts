using System.Collections.Generic;

namespace Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public IList<Product> Products { get; set; }

        public void AddProduct(Product product)
        {
            this.Products.Add(product);
        }
    }
}