using Microsoft.AspNetCore.Identity;

namespace Domain.Users
{
    public class User : IdentityUser
    {
        public string FullName
        {
            get
            {
                return FullName;
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                if (value.Length > 50)
                    throw new InvalidDataException($"{nameof(FullName)} is more than 50 character");

                FullName = value;
            }
        }
        public string ImageName { get; set; }
        //Navs
        public ICollection<Token> Tokens { get; set; }
        public DateTime TimeCreate { get; set; }
    }
}
