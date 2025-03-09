using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ConfigService
{
    public interface IConfigService
    {
        AppSettings Config { get; }
    }

    public class AppSettings
    {
        public DatabaseSettings DatabaseSettings { get; set; }
        public IdentitySettings IdentitySettings { get; set; }  
        public EmailSetting EmailSetting { get; set; }
        public string[] CorsOrigins { get; set; }
        public Localization Localization { get; set; }
    }
    public class DatabaseSettings
    {
        public string Provider { get; set; }
        public string ConnectionString { get; set; }
    }
    public class EmailSetting
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public bool EnableSsl { get; set; }
        public int Timeout { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string SecureSocketOptions { get; set; }
    }
    public class Localization
    {
        public string DefaultCulture { get; set; } = "fa";
        public string[] SupportedCultures { get; set; } = { "fa" };
        public bool Visible_AcceptLanguageForSwagger { get; set; } = false;
    }
    public class IdentitySettings
    {
        public bool RequireDigit { get; set; }
        public int RequiredLength { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool RequireNonAlphanumeric { get; set; }
        public int RequiredUniqueChars { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
        public int DefaultLockoutTimeSpan    { get; set; }
    }
}
