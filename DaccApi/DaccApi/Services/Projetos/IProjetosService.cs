using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Projetos
{
    public interface IProjetosService
    {
        public Task<IActionResult> GetAllProjetos();
        public Task<IActionResult> GetProjetoById(int id);
        
        public Task<IActionResult> CreateProjeto(RequestProjeto projeto);

        public Task<IActionResult> DeleteProjeto(int id);
        
        public Task<IActionResult> UpdateProjeto(int id, RequestProjeto projeto);

    }
}
