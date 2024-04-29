using E_Commerce.Data;
using E_Commerce.Helpers;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Repository
{
    public class CartRepository : ICartRepository
    {
        AppDbContext context;

        // inject Context
        public CartRepository(AppDbContext _context)//ask context not create 
        {
            context = _context;
        }
        public List<Cart> GetAll()
        {
            
            return context.Carts
                 .Include(c => c.CartItems)
                 .Include(c => c.User)
                 .Where(c => !c.IsDeleted)
                 .ToList();
        }

        public Cart GetById(int id)
        {
            return context.Carts
                 .Include(c => c.CartItems)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }

        public List<Cart> GetCartItemsOfCustomer(int customerId)
        {

            return GetAll().Where(items => items.UserId == customerId).ToList();
        }
        public int GetTotalPrice(int customerId)
        {
            int totalPrice = 0;
            foreach (Cart cartItem in GetCartItemsOfCustomer(customerId))
            {
                //foreach (CartItem item in cartItem.CartItems)
                //    totalPrice += ((int)product?.Price * cartItem.Quantity);

            }

            return totalPrice;
        }

        public void Insert(Cart obj)
        {
            context.Add(obj);
        }
        public void Update(Cart obj)
        {
            context.Update(obj);
        }

        public void Delete(int id)
        {
            Cart crs = GetById(id);
            crs.IsDeleted = true;
            Update(crs);


        }

        public void Save()
        {
            context.SaveChanges();
        }

        public int GetTotalPrice(string customerId)
        {
            throw new NotImplementedException();
        }

        public List<Cart> GetCartItemsOfCustomer(string customerId)
        {
            throw new NotImplementedException();
        }
    }
}
