using Project.BusinessLogic.EntitiesDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.BusinessLogic.Interfaces {
    interface IRatingService {
        Task<IEnumerable<RatingDTO>> GetAllAsync();
        Task<RatingDTO> GetByIdAsync(int id);
        Task<bool> AddAsync(RatingDTO entity);
        Task<IEnumerable<RatingDTO>> GetByUserIdAsync(string userId);
        Task<int> GetRatingMarkAsync(int imageId);
        Task<RatingDTO> GetByUserAndImageAsync(string userId, int imageId);

    }
}
