namespace DaccApi.Infrastructure.Mail
{
    /// <summary>
    /// Contém as configurações para conexão com o servidor SMTP.
    /// </summary>
    public class SmtpSettings
    {
        /// <summary>
        /// Obtém ou define o endereço do servidor SMTP.
        /// </summary>
        public string? Server { get; set; }
        /// <summary>
        /// Obtém ou define a porta do servidor SMTP.
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// Obtém ou define o nome de usuário para autenticação SMTP.
        /// </summary>
        public string? Username { get; set; }
        /// <summary>
        /// Obtém ou define o nome do remetente do e-mail.
        /// </summary>
        public string? SenderName { get; set; }
        /// <summary>
        /// Obtém ou define o endereço de e-mail do remetente.
        /// </summary>
        public string? SenderEmail { get; set; }
        /// <summary>
        /// Obtém ou define a senha para autenticação SMTP.
        /// </summary>
        public string? Password { get; set; }
    }
}