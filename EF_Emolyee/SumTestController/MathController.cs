using Microsoft.AspNetCore.Mvc;

namespace MyWebApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MathController : ControllerBase
    {
        [HttpGet("Sum")]
        public IActionResult Sum(int a, int b)
        {
            int result = a + b;
            return Ok(result);
        }
    }
}
