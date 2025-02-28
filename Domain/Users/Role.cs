using Microsoft.AspNetCore.Identity;

namespace Domain.Users
{
    public class Role : IdentityRole
    {
        public string Description
        {
            get
            {
                return Description;
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));

                Description = value;
            }
        }
        public DateTime TimeCreate { get; set; }
    }
}
