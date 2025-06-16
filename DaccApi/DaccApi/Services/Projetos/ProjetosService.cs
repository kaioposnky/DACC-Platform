using DaccApi.Helpers;
using DaccApi.Infrastructure.Repositories.Projetos;
using Microsoft.AspNetCore.Mvc;

namespace DaccApi.Services.Projetos
{
    public class ProjetosService : IProjetosService
    {
        private readonly IProjetosRepository _projetosRepository;

        public ProjetosService(IProjetosRepository projetosRepository)
        {
            _projetosRepository = projetosRepository;
        }
        public IActionResult GetAllProjetos()
        {
            try
            {
                var projetos = _projetosRepository.GetAllProjetos();
                
                if (projetos.Count == 0) return ResponseHelper.CreateBadRequestResponse("Nenhum projeto foi encontrado!");


                return ResponseHelper.CreateSuccessResponse(new { Projetos = projetos }, "Projetos obtidos com sucesso!");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse("Erro ao obter Projetos!" + ex);
            }
            
        }

    }
}
