using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Interfaces.Localization
{
    public interface ILocalizationService
    {
        string GetMessageAccount(string key);
        string GetMessageIdentity(string key);
        string GetMessageCategory(string key);
        string GetMessagePermission(string key);
        string GetMessageProduct(string key);
    }
}
