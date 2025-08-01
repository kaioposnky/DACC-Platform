using DaccApi.Responses;
using DaccApi.Responses.UserResponse;
using Helpers.Response;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Helpers
{
    /// <summary>
    /// Helper para criar respostas de retorno da API de forma mais fácil.
    /// </summary>
    public static class ResponseHelper
    {
        /// <summary>
        /// Deprecated: Use CreateSuccessResponse with ResponseSuccess instead
        /// </summary>
        public static IActionResult CreateSuccessResponse(object? data = null, string? message = null)
        {
            message ??= ResponseMessages.SuccessRequestMessages.GENERIC;
            data ??= new { };
            
            var response = ResponseSuccess.WithData(ResponseSuccess.OK, data);
            return new ObjectResult(response) { StatusCode = 200 };
        }
        
        /// <summary>
        /// Deprecated: Use CreateErrorResponse with ResponseError instead
        /// </summary>
        public static IActionResult CreateBadRequestResponse(string? error = null)
        {
            error ??= ResponseMessages.BadRequestMessages.GENERIC;
            
            var response = new BadRequest(error);
            return new ObjectResult(response) { StatusCode = 400 };
        }

        /// <summary>
        /// Deprecated: Use CreateErrorResponse with ResponseError instead
        /// </summary>
        public static IActionResult CreateErrorResponse(string? error = null, Exception? exception = null)
        {
            error ??= ResponseMessages.ErrorRequestMessages.GENERIC;
            exception ??= new Exception("Exception não informada!");
            
            var response = new ErrorRequest(error, exception);
            return new ObjectResult(response) { StatusCode = 500 };
        }

        /// <summary>
        /// Cria uma resposta customizada
        /// </summary>
        /// <param name="statusCode">Código de status HTTP</param>
        /// <param name="data">Dados da resposta</param>
        /// <param name="message">Mensagem Customizada, Deixe vazio para usar uma mensagem genérica.</param>
        public static IActionResult CreateCustomResponse(int statusCode, object? data = null, string? message = null){
            var response = new ApiResponse(true, data);
            return new ObjectResult(response) { StatusCode = statusCode };
        }

        /// <summary>
        /// Cria uma resposta de erro
        /// </summary>
        /// <param name="error">Mensagem de Error, Deixe vazio para usar uma mensagem genérica.</param>
        /// <param name="message">Mensagem customizada para o erro.</param>
        /// <returns>IActionResult</returns>
        public static IActionResult CreateErrorResponse(ResponseError error, string? message = null){
            if (message != null) error.ErrorInfo.Message = message;
            var response = new ApiResponse(false, error);
            return new ObjectResult(response) { StatusCode = error.StatusCode };
        }

        /// <summary>
        /// Cria uma resposta de sucesso
        /// </summary>
        /// <param name="success">Mensagem de Success, Deixe vazio para usar uma mensagem genérica.</param>
        /// <param name="message">Mensagem customizada para o sucesso.</param>
        /// <returns>IActionResult</returns>
        public static IActionResult CreateSuccessResponse(ResponseSuccess success, string? message = null){
            if (message != null) success.SuccessInfo.Message = message;
            var response = new ApiResponse(true, success);
            return new ObjectResult(response) { StatusCode = success.StatusCode };
        }
    }
}
