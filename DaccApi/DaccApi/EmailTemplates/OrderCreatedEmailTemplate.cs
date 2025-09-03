using System.Text;
using DaccApi.Model;
using DaccApi.Model.Objects.Order;
using MimeKit;

namespace DaccApi.EmailTemplates
{
    public static class OrderCreatedEmailTemplate
    {
        public static TextPart GenerateBodyHtml(Usuario user, Order order)
        {
            var html = new StringBuilder();

            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<style>");
            html.AppendLine("  body { font-family: Arial, sans-serif; color: #333; margin: 0; padding: 20px; }");
            html.AppendLine("  .header { background-color: #f8f9fa; padding: 20px; border-radius: 5px; margin-bottom: 20px; }");
            html.AppendLine("  .order-details { background-color: #f8f9fa; padding: 20px; border-radius: 5px; margin-bottom: 20px; }");
            html.AppendLine("  table { width: 100%; border-collapse: collapse; margin: 20px 0; }");
            html.AppendLine("  th, td { border: 1px solid #ddd; padding: 12px; text-align: left; }");
            html.AppendLine("  th { background-color: #e9ecef; }");
            html.AppendLine("  .total-row { font-weight: bold; }");
            html.AppendLine("  .footer { margin-top: 30px; padding: 20px; background-color: #f8f9fa; border-radius: 5px; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            html.AppendLine("<div class='header'>");
            html.AppendLine($"<h1>Olá {user.Nome}!</h1>");
            html.AppendLine($"<p>Seu pedido <strong>{order.Id}</strong> foi criado com sucesso!</p>");
            html.AppendLine("</div>");

            html.AppendLine("<div class='order-details'>");
            html.AppendLine("<h2>Detalhes do Pedido</h2>");
            html.AppendLine("<p><strong>ID do Pedido:</strong> {order.Id}</p>");
            html.AppendLine("<p><strong>Data do Pedido:</strong> {order.OrderDate:dd/MM/yyyy HH:mm}</p>");
            html.AppendLine("<p><strong>Status:</strong> {order.Status}</p>");
            html.AppendLine("<p><strong>Total:</strong> R$ {order.TotalAmount:F2}</p>");
            html.AppendLine("</div>");

            html.AppendLine("<h2>Itens do Pedido</h2>");
            html.AppendLine("<table>");
            html.AppendLine("<thead>");
            html.AppendLine("<tr>");
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
            html.AppendLine("<tfoot>");
            html.AppendLine("<tr class='total-row'>");
            html.AppendLine("<td colspan='3'><strong>Total do Pedido:</strong></td>");
            html.AppendLine($"<td><strong>R$ {totalPedido:F2}</strong></td>");
            html.AppendLine("</tr>");
            html.AppendLine("</tfoot>");
            html.AppendLine("</table>");

            html.AppendLine("<div class='footer'>");
            html.AppendLine("<p>Obrigado por comprar conosco!</p>");
            html.AppendLine("<p>Seu pedido será processado em breve.</p>");
            html.AppendLine("</div>");

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return new TextPart()
            {
                Text = html.ToString()
            };
        }

    }
}