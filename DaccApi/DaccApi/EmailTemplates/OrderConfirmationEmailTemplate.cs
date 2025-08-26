using System.Text;
using DaccApi.Model;
using DaccApi.Model.Objects.Order;
using MimeKit;

namespace DaccApi.EmailTemplates
{
    public static class OrderConfirmationEmailTemplate
    {
        public static TextPart GenerateBodyHtml(Usuario user, Order order)
        {
            var html = new StringBuilder();

            html.AppendLine("<html>");
            html.AppendLine("<body style='font-family: Arial, sans-serif; color: #333;'>");
            html.AppendLine($"<h2>Olá {user.Nome}!</h2>");
            html.AppendLine($"<p>Seu pedido <strong>{order.Id}</strong> foi realizado com sucesso!</p>");

            html.AppendLine("<h3>Detalhes do Pedido:</h3>");
            html.AppendLine("<table border='1' style='border-collapse: collapse; width: 100%;'>");
            html.AppendLine("<thead>");
            html.AppendLine("<tr style='background-color: #f2f2f2;'>");
            html.AppendLine("<th>Produto</th>");
            html.AppendLine("<th>Quantidade</th>");
            html.AppendLine("<th>Preço Unitário (R$)</th>");
            html.AppendLine("<th>Total (R$)</th>");
            html.AppendLine("</tr>");
            html.AppendLine("</thead>");
            html.AppendLine("<tbody>");

            decimal totalPedido = 0;

            foreach (var item in order.OrderItems)
            {
                var itemTotal = item.Quantidade * item.PrecoUnitario;
                totalPedido += itemTotal;

                html.AppendLine("<tr>");
                html.AppendLine($"<td>{item.ProdutoId}</td>");
                html.AppendLine($"<td>{item.Quantidade}</td>");
                html.AppendLine($"<td>R$ {item.PrecoUnitario:F2}</td>");
                html.AppendLine($"<td>R$ {itemTotal:F2}</td>");
                html.AppendLine("</tr>");
            }

            html.AppendLine("</tbody>");
            html.AppendLine("</table>");

            html.AppendLine($"<p><strong>Total do Pedido: R$ {totalPedido:F2}</strong></p>");
            html.AppendLine("<p>Data de entrega estimada: " + order.OrderDate.AddDays(3).ToString("dd/MM/yyyy") + "</p>");
            html.AppendLine("<p>Obrigado por comprar conosco!</p>");

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return new TextPart()
            {
                Text = html.ToString()
            };
        }
    }
}