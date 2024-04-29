using E_Commerce.Data;
using E_Commerce.Helpers;
using E_Commerce.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;





namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        /*
        private readonly ICartRepository cartRepository;
        

        public CartController( ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var cartsWithProductNames = cartRepository.GetAll()
                 .Select(cart => new
                 {
                     cart.Id,
                    // cart.ProductId,
                     CustomerName = cart.Account.UserName,
                     CustomerEmail = cart.Account.Email,
                    // ProductNames = cart.product.Name
                 })
                 .ToList();

            return Ok(cartsWithProductNames);

        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(CartDTO cartDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var cart = new Cart
                {
                   // Quantity = cartDTO.Quantity,
                    //ProductId = cartDTO.Product_Id
                };

                cartRepository.Insert(cart);
                cartRepository.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500, "An error occurred while adding the item to the cart.");
            }
        }

        [HttpPut]
        public ActionResult Edit(int id, CartDTO updatedCart)
        {
            Cart OldCart = cartRepository.GetById(id);
            if (OldCart == null)
            {
                return NotFound();
            }
            //OldCart.Quantity = updatedCart.Quantity;
           // OldCart.ProductId = updatedCart.Product_Id;




            cartRepository.Update(OldCart);
            cartRepository.Save();
            return Ok();
        }



        [HttpDelete("{id}")]
        public ActionResult RemoveFromCart(int id)
        {
            try
            {
                cartRepository.Delete(id);
                cartRepository.Save();
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500);
            }
        }
        */

        AppDbContext context;
        UserManager<AppUser> userManager;

        public CartController(AppDbContext _context,UserManager<AppUser> _userManager)
        {
            context = _context;   
            userManager = _userManager;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var userId = GetUserId();
            //var user = context.Users.FindAsync(userId);
            Cart cart = context.Carts.Include(c=>c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c=>c.UserId == userId);
            if (cart == null) return NotFound();
           
            context.SaveChanges();
            return Ok(cart);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productid)
        {
            var userId = GetUserId();
            var user = await context.Users.FindAsync(userId);

            Product p = context.Products.Find(productid);
           // p.QuantityAvailable -= quantity;
            if (p == null) return NotFound();
            CartItem cartItem = new CartItem() { Product=p,CartID=GetCartId(),Quantity=1,Price=p.Price*1};
            Cart c = context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c => c.UserId == GetUserId());
            c.CartItems.Add(cartItem);
            c.TotalPrice = 0;
            foreach (var item in c.CartItems)
            {
                c.TotalPrice += item.Price;
            }
            context.SaveChanges();
            return Ok(c);
        }

        private int GetCartId()
        {
            Cart c = context.Carts.FirstOrDefault(c=>c.UserId == GetUserId());
            return c != null ? c.Id : 0;
        }
       

        private int GetUserId()
        {
            var userIdClaim = HttpContext.User.FindFirst("id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            else
            {
               throw new InvalidOperationException("Unable to retrieve user ID from the JWT token.");
            }
        }


        

        

        // DELETE: api/cart/remove/{productId}
        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveCartItemFromCart(int productId)
        {
            var userId = GetUserId();
            var user = await context.Users.FindAsync(userId);

            Cart cart = context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c => c.UserId == GetUserId());

            if (user == null || cart == null)
            {
                return NotFound("Shopping cart not found for the user");
            }

            var shoppingCart = cart;
            var cartItemToRemove = shoppingCart.CartItems.FirstOrDefault(ci => ci.ProductID == productId);
            if (cartItemToRemove == null)
            {
                return NotFound("Cart item not found in the shopping cart");
            }

            shoppingCart.CartItems.Remove(cartItemToRemove);

            cart.TotalPrice = 0;
            foreach (var item in cart.CartItems)
            {
                cart.TotalPrice += item.Price;
            }
            await context.SaveChangesAsync();

            return Ok(cart);
        }

        // PUT: api/cart/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateCart(int productid, int quantity)
        {
            var userId = GetUserId();
            var user = await context.Users.FindAsync(userId);

            Cart cart = context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c => c.UserId == GetUserId());

            if (user == null || cart == null)
            {
                return NotFound("Shopping cart not found for the user");
            }

            var shoppingCart = cart;

            var cartItemToUpdate = shoppingCart.CartItems.FirstOrDefault(ci => ci.ProductID == productid);
            if (cartItemToUpdate == null)
            {
                return NotFound("Cart item not found in the shopping cart");
            }

            cartItemToUpdate.Quantity = quantity;
            cartItemToUpdate.Price = cartItemToUpdate.Quantity * cartItemToUpdate.Product.Price;

            cart.TotalPrice = 0;
            foreach (var item in cart.CartItems)
            {
                cart.TotalPrice += item.Price;
            }
            await context.SaveChangesAsync();

            return Ok(cart);
        }

        // DELETE: api/cart/remove-all
        [HttpDelete("remove-all")]
        public async Task<IActionResult> RemoveAllCartItemsFromCart()
        {
            var userId = GetUserId();
            var user = await context.Users.FindAsync(userId);

            Cart cart = context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Product).FirstOrDefault(c => c.UserId == GetUserId());
            if (user == null || cart == null)
            {
                return NotFound("Shopping cart not found for the user");
            }

            var shoppingCart = cart;

            shoppingCart.CartItems.Clear();
            cart.TotalPrice = 0;
            await context.SaveChangesAsync();

            return Ok(cart);
        }


    }
}
