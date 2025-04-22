using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Avaliacao;

public interface IAvaliacaoService
{
    public IActionResult CreateAvaliacaoProduct(RequestAvaliacao requestAvaliacao);
    public IActionResult GetAllAvaliacoes();
    public IActionResult GetAvaliacoesProduct(RequestAvaliacao request);
    public IActionResult GetAvaliacoesUser(RequestAvaliacao request);
}