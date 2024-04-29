using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Task.Helpers;

namespace E_Commerce.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(30), MinLength(3)]
        //[UniqueProduct]
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuantityAvailable { get; set; }
        public decimal Price { get; set; }
        public string img { get; set; }

        [ForeignKey("Category")]
        public int CatId { get; set; }

        [JsonIgnore]
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public virtual Category? Category { get; set; }
        //[ForeignKey("Cart")]
        //public int? CartId { get; set; }
        //[JsonIgnore]
        //public Cart? Cart { get; set; }


        //public List<Cart>? Carts { get; set; }

    }
}
