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
    public class RatingRepositoryTests : ApplicationContextTestBase {
        [Fact]
        public async Task RatingRepo_GetAll_ReturnsAllElementsWithCorrectType() {
            var repository = new RatingRepository(_context);
            var result = await repository.GetAllAsync();
            Assert.IsAssignableFrom<IEnumerable<Rating>>(result);
        }

        [Fact]
        public async Task RatingRepo_GetAll_ReturnsAllFiveSeededElements() {
            var repository = new RatingRepository(_context);
            var result = await repository.GetAllAsync();
            int expectedCount = 3;
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public async Task RatingRepo_Add_AddsOneRating() {
            var repository = new RatingRepository(_context);

            var expected = new Rating {
                RatingId = 4,
                UserId = "user",
                ImageId = 1
            };
            await repository.AddAsync(expected);
            _context.SaveChanges();

            var result = (await repository.GetAllAsync()).Last();
            Assert.Equal(expected.RatingId, result.RatingId);
        }

        [Theory]
        [InlineData(1)]
        public async Task RatingRepo_GetById_ReturnsElementByGivenId(int id) {
            var repository = new RatingRepository(_context);
            var result = await repository.GetByIdAsync(id);
            var expected = new Rating {
                RatingId = 1,
                ImageId = 1,
                UserId = "user"
            };
            Assert.Equal(expected.ImageId, result.ImageId);
            Assert.Equal(expected.RatingId, result.RatingId);
            Assert.Equal(expected.UserId, result.UserId);
        }

        [Fact]
        public async Task RatingRepo_Remove_RemovesOneRatingWhichIsTheLastOne() {
            
            var repository = new RatingRepository(_context);

            var ratings = await repository.GetAllAsync();
            var ratingToRemove = ratings.Last();
            repository.Remove(ratingToRemove);
            _context.SaveChanges();

            int expected = ratings.Count() - 1;
            var result = await repository.GetAllAsync();
            Assert.Equal(expected, result.Count());
            Assert.NotEqual(ratingToRemove.RatingId, result.Last().RatingId);
            Assert.Null(await repository.GetByIdAsync(ratingToRemove.RatingId));
        }

        [Fact]
        public async Task RatingRepo_Update_UpdatesOneRatingWhichIsTheFirstOne() {

            var repository = new RatingRepository(_context);
            var ratings = await repository.GetAllAsync();
            var ratingToUpdate = ratings.First();
            ratingToUpdate.UserId = "user123";
            repository.Update(ratingToUpdate);
            _context.SaveChanges();

            var expectedUserId = "user123";
            var result = await repository.GetByIdAsync(ratingToUpdate.RatingId);
            Assert.Equal(expectedUserId, result.UserId);
        }
    }
}
