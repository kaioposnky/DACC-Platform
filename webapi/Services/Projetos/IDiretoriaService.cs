using DaccApi.Model.Requests.Projetos;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Projetos
{
    public interface IDiretoriaService
    {
        Task<IActionResult> GetAllDiretorias();
        Task<IActionResult> GetDiretoriaById(Guid id);
        Task<IActionResult> SearchDiretorias(RequestQueryDiretoria query);
        Task<IActionResult> CreateDiretoria(RequestCreateDiretoria request);
        Task<IActionResult> UpdateDiretoria(Guid id, RequestUpdateDiretoria request);
        Task<IActionResult> DeleteDiretoria(Guid id);
    }
}
