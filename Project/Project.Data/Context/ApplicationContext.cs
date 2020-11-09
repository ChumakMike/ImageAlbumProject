using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.Context {
    public class ApplicationContext : IdentityDbContext<ApplicationUser> {
        public ApplicationContext(DbContextOptions optionsBuilder) : base(optionsBuilder) {
        }
        public DbSet<Image> Images { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
