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
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase {

        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;
        public CategoriesController(IMapper mapper, ICategoryService categoryService, IImageService imageService) {
            _mapper = mapper;
            _categoryService = categoryService;
            _imageService = imageService;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>
        /// <response code="200">Categories list</response>
        /// <response code="404">Not Found</response>
        /// </returns>
        [HttpGet]
        [Authorize(Roles = "Admin, User, Manager")]
        public async Task<IActionResult> GetAll() {
            var categoriesList = _mapper.Map<IEnumerable<CategoryVM>>
                (await _categoryService.GetAllAsync());
            return (categoriesList == null) 
                ? (IActionResult)NotFound()
                : Ok(categoriesList);
        }


        /// <summary>
        /// Create a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>
        /// <response code="200">Category created</response>
        /// <response code="400">Bad request</response>
        /// </returns>

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Create([FromBody] CategoryVM category) {
            if (category == null)
                return BadRequest("The category model is null");
            await _categoryService.AddAsync(_mapper.Map<CategoryDTO>(category));
            return Ok();
        }

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>
        /// <response code="200">Category deleted</response>
        /// <response code="400">Bad request</response>
        /// </returns>
        [HttpPost]
        [Route("delete")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Delete([FromBody] CategoryVM category) {
            if (category == null)
                return BadRequest("The category model is null");
            await _categoryService.Remove(_mapper.Map<CategoryDTO>(category));
            return Ok();
        }

        /// <summary>
        /// Gets category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <response code="200">Category found</response>
        /// <response code="404">Not Found</response>
        /// </returns>
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetById(int id) {
            var category = _mapper.Map<CategoryVM>(await _categoryService.GetByIdAsync(id));
            return (category == null) 
                ? (IActionResult)NotFound() 
                : Ok(category);
        }

        /// <summary>
        /// Gets images by category id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <response code="200">Images list found</response>
        /// <response code="404">Not Found</response>
        /// </returns>
        [HttpGet]
        [Route("{id}/images")]
        [Authorize(Roles = "Admin, Manager, User")]
        public async Task<IActionResult> GetImagesByCategoryId(int id) {
            var imagesList = _mapper.Map<ImageVM>(await _imageService.GetByCategoryIdAsync(id));
            return (imagesList == null)
                ? (IActionResult)NotFound()
                : Ok(imagesList);
        }
    }
}
