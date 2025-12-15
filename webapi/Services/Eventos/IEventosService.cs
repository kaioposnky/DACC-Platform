using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Eventos;

public interface IEventosService
{
    public Task<IActionResult> GetAllEventos();

    public Task<IActionResult> CreateEvento(Guid autorId, RequestEvento request);
        
    public Task<IActionResult> DeleteEvento(Guid id);
        
    public Task <IActionResult> GetEventoById(Guid id);
        
    public Task<IActionResult> UpdateEvento(Guid id,RequestEvento request);
}