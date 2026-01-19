using ChineseRaffleApi.Services.DI;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace ChineseRaffleApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendWinnerEmailAsync(string toEmail, string userName, string giftTitle)
        {
            var senderEmail = _configuration["EmailSettings:Email"];
            var appPassword = _configuration["EmailSettings:Password"];
            var host = _configuration["EmailSettings:Host"] ?? "smtp.gmail.com";
            var port = int.Parse(_configuration["EmailSettings:Port"] ?? "587");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Chinese Raffle", senderEmail));
            message.To.Add(new MailboxAddress(userName, toEmail));
            message.Subject = "🎉 מזל טוב! זכית בהגרלה!";

            message.Body = new TextPart("html")
            {
                Text = $@"
                <div dir='rtl' style='font-family: Arial, sans-serif; border: 2px solid #10b981; padding: 20px; border-radius: 10px; text-align: center;'>
                    <h1 style='color: #10b981;'>שלום {userName}!</h1>
                    <p style='font-size: 18px;'>אנו שמחים לבשר לך שזכית במתנה המדהימה:</p>
                    <div style='background-color: #f0fdf4; padding: 15px; margin: 10px 0; border-radius: 5px; font-weight: bold; font-size: 22px; color: #166534;'>
                        {giftTitle}
                    </div>
                    <p>נציג מטעמנו יצור איתך קשר בקרוב לתיאום מסירת הפרס.</p>
                    <hr style='border: 0; border-top: 1px solid #eee; margin: 20px 0;'>
                    <p style='color: #666;'>תודה על השתתפותך במכירה הסינית שלנו ובהצלחה בהמשך!</p>
                </div>"
            };

            using var client = new SmtpClient();
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            await client.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(senderEmail, appPassword);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }
    }
}