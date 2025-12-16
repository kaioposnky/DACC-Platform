using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using DaccApi.Responses;

namespace DaccApi.Helpers.Attributes
{
    /// <summary>
    /// Atributo base para respostas padronizadas da API
    /// </summary>
    public abstract class BaseApiResponseAttribute : Attribute, IApiResponseMetadataProvider
    {
        /// <summary>
        /// Define os tipos de conteúdo da resposta.
        /// </summary>
        public void SetContentTypes(MediaTypeCollection contentTypes)
        {
            contentTypes.Add("application/json");
        }

        /// <summary>
        /// Obtém o tipo da resposta.
        /// </summary>
        public abstract Type Type { get; }
        /// <summary>
        /// Obtém o código de status da resposta.
        /// </summary>
        public abstract int StatusCode { get; }
    }

    /// <summary>
    /// Respostas padrão para endpoints GET públicos (AllowAnonymous)
    /// Inclui: 200 (OK), 404 (Not Found), 500 (Internal Server Error)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PublicGetResponsesAttribute : Attribute
    {
        /// <summary>
        /// Inicia uma nova instância da classe <see cref="PublicGetResponsesAttribute"/>.
        /// </summary>
        public PublicGetResponsesAttribute()
        {
            // Este atributo será processado via reflection para adicionar múltiplos ProducesResponseType
        }
    }

    /// <summary>
    /// Respostas padrão para endpoints GET que requerem autenticação
    /// Inclui: 200 (OK), 401 (Unauthorized), 403 (Forbidden), 404 (Not Found), 500 (Internal Server Error)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthenticatedGetResponsesAttribute : Attribute
    {
    }

    /// <summary>
    /// Respostas padrão para endpoints POST que requerem autenticação
    /// Inclui: 201 (Created), 400 (Bad Request), 401 (Unauthorized), 403 (Forbidden), 409 (Conflict), 500 (Internal Server Error)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthenticatedPostResponsesAttribute : Attribute
    {
    }

    /// <summary>
    /// Respostas padrão para endpoints PATCH que requerem autenticação
    /// Inclui: 200 (OK), 400 (Bad Request), 401 (Unauthorized), 403 (Forbidden), 404 (Not Found), 500 (Internal Server Error)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthenticatedPatchResponsesAttribute : Attribute
    {
    }

    /// <summary>
    /// Respostas padrão para endpoints DELETE que requerem autenticação
    /// Inclui: 204 (No Content), 401 (Unauthorized), 403 (Forbidden), 404 (Not Found), 500 (Internal Server Error)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthenticatedDeleteResponsesAttribute : Attribute
    {
    }

    /// <summary>
    /// Respostas padrão para endpoints de upload de arquivos
    /// Inclui: 200 (OK), 400 (Bad Request), 401 (Unauthorized), 403 (Forbidden), 413 (Content Too Large), 500 (Internal Server Error)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FileUploadResponsesAttribute : Attribute
    {
    }

    /// <summary>
    /// Respostas padrão para endpoints de webhook (externos)
    /// Inclui: 200 (OK), 400 (Bad Request), 500 (Internal Server Error)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class WebhookResponsesAttribute : Attribute
    {
    }

    /// <summary>
    /// Respostas padrão para endpoints de listagem com paginação
    /// Inclui: 200 (OK), 400 (Bad Request), 401 (Unauthorized), 403 (Forbidden), 500 (Internal Server Error)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PaginatedListResponsesAttribute : Attribute
    {
    }
}