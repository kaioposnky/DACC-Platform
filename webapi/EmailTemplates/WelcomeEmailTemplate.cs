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

            html.AppendLine(@"
<!DOCTYPE html>
<html lang=""pt-BR"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Bem-vindo(a) à Nossa Plataforma!</title>
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
            text-align: center;
        }
        .content h1 {
            color: #333333;
        }
        .content p {
            color: #666666;
            line-height: 1.6;
        }
        .button {
            display: inline-block;
            padding: 15px 25px;
            margin: 20px 0;
            background-color: #007bff;
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
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
            <h1>Bem-vindo(a), " + user.Nome + @"!</h1>
            <p>Estamos muito felizes em ter você conosco. Sua conta foi criada com sucesso e agora você pode aproveitar todos os recursos da nossa plataforma.</p>
            <a href=""#"" class=""button"">Acessar Minha Conta</a>
        </div>
        <div class=""footer"">
            <p>&copy; 2025 DaccApi. Todos os direitos reservados.</p>
            <p>Se você não se cadastrou em nossa plataforma, por favor, ignore este e-mail.</p>
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