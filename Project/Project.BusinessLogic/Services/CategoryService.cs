using AutoMapper;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Exceptions;
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

        public CategoryService(IUnitOfWork uow, IMapper map) {
            unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
            mapper = map ?? throw new ArgumentNullException(nameof(map));
        }

        public async Task AddAsync(CategoryDTO entity) {
            var category = mapper.Map<Category>(entity);
            await unitOfWork.CategoryRepository.AddAsync(category);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync() {
            return mapper.Map<IEnumerable<CategoryDTO>>(await unitOfWork.CategoryRepository.GetAllAsync());
        }

        public async Task<CategoryDTO> GetByIdAsync(int id) {
            return mapper.Map<CategoryDTO>(await unitOfWork.CategoryRepository.GetByIdAsync(id));
        }

        public async Task Remove(CategoryDTO entity) {
            var existingCategory = await GetByIdAsync(entity.CategoryId);
            if (existingCategory == null)
                throw new NoSuchEntityException(existingCategory.GetType().Name);
            unitOfWork.CategoryRepository.Remove(mapper.Map<Category>(entity));
            await unitOfWork.SaveAsync();
        }

        public async Task Update(CategoryDTO entity) {
            var existingCategory = await GetByIdAsync(entity.CategoryId);
            if (existingCategory == null)
                throw new NoSuchEntityException(existingCategory.GetType().Name);
            unitOfWork.CategoryRepository.Update(mapper.Map<Category>(entity));
            await unitOfWork.SaveAsync();
        }
    }
}
