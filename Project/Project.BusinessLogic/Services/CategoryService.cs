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
using System.Linq;

namespace Project.BusinessLogic.Services {
    public class CategoryService : ICategoryService {

        private IUnitOfWork unitOfWork;
        private IMapper mapper;

        public CategoryService(IUnitOfWork uow, IMapper map) {
            unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
            mapper = map ?? throw new ArgumentNullException(nameof(map));
        }

        public async Task AddAsync(CategoryDTO entity) {
            var existingCategory = await GetByNameAsync(entity);
            if (existingCategory == null) {
                var category = mapper.Map<Category>(entity);
                await unitOfWork.CategoryRepository.AddAsync(category);
                await unitOfWork.SaveAsync();
            }
            else throw new SuchEntityExistsException(entity.GetType().Name);
            
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync() {
            return mapper.Map<IEnumerable<CategoryDTO>>(await unitOfWork.CategoryRepository.GetAllAsync());
        }

        public async Task<CategoryDTO> GetByIdAsync(int id) {
            return mapper.Map<CategoryDTO>(await unitOfWork.CategoryRepository.GetByIdAsync(id));
        }

        public async Task Remove(CategoryDTO entity) {
            var existingCategory = await GetByNameAsync(entity);
            if (existingCategory == null)
                throw new NoSuchEntityException(typeof(CategoryDTO).Name);
            unitOfWork.CategoryRepository.Remove(mapper.Map<Category>(existingCategory));
            await unitOfWork.SaveAsync();
        }

        public async Task Update(CategoryDTO entity) {
            var existingCategory = await GetByIdAsync(entity.CategoryId);
            if (existingCategory == null)
                throw new NoSuchEntityException(typeof(CategoryDTO).Name);
            unitOfWork.CategoryRepository.Update(mapper.Map<Category>(entity));
            await unitOfWork.SaveAsync();
        }

        public async Task<CategoryDTO> GetByNameAsync(CategoryDTO entity) {
            var categoriesList = mapper.Map < IEnumerable < CategoryDTO >>(await unitOfWork.CategoryRepository.GetAllAsync());
            return categoriesList.FirstOrDefault(x => x.Name == entity.Name);
        }
    }
}
