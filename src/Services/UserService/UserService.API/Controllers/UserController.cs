﻿using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Controllers
{
    public class UserController : Controller
    {
        [HttpGet("GetIndex")]
        public IActionResult Index()
        {
            return Ok("User Service is running!");
        }
    }
}
