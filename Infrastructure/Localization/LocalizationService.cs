using Application.Interfaces.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public LocalizationService(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;
        }

        public string GetMessage(string key)
        {
            return Messages.ResourceManager.GetString(key);
        }
        public string GetMessageIdentity(string key)
        {
            return MessagesIdentity.ResourceManager.GetString(key);
        }
    }
}
