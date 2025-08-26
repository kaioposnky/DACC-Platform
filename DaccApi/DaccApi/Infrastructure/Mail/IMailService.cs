using DaccApi.Model;
using DaccApi.Model.Objects.Order;

namespace DaccApi.Infrastructure.Mail
{
    public interface IMailService
    {
        Task SendCustomEmailAsync(string name, string email, string subject, string body);
        Task SendOrderConfirmationEmailAsync(Usuario user, Order order);
        Task SendOrderCreatedEmailAsync(Usuario user, Order order);
        Task SendWelcomeEmailAsync(Usuario user);

    }
}