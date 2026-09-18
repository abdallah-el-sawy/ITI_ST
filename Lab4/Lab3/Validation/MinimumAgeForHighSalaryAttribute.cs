using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lab3.Validation
{
    public class MinimumAgeForHighSalaryAttribute : ValidationAttribute, IClientModelValidator
    {
        private readonly string _salaryPropertyName;
        private readonly decimal _salaryThreshold;
        private readonly int _minimumAge;

        public MinimumAgeForHighSalaryAttribute(string salaryPropertyName, double salaryThreshold, int minimumAge)
        {
            _salaryPropertyName = salaryPropertyName;
            _salaryThreshold = (decimal)salaryThreshold;
            _minimumAge = minimumAge;
            ErrorMessage = $"Employees with a salary above {salaryThreshold:N0} must be at least {minimumAge} years old.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var salaryProperty = validationContext.ObjectType.GetProperty(_salaryPropertyName);
            if (salaryProperty == null)
            {
                return new ValidationResult($"Unknown property: {_salaryPropertyName}");
            }

            var salaryValue = salaryProperty.GetValue(validationContext.ObjectInstance);
            if (salaryValue == null || value == null) return ValidationResult.Success;

            decimal salary = Convert.ToDecimal(salaryValue);
            int age = Convert.ToInt32(value);

            if (salary > _salaryThreshold && age < _minimumAge)
            {
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName ?? "Age" });
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-minimumageforhighsalary", ErrorMessage ?? string.Empty);
            MergeAttribute(context.Attributes, "data-val-minimumageforhighsalary-salaryfield", _salaryPropertyName);
            MergeAttribute(context.Attributes, "data-val-minimumageforhighsalary-threshold", _salaryThreshold.ToString());
            MergeAttribute(context.Attributes, "data-val-minimumageforhighsalary-minage", _minimumAge.ToString());
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