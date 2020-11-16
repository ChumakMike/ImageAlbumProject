using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.BusinessLogic.Interfaces;

namespace Project.WebApi.Controllers {
    [Route("api/images")]
    [ApiController]
    public class ImagesController : ControllerBase {
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        private readonly IImageService _imageService;

        public ImagesController(IMapper mapper, IRatingService ratingService, IImageService imageService) {
            _mapper = mapper;
            _ratingService = ratingService;
            _imageService = imageService;
        }

        //   Task<int> GetRatingMarkAsync(int imageId); --> image cntrl

    }
}
