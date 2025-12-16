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

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="MailService"/>.
        /// </summary>
        public MailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        /// <summary>
        /// Envia um email para o usuário informado.
        /// </summary>
        /// <param name="name">Nome do usuário.</param>
        /// <param name="email">Endereço de e-mail do usuário.</param>
        /// <param name="subject">Assunto do email.</param>
        /// <param name="body">Corpo do email.</param>
        public async Task SendCustomEmailAsync(string name, string email, string subject, string body)
        {
            try
            {
                var message = GenerateMessage(name, email, subject);

                message.Subject = subject;

                message.Body = new TextPart()
                {
                    Text = body,
                };

                await SendEmailToSMTP(message);
            }
            catch (Exception ex)
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
                throw new SendEmailException("Erro ao enviar email!" + ex.Message);
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
                throw new SendEmailException("Erro ao enviar email!" + ex.Message);
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

                // Conteúdo do e-mail de boas-vindas
                message.Body = WelcomeEmailTemplate.GenerateBodyHtml(user);
                
                await SendEmailToSMTP(message);
            }
            catch (Exception e)
            {
                throw new SendEmailException("Erro ao enviar email!" + e.Message);
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
            message.To.Add(new MailboxAddress(userName, email));

            return message;
        }
    }
}