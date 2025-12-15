using DaccApi.Model;
using DaccApi.Model.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Anuncios
{
    public interface IAnuncioService
    {
        public Task<IActionResult> GetAllAnuncio();
        public Task<IActionResult> CreateAnuncio(RequestAnuncio anuncio);
        public Task<IActionResult> DeleteAnuncio(Guid id);
        public Task<IActionResult> AddAnuncioImage(Guid id,ImageRequest image);
        public Task<IActionResult> GetAnuncioById(Guid id);
        public Task<IActionResult> UpdateAnuncio(Guid id, RequestAnuncio request);
    }
}