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
                AddPublicGetResponses(operation);
            }
            
            // Processa AuthenticatedGetResponsesAttribute
            if (methodInfo.GetCustomAttribute<AuthenticatedGetResponsesAttribute>() != null)
            {
                AddAuthenticatedGetResponses(operation);
            }
            
            // Processa AuthenticatedPostResponsesAttribute
            if (methodInfo.GetCustomAttribute<AuthenticatedPostResponsesAttribute>() != null)
            {
                AddAuthenticatedPostResponses(operation);
            }
            
            // Processa AuthenticatedPatchResponsesAttribute
            if (methodInfo.GetCustomAttribute<AuthenticatedPatchResponsesAttribute>() != null)
            {
                AddAuthenticatedPatchResponses(operation);
            }
            
            // Processa AuthenticatedDeleteResponsesAttribute
            if (methodInfo.GetCustomAttribute<AuthenticatedDeleteResponsesAttribute>() != null)
            {
                AddAuthenticatedDeleteResponses(operation);
            }
            
            // Processa FileUploadResponsesAttribute
            if (methodInfo.GetCustomAttribute<FileUploadResponsesAttribute>() != null)
            {
                AddFileUploadResponses(operation);
            }
            
            // Processa WebhookResponsesAttribute
            if (methodInfo.GetCustomAttribute<WebhookResponsesAttribute>() != null)
            {
                AddWebhookResponses(operation);
            }
            
            // Processa PaginatedListResponsesAttribute
            if (methodInfo.GetCustomAttribute<PaginatedListResponsesAttribute>() != null)
            {
                AddPaginatedListResponses(operation);
            }
        }

        private void AddPublicGetResponses(OpenApiOperation operation)
        {
            AddResponse(operation, "200", "Requisição bem-sucedida", typeof(ApiResponse));
            AddResponse(operation, "404", "Recurso não encontrado", typeof(ResponseError));
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError));
        }

        private void AddAuthenticatedGetResponses(OpenApiOperation operation)
        {
            AddResponse(operation, "200", "Requisição bem-sucedida", typeof(ApiResponse));
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError));
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError));
            AddResponse(operation, "404", "Recurso não encontrado", typeof(ResponseError));
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError));
        }

        private void AddAuthenticatedPostResponses(OpenApiOperation operation)
        {
            AddResponse(operation, "201", "Recurso criado com sucesso", typeof(ApiResponse));
            AddResponse(operation, "400", "Dados inválidos na requisição", typeof(ResponseError));
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError));
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError));
            AddResponse(operation, "409", "Recurso já existe", typeof(ResponseError));
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError));
        }

        private void AddAuthenticatedPatchResponses(OpenApiOperation operation)
        {
            AddResponse(operation, "200", "Recurso atualizado com sucesso", typeof(ApiResponse));
            AddResponse(operation, "400", "Dados inválidos na requisição", typeof(ResponseError));
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError));
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError));
            AddResponse(operation, "404", "Recurso não encontrado", typeof(ResponseError));
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError));
        }

        private void AddAuthenticatedDeleteResponses(OpenApiOperation operation)
        {
            AddResponse(operation, "204", "Recurso removido com sucesso", null);
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError));
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError));
            AddResponse(operation, "404", "Recurso não encontrado", typeof(ResponseError));
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError));
        }

        private void AddFileUploadResponses(OpenApiOperation operation)
        {
            AddResponse(operation, "200", "Arquivo enviado com sucesso", typeof(ApiResponse));
            AddResponse(operation, "400", "Dados inválidos na requisição", typeof(ResponseError));
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError));
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError));
            AddResponse(operation, "413", "Arquivo muito grande (máximo 5MB)", typeof(ResponseError));
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError));
        }

        private void AddWebhookResponses(OpenApiOperation operation)
        {
            AddResponse(operation, "200", "Webhook processado com sucesso", typeof(ApiResponse));
            AddResponse(operation, "400", "Dados inválidos no webhook", typeof(ResponseError));
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError));
        }

        private void AddPaginatedListResponses(OpenApiOperation operation)
        {
            AddResponse(operation, "200", "Lista recuperada com sucesso", typeof(ApiResponse));
            AddResponse(operation, "400", "Parâmetros de paginação inválidos", typeof(ResponseError));
            AddResponse(operation, "401", "Token JWT inválido ou expirado", typeof(ResponseError));
            AddResponse(operation, "403", "Permissões insuficientes", typeof(ResponseError));
            AddResponse(operation, "500", "Erro interno do servidor", typeof(ResponseError));
        }

        private static void AddResponse(OpenApiOperation operation, string statusCode, string description, Type? responseType)
        {
            if (operation.Responses.ContainsKey(statusCode)) return;
            var response = new OpenApiResponse
            {
                Description = description
            };

            if (responseType != null)
            {
                response.Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.Schema,
                                Id = responseType.Name
                            }
                        }
                    }
                };
            }

            operation.Responses[statusCode] = response;
        }
    }
}