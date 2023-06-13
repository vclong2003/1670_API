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
            if (exsistingAccount != null) { return StatusCode(400, "Email already in use!"); } // Check if an account with the same email already exists

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
            if (account == null) { return StatusCode(404, "Aaccount not found!"); }

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

            return StatusCode(400, "Wrong password!");
        }

        // GET: /auth
        // Retrieves the authenticated account information
        [HttpGet]
        public ActionResult Auth()
        {
            AccountDTO? account = JwtHandler.ValiateToken(Request.HttpContext);

            if (account != null) { return StatusCode(200, account); }

            return StatusCode(401, "validation-fail");
        }


        // DELETE: /auth/logout
        // Log out (clear token)
        [HttpDelete("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("token", new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            });
            return StatusCode(200);
        }

        // PUT: /auth/update-password
        // body: oldPassword, newPassword
        [HttpPut("update-password")]
        public async Task<ActionResult> ChangePassword(UpdatePasswordDTO updatePasswordDTO)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null)
            {
                return StatusCode(401, "Unauthorized!");
            }

            var account = _dataContext.Accounts.Where(acc => acc.Id == accountDTO.Id).FirstOrDefault();
            bool passwordMatched = BCrypt.Net.BCrypt.Verify(updatePasswordDTO.OldPassword, account.Password);

            if (passwordMatched)
            {
                account.Password = BCrypt.Net.BCrypt.HashPassword(updatePasswordDTO.NewPassword);
                await _dataContext.SaveChangesAsync();

                return StatusCode(200);
            }

            return StatusCode(400, "Wrong password!");
        }
    }
}
