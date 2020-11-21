using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Interfaces;
using Project.WebApi.Models;

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

        [HttpGet]
        [Authorize(Roles = "Admin, Manager, User")]
        public async Task<IActionResult> GetAll() {
            var imagesList = _mapper.Map<IEnumerable<ImageVM>>
             (await _imageService.GetAllAsync());
            return (imagesList == null)
                ? (IActionResult)NotFound()
                : Ok(imagesList);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Admin, Manager, User")]
        public async Task<IActionResult> GetById(int id) {
            var image = _mapper.Map<ImageVM>(await _imageService.GetByIdAsync(id));
            image.Rating = await _ratingService.GetRatingMarkAsync(image.ImageId);
            return Ok(image);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("create")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddImage([FromBody] ImageVM image) {
            if (image == null)
                return BadRequest("The image model is null");
            try {

                //check

                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "ImagesStorage");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0) {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create)) {
                        file.CopyTo(stream);
                    }
                    
                    image.CreatedAt = DateTime.Now;
                    image.Name = fileName;
                    image.Link = fullPath;

                    await _imageService.AddAsync(_mapper.Map<ImageDTO>(image));
                    return Ok();
                }
                else return BadRequest();

            }
            catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = "Manager, User")]
        public async Task<IActionResult> DeleteImage([FromBody] ImageVM image) {
            if (image == null)
                return BadRequest("The image model is null");

            await _imageService.Remove(_mapper.Map<ImageDTO>(image));
            return Ok();
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Manager, User")]
        public async Task<IActionResult> UpdateImage([FromBody] ImageVM image) {
            if (image == null)
                return BadRequest("The image model is null");
            await _imageService.Update(_mapper.Map<ImageDTO>(image));
            return Ok();
        }

    }
}
