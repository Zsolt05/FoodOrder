using AutoMapper;
using FoodOrder.Core.Models.User;
using FoodOrder.Core.Services;
using FoodOrder.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseDto))]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var response = await _authService.Login(loginDto);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            User user = _mapper.Map<UserRegisterDto, User>(registerDto);
            _mapper.Map<UserRegisterDto, User>(registerDto);
            await _authService.Register(user, registerDto.Password);
            return Ok();
        }
    }
}
