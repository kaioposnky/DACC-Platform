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

            html.AppendLine(@"
<!DOCTYPE html>
<html lang=""pt-BR"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Pedido Criado</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
            font-family: Arial, sans-serif;
        }
        .container {
            width: 100%;
            max-width: 600px;
            margin: 0 auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 10px;
        }
        .header {
            text-align: center;
            padding: 20px 0;
        }
        .header img {
            max-width: 150px;
        }
        .content {
            padding: 20px;
        }
        .content h1 {
            color: #333333;
            text-align: center;
        }
        .content p {
            color: #666666;
            line-height: 1.6;
        }
        .order-details {
            margin: 20px 0;
        }
        .order-details table {
            width: 100%;
            border-collapse: collapse;
        }
        .order-details th, .order-details td {
            padding: 10px;
            border-bottom: 1px solid #dddddd;
        }
        .order-details th {
            text-align: left;
            background-color: #f9f9f9;
        }
        .total {
            text-align: right;
            font-weight: bold;
            font-size: 1.2em;
            margin-top: 20px;
        }
        .button {
            display: inline-block;
            padding: 15px 25px;
            margin: 20px auto;
            background-color: #007bff;
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
            text-align: center;
        }
        .footer {
            text-align: center;
            padding: 20px;
            font-size: 12px;
            color: #999999;
        }
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <img src=""https://via.placeholder.com/150"" alt=""Logo da Empresa"">
        </div>
        <div class=""content"">
            <h1>Seu Pedido Foi Criado!</h1>
            <p>Olá, " + user.Nome + @",</p>
            <p>Recebemos o seu pedido <strong>#" + order.Id + @"</strong>. Ele está aguardando a confirmação do pagamento.</p>
            
            <div class=""order-details"">
                <h2>Detalhes do Pedido</h2>
                <table>
                    <thead>
                        <tr>
                            <th>Produto</th>
                            <th>Quantidade</th>
                            <th>Preço</th>
                        </tr>
                    </thead>
                    <tbody>");

            foreach (var item in order.OrderItems)
            {
                html.AppendLine($@"
                        <tr>
                            <td>{item.ProdutoId}</td>
                            <td>{item.Quantidade}</td>
                            <td>R$ {item.PrecoUnitario:F2}</td>
                        </tr>");
            }

            html.AppendLine(@"
                    </tbody>
                </table>
                <div class=""total"">
                    <p>Total: R$ " + order.TotalAmount.ToString("F2") + @"</p>
                </div>
            </div>

            <div style=""text-align: center;"">
                 <a href=""#"" class=""button"">Finalizar Pagamento</a>
            </div>

            <p>Se o pagamento já foi efetuado, por favor, aguarde a confirmação. Você receberá um novo e-mail assim que o pagamento for aprovado.</p>
        </div>
        <div class=""footer"">
            <p>&copy; 2025 DaccApi. Todos os direitos reservados.</p>
        </div>
    </div>
</body>
</html>
");

            return new TextPart("html")
            {
                Text = html.ToString()
            };
        }
    }
}