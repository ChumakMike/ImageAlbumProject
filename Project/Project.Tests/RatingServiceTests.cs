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

namespace Project.Tests {
    public class RatingServiceTests {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly RatingService _ratingService;
        public RatingServiceTests() {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapper = configuration.CreateMapper();
            _ratingService = new RatingService(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task RatingService_GetAll_ReturnsAllElementsWithCorrectValues() {

            var ratings = GetRatingsList();
            _unitOfWorkMock.Setup(x => x.RatingRepository.GetAllAsync()).ReturnsAsync(ratings);

            var result = await _ratingService.GetAllAsync();

            Assert.Contains(result, x => x.RatingId == 1);
            Assert.Contains(result, x => x.RatingId == 2);
            _unitOfWorkMock.Verify(x => x.RatingRepository.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task RatingService_GetAll_ReturnsAllElementsWithCorrectType() {

            var ratings = GetRatingsList();
            _unitOfWorkMock.Setup(x => x.RatingRepository.GetAllAsync()).ReturnsAsync(ratings);

            var result = await _ratingService.GetAllAsync();

            Assert.IsAssignableFrom<IEnumerable<RatingDTO>>(result);
            _unitOfWorkMock.Verify(x => x.RatingRepository.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task RatingService_GetAll_ReturnsAllTwoElements() {

            var ratings = GetRatingsList();
            _unitOfWorkMock.Setup(x => x.RatingRepository.GetAllAsync()).ReturnsAsync(ratings);

            var result = (await _ratingService.GetAllAsync()).Count();
            var expectedCount = 2;

            _unitOfWorkMock.Verify(x => x.RatingRepository.GetAllAsync(), Times.Once());
            Assert.Equal(expectedCount, result);
        }

        [Theory]
        [InlineData(3)]
        public async Task RatingService_GetById_ReturnsElementByGivenId(int id) {

            int expectedRatingId = 3;
            int expectedImageId = 6;
            string expectedUserId = "user1234";
            
            var rating = new Rating { 
                RatingId = expectedRatingId, 
                ImageId = expectedImageId, 
                UserId = expectedUserId 
            };
            _unitOfWorkMock.Setup(x => x.RatingRepository.GetByIdAsync(id)).ReturnsAsync(rating);

            var result = await _ratingService.GetByIdAsync(id);

            _unitOfWorkMock.Verify(x => x.RatingRepository.GetByIdAsync(id), Times.Once());
            Assert.Equal(expectedRatingId, result.RatingId);
            Assert.Equal(expectedUserId, result.UserId);
            Assert.Equal(expectedImageId, result.ImageId);
        }

        [Fact]
        public async Task RatingService_AddAsync_AddsNewRatingToEmptyBase() {
           
            int expectedRatingId = 3;
            int expectedImageId = 6;
            string expectedUserId = "user1234";

            var rating = new RatingDTO { RatingId = expectedRatingId, ImageId = expectedImageId, UserId = expectedUserId };
            _unitOfWorkMock.Setup(x => x.RatingRepository.AddAsync(It.IsAny<Rating>())).Verifiable();

            var result = await _ratingService.AddAsync(rating);

            Assert.True(result);
            _unitOfWorkMock.Verify(x => x.RatingRepository.AddAsync(It.IsAny<Rating>()), Times.Once());
        }

        [Fact]
        public async Task RatingService_GetRatingMark_GetsRightRatingValue() {
            var ratings = GetRatingsList();
            _unitOfWorkMock.Setup(x => x.RatingRepository.GetAllAsync()).ReturnsAsync(ratings);

            int result = await _ratingService.GetRatingMarkAsync(1);

            int expected = 2;
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task RatingService_GetRatingByUserAndImageIds_GetsRightRating() {
            var ratings = GetRatingsList();
            _unitOfWorkMock.Setup(x => x.RatingRepository.GetAllAsync()).ReturnsAsync(ratings);

            var result = await _ratingService.GetByUserAndImageAsync("user1", 1);

            var expected = _mapper.Map<RatingDTO>(ratings.FirstOrDefault());
            Assert.Equal(expected.ImageId, result.ImageId);
            Assert.Equal(expected.RatingId, result.RatingId);
            Assert.Equal(expected.UserId, result.UserId);
        }

        [Fact]
        public async Task RatingService_GetRatingByUserId_GetsRightRating() {
            var ratings = GetRatingsList();
            _unitOfWorkMock.Setup(x => x.RatingRepository.GetAllAsync()).ReturnsAsync(ratings);

            var result = await _ratingService.GetByUserIdAsync("user1");

            var expected = _mapper.Map<RatingDTO>(ratings.FirstOrDefault());
            Assert.Contains(result, x => 
                x.UserId == expected.UserId 
                && x.RatingId == expected.RatingId 
                && x.ImageId == expected.ImageId);
        }
        private IEnumerable<Rating> GetRatingsList() {
            return new List<Rating> {
                new Rating { RatingId = 1, ImageId = 1, UserId = "user1" },
                new Rating { RatingId = 2, ImageId = 1, UserId = "user2" }
            };
        }
    }
}