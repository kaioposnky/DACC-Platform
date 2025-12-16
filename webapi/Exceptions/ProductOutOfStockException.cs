using System.Runtime.Serialization;

namespace DaccApi.Exceptions
{
    /// <summary>
    /// Representa erros que ocorrem quando um produto está fora de estoque.
    /// </summary>
    public class ProductOutOfStockException : Exception
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProductOutOfStockException"/>.
        /// </summary>
        public ProductOutOfStockException()
        {
        }

        protected ProductOutOfStockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProductOutOfStockException"/> com uma mensagem de erro especificada.
        /// </summary>
        public ProductOutOfStockException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Inicia uma nova instância da classe <see cref="ProductOutOfStockException"/> com uma mensagem de erro especificada e uma referência à exceção interna que é a causa desta exceção.
        /// </summary>
        public ProductOutOfStockException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}