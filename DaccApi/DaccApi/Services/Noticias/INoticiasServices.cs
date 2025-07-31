using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Noticias;

public interface INoticiasServices
{
    public Task<IActionResult> GetAllNoticias();
    public Task<IActionResult> CreateNoticia(RequestNoticia noticia);
    public Task<IActionResult> DeleteNoticia(Guid id);
    public Task<IActionResult> GetNoticiaById(Guid id);
    
    public Task<IActionResult> UpdateNoticia(Guid id, RequestNoticia noticia);
}