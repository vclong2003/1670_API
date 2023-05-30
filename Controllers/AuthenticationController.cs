using _1670_API.Data;
using _1670_API.Helpers;
using _1670_API.Models;
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
        // Body: email, password
        [HttpPost("register")]
        public async Task<ActionResult> Register(AccountDTO accountDTO)
        {
            var exsistingAccount = await _dataContext.Accounts.FirstOrDefaultAsync(a => a.Email == accountDTO.Email);
            if (exsistingAccount != null) { return StatusCode(400, "user-existed"); } // Check if an account with the same email already exists

            Account newAccount = new()
            {
                Email = accountDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(accountDTO.Password),
                Role = "CUSTOMER"
            };

            _dataContext.Add(newAccount);
            try { await _dataContext.SaveChangesAsync(); }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return StatusCode(500);
            }

            // Generate a JWT token and append it to the response cookies
            Response.Cookies.Append("token", JwtHandler.GenerateToken(newAccount), new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            });

            return StatusCode(200);
        }

        // POST: /auth/login
        // Authenticates a user with the provided credentials
        // Body: email, password
        [HttpPost("login")]
        public async Task<ActionResult> Login(AccountDTO accountDTO)
        {

            var account = await _dataContext.Accounts.FirstOrDefaultAsync(a => a.Email == accountDTO.Email);
            if (account == null) { return StatusCode(404, "user-not-found"); }

            bool passwordMatched = BCrypt.Net.BCrypt.Verify(accountDTO.Password, account.Password);

            if (passwordMatched)
            {

                Response.Cookies.Append("token", JwtHandler.GenerateToken(account), new CookieOptions
                {
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None
                });
                return StatusCode(200);
            }

            return StatusCode(400, "validation-error");
        }

        // GET: /auth
        // Retrieves the authenticated account information
        [HttpGet]
        public ActionResult Auth()
        {
            AccountDTO? account = JwtHandler.ValiateToken(Request.HttpContext);

            if (account != null) { return StatusCode(200, account); }

            Response.Cookies.Delete("token");

            return StatusCode(401, "validation-fail");
        }

        [HttpDelete("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("token");
            return StatusCode(200);
        }
    }
}
