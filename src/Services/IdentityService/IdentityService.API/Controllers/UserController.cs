using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers
{
    public class UserController : Controller
    {
        [HttpGet("GetIndex")]
        public IActionResult Index()
        {
            return Ok("Identity Service is running!");
        }
    }
}
