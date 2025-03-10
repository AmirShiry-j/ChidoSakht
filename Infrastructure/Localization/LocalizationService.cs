using Application.Interfaces.Localization;
using Infrastructure.Localization.AccountMessages;
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
        private readonly IStringLocalizer<MessagesAccount> _localizer;

        public LocalizationService(IStringLocalizer<MessagesAccount> localizer)
        {
            _localizer = localizer;
        }

        public string GetMessageAccount(string key)
        {
            return MessagesAccount.ResourceManager.GetString(key);
        }
        public string GetMessageIdentity(string key)
        {
            return MessagesIdentity.ResourceManager.GetString(key);
        }
    }
}
