using System.Text.Json.Serialization;

namespace _1670_API.Models.DTOs
{
    public class OrderDTO
    {
        public string Phone { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public int userID { get; set; }
        public int? storeID { get; set; }
    }
}
