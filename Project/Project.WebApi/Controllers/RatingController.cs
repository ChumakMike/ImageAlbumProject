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
    [Route("api/ratings")]
    [ApiController]
    public class RatingController : ControllerBase {

        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        public RatingController(IMapper mapper, IRatingService ratingService) {
            _mapper = mapper;
            _ratingService = ratingService;
        }

        /// <summary>
        /// Get all ratings
        /// </summary>
        /// <returns>
        /// <response code="200">Ratings list</response>
        /// <response code="404">Not Found</response>
        /// </returns>
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAll() {
            var ratingsList = _mapper.Map<IEnumerable<RatingVM>>
               (await _ratingService.GetAllAsync());
            return (ratingsList == null)
                ? (IActionResult)NotFound()
                : Ok(ratingsList);
        }

        /// <summary>
        ///  Create rating
        /// </summary>
        /// <param name="rating"></param>
        /// <returns>
        /// <response code="200">Image created</response>
        /// <response code="404">Not Found</response>
        /// </returns>
        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddRating(RatingVM rating) {
            if(rating == null)
                return BadRequest("The rating model is null");
            bool result = await _ratingService.AddAsync(_mapper.Map<RatingDTO>(rating));
            return (result == true)
                ? Ok()
                : (IActionResult)NotFound();
        }

        /// <summary>
        ///  Gets rating by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <response code="200">Rating found</response>
        /// <response code="400">Bad request</response>
        /// </returns>
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetById(int id) {
            var rating = _mapper.Map<RatingVM>(await _ratingService.GetByIdAsync(id));
            return Ok(rating);
        }

    }
}
