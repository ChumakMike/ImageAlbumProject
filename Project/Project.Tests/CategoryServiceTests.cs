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
using Project.BusinessLogic.Interfaces;
using Moq;
using AutoMapper;
using Project.BusinessLogic.Mapping;
using Project.BusinessLogic.Services;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Exceptions;

namespace Project.Tests {
    public class CategoryServiceTests {

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;
        public CategoryServiceTests() {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapper = configuration.CreateMapper();
            _categoryService = new CategoryService(_unitOfWorkMock.Object, _mapper);
        }
        
        [Fact]
        public async Task CategoryService_GetAll_ReturnsAllElementsWithCorrectValues() {
           
            var categories = GetCategoriesList();
            _unitOfWorkMock.Setup(x => x.CategoryRepository.GetAllAsync()).ReturnsAsync(categories);

            var result = await _categoryService.GetAllAsync();
            
            Assert.Contains(result, x => x.CategoryId == 1);
            Assert.Contains(result, x => x.CategoryId == 2);
            _unitOfWorkMock.Verify(x => x.CategoryRepository.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task CategoryService_GetAll_ReturnsAllElementsWithCorrectType() {

            var categories = GetCategoriesList();
            _unitOfWorkMock.Setup(x => x.CategoryRepository.GetAllAsync()).ReturnsAsync(categories);

            var result = await _categoryService.GetAllAsync();

            Assert.IsAssignableFrom<IEnumerable<CategoryDTO>>(result);
            _unitOfWorkMock.Verify(x => x.CategoryRepository.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task CategoryService_GetAll_ReturnsAllTwoElements() {

            var categories = GetCategoriesList();
            _unitOfWorkMock.Setup(x => x.CategoryRepository.GetAllAsync()).ReturnsAsync(categories);

            var result = (await _categoryService.GetAllAsync()).Count();
            var expectedCount = 2;

            _unitOfWorkMock.Verify(x => x.CategoryRepository.GetAllAsync(), Times.Once());
            Assert.Equal(expectedCount, result);
        }

        [Theory]
        [InlineData(3)]
        public async Task CategoryService_GetById_ReturnsElementByGivenId(int id) {

            int expectedId = 3;
            string expectedName = "NewName";
            var category = new Category { CategoryId = expectedId, Name = expectedName };
            _unitOfWorkMock.Setup(x => x.CategoryRepository.GetByIdAsync(id)).ReturnsAsync(category);

            var result = await _categoryService.GetByIdAsync(id);

            _unitOfWorkMock.Verify(x => x.CategoryRepository.GetByIdAsync(id), Times.Once());
            Assert.Equal(expectedId, result.CategoryId);
            Assert.Equal(expectedName, result.Name);
        }

        [Fact]
        public async Task CategoryService_AddAsync_AddsNewCategory() {

            _unitOfWorkMock.Setup(x => x.CategoryRepository.AddAsync(It.IsAny<Category>())).Verifiable();

            await _categoryService.AddAsync(_mapper.Map<CategoryDTO>(It.IsAny<Category>()));

            _unitOfWorkMock.Verify(x => x.CategoryRepository.AddAsync(It.IsAny<Category>()), Times.Once());
        }

        [Fact]
        public async Task CategoryService_AddAsync_ThrowsSuchEntityExistsException() {

            _unitOfWorkMock.Setup(x => x.CategoryRepository.AddAsync(It.IsAny<Category>())).Throws(new SuchEntityExistsException(""));

            await Assert.ThrowsAsync<SuchEntityExistsException>(
                () => _categoryService.AddAsync(
                    _mapper.Map<CategoryDTO>(It.IsAny<Category>())));
        }

        [Fact]
        public async Task CategoryService_Remove_RemovesOneCategory() {
            
            var category = new Category { CategoryId = 2, Name = "Animals" };
            var categories = GetCategoriesList();
            _unitOfWorkMock.Setup(x => x.CategoryRepository.GetAllAsync()).ReturnsAsync(categories);
            _unitOfWorkMock.Setup(x => x.CategoryRepository.Remove(category)).Verifiable();

            await _categoryService.Remove(_mapper.Map<CategoryDTO>(category));

            _unitOfWorkMock.Verify(x => x.CategoryRepository.Remove(It.IsAny<Category>()), Times.Once());
        }

        [Fact]
        public async Task CategoryService_Remove_ThrowsNoSuchEntityException() {

            var category = new Category { CategoryId = 3, Name = "SomeCategory" };
            var categories = GetCategoriesList();
            _unitOfWorkMock.Setup(x => x.CategoryRepository.GetAllAsync()).ReturnsAsync(categories);
            _unitOfWorkMock.Setup(x => x.CategoryRepository.Remove(category)).Throws(new NoSuchEntityException(""));

            await Assert.ThrowsAsync<NullReferenceException>(
                () => _categoryService.Remove(
                    _mapper.Map<CategoryDTO>(category)));
        }

        [Fact]
        public async Task CategoryService_Update_ThrowsNoSuchEntityException() {

            var category = new Category { CategoryId = 3, Name = "SomeCategory" };
            var categories = GetCategoriesList();
            _unitOfWorkMock.Setup(x => x.CategoryRepository.GetAllAsync()).ReturnsAsync(categories);
            _unitOfWorkMock.Setup(x => x.CategoryRepository.Update(category)).Throws(new NoSuchEntityException(""));

            await Assert.ThrowsAsync<NoSuchEntityException>(
                () => _categoryService.Update(
                    _mapper.Map<CategoryDTO>(category)));
        }

        private IEnumerable<Category> GetCategoriesList() {
            return new List<Category> { 
                new Category {CategoryId = 1, Name = "Nature"},
                new Category {CategoryId = 2, Name = "Animals"}
            };
        }
    }
}
