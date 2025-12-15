using System.Runtime.Serialization;

namespace DaccApi.Exceptions
{
    public class ProductOutOfStockException : Exception
    {
        public ProductOutOfStockException()
        {
        }

        protected ProductOutOfStockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ProductOutOfStockException(string? message) : base(message)
        {
        }

        public ProductOutOfStockException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}