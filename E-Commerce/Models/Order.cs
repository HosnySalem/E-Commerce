using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Task.Helpers;

namespace E_Commerce.Models
{
    [Authorize]
    public class Order
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }
        public Status Status { get; set; } 

        [FutureDate]
        public DateOnly OrderDate { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }


        [JsonIgnore]
        public AppUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; }= new List<OrderItem>();
    }

   public enum Status
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }
}
