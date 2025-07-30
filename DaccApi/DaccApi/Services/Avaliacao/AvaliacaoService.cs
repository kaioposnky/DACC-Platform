using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Avaliacao;
using DaccApi.Model;
using Helpers.Response;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Avaliacao;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IAvaliacaoRepository _avaliacaoRepository;

    public AvaliacaoService(IAvaliacaoRepository avaliacaoRepository)
    {
        _avaliacaoRepository = avaliacaoRepository;
    }
    public async Task<IActionResult> CreateAvaliacao(RequestCreateAvaliacao avaliacao)
    {
        try
        {
            var newProductRating = new RequestCreateAvaliacao
            {
                ProdutoId = avaliacao.ProdutoId,
                UsuarioId = avaliacao.UsuarioId,
                Comentario = avaliacao.Comentario,
                Nota = avaliacao.Nota,
            };

            await _avaliacaoRepository.CreateAvaliacao(newProductRating);
                
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message + ex);
        }
    }

    public async Task<IActionResult> GetAllAvaliacoes()
    {
        try
        {
            var avaliacoes = await _avaliacaoRepository.GetAllAvaliacoes();

            if (avaliacoes.Count == 0) return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { avaliacoes = avaliacoes}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message + ex);
        }
    }
    
    
    public async Task<IActionResult> GetAvaliacaoById(int id)
    {
        try
        {
            var avaliacao = await _avaliacaoRepository.GetAvaliacaoById(id);

                
            if (avaliacao == null) 
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { avaliacao = avaliacao}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message + ex);
        }
    }
    
    

    public async Task<IActionResult> GetAvaliacoesByProductId(Guid produtoId)
    {
        try
        {
            var avaliacoes = await _avaliacaoRepository.GetAvaliacoesByProductId(produtoId);

            if (avaliacoes.Count == 0) 
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { avaliacoes = avaliacoes}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message + ex);
        }
    }

    public async Task<IActionResult> GetAvaliacoesByUserId(int usuarioId)
    {
        try
        {
            
            var avaliacoes = await _avaliacaoRepository.GetAvaliacoesByUserId(usuarioId);

            if (avaliacoes.Count == 0)
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { avaliacoes = avaliacoes}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public  async Task<IActionResult> DeleteAvaliacao(int id)
    {
        try
        {
            var avaliacao =  await _avaliacaoRepository.GetAvaliacaoById(id);
            
            if (avaliacao == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
            }
            _avaliacaoRepository.DeleteAvaliacao(id);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message + ex);
        }
    }

    public async Task<IActionResult> UpdateAvaliacao(int id, RequestUpdateAvaliacao avaliacao)
    {
        try
        {
            var avaliacaoQuery = await _avaliacaoRepository.GetAvaliacaoById(id);
            if (avaliacaoQuery == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            _avaliacaoRepository.UpdateAvaliacao(id, avaliacao);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { avaliacao = avaliacao}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

}