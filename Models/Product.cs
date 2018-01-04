namespace Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }

        public void SetCategory(Category category)
        {
            this.Category = category;
            category.AddProduct(this);
        }
    }
}