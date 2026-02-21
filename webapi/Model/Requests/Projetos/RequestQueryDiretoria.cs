namespace DaccApi.Model.Requests.Projetos
{
    /// <summary>
    /// Request para buscar diretorias com paginação.
    /// </summary>
    public class RequestQueryDiretoria : BaseQueryRequest
    {
        // Herda SearchQuery, Page, Limit de BaseQueryRequest
        // Pode adicionar filtros específicos no futuro se necessário
    }
}
