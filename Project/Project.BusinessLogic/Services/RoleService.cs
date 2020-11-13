using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Exceptions;
using Project.BusinessLogic.Identity;
using Project.BusinessLogic.Interfaces;
using Project.Data.Context;
using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.BusinessLogic.Services {
    public class RoleService : IRoleService {
        private IMapper mapper;
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
        public RoleService(IMapper map, ApplicationRoleManager roleManager, ApplicationUserManager userManager) {
            mapper = map ?? throw new ArgumentNullException(nameof(map));
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task CreateRoleAsync(RoleDTO role) {
            var existingRole = await _roleManager.FindByNameAsync(role.Name);
            if (existingRole == null) {
                IdentityRole roleToCreate = new IdentityRole { Name = role.Name };
                await _roleManager.CreateAsync(roleToCreate);
            }
                
            else throw new RoleException($"The role with name: {existingRole.Name} already exists!");
        }

        public async Task DeleteRoleAsync(RoleDTO role) {
            var existingRole = await _roleManager.FindByNameAsync(role.Name);
            if (existingRole != null)
                await _roleManager.DeleteAsync(mapper.Map<IdentityRole>(existingRole));
            else throw new RoleException($"The role with name: {existingRole.Name} does not exist!");
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesByUserAsync(UserDTO user) {
            return mapper.Map<IEnumerable<RoleDTO>>
                (await _userManager.GetRolesAsync(
                    mapper.Map<ApplicationUser>(user)
                ));
        }

        public IEnumerable<RoleDTO> GetAllRoles() {
            return mapper.Map<IEnumerable<RoleDTO>>(_roleManager.Roles);
        }
    }
}
