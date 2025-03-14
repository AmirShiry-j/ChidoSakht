using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Account
{
    public class LoginDto
    {
        [Required]
        [RegularExpression("(09)[0-9]{9}")]
        public string PhoneNumber { get; set; }

        [RequiredIfLoginTypeIsPassword]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [EnumDataType(typeof(LoginType), ErrorMessage = "Invalid LoginType value.")]
        public LoginType LoginType { get; set; }
    }
    public enum LoginType
    {
        ByPassword=1,
        ByOTP=2
    }

    public class RequiredIfLoginTypeIsPasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var loginDto = (LoginDto)validationContext.ObjectInstance;

            // اگر نوع لاگین ByPassword باشد، پسورد باید مقدار داشته باشد
            if (loginDto.LoginType == LoginType.ByPassword && string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult("Password is required when LoginType is ByPassword.");
            }

            return ValidationResult.Success;
        }
    }
}
