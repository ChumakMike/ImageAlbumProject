using Project.BusinessLogic.EntitiesDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.BusinessLogic.Interfaces {
    public interface IImageService {
        Task<IEnumerable<ImageDTO>> GetAllAsync();
        Task<ImageDTO> GetByIdAsync(int id);
        Task AddAsync(ImageDTO entity);
        Task<IEnumerable<ImageDTO>> GetByUserIdAsync(string userId);
        Task<IEnumerable<ImageDTO>> GetByCategoryIdAsync(int categoryId);
        Task Remove(ImageDTO entity);
        Task Update(ImageDTO entity);
    }
}
