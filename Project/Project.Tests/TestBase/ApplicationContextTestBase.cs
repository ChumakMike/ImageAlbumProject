using Microsoft.EntityFrameworkCore;
using Project.Data.Context;
using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Tests.TestBase {
    public class ApplicationContextTestBase : IDisposable {
        
        protected readonly ApplicationContext _context;
        public ApplicationContextTestBase() {
            
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationContext(options);
            _context.Database.EnsureCreated();
            
            SeedCategories();
            SeedImages();
            SeedRatings();
        }

        private void SeedCategories() {
            var categories = new[] {
                new Category {CategoryId = 1, Name = "Nature"},
                new Category {CategoryId = 2, Name = "Animals"},
                new Category {CategoryId = 3, Name = "Cars"},
                new Category {CategoryId = 4, Name = "Architecture"},
                new Category {CategoryId = 5, Name = "Fashion"}
            };

            _context.Category.AddRange(categories);
            _context.SaveChanges();
        }

        private void SeedImages() {
            var images = new[] {
                new Image { ImageId = 1, Caption = "Caption", Name = "Image1", Link = "/image1", CategoryId = 1, UserRefId = "user"},
                new Image { ImageId = 2, Caption = "Caption2", Name = "Image2", Link = "/image2", CategoryId = 3, UserRefId = "user2"},
                new Image { ImageId = 3, Caption = "Caption3", Name = "Image3", Link = "/image3", CategoryId = 1, UserRefId = "user"},
                new Image { ImageId = 4, Caption = "Caption4", Name = "Image4", Link = "/image4", CategoryId = 2, UserRefId = "user4"},
                new Image { ImageId = 5, Caption = "Caption5", Name = "Image5", Link = "/image5", CategoryId = 4, UserRefId = "user5"}
            };
            _context.Images.AddRange(images);
            _context.SaveChanges();
        }

        private void SeedRatings() {
            var ratings = new[] {
                new Rating { RatingId = 1, ImageId = 1, UserId = "user" },
                new Rating { RatingId = 2, ImageId = 2, UserId = "user" },
                new Rating { RatingId = 3, ImageId = 1, UserId = "user1" }
            };
            _context.Ratings.AddRange(ratings);
            _context.SaveChanges();
        }
        public void Dispose() {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
