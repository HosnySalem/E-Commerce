using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Task.Helpers;

namespace E_Commerce.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required,MaxLength(30),MinLength(3)]
        [UniqueCategory]
        public string Name { get; set; }    
        public string Description { get; set; }
        [JsonIgnore]

        public virtual ICollection<Product>? Products { get; set; }
    }
}
