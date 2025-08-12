using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Avaliacao;

public interface IAvaliacaoService
{
    public Task<IActionResult> CreateAvaliacao(Guid userId, RequestCreateAvaliacao avaliacao);
    public Task<IActionResult> GetAllAvaliacoes();
    
    public Task<IActionResult> GetAvaliacaoById(Guid id);
    
    public Task<IActionResult> GetAvaliacoesByProductId(Guid produtoId);
    public Task<IActionResult> GetAvaliacoesByUserId(Guid usuarioId);

    public Task<IActionResult> DeleteAvaliacao(Guid id);
    
    public Task<IActionResult> UpdateAvaliacao(Guid usuarioId,  RequestUpdateAvaliacao avaliacao);
}