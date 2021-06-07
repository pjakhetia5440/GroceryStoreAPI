using GroceryStoreAPI.Data;
using GroceryStoreAPI.Data.DataProviders;
using GroceryStoreAPI.Entities;
using GroceryStoreAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI
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
            services.AddDbContext<GroceryStoreAPIDBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("CustomerDBConnection")));

            services.AddSingleton<Serilog.ILogger>(Log.Logger);
            services.AddSingleton(Configuration);
            services.AddScoped<IGroceryStoreAPIServices, GroceryStoreAPIServices>();
            services.AddScoped<IDBServices, DBServices>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GroceryStoreAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GroceryStoreAPI v1"));
            }
            loggerFactory.AddSerilog();
            InitializeDatabase(app);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<GroceryStoreAPIDBContext>())
                {
                    context.Database.Migrate();
                    if (!context.Customer.Any())
                    {
                        context.HasData<Customer>(
                         new Customer { Name = "Bob" },
                         new Customer { Name = "Mary" },
                         new Customer { Name = "Joe" });
                    }
                }
            }
        }
    }
}
