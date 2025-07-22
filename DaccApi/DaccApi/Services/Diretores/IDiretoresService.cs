using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Diretores
{
    public interface IDiretoresService
    {
        public IActionResult GetAllDiretores();
        
        public IActionResult CreateDiretor(RequestDiretor diretor);
        
        public IActionResult DeleteDiretor(int id);
        
        public  IActionResult GetDiretorById(int id);
        
        public IActionResult UpdateDiretor(int id,RequestDiretor diretor);

    }
}
