using System;
using System.Linq;
using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccess
{
    public class DomainContext : DbContext
    {
        public DomainContext(DbContextOptions<DomainContext> options) :base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>().ToTable("productos"); //NO HACE FALTA, ES SOLO PARA PONERLE UN NOMBRE CUSTOM A LA TABLA
            builder.Entity<Category>().ToTable("categorias"); //NO HACE FALTA, ES SOLO PARA PONERLE UN NOMBRE CUSTOM A LA TABLA

            base.OnModelCreating(builder);
        }
    }
}
