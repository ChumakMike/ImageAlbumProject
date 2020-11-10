using Project.BusinessLogic.EntitiesDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.BusinessLogic.Interfaces {
    public interface ICategoryService {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO> GetByIdAsync(int id);
        Task AddAsync(CategoryDTO entity);
        Task Remove(CategoryDTO entity);
        Task Update(CategoryDTO entity);
    }
}
