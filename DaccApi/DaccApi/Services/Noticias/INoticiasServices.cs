using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Noticias;

public interface INoticiasServices
{
    public Task<IActionResult> GetAllNoticias();
    public Task<IActionResult> CreateNoticia(RequestNoticia noticia);
    public Task<IActionResult> DeleteNoticia(int id);
    public Task<IActionResult> GetNoticiaById(int id);
    
    public Task<IActionResult> UpdateNoticia(int id, RequestNoticia noticia);
}