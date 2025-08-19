using DaccApi.Model;
using DaccApi.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Noticias;

public interface INoticiasServices
{
    public Task<IActionResult> GetAllNoticias();
    Task<IActionResult> CreateNoticia(Guid autorId, RequestNoticia request);
    Task <IActionResult> AddNoticiaImage(Guid noticiaId, ImageRequest request);
    public Task<IActionResult> DeleteNoticia(Guid id);
    public Task<IActionResult> GetNoticiaById(Guid id);
    
    public Task<IActionResult> UpdateNoticia(Guid id, RequestNoticia request);
}