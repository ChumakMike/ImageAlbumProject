using Project.BusinessLogic.EntitiesDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.BusinessLogic.Interfaces {
    public interface IUserService {
        Task<UserDTO> GetByIdAsync(string id);
        Task<UserDTO> GetByUserNameAsync(string userName);
        Task<IEnumerable<UserDTO>> GetAll();
        Task AddUserToRoleAsync(string userName, string role);
        Task AddUserAsync(UserDTO user);
        Task UpdateAsync(UserDTO user);
        Task RemoveAsync(UserDTO user);
        Task<bool> AuthenticateAsync(UserDTO user);
        Task UpdateRoleAsync(UserDTO user, string newRole);
    }
}
