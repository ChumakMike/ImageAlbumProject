using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Project.Data.Context;
using Project.Data.Repositories;
using Project.Data.Unit_Of_Work;
using Project.Data.Interfaces;
using AutoMapper;

namespace Project.WebApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
         
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            AddDatabaseConfiguration(services);
            AddDependencyInjection(services);

            services.AddAutoMapper(
                typeof(BusinessLogic.Mapping.MappingProfile)
            );
            services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder =>
                builder.WithOrigins(Configuration.GetConnectionString("DefaultConnection"))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

        private void AddDatabaseConfiguration(IServiceCollection services) { 
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => {
                options.UseSqlServer(connectionString);
            });

            services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;
            });
        }

        private void AddDependencyInjection(IServiceCollection services) {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IRatingRepository, RatingRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
        }

    }
}
