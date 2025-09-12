using System.ComponentModel.DataAnnotations;

namespace WebApi.ModelsAndDtoes.Address
{
    public class CreateAddressApiDto
    {
        [Required]
        public string Name { get; set; }
    }
    public class UpdateAddressApiDto
    {
        [Required]
        public int AddressId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
