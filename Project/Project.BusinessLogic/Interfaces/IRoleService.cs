using Microsoft.AspNetCore.Identity;
using Project.BusinessLogic.EntitiesDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.BusinessLogic.Interfaces {
    public interface IRoleService {
        Task CreateRoleAsync(RoleDTO role);
        Task DeleteRoleAsync(RoleDTO role);
        Task<IList<string>> GetRolesByUserAsync(UserDTO user);
        IEnumerable<RoleDTO> GetAllRoles();
    }
}
