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
    public IActionResult AddAvaliacaoProduct(RequestAvaliacao requestAvaliacao)
    {
        try
        {
            if (requestAvaliacao.Rating is <= 0 or >= 5)
            {
                return ResponseHelper.CreateBadRequestResponse("Request inválido. A nota da avaliação deve ser um valor entre 0 e 5!");
            }
            
            var newProductRating = new Model.AvaliacaoProduto
            {
                ProductId = requestAvaliacao.ProductId,
                UserId = requestAvaliacao.UserId,
                Commentary = requestAvaliacao.Commentary,
                Rating = requestAvaliacao.Rating,
                DatePosted = DateTime.Now
            };

            _avaliacaoRepository.AddProductRatingAsync(newProductRating);
                
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
}