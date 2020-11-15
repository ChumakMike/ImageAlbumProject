using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public UsersController(IUserService userService, IMapper mapper) {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll() {
            var usersList = _mapper.Map<IEnumerable<UserVM>>(
                await _userService.GetAll());
            if (usersList == null) return NotFound();
            else return Ok(usersList);
        }

        [HttpGet]
        [Route("{username}")]
        [Authorize(Roles = "Admin, User, Manager")]
        public async Task<IActionResult> GetById(string username) {
            var user = _mapper.Map<UserVM>(
                await _userService.GetByUserNameAsync(username));
            if (user == null) return NotFound();
            else return Ok(user);
        }

        [HttpPost]
        [Route("addrole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddToRole([FromBody] UserVM user) {
            await _userService.AddUserToRoleAsync(user.UserName, user.Role);
            return Ok();
        }
        
        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Update([FromBody]UserVM user) {
            await _userService.UpdateAsync(_mapper.Map<UserDTO>(user));
            return Ok();
        }

        [HttpPut]
        [Route("roleupdate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UserVM user) {
            await _userService.UpdateRoleAsync(_mapper.Map<UserDTO>(user), user.Role);
            return Ok();
        }

    }
}
