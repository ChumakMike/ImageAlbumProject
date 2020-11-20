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
    public class CategoryRepositoryTests : ApplicationContextTestBase {

        [Fact]
        public async Task CategoryRepo_GetAll_ReturnsAllElementsWithCorrectType() {
            var repository = new CategoryRepository(_context);
            var result = await repository.GetAllAsync();
            Assert.IsAssignableFrom<IEnumerable<Category>>(result);
        }

        [Fact]
        public async Task CategoryRepo_GetAll_ReturnsAllFiveSeededElements() {
            var repository = new CategoryRepository(_context);
            var result = await repository.GetAllAsync();
            int expectedCount = 5;
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public async Task CategoryRepo_GetAll_ReturnsAllElementsAndTheFirstOnesAreEqual() {
            var repository = new CategoryRepository(_context);
            var result = await repository.GetAllAsync();
            var expected = new Category { CategoryId = 1, Name = "Nature"};
            Assert.Equal(expected.CategoryId, result.First().CategoryId);
            Assert.Equal(expected.Name, result.First().Name);
        }

        [Fact]
        public async Task CategoryRepo_Add_AddsOneCategory() {
            var repository = new CategoryRepository(_context);
            
            var expected = new Category { CategoryId = 6, Name = "NewCategory" };
            await repository.AddAsync(expected);
            _context.SaveChanges();

            var result = (await repository.GetAllAsync()).Last();
            Assert.Equal(expected.CategoryId, result.CategoryId);
            Assert.Equal(expected.Name, result.Name);
        }

        [Theory]
        [InlineData(3)]
        public async Task CategoryRepo_GetById_ReturnsElementByGivenId(int id) {
            var repository = new CategoryRepository(_context);
            var result = await repository.GetByIdAsync(id);
            var expected = new Category { CategoryId = 3, Name = "Cars" };
            Assert.Equal(expected.CategoryId, result.CategoryId);
            Assert.Equal(expected.Name, result.Name);
        }

        [Fact]
        public async Task CategoryRepo_Remove_RemovesOneCategoryWhichIsTheLastOne() {
            var repository = new CategoryRepository(_context);

            var categories = await repository.GetAllAsync();
            var categoryToRemove = categories.Last();
            repository.Remove(categoryToRemove);
            _context.SaveChanges();

            int expected = categories.Count() - 1;
            var result = await repository.GetAllAsync();
            Assert.Equal(expected, result.Count());
            Assert.NotEqual(categoryToRemove.CategoryId, result.Last().CategoryId);
            Assert.Null(await repository.GetByIdAsync(categoryToRemove.CategoryId));
        }

        [Fact]
        public async Task CategoryRepo_Update_UpdatesOneCategoryWhichIsTheFirstOne() {
            var repository = new CategoryRepository(_context);

            var categories = await repository.GetAllAsync();
            var categoryToUpdate = categories.First();
            categoryToUpdate.Name = "NewName";
            repository.Update(categoryToUpdate);
            _context.SaveChanges();

            var expectedName = "NewName";
            var result = await repository.GetByIdAsync(categoryToUpdate.CategoryId);
            Assert.Equal(expectedName, result.Name);
        }


    }
}
