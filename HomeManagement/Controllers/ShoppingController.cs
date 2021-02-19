using HomeManagement.Entities;
using HomeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeManagement.Controllers
{
    [ApiController]
    [Route("/Shopping")]
    public class ShoppingController : Controller
    {
        private readonly ShoppingService _shoppingService;
        public ShoppingController(ShoppingService shoppingService)
        {
            _shoppingService = shoppingService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetShoppingList()
        {
            var isAdmin = HttpContext.Items["user"].ToString();
            if (isAdmin != "True")
                return BadRequest("unauthorised_request");

            var result = await _shoppingService.GetShoppingList();
            
            if (result == null)
                return BadRequest("inexistent_shopping_list");

            return Json(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShoppingItemById(int id)
        {
            var result = await _shoppingService.GetShoppingItem(id);

            if (result == null)
                return BadRequest("inexistent_shopping_list");

            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddShoppingItemToShoppingList([FromBody] ShoppingItem item)
        {
            var result = await _shoppingService.AddShoppingItem(item);

            if (!result.Success)
                return BadRequest(result.Exception);

            return Json(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditShoppingItem(int id, [FromBody] ShoppingItem item)
        {
            var result = await _shoppingService.EditShoppingItem(id, item);

            if (!result.Success)
                return BadRequest(result.Exception);

            return Json(result);
        }
    }
}
