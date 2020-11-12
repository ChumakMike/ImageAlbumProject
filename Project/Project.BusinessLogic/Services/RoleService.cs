using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Exceptions;
using Project.BusinessLogic.Identity;
using Project.BusinessLogic.Interfaces;
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
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task CreateRoleAsync(RoleDTO role) {
            var existingRole = await _roleManager.FindByNameAsync(role.Name);
            if (existingRole == null)
                await _roleManager.CreateAsync(mapper.Map<IdentityRole>(role));
            else throw new RoleException($"The role with name: {existingRole.Name} already exists!");
        }

        public async Task DeleteRoleASync(RoleDTO role) {
            var existingRole = await _roleManager.FindByNameAsync(role.Name);
            if (existingRole != null)
                await _roleManager.DeleteAsync(mapper.Map<IdentityRole>(role));
            else throw new RoleException($"The role with name: {existingRole.Name} does not exist!");
        }

        public async Task<IEnumerable<RoleDTO>> GetRolesByUserAsync(UserDTO user) {
            return mapper.Map<IEnumerable<RoleDTO>>
                (await _userManager.GetRolesAsync(
                    mapper.Map<ApplicationUser>(user)
                ));
        }
    }
}
