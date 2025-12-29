using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Orders;
using DaccApi.Model.Objects.Order;
using DaccApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Orders
{
    public class CupomService : ICupomService
    {
        private readonly ICupomRepository _cupomRepository;

        public CupomService(ICupomRepository cupomRepository)
        {
            _cupomRepository = cupomRepository;
        }

        public async Task<IActionResult> ValidateCupom(string code)
        {
            try
            {
                var cupom = await _cupomRepository.GetByCodeAsync(code);

                if (cupom == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Cupom inválido ou inexistente.");
                }

                if (!cupom.Ativo)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Este cupom não está mais ativo.");
                }

                if (cupom.DataExpiracao.HasValue && cupom.DataExpiracao.Value < DateTime.UtcNow)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Este cupom expirou.");
                }

                if (cupom.LimiteUso.HasValue && cupom.UsoAtual >= cupom.LimiteUso.Value)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Este cupom atingiu o limite de usos.");
                }

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(cupom), "Cupom aplicado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> GetAllCupons()
        {
            try
            {
                var cupons = await _cupomRepository.GetAllAsync();
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(cupons));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> CreateCupom(Cupom cupom)
        {
            try
            {
                await _cupomRepository.CreateAsync(cupom);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED, "Cupom criado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> DeleteCupom(Guid id)
        {
            try
            {
                await _cupomRepository.DeleteAsync(id);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Cupom removido com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }
    }
}
