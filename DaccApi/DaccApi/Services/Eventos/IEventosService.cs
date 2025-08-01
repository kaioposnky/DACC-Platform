using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Eventos;

public interface IEventosService
{
    public Task<IActionResult> GetAllEventos();
        
    public Task<IActionResult> CreateEvento(RequestEvento evento);
        
    public Task<IActionResult> DeleteEvento(int id);
        
    public Task <IActionResult> GetEventoById(int id);
        
    public Task<IActionResult> UpdateEvento(int id,RequestEvento evento);
}