using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using backend_cshar.Entities;
using Microsoft.EntityFrameworkCore;

using backend_cshar.Repositories;

// ADD SWAGGER
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.OpenApi.Models;


namespace backend_cshar
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
            var connectionString = "Data Source=test.db";
            services.AddDbContext<DataBaseDbContext>(options => options.UseSqlite(connectionString));
        
            services.AddControllers();

            // Add Customer Repository
            services.AddScoped<IUserRepository, UserRepository>();

            // Register the Swagger generator and define a Swagger document
            // for Northwind service
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(name: "v1", info: new OpenApiInfo
                            { Title = "CRUD C# Service API", Version = "v1" });
            });

            // Get the database context and apply the migrations
            var context = services.BuildServiceProvider().GetService<DataBaseDbContext>();
            context.Database.Migrate();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json",
                "CRUD C# Service API Version 1");
                options.SupportedSubmitMethods(new[] {
                    SubmitMethod.Get, SubmitMethod.Post,
                    SubmitMethod.Put, SubmitMethod.Delete });
            });

        }
    }
}
