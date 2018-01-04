using System.Collections.Generic;
using Models;

namespace ODataService.Helpers
{
    public static class ObjectsBuilder
    {
        private static Category categoryFood = new Category()
        {
            ID = 1,
            Name = "Food"
        };

        private static Category categoryGym = new Category()
        {
            ID = 2,
            Name = "Gym"
        }; 

        private static Product productSteroids = new Product()
        {
            ID = 3,
            Name = "Steroids"
        };

        private static Product productBread = new Product()
        {
            ID = 1,
            Name = "Bread",
        };

        private static Product productChocolate = new Product()
        {
            ID = 2,
            Name = "Chocolate",
        };

        static ObjectsBuilder()
        {
            productBread.SetCategory(categoryFood);
            productChocolate.SetCategory(categoryFood);
            productSteroids.SetCategory(categoryGym);
        }

        public static List<Product> BuildProducts()
        {
            return new List<Product> { productBread, productChocolate, productSteroids };   
        }

        public static List<Category> BuildCategories()
        {
            return new List<Category> { categoryFood, categoryGym };
        }
    }
}