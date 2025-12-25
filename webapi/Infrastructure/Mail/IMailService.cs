using DaccApi.Model;
using DaccApi.Model.Objects.Order;

namespace DaccApi.Infrastructure.Mail
{
    /// <summary>
    /// Define a interface para o serviço de envio de e-mails.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Envia um e-mail customizado.
        /// </summary>
        Task SendCustomEmailAsync(string name, string email, string subject, string body);
        /// <summary>
        /// Envia um e-mail de confirmação de pedido.
        /// </summary>
        Task SendOrderConfirmationEmailAsync(Usuario user, Order order);
        /// <summary>
        /// Envia um e-mail de notificação de pedido criado.
        /// </summary>
        Task SendOrderCreatedEmailAsync(Usuario user, Order order);
        /// <summary>
        /// Envia um e-mail de boas-vindas para um novo usuário.
        /// </summary>
        Task SendWelcomeEmailAsync(Usuario user);

        /// <summary>
        /// Envia um e-mail de reset de senha para o usuário.
        /// </summary>
        Task SendResetPasswordEmailAsync(Usuario user, string token);

    }
}