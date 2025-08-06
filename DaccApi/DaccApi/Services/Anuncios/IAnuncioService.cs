using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Anuncios
{
    public interface IAnuncioService
    {
        public Task<IActionResult> GetAllAnuncio();
        public Task<IActionResult> CreateAnuncio(RequestAnuncio anuncio);
        public Task<IActionResult> DeleteAnuncio(Guid id);
        public Task<IActionResult> GetAnuncioById(Guid id);
        public Task<IActionResult> UpdateAnuncio(Guid id, RequestAnuncio request);
    }
}