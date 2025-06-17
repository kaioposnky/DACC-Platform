using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Avaliacao;

public interface IAvaliacaoService
{
    public IActionResult CreateAvaliacao(RequestAvaliacao requestAvaliacao);
    public IActionResult GetAllAvaliacoes();
    public IActionResult GetAvaliacoesProductById(Guid? productId);
    public IActionResult GetAvaliacoesUserById(Guid? userId);
}