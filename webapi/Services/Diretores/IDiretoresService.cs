using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Diretores
{
    public interface IDiretoresService
    {
        Task<IActionResult> GetAllDiretores();
        Task<IActionResult> GetDiretorById(Guid id);
        Task<IActionResult> CreateDiretor(RequestDiretor request);
        Task<IActionResult> UpdateDiretor(Guid id, RequestDiretor request);
        Task<IActionResult> DeleteDiretor(Guid id);
    }
}