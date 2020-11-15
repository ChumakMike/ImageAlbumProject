using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class UserService : IUserService {

        private IMapper mapper;
        private ApplicationUserManager _userManager;
        public UserService(IMapper map, ApplicationUserManager userManager) {
            mapper = map ?? throw new ArgumentNullException(nameof(map));
            _userManager = userManager;
        }
        public async Task AddUserAsync(UserDTO user) {
            string defaultStatus = "Active";
            ApplicationUser userToAdd = new ApplicationUser {
                UserName = user.UserName,
                Email = user.Email,
                Status = defaultStatus
            };
            IdentityResult result = await _userManager.CreateAsync(userToAdd, user.Password);
            if (!result.Succeeded) throw new UserInvalidOperationException(result.ToString());
        }

        public async Task AddUserToRoleAsync(string userName, string role) {
            var existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser == null) throw new NoSuchEntityException(existingUser.GetType().Name);

            IdentityResult result = await _userManager.AddToRoleAsync(existingUser, role);
            if (!result.Succeeded) throw new UserInvalidOperationException(result.ToString());
        }

        public async Task<bool> AuthenticateAsync(UserDTO user) {
            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            if (existingUser == null || !existingUser.Status.Equals("Active")) return false;

            return await _userManager.CheckPasswordAsync(existingUser, user.Password);
        }

        public async Task<IEnumerable<UserDTO>> GetAll() {
            return mapper.Map<IEnumerable<UserDTO>>(
                    await _userManager.Users.ToListAsync()
                );
        }

        public async Task<UserDTO> GetByIdAsync(string id) {
            return mapper.Map<UserDTO>(await _userManager.FindByIdAsync(id));
        }

        public async Task<UserDTO> GetByUserNameAsync(string userName) {
            return mapper.Map<UserDTO>(await _userManager.FindByNameAsync(userName));
        }

        public async Task RemoveAsync(UserDTO user) {
            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            if (existingUser == null) throw new NoSuchEntityException(existingUser.GetType().Name);

            await _userManager.DeleteAsync(existingUser);
        }

        public async Task UpdateAsync(UserDTO user) {
            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            if (existingUser == null) throw new NoSuchEntityException(existingUser.GetType().Name);

            await _userManager.UpdateAsync(mapper.Map<ApplicationUser>(user));
        }
    }
}
