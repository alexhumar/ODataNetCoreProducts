
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
    public class CategoriesController : ODataController
    {
        DomainContext _dbcontext;

        public DomainContext DBContext { get => _dbcontext; set => _dbcontext = value; }

        public CategoriesController(DomainContext context)
        {
            this.DBContext = context;
        }

        private bool ExisteEntidad(long key)
        {
            return this.DBContext.Products.Any(p => p.ID == key);
        }

        public IActionResult GetNameFromCategory(long key)
        {
            //return Ok(ObjectsBuilder.BuildCategories().FirstOrDefault(c => c.ID == key).Name);
            var category = this.DBContext.Categories.FirstOrDefault(c => c.ID == key);

            if (category == null) return NotFound();

            return Ok(category.Name);
        }

        [EnableQuery]
        public SingleResult<Category> GetCategory(long key)
        {
            //return SingleResult.Create(ObjectsBuilder.BuildCategories().Where(c => c.ID == key).AsQueryable());
            return SingleResult.Create(this.DBContext.Categories.Where(c => c.ID == key).AsQueryable());
        }

        [EnableQuery]
        public IQueryable<Category> GetCategories()
        {
            //return ObjectsBuilder.BuildCategories().AsQueryable();
            return this.DBContext.Categories.AsQueryable();
        }

        [EnableQuery]
        public IQueryable<Product> GetProductsFromCategory(long key)
        {
            //return ObjectsBuilder.BuildCategories().FirstOrDefault(c => c.ID == key).Products.AsQueryable();
            var category = this.DBContext.Categories
                                         .Include(c => c.Products) //LAZY LOADING TODAVIA NO ESTA DISPONIBLE EN EF CORE
                                         .FirstOrDefault(c => c.ID == key);
            var products = (category != null) ? category.Products : new List<Product>();

            return products.AsQueryable();
        }

        public async Task<IActionResult> Post([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            this.DBContext.Add(category);
            await this.DBContext.SaveChangesAsync();

            return Created(category);
        }

        public async Task<IActionResult> Put([FromODataUri] long key, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (key != category.ID)
                return BadRequest();

            this.DBContext.Entry(category).State = EntityState.Modified;

            try
            {
                await this.DBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteEntidad(key))
                    return NotFound();
                else
                    throw;
            }

            return Updated(category);
        }

        public async Task<IActionResult> Patch([FromODataUri] long key, [FromBody] Delta<Category> category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var entity = await this.DBContext.Categories.Include(c => c.Products)
                                                        .FirstOrDefaultAsync(c => c.ID == key);
            if (entity == null)
                return NotFound();

            category.Patch(entity);

            try
            {
                await this.DBContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExisteEntidad(key))
                    return NotFound();
                else
                    throw;
            }

            return Updated(entity);
        }

        public async Task<IActionResult> Delete(long key)
        {
            var category = await this.DBContext.Categories.FindAsync(key);
            if (category == null)
                return NotFound();

            this.DBContext.Categories.Remove(category);
            await this.DBContext.SaveChangesAsync();

            return NoContent();
        }
    }
}