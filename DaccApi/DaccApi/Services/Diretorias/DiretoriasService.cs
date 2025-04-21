using DaccApi.Model;
using DaccApi.Infrastructure.Repositories.Products;
using DaccApi.Infrastructure.Repositories.Diretorias;
using DaccApi.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Diretorias
{
    public class DiretoriasService : IDiretoriasService
    {
        private readonly IDiretoriasRepository _diretoriasRepository;
        public IActionResult GetAllDiretorias()
        {
            try
            {
                var diretorias = _diretoriasRepository.GetAllDiretoriasAsync().Result;

                if (diretorias == null || diretorias.Count == 0)
                {
                    return ResponseHelper.CreateBadRequestResponse("Nenhuma diretoria foi encontrada!");
                }

                return ResponseHelper.CreateSuccessResponse(new { Diretorias = diretorias }, "Diretorias obtidas com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao obter diretorias!" + ex);
            }
            
        }
    }
}
