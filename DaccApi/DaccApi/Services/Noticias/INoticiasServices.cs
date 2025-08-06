using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Noticias;

public interface INoticiasServices
{
    public Task<IActionResult> GetAllNoticias();
    Task<IActionResult> CreateNoticia(Guid autorId, RequestNoticia request);
    public Task<IActionResult> DeleteNoticia(Guid id);
    public Task<IActionResult> GetNoticiaById(Guid id);
    
    public Task<IActionResult> UpdateNoticia(Guid id, RequestNoticia request);
}