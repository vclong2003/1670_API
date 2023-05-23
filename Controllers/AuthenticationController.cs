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

        // POST: /auth/register
        // Registers a new user with the provided information
        // Body: name, email, password
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserDTO userDto)
        {
            // Check if a user with the same email already exists
            var exsistedUser = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (exsistedUser != null) { return StatusCode(400, "user_existed"); }

            // Create a new user entity with the provided details
            User newUser = new()
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role = "CUSTOMER"
            };

            // Add the new user to the db
            _dataContext.Add(newUser);
            try { await _dataContext.SaveChangesAsync(); }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return StatusCode(500);
            }

            // Success: Generate a JWT token for the new user and set it in the response cookie
            Response.Cookies.Append("token", JwtHandler.GenerateToken(newUser));
            return StatusCode(200);
        }

        // POST: /auth/login
        // Logs in a user with the provided email and password
        // Body: email, password
        [HttpPost("login")]
        public async Task<ActionResult> Login(UserDTO userDto)
        {
            // Find the user with the provided email
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

        // GET: /auth
        // Retrieves the current authenticated user
        [HttpGet]
        public ActionResult Auth()
        {
            UserDTO? currentUser = JwtHandler.ValiateToken(Request.HttpContext);

            if (currentUser != null)
            {
                // User is authenticated: Return the current user information
                return StatusCode(200, currentUser);
            }

            // Invalid or no token: Remove the token from the response cookie and return unauthorized status
            Response.Cookies.Delete("token");
            return StatusCode(401, "validation_fail");
        }

        // DELETE: /auth/logout
        // Logs out the current user by removing the token from the response cookie
        [HttpDelete("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return StatusCode(200);
        }

        //test notify
    }
}
