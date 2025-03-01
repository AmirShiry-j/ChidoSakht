using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messagers.SmsService
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(string ToPhoneNumber, string Message);
    }
}
