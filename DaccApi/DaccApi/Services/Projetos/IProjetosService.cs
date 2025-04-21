using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Projetos
{
    public interface IProjetosService
    {
        public IActionResult GetAllProjetos();
    }
}
