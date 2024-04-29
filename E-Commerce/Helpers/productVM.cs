using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Helpers
{
    public class productVM
    {
        [Required, MaxLength(30), MinLength(3)]
        //[UniqueProduct]
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuantityAvailable { get; set; }
        public decimal Price { get; set; }
        public string CatId { get; set; }
    }
}
