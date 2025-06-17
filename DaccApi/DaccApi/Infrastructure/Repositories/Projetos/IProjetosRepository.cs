using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Projetos
{
    public interface IProjetosRepository 
    {
        public Task<List<Projeto>> GetAllProjetos();
    }
}
