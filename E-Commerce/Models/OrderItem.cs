using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Commerce.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [JsonIgnore]

        // Navigation property for one-to-many relationship with Order
        public Order Order { get; set; }

        // Navigation property for one-to-one relationship with Product
        public Product Product { get; set; }
    }
}
