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
using Project.WebApi.Middleware;
using Project.WebApi.Helpers;
using Project.BusinessLogic.Interfaces;
using Project.BusinessLogic.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Project.BusinessLogic.Identity;

namespace Project.WebApi {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
         
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.Configure<ApplicationSettingsHelper>(Configuration.GetSection("ApplicationSettings"));
            services.AddControllers();

            AddDatabaseConfiguration(services);
            AddDependencyInjection(services);
            AddJWTAuthentication(services);

            services.AddAutoMapper(
                typeof(BusinessLogic.Mapping.MappingProfile),
                typeof(WebApi.Mapping.MappingProfile)
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
                builder.WithOrigins(Configuration["ApplicationSettings:Client_Url"].ToString())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );

            //app.UseMiddleware<ProtectionMiddleware>();

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
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>();

            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;
            });
        }

        private void AddDependencyInjection(IServiceCollection services) {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IRatingRepository, RatingRepository>();
            services.AddTransient<IImageRepository, ImageRepository>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IRatingService, RatingService>();
            services.AddTransient<IImageService, ImageService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
        }

        private void AddJWTAuthentication(IServiceCollection services) {
            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x => {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidIssuer = Configuration["ApplicationSettings:Issuer_Url"].ToString(),
                        ValidateIssuer = true,

                        ValidAudience = Configuration["ApplicationSettings:Client_Url"].ToString(),
                        ValidateAudience = true
                    };
                });
        }

    }
}
