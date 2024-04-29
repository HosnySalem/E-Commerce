using System.ComponentModel.DataAnnotations;

namespace Task.Helpers
{
    public class PastDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var date = (DateOnly)value;

            if (date >= DateOnly.FromDateTime(DateTime.Today))
            {
                return false;
            }

            return true;
        }
    }
}
