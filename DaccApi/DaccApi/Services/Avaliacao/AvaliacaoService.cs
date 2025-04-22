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

            if (requestAvaliacao.ProductId == null ||
                requestAvaliacao.Commentary == null ||
                requestAvaliacao.Rating == null ||
                requestAvaliacao.UserId == null)
            {
                return ResponseHelper.CreateBadRequestResponse(AvaliacaoResponseMessages.BadRequestMessages.NULL_BODY);
            }
            
            if (requestAvaliacao.Rating is <= 0 or >= 5)
            {
                return ResponseHelper.CreateBadRequestResponse(AvaliacaoResponseMessages.BadRequestMessages.INVALID_RATING);
            }
            
            var newProductRating = new AvaliacaoProduto
            {
                ProductId = requestAvaliacao.ProductId.Value,
                UserId = requestAvaliacao.UserId.Value,
                Commentary = requestAvaliacao.Commentary,
                Rating = requestAvaliacao.Rating.Value,
                DatePosted = DateTime.Now
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
            var avaliacoes = _avaliacaoRepository.GetAllAvaliacoesAsync().Result;

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

    public IActionResult GetAvaliacoesProduct(RequestAvaliacao request)
    {
        try
        {
            if (request.ProductId == null) return ResponseHelper.CreateBadRequestResponse(
                "Request inválido. O campo ProductID não pode ser nulo.");

            var avaliacoes = _avaliacaoRepository.GetAvaliacoesByProductIdAsync(request.ProductId).Result;

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

    public IActionResult GetAvaliacoesUser(RequestAvaliacao request)
    {
        try
        {
            if (request.UserId == null)
            {
                return ResponseHelper.CreateBadRequestResponse(AvaliacaoResponseMessages.BadRequestMessages.NULL_USER_ID);
            }
            
            var avaliacoes = _avaliacaoRepository.GetAvaliacoesByUserIdAsync(request.UserId).Result;

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