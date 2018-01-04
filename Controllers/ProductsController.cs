
using Microsoft.AspNet.OData;
using Models;
using System.Collections.Generic;
using System.Linq;
using ODataService.Helpers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace ODataService.Controllers
{
    public class ProductsController : ODataController
    {
        [EnableQuery]
        public SingleResult<Product> GetProduct(int key)
        {
            return SingleResult.Create(ObjectsBuilder.BuildProducts().Where(p => p.ID == key).AsQueryable());
        }

        [EnableQuery]
        public IQueryable<Product> GetProducts()
        {
            return ObjectsBuilder.BuildProducts().AsQueryable();
        }

        [EnableQuery]
        public Category GetCategoryFromProduct(int key)
        {
            return ObjectsBuilder.BuildProducts().FirstOrDefault(p => p.ID == key).Category;
        }

        [EnableQuery]
        public IQueryable<Product> GetProductsFromCategory(int key)
        {
            return ObjectsBuilder.BuildCategories().FirstOrDefault(c => c.ID == key).Products.AsQueryable();
        }
    }
}