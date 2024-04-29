using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Data;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using E_Commerce.Helpers;
using System.Net.Http.Headers;
using System.IO;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize] 
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.Include(e => e.Category).ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize] 
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(e => e.Category).FirstOrDefaultAsync(e => e.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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


        [HttpPost, DisableRequestSizeLimit]
        [Authorize]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            //if (image != null && image.Length > 0)
            //{
            //    var fileName = Path.GetFileNameWithoutExtension(image.FileName) +
            //                   Path.GetExtension(image.FileName).ToLowerInvariant();
            //    product.img = Path.Combine("Images", fileName); // Adjust path as needed

            //    using (var fileStream = new FileStream(Path.Combine("wwwroot", product.img), FileMode.Create))
            //    {
            //        await image.CopyToAsync(fileStream);
            //    }
            //}
            
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        //[HttpPost, DisableRequestSizeLimit]
        //public async Task<IActionResult> Upload()
        //{
        //    try
        //    {
        //        var formCollection = await Request.ReadFormAsync();
        //        var file = formCollection.Files.First();
        //        var folderName = Path.Combine("Resources", "Images");
        //        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        //        if (file.Length > 0)
        //        {
        //            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //            var fullPath = Path.Combine(pathToSave, fileName);
        //            var dbPath = Path.Combine(folderName, fileName);
        //            using (var stream = new FileStream(fullPath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }
        //            return Ok(new { dbPath });
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex}");
        //    }
        //}

       

        //// POST: api/Products
        //[HttpPost, DisableRequestSizeLimit]
        //public async Task<IActionResult> CreateProduct([FromForm] productVM product, IFormFile image)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    //Category c = _context.Categories.Find(product.CatId);
        //    Product p1 = new Product() { Name = product.Name,Description = product.Description, CatId = int.Parse(product.CatId), Price = product.Price, QuantityAvailable = product.QuantityAvailable };

//            if (image != null && image.Length > 0)
//            {
//                var fileName = Path.GetFileNameWithoutExtension(image.FileName) +
//                               Path.GetExtension(image.FileName).ToLowerInvariant();
//        p1.img = Path.Combine("Images", fileName); // Adjust path as needed

//                using (var fileStream = new FileStream(Path.Combine("wwwroot", p1.img), FileMode.Create))
//                {
//                    await image.CopyToAsync(fileStream);
//    }
//}

//   // await _productRepository.AddProductAsync(product);
//    await _context.Products.AddRangeAsync(p1);
//    _context.SaveChanges();

//    return CreatedAtAction("GetProduct", new { id = p1.Id }, product);
//}

//// PUT: api/Products/5
//[HttpPut("{id}")]
//public async Task<IActionResult> UpdateProduct(int id, [FromForm] Product product, IFormFile image)
//{
//    if (!ModelState.IsValid)
//    {
//        return BadRequest(ModelState);
//    }

//    if (id != product.Id)
//    {
//        return BadRequest();
//    }

//    var existingProduct = await GetProduct(id);
//    if (existingProduct == null)
//    {
//        return NotFound();
//    }

//    if (image != null && image.Length > 0)
//    {
//        // Handle existing image deletion (optional)
//        if (!string.IsNullOrEmpty(existingProduct.Value?.img))
//        {
//            System.IO.File.Delete(Path.Combine("wwwroot", existingProduct.Value.img));
//        }

//        var fileName = Path.GetFileNameWithoutExtension(image.FileName) +
//                       Path.GetExtension(image.FileName).ToLowerInvariant();
//        product.img = Path.Combine("Images", fileName); // Adjust path as needed

//        using (var fileStream = new FileStream(Path.Combine("wwwroot", product.img), FileMode.Create))
//        {
//            await image.CopyToAsync(fileStream);
//        }
//    }
//    else
//    {
//        product.img = existingProduct.Value.img; // Maintain existing image path
//    }

//     _context.Products.Update(product);
//    _context.SaveChanges();

//    return NoContent();
//}

[HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
