using System.Text;
using DaccApi.Model;
using MimeKit;

namespace DaccApi.EmailTemplates
{
    public static class WelcomeEmailTemplate
    {
        public static TextPart GenerateBodyHtml(Usuario user)
        {
            var html = new StringBuilder();

            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<style>");
            html.AppendLine("  body { font-family: Arial, sans-serif; color: #333; margin: 0; padding: 20px; background-color: #f8f9fa; }");
            html.AppendLine("  .container { max-width: 600px; margin: 0 auto; background-color: white; border-radius: 10px; padding: 30px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }");
            html.AppendLine("  .header { text-align: center; padding-bottom: 20px; border-bottom: 1px solid #eee; margin-bottom: 30px; }");
            html.AppendLine("  .content { line-height: 1.6; }");
            html.AppendLine("  .highlight { background-color: #e9f7ff; padding: 15px; border-radius: 5px; margin: 20px 0; }");
            html.AppendLine("  .footer { text-align: center; padding-top: 20px; border-top: 1px solid #eee; margin-top: 30px; color: #666; font-size: 14px; }");
            html.AppendLine("  .btn { display: inline-block; background-color: #007bff; color: white; padding: 12px 25px; text-decoration: none; border-radius: 5px; margin: 20px 0; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");

            html.AppendLine("<div class='container'>");
            html.AppendLine("<div class='header'>");
            html.AppendLine("<h1>Bem-vindo(a) à Nossa Plataforma!</h1>");
            html.AppendLine("<p>É um prazer ter você conosco</p>");
            html.AppendLine("</div>");

            html.AppendLine("<div class='content'>");
            html.AppendLine($"<h2>Olá {user.Nome} {user.Sobrenome}!</h2>");
            html.AppendLine("<p>Estamos muito felizes em tê-lo(a) como nosso novo usuário. Sua conta foi criada com sucesso e agora você pode acessar todos os recursos disponíveis na nossa plataforma.</p>");

            html.AppendLine("<div class='highlight'>");
            html.AppendLine("<h3>Informações da sua conta:</h3>");
            html.AppendLine($"<p><strong>Nome:</strong> {user.Nome} {user.Sobrenome}</p>");
            html.AppendLine($"<p><strong>Email:</strong> {user.Email}</p>");
            html.AppendLine($"<p><strong>RA:</strong> {user.Ra}</p>");
            html.AppendLine("</div>");

            html.AppendLine("<p>Agora você pode:</p>");
            html.AppendLine("<ul>");
            html.AppendLine("<li>Navegar por nossos produtos e serviços</li>");
            html.AppendLine("<li>Fazer pedidos e compras</li>");
            html.AppendLine("<li>Participar de nossa comunidade</li>");
            html.AppendLine("<li>Acessar notícias e eventos importantes</li>");
            html.AppendLine("</ul>");

            html.AppendLine("<p>Se tiver alguma dúvida ou precisar de ajuda, não hesite em entrar em contato com nosso suporte.</p>");

            html.AppendLine("<a href='#' class='btn'>Acessar Minha Conta</a>");

            html.AppendLine("</div>");

            html.AppendLine("<div class='footer'>");
            html.AppendLine("<p>Atenciosamente,</p>");
            html.AppendLine("<p>A Equipe da Nossa Plataforma</p>");
            html.AppendLine("<p>© 2025 Nome da Empresa. Todos os direitos reservados.</p>");
            html.AppendLine("</div>");

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