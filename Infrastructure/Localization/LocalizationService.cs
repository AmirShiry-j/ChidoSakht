using Application.Interfaces.Localization;
using Infrastructure.Localization.AccountMessages;
using Infrastructure.Localization.CategoryMessages;
using Infrastructure.Localization.PermissionMessages;
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
        public string GetMessageAccount(string key)
        {
            return MessagesAccount.ResourceManager.GetString(key);
        }
        public string GetMessageIdentity(string key)
        {
            return MessagesIdentity.ResourceManager.GetString(key);
        }
        public string GetMessageCategory(string key)
        {
            return MessagesCategory.ResourceManager.GetString(key);
        }

        public string GetMessagePermission(string key)
        {
            return MessagesPermission.ResourceManager.GetString(key);
        }
    }
}
