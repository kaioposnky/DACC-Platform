using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Projetos
{
    public interface IProjetosService
    {
        public Task<IActionResult> GetAllProjetos();
        public Task<IActionResult> GetProjetoById(Guid id);
        
        public Task<IActionResult> CreateProjeto(RequestProjeto projeto);

        public Task<IActionResult> DeleteProjeto(Guid id);
        
        public Task<IActionResult> UpdateProjeto(Guid id, RequestProjeto projeto);

    }
}
