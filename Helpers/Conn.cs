using System.Data.SqlClient;

namespace _1670_API.Helpers
{
    public class Conn
    {
        public static SqlConnection Connection()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=tcp:1670.database.windows.net,1433;Initial Catalog=1670_db;Persist Security Info=False;User ID=admin123;Password=Admin1670;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            return conn;
        }
    }
}
