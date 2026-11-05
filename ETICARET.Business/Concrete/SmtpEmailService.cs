using ETICARET.Business.Abstract;
using ETICARET.Business.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ETICARET.Business.Concrete
{
    public class SmtpEmailService(IOptionsSnapshot<EmailSettings> options, ILogger<SmtpEmailService> logger) : IEmailService
    {
        public async Task<bool> SendAsync(string to, string subject, string htmlBody)
        {
            var settings = options.Value;

            if (string.IsNullOrWhiteSpace(settings.Host) || string.IsNullOrWhiteSpace(settings.Password))
            {
                LogContent("E-posta ayarları eksik (Email:Password tanımlı değil), mail gönderilmedi.", to, subject, htmlBody);
                return false;
            }

            try
            {
                using var message = new MailMessage
                {
                    From = new MailAddress(settings.UserName, settings.FromName),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };
                message.To.Add(to);

                using var smtp = new SmtpClient(settings.Host, settings.Port)
                {
                    EnableSsl = settings.EnableSsl,
                    Credentials = new NetworkCredential(settings.UserName, settings.Password)
                };

                await smtp.SendMailAsync(message);
                logger.LogInformation("E-posta gönderildi. Gönderen: {From} → Alıcı: {To} | Konu: {Subject}", settings.UserName, to, subject);
                return true;
            }
            catch (Exception ex)
            {
                // Örn. Gmail "535 BadCredentials" → şifre yanlış, "534 WebLoginRequired" → hesaba tarayıcıdan giriş yapılmalı
                logger.LogError(ex, "E-posta gönderilemedi. Alıcı: {To}", to);
                LogContent("Gönderilemeyen e-postanın içeriği:", to, subject, htmlBody);
                return false;
            }
        }

        // İçindeki onay/sıfırlama linki konsoldan kopyalanıp tarayıcıda açılabilir.
        private void LogContent(string reason, string to, string subject, string htmlBody)
            => logger.LogWarning("{Reason}\nAlıcı: {To}\nKonu: {Subject}\n{Body}", reason, to, subject, htmlBody);
    }
}
