using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Project.BusinessLogic.EntitiesDTO;
using Project.BusinessLogic.Interfaces;
using Project.BusinessLogic.Services;
using Project.WebApi.Helpers;
using Project.WebApi.Models.Auth;

namespace Project.WebApi.Controllers {

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly ApplicationSettingsHelper _applicationSettings;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AuthController(IOptions<ApplicationSettingsHelper> applicationSettings,
            IUserService userService, IMapper mapper) {
            _applicationSettings = applicationSettings.Value;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) {

            return Ok();
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
    }
}
