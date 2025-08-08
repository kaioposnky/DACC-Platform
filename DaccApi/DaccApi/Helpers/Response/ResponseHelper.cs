using DaccApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Helpers
{
    /// <summary>
    /// Helper para criação de respostas padronizadas da API
    /// </summary>
    public static class ResponseHelper
    {
        /// <summary>
        /// Cria uma resposta de erro
        /// </summary>
        /// <param name="errorResponse">Tipo de erro predefinido</param>
        /// <param name="message">Mensagem customizada para o erro (opcional)</param>
        /// <returns>IActionResult</returns>
        public static IActionResult CreateErrorResponse(ResponseError errorResponse, string? message = null)
        {
            if (message != null)
            {
                // Cria uma nova instância com a mensagem customizada
                errorResponse = errorResponse.WithDetails(null);
                // Como não podemos modificar diretamente, vamos criar um novo objeto
                var customError = new ResponseError(errorResponse.StatusCode, errorResponse.Code, message, errorResponse.Details);
                return new ObjectResult(customError) { StatusCode = customError.StatusCode };
            }
            
            return new ObjectResult(errorResponse) { StatusCode = errorResponse.StatusCode };
        }

        /// <summary>
        /// Cria uma resposta de sucesso
        /// </summary>
        /// <param name="successResponse">Tipo de sucesso predefinido</param>
        /// <param name="message">Mensagem customizada para o sucesso (opcional)</param>
        /// <returns>IActionResult</returns>
        public static IActionResult CreateSuccessResponse(ResponseSuccess successResponse, string? message = null)
        {
            if (message != null)
            {
                // Cria uma nova instância com a mensagem customizada
                var customSuccess = new ResponseSuccess(successResponse.StatusCode, successResponse.Code, message, successResponse.Data);
                return new ObjectResult(customSuccess) { StatusCode = customSuccess.StatusCode };
            }
            
            return new ObjectResult(successResponse) { StatusCode = successResponse.StatusCode };
        }
    }
}
