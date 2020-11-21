using AutoMapper;
using Moq;
using Project.BusinessLogic.Mapping;
using Project.BusinessLogic.Services;
using Project.Data.Entities;
using Project.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Tests.TestBase {
    public class ApplicationServicesTestBase {
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly IMapper _mapper;
        protected readonly CategoryService _categoryService;
        protected readonly ImageService _imageService;
        protected readonly RatingService _ratingService;

        public ApplicationServicesTestBase() {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = configuration.CreateMapper();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _categoryService = new CategoryService(_unitOfWorkMock.Object, _mapper);
            _imageService = new ImageService(_unitOfWorkMock.Object, _mapper);
            _ratingService = new RatingService(_unitOfWorkMock.Object, _mapper);
        }

        protected IEnumerable<Category> GetCategoriesList() {
            return new List<Category> {
                new Category {CategoryId = 1, Name = "Nature"},
                new Category {CategoryId = 2, Name = "Animals"}
            };
        }

        protected IEnumerable<Image> GetImagesList() {
            return new List<Image> {
                new Image { 
                    ImageId = 1, 
                    Caption = "Caption", 
                    Name = "Image1", 
                    Link = "/image1", 
                    CategoryId = 1, 
                    UserRefId = "user"
                },
                new Image { 
                    ImageId = 2, 
                    Caption = "Caption2", 
                    Name = "Image2", 
                    Link = "/image2", 
                    CategoryId = 3, 
                    UserRefId = "user2"
                },
                new Image { 
                    ImageId = 3, 
                    Caption = "Caption3", 
                    Name = "Image3", 
                    Link = "/image3",
                    CategoryId = 1, 
                    UserRefId = "user"
                },
            };
        }

        protected IEnumerable<Rating> GetRatingsList() {
            return new List<Rating> {
                new Rating { RatingId = 1, ImageId = 1, UserId = "user1" },
                new Rating { RatingId = 2, ImageId = 1, UserId = "user2" }
            };
        }
    }
}
