using DaccApi.Infrastructure.Repositories.Projetos;
using DaccApi.Model;

namespace DaccApi.Services.Projetos
{
    public class ProjetosService : IProjetosService
    {
        private readonly IProjetosRepository _projetosRepository;
        public List<Projeto> GetProjetos()
        {
            List<Projeto> projetos = _projetosRepository.GetProjetosAsync();

            return projetos;
        }

    }
}
