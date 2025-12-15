using DaccApi.Model;
using DaccApi.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Projetos
{
    public interface IProjetosService
    {
        public Task<IActionResult> GetAllProjetos();
        public Task<IActionResult> GetProjetoById(Guid id);
        
        public Task<IActionResult> CreateProjeto(RequestProjeto request);
        
        public Task<IActionResult> AddProjetoImage(Guid id, ImageRequest request);

        public Task<IActionResult> DeleteProjeto(Guid id);
        
        public Task<IActionResult> UpdateProjeto(Guid id, RequestProjeto request);

    }
}
