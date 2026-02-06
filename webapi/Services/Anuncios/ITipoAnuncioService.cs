using Microsoft.AspNetCore.Mvc;
using DaccApi.Model.Requests.Anuncio;

namespace DaccApi.Services.Anuncios
{
    public interface ITipoAnuncioService
    {
        Task<IActionResult> GetAllTiposAnuncio();
        Task<IActionResult> GetTipoAnuncioById(Guid id);
        Task<IActionResult> CreateTipoAnuncio(RequestCreateTipoAnuncio request);
        Task<IActionResult> UpdateTipoAnuncio(Guid id, RequestUpdateTipoAnuncio request);
        Task<IActionResult> DeleteTipoAnuncio(Guid id);
    }
}
