using AutoMapper;
using FoodOrder.Core.Models;
using FoodOrder.Core.Models.Food;
using FoodOrder.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;
        private readonly IMapper _mapper;

        public FoodController(IFoodService foodService, IMapper mapper)
        {
            _foodService = foodService;
            _mapper = mapper;
        }

        [HttpGet("category")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<FoodCategoryDto>))]
        public async Task<IActionResult> GetCategories(int pageNumber, int pageSize)
        {
            PagedResult.CheckParameters(ref pageNumber, ref pageSize);
            var categories = await _foodService.GetCategories(pageNumber, pageSize);
            var categoryDtos = new PagedResult<FoodCategoryDto>
            {
                CurrentPage = categories.CurrentPage,
                PageSize = categories.PageSize,
                TotalItems = categories.TotalItems,
                TotalPages = categories.TotalPages,
                Items = _mapper.Map<List<FoodCategoryDto>>(categories.Items)
            };
            return Ok(categoryDtos);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<FoodDto>))]
        public async Task<IActionResult> GetFoods(int pageNumber, int pageSize)
        {
            PagedResult.CheckParameters(ref pageNumber, ref pageSize);
            var foods = await _foodService.GetFoods(pageNumber, pageSize);
            var foodDtos = new PagedResult<FoodDto>
            {
                CurrentPage = foods.CurrentPage,
                PageSize = foods.PageSize,
                TotalItems = foods.TotalItems,
                TotalPages = foods.TotalPages,
                Items = _mapper.Map<List<FoodDto>>(foods.Items)
            };
            return Ok(foodDtos);
        }
    }
}
