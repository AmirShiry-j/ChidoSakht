using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Account
{
    public class VerifyPhoneNumberDto
    {
        [Required]
        [RegularExpression("(09)[0-9]{9}")]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(6)]
        [MinLength(6)]
        public string Code { get; set; }
    }
}
