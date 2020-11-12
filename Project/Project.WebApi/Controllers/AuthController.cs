using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Project.WebApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        public AuthController() {

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
