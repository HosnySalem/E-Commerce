using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Task.Helpers
{
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateOnly date = (DateOnly)value;
            if (date <= DateOnly.FromDateTime(DateTime.Today))
            {
                return false;
            }
            return true;

        }
    }
}
