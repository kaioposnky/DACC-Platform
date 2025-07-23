using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Diretores
{
    public interface IDiretoresService
    {
        public Task<IActionResult> GetAllDiretores();
        
        public Task<IActionResult> CreateDiretor(RequestDiretor diretor);
        
        public Task<IActionResult> DeleteDiretor(int id);
        
        public  Task <IActionResult> GetDiretorById(int id);
        
        public Task<IActionResult> UpdateDiretor(int id,RequestDiretor diretor);

    }
}
