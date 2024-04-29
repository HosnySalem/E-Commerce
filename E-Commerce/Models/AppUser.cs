using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class AppUser : IdentityUser<int>
{
    //public new int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }

    //[ForeignKey("Cart")]
    //public int CartId { get; set; }
    // public  Cart Cart { get; set; }
    public virtual List<Cart> Carts { get; set; } = new List<Cart>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();



    //public AppUser() { }

    //public AppUser(string username, string email) : base(username)
    //{
    //    Email = email;
    //}
}
