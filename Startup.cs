using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using Microsoft.EntityFrameworkCore;
using Models;
using DataAccess;

namespace WebApiNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DomainContext>(opt => opt.UseMySql(config));
            services.AddMvc();
            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var builder = new ODataConventionModelBuilder(app.ApplicationServices);

            var products = builder.EntitySet<Product>("Products");
            var categories = builder.EntitySet<Category>("Categories");

            products.EntityType
                    .Count()
                    .Filter()
                    .Expand(1, new string[]{ "Category" })
                    .Page(5, 2)
                    .OrderBy(new string[] { "ID", "Name" });

            categories.EntityType
                    .Count()
                    .Filter()
                    .Expand(1, new string[] { "Products" })
                    .Page(5, 2)
                    .OrderBy(new string[] { "ID", "Name" });

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());

                //Work-around for issue #1175
                routeBuilder.EnableDependencyInjection();
            });
        }
    }
}
