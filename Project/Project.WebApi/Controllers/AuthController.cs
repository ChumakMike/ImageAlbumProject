using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Interfaces;
using Project.BusinessLogic.Services;
using Project.WebApi.Helpers;
using Project.WebApi.Models;
using Project.WebApi.Models.Auth;

namespace Project.WebApi.Controllers {

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly ApplicationSettingsHelper _applicationSettings;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        public AuthController(IOptions<ApplicationSettingsHelper> applicationSettings,
            IUserService userService, IMapper mapper,
            IRoleService roleService) {
            _applicationSettings = applicationSettings.Value;
            _userService = userService;
            _mapper = mapper;
            _roleService = roleService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) {
            if(await _userService.AuthenticateAsync(_mapper.Map<UserDTO>(model))) {

                var existingUser = await _userService.GetByUserNameAsync(model.UserName); 
                IList<string> roles = await _roleService.GetRolesByUserAsync(existingUser);

                SecurityTokenDescriptor tokenDescriptor = CreateTokenDescriptor(roles, existingUser.Id.ToString());
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);               
                var token = tokenHandler.WriteToken(securityToken);

                return Ok(new { token });
            }
            return BadRequest(new { 
                message = "Username or password is incorrect! " +
                "Your account may be blocked, connect with our support team!" 
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Models.Auth.RegisterModel user) {
            if(user == null)
                return BadRequest("The user model is null");
            string defaultRole = "User";
            await _userService.AddUserAsync(_mapper.Map<UserDTO>(user));
            await _userService.AddUserToRoleAsync(user.UserName, defaultRole);
            return Ok();
        }

        private SecurityTokenDescriptor CreateTokenDescriptor(IList<string> roles, string userId) {
            IdentityOptions options = new IdentityOptions();
            var tokenDescriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(options.ClaimsIdentity.UserIdClaimType, userId),
                        new Claim(options.ClaimsIdentity.RoleClaimType, roles.FirstOrDefault())
                    }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_applicationSettings.JWT_Secret)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenDescriptor;
        }
    }
}
