namespace DataEntryOverlay.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Car
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Color is required.")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Color can only contain alphabetic characters.")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(1900, 2100, ErrorMessage = "Year must be between 1900 and 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        [StringLength(50, ErrorMessage = "Model cannot exceed 50 characters.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "ReleaseDate is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [ReleaseDateValidation(ErrorMessage = "ReleaseDate must be a past or present date.")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "LastAvailableDate is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [LastAvailableDateValidation("ReleaseDate", ErrorMessage = "LastAvailableDate must be on or after the ReleaseDate.")]
        public DateTime LastAvailableDate { get; set; }

        [Required(ErrorMessage = "Total is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total must be greater than 0.")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Owner is required.")]
        [StringLength(100, ErrorMessage = "Owner name cannot exceed 100 characters.")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [RegularExpression("^(Available|Unavailable|Discontinued)$", ErrorMessage = "Status must be 'Available', 'Unavailable', or 'Discontinued'.")]
        public string Status { get; set; }
    }

    public class ReleaseDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid (object value, ValidationContext validationContext)
        {
            if (value is DateTime date && date > DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }

    public class LastAvailableDateValidationAttribute : ValidationAttribute
    {
        private readonly string _releaseDatePropertyName;

        public LastAvailableDateValidationAttribute (string releaseDatePropertyName)
        {
            _releaseDatePropertyName = releaseDatePropertyName;
        }

        protected override ValidationResult IsValid (object value, ValidationContext validationContext)
        {
            var lastAvailableDate = value as DateTime?;
            var releaseDateProperty = validationContext.ObjectType.GetProperty(_releaseDatePropertyName);

            if (releaseDateProperty == null)
            {
                return new ValidationResult($"Unknown property: {_releaseDatePropertyName}");
            }

            var releaseDate = releaseDateProperty.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (lastAvailableDate.HasValue && releaseDate.HasValue && lastAvailableDate < releaseDate)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

}
