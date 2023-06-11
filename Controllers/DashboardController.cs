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
            StatisticDTO statistic = new StatisticDTO();

            AccountDTO accountDTO = JwtHandler.ValiateToken(Request.HttpContext);
            if (accountDTO == null || accountDTO.Role == "CUSTOMER") { return StatusCode(401, "Unauthorized"); }
            if (accountDTO.Role == "STAFF")
            {
                _ExecuteStatisticNumber(statistic);
                return StatusCode(200, new
                {
                    revenue = statistic.revenue,
                    orders = statistic.orders,
                    users = statistic.users,
                });
            }
            else
            {
                return StatusCode(401, "Unauthorized");
            }
        }

        private void _ExecuteStatisticNumber(StatisticDTO statistic)
        {
            try
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = "Server=tcp:1670.database.windows.net,1433;Initial Catalog=1670_db;Persist Security Info=False;User ID=admin123;Password=Admin1670;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                SqlCommand cmdRevenue = new SqlCommand("PRO_TotalRevenue", conn);
                cmdRevenue.CommandType = CommandType.StoredProcedure;
                cmdRevenue.Parameters.AddWithValue("@status", "Delivering");
                conn.Open();

                SqlDataReader cmdRdRevenue = cmdRevenue.ExecuteReader();
                while (cmdRdRevenue.Read())
                {
                    var r = new
                    {
                        month = cmdRdRevenue.GetValue(0).ToString(),
                        revenue = cmdRdRevenue.GetValue(1).ToString(),
                        quantity = cmdRdRevenue.GetValue(2).ToString()
                    };
                    statistic.revenue.Add(r);
                }
                conn.Close();

                conn.Open();
                SqlCommand cmdOrders = new SqlCommand("EXEC PRO_OrderCalculating", conn);
                SqlDataReader cmdRdorders = cmdOrders.ExecuteReader();

                while (cmdRdorders.Read())
                {
                    var r = new
                    {
                        status = cmdRdorders.GetValue(0).ToString(),
                        quantity = cmdRdorders.GetValue(1).ToString()
                    };
                    statistic.orders.Add(r);
                }

                conn.Close();
                conn.Open();
                SqlCommand cmdUser = new SqlCommand("EXEC PRO_UserCalculating", conn);
                SqlDataReader cmdRdUser = cmdUser.ExecuteReader();
                while (cmdRdUser.Read())
                {
                    var r = new
                    {
                        role = cmdRdUser.GetValue(0).ToString(),
                        quantity = cmdRdUser.GetValue(1).ToString()
                    };
                    statistic.users.Add(r);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                statistic.revenue = null;
                statistic.orders = null;
                statistic.users = null;
            }
        }
    }
}
