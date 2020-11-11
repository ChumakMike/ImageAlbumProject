using AutoMapper;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Exceptions;
using Project.BusinessLogic.Interfaces;
using Project.Data.Entities;
using Project.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BusinessLogic.Services {
    public class ImageService : IImageService {

        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        public ImageService(IUnitOfWork uow, IMapper map) {
            unitOfWork = uow ?? throw new ArgumentNullException(nameof(uow));
            mapper = map ?? throw new ArgumentNullException(nameof(map));
        }
        public async Task AddAsync(ImageDTO entity) {
            var image = mapper.Map<Image>(entity);
            await unitOfWork.ImageRepository.AddAsync(image);
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ImageDTO>> GetAllAsync() {
            return mapper.Map<IEnumerable<ImageDTO>>(await unitOfWork.ImageRepository.GetAllAsync());
        }

        public async Task<IEnumerable<ImageDTO>> GetByCategoryIdAsync(int categoryId) {
            var imagesList = await unitOfWork.ImageRepository.GetAllAsync();
            return mapper.Map<IEnumerable<ImageDTO>>
                (imagesList.Where(x => x.CategoryId == categoryId));
        }

        public async Task<ImageDTO> GetByIdAsync(int id) {
            return mapper.Map<ImageDTO>(await unitOfWork.ImageRepository.GetByIdAsync(id));
        }

        public async Task<IEnumerable<ImageDTO>> GetByUserIdAsync(string userId) {
            var imagesList = await unitOfWork.ImageRepository.GetAllAsync();
            return mapper.Map<IEnumerable<ImageDTO>>
                (imagesList.Where(x => x.UserRefId == userId));
        }

        public async Task Remove(ImageDTO entity) {
            var existingImage = await GetByIdAsync(entity.ImageId);
            if (existingImage == null)
                throw new NoSuchEntityException(existingImage.GetType().Name);
            unitOfWork.ImageRepository.Remove(mapper.Map<Image>(entity));
            await unitOfWork.SaveAsync();
        }

        public async Task Update(ImageDTO entity) {
            var existingImage = await GetByIdAsync(entity.ImageId);
            if (existingImage == null)
                throw new NoSuchEntityException(existingImage.GetType().Name);
            unitOfWork.ImageRepository.Update(mapper.Map<Image>(entity));
            await unitOfWork.SaveAsync();
        }
    }
}
