using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Orders;
using DaccApi.Model.Objects.Order;
using DaccApi.Model.Requests.Orders;
using DaccApi.Model.Responses.Order;
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

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new ResponseCupom(cupom)), "Cupom aplicado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> GetAllCuponsAsync()
        {
            try
            {
                var cupons = await _cupomRepository.GetAllAsync();
                var response = cupons.Select(c => new ResponseCupom(c)).ToList();
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(response));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var cupom = await _cupomRepository.GetByIdAsync(id);
                if (cupom == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Cupom não encontrado.");
                }

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new ResponseCupom(cupom)));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> CreateCupomAsync(RequestCreateCupom request)
        {
            try
            {
                var existing = await _cupomRepository.GetByCodeAsync(request.Code);
                if (existing != null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe um cupom com este código.");
                }

                var cupom = new Cupom
                {
                    Id = Guid.NewGuid(),
                    Codigo = request.Code,
                    TipoDesconto = request.DiscountType,
                    Valor = request.Value,
                    DataExpiracao = request.ExpirationDate,
                    LimiteUso = request.UsageLimit,
                    Ativo = request.IsActive,
                    DataCriacao = DateTime.UtcNow
                };

                await _cupomRepository.CreateAsync(cupom);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED, "Cupom criado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> UpdateCupomAsync(Guid id, RequestUpdateCupom request)
        {
            try
            {
                var cupom = await _cupomRepository.GetByIdAsync(id);
                if (cupom == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Cupom não encontrado.");
                }

                // Verifica se mudou o código e se o novo já existe
                if (cupom.Codigo.ToLower() != request.Code.ToLower())
                {
                    var existing = await _cupomRepository.GetByCodeAsync(request.Code);
                    if (existing != null)
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST, "Já existe outro cupom com este código.");
                    }
                }

                cupom.Codigo = request.Code;
                cupom.TipoDesconto = request.DiscountType;
                cupom.Valor = request.Value;
                cupom.DataExpiracao = request.ExpirationDate;
                cupom.LimiteUso = request.UsageLimit;
                cupom.Ativo = request.IsActive;

                await _cupomRepository.UpdateAsync(id, cupom);
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK, "Cupom atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.Message);
            }
        }

        public async Task<IActionResult> DeleteCupomAsync(Guid id)
        {
            try
            {
                var cupom = await _cupomRepository.GetByIdAsync(id);
                if (cupom == null)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND, "Cupom não encontrado.");
                }

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
