using _1670_API.Data;
using _1670_API.Helpers;
using _1670_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace _1670_API.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public DashboardController(DataContext context)
        {
            _dataContext = context;
        }

        [HttpGet]
        public async Task<ActionResult> Dashboard()
        {
            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null) { return StatusCode(401, "Unauthorized"); }

            if (accountDTO.Role == "STAFF")
            {
                List<object> results = new List<object>();
                _ExecuteStatisticNumber(results, "Delivering");
                return StatusCode(200, results);
            }
            else
            {

                return StatusCode(401, "Unauthorized");
            }
        }

        private void _ExecuteStatisticNumber(List<object> result, string status)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=tcp:1670.database.windows.net,1433;Initial Catalog=1670_db;Persist Security Info=False;User ID=admin123;Password=Admin1670;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            SqlCommand cmd = new SqlCommand("PRO_TotalRevenue", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@status", status);
            conn.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                var r = new
                {
                    month = sdr.GetValue(0).ToString(),
                    revenue = sdr.GetValue(1).ToString(),
                    quantity = sdr.GetValue(2).ToString()
                };
                result.Add(r);
            }
            conn.Close();
        }
    }
}
