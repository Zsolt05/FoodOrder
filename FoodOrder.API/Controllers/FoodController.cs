using AutoMapper;
using FoodOrder.Core.Constans;
using FoodOrder.Core.Models;
using FoodOrder.Core.Models.Food;
using FoodOrder.Core.Services;
using FoodOrder.Data.Entities;
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
            ConvertPagedResult(ref foods, out var foodDtos);
            return Ok(foodDtos);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FoodDto))]
        public async Task<IActionResult> GetFood(int id)
        {
            var food = await _foodService.GetFood(id);
            var foodDto = _mapper.Map<FoodDto>(food);
            return Ok(foodDto);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<FoodDto>))]
        public async Task<IActionResult> CreateFood([FromBody] CreateFoodDto createFoodDto)
        {
            var foods = await _foodService.CreateFood(createFoodDto);
            ConvertPagedResult(ref foods, out var foodDtos);
            return Ok(foodDtos);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<FoodDto>))]
        public async Task<IActionResult> UpdateFood(int id, [FromBody] CreateFoodDto updateFoodDto)
        {
            var foods = await _foodService.UpdateFood(id, updateFoodDto);
            ConvertPagedResult(ref foods, out var foodDtos);
            return Ok(foodDtos);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<FoodDto>))]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var foods = await _foodService.DeleteFood(id);
            ConvertPagedResult(ref foods, out var foodDtos);
            return Ok(foodDtos);
        }

        private void ConvertPagedResult(ref PagedResult<Food> foods, out PagedResult<FoodDto> foodDtos)
        {
            foodDtos = new PagedResult<FoodDto>
            {
                CurrentPage = foods.CurrentPage,
                PageSize = foods.PageSize,
                TotalItems = foods.TotalItems,
                TotalPages = foods.TotalPages,
                Items = _mapper.Map<List<FoodDto>>(foods.Items)
            };
        }
    }
}
