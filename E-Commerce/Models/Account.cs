using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Task.Helpers;

namespace E_Commerce.Models
{
    public class cccount
    {
        public int Id { get; set; }


        [Required,StringLength(maximumLength:50,MinimumLength =3)]
        public string UserName { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Compare("Password")]
        public string PasswordConfirmed { get; set; }

        public string Type { get; set; }

        [PastDate]
        public DateOnly CreatedOn { get; set;}
        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public Cart Cart { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
