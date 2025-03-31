using Microsoft.AspNetCore.Identity;

namespace Domain.Users
{
    public class Role : IdentityRole
    {
        private string _Description = "";
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));

                _Description = value;
            }
        }
        public DateTime TimeCreate { get; set; }
    }
}
