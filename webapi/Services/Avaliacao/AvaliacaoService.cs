using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Avaliacao;
using DaccApi.Model;
using DaccApi.Model.Responses;
using DaccApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Avaliacao;

public class AvaliacaoService : IAvaliacaoService
{
    private readonly IAvaliacaoRepository _avaliacaoRepository;

    public AvaliacaoService(IAvaliacaoRepository avaliacaoRepository)
    {
        _avaliacaoRepository = avaliacaoRepository;
    }
    public async Task<IActionResult> CreateAvaliacao(Guid userId, RequestCreateAvaliacao avaliacao)
    {
        try
        {
            var newProductRating = new AvaliacaoProduto()
            {
                Id = Guid.NewGuid(),
                ProdutoId = avaliacao.ProdutoId,
                UsuarioId = userId,
                Comentario = avaliacao.Comentario,
                Nota = avaliacao.Nota,
                DataPostada = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow,
                Ativo = true
            };

            await _avaliacaoRepository.CreateAsync(newProductRating);
                
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public async Task<IActionResult> GetAllAvaliacoes()
    {
        try
        {
            var avaliacoes = await _avaliacaoRepository.GetAllAsync();

            if (avaliacoes.Count == 0) return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            var responseAvaliacoes = avaliacoes.Select(avaliacao => new ResponseAvaliacaoProduto(avaliacao));
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { avaliacoes = responseAvaliacoes}));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro completo em GetAllAvaliacoes: " + ex.ToString());
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.ToString());
        }
    }
    
    
    public async Task<IActionResult> GetAvaliacaoById(Guid id)
    {
        try
        {
            var avaliacao = await _avaliacaoRepository.GetByIdAsync(id);

                
            if (avaliacao == null) 
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                new { avaliacao = new ResponseAvaliacaoProduto(avaliacao)}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }
    
    

    public async Task<IActionResult> GetAvaliacoesByProductId(Guid produtoId)
    {
        try
        {
            var avaliacoes = await _avaliacaoRepository.GetAvaliacoesByProductId(produtoId);

            if (avaliacoes.Count == 0) 
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            var responseAvaliacoes = avaliacoes.Select(avaliacao => new ResponseAvaliacaoProduto(avaliacao));
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                new { avaliacoes = responseAvaliacoes}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public async Task<IActionResult> GetAvaliacoesByUserId(Guid usuarioId)
    {
        try
        {
            
            var avaliacoes = await _avaliacaoRepository.GetAvaliacoesByUserId(usuarioId);

            if (avaliacoes.Count == 0)
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

            var responseAvaliacoes = avaliacoes.Select(avaliacao => new ResponseAvaliacaoProduto(avaliacao));
            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                new { avaliacoes = responseAvaliacoes}));
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public  async Task<IActionResult> DeleteAvaliacao(Guid id)
    {
        try
        {
            var avaliacao =  await _avaliacaoRepository.GetByIdAsync(id);
            
            if (avaliacao == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
            }
            await _avaliacaoRepository.DeleteAsync(id);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

    public async Task<IActionResult> UpdateAvaliacao(Guid id, RequestUpdateAvaliacao avaliacao)
    {
        try
        {
            var avaliacaoQuery = await _avaliacaoRepository.GetByIdAsync(id);
            if (avaliacaoQuery == null)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
            }
            
            avaliacaoQuery.Nota = avaliacao.Nota;
            avaliacaoQuery.Comentario = avaliacao.Comentario;
            avaliacaoQuery.DataAtualizacao = DateTime.UtcNow;
            
            await _avaliacaoRepository.UpdateAsync(id, avaliacaoQuery);

            return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
        }
        catch (Exception ex)
        {
            return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
        }
    }

}