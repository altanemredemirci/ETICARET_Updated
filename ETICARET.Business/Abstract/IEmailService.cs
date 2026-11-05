using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Abstract
{
    public interface IEmailService
    {
        Task<bool> SendAsync(string to, string subject, string htmlBody);
    }
}
