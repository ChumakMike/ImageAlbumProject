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

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRole([FromBody] RoleVM role) {
            if (role == null)
                return BadRequest("The role model is null");
            await _roleService.CreateRoleAsync(_mapper.Map<RoleDTO>(role)); 
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteRole([FromBody]RoleVM role) {
            if (role == null)
                return BadRequest("The role model is null");
            await _roleService.DeleteRoleAsync(_mapper.Map<RoleDTO>(role));
            return Ok();
        }

        [HttpGet]
        public IActionResult GetRoles() {
            var rolesList = _mapper.Map<IEnumerable<RoleVM>>(_roleService.GetAllRoles());
            return Ok(rolesList);
        }

        
        [HttpPost]
        public async Task<IActionResult> GetRolesByUser([FromBody]UserVM user) {
            var userDTO = _mapper.Map<UserDTO>(user);
            var rolesList = _mapper.Map<IEnumerable<RoleVM>>(await _roleService.GetRolesByUserAsync(userDTO));
            return Ok(rolesList);
        }
    }
}
