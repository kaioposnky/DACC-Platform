using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Projetos
{
    public interface IProjetosService
    {
        public IActionResult GetAllProjetos();
        public IActionResult GetProjetoById(int id);
        
        public IActionResult CreateProjeto(RequestProjeto projeto);

        public IActionResult DeleteProjeto(int id);
        
        public IActionResult UpdateProjeto(int id, RequestProjeto projeto);

    }
}
