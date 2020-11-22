using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Interfaces;
using Project.BusinessLogic.Services;
using Project.WebApi.Models;

namespace Project.WebApi.Controllers {
    [Route("api/roles")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RolesController(IRoleService roleService, IMapper mapper) {
            _roleService = roleService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new role
        /// </summary>
        /// <param name="role"></param>
        /// <returns>
        /// <response code="200">Role is created</response>
        /// <response code="400">Bad request</response>
        /// </returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRole([FromBody] RoleVM role) {
            if (role == null)
                return BadRequest("The role model is null");
            await _roleService.CreateRoleAsync(_mapper.Map<RoleDTO>(role)); 
            return Ok();
        }

        /// <summary>
        /// Deletes the role
        /// </summary>
        /// <param name="role"></param>
        /// <returns>
        /// <response code="200">Role is deleted</response>
        /// <response code="400">Bad request</response>
        /// </returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteRole([FromBody]RoleVM role) {
            if (role == null)
                return BadRequest("The role model is null");
            await _roleService.DeleteRoleAsync(_mapper.Map<RoleDTO>(role));
            return Ok();
        }


        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>
        /// <response code="200">Roles list</response>
        /// <response code="400">Bad request</response>
        /// </returns>
        [HttpGet]
        public IActionResult GetRoles() {
            var rolesList = _mapper.Map<IEnumerable<RoleVM>>(_roleService.GetAllRoles());
            return Ok(rolesList);
        }

        /// <summary>
        /// Get all roles by user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        /// <response code="200">Roles list</response>
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> GetRolesByUser([FromBody]UserVM user) {
            var userDTO = _mapper.Map<UserDTO>(user);
            var rolesList = _mapper.Map<IEnumerable<RoleVM>>(await _roleService.GetRolesByUserAsync(userDTO));
            return Ok(rolesList);
        }
    }
}
