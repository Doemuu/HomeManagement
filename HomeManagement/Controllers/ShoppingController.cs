using HomeManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        public async Task<IActionResult> GetShoppingList()
        {
            var result = await _shoppingService.GetShoppingList();
            
            if (result == null)
                return BadRequest("inexistent_shopping_list");

            return Json(result);
        }
    }
}
