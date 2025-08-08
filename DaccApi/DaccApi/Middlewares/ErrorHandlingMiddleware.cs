using System.Net;
using System.Text.Json;
using DaccApi.Helpers;
using DaccApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            
            // Checa se a resposta já foi criada, se não foi criada intervir e criar uma resposta
            if (!httpContext.Response.HasStarted && httpContext.Response.StatusCode >= 400)
            {
                await HandleErrorAsync(httpContext);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            if (httpContext.Response.HasStarted)
            {
                return;
            }
            var responseError = ResponseError.INTERNAL_SERVER_ERROR;
            var message = ex.Message;
            await WriteResponseErrorAsync(httpContext, HttpStatusCode.InternalServerError, responseError, message);
        }

        private static async Task HandleErrorAsync(HttpContext httpContext)
        {
            var statusCode = (HttpStatusCode)httpContext.Response.StatusCode;
            var responseError = statusCode switch
            {
                HttpStatusCode.Unauthorized => ResponseError.INVALID_CREDENTIALS,
                HttpStatusCode.Forbidden => ResponseError.AUTH_INSUFFICIENT_PERMISSIONS,
                HttpStatusCode.NotFound => ResponseError.RESOURCE_NOT_FOUND,
                _ => ResponseError.INTERNAL_SERVER_ERROR
            };

            await WriteResponseErrorAsync(httpContext, statusCode, responseError);
        }

        private static async Task WriteResponseErrorAsync(HttpContext httpContext, HttpStatusCode statusCode, ResponseError responseError, string? message = null)
        {
            var response = (ObjectResult)ResponseHelper.CreateErrorResponse(responseError, message);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)statusCode;
            // Escreve o body da resposta que será enviada
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response.Value));
        }
        
    }
}