using Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Address
{
    public class CreateAddressApiDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public string FullAddress { get; set; }
        [Required]
        public string Pelak { get; set; }
        [Required]
        [RegularExpression(@"^\d+$")]
        public string PostalCode { get; set; }
        public string? UnitNumber { get; set; }
        [Required]
        [Range(1, 2)]
        public WhoRecept WhoRecept { get; set; }
        [RequiredIfWhoOther(WhoRecept.Other)]
        public string? NameRecipient { get; set; }
        [RequiredIfWhoOther(WhoRecept.Other)]
        [RegularExpression("(09)[0-9]{9}")]
        public string? PhoneNumberRecipient { get; set; }
    }

    public class RequiredIfWhoOtherAttribute : ValidationAttribute
    {
        private readonly WhoRecept _targetValue;

        public RequiredIfWhoOtherAttribute(WhoRecept targetValue)
        {
            _targetValue = targetValue;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var instance = (CreateAddressApiDto)validationContext.ObjectInstance;

            if (instance.WhoRecept == _targetValue && string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult($"{validationContext.MemberName} الزامی است وقتی WhoRecept = {_targetValue}");
            }

            return ValidationResult.Success;
        }

        public class UpdateAddressApiDto
        {
            [Required]
            public int AddressId { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public int CityId { get; set; }
            [Required]
            public string FullAddress { get; set; }
            [Required]
            public string Pelak { get; set; }
            [Required]
            [RegularExpression(@"^\d+$")]
            public string PostalCode { get; set; }
            public string? UnitNumber { get; set; }
            [Required]
            [Range(1, 2)]
            public WhoRecept WhoRecept { get; set; }
            [RequiredIfWhoOther2(WhoRecept.Other)]
            public string? NameRecipient { get; set; }
            [RequiredIfWhoOther2(WhoRecept.Other)]
            [RegularExpression("(09)[0-9]{9}")]
            public string? PhoneNumberRecipient { get; set; }
        }

        public class RequiredIfWhoOther2Attribute : ValidationAttribute
        {
            private readonly WhoRecept _targetValue;

            public RequiredIfWhoOther2Attribute(WhoRecept targetValue)
            {
                _targetValue = targetValue;
            }

            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                var instance = (UpdateAddressApiDto)validationContext.ObjectInstance;

                if (instance.WhoRecept == _targetValue && string.IsNullOrWhiteSpace(value?.ToString()))
                {
                    return new ValidationResult($"{validationContext.MemberName} الزامی است وقتی WhoRecept = {_targetValue}");
                }

                return ValidationResult.Success;
            }
        }
    }
}

