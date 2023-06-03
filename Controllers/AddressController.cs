using _1670_API.Data;
using _1670_API.Helpers;
using _1670_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1670_API.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AddressController(DataContext context)
        {
            _dataContext = context;
        }

        // GET: /api/addresses
        [HttpGet]
        public async Task<ActionResult> GetAllAddresses()
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }
            if (accountDTO.Role != "CUSTOMER") { return StatusCode(403, "Forbidden"); }

            var addresses = await _dataContext.ShippingAddresses.Where(s => s.CustomerId == accountDTO.Id).ToListAsync();

            return StatusCode(200, addresses);

        }

        // POST: /api/addresses
        // Body parameters: name, phone, address, city, country
        [HttpPost]
        public async Task<ActionResult> AddAddress(ShippingAddressDTO addressDTO)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }
            if (accountDTO.Role != "CUSTOMER") { return StatusCode(403, "Forbidden"); }

            ShippingAddress newAddress = new()
            {
                CustomerId = (int)accountDTO.Id,
                Name = addressDTO.Name,
                Phone = addressDTO.Phone,
                Address = addressDTO.Address,
                City = addressDTO.City,
                Country = addressDTO.Country
            };

            _dataContext.ShippingAddresses.Add(newAddress);
            await _dataContext.SaveChangesAsync();

            return StatusCode(200, newAddress);
        }

        // PUT: /api/addresses/{id}
        // Body parameters: name, phone, address, city, country
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, ShippingAddressDTO addressDTO)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }
            if (accountDTO.Role != "CUSTOMER") { return StatusCode(403, "Forbidden"); }

            var shippingAddress = await _dataContext.ShippingAddresses.Where(s => s.Id == id && s.CustomerId == accountDTO.Id).FirstOrDefaultAsync();
            if (shippingAddress == null) { return StatusCode(404, "Not Found"); }

            shippingAddress.Name = addressDTO.Name ?? shippingAddress.Name;
            shippingAddress.Phone = addressDTO.Phone ?? shippingAddress.Phone;
            shippingAddress.Address = addressDTO.Address ?? shippingAddress.Address;
            shippingAddress.City = addressDTO.City ?? shippingAddress.City;
            shippingAddress.Country = addressDTO.Country ?? shippingAddress.Country;

            await _dataContext.SaveChangesAsync();

            return StatusCode(200);
        }

        // DELETE: /api/addresses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }
            if (accountDTO.Role != "CUSTOMER") { return StatusCode(403, "Forbidden"); }

            var shippingAddress = await _dataContext.ShippingAddresses.Where(s => s.Id == id && s.CustomerId == accountDTO.Id).FirstOrDefaultAsync();
            if (shippingAddress == null) { return StatusCode(404, "Not Found"); }

            _dataContext.ShippingAddresses.Remove(shippingAddress);
            await _dataContext.SaveChangesAsync();

            return StatusCode(200);
        }
    }
}
