using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using DaccApi.Responses;
using System.Reflection;

namespace DaccApi.Helpers.Attributes
{
    /// <summary>
    /// Filtro que processa os atributos de resposta customizados e adiciona automaticamente os ProducesResponseType
    /// </summary>
    public class ApiResponseDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Este filtro será usado pelo Swagger para processar nossos atributos customizados
        }
    }

    /// <summary>
    /// Filtro de operação que adiciona as respostas baseadas nos atributos customizados
    /// </summary>
    public class ApiResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var methodInfo = context.MethodInfo;
            
            // Processa PublicGetResponsesAttribute
            if (methodInfo.GetCustomAttribute<PublicGetResponsesAttribute>() != null)
            {
                AddPublicGetResponses(operation, context);
            }
            
            // Processa AuthenticatedGetResponsesAttribute
            if (methodInfo.GetCustomAttribute<AuthenticatedGetResponsesAttribute>() != null)
            {
                AddAuthenticatedGetResponses(operation, context);
            }
            
            // Processa AuthenticatedPostResponsesAttribute
            if (methodInfo.GetCustomAttribute<AuthenticatedPostResponsesAttribute>() != null)
            {
                AddAuthenticatedPostResponses(operation, context);
            }
            
            // Processa AuthenticatedPatchResponsesAttribute
            if (methodInfo.GetCustomAttribute<AuthenticatedPatchResponsesAttribute>() != null)
            {
                AddAuthenticatedPatchResponses(operation, context);
            }
            
            // Processa AuthenticatedDeleteResponsesAttribute
            if (methodInfo.GetCustomAttribute<AuthenticatedDeleteResponsesAttribute>() != null)
            {
                AddAuthenticatedDeleteResponses(operation, context);
            }
            
            // Processa FileUploadResponsesAttribute
            if (methodInfo.GetCustomAttribute<FileUploadResponsesAttribute>() != null)
            {
                AddFileUploadResponses(operation, context);
            }
            
            // Processa WebhookResponsesAttribute
            if (methodInfo.GetCustomAttribute<WebhookResponsesAttribute>() != null)
            {
                AddWebhookResponses(operation, context);
            }
            
            // Processa PaginatedListResponsesAttribute
            if (methodInfo.GetCustomAttribute<PaginatedListResponsesAttribute>() != null)
            {
                AddPaginatedListResponses(operation, context);
            }
        }

        private void AddPublicGetResponses(OpenApiOperation operation, OperationFilterContext context)
        {
            AddResponse(operation, "200", "Requisição bem-sucedida", typeof(ApiResponse), context);
            AddResponse(operation, "404", "Recurso não encontrado", typeof(ResponseError), context);
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError), context);
        }

        private void AddAuthenticatedGetResponses(OpenApiOperation operation, OperationFilterContext context)
        {
            AddResponse(operation, "200", "Requisição bem-sucedida", typeof(ApiResponse), context);
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError), context);
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError), context);
            AddResponse(operation, "404", "Recurso não encontrado", typeof(ResponseError), context);
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError), context);
        }

        private void AddAuthenticatedPostResponses(OpenApiOperation operation, OperationFilterContext context)
        {
            AddResponse(operation, "201", "Recurso criado com sucesso", typeof(ApiResponse), context);
            AddResponse(operation, "400", "Dados inválidos na requisição", typeof(ResponseError), context);
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError), context);
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError), context);
            AddResponse(operation, "409", "Recurso já existe", typeof(ResponseError), context);
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError), context);
        }

        private void AddAuthenticatedPatchResponses(OpenApiOperation operation, OperationFilterContext context)
        {
            AddResponse(operation, "200", "Recurso atualizado com sucesso", typeof(ApiResponse), context);
            AddResponse(operation, "400", "Dados inválidos na requisição", typeof(ResponseError), context);
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError), context);
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError), context);
            AddResponse(operation, "404", "Recurso não encontrado", typeof(ResponseError), context);
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError), context);
        }

        private void AddAuthenticatedDeleteResponses(OpenApiOperation operation, OperationFilterContext context)
        {
            AddResponse(operation, "204", "Recurso removido com sucesso", null, context);
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError), context);
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError), context);
            AddResponse(operation, "404", "Recurso não encontrado", typeof(ResponseError), context);
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError), context);
        }

        private void AddFileUploadResponses(OpenApiOperation operation, OperationFilterContext context)
        {
            AddResponse(operation, "200", "Arquivo enviado com sucesso", typeof(ApiResponse), context);
            AddResponse(operation, "400", "Dados inválidos na requisição", typeof(ResponseError), context);
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError), context);
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError), context);
            AddResponse(operation, "413", "Arquivo muito grande (máximo 5MB)", typeof(ResponseError), context);
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError), context);
        }

        private void AddWebhookResponses(OpenApiOperation operation, OperationFilterContext context)
        {
            AddResponse(operation, "200", "Webhook processado com sucesso", typeof(ApiResponse), context);
            AddResponse(operation, "400", "Dados inválidos no webhook", typeof(ResponseError), context);
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError), context);
        }

        private void AddPaginatedListResponses(OpenApiOperation operation, OperationFilterContext context)
        {
            AddResponse(operation, "200", "Lista recuperada com sucesso", typeof(ApiResponse), context);
            AddResponse(operation, "400", "Parâmetros de paginação inválidos", typeof(ResponseError), context);
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError), context);
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError), context);
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError), context);
        }

        private static void AddResponse(OpenApiOperation operation, string statusCode, string description, Type? responseType, OperationFilterContext context)
        {
            if (operation.Responses.ContainsKey(statusCode)) return;
            var response = new OpenApiResponse
            {
                Description = description
            };

            if (responseType != null)
            {
                var mediaType = new OpenApiMediaType
                {
                    Schema = context.SchemaGenerator.GenerateSchema(responseType, context.SchemaRepository)
                };

                // Adiciona exemplos específicos para ResponseError baseados no status code
                if (responseType == typeof(ResponseError))
                {
                    mediaType.Example = GetResponseErrorExample(statusCode);
                }

                response.Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = mediaType
                };
            }

            operation.Responses[statusCode] = response;
        }

        private static Microsoft.OpenApi.Any.IOpenApiAny GetResponseErrorExample(string statusCode)
        {
            var (code, message) = statusCode switch
            {
                "400" => ("VALIDATION_ERROR", "Dados inválidos na requisição"),
                "401" => ("AUTH_TOKEN_INVALID", "Token JWT inválido ou expirado"),
                "403" => ("AUTH_INSUFFICIENT_PERMISSIONS", "Permissões insuficientes"),
                "404" => ("RESOURCE_NOT_FOUND", "Recurso não encontrado"),
                "409" => ("RESOURCE_ALREADY_EXISTS", "Recurso já existe"),
                "413" => ("CONTENT_TOO_LARGE", "O arquivo enviado excede o limite de tamanho"),
                "429" => ("RATE_LIMIT_EXCEEDED", "Limite de requisições excedido"),
                "500" => ("INTERNAL_SERVER_ERROR", "Erro interno do servidor"),
                _ => ("ERROR", "Ocorreu um erro")
            };

            return new Microsoft.OpenApi.Any.OpenApiObject
            {
                ["success"] = new Microsoft.OpenApi.Any.OpenApiBoolean(false),
                ["code"] = new Microsoft.OpenApi.Any.OpenApiString(code),
                ["message"] = new Microsoft.OpenApi.Any.OpenApiString(message),
                ["data"] = new Microsoft.OpenApi.Any.OpenApiNull(),
                ["details"] = new Microsoft.OpenApi.Any.OpenApiNull(),
                ["statusCode"] = new Microsoft.OpenApi.Any.OpenApiInteger(int.Parse(statusCode))
            };
        }
    }
}
