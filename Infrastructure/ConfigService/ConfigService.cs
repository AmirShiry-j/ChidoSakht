using Application.ConfigService;
using Microsoft.Extensions.Configuration;


namespace Infrastructure.ConfigService
{
    public sealed class ConfigService : IConfigService
    {
        private readonly AppSettings _appConfig;
        public ConfigService(IConfiguration configuration)
        {
            _appConfig = configuration.Get<AppSettings>();
        }
        public AppSettings Config { get { return _appConfig; } }
    }
}
