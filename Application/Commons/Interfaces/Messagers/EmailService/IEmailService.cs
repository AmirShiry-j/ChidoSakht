using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.Interfaces.Messagers.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string To, string Subject, string Body);
    }
}
