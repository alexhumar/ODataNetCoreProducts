using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using Models;
using System;

namespace DataAccess
{
    public class MyModelBuilder
    {
        public static IEdmModel getEdmModel(IServiceProvider serviceProvider)
        {
            var builder = new ODataConventionModelBuilder(serviceProvider);

            var products = builder.EntitySet<Product>("Products");
            var categories = builder.EntitySet<Category>("Categories");

            products.EntityType
                    .Count()
                    .Filter()
                    .Expand(1, new string[] { "Category" })
                    .Page(5, 2)
                    .OrderBy(new string[] { "ID", "Name" });

            categories.EntityType
                    .Count()
                    .Filter()
                    .Expand(1, new string[] { "Products" })
                    .Page(5, 2)
                    .OrderBy(new string[] { "ID", "Name" });

            return builder.GetEdmModel();
        }
    }
}
