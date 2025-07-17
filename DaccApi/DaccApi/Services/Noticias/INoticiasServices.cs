using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Noticias;

public interface INoticiasServices
{
    public IActionResult GetAllNoticias();
    public IActionResult CreateNoticia(RequestNoticia noticia);
    public IActionResult DeleteNoticia(int id);
    public IActionResult GetNoticiaById(int id);
    
    public IActionResult UpdateNoticia(int id, RequestNoticia noticia);
}