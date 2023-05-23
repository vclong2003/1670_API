using _1670_API.Data;
using _1670_API.Helpers;
using _1670_API.Models;
using _1670_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace _1670_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public AuthenticationController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpPost("register")] // Body: name, email, password
        public async Task<ActionResult> Register(UserDTO userDto)
        {
            var exsistedUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (exsistedUser != null) { return StatusCode(400, "user_existed"); }

            User newUser = new()
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role = "CUSTOMER"
            };

            _dataContext.Add(newUser);

            try { await _dataContext.SaveChangesAsync(); }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return StatusCode(500);
            }

            //success
            Response.Cookies.Append("token", JwtHandler.GenerateToken(newUser));
            return StatusCode(200);
        }

        [HttpPost("login")] // Body: email, password
        public async Task<ActionResult> Login(UserDTO userDto)
        {
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (user == null) { return StatusCode(404, "user_not_found"); }

            bool passwordMatched = BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password);

            if (passwordMatched)
            {
                Response.Cookies.Append("token", JwtHandler.GenerateToken(user));
                return StatusCode(200);
            }

            return StatusCode(400, "validation_error");
        }

        [HttpGet] //get current user
        public ActionResult Auth()
        {
            UserDTO? currentUser = JwtHandler.ValiateToken(Request.HttpContext);

            if (currentUser != null) { return StatusCode(200, currentUser); }

            return StatusCode(401, "validation_fail");
        }
    }


}
