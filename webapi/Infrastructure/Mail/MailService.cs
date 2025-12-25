using DaccApi.EmailTemplates;
using DaccApi.Exceptions;
using DaccApi.Model;
using DaccApi.Model.Objects.Order;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DaccApi.Infrastructure.Mail
{
    public class MailService : IMailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly string _websiteUrl;

        public MailService(IOptions<SmtpSettings> smtpSettings, IConfiguration configuration)
        {
            _smtpSettings = smtpSettings.Value;
            _websiteUrl = configuration["WebsiteURL"]!;
        }

        public async Task SendCustomEmailAsync(string name, string email, string subject, string body)
        {
            var message = GenerateMessage(name, email, subject);
            message.Body = new TextPart("plain") { Text = body };
            await SendEmailToSMTP(message);
        }

        public async Task SendOrderConfirmationEmailAsync(Usuario user, Order order)
        {
            var message = GenerateMessage(user.Nome, user.Email, $"Coruja Overflow - Pedido {order.Id} Confirmado");
            message.Body = OrderConfirmationEmailTemplate.GenerateBodyHtml(user, order);
            await SendEmailToSMTP(message);
        }

        public async Task SendOrderCreatedEmailAsync(Usuario user, Order order)
        {
            var message = GenerateMessage(user.Nome, user.Email, $"Coruja Overflow - Pedido {order.Id} Criado");
            message.Body = OrderCreatedEmailTemplate.GenerateBodyHtml(user, order);
            await SendEmailToSMTP(message);
        }

        public async Task SendWelcomeEmailAsync(Usuario user)
        {
            var message = GenerateMessage(user.Nome, user.Email, "Bem-vindo ao Coruja Overflow!");
            message.Body = WelcomeEmailTemplate.GenerateBodyHtml(user);
            await SendEmailToSMTP(message);
        }

        public async Task SendResetPasswordEmailAsync(Usuario user, string token)
        {
            var resetLink = $"{_websiteUrl}/reset-password?token={token}";
            var message = GenerateMessage(user.Nome, user.Email, "Recuperação de Senha - Coruja Overflow");
            message.Body = ResetPasswordEmailTemplate.GenerateBodyHtml(user, resetLink);
            await SendEmailToSMTP(message);
        }

        private async Task SendEmailToSMTP(MimeMessage message)
        {
            using var client = new SmtpClient();
            try
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                
                if (!string.IsNullOrEmpty(_smtpSettings.Username))
                {
                    await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                }

                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha técnica no envio SMTP: {ex.Message}", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        private MimeMessage GenerateMessage(string userName, string email, string subject)
        {
            var message = new MimeMessage();
            var fromName = _smtpSettings.SenderName ?? "DACC Platform";
            var fromEmail = _smtpSettings.SenderEmail ?? "contato@dacc.com";
            var toName = string.IsNullOrWhiteSpace(userName) ? email : userName;

            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(toName, email));
            message.Subject = subject;

            return message;
        }
    }
}
