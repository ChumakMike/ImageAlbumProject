﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Interfaces;
using Project.WebApi.Helpers;
using Project.WebApi.Models;

namespace Project.WebApi.Controllers {
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        private readonly IImageService _imageService;

        public UsersController(IUserService userService, IMapper mapper, 
            IRatingService ratingService, IImageService imageService) {
            _userService = userService;
            _mapper = mapper;
            _ratingService = ratingService;
            _imageService = imageService;
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns>
        /// <response code="200">Users list</response>
        /// <response code="400">Bad request</response>
        /// </returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll() {
            var usersList = _mapper.Map<IEnumerable<UserVM>>(
                await _userService.GetAll());
            if (usersList == null) return NotFound();
            else return Ok(usersList);
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// <response code="200">User found</response>
        /// <response code="404">Not Found</response>
        /// </returns>
        [HttpGet]
        [Route("{username}")]
        [Authorize(Roles = "Admin, User, Manager")]
        public async Task<IActionResult> GetByName(string username) {
            var user = _mapper.Map<UserVM>(
                await _userService.GetByUserNameAsync(username));
            if (user == null) return NotFound();
            else return Ok(user);
        }

        /// <summary>
        /// Get Current User 
        /// </summary>
        /// <returns>
        /// <response code="200">User found</response>
        /// <response code="404">Not Found</response>
        /// </returns>
        [HttpGet]
        [Route("profile")]
        [Authorize(Roles = "Admin, User, Manager")]
        public async Task<IActionResult> GetCurrentUserDataById() {
            var options = new IdentityOptions();
            string userId = User.Claims.First(c => c.Type == options.ClaimsIdentity.UserIdClaimType).Value;
            var user = _mapper.Map<UserVM>(
                await _userService.GetByIdAsync(userId));
            if (user == null) return NotFound();
            else return Ok(user);
        }

        /// <summary>
        /// Add user to role
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// <response code="200">User added</response>
        /// </returns>
        [HttpPost]
        [Route("addrole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddToRole([FromBody] UserVM user) {
            await _userService.AddUserToRoleAsync(user.UserName, user.Role);
            return Ok();
        }

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// <response code="200">User updated</response>
        /// </returns>
        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Update([FromBody]UserVM user) {
            await _userService.UpdateAsync(_mapper.Map<UserDTO>(user));
            return Ok();
        }

        /// <summary>
        /// Update user to role
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// <response code="200">User to role updated</response>
        /// </returns>
        [HttpPut]
        [Route("roleupdate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserVM user) {
            await _userService.UpdateRoleAsync(_mapper.Map<UserDTO>(user), user.Role);
            return Ok();
        }


        /// <summary>
        /// Get Rated Posts 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <response code="200">Rated Posts  found</response>
        /// <response code="404">Not Found</response>
        /// </returns>
        [HttpGet]
        [Route("{id}/rated")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRated(string id) {
            var ratingsList = _mapper.Map<IEnumerable<RatingVM>>(await _ratingService.GetByUserIdAsync(id));
            return (ratingsList == null)
                ? (IActionResult)NotFound()
                : Ok(ratingsList);
        }

        /// <summary>
        /// Get all user's images
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <response code="200">User's images found</response>
        /// <response code="404">Not Found</response>
        /// </returns>
        [HttpGet]
        [Route("{id}/images")]
        [Authorize(Roles = "Admin, Manager, User")]
        public async Task<IActionResult> GetImagesByUserId(string id) {
            var imagesList = _mapper.Map<ImageVM>(await _imageService.GetByUserIdAsync(id));
            return (imagesList == null)
                ? (IActionResult)NotFound()
                : Ok(imagesList);
        }
    }
}
