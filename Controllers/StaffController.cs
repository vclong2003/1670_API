using _1670_API.Data;
using _1670_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _1670_API.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public StaffController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpPost]
        public async Task<ActionResult> CreateStaff(AddStaffDTO staffDTO)
        {
            var exsistingAccount = await _dataContext.Accounts.FirstOrDefaultAsync(a => a.Email == staffDTO.Email);
            if (exsistingAccount is null)
            {
                Account staff = new Account()
                {
                    Email = staffDTO.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(staffDTO.Password),
                    Role = "STAFF"
                };
                Staff newStaff = new Staff()
                {
                    Name = staffDTO.Name,
                    Phone = staffDTO.Phone,
                    Address = staffDTO.Address,
                };
                _dataContext.Accounts.Add(staff);
                await _dataContext.SaveChangesAsync();//save the account of the staff
                //get the last id of the staff account record
                int lastRecordId = _dataContext.Accounts.Where(a => a.Role == "STAFF").OrderByDescending(r => r.Id).Select(r => r.Id).FirstOrDefault();
                newStaff.AccountId = lastRecordId;//set staff account id
                _dataContext.Staffs.Add(newStaff);
                await _dataContext.SaveChangesAsync();//save the staff information
                return StatusCode(200, "Add Staff Successfully! Now try to sign in");
            }
            else
            {
                return StatusCode(401, exsistingAccount);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAllStaff()
        {
            var result = await _dataContext.Staffs.ToListAsync();
            return StatusCode(200, result);
        }

        [HttpGet("id")]
        public async Task<ActionResult> GetStaff(int id)
        {
            var result = await _dataContext.Staffs.Where(s => s.AccountId == id)
                .Include(s => s.Account)
                .Select(s => new
                {
                    id = s.Account.Id,
                    name = s.Name,
                    email = s.Account.Email,
                    phone = s.Phone,
                    address = s.Address,

                }).ToListAsync();
            return StatusCode(200, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStaff(StaffDTO staffDTO, int id)
        {
            var staffInfor = await _dataContext.Staffs.FirstOrDefaultAsync(s=>s.AccountId==id);

            staffInfor.Name = staffDTO.Name;
            staffInfor.Phone = staffDTO.Phone;
            staffInfor.Address = staffDTO.Address;

            await _dataContext.SaveChangesAsync();

            return StatusCode(200, "Update Successfully");
        }
    }
}
