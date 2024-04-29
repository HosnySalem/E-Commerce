using E_Commerce.Data;
using E_Commerce.Models;
using System.ComponentModel.DataAnnotations;


namespace Task.Helpers
{
    public class UniqueProduct : ValidationAttribute
    {
        AppDbContext? db { get; set; }
        public UniqueProduct()
        {
            
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            db = validationContext.GetService<AppDbContext>();
            var productName = value.ToString();

            var existingProduct = db.Products.FirstOrDefault(c => c.Name == productName);

            if (existingProduct != null)
            {
                return new ValidationResult("Product name already exists.");
            }

            return ValidationResult.Success;
        }
    }
}
