using DaccApi.EmailTemplates;
using DaccApi.Exceptions;
using DaccApi.Model;
using DaccApi.Model.Objects.Order;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DaccApi.Infrastructure.Mail
{
    /// <summary>
    /// Implementação do serviço de envio de e-mails usando SMTP.
    /// </summary>
    public class MailService : IMailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly string _applicationUrl;

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MailService"/>.
        /// </summary>
        public MailService(IOptions<SmtpSettings> smtpSettings, IConfiguration configuration)
        {
            _smtpSettings = smtpSettings.Value;
            _applicationUrl = configuration["ApplicationURL"]!;
        }

        /// <summary>
        /// Envia um email para o usuário informado.
        /// </summary>
        public async Task SendCustomEmailAsync(string name, string email, string subject, string body)
        {
            try
            {
                var message = GenerateMessage(name, email, subject);

                message.Body = new TextPart()
                {
                    Text = body,
                };

                await SendEmailToSMTP(message);
            }
            catch (Exception)
            {
                throw new SendEmailException("Falha ao enviar email para " + nameof(email));
            }
        }

        /// <summary>
        /// Envia um e-mail de confirmação de pedido.
        /// </summary>
        public async Task SendOrderConfirmationEmailAsync(Usuario user, Order order)
        {
            try
            {
                var message = GenerateMessage(user.Nome, user.Email, $"Coruja Overflow - Pedido {order.Id} Confirmado");

                message.Body  = OrderConfirmationEmailTemplate.GenerateBodyHtml(user, order);
                
                await SendEmailToSMTP(message);
            }
            catch (Exception ex)
            {
                throw new SendEmailException("Erro ao enviar email de confirmação de pedido! " + ex.Message);
            }
        }

        /// <summary>
        /// Envia um e-mail de notificação de pedido criado.
        /// </summary>
        public async Task SendOrderCreatedEmailAsync(Usuario user, Order order)
        {
            try
            {
                var message = GenerateMessage(user.Nome, user.Email,
                    $"Coruja Overflow - Pedido {order.Id} Criado");
                
                message.Body = OrderCreatedEmailTemplate.GenerateBodyHtml(user, order);
                
                await SendEmailToSMTP(message);
            }
            catch (Exception ex)
            {
                throw new SendEmailException("Erro ao enviar email de pedido criado! " + ex.Message);
            }
        }

        /// <summary>
        /// Envia um e-mail de boas-vindas para um novo usuário.
        /// </summary>
        public async Task SendWelcomeEmailAsync(Usuario user)
        {
            try
            {
                var message = GenerateMessage(user.Nome, user.Email,
                    "Bem-vindo ao Coruja Overflow!");

                message.Body = WelcomeEmailTemplate.GenerateBodyHtml(user);
                
                await SendEmailToSMTP(message);
            }
            catch (Exception e)
            {
                throw new SendEmailException("Erro ao enviar email de boas-vindas! " + e.Message);
            }
        }

        /// <summary>
        /// Envia um e-mail para recuperação de senha.
        /// </summary>
        public async Task SendResetPasswordEmailAsync(Usuario user, string token)
        {
            try
            {
                var resetLink = $"{_applicationUrl}/reset-password?token={token}";
                var message = GenerateMessage(user.Nome, user.Email, "Recuperação de Senha - Coruja Overflow");

                message.Body = ResetPasswordEmailTemplate.GenerateBodyHtml(user, resetLink);

                await SendEmailToSMTP(message);
            }
            catch (Exception ex)
            {
                throw new SendEmailException("Erro ao enviar email de recuperação de senha! " + ex.Message);
            }
        }

        private async Task SendEmailToSMTP(MimeMessage message)
        {
            // Usar using para poder liberar recursos automaticamente
            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, false);
            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        /// <summary>
        /// Cria uma mensagem com remetente e destinatário e o assunto.
        /// <param name="userName">Nome do usuário</param>
        /// <param name="email">Email do usuário</param>
        /// <param name="subject">Assunto do e-mail</param>
        /// <returns type="MimeMessage"> Mensagem com remetete e destinatário</returns>
        /// </summary>
        private MimeMessage GenerateMessage(string userName, string email, string subject)
        {
            var message = new MimeMessage();
            
            message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.Username));
            var recipientName = string.IsNullOrWhiteSpace(userName) ? email : userName;
            message.To.Add(new MailboxAddress(recipientName, email));
            
            // O Subject deve ser atribuído aqui para evitar repetição nos métodos
            message.Subject = subject;

            return message;
        }
    }
}
