using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateTime dateValue = Convert.ToDateTime(value);
            return dateValue >= DateTime.UtcNow;
        }
    }
    
}
