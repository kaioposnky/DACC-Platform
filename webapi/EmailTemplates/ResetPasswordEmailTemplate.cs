using System.Text;
using DaccApi.Model;
using MimeKit;

namespace DaccApi.EmailTemplates
{
    /// <summary>
    /// Gera o template de e-mail para recuperação de senha.
    /// </summary>
    public static class ResetPasswordEmailTemplate
    {
        /// <summary>
        /// Gera o corpo do e-mail em HTML para recuperação de senha.
        /// </summary>
        public static TextPart GenerateBodyHtml(Usuario user, string resetLink)
        {
            var html = new StringBuilder();

            html.AppendLine("""

                            <!DOCTYPE html>
                            <html lang="pt-BR">
                            <head>
                                <meta charset="UTF-8">
                                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                                <title>Recuperação de Senha - Coruja Overflow</title>
                                <style>
                                    body { margin: 0; padding: 0; background-color: #f4f4f4; font-family: Arial, sans-serif; }
                                    .container { width: 100%; max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 10px; }
                                    .header { text-align: center; padding: 20px 0; }
                                    .header img { max-width: 150px; }
                                    .content { padding: 20px; text-align: center; }
                                    .content h1 { color: #333333; }
                                    .content p { color: #666666; line-height: 1.6; }
                                    .button { display: inline-block; padding: 15px 25px; margin: 20px 0; background-color: #007bff; color: #ffffff; text-decoration: none; border-radius: 5px; font-weight: bold; }
                                    .footer { text-align: center; padding: 20px; font-size: 12px; color: #999999; }
                                </style>
                            </head>
                            <body>
                                <div class="container">
                                    <div class="header">
                                        <img src="https://i.postimg.cc/vHBtpYFp/invertidoshh.png" alt="Logo Coruja Overflow">
                                    </div>
                                    <div class="content">
                                        <h1>Olá, 
                            """ + user.Nome + @"!</h1>
            <p>Recebemos uma solicitação para redefinir a senha da sua conta no Coruja Overflow.</p>
            <p>Clique no botão abaixo para escolher uma nova senha. Este link é válido por 30 minutos.</p>
            <a href=""" + resetLink + @""" class=""button"">Redefinir Minha Senha</a>
            <p>Se você não solicitou a alteração da senha, pode ignorar este e-mail com segurança.</p>
        </div>
        <div class=""footer"">
            <p>&copy; 2025 Dacc Platform. Todos os direitos reservados.</p>
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
