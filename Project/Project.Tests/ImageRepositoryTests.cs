using System;
using Xunit;
using Project.Data.Context;
using Project.Data.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Tests.TestBase;
using Project.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Project.Tests {
    public class ImageRepositoryTests : ApplicationContextTestBase {
        [Fact]
        public async Task ImageRepo_GetAll_ReturnsAllElementsWithCorrectType() {
            var repository = new ImageRepository(_context);
            var result = await repository.GetAllAsync();
            Assert.IsAssignableFrom<IEnumerable<Image>>(result);
        }

        [Fact]
        public async Task ImageRepo_GetAll_ReturnsAllFiveSeededElements() {
            var repository = new ImageRepository(_context);
            var result = await repository.GetAllAsync();
            int expectedCount = 5;
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public async Task ImageRepo_Add_AddsOneImage() {
            var repository = new ImageRepository(_context);

            var expected = new Image {
                ImageId = 6,
                Caption = "Caption",
                Name = "Image6",
                Link = "/image6",
                CategoryId = 1,
                UserRefId = "user"
            };
            await repository.AddAsync(expected);
            _context.SaveChanges();

            var result = (await repository.GetAllAsync()).Last();
            Assert.Equal(expected.ImageId, result.ImageId);
            Assert.Equal(expected.Name, result.Name);
        }

        [Theory]
        [InlineData(5)]
        public async Task ImageRepo_GetById_ReturnsElementByGivenId(int id) {
            var repository = new ImageRepository(_context);
            var result = await repository.GetByIdAsync(id);
            var expected = new Image { 
                ImageId = 5, 
                Caption = "Caption5", 
                Name = "Image5", 
                Link = "/image5", 
                CategoryId = 4, 
                UserRefId = "user5" 
            };
            Assert.Equal(expected.ImageId, result.ImageId);
            Assert.Equal(expected.Name, result.Name);
            Assert.Equal(expected.UserRefId, result.UserRefId);
        }

        [Fact]
        public async Task ImageRepo_Remove_RemovesOneImageWhichIsTheLastOne() {
            var repository = new ImageRepository(_context);

            var images = await repository.GetAllAsync();
            var imageToRemove = images.Last();
            repository.Remove(imageToRemove);
            _context.SaveChanges();

            int expected = images.Count() - 1;
            var result = await repository.GetAllAsync();
            Assert.Equal(expected, result.Count());
            Assert.NotEqual(imageToRemove.CategoryId, result.Last().CategoryId);
            Assert.Null(await repository.GetByIdAsync(imageToRemove.CategoryId));
        }

        [Fact]
        public async Task ImageRepo_Update_UpdatesOneImageWhichIsTheFirstOne() {
            
            var repository = new ImageRepository(_context);
            var images = await repository.GetAllAsync();
            var imageToUpdate = images.First();
            imageToUpdate.Caption = "This is new caption!";
            repository.Update(imageToUpdate);
            _context.SaveChanges();

            var expectedCaption = "This is new caption!";
            var result = await repository.GetByIdAsync(imageToUpdate.ImageId);
            Assert.Equal(expectedCaption, result.Caption);
        }
    }
}
