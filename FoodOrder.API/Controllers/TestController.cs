using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public TestController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult GetJsonFile()
        {
            try
            {
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "food.json");

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var json = System.IO.File.ReadAllText(filePath);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
