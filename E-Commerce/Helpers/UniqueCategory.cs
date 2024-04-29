using E_Commerce.Data;
using System.ComponentModel.DataAnnotations;


namespace Task.Helpers
{
    public class UniqueCategory : ValidationAttribute
    {
        AppDbContext? db { get; set; }
        public UniqueCategory()
        {
            
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            db = validationContext.GetService<AppDbContext>();
            var categoryName = value.ToString();

            var existingCategory = db.Categories.FirstOrDefault(c => c.Name == categoryName);

            if (existingCategory != null)
            {
                return new ValidationResult("Category name already exists.");
            }

            return ValidationResult.Success;
        }
    }
}
