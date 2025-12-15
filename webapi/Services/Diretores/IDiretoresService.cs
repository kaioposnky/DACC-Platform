using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Diretores
{
    public interface IDiretoresService
    {
        public Task<IActionResult> GetAllDiretores();
        
        public Task<IActionResult> CreateDiretor(RequestDiretor request);
        
        public Task<IActionResult> DeleteDiretor(Guid id);
        
        public  Task <IActionResult> GetDiretorById(Guid id);
        
        public Task<IActionResult> UpdateDiretor(Guid id,RequestDiretor request);

    }
}
