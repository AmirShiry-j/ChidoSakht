using Application.Messagers.SmsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messagers.SmsService
{
    public class FakeSmsService : ISmsService
    {
        public async Task<bool> SendSmsAsync(string ToPhoneNumber, string Message)
        {
            //Send a message...

            return true;
        }
    }
}
