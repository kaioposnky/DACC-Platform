using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Avaliacao;

public interface IAvaliacaoService
{
    public Task<IActionResult> CreateAvaliacao(RequestCreateAvaliacao avaliacao);
    public Task<IActionResult> GetAllAvaliacoes();
    
    public Task<IActionResult> GetAvaliacaoById(int id);
    
    public Task<IActionResult> GetAvaliacoesByProductId(Guid produtoId);
    public Task<IActionResult> GetAvaliacoesByUserId(int usuarioId);

    public Task<IActionResult> DeleteAvaliacao(int id);
    
    public Task<IActionResult> UpdateAvaliacao(int usuarioId,  RequestUpdateAvaliacao avaliacao);
}