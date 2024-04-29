using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Commerce.Models
{
    public class Cart
    {
        public int Id { get; set; }
        //public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        public decimal TotalPrice { get; set; } // Calculated property


        // Change Customer_Id to string
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual List<CartItem>? CartItems { get; set; } = new List<CartItem>();
        [JsonIgnore]
        public virtual AppUser User { get; set; }

        //[ForeignKey("product")]
        //public int ProductId { get; set; }
        //public Product? product { get; set; }
    }
}
