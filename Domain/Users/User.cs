using Microsoft.AspNetCore.Identity;

namespace Domain.Users
{
    public class User : IdentityUser
    {
        public override string? Email { get; set; }
        public override string PhoneNumber { get; set; }
        private string _FullName;
        public string FullName
        {
            get
            {
                return _FullName;
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                if (value.Length > 50)
                    throw new InvalidDataException($"{nameof(FullName)} is more than 50 character");

                _FullName = value;
            }
        }
        public string ImageName { get; set; }
        //Navs
        public ICollection<Token> Tokens { get; set; }
        public DateTime TimeCreate { get; set; }
    }
}
