using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Interfaces;
using Project.WebApi.Models;

namespace Project.WebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase {

        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        public RatingController(IMapper mapper, IRatingService ratingService) {
            _mapper = mapper;
            _ratingService = ratingService;
        }

        /*
            Task<RatingDTO> GetByIdAsync(int id);
            Task<bool> AddAsync(RatingDTO entity);
            Task<IEnumerable<RatingDTO>> GetByUserIdAsync(string userId);
            Task<int> GetRatingMarkAsync(int imageId);
            Task<RatingDTO> GetByUserAndImageAsync(string userId, int imageId);
        */
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAll() {
            var ratingsList = _mapper.Map<IEnumerable<RatingVM>>
               (await _ratingService.GetAllAsync());
            return (ratingsList == null)
                ? (IActionResult)NotFound()
                : Ok(ratingsList);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddRating(RatingVM rating) {
            if(rating == null)
                return BadRequest("The rating model is null");
            bool result = await _ratingService.AddAsync(_mapper.Map<RatingDTO>(rating));
            return (result == true)
                ? Ok()
                : (IActionResult)BadRequest();
        }


    }
}
