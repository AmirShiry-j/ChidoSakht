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
        public EmailSetting EmailSetting { get; set; }
        public string[] CorsOrigins { get; set; }
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
}
