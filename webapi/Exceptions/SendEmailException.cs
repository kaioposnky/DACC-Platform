namespace DaccApi.Exceptions
{
    /// <summary>
    /// Representa erros que ocorrem durante o envio de e-mails.
    /// </summary>
    public class SendEmailException : Exception
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="SendEmailException"/> com uma mensagem de erro especificada.
        /// </summary>
        public SendEmailException(string message) : base(message)
        { }
    }
}