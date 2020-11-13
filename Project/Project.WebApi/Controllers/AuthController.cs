using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Project.BusinessLogic.Services;
using Project.WebApi.Helpers;

namespace Project.WebApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly ApplicationSettingsHelper _applicationSettings;
        private readonly UserService _userService;
        public AuthController(IOptions<ApplicationSettingsHelper> applicationSettings, UserService userService) {
            _applicationSettings = applicationSettings.Value;
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) {

            return Ok();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Object obj) {
            return Ok();
        }
    }
}
