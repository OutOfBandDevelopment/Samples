using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OobDev.AspNet.Identity
{
    public class SmtpClientService : IIdentityMessageService
    {
        public bool IsBodyHtml { get; private set; }

        public SmtpClientService(bool isBodyHtml = true)
        {
            this.IsBodyHtml = isBodyHtml;
        }

        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.

            using (var client = new SmtpClient())
            using (var mailmessage = new MailMessage
            {
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = this.IsBodyHtml,
                To =
                {
                    message.Destination
                },
            })
            {
                await client.SendMailAsync(mailmessage);
            }
        }
    }
}
