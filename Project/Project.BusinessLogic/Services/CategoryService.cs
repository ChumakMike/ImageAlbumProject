using AutoMapper;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Interfaces;
using Project.Data.Entities;
using Project.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.BusinessLogic.Services {
    public class CategoryService : ICategoryService {

        private IUnitOfWork unitOfWork;
        private IMapper mapper;

        public CategoryService(IUnitOfWork unit, IMapper map) {
            unitOfWork = unit ?? throw new ArgumentNullException(nameof(unit));
            mapper = map ?? throw new ArgumentNullException(nameof(map));
        }

        public async Task AddAsync(CategoryDTO entity) {
            var category = mapper.Map<Category>(entity);
            await unitOfWork.CategoryRepository.AddAsync(category);
            await unitOfWork.SaveAsync();
        }

        public Task<IEnumerable<CategoryDTO>> GetAllAsync() {
            throw new NotImplementedException();
        }

        public Task<CategoryDTO> GetByIdAsync(int id) {
            throw new NotImplementedException();
        }

        public Task Remove(CategoryDTO entity) {
            throw new NotImplementedException();
        }

        public Task Update(CategoryDTO entity) {
            throw new NotImplementedException();
        }
    }
}
