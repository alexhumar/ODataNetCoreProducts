
using DataAccess;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataService.Controllers
{
    public class ProductsController : ODataController
    {
        DomainContext _dbcontext;

        public DomainContext DBContext { get => _dbcontext; set => _dbcontext = value; }

        public ProductsController(DomainContext context)
        {
            this.DBContext = context;
        }

        public IActionResult GetNameFromProduct(long key)
        {
            //return Ok(ObjectsBuilder.BuildProducts().FirstOrDefault(p => p.ID == key).Name);

            var product = this.DBContext.Products.FirstOrDefault(p => p.ID == key);

            if (product == null) return NotFound();

            return Ok(product.Name);
        }

        [EnableQuery]
        public SingleResult<Product> GetProduct(long key)
        {
            //return SingleResult.Create(ObjectsBuilder.BuildProducts().Where(p => p.ID == key).AsQueryable());
            return SingleResult.Create(this.DBContext.Products.Where(p => p.ID == key).AsQueryable());
        }

        [EnableQuery]
        public IQueryable<Product> GetProducts()
        {
            //return ObjectsBuilder.BuildProducts().AsQueryable();
            return this.DBContext.Products.AsQueryable();
        }

        [EnableQuery]
        public SingleResult<Category> GetCategoryFromProduct(long key)
        {
            var product = this.DBContext.Products
                                        .Include(p => p.Category) //LAZY LOADING TODAVIA NO ESTA DISPONIBLE EN EF CORE
                                        .ThenInclude(c => c.Products) //PARA QUE ME TRAIGA TODOS SI HAGO UN EXPAND POR PRODUCTOS
                                        .FirstOrDefault(p => p.ID == key);
            var category = (product != null) ? product.Category : null;

            return SingleResult.Create(new List<Category> { category }.AsQueryable());
        }
        
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            this.DBContext.Add(product);
            await this.DBContext.SaveChangesAsync();

            return Created(product);
        }
    }
}