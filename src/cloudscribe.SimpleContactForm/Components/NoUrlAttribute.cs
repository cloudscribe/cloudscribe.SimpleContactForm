using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class NoUrlAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value != null)
        {
            var stringToValidate = value.ToString();
            var urlPattern = @"((http|https|ftp|file)://)?([\w-]+(\.[\w-]+)+)(/[\w- ./?%&=]*)?";
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (Regex.IsMatch(stringToValidate, urlPattern) && !Regex.IsMatch(stringToValidate, emailPattern, RegexOptions.IgnoreCase))
            {
                return new ValidationResult("URLs are not allowed.");
            }
        }

        return ValidationResult.Success;
    }
}