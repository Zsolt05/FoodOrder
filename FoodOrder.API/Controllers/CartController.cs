using FoodOrder.Core.Constans;
using FoodOrder.Core.Models;
using FoodOrder.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = $"{Roles.User}")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            await _cartService.AddToCart(productId, quantity);
            return Ok();
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            await _cartService.RemoveFromCart(productId);
            return Ok();
        }

        [HttpPost("clear")]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCart();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCartFoods(int pageNumber, int pageSize)
        {
            PagedResult.CheckParameters(ref pageNumber, ref pageSize);
            var cartFoods = await _cartService.GetCartFoods(pageNumber, pageSize);
            return Ok(cartFoods);
        }

        [HttpPost("finish")]
        public async Task<IActionResult> FinishOrder()
        {
            await _cartService.FisishOrder();
            return Ok();
        }
    }
}
