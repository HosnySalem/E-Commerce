using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Commerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [ForeignKey("Cart")]
        public int CartID { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        // Navigation property for one-to-one relationship with Product
        public Product Product { get; set; }

        [JsonIgnore]

        // Navigation property for one-to-many relationship with ShoppingCart
        public virtual Cart Cart { get; set; }

    }
}
