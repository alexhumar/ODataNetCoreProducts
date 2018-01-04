
using Microsoft.AspNet.OData;
using Models;
using System.Collections.Generic;
using System.Linq;
using ODataService.Helpers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace ODataService.Controllers
{
    public class CategoriesController : ODataController
    {
        [EnableQuery]
        public SingleResult<Category> GetCategory(int key)
        {
            return SingleResult.Create(ObjectsBuilder.BuildCategories().Where(c => c.ID == key).AsQueryable());
        }

        [EnableQuery]
        public IQueryable<Category> GetCategories()
        {
            return ObjectsBuilder.BuildCategories().AsQueryable();
        }

        [EnableQuery]
        public IQueryable<Product> GetProductsFromCategory(int key)
        {
            return ObjectsBuilder.BuildCategories().FirstOrDefault(c => c.ID == key).Products.AsQueryable();
        }
    }
}