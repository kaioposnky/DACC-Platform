using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Avaliacao;
using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Avaliacao;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IAvaliacaoRepository _avaliacaoRepository;

    public AvaliacaoService(IAvaliacaoRepository avaliacaoRepository)
    {
        _avaliacaoRepository = avaliacaoRepository;
    }
    public IActionResult CreateAvaliacao(RequestAvaliacao requestAvaliacao)
    {
        try
        {
            
            if (requestAvaliacao.Nota is <= 0 or >= 5)
            {
                return ResponseHelper.CreateBadRequestResponse(AvaliacaoResponseMessages.BadRequestMessages.INVALID_RATING);
            }
            
            var newProductRating = new AvaliacaoProduto
            {
                ProdutoId = requestAvaliacao.ProdutoId,
                UsuarioId = requestAvaliacao.UsuarioId,
                Comentario = requestAvaliacao.Comentario,
                Nota = requestAvaliacao.Nota,
            };

            _avaliacaoRepository.CreateAvaliacaoAsync(newProductRating);
                
            // Implementar lógica para obter nome de usuário que fez a operação para guardar em banco de dados
                
            return ResponseHelper.CreateSuccessResponse("", AvaliacaoResponseMessages.SuccessRequestMessages.GENERIC);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(AvaliacaoResponseMessages.ErrorRequestMessages.GENERIC + ex);
        }
    }

    public IActionResult GetAllAvaliacoes()
    {
        try
        {
            var avaliacoes = _avaliacaoRepository.GetAllAvaliacoes().Result;

            if (avaliacoes.Count == 0) return ResponseHelper.CreateBadRequestResponse(
                AvaliacaoResponseMessages.BadRequestMessages.NONE_FOUND);

            return ResponseHelper.CreateSuccessResponse(new { Avaliacoes = avaliacoes }, 
                AvaliacaoResponseMessages.SuccessRequestMessages.GENERIC);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(AvaliacaoResponseMessages.ErrorRequestMessages.GENERIC + ex);
        }
    }

    public IActionResult GetAvaliacoesProductById(int productId)
    {
        try
        {
            var avaliacoes = _avaliacaoRepository.GetAvaliacoesByProductId(productId).Result;

            if (avaliacoes.Count == 0) 
                return ResponseHelper.CreateBadRequestResponse(AvaliacaoResponseMessages.BadRequestMessages.NONE_FOUND);
        
            return ResponseHelper.CreateSuccessResponse(new { Avaliacoes = avaliacoes }, 
                AvaliacaoResponseMessages.SuccessRequestMessages.GENERIC);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(AvaliacaoResponseMessages.SuccessRequestMessages.GENERIC + ex);
        }
    }

    public IActionResult GetAvaliacoesUserById(int userId)
    {
        try
        {
            
            var avaliacoes = _avaliacaoRepository.GetAvaliacoesByUserId(userId).Result;

            if (avaliacoes.Count == 0)
                return ResponseHelper.CreateBadRequestResponse(AvaliacaoResponseMessages.BadRequestMessages.NONE_FOUND);
            return ResponseHelper.CreateSuccessResponse(new { Avaliacoes = avaliacoes },
                AvaliacaoResponseMessages.SuccessRequestMessages.GENERIC);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(AvaliacaoResponseMessages.ErrorRequestMessages.GENERIC + ex);
        }
    }
}