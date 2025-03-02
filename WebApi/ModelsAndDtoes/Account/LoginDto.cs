using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Account
{
    public class LoginDto
    {
        [Required]
        [RegularExpression("(09)[0-9]{9}")]
        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
