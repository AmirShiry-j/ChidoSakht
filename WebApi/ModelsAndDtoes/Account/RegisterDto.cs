using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Account
{
    
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression("(09)[0-9]{9}")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(4)]
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }
    }
}
