using HomeManagement.Entities;
using HomeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Controllers
{
    [ApiController]
    [Route("/User")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var result = await _userService.RegisterUser(user);
            if (!result.Success)
                return BadRequest(result.Exception);

            return Json(result);
        }
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserData(string username)
        {
            var result = await _userService.GetUserByUserName(username);
            if (result == null)
                return BadRequest("inexistent_user");

            return Json(result);
        }

        [HttpPut("/password")]
        public async Task<IActionResult> ChangePassword([FromBody] User user)
        {
            var result = await _userService.ChangePassword(user.UserName, user.UserPassword);
            if (!result.Success)
                return BadRequest(result.Exception);

            return Json(result);
        }

        [HttpPut("/userdata")]
        public async Task<IActionResult> EditUserData([FromBody] User user)
        {
            var result = await _userService.EditUserData(user);
            if (!result.Success)
                return BadRequest(result.Exception);

            return Json(result);
        }
    }
}
