using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Helpers
{
    /// <summary>
    /// Helper para criar respostas de retorno da API de forma mais fácil.
    /// </summary>
    public static class ResponseHelper
    {
        /// <summary>
        /// Cria uma resposta de sucesso (200)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message">Mensagem de Success Deixe vazio para usar uma mensagem genérica.</param>
        /// <returns>IActionResult</returns>
        public static IActionResult CreateSuccessResponse(object? data = null, string? message = null)
        {
            message ??= ResponseMessages.SuccessRequestMessages.GENERIC;
            data ??= new { };
            
            var response = new ResponseRequest(message, data);
            return new ObjectResult(response) { StatusCode = 200 };
        }

        /// <summary>
        /// Cria uma resposta de BadRequest (400)
        /// </summary>
        /// <param name="error">Mensagem de BadRequest, Deixe vazio para usar uma mensagem genérica.</param>
        /// <returns>IActionResult</returns>
        public static IActionResult CreateBadRequestResponse(string? error = null)
        {
            error ??= ResponseMessages.BadRequestMessages.GENERIC;
            
            var response = new BadRequest(error);
            return new ObjectResult(response) { StatusCode = 400 };
        }

        /// <summary>
        /// Cria uma resposta de BadRequest mas representa InternalServerError(500) 
        /// </summary>
        /// <param name="error">Mensagem de Error, Deixe vazio para usar uma mensagem genérica.</param>
        /// <param name="exception">Excessão lançada pelo Servidor.</param>
        /// <returns>IActionResult</returns>
        public static IActionResult CreateErrorResponse(string? error = null, Exception? exception = null)
        {
            error ??= ResponseMessages.ErrorRequestMessages.GENERIC;
            exception ??= new Exception("Exception não informada!");
            
            var response = new ErrorRequest(error, exception);
            return new ObjectResult(response) { StatusCode = 500 };
        }
    }
}
