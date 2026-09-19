using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lab3.Validation
{
    public class NoNumbersAttribute : ValidationAttribute, IClientModelValidator
    {
        public NoNumbersAttribute()
        {
            ErrorMessage = "Name must not contain any numbers.";
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true; // [Required] handles empty values separately
            string name = value.ToString() ?? string.Empty;
            return !Regex.IsMatch(name, @"\d");
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-nonumbers", ErrorMessage ?? "Name must not contain any numbers.");
        }

        private static void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, value);
            }
        }
    }
}