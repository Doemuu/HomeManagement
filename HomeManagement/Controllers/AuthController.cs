using HomeManagement.Entities;
using HomeManagement.Models;
using HomeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Controllers
{
    [ApiController]
    [Route("/Auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest user)
        {
            var result = await _authService.RegisterUser(user);
            if (result == null)
                return BadRequest("invalid_entry");

            return Json(result);
        }

        [HttpPut]
        public async Task<IActionResult> Login([FromBody] AuthenticationRequest model)
        {
            var result = await _authService.Authenticate(model);
            if (result == null)
                return BadRequest("invalid_entry");

            return Json(result);
        }

        [HttpGet("{token}/revoke")]
        public async Task<IActionResult> RevokeToken(string token)
        {
            var result = await _authService.RevokeToken(token);
            if (!result)
                return BadRequest("invalid_token");

            return Ok();
        }

        [HttpGet("{token}/refresh")]
        public async Task<IActionResult> RefreshToken(string token)
        {
            var result = await _authService.RefreshToken(token);
            if (result == null)
                return BadRequest("invalid_token");

            return Ok();
        }
    }
}
