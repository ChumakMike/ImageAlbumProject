using AutoMapper;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Interfaces;
using Project.Data.Entities;
using Project.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BusinessLogic.Services {
    public class RatingService : IRatingService {

        private IUnitOfWork unitOfWork;
        private IMapper mapper;

        public RatingService(IUnitOfWork uow, IMapper map) {
            unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
            mapper = map ?? throw new ArgumentNullException(nameof(map));
        }
        
        public async Task<bool> AddAsync(RatingDTO entity) {
            var existingRating = await GetByUserAndImageAsync(entity.UserId, entity.ImageId);
            if(existingRating != null) 
                return false;
            else {
                var rating = mapper.Map<Rating>(entity);
                await unitOfWork.RatingRepository.AddAsync(rating);
                await unitOfWork.SaveAsync();
                return true;
            }
        }

        public async Task<IEnumerable<RatingDTO>> GetAllAsync() {
            return mapper.Map<IEnumerable<RatingDTO>>(await unitOfWork.RatingRepository.GetAllAsync());
        }

        public async Task<RatingDTO> GetByIdAsync(int id) {
            return mapper.Map<RatingDTO>(await unitOfWork.RatingRepository.GetByIdAsync(id));
        }

        public async Task<RatingDTO> GetByUserAndImageAsync(string userId, int imageId) {
            var ratingsList = await unitOfWork.RatingRepository.GetAllAsync();
            return mapper.Map<RatingDTO>
                (ratingsList.Where(x => x.UserId == userId && x.ImageId == imageId).FirstOrDefault());
        }

        public async Task<IEnumerable<RatingDTO>> GetByUserIdAsync(string userId) {
            var ratingsList = await unitOfWork.RatingRepository.GetAllAsync();
            return mapper.Map<IEnumerable<RatingDTO>>
                (ratingsList.Where(x => x.UserId == userId));
        }

        public async Task<int> GetRatingMarkAsync(int imageId) {
            var ratingsList = await unitOfWork.RatingRepository.GetAllAsync();
            return ratingsList.Count() <= 0 
                ? 0 
                : mapper.Map<IEnumerable<RatingDTO>>(ratingsList.Where(x => x.ImageId == imageId)).Count();
        }
    }
}
