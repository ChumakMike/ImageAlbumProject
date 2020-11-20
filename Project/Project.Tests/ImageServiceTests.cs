using System.Collections.Generic;
using Moq;
using AutoMapper;
using Project.BusinessLogic.Mapping;
using Project.BusinessLogic.Services;
using Project.BusinessLogic.EntitiesDTO;
using Project.Data.Repositories;
using Project.Data.Entities;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using Project.BusinessLogic.Exceptions;

namespace Project.Tests {
    public class ImageServiceTests {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;
        public ImageServiceTests() {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapper = configuration.CreateMapper();
            _imageService = new ImageService(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task ImageService_GetAll_ReturnsAllElementsWithCorrectValues() {

            var images = GetImagesList();
            _unitOfWorkMock.Setup(x => x.ImageRepository.GetAllAsync()).ReturnsAsync(images);

            var result = await _imageService.GetAllAsync();

            Assert.Contains(result, x => x.ImageId == 1 && x.Name == "Image1");
            Assert.Contains(result, x => x.ImageId == 2 && x.Name == "Image2");
            Assert.Contains(result, x => x.ImageId == 3 && x.Name == "Image3");
            _unitOfWorkMock.Verify(x => x.ImageRepository.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task ImageService_GetAll_ReturnsAllElementsWithCorrectType() {

            var images = GetImagesList();
            _unitOfWorkMock.Setup(x => x.ImageRepository.GetAllAsync()).ReturnsAsync(images);

            var result = await _imageService.GetAllAsync();

            Assert.IsAssignableFrom<IEnumerable<ImageDTO>>(result);
            _unitOfWorkMock.Verify(x => x.ImageRepository.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task ImageService_GetAll_ReturnsThreeElements() {

            var images = GetImagesList();
            _unitOfWorkMock.Setup(x => x.ImageRepository.GetAllAsync()).ReturnsAsync(images);

            var result = await _imageService.GetAllAsync();

            int expectedCount = 3;
            Assert.Equal(expectedCount, result.Count());
        }


        [Theory]
        [InlineData(3)]
        public async Task ImageService_GetById_ReturnsElementByGivenId(int id) {

            int expectedId = id;
            string expectedName = "ImageExpectedName";
            var image = new Image { 
                ImageId = expectedId, 
                Caption = "Caption", 
                Name = expectedName, 
                Link = "/image1", 
                CategoryId = 1, 
                UserRefId = "user" 
            };
            _unitOfWorkMock.Setup(x => x.ImageRepository.GetByIdAsync(id)).ReturnsAsync(image);

            var result = await _imageService.GetByIdAsync(id);

            _unitOfWorkMock.Verify(x => x.ImageRepository.GetByIdAsync(id), Times.Once());
            Assert.Equal(expectedId, result.ImageId);
            Assert.Equal(expectedName, result.Name);
        }

        [Fact]
        public async Task ImageService_GetImageByUserId_GetsRightImage() {
            var images = GetImagesList();
            _unitOfWorkMock.Setup(x => x.ImageRepository.GetAllAsync()).ReturnsAsync(images);

            var result = await _imageService.GetByUserIdAsync("user");

            Assert.Contains(result, x =>
                x.ImageId == 1
                && x.Name == "Image1"
                && x.Link == "/image1");
            Assert.Contains(result, x =>
                x.ImageId == 3
                && x.Name == "Image3"
                && x.Link == "/image3");
        }

        [Fact]
        public async Task ImageService_GetImageByCategoryId_GetsRightImage() {
            var images = GetImagesList();
            _unitOfWorkMock.Setup(x => x.ImageRepository.GetAllAsync()).ReturnsAsync(images);

            var result = await _imageService.GetByCategoryIdAsync(3);

            Assert.Contains(result, x =>
                x.ImageId == 2
                && x.Name == "Image2"
                && x.Link == "/image2");
        }

        [Fact]
        public async Task ImageService_AddAsync_AddsNewImage() {

            _unitOfWorkMock.Setup(x => x.ImageRepository.AddAsync(It.IsAny<Image>())).Verifiable();

            await _imageService.AddAsync(_mapper.Map<ImageDTO>(It.IsAny<Image>()));

            _unitOfWorkMock.Verify(x => x.ImageRepository.AddAsync(It.IsAny<Image>()), Times.Once());
        }

        [Fact]
        public async Task ImageService_Remove_RemovesOneImage() {

            var image = new Image { 
                ImageId = 2, 
                Caption = "Caption2", 
                Name = "Image2", 
                Link = "/image2", 
                CategoryId = 3, 
                UserRefId = "user2"
            };
            var images = GetImagesList();
            _unitOfWorkMock.Setup(x => x.ImageRepository.GetByIdAsync(image.ImageId)).ReturnsAsync(image);
            _unitOfWorkMock.Setup(x => x.ImageRepository.Remove(image)).Verifiable();

            await _imageService.Remove(_mapper.Map<ImageDTO>(image));

            _unitOfWorkMock.Verify(x => x.ImageRepository.Remove(It.IsAny<Image>()), Times.Once());
        }

        [Fact]
        public async Task ImageService_Remove_ThrowsNoSuchEntityException() {

            var image = new Image {
                ImageId = 4,
                Caption = "Caption2",
                Name = "Image2",
                Link = "/image2",
                CategoryId = 3,
                UserRefId = "user2"
            };
            var images = GetImagesList();
            _unitOfWorkMock.Setup(x => x.ImageRepository.GetAllAsync()).ReturnsAsync(images);
            _unitOfWorkMock.Setup(x => x.ImageRepository.Remove(image)).Throws(new NoSuchEntityException(""));

            await Assert.ThrowsAsync<NoSuchEntityException>(
                () => _imageService.Remove(
                    _mapper.Map<ImageDTO>(image)));
        }

        [Fact]
        public async Task ImageService_Update_ThrowsNoSuchEntityException() {

            var image = new Image {
                ImageId = 4,
                Caption = "Caption2",
                Name = "Image2",
                Link = "/image2",
                CategoryId = 3,
                UserRefId = "user2"
            };
            var images = GetImagesList();
            _unitOfWorkMock.Setup(x => x.ImageRepository.GetAllAsync()).ReturnsAsync(images);
            _unitOfWorkMock.Setup(x => x.ImageRepository.Update(image)).Throws(new NoSuchEntityException(""));

            await Assert.ThrowsAsync<NoSuchEntityException>(
                () => _imageService.Update(
                    _mapper.Map<ImageDTO>(image)));
        }

        [Fact]
        public async Task ImageService_Update_UpdatesOneImage() {

            var image = new Image {
                ImageId = 1,
                Caption = "New Caption!",
                Name = "Image1",
                Link = "/image1",
                CategoryId = 1,
                UserRefId = "user"
            };
            var images = GetImagesList();
            _unitOfWorkMock.Setup(x => x.ImageRepository.GetByIdAsync(image.ImageId)).ReturnsAsync(images.First());
            _unitOfWorkMock.Setup(x => x.ImageRepository.Update(image)).Verifiable();

            await _imageService.Update(_mapper.Map<ImageDTO>(image));

            _unitOfWorkMock.Verify(x => x.ImageRepository.Update(It.IsAny<Image>()), Times.Once());
        }

        private IEnumerable<Image> GetImagesList() {
            return new List<Image> {
                new Image { ImageId = 1, Caption = "Caption", Name = "Image1", Link = "/image1", CategoryId = 1, UserRefId = "user"},
                new Image { ImageId = 2, Caption = "Caption2", Name = "Image2", Link = "/image2", CategoryId = 3, UserRefId = "user2"},
                new Image { ImageId = 3, Caption = "Caption3", Name = "Image3", Link = "/image3", CategoryId = 1, UserRefId = "user"},
            };
        }
    }
}
