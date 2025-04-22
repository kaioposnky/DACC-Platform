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
    public IActionResult CreateAvaliacaoProduct(RequestAvaliacao requestAvaliacao)
    {
        try
        {

            if (requestAvaliacao.ProductId == null ||
                requestAvaliacao.Commentary == null ||
                requestAvaliacao.Rating == null ||
                requestAvaliacao.UserId == null)
            {
                return ResponseHelper.CreateBadRequestResponse(
                    "Request inválido. Os campos ProductId, Commentary, Rating, UserId não podem ser nulos.");
            }
            
            if (requestAvaliacao.Rating is <= 0 or >= 5)
            {
                return ResponseHelper.CreateBadRequestResponse("Request inválido. A nota da avaliação deve ser um valor entre 0 e 5!");
            }
            
            var newProductRating = new Model.AvaliacaoProduto
            {
                ProductId = requestAvaliacao.ProductId.Value,
                UserId = requestAvaliacao.UserId.Value,
                Commentary = requestAvaliacao.Commentary,
                Rating = requestAvaliacao.Rating.Value,
                DatePosted = DateTime.Now
            };

            _avaliacaoRepository.CreateProductRatingAsync(newProductRating);
                
            // Implementar lógica para obter nome de usuário que fez a operação para guardar em banco de dados
                
            return ResponseHelper.CreateSuccessResponse("", "Avaliação adicionada com sucesso!");
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse("Erro ao adicionar avaliação ao produto!" + ex);
        }
    }

    public IActionResult GetAllAvaliacoes()
    {
        try
        {
            var avaliacoes = _avaliacaoRepository.GetAllAvaliacoesAsync().Result;

            if (avaliacoes.Count == 0) return ResponseHelper.CreateBadRequestResponse("Nenhuma avaliação foi encontrada!");

            return ResponseHelper.CreateSuccessResponse(new { Avaliacoes = avaliacoes },
                "Avaliações obtidas com sucesso");
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse("Erro ao obter todas as avaliações! " + ex);
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
                return ResponseHelper.CreateBadRequestResponse("O produto não tem nenhuma avaliação!");
        
            return ResponseHelper.CreateSuccessResponse(new { Avaliacoes = avaliacoes }, "Avaliações obtidas com sucesso!");
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse("Erro ao obter avaliacao do produto! " + ex);
        }
    }

    public IActionResult GetAvaliacoesUser(RequestAvaliacao request)
    {
        try
        {
            if (request.UserId == null)
            {
                return ResponseHelper.CreateBadRequestResponse("Request inválido. O campo UserId não pode ser nulo.");
            }
            
            var avaliacoes = _avaliacaoRepository.GetAvaliacoesByUserIdAsync(request.UserId).Result;

            if (avaliacoes.Count == 0)
                return ResponseHelper.CreateBadRequestResponse("Nenhuma avaliação foi encontrada para esse usuário!");
            return ResponseHelper.CreateSuccessResponse(new { Avaliacoes = avaliacoes },
                "Avalições do usuário obtidas com sucesso!");
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse("Erro ao obter avaliacoes do usuário!");
        }
    }
}