using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountsController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/Accounts
        [HttpGet]
        [Authorize] 
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAccount()
        {
            return await _context.Users.Include(p => p.Orders).ToListAsync();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        [Authorize] 
        public async Task<ActionResult<AppUser>> GetAccount(int id)
        {
            var account = await _context.Users.Include(p => p.Orders).FirstOrDefaultAsync(d => d.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

     
        [HttpPut("{id}")]
        [Authorize] 
        public async Task<IActionResult> PutAccount(int id, AppUser account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Create")]
        public async Task<IActionResult> CreateAccount(CreateAccountModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = "DefaultFirstName",
                    LastName = "DefaultLastName",
                    Address = model.Address
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                Cart cart = new Cart() { UserId = Convert.ToInt32(user.Id), CreatedAt = DateTime.Now, IsDeleted = false, TotalPrice = 0 };
                _context.Carts.Add(cart);
                _context.SaveChanges();
                _context.SaveChanges();

                //await transaction.CommitAsync();

                return Ok(new { message = "Account created successfully" });
                }

                return BadRequest(result.Errors);
            }
        

        // POST: api/Accounts/Login
        [HttpPost]
        [AllowAnonymous] 
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
              
                return BadRequest(new { message = "Invalid email or password." });
            }

            // Sign in the user
            await _signInManager.SignInAsync(user, isPersistent: false);

            #region Hosny Code:
            List<Claim> userData = new List<Claim>()
            {
                new Claim("name", user.FirstName),
                new Claim("id",user.Id.ToString()),
            };
            userData.Add(new Claim("name", "admin"));
            userData.Add(new Claim(ClaimTypes.MobilePhone, "01141402370"));

            /////////////////
            ///
            //string key = "Welcome to the accoumt of hosny mohamed salem";
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Welcome to the accoumt of hosny mohamed salem"));

            var signcer = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                claims: userData,
                signingCredentials: signcer,
                expires: DateTime.Now.AddDays(1));

            var SecurityToken = new JwtSecurityTokenHandler().WriteToken(Token);
            #endregion

            return Ok(new { message = "Login successful", token = SecurityToken });

           // return Ok(new { message = "Login successful" });
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        [Authorize] 
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Users.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Users.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
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
    }
}
